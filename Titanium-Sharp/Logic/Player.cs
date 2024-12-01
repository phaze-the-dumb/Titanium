using System.Net;
using System.Net.Sockets;
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

  private int _connector;

  private Proxy _proxy;
  
  public int Version
  {
    get { return _version; }
  }
  
  public string VersionType
  {
    get { return _versionType; }
  }
  
  public List<string> Mods
  {
    get { return _mods; }
  }
  
  public string Locale
  {
    get { return _locale; }
  }
  
  public int ID
  {
    get { return _id; }
  }
  
  public string Name
  {
    get { return _name; }
  }
  
  public string Uuid
  {
    get { return _uuid; }
  }
  
  public string Usid
  {
    get { return _usid; }
  }
  
  public bool Mobile
  {
    get { return _mobile; }
  }
  
  public Colour Colour
  {
    get { return _colour; }
  }  
  
  public int Connector
  {
    get { return _connector; }
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
    _connector = packet.Connector;
    
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
    _proxy.Close();
  }

  public void Kick(KickReason reason)
  {
    Buffer buf = new();
    buf.PutBytes([1, 0, (byte)reason]);

    _socket.Send(NetSerialiser.WritePacketType(PacketType.KickPacket, 3).Add(buf));
    _socket.Close();
  }

  public void JoinTo(Address remoteServer)
  {
    _proxy = new(this, remoteServer);
  }

  public void SendTcp(Packet packet, PacketType type)
  {
    Buffer buf = packet.GetBuffer();
    _socket.Send(NetSerialiser.WritePacketType(PacketType.KickPacket, buf.Length()).Add(buf));
  }

  public TcpSocket GetTcpSocket()
  {
    return _socket;
  }

  public Address GetUdpAddress()
  {
    return _udpAddress;
  }
}