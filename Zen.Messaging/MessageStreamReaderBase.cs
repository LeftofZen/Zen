using Serilog;

namespace Zen.Messaging
{
	public class MessageStreamReaderBase : IDisposable
	{
		private BufferedStream BufferedStream { get; init; }

		private readonly byte[] cbuf;
		private int ptrStart;
		private int ptrEnd; // exclusive

		public int MaxMsgSize { get; init; }
		public Queue<(Header hdr, byte[] msg)> DelimitedMessageQueue { get; init; } = new();

		private int DataAvailable => ptrEnd - ptrStart;

		ILogger? Logger { get; init; }

		public MessageStreamReaderBase(Stream stream, int maxMsgSize = Constants.DefaultMaxMsgSize, ILogger? logger = null)
		{
			MaxMsgSize = maxMsgSize;
			BufferedStream = new BufferedStream(stream, MaxMsgSize);
			cbuf = new byte[MaxMsgSize];
			Logger = logger;
		}

		public void Update()
		{
			try
			{
				UpdateInternal();
			}
			catch (Exception ex)
			{
				Logger?.Error(ex, "Couldn't read from stream");
			}
		}

		private void UpdateInternal()
		{
			// read from stream
			ptrEnd += BufferedStream.Read(cbuf, ptrEnd, Math.Min(cbuf.Length - ptrEnd, 256));

			// this allocates for every message - easy future performance optmisation is using pools
			var rom = new ReadOnlyMemory<byte>(cbuf);

			// process buffer into as many messages as possible
			while (DataAvailable >= Constants.MessageHeaderSize)
			{
				// read header
				var type = BitConverter.ToUInt32(rom.Slice(ptrStart, 4).Span);
				var length = BitConverter.ToUInt32(rom.Slice(ptrStart + 4, 4).Span);

				if (DataAvailable >= Constants.MessageHeaderSize + length)
				{
					var offset = ptrStart + Constants.MessageHeaderSize;
					var msgBytes = rom.Slice(offset, (int)length);

					// external deserialisation
					var hdr = new Header() { Type = type, Length = length };
					DelimitedMessageQueue.Enqueue((hdr, msgBytes.ToArray()));

					ptrStart += Constants.MessageHeaderSize + (int)length;

					Logger?.Debug("Read {bytes} bytes starting from {start}. MessageType={type} MessageLength={length}", msgBytes.Length, offset, type, length);
				}
			}

			// copy any remaining data back to start of buffer
			if (DataAvailable > 0)
			{
				Array.ConstrainedCopy(cbuf, ptrStart, cbuf, 0, DataAvailable);
				ptrStart = 0;
				ptrEnd = DataAvailable; // aka 0 + available

				// we should zero out the rest of the buffer if we care about security more than performance
				// (or just use double buffering and zero it out in another thread/task)
			}
		}

		public void Dispose()
		{
			BufferedStream.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
