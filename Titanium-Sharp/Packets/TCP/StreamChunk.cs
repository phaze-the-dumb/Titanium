using Titanium.Net;
using Titanium.Net.Structs;
using Buffer = Titanium.Net.Buffer;

namespace Titanium.Packets.TCP;

public class StreamChunkPacket : Packet
{
  public int ID;
  public byte[] Data;

  public override Buffer GetBuffer()
  {
    Buffer buf = new();
    
    buf.PutInt(ID);
    buf.PutBytes(Data);

    return NetSerialiser.WritePacketType(PacketType.StreamChunk, buf.Length()).Add(buf);
  }
}