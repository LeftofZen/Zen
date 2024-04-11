using Serilog;
using System.Buffers;
using System.IO.Pipelines;

namespace Zenith.Messaging
{
	public class MessageStreamReaderBase
	{
		PipeReader Reader { get; init; }

		public int MaxMsgSize { get; init; }
		public Queue<(Header hdr, byte[] msg)> DelimitedMessageQueue { get; init; } = new();
		public const int HeaderSize = 8;
		protected ILogger? Logger { get; init; }

		public MessageStreamReaderBase(PipeReader reader, int maxMsgSize = Constants.DefaultMaxMsgSize, ILogger? logger = null)
		{
			Reader = reader;
			MaxMsgSize = maxMsgSize;
			Logger = logger;
		}

		public async Task Update()
		{
			var result = await Reader.ReadAsync();
			var buffer = result.Buffer;

			if (buffer.Length >= HeaderSize)
			{
				var type = BitConverter.ToUInt32(buffer.Slice(0, 4).FirstSpan);
				var length = BitConverter.ToUInt32(buffer.Slice(0 + 4, 4).FirstSpan);

				if (buffer.Length >= HeaderSize + length)
				{
					// we have a full message
					var msgBytes = buffer.Slice(HeaderSize, (int)length);

					// external deserialisation
					var hdr = new Header() { Type = type, Length = length };
					DelimitedMessageQueue.Enqueue((hdr, msgBytes.ToArray()));
				}
			}
		}

	}
}
