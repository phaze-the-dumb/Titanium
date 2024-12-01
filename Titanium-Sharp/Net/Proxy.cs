using System.Net;
using System.Net.Sockets;
using Titanium.Logic;
using Titanium.Net.Structs;
using Titanium.Packets.TCP;

namespace Titanium.Net;

public class Proxy
{
  private UdpClient _proxyUdpClient = new();
  private TcpClient _proxyTcpClient = new();

  private NetworkStream _stream;
  
  private bool _connectedToProxy = false;

  private TcpSocket _playerTcpSocket;
  private Address _playerUdpAddress;

  private Address _connectedServer;

  private bool _hasAuthenticated = false;

  private Player _player;

  public Proxy(Player player, Address remoteServer)
  {
    Console.WriteLine("Connecting " + player.Name + " to " + remoteServer.IpAddress + ":" + remoteServer.Port);

    _player = player;
    _connectedServer = remoteServer;
    
    _proxyUdpClient.Connect(remoteServer.IpAddress, remoteServer.Port);
    _proxyTcpClient.Connect(remoteServer.IpAddress, remoteServer.Port);

    _connectedToProxy = true;

    _playerTcpSocket = player.GetTcpSocket();
    _playerUdpAddress = player.GetUdpAddress();
    
    new Thread(ProxyUdpLoop){ Name = "UdpThread-Proxy-" + player.ID }.Start();
    new Thread(ProxyTcpLoop){ Name = "TcpThread-Proxy-" + player.ID }.Start();
  }

  private void ProxyUdpLoop()
  {
    IPEndPoint proxyIpEndPoint = new IPEndPoint(IPAddress.Any, 0);

    try
    {
      while (_connectedToProxy)
      {
        byte[] proxyRecieve = _proxyUdpClient.Receive(ref proxyIpEndPoint);

        if (_hasAuthenticated)
        {
          Vars.GlobalUdpServer.Send(
            new ReadOnlySpan<byte>(proxyRecieve),
            _playerUdpAddress.IpAddress.ToString(),
            _playerUdpAddress.Port);
        }
      }
    }
    catch (SocketException e)
    {
      // Ignored
    }
  }

  private void ProxyTcpLoop()
  {
    _stream = _proxyTcpClient.GetStream();
    try
    {
      while (_connectedToProxy)
      {
        byte[] bytes = new byte[8_196];
        int length = _stream.Read(bytes);

        if (length == 0)
        {
          continue;
        }

        byte[] proxyRecieve = new byte[length];
        Array.Copy(bytes, proxyRecieve, length);

        Buffer buf = Buffer.From(proxyRecieve);
        PacketType type = NetSerialiser.GetPacketType(buf).Item1;

        switch (type)
        {
          case PacketType.FrameworkPacket:
            byte id = buf.GetByte();

            switch (id)
            {
              case 4:
                int idFromServer = buf.GetInt();
                HelloPacket packet = new(idFromServer);

                _proxyUdpClient.Send(packet.GetBuffer().Export());
                break;
              case 3:
                PlayerConnectPacket connect = new()
                {
                  Version = _player.Version,
                  VersionType = _player.VersionType,
                  Name = _player.Name,
                  Usid = _player.Usid,
                  Uuid = _player.Uuid,
                  Mobile = _player.Mobile,
                  Colour = _player.Colour,
                  Mods = _player.Mods,
                  Locale = _player.Locale,
                  Connector = _player.Connector
                };
                
                connect.Write();
                Buffer buffer = connect.GetBuffer();
                Buffer data = NetSerialiser.WritePacketType(PacketType.ConnectPacket, buffer.Length());
                
                data = data.Add(buffer);
                
                _stream.Write(data.Export());
                _hasAuthenticated = true;

                continue;
            }

            break;
        }

        if (_hasAuthenticated)
        {
          _playerTcpSocket.SendRaw(proxyRecieve);
        }
      }
    }
    catch (IOException e)
    {
      // Ignored
    }
  }

  public void Close()
  {
    _connectedToProxy = false;
    
    _proxyUdpClient.Close();
    _proxyTcpClient.Close();
  }
}