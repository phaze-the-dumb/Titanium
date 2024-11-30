namespace Titanium.Packets.UDP;

public class AuthenticationPacket : Packet
{
  public int ID;
  
  public AuthenticationPacket( Packet packet )
  {
    packet.Bytes.GetBytes(2);
    ID = packet.Bytes.GetInt();
  }
}