namespace Titanium.Net.Structs;

public enum PacketType
{
  StreamBegin,
  StreamChunk,
  WorldStream,
  ConnectPacket,
  KickPacket = 45,
  FrameworkPacket = 254
}