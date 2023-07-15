using Serilog;

namespace Zenith.Messaging
{
	public class MessageStreamWriterBase : IDisposable
	{
		public int MaxMsgSize { get; init; }

		private BufferedStream BufferedStream { get; init; }

		public int PendingMessages { get; private set; }

		ILogger? Logger { get; init; }

		public MessageStreamWriterBase(Stream stream, int maxMsgSize = Constants.DefaultMaxMsgSize, ILogger? logger = null)
		{
			MaxMsgSize = maxMsgSize;
			BufferedStream = new BufferedStream(stream, MaxMsgSize);
			Logger = logger;
		}

		public void Enqueue(uint type, byte[] msg)
		{
			Logger?.Debug("[MessageStreamWriterBase::Enqueue] {type}", type);

			var a = BitConverter.GetBytes(type);
			var b = BitConverter.GetBytes(msg.Length);

			BufferedStream.Write(a);
			BufferedStream.Write(b);
			BufferedStream.Write(msg);

			PendingMessages++;
		}

		public void Update()
		{
			if (PendingMessages > 0)
			{
				Logger?.Debug("[MessageStreamWriterBase::Update] {pendingMessages}", PendingMessages);
				BufferedStream.Flush();
				PendingMessages = 0;
			}
		}

		public void Dispose()
		{
			BufferedStream.Dispose();
			GC.SuppressFinalize(this);
		}
	}
}
