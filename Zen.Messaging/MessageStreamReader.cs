using Serilog;

namespace Zen.Messaging
{
	public class MessageStreamReader<T> : MessageStreamReaderBase where T : IMessage
	{
		private IMessageStreamDeserialiser<T> Deserialiser { get; init; }
		private ILogger? Logger { get; init; }

		public MessageStreamReader(Stream stream, IMessageStreamDeserialiser<T> deserialiser, int maxMsgSize = Constants.DefaultMaxMsgSize, ILogger? logger = null) : base(stream, maxMsgSize)
		{
			Deserialiser = deserialiser;
			Logger = logger;
		}

		public bool TryDequeue(out (Header hdr, T? body) msg)
		{
			if (DelimitedMessageQueue.TryDequeue(out var outMsg))
			{
				msg = (outMsg.hdr, Deserialiser.Deserialise(outMsg.hdr, outMsg.msg));
				Logger?.Debug("[MessageStreamWriter::TryDequeue] {type}", msg.hdr.Type);
				return true;
			}

			msg = default;
			return false;
		}
	}
}
