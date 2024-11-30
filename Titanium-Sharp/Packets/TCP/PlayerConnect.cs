using Titanium.Net.Structs;
using Buffer = Titanium.Net.Buffer;

namespace Titanium.Packets.TCP;

public class PlayerConnectPacket : Packet
{
  public int Version;
  public string VersionType;
  public List<string> Mods = new();
  public String Name;
  public String Locale;
  public String Uuid;
  public String Usid;
  public bool Mobile;
  public Colour Colour;
  
  public PlayerConnectPacket(Buffer buf)
  {
    Version = buf.GetInt();
    VersionType = buf.GetStringChecked();

    Name = buf.GetStringChecked();
    Locale = buf.GetStringChecked();
    Usid = buf.GetStringChecked();

    byte[] idBytes = buf.GetBytes(16);
    Uuid = Convert.ToBase64String(idBytes);

    Mobile = buf.GetByte() == 1;
    Colour = new Colour(buf.GetInt());

    int totalMods = buf.GetByte();
    for (int i = 0; i < totalMods; i++)
    {
      Mods.Add(buf.GetStringChecked());
    }
  }
}