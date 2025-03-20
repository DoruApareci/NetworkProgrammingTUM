using System.Net.Sockets;
using System.Net;
using System.Text;
using System.ComponentModel;
using PropertyChanged;

namespace Server.BL;

public class ServerBL : INotifyPropertyChanged
{
    public string IP { get; set; } = "127.0.0.1";
    private IPAddress ServerIP { get; set; }
    public int Port { get; set; } = 9000;
    private readonly ILogger _logger;
    private readonly IMessager _messager;
    private TcpListener tcpListener;
    private List<TcpClient> clients = new List<TcpClient>();
    private Thread listenerThread { get; set; }

    public bool IsServerRunning { get; set; }

    public ServerBL(ILogger logger, IMessager messager)
    {
        _logger = logger;
        _messager = messager;
    }

    private void Init()
    {
        _logger.Log("Init socket: ProtocolType.Tcp\n");
        try
        {
            ServerIP = IP == "Any" ? IPAddress.Any : IPAddress.Parse(IP);
            _logger.Log("IP adress parsed succesfuly\n");
        }
        catch
        {
            _logger.Log("Invalid IP was submited\n");
            throw new Exception("Invalid IP was submited");
        }
    }

    public void StartServer(object param)
    {
        Init();
        tcpListener = new TcpListener(ServerIP, Port);
        tcpListener.Start();
        listenerThread = new Thread(new ThreadStart(ListenForClients));
        listenerThread.IsBackground = true;
        listenerThread.Start();
        IsServerRunning = true;
        _logger.Log($"Server listening on port: {Port}\n");
    }

    private void ListenForClients()
    {
        while (IsServerRunning)
        {
            if (!IsServerRunning)
                break;
            TcpClient client = tcpListener == null ? null:tcpListener.AcceptTcpClient();
            if (client != null)
            {
                clients.Add(client);
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientConn));
                clientThread.Start(client);
            }
        }
    }

    private void HandleClientConn(object clientObj)
    {
        TcpClient tcpClient = (TcpClient)clientObj;
        NetworkStream clientStream = tcpClient.GetStream();
        _logger.Log("Client connected\n");

        byte[] message = new byte[4096];
        int bytesRead;

        while (IsServerRunning)
        {
            bytesRead = 0;
            try
            {
                bytesRead = clientStream.Read(message, 0, 4096);
            }
            catch
            {
                break;
            }
            string clientMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
            if (MessageComp(clientMessage) == "")
                break;
            _messager.Message(SenderComp(clientMessage), MessageComp(clientMessage));
            BroadcastMessage(clientMessage);
        }
        clients.Remove(tcpClient);
        tcpClient.Close();
        _logger.Log("Client disconnected\n");
    }

    private void BroadcastMessage(string message)
    {
        foreach (TcpClient client in clients)
        {
            NetworkStream clientStream = client.GetStream();
            byte[] broadcastMessage = Encoding.ASCII.GetBytes(message);
            clientStream.Write(broadcastMessage, 0, broadcastMessage.Length);
            clientStream.Flush();
        }
    }

    public void StopServer(object param)
    {
        BroadcastMessage("");
        IsServerRunning = false;
        tcpListener.Stop();
        foreach (TcpClient client in clients)
        {
            client.Close();
        }
        _logger.Log("Server stopped\n");
    }

    private static string SenderComp(string message)
    {
        return message.Substring(0, message.IndexOf("@_@"));
    }

    private static string MessageComp(string message)
    {
        return message.Substring(message.IndexOf("@_@") + 4, message.Length - (SenderComp(message).Length + 4));
    }

    public event PropertyChangedEventHandler? PropertyChanged;
}
