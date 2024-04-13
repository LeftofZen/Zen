using Serilog;
using System.Net.Sockets;
using Zenith.Messaging;

namespace Zenith.Networking
{
	public interface IEntity
	{
		Guid Id { get; set; }
	}

	public class Server
	{
		private List<Client> clientList;
		private TcpListener clientNegotiator;
		//private TcpClient client;
		private bool negotiatorRun = true;
		private Task clientNegotiatorTask;

		public IList<Client> Clients => clientList;
		ILogger Logger;

		public Server(ILogger logger)
		{
			clientList = [];
			Logger = logger;
		}

		public Server()
		{
			clientList = [];
			Logger = Log.Logger;
		}

		public bool Start()
		{
			//if (negotiatorRun)
			//{
			//	// Ensure we're stopped first
			//	// Stop()
			//	Logger.Warning("[OdysseyServer::Start] Server cannot start on {name} {port} - client negotiator already running", Constants.DefaultHostname, Constants.DefaultPort);
			//	return false;
			//}

			Logger.Information("[OdysseyServer::Start] Server starting on {name} {port}", Constants.DefaultHostname, Constants.DefaultPort);
			clientNegotiator = new TcpListener(Constants.DefaultHostname, Constants.DefaultPort);

			// Start listening for client requests.

			clientNegotiatorTask = new Task(ClientLoop);
			clientNegotiatorTask.Start();

			return true;
		}

		public int ClientCount => clientList.Count;

		public IEnumerable<(Client, IMessage)> GetReceivedMessages()
		{
			foreach (var client in clientList)
			{
				while (client.TryDequeueMessage(out var dmsg))
				{
					Logger.Information("[OdysseyServer::GetServerMessages] {msgType}", dmsg.hdr.Type);
					yield return (client, dmsg.msg);
				}
			}
		}

		public IEnumerable<IEntity> GetConnectedEntities()
		{
			foreach (var c in clientList)
			{
				yield return c.ControllingEntity;
			}
		}

		public void Update(/*GameTime? gameTime*/)
		{
			clientList = clientList.Where(c => c.Connected).ToList();
			foreach (var c in clientList)
			{
				c.FlushMessages();
			}
		}

		//public void Update()
		//	=> Update(null);

		public void SendMessageToAllClients<T>(T message) where T : struct, IMessage
		{
			Logger.Debug("[OdysseyServer::SendMessageToAllClients]");

			foreach (var c in clientList)
			{
				if (!c.QueueMessage(message))
				{
					c.Disconnect();
				}
			}
		}

		public void SendMessageToAllClientsExcept<T>(T message, IEntity exceptedEntity) where T : struct, IMessage
		{
			Logger.Debug("[OdysseyServer::SendMessageToAllClientsExcept]");

			foreach (var c in clientList)
			{
				if (c.ControllingEntity != null && c.ControllingEntity.Id != exceptedEntity.Id)
				{
					if (!c.QueueMessage(message))
					{
						c.Disconnect();
					}
				}
			}
		}

		public void SendMessageToClient<T>(Guid id, T message) where T : struct, IMessage
		{
			Logger.Debug("[OdysseyServer::SendMessageToClient]");

			var client = clientList.SingleOrDefault(c => c.ControllingEntity.Id == id);
			if (client != null)
			{
				client.QueueMessage(message);
			}
		}

		private void ClientLoop()
		{
			Logger.Debug("[OdysseyServer::ClientLoop] {negotiatorRun}", negotiatorRun);

			clientNegotiator.Start();
			while (negotiatorRun)
			{
				Logger.Debug("[OdysseyServer::ClientLoop] Waiting for a connection... ");

				var client = clientNegotiator.AcceptTcpClientAsync().ContinueWith(x => ClientAccepted(x.Result));
				Thread.Sleep(1000);
			}

			clientNegotiator.Stop();
			Logger.Debug("[OdysseyServer::ClientLoop] Client listener stopped!");
		}

		private void ClientAccepted(TcpClient client)
		{
			Logger.Debug("[OdysseyServer::ClientLoop] Connected! {connected} {endpoint}", client.Client.Connected, client.Client.RemoteEndPoint.ToString());
			var oc = new Client(client);
			oc.InitMessaging();
			clientList.Add(oc);
		}

		public bool Stop() => negotiatorRun = false;
	}
}
