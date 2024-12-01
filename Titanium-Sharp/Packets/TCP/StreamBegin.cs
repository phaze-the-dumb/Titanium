using Titanium.Net;
using Titanium.Net.Structs;
using Buffer = Titanium.Net.Buffer;

namespace Titanium.Packets.TCP;

public class StreamBeginPacket : Packet
{
  private static int _lastId = 0;

  public int ID = _lastId++;
  public int Total;
  public byte Type;

  public override Buffer GetBuffer()
  {
    Buffer buf = new();
    
    buf.PutInt(ID);
    buf.PutInt(Total);
    buf.PutByte(Type);

    return NetSerialiser.WritePacketType(PacketType.StreamBegin, buf.Length()).Add(buf);
  }
}