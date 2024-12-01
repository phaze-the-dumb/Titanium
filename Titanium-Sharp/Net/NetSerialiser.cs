using Titanium.Net.Structs;

namespace Titanium.Net;

public class NetSerialiser
{
  public static Buffer WritePacketType(PacketType type, short length)
  {
    Buffer buf = new();
    length += 2;
    
    buf.PutBytes(BitConverter.GetBytes(length).Reverse().ToArray());
    
    buf.PutByte((byte)type);
    buf.PutByte(0); // No Compression
    
    Console.WriteLine(BitConverter.ToString(buf.Export()));
    return buf;
  }
  
  public static PacketType GetPacketType(Buffer buf)
  {
    buf.GetBytes(2);

    byte id = buf.GetByte();
    
    bool compression = buf.GetByte() == 1;
    // I'll do compression eventually

    return (PacketType)id;
  }
}