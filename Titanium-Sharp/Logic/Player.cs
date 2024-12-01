using Titanium.Net;
using Titanium.Net.Structs;
using Titanium.Packets.TCP;
using Buffer = Titanium.Net.Buffer;

namespace Titanium.Logic;

public class Player
{
  private int _id;
  private string _name;

  private int _version;
  private string _versionType;
  private List<string> _mods = new();
  private string _locale;
  private string _uuid;
  private string _usid;
  private bool _mobile;
  private Colour _colour;
  
  private Address? _udpAddress;
  private TcpSocket _socket;
  
  public int ID
  {
    get { return _id; }
  }  
  
  public string Name
  {
    get { return _name; }
  }

  public Player(TcpSocket socket)
  {
    _socket = socket;
    
    Random rnd = new Random();
    _id = rnd.Next();
  }

  public void Join(PlayerConnectPacket packet)
  {
    _version = packet.Version;
    _versionType = packet.VersionType;
    _mods = packet.Mods;
    _name = packet.Name;
    _locale = packet.Locale;
    _uuid = packet.Uuid;
    _usid = packet.Usid;
    _mobile = packet.Mobile;
    _colour = packet.Colour;
    
    Console.WriteLine(_name + " (" + _uuid + ") Joined.");
  }

  public void SetUdpAddress(Address addr)
  {
    _udpAddress = addr;
    Buffer buf = new();
    
    buf.PutByte(0);
    buf.PutByte(6);
    buf.PutByte(254);
    buf.PutByte(3);
    
    buf.PutByte(0);
    buf.PutByte(0);
    buf.PutByte(0);
    buf.PutByte(0);
    
    _socket.Send(buf);
  }

  public void Leave()
  {
    Console.WriteLine(_name + " (" + _uuid + ") Left.");
  }
}