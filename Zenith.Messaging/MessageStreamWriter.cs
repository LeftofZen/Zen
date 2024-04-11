using Serilog;
using System.Formats.Asn1;
using System.IO.Pipelines;

namespace Zenith.Messaging
{
	public class MessageStreamWriter<T>(PipeWriter writer, IMessageStreamSerialiser<T> serialiser, int maxMsgSize = Constants.DefaultMaxMsgSize, ILogger? logger = null)
		: MessageStreamWriterBase(writer, maxMsgSize) where T : IMessage
	{
		public async Task Enqueue(T msg)
		{
			Logger?.Debug("[MessageStreamWriter::Enqueue] {type}", msg.Type);
			var s = serialiser.Serialise(msg);
			await Enqueue(msg.Type, s);
		}
	}
}
