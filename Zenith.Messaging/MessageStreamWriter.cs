using Serilog;

namespace Zenith.Messaging
{
	public class MessageStreamWriter<T> : MessageStreamWriterBase where T : IMessage
	{
		private IMessageStreamSerialiser<T> Serialiser { get; init; }
		private ILogger? Logger { get; init; }

		public MessageStreamWriter(Stream stream, IMessageStreamSerialiser<T> serialiser, int maxMsgSize = Constants.DefaultMaxMsgSize, ILogger? logger = null) : base(stream, maxMsgSize)
		{
			Serialiser = serialiser;
			Logger = logger;
		}

		public void Enqueue(T msg)
		{
			Logger?.Debug("[MessageStreamWriter::Enqueue] {type}", msg.Type);

			var s = Serialiser.Serialise(msg);
			Enqueue(msg.Type, s);
		}
	}
}
