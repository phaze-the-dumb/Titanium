using System.Net;

namespace Titanium.Net.Structs;

public class Address
{
  public IPAddress IpAddress;
  public int Port;

  public Address( IPAddress ipAddress, int port )
  {
    IpAddress = ipAddress;
    Port = port;
  }
}