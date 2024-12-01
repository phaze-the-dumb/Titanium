using System.Net.Sockets;
using Titanium.Logic.Structs;

namespace Titanium;

public static class Vars
{
  public static int MaxTcpSize = 1100;
  public static int Port = 6567;

  public static GameState State;
  public static UdpClient GlobalUdpServer;
}