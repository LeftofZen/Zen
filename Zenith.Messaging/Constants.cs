using System.Net;

namespace Zenith.Messaging
{
	public static class Constants
	{
		public const int MessageHeaderSize = 8;
		public const int NetworkInputSize = 128;
		public const int DefaultMaxMsgSize = 1024;

		public static readonly IPAddress DefaultHostname = IPAddress.Parse("127.0.0.1");
		public const int DefaultPort = 13002;
	}
}
