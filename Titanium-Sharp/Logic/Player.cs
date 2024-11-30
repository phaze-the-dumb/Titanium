using Titanium.Net;
using Titanium.Net.Structs;
using Buffer = Titanium.Net.Buffer;

namespace Titanium.Logic;

public class Player
{
  private int _id;
  
  private Address? _udpAddress;
  private TcpSocket _socket;
  
  public int ID
  {
    get { return _id; }
  }

  public Player(TcpSocket socket)
  {
    _socket = socket;
    
    Random rnd = new Random();
    _id = rnd.Next();
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
}