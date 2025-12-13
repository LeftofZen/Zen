namespace Zenith.Messaging
{
	public interface IBasicMessage
	{ }

	public interface IMessage
	{
		uint Type { get; }
		bool RequiresLogin { get; }
	}
}
