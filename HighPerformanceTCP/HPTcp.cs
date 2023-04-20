using System;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace HighPerformanceTCP;
public class HPTcp
{
    //static string ServerPath = "172.16.172.212";
    //static string ServerPath = "37.192.37.60";
    static string ServerPath = "127.0.0.1";
    static int ServerPort = 8091;
    static int HeaderLenght = 6;


    public static async Task<TcpClient> ClientConnectAsync()
    {
        TcpClient tcpClient = new TcpClient();
        //await tcpClient.ConnectAsync("127.0.0.1", 8888);
        await tcpClient.ConnectAsync(ServerPath, ServerPort);
        
        return tcpClient;
    }

    public static async Task<byte[]> SendMessageAsync(byte commandType, string message, Stream stream)
    {
        var byteLenght = BitConverter.GetBytes(HeaderLenght + message.Length);
        byte[] data = new byte[HeaderLenght];
        data[0] = Convert.ToByte(0x00);
        data[1] = commandType;
        data[2] = byteLenght[3];
        data[3] = byteLenght[2];
        data[4] = byteLenght[1];
        data[5] = byteLenght[0];

        var mesageData = data;//data.Concat(Encoding.UTF8.GetBytes(message + '\0')).ToArray();
        await stream.WriteAsync(mesageData);

        return mesageData;
    }

    public static void ClientDisconnect(TcpClient tcpClient)
    {
        tcpClient.Close();
    }

    public static async Task<string> GetMessageAsync(Stream stream)
    {
        var responseHeader = new byte[HeaderLenght];

        await stream.ReadAsync(responseHeader);


        var messageLenght = responseHeader[5] + (responseHeader[4] * 16) + (responseHeader[3] * 256) + (responseHeader[2] * 4096);

        if (messageLenght == 6)
            return "";


        var response = new byte[messageLenght];

        await stream.ReadAsync(response);


        //while (responseJson.Count() < messageLenght)
          //  responseJson.AddAfter(responseJson.Last, (byte)(stream.ReadAsync()));

        return Encoding.UTF8.GetString(response.ToArray());

    }
}

