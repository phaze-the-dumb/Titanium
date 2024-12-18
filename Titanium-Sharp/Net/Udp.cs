﻿using System.Net;
using System.Net.Sockets;
using Titanium.Net.Structs;

namespace Titanium.Net;

public class UdpServer
{
  public Action<UDPPacket, Address> OnRecievePacket;
  public Action<byte[], string> OnRecieveProxyablePacket;

  private int _port;
  private UdpClient _listener;

  public UdpServer(int port)
  {
    _port = port;
  }
  
  public void Run()
  {
    _listener = new(_port);
    Vars.GlobalUdpServer = _listener;
    
    var groupEp = new IPEndPoint(IPAddress.Any, _port);

    while (true)
    {
      var bytes = _listener.Receive(ref groupEp);

      if (bytes[0] == 254)
      {
        switch (bytes[1])
        {
          case 1:
            OnRecievePacket.Invoke(new UDPPacket(UDPPacketType.ServerData, bytes), new Address(groupEp.Address, groupEp.Port));
            break;
          case 3:
            OnRecievePacket.Invoke(new UDPPacket(UDPPacketType.Authentication, bytes), new Address(groupEp.Address, groupEp.Port));
            break;
          default:
            OnRecievePacket.Invoke(new UDPPacket(UDPPacketType.Unknown, bytes), new Address(groupEp.Address, groupEp.Port));
            break;
        }
      }
      else
      {
        OnRecieveProxyablePacket.Invoke(bytes, groupEp.Address + ":" + groupEp.Port);
      }
    }
  }

  public void SendTo(Packet packet, Address addr)
  {
    var dat = packet.GetBuffer().Export();
    _listener.Send(dat, dat.Length, addr.IpAddress.ToString(), addr.Port);
  }
}