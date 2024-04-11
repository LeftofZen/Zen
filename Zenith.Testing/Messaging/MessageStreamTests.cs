using MessagePack;
using NUnit.Framework;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using Zenith.Messaging;

namespace Zenith.Testing.Messaging
{
	public class TestMessageLookup : IMessageLookup<TestMessageProto>
	{
		static readonly Dictionary<uint, Type> _ToType = new()
		{
			{ (uint)TestMessageProto.Chat, typeof(TestChatMessage) },
		};

		static readonly Dictionary<Type, TestMessageProto> _ToNetwork = new()
		{
			{ typeof(TestChatMessage), TestMessageProto.Chat },
		};

		public IDictionary<uint, Type> ToType
			=> _ToType;

		public IDictionary<Type, TestMessageProto> ToNetwork
			=> _ToNetwork;
	}

	public enum TestMessageProto
	{
		Foo = 1,
		Blah = 2,
		Chat = 3,
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public record TestChatMessage(Guid ClientId, string Message) : IMessage, IClientId
	{
		public uint Type => (uint)TestMessageProto.Chat; // this is just a 'random' number
		public bool RequiresLogin => true;
	}

	public class MessageStreamTests
	{
		[Test]
		public async Task Simplex_Single()
		{
			// arrange
			var pipe = new Pipe();
			var writer = new MessageStreamWriter<IMessage>(pipe.Writer, new MessagePackSerialiser());
			var reader = new MessageStreamReader<IMessage>(pipe.Reader, new MessagePackDeserialiser<TestMessageProto>(new TestMessageLookup()));

			// act - write a message from server to client
			var msg = new TestChatMessage(Guid.NewGuid(), "Hello World");

			Assert.That(writer.PendingMessages, Is.EqualTo(0));
			await writer.Enqueue(msg);
			Assert.That(writer.PendingMessages, Is.EqualTo(1));

			await writer.Update();
			Assert.That(writer.PendingMessages, Is.EqualTo(0));

			Assert.That(reader.DelimitedMessageQueue.Count, Is.EqualTo(0));
			await reader.Update();
			Assert.That(reader.DelimitedMessageQueue.Count, Is.EqualTo(1));

			// assert
			Assert.True(reader.TryDequeue(out var dmsg));

			Assert.Multiple(() =>
			{
				Assert.That(dmsg.hdr.Type, Is.EqualTo((uint)TestMessageProto.Chat));
				Assert.That(dmsg.hdr.Length, Is.EqualTo(68));
				Assert.That(((TestChatMessage)dmsg.body).ClientId, Is.EqualTo(msg.ClientId));
				Assert.That(((TestChatMessage)dmsg.body).Message, Is.EqualTo(msg.Message));
			});
		}
	}
}
