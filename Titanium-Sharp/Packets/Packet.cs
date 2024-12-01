using Titanium.Net.Structs;
using Buffer = Titanium.Net.Buffer;

namespace Titanium;

public abstract class Packet
{
  public abstract Buffer GetBuffer();
}