using System.Runtime.InteropServices;
using MessagePack;

namespace Zenith.Messaging
{
	[StructLayout(LayoutKind.Sequential, Pack = 1, Size = Constants.MessageHeaderSize)] // MessageHeaderSize must == sizeof(Header)
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public record struct Header(uint Type, uint Length);
}
