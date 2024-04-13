using Serilog;
using System.IO.Pipelines;

namespace Zenith.Messaging
{
	public class MessageStreamWriterBase
	{
		public int MaxMsgSize { get; init; }

		public int PendingMessages { get; private set; }

		protected ILogger? Logger { get; init; }

		PipeWriter Writer { get; init; }

		public MessageStreamWriterBase(PipeWriter writer, int maxMsgSize = Constants.DefaultMaxMsgSize, ILogger? logger = null)
		{
			Writer = writer;
			MaxMsgSize = maxMsgSize;
			Logger = logger;
		}

		public async Task Enqueue(uint type, byte[] msg)
		{
			Logger?.Debug("[MessageStreamWriterBase::Enqueue] {type}", type);

			byte[] bytes = [.. BitConverter.GetBytes(type), .. BitConverter.GetBytes(msg.Length), .. msg];
			_ = await Writer.WriteAsync(bytes);
			PendingMessages++;
		}

		public async Task Update()
		{
			if (PendingMessages > 0)
			{
				Logger?.Debug("[MessageStreamWriterBase::Update] {pendingMessages}", PendingMessages);

				_ = await Writer.FlushAsync();
				PendingMessages = 0;
			}
		}
	}
}
