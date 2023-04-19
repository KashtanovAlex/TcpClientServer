using System.Net.Sockets;
using System.Text;
using HighPerformanceTCP;

var word = "salam";

Console.WriteLine("Длина сообщения = " + word.Length);

var stream = await HPTcp.ClientConnectAsync();

await HPTcp.SendMessageAsync(Convert.ToByte(0x03), word, stream);
var translation = await HPTcp.GetMessageAsync(stream);

//Console.WriteLine($"Сообщение: {translation}");
//Console.WriteLine($"Сообщение: {translation}");
