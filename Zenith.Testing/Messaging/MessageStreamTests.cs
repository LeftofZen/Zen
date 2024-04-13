using MessagePack;
using NUnit.Framework;
using System.Collections.Concurrent;
using System.IO.Pipelines;
using System.Runtime.InteropServices;
using Zenith.Messaging;

namespace Zenith.Testing.Messaging
{
	public class TestMessageLookup : IMessageLookup<TestMessageProto>
	{
		static readonly Dictionary<TestMessageProto, Type> _ToType = new()
		{
			{ TestMessageProto.Login, typeof(TestLoginMessage) },
			{ TestMessageProto.Logout, typeof(TestLogoutMessage) },
			{ TestMessageProto.Chat, typeof(TestChatMessage) },
		};

		static readonly Dictionary<Type, TestMessageProto> _ToNetwork = new()
		{
			{ typeof(TestLoginMessage), TestMessageProto.Login },
			{ typeof(TestLogoutMessage), TestMessageProto.Logout },
			{ typeof(TestChatMessage), TestMessageProto.Chat },
		};

		public IDictionary<TestMessageProto, Type> ToType
			=> _ToType;

		public IDictionary<Type, TestMessageProto> ToNetwork
			=> _ToNetwork;
	}

	public enum TestMessageProto
	{
		Login = 1,
		Logout = 2,
		Chat = 3,
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public record TestLoginMessage(Guid ClientId, string Message) : IMessage, IClientId
	{
		public uint Type { get; init; } = (uint)TestMessageProto.Login;
		public bool RequiresLogin => false;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public record TestLogoutMessage(Guid ClientId, string Message) : IMessage, IClientId
	{
		public uint Type { get; init; } = (uint)TestMessageProto.Logout;
		public bool RequiresLogin => true;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public record TestChatMessage(Guid ClientId, string Message) : IMessage, IClientId
	{
		public uint Type { get; init; } = (uint)TestMessageProto.Chat;
		public bool RequiresLogin => true;
	}

	public class MessageStreamTests
	{
		[Test]
		public async Task SingleWriterSingleMessage()
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
			reader.Update(); // todo: test/call TryReadSingleMessage as well
			Assert.That(reader.DelimitedMessageQueue.Count, Is.EqualTo(1));

			// assert
			Assert.That(reader.TryDequeue(out var dmsg), Is.True);

			Assert.Multiple(() =>
			{
				Assert.That(dmsg.hdr.Type, Is.EqualTo((uint)TestMessageProto.Chat));
				Assert.That(dmsg.hdr.Length, Is.EqualTo(74));
				var body = dmsg.body as TestChatMessage;
				Assert.That(body, Is.Not.Null);
				if (body != null)
				{
					Assert.That(body.ClientId, Is.EqualTo(msg.ClientId));
					Assert.That(body.Message, Is.EqualTo(msg.Message));
				}
			});
		}

		[Test]
		public async Task SingleWriterMultipleMessage()
		{
			// arrange
			var pipe = new Pipe();
			var writer = new MessageStreamWriter<IMessage>(pipe.Writer, new MessagePackSerialiser());
			var reader = new MessageStreamReader<IMessage>(pipe.Reader, new MessagePackDeserialiser<TestMessageProto>(new TestMessageLookup()));

			// act - write a message from server to client
			var msg1 = new TestLoginMessage(Guid.NewGuid(), "Hello World");
			var msg2 = new TestChatMessage(Guid.NewGuid(), "This is cool");
			var msg3 = new TestLogoutMessage(Guid.NewGuid(), "Goodbye");

			await writer.Enqueue(msg1);
			await writer.Enqueue(msg2);
			await writer.Enqueue(msg3);
			await writer.Update();

			// act
			reader.Update();

			// assert
			Assert.That(reader.TryDequeue(out var dmsg1), Is.True);
			Assert.That(reader.TryDequeue(out var dmsg2), Is.True);
			Assert.That(reader.TryDequeue(out var dmsg3), Is.True);

			Assert.Multiple(() =>
			{
				Assert.That((dmsg1.body as TestLoginMessage).Message, Is.EqualTo(msg1.Message));
				Assert.That((dmsg2.body as TestChatMessage).Message, Is.EqualTo(msg2.Message));
				Assert.That((dmsg3.body as TestLogoutMessage).Message, Is.EqualTo(msg3.Message));

				Assert.That((dmsg1.body as TestLoginMessage).Type, Is.EqualTo(msg1.Type));
				Assert.That((dmsg2.body as TestChatMessage).Type, Is.EqualTo(msg2.Type));
				Assert.That((dmsg3.body as TestLogoutMessage).Type, Is.EqualTo(msg3.Type));
			});
		}

		[Test]
		public async Task MultipleWriterMultipleMessage()
		{
			// arrange
			var pipe = new Pipe();

			var writer1 = new MessageStreamWriter<IMessage>(pipe.Writer, new MessagePackSerialiser());
			var writer2 = new MessageStreamWriter<IMessage>(pipe.Writer, new MessagePackSerialiser());
			var writer3 = new MessageStreamWriter<IMessage>(pipe.Writer, new MessagePackSerialiser());
			var writer4 = new MessageStreamWriter<IMessage>(pipe.Writer, new MessagePackSerialiser());
			var writer5 = new MessageStreamWriter<IMessage>(pipe.Writer, new MessagePackSerialiser());

			var reader = new MessageStreamReader<IMessage>(pipe.Reader, new MessagePackDeserialiser<TestMessageProto>(new TestMessageLookup()));

			// act - write a message from server to client

			var rng = new Random();
			List<MessageStreamWriter<IMessage>> writerList = [writer1, writer2, writer3, writer4, writer5];
			List<Guid> guidList = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()];

			ConcurrentQueue<TestChatMessage> received = [];
			const int MessageCount = 100;

			for (var i = 0; i < MessageCount; ++i)
			{
				var writer = rng.Next(0, 5);
				var selectedWriter = writerList[writer];
				await selectedWriter.Enqueue(new TestChatMessage(guidList[writer], $"[{writer}] Message={i}"));
				await selectedWriter.Update();

				// thread.sleep

				reader.Update();
				if (reader.TryDequeue(out var dmsg))
				{
					received.Enqueue(dmsg.body as TestChatMessage ?? new TestChatMessage(Guid.Empty, $"<failure> {dmsg.hdr}"));
				}
			}
			Assert.That(received, Has.Count.EqualTo(MessageCount));
		}
	}
}
