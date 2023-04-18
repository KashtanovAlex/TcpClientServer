using System.Net.Sockets;
using System.Text;
using HighPerformanceTCP;

var word = "salam";


Console.WriteLine("Длина сообщения = " + word.Length);


var client = await HPTcp.ClientConnectAsync();
var stream = client.GetStream();

await HPTcp.SendMessageAsync(Convert.ToByte(0x03), word, stream);

// получение заголовка
var response = new List<byte>();
while (response.Count() <= 6)
{
    response.Add((byte)(stream.ReadByte()));
}

int messageLenght = Convert.ToInt32(new byte[]{response[5], response[4], response[3], response[2]});
while (response.Count() <= messageLenght)
{
    response.Add((byte)(stream.ReadByte()));
}


var translation = Encoding.UTF8.GetString(response.ToArray());
Console.WriteLine($"Сообщение: {translation}");
response.Clear();


// отправляем маркер завершения подключения - END
//await stream.WriteAsync(Encoding.UTF8.GetBytes("END\n"));