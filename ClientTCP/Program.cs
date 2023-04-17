using System.Net.Sockets;
using System.Text;

// слова для отправки для получения перевода
//var words = new string[] { "red", "yellow", "blue", "green" };

var word = string.Empty;
for (int i = 0; i < 100000; i++)
{
    word += "TestingBigstring" + i + "TestingBigstring" + "TestingBigstring";
}


Console.WriteLine("Длина сообщения = " + word.Length);


using TcpClient tcpClient = new TcpClient();
await tcpClient.ConnectAsync("127.0.0.1", 8888);
var stream = tcpClient.GetStream();

// буфер для входящих данных
var response = new List<byte>();
int bytesRead = 10; // для считывания байтов из потока


// считываем строку в массив байтов
// при отправке добавляем маркер завершения сообщения - \n
byte[] data = Encoding.UTF8.GetBytes(word + '\n');
// отправляем данные
await stream.WriteAsync(data);

// считываем данные до конечного символа
while ((bytesRead = stream.ReadByte()) != '\n')
{
    // добавляем в буфер
    response.Add((byte)bytesRead);
}
var translation = Encoding.UTF8.GetString(response.ToArray());
Console.WriteLine($"Слово {word}: {translation}");
response.Clear();
// имитируем долговременную работу, чтобы одновременно несколько клиентов обрабатывались
await Task.Delay(2000);


// отправляем маркер завершения подключения - END
//await stream.WriteAsync(Encoding.UTF8.GetBytes("END\n"));