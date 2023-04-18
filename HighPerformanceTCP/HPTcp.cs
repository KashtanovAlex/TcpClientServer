using System.IO;
using System.Net.Sockets;
using System.Text;

namespace HighPerformanceTCP;
public static class HPTcp
{
    //static string ServerPath = "172.16.172.212";
    static string ServerPath = "37.192.37.60";
    static int ServerPort = 8091;

    public static async Task<TcpClient> ClientConnectAsync()
    {
        using TcpClient tcpClient = new TcpClient();
        //await tcpClient.ConnectAsync("127.0.0.1", 8888);
        await tcpClient.ConnectAsync(ServerPath, ServerPort);
        return tcpClient;
    }

    public static async Task<byte[]> SendMessageAsync(byte commandType, string message, Stream stream)
    {
        //var byteLenght = BitConverter.GetBytes(6 + message.Length);
        var byteLenght = BitConverter.GetBytes(6);
        byte[] data = new byte[] { Convert.ToByte(0x00), Convert.ToByte(0x03), byteLenght[3], byteLenght[2], byteLenght[1], byteLenght[0] };
        //data.Concat(Encoding.UTF8.GetBytes(message));
        await stream.WriteAsync(data.ToArray());

        return data;
    }

    public static async void GetMessageAsync()
    {

    }
}

