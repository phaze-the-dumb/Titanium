using Titanium.Net.Structs;
using Buffer = Titanium.Net.Buffer;

namespace Titanium.Packets.TCP;

public class PlayerConnectPacket : Packet
{
  private Buffer _buf = new();

  public int Connector;
  public int Version;
  public string VersionType;
  public List<string> Mods = new();
  public String Name;
  public String Locale;
  public String Uuid;
  public String Usid;
  public bool Mobile;
  public Colour Colour;
  
  public void Read(Buffer buf)
  {
    Connector = buf.GetInt();
    
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

  public void Write()
  {
    _buf.PutInt(Connector);
    
    _buf.PutInt(Version);
    _buf.PutStringChecked(VersionType);
    
    _buf.PutStringChecked(Name);
    _buf.PutStringChecked(Locale);
    _buf.PutStringChecked(Usid);
    
    _buf.PutBytes(Convert.FromBase64String(Uuid));
    
    _buf.PutByte(Mobile ? (byte)1 : (byte)0);
    _buf.PutInt(Colour.ToInt());
    
    _buf.PutByte(0); // No mods installed
  }

  public override Buffer GetBuffer()
  {
    return _buf;
  }
}