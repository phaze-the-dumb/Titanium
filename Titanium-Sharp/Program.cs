using System.Net;
using Titanium.Logic;
using Titanium.Net.Structs;

internal class Program
{
  private static void Main()
  {
    Titanium.Main titanium = new(Titanium.Vars.Port);

    titanium.OnPlayerJoin += ( Player player ) => {
      player.JoinTo(new Address(new IPAddress([ 127, 0, 0, 1 ]), 6568));
    };

    titanium.OnPlayerLeave += ( Player player ) => {

    };
  }
}