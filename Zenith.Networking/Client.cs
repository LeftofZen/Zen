using System.Net.Sockets;
using System.Net;
using Zenith.Messaging;
using Serilog;
using System.IO.Pipelines;
using System.Buffers;

namespace Zenith.Networking
{
	public class Client : IDisposable
	{
		private TcpClient tcpClient;

		public IPEndPoint Endpoint { get; init; }

		public IEntity? ControllingEntity;

		private MessageStreamWriter<IMessage>? senderWriter;
		private Pipe senderPipe = new();
		Task senderWriteDataToPipe;
		Task senderWriteFromPipeToSocket;

		private MessageStreamReader<IMessage>? receiverReader;
		private Pipe receiverPipe = new();
		Task receiverFromSocketToPipe;
		Task receiverReadDataFromPipe;

		public bool IsLoggedIn { get; set; }
		public bool LoginMessageInFlight { get; set; }
		public bool LogoutMessageInFlight { get; set; }

		ILogger Logger;

		// manual connection verification 
		private bool _connected = false;
		public bool Connected
			=> tcpClient != null && tcpClient.Connected; // && _connected;

		public string ConnectionDetails => $"[{Endpoint.Address}:{Endpoint.Port}] Connected={Connected} LoggedIn={IsLoggedIn}";

		//public int PendingMessages => writer?.PendingMessages ?? 0;
		private bool readMsgs;

		public Client(IPAddress address, int port)
		{
			Endpoint = new IPEndPoint(address, port);
			tcpClient = new TcpClient();
			Logger.Debug("[Client::OdysseyClient] New OdysseyClient via endpoint {hostname} {port}", Endpoint.Address, Endpoint.Port);
		}

		public Client(TcpClient client)
		{
			Endpoint = client.Client.RemoteEndPoint as IPEndPoint;
			tcpClient = client;
			Logger.Debug("[Client::OdysseyClient] New OdysseyClient via endpoint {hostname} {port}", Endpoint.Address, Endpoint.Port);
		}

		public bool Connect()
			=> ConnectAsync().Result;

		public async Task<bool> ConnectAsync()
		{
			Logger.Information("[Client::Connect] Client connecting on {endpoint}", Endpoint);

			try
			{
				if (!tcpClient.Connected)
				{
					await tcpClient.ConnectAsync(Endpoint);
					if (!tcpClient.Connected)
					{
						throw new InvalidOperationException("client couldn't connect");
					}

					InitMessaging();
				}
				return true;
			}
			catch (Exception ex)
			{
				Logger.Error("Ex={0}", ex);
				return false;
			}
		}

		public void InitMessaging()
		{
			Logger.Debug("[Client::InitMessaging] {connected}", tcpClient.Connected);
			if (tcpClient.Connected)
			{
				readMsgs = true;
				var msgLookup = new MessageLookup(); // should be static or otherwise

				// reading
				receiverFromSocketToPipe = FillPipeAsync(tcpClient.Client, receiverPipe.Writer);
				receiverReadDataFromPipe = ReadPipeAsync(receiverPipe.Reader);
				receiverReader = new MessageStreamReader<IMessage>(receiverPipe.Reader, new MessagePackDeserialiser<NetworkMessageType>(msgLookup));

				// writing
				senderWriteDataToPipe = FillPipeAsync(tcpClient.Client, senderPipe.Writer);
				senderWriteFromPipeToSocket = ReadPipeAsync(senderPipe.Reader);
				senderWriter = new MessageStreamWriter<IMessage>(senderPipe.Writer, new MessagePackSerialiser());
			}
			else
			{
				throw new InvalidOperationException("cannot init messaging if client isn't connected");
			}
		}

		//public bool Login(string user, string pass)
		//{
		//	if (!LoginMessageInFlight)
		//	{
		//		Logger.Information("[Client::Login] {user} {pass}", user, pass);
		//		LoginMessageInFlight = true;
		//		return QueueMessage(new LoginRequest() { Username = user, Password = pass });
		//	}
		//	return false;
		//}

