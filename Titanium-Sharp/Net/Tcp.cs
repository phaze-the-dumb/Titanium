using System.Net;
using System.Net.Sockets;

namespace Titanium.Net;

public class TcpServer
{
  public Action<TcpSocket> OnConnection;
  
  private int _port;
  private TcpListener _listener;

  public TcpServer(int port)
  {
    _port = port;
  }

  public void Run()
  {
    _listener = new(IPAddress.Parse("0.0.0.0"), _port);
    _listener.Start();

    while (true)
    {
      TcpClient client = _listener.AcceptTcpClient();
      TcpSocket socket = new(client);

      new Thread(() =>
      {
        OnConnection.Invoke(socket);
        socket.Run();
      }).Start();
    }
  }
}