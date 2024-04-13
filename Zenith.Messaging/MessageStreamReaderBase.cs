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

		public void Update()
		{
			while (TryReadSingleMessage()) ;
		}

		/// <summary>
		/// Tries to read a single message from the pipe
		/// </summary>
		/// <returns>True is a message was successfully read</returns>
		public bool TryReadSingleMessage()
		{
			var hasData = Reader.TryRead(out var result);
			if (!hasData || result.Buffer.Length <= 0 || result.IsCompleted || result.IsCanceled)
			{
				return false;
			}

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

					// advance read position
					Reader.AdvanceTo(buffer.GetPosition(4 + 4 + length));
					return true;
				}
			}

			return false;
		}
	}
}
