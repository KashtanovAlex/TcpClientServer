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

    var stream = tcpClient.GetStream();
    // буфер для входящих данных
    int bytesRead = 10;
    while (true)
    {
        var initResponse = new List<byte>();

        // считываем данные до конечного символа
        while (initResponse.Count <= 4)
        {
            // добавляем в буфер
            initResponse.Add((byte)(bytesRead = stream.ReadByte()));
        }

        var respLenght = (int)initResponse[2] + (int)initResponse[3];

        var response = new List<byte>();// переделать в массив

        while (response.Count <= respLenght)
        {
            response.Add((byte)(bytesRead = stream.ReadByte()));
        }


        var word = Encoding.UTF8.GetString(response.ToArray());

        // если прислан маркер окончания взаимодействия,
        // выходим из цикла и завершаем взаимодействие с клиентом
        if (word == "END") break;

        Console.WriteLine($"Расчетная длина текста:" + respLenght);

        Console.WriteLine($"Размер водного пакета:" + word.Length);

        await stream.WriteAsync(Encoding.UTF8.GetBytes("получен ответ"));
        response.Clear();
    }
    tcpClient.Close();
}