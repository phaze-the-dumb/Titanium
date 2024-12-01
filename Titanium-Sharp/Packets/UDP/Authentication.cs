using Buffer = Titanium.Net.Buffer;

namespace Titanium.Packets.UDP;

public class AuthenticationPacket : Packet
{
  private Buffer _buf = new();
  
  public int ID;
  
  public AuthenticationPacket( Packet packet )
  {
    Buffer bytes = packet.GetBuffer();
    
    bytes.GetBytes(2);
    ID = bytes.GetInt();
  }

  public override Buffer GetBuffer()
  {
    return _buf;
  }
}