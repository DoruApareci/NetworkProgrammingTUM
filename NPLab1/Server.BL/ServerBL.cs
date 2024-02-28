using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Server.BL;

public class ServerBL
{
    private readonly IServerLogger _logger;
    private IPAddress _serverIP;
    private int _port;

    private bool _isRunning;
    private TcpListener _tcpListener;
    private List<TcpClient> _clients;
    private Thread _listenerThread;

    public ServerBL(IServerLogger logger, string ServerIP = "Any", int Port = 9000)
    {
        _logger = logger;
        ParseIP(ServerIP);
        _port = Port;
        _clients = new List<TcpClient>();
        InitListener();
        _listenerThread = new Thread(new ThreadStart(ListenForClients)); //cream un thread nou pentru a primi conexiunile de la clienti
    }

    private void InitListener()
    {
        try
        {
            _tcpListener = new TcpListener(_serverIP, _port);
        }
        catch (Exception e)
        {
            _logger.Log(e.Message);//mby ivalid port was submited
        }
    }

    private void ParseIP(string IP)
    {
        try
        {
            _serverIP = IP == "Any" ? IPAddress.Any : IPAddress.Parse(IP);
        }
        catch (Exception e)
        {
            _logger.Log(e.Message); // IP-ul introdus este invalid
                                    //     (format: "Any" pentru conexiuni externe/ "127.0.0.1" pentru conexiui locale)
        }
    }
    public void StartServer()
    {
        _tcpListener.Start();
        _isRunning = true;
        _listenerThread.Start();
        _logger.Log($"Server started: {_serverIP}:{_port}");
    }
    public void StopServer()
    {
        BroadcastMessage("");//la receptionarea unui mesaj gol, clientii urmeaza sa inchida conexiunile cu serverul
        _isRunning = false;
        foreach (TcpClient client in _clients)
            client.Close();
        _tcpListener.Stop();
        _logger.Log("Server stopped\n");
    }

    private void ListenForClients()
    {
        while (_isRunning)
        {
            try
            {
                TcpClient client = _tcpListener.AcceptTcpClient();
                if (client != null)
                {
                    _clients.Add(client); //Adaugam clientul nou in lista clientilor conectati
                    Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientConn)); //cream un ththread nou pentru fiecare client conetat
                    clientThread.Start(client);
                }
            }
            catch
            {
                //we need to do something here when _tcpListener is disposed
            }
        }
    }

    private void HandleClientConn(object? obj)
    {
        if (obj == null)
            return;
        TcpClient tcpClient = (TcpClient)obj;
        NetworkStream clientStream = tcpClient.GetStream();
        _logger.Log("New client connected\n");

        byte[] message = new byte[4096];
        int bytesRead;

        while (_isRunning)
        {
            bytesRead = 0;
            try
            {
                bytesRead = clientStream.Read(message, 0, 4096);
            }
            catch (Exception e)
            {
                _logger.Log(e.Message);
                break;
            }

            string clientMessage = Encoding.UTF8.GetString(message, 0, bytesRead);

            if (clientMessage == "") //daca mesajul primit este gol, inchidem conexiunea
                break;

            _logger.Log($"Client message: {clientMessage}");

            BroadcastMessage(clientMessage); //transmitem mesajul receptionat tuturor clientilor;
        }

        _clients.Remove(tcpClient);
        tcpClient.Close();

        _logger.Log("Client disconnected\n");
    }

    private void BroadcastMessage(string clientMessage) //facem broadcast unui mesaj tuturor clientilor conectati
    {
        foreach (TcpClient client in _clients)
        {
            NetworkStream clientStream = client.GetStream();
            byte[] broadcastMessage = Encoding.UTF8.GetBytes(clientMessage);
            clientStream.Write(broadcastMessage, 0, broadcastMessage.Length);
            clientStream.Flush();
        }
    }
}
