using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zenith.Messaging
{
	public class MessageSystem
	{
		ConcurrentDictionary<uint, ConcurrentQueue<IBasicMessage>> PendingRead { get; init; }

		void Enqueue(Header hdr, IBasicMessage msg)
		{
			PendingRead[hdr.Type].Enqueue(msg);
		}

		void UpdateRead()
		{

		}
	}

	public class MessageSystemBuilder
	{
	}

	public interface IMessageProducer
	{
		void Enqueue(IBasicMessage message);
	}

	public interface IMessageConsumer
	{
		void Enqueue(IBasicMessage message);
	}
}
