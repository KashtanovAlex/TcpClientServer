using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using HighPerformanceTCP;

var word = "salam";

Console.WriteLine("Длина сообщения = " + word.Length);

var tcpClient = await HPTcp.ClientConnectAsync();
var stream = tcpClient.GetStream();

await HPTcp.SendMessageAsync(Convert.ToByte(2), word, stream);
var translation = await HPTcp.GetMessageAsync(stream);

HPTcp.ClientDisconnect(tcpClient);

Console.WriteLine($"Сообщение: {translation}");
Console.WriteLine($"Сообщение: {translation}");
