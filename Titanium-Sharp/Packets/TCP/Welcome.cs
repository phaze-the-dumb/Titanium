using Titanium.Logic;
using Buffer = Titanium.Net.Buffer;

namespace Titanium.Packets.TCP;

public class WelcomePacket : Packet
{
  public WelcomePacket(Player player)
  {
    _buf.PutByte(0);
    _buf.PutByte(6);
    _buf.PutByte(254);
    _buf.PutByte(4);
    
    _buf.PutInt(player.ID);
  }
}