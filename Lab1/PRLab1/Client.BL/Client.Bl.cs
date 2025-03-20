using System.ComponentModel;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Client.BL
{
    public class ClientBL : INotifyPropertyChanged
    {
        public  TcpClient tcpClient { get; set; }
        public string IP { get; set; } = "127.0.0.1";
        private IPAddress ServerIP { get; set; }
        public int Port { get; set; } = 9000;
        public string Username { get; set; } = "";
        public string UserMessage { get; set; } = "";
        public volatile bool IsConnected = false;
        private readonly IMessager _messager;
        private Thread receiveThread { get; set; }

        public ClientBL(IMessager messager)
        {
            _messager = messager;
        }

        private void Init()
        {
            try
            {
                ServerIP = IP == "Any" ? IPAddress.Any : IPAddress.Parse(IP);
            }
            catch
            {
                throw new Exception("Invalid IP was submited");
            }
        }

        public void ConnectServer(object param)
        {
            Init();
            tcpClient = new TcpClient();
            tcpClient.Connect(ServerIP, Port);
            receiveThread = new Thread(new ThreadStart(ReceiveMessages));
            IsConnected = true;
            receiveThread.Start();
        }

        private void ReceiveMessages()
        {
            while (IsConnected)
            {
                try
                {
                    NetworkStream clientStream = tcpClient.GetStream();
                    byte[] message = new byte[4096];
                    int bytesRead = clientStream.Read(message, 0, 4096);
                    if (bytesRead == 0)
                    {
                        _messager.Message("", "Server closed the connection");
                        IsConnected = false;
                        break;
                    }
                    string serverMessage = Encoding.ASCII.GetString(message, 0, bytesRead);
                    _messager.Message(SenderComp(serverMessage), MessageComp(serverMessage));
                }
                catch (Exception)
                {
                    IsConnected = false;
                    _messager.Message("", "Disconnected from chat room");
                    return;
                }
            }
        }
        public void Send(object param)
        {
            SendMessage(UserMessage);
            UserMessage = "";
        }

        public void SendMessage(string message)
        {
            NetworkStream clientStream = tcpClient.GetStream();
            byte[] buffer = Encoding.ASCII.GetBytes($"@{Username}@_@:{message}");
            clientStream.Write(buffer, 0, buffer.Length);
            clientStream.Flush();
        }

        public void DisconnectServer(object param)
        {
            IsConnected = false;
            SendMessage("");
            tcpClient.Close();
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
}
