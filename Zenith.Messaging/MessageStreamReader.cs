using Serilog;
using System.IO.Pipelines;

namespace Zenith.Messaging
{
	public class MessageStreamReader<T>(PipeReader reader, IMessageStreamDeserialiser<T> deserialiser, int maxMsgSize = Constants.DefaultMaxMsgSize, ILogger? logger = null)
		: MessageStreamReaderBase(reader, maxMsgSize) where T : IMessage
	{
		public bool TryDequeue(out (Header hdr, T? body) msg)
		{
			if (DelimitedMessageQueue.TryDequeue(out var outMsg))
			{
				msg = (outMsg.hdr, deserialiser.Deserialise(outMsg.hdr, outMsg.msg));
				logger?.Debug("[MessageStreamWriter::TryDequeue] {type}", msg.hdr.Type);
				return true;
			}

			msg = default;
			return false;
		}
	}
}
