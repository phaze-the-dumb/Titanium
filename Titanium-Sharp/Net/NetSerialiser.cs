using Titanium.Logic;
using Titanium.Net.Structs;
using Titanium.Packets.TCP;

namespace Titanium.Net;

public class NetSerialiser
{
  public static Buffer WritePacketType(PacketType type, short length)
  {
    Buffer buf = new();
    length += 2;

    buf.PutShort(length);
    
    buf.PutByte((byte)type);
    buf.PutByte(0); // No Compression
    
    return buf;
  }
  
  public static ( PacketType, short, bool ) GetPacketType(Buffer buf)
  {
    short length = buf.GetShort();

    PacketType id = (PacketType)buf.GetByte();

    if (id == PacketType.FrameworkPacket)
    {
      // If it's a framework packet, we need the 4th byte to get the packet ID
      return (id, length, false);
    }
    
    // If it's not a framework packet, the 4th byte is the compression bool
    
    bool compression = buf.GetByte() == 1;
    return (id, length, compression);
  }

  public static void SendStream(Player player, byte[] data, PacketType type)
  {
    StreamBeginPacket begin = new();

    begin.Total = data.Length;
    begin.Type = (byte)type;
    
    player.SendTcp(begin, PacketType.StreamBegin);

    int pointerIndex = 0;
    while (pointerIndex <= data.Length)
    {
      int bytesLeft = data.Length - pointerIndex;
      int chunkSize = Math.Min(bytesLeft, Vars.MaxTcpSize);
      
      byte[] bytes = new byte[chunkSize];
      Array.Copy(data, pointerIndex, bytes, 0, chunkSize);
      
      pointerIndex += chunkSize;

      StreamChunkPacket chunk = new();
      chunk.ID = begin.ID;
      chunk.Data = bytes;
      
      player.SendTcp(chunk, PacketType.StreamChunk);
    }
  }
}