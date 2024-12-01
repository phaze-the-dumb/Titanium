using Titanium.Net.Structs;
using Buffer = Titanium.Net.Buffer;

namespace Titanium;

public class Packet
{
  protected Buffer _buf = new();
  
  public Buffer Bytes
  {
    get { return _buf; }
  }
}