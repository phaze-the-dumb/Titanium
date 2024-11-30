using Titanium.Net.Structs;

namespace Titanium.Net;

public class UDPPacket : Packet
{
  public UDPPacketType Type { get; }
  
  public UDPPacket(UDPPacketType type, byte[] buf)
  {
    _buf.PutBytes(buf);
    Type = type;
  }
}