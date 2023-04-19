using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace HighPerformanceTCP;
public class HPTcp
{
    static string ServerPath = "172.16.172.212";
    //static string ServerPath = "37.192.37.60";
    static int ServerPort = 8091;

    public static async Task<Stream> ClientConnectAsync()
    {
        TcpClient tcpClient = new TcpClient();
        //await tcpClient.ConnectAsync("127.0.0.1", 8888);
        await tcpClient.ConnectAsync(ServerPath, ServerPort);
        
        var stream = tcpClient.GetStream();
        return stream;
    }

    public static async Task<byte[]> SendMessageAsync(byte commandType, string message, Stream stream)
    {
        var byteLenght = BitConverter.GetBytes(6 + message.Length);
        byte[] data = new byte[6];
        data[0] = Convert.ToByte(0x00);
        data[1] = commandType;
        data[2] = byteLenght[3];
        data[3] = byteLenght[2];
        data[4] = byteLenght[1];
        data[5] = byteLenght[0];

        var mesageData = data.Concat(Encoding.UTF8.GetBytes(message + '\0')).ToArray();
        await stream.WriteAsync(mesageData);

        return mesageData;
    }

    public static async Task ClientDisconnectAsync()
    {
    }

    public static async Task<string> GetMessageAsync(Stream stream)
    {
        // получение заголовка
        var responseHeader = new byte[6];
        for (int i =0; responseHeader.Count() <= 5; i++)
        {
            responseHeader[i] = (byte)(stream.ReadByte());
        }

        var messageLenght = responseHeader[5] + (responseHeader[4] * 16) + (responseHeader[3] * 256) + (responseHeader[2] * 4096);

        if (messageLenght == 6)
            return "";

        var responseJson = new LinkedList<byte>();
        responseJson.AddFirst((byte)(stream.ReadByte()));

        while (responseJson.Count() < messageLenght)
        {
            responseJson.AddAfter(responseJson.Last, (byte)(stream.ReadByte()));
        }
        return Encoding.UTF8.GetString(responseJson.ToArray());

    }
}

