using System.Diagnostics;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using HighPerformanceTCP;

var word = "salam";

Console.WriteLine("Длина сообщения = " + word.Length);


Stopwatch stopwatch = new Stopwatch();
stopwatch.Start();


var tcpClient = await HPTcp.ClientConnectAsync();
var stream = tcpClient.GetStream();

for (var i = 0; i < 100; i++)
{
    await HPTcp.SendMessageAsync(Convert.ToByte(3), "", stream);
    await HPTcp.GetMessageAsync(stream);
}

stopwatch.Stop();

HPTcp.ClientDisconnect(tcpClient);

Console.WriteLine($"Время работы {stopwatch.ElapsedMilliseconds}");
Console.WriteLine($"Сообщение: ");