		//public bool Logout(string user)
		//{
		//	if (!LogoutMessageInFlight)
		//	{
		//		Logger.Information("[Client::Logout] {user}", user);
		//		LogoutMessageInFlight = true;
		//		return QueueMessage(new LogoutRequest() { Username = user });
		//	}
		//	return false;
		//}

		public void Disconnect()
		{
			tcpClient.Close(); // cancel/join msgReaderTask as well
			tcpClient.Dispose();
		}

		void ReadMessageLoop()
		{
			Logger.Debug("[Client::ReadMessageLoop] Client message loop starting {readMsgs}", readMsgs);
			while (readMsgs)
			{
				if (!tcpClient.Connected)
				{
					Logger.Information("[Client::ReadMessageLoop] Client disconnected from server. Aborting message loop");
					break;
				}

				if (receiverReader is null)
				{
					InitMessaging();

					if (receiverReader is null)
					{
						Logger.Error("[Client::ReadMessageLoop] Message reader is null");
						break;
					}
				}

				receiverReader.Update();
			}

			Logger.Information("[Client::ReadMessageLoop] Loop terminated");
		}

		public bool TryDequeueMessage(out (Header hdr, IMessage msg) msg)
		{
			if (receiverReader != null && receiverReader.TryDequeue(out msg))
			{
				return true;
			}

			msg = default;
			return false;
		}

		// this will ALWAYS send AT LEAST ONE message.it'll either be whatever the consumer wants to send, or a keep-alive message
		public void FlushMessages()
		{
			try
			{
				if (senderWriter is null)
				{
					return;
				}

				if (senderWriter.PendingMessages == 0)
				{
					senderWriter.Enqueue(new KeepAliveMessage() { ClientId = ControllingEntity?.Id ?? Guid.Empty, Timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds() });
				}

				senderWriter.Update();

				// if we were able to send a message, we are now connected as far as tcp is concerned
				_connected = true;
			}
			catch (Exception)
			{
				tcpClient.Close();
				_connected = false;
			}
		}

		public bool QueueMessage<T>(T message) where T : struct, IMessage
		{
			Logger.Debug("[Client::QueueMessage] {type} {ctype}", message.Type, message.GetType());

			if (senderWriter is null)
			{
				InitMessaging();

				if (senderWriter is null)
				{
					Logger.Error("[Client::QueueMessage] Message writer is null");
					return false;
				}
			}

			senderWriter.Enqueue(message);
			return true;
		}

		public void Dispose()
		{
			tcpClient.Dispose();
		}

		async Task FillPipeAsync(Socket socket, PipeWriter writer)
		{
			const int minimumBufferSize = 512;

			while (true)
			{
				// Allocate at least 512 bytes from the PipeWriter
				Memory<byte> memory = writer.GetMemory(minimumBufferSize);
				try
				{
					int bytesRead = await socket.ReceiveAsync(memory, SocketFlags.None);
					if (bytesRead == 0)
					{
						break;
					}
					// Tell the PipeWriter how much was read from the Socket
					writer.Advance(bytesRead);
				}
				catch (Exception ex)
				{
					Logger.Error(ex.ToString());
					break;
				}

				// Make the data available to the PipeReader
				FlushResult result = await writer.FlushAsync();

				if (result.IsCompleted)
				{
					break;
				}
			}

			// Tell the PipeReader that there's no more data coming
			writer.Complete();
		}

		async Task ReadPipeAsync(PipeReader reader)
		{
			while (true)
			{
				ReadResult result = await reader.ReadAsync();

				ReadOnlySequence<byte> buffer = result.Buffer;
				SequencePosition? position = null;

				do
				{
					// Look for a EOL in the buffer
					position = buffer.PositionOf((byte)'\n');

					if (position != null)
					{
						// Process the line
						//ProcessLine(buffer.Slice(0, position.Value));

						// Skip the line + the \n character (basically position)
						buffer = buffer.Slice(buffer.GetPosition(1, position.Value));
					}
				}
				while (position != null);

				// Tell the PipeReader how much of the buffer we have consumed
				reader.AdvanceTo(buffer.Start, buffer.End);

				// Stop reading if there's no more data coming
				if (result.IsCompleted)
				{
					break;
				}
			}

			// Mark the PipeReader as complete
			reader.Complete();
		}
	}
}
