using System.Net;
using Titanium.Logic;
using Titanium.Net;
using Titanium.Net.Structs;
using Titanium.Packets.TCP;
using Titanium.Packets.UDP;
using Buffer = Titanium.Net.Buffer;

namespace Titanium;

public class Main
{
  private int _port;

  private List<Player> _players = new();

  public Action<Player> OnPlayerJoin;
  public Action<Player> OnPlayerLeave;
  
  public Main(int port)
  {
    _port = port;
    Console.WriteLine("Titanium Started! Hello World.");
    
    Thread udpThread = new Thread(UdpLoop){ Name = "UdpThread" };
    Thread tcpThread = new Thread(TcpLoop){ Name = "TcpThread" };
    
    udpThread.Start();
    tcpThread.Start();
  }
  
  private void UdpLoop()
  {
    Console.WriteLine("UDP Server Started.");
    UdpServer udpServer = new(_port);

    Dictionary<string, Player> playerMap = new Dictionary<string, Player>();

    udpServer.OnRecieveProxyablePacket += (buf, addr) =>
    {
      Player plyr = playerMap[addr];
      if (plyr == null) return;

      plyr.OnRecieveUdpPacket(buf);
    };
    
    udpServer.OnRecievePacket += ( p, addr ) =>
    {
      switch (p.Type)
      {
        case UDPPacketType.ServerData:
          udpServer.SendTo(new ServerDataPacket(), addr);
          break;
        case UDPPacketType.Authentication:
          AuthenticationPacket packet = new AuthenticationPacket(p);
          var player = _players.Find(x => x.ID == packet.ID);
          
          if (player == null) return;
          
          playerMap.Add(addr.IpAddress + ":" + addr.Port, player);
          player.SetUdpAddress(addr);
          
          break;
      }
    };

    udpServer.Run();
  }

  private void TcpLoop()
  {
    Console.WriteLine("TCP Server Started.");
    TcpServer tcpServer = new(_port);

    tcpServer.OnConnection += socket =>
    {
      Player p = new(socket);
      
      socket.Send(new WelcomePacket(p).GetBuffer());
      _players.Add(p);
      
      socket.OnMessage += buf =>
      {
        Buffer buffer = Buffer.From(buf);
        PacketType type = NetSerialiser.GetPacketType(buffer).Item1;
        
        switch(type){
          case PacketType.ConnectPacket:
            PlayerConnectPacket packet = new();
            packet.Read(buffer);
            
            p.Join(packet);
            OnPlayerJoin.Invoke(p);
            break;
          default:
            p.OnRecieveTcpPacket(buf);
            break;
        }
      };

      socket.OnClose += () =>
      {
        _players.Remove(p);
        p.Leave();

        OnPlayerLeave.Invoke(p);
      };
    };
    
    tcpServer.Run();
  }
}