using Buffer = Titanium.Net.Buffer;

namespace Titanium.Packets.UDP;

public class ServerDataPacket : Packet
{
  private Buffer _buf = new();
  
  public ServerDataPacket()
  {
    _buf.PutString("Test");
    _buf.PutString("Test Map");
    
    _buf.PutInt(100); // Total Players
    _buf.PutInt(32); // Wave
    _buf.PutInt(146); // Build Version
    
    _buf.PutString("[#a4b8fa]Titanium"); // Server Software / Version
    
    _buf.PutByte(0); // Game Mode
    _buf.PutInt(1000); // Player Limit
    
    _buf.PutString("Server Desc");
    _buf.PutString("Hell");
  }

  public override Buffer GetBuffer()
  {
    return _buf;
  }
}