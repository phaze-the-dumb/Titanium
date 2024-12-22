namespace Titanium.Net.Structs;

public class Colour
{
  private int _red;
  private int _green;
  private int _blue;
  
  public int Red
  {
    get { return _red; }
  }
  
  public int Green
  {
    get { return _green; }
  }
  
  public int Blue
  {
    get { return _blue; }
  }
  
  public Colour(int colour)
  {
    byte[] bytes = BitConverter.GetBytes(colour);

    _red = bytes[3];
    _green = bytes[2];
    _blue = bytes[1];
  }

  public int ToInt()
  {
    byte[] bytes = [ 255, (byte)_blue, (byte)_green, (byte)_red ];
    return BitConverter.ToInt32(bytes);
  }
}