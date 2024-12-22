using System.Text;

namespace Titanium.Net;

public class Buffer
{
  private readonly List<byte> _data = new();

  public static Buffer From( byte[] bytes )
  {
    Buffer buf = new Buffer();
    buf._data.AddRange(bytes);

    return buf;
  }

  public byte GetByte()
  {
    var dat = _data[0];
    _data.RemoveAt(0);

    return dat;
  }

  public byte[] GetBytes(int count)
  {
    var dat = _data.Slice(0, count).ToArray();
    _data.RemoveRange(0, count);

    return dat;
  }

  public short GetShort()
  {
    var dat = GetBytes(2);
    return BitConverter.ToInt16(dat.Reverse().ToArray());
  }

  public int GetInt()
  {
    var dat = GetBytes(4);
    return BitConverter.ToInt32(dat.Reverse().ToArray());
  }

  public string GetStringChecked()
  {
    byte exists = GetByte();
    if (exists == 0)
    {
      return "";
    }

    GetByte();
    
    int length = GetByte();
    var dat = GetBytes(length);

    return Encoding.ASCII.GetString(dat);
  }
  
  public string GetString()
  {
    int length = GetByte();
    var dat = GetBytes(length);

    return Encoding.ASCII.GetString(dat);
  }

  public void PutByte(byte dat)
  {
    _data.Add(dat);
  }

  public void PutBytes(byte[] dat)
  {
    _data.AddRange(dat);
  }

  public void PutShort(short num)
  {
    var dat = BitConverter.GetBytes(num)
      .Reverse().ToArray();

    PutBytes(dat);
  }

  public void PutInt(int num)
  {
    var dat = BitConverter.GetBytes(num)
      .Reverse().ToArray();

    PutBytes(dat);
  }

  public void PutString(string text)
  {
    var dat = Encoding.ASCII.GetBytes(text);

    PutByte(Convert.ToByte(text.Length));
    PutBytes(dat);
  }
  
  public void PutStringChecked(string text)
  {
    PutBytes([ 1, 0 ]);
    var dat = Encoding.ASCII.GetBytes(text);

    PutByte(Convert.ToByte(text.Length));
    PutBytes(dat);
  }

  public Buffer Add(Buffer buf)
  {
    PutBytes(buf.Export());
    return this;
  }

  public short Length()
  {
    return (short)_data.Count;
  }

  internal Byte[] Export()
  {
    return _data.ToArray();
  }
}