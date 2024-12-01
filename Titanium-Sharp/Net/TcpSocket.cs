using System.Net.Sockets;
using System.Text;

namespace Titanium.Net;

public class TcpSocket
{
  public Action<Buffer> OnMessage;
  public Action OnClose;

  private NetworkStream _stream;
  private TcpClient _socket;
  private bool _closed = false;
  
  public TcpSocket( TcpClient socket )
  {
    _socket = socket;
    _stream = _socket.GetStream();
  }

  public void Run()
  {
    // Runs in the "client" thread

    while (_closed == false)
    {
      var buffer = new byte[8_192];
      var length = _stream.Read(buffer);

      if (length > 0)
      {
        byte[] dat = new byte[length];
        Array.Copy(buffer, dat, length);
        
        OnMessage.Invoke(Buffer.From(dat));
      }
      else
      {
        Close();
      }
    }
  }

  public void Send(Buffer buf)
  {
    if(!_closed)
      _stream.Write(buf.Export());
  }
  
  public void SendRaw(byte[] buf)
  {
    if(!_closed)
      _stream.Write(buf);
  }

  public void Close()
  {
    if (!_closed)
    {
      _closed = true;
      
      _socket.Close();
      _socket.Dispose();
      
      OnClose.Invoke();
    }
  }
}