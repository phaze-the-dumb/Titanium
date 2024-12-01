using Buffer = Titanium.Net.Buffer;

namespace Titanium.Packets.TCP;

public class StreamBegin
{
  private static int _lastId = 0;

  public int ID = _lastId++;
  public int Total;
  public byte Type;

  public Buffer GetBuffer()
  {
    Buffer buf = new();
    
    buf.PutInt(ID);
    buf.PutInt(Total);
    buf.PutByte(Type);

    return buf;
  }
}