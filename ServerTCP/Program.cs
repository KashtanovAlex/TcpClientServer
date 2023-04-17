using System.Net;
using System.Net.Sockets;
using System.Text;
 
var tcpListener = new TcpListener(IPAddress.Any, 8888);

try
{
    tcpListener.Start();    // запускаем сервер
    Console.WriteLine("Сервер запущен. Ожидание подключений... ");

    while (true)
    {
        // получаем подключение в виде TcpClient
        var tcpClient = await tcpListener.AcceptTcpClientAsync();

        // создаем новую задачу для обслуживания нового клиента
        Task.Run(async () => await ProcessClientAsync(tcpClient));

        // вместо задач можно использовать стандартный Thread
        // new Thread(async ()=>await ProcessClientAsync(tcpClient)).Start();
    }
}
finally
{
    tcpListener.Stop();
}
// обрабатываем клиент
async Task ProcessClientAsync(TcpClient tcpClient)
{
    // условный словарь
    var words = new Dictionary<string, string>()
    {
        {"red", "красный" },
        {"blue", "синий" },
        {"green", "зеленый" },
    };
    var stream = tcpClient.GetStream();
    // буфер для входящих данных
    var response = new List<byte>();
    int bytesRead = 10;
    while (true)
    {
        // считываем данные до конечного символа
        while ((bytesRead = stream.ReadByte()) != '\n')
        {
            // добавляем в буфер
            response.Add((byte)bytesRead);
        }
        var word = Encoding.UTF8.GetString(response.ToArray());

        // если прислан маркер окончания взаимодействия,
        // выходим из цикла и завершаем взаимодействие с клиентом
        if (word == "END") break;

        Console.WriteLine($"Размер водного пакета:" + word.Length);

        await stream.WriteAsync(Encoding.UTF8.GetBytes("э ты, не пиши сюда больше"));
        response.Clear();
    }
    tcpClient.Close();
}