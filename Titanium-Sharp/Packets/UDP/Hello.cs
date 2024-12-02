using Buffer = Titanium.Net.Buffer;

namespace Titanium.Packets.TCP;

public class HelloPacket : Packet
{
  private Buffer _buf = new();
  
  public HelloPacket(int id)
  {
    _buf.PutByte(254);
    _buf.PutByte(3);
    
    _buf.PutInt(id);
  }

  public override Buffer GetBuffer()
  {
    return _buf;
  }
}