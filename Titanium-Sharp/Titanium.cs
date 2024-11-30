using Titanium.Logic;
using Titanium.Net;
using Titanium.Net.Structs;
using Titanium.Packets.TCP;
using Titanium.Packets.UDP;

namespace Titanium;

public class Main
{
  private int _port = 6567;

  private List<Player> _players = new();
  
  public Main()
  {
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
      Console.WriteLine("TCP Client Connected");
      Player p = new(socket);
      
      Console.WriteLine("Connecting Player: " + p.ID + ". Stage 1");
      
      socket.Send(new WelcomePacket(p).Bytes);
      _players.Add(p);
      
      socket.OnMessage += buffer =>
      {
        if (buffer.GetInt() == 4850432 && buffer.GetInt() == 1140977717)
        {
          Console.WriteLine("Connecting Player: " + p.ID + ". Stage 2");
          PlayerConnectPacket packet = new(buffer);
          
          Console.WriteLine(": " + packet.Version);
          Console.WriteLine(": " + packet.VersionType);
          Console.WriteLine(": " + packet.Name);
          Console.WriteLine(": " + packet.Locale);
          Console.WriteLine(": " + packet.Usid);
          Console.WriteLine(": " + packet.Uuid);
          Console.WriteLine(": " + packet.Mobile);
          Console.WriteLine(": " + packet.Colour);
          Console.WriteLine(": " + packet.Mods);
        }
      }; 

      socket.OnClose += () =>
      {
        _players.Remove(p);
      };
    };
    
    tcpServer.Run();
  }
}