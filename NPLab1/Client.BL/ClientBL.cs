using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client.BL;

public class ClientBL
{
    private readonly IClientLogger _logger;
    private IPAddress _serverIP;
    private int _port;

    private bool _isConnected;
    private TcpClient _tcpClient;
    private Thread _receiverThread;

    public ClientBL(IClientLogger logger, string ServerIP = "127.0.0.1", int Port = 9000)
    {
        _logger = logger;
        ParseIP(ServerIP);
        _port = Port;
        _tcpClient = new TcpClient();
        _receiverThread = new Thread(new ThreadStart(ReceiveMessages));
    }

    private void ParseIP(string IP)
    { 
        try
        {
            _serverIP = IPAddress.Parse(IP);
        }
        catch (Exception e)
        {
            _logger.Log(e.Message); // IP-ul introdus este invalid
                                    //     (format: "Any" pentru conexiuni externe/ "127.0.0.1" pentru conexiuni locale)
        }
    }

    public void Connect()
    {
        _tcpClient.Connect(_serverIP, _port);
        _isConnected = true;
        _receiverThread.Start();
    }

    public void Disconnect()
    {
        _isConnected = false;
        SendMessage("");
        _tcpClient?.Close();
    }

    public void SendMessage(string message)
    {
        NetworkStream clientStream = _tcpClient.GetStream();
        byte[] buffer = Encoding.UTF8.GetBytes(message);
        clientStream.Write(buffer, 0, buffer.Length);
        clientStream.Flush();
    }

    private void ReceiveMessages()
    {
        while (_isConnected)
        {
            try
            {
                NetworkStream clientStream = _tcpClient.GetStream();
                byte[] message = new byte[4096];
                int bytesRead = clientStream.Read(message, 0, 4096);
                if (bytesRead == 0)
                {
                    _logger.Log("Server closed the connection");
                    _isConnected = false;
                    break;
                }
                string serverMessage = Encoding.UTF8.GetString(message, 0, bytesRead);
                _logger.Log(serverMessage);

            }
            catch (Exception e)
            {
                _logger.Log($"Some error when recieving a message from server\n{e.Message}");
            }

        }
    }
}
