using MessagePack;
using System.Runtime.InteropServices;
using Zenith.Messaging;

namespace Zenith.Networking
{
	public enum NetworkMessageType
	{
		#region Server-Specific
		LoginResponse,
		LogoutResponse,
		WorldUpdate,
		PlayerUpdate,
		Broadcast,
		#endregion

		#region Client-Specific
		LoginRequest,
		LogoutRequest,
		InputUpdate,
		#endregion

		#region TwoWay
		ChatMessage,
		KeepAlive,
		#endregion
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public struct InputUpdate : IMessage, IClientId
	{
		public uint Type => (uint)NetworkMessageType.InputUpdate;

		//public MouseState Mouse;
		//public KeyboardState Keyboard;
		//public GamePadState Gamepad;

		public long InputTimeUnixMilliseconds;

		public Guid ClientId { get; init; }

		public bool RequiresLogin => true;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public struct WorldUpdate : IMessage
	{
		public uint Type => (uint)NetworkMessageType.WorldUpdate;
		public bool RequiresLogin => false;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public struct PlayerUpdate : IMessage, IClientId
	{
		public uint Type => (uint)NetworkMessageType.PlayerUpdate;
		public Guid ClientId { get; init; }

		//public Vector2 Position;

		public bool RequiresLogin => false;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public struct LoginRequest : IMessage
	{
		public uint Type => (uint)NetworkMessageType.LoginRequest;
		public string Username { get; init; }
		public string Password { get; init; }
		public bool RequiresLogin => false;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public struct LoginResponse : IMessage, IClientId
	{
		public uint Type => (uint)NetworkMessageType.LoginResponse;
		public Guid ClientId { get; init; }
		public bool RequiresLogin => false;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public struct LogoutRequest : IMessage
	{
		public uint Type => (uint)NetworkMessageType.LogoutRequest;
		public bool RequiresLogin => true;
		public string Username { get; init; }
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public struct LogoutResponse : IMessage, IClientId
	{
		public uint Type => (uint)NetworkMessageType.LogoutResponse;
		public Guid ClientId { get; init; }
		public bool RequiresLogin => true;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public struct ChatMessage : IMessage, IClientId
	{
		public uint Type => (uint)NetworkMessageType.ChatMessage;
		public Guid ClientId { get; init; }
		public bool RequiresLogin => true;
		public string Message { get; init; }
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public struct BroadcastMessage : IMessage
	{
		public uint Type => (uint)NetworkMessageType.Broadcast;
		public bool RequiresLogin => false;
		public string Message { get; init; }
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	[Serializable]
	[MessagePackObject(keyAsPropertyName: true)]
	public struct KeepAliveMessage : IMessage, IClientId
	{
		public uint Type => (uint)NetworkMessageType.KeepAlive;
		public Guid ClientId { get; init; }
		public bool RequiresLogin => false;
		public long Timestamp { get; init; }
	}
}
