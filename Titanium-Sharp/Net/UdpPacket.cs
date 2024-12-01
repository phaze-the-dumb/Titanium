using Titanium.Net.Structs;

namespace Titanium.Net;

public class UDPPacket : Packet
{
  private Buffer _buf = new();
  
  public UDPPacketType Type { get; }
  
  public UDPPacket(UDPPacketType type, byte[] buf)
  {
    _buf.PutBytes(buf);
    Type = type;
  }

  public override Buffer GetBuffer()
  {
    return _buf;
  }
}