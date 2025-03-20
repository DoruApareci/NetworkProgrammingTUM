using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using UDPChat.Models;
using UDPChat.Views;

namespace UDPChat
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public List<Friend> Friends { get; set; }
        public int Port { get; set; } = 17777;
        public string Username { get; set; } = "Username" + Guid.NewGuid().ToString().Substring(0, 5).ToUpper();

        private Socket socket;
        private Thread listenThread;
        private bool IsListening = false;

        public ICommand ConnectCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }

        public ChatView SelectedChat { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            ConnectCommand = new BaseCommand(Connect, CanConnect);
            DisconnectCommand = new BaseCommand(Disconnect, CanDisconnect);
            SelectedChat = new ChatView();
        }

        private void Disconnect(object param)
        {
            IsListening = false;
            SendMessage(MessageType.Broadcast, ActionType.Deauthorize);
            socket.Disconnect(true);
            Friends.Clear();
        }

        private bool CanDisconnect(object param)
        {
            return IsListening;
        }

        private void Connect(object param)
        {
            if (socket != null)
            {
                socket.Disconnect(true);
            }
            socket = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Bind(new IPEndPoint(IPAddress.Any, Port));
            socket.EnableBroadcast = true;
            Friends = new();
            Friends.Add(
                new Friend()
                {
                    IP = IPAddress.Broadcast,
                    Username = "General",
                    Messages = new List<string>()
                });
            IsListening = true;
            StartListening();
        }

        private bool CanConnect(object param)
        {
            if (Port <= 10000 && string.IsNullOrEmpty(Username) && !IsListening)
                return false;
            return true;
        }

        private void SendMessage(MessageType type, ActionType action, string? message = null, int recipientIndex = 0)
        {
            byte[] bytes;
            switch (type)
            {
                case MessageType.Broadcast:
                    {
                        bytes = action switch
                        {
                            ActionType.Authorize => Encoding.UTF8.GetBytes($"BROADCAST>_<Authorize>_<{Username}"),
                            ActionType.Deauthorize => bytes = Encoding.UTF8.GetBytes($"BROADCAST>_<Deauthorize"),
                            _ => bytes = Encoding.UTF8.GetBytes($"BROADCAST>_<>_<{message}")
                        };
                    }
                    break;

                case MessageType.Private:
                    {
                        bytes = action switch
                        {
                            ActionType.None => bytes = Encoding.UTF8.GetBytes($"PRIVATE>_<>_<{message}"),
                            ActionType.Authorized => bytes = Encoding.UTF8.GetBytes($"PRIVATE>_<Authorized>_<{Username}"),
                            _ => throw new Exception("Something went wrong")
                        };
                    }
                    break;

                default: throw new Exception("Something went wrong");
            }
            IPEndPoint endPoint = new IPEndPoint(Friends[recipientIndex].IP, Port);
            if((type==MessageType.Private)&&(action == ActionType.None))
                Friends[recipientIndex].Messages.Add($"{Username}: {message}");
            socket.SendTo(bytes, endPoint);
        }

        private void RecieveMessage()
        {
            try
            {
                EndPoint receiveEndPoint = new IPEndPoint(IPAddress.Any, 0);
                while (IsListening)
                {
                    byte[] receivedBytes = new byte[1024];
                    socket.ReceiveFrom(receivedBytes, ref receiveEndPoint);
                    string[] response = Encoding.UTF8.GetString(receivedBytes).Split(">_<");

                    if (response[0] == "BROADCAST")
                    {
                        if (response[1] == "Authorize")
                        {
                            AddFriend(new Friend() { IP = ((IPEndPoint)receiveEndPoint).Address, Username = response[2].Replace("\0", string.Empty), Messages = new() });
                            SendMessage(MessageType.Private, ActionType.Authorized, "",Friends.IndexOf(Friends.Where(x => x.IP == ((IPEndPoint)receiveEndPoint).Address).First()));
                            continue;
                        }
                        else if (response[1] == "Deauthorize")
                        {
                            RemoveFriend(((IPEndPoint)receiveEndPoint).Address);
                            continue;
                        }
                        else
                        {
                            var username = FriendUsername(((IPEndPoint)receiveEndPoint).Address);
                            lock (Friends[0].Messages)
                            {
                                Friends[0].Messages.Add($"{username}: {response[2]}"); // log general message
                                SelectedChat.UpdateUI();
                            }
                            continue;
                        }
                    }
                    else if (response[0] == "PRIVATE")
                    {
                        if (response[1] == "Authorized")
                        {
                            if (Username != response[2].Replace("\0", string.Empty) && !Friends.Exists(c => c.Username== response[2].Replace("\0", string.Empty)))
                                AddFriend(new Friend() { IP = ((IPEndPoint)receiveEndPoint).Address, Username = response[2].Replace("\0", string.Empty), Messages = new() });
                            continue;
                        }
                        var fr = Friends.First(x => x.IP.ToString() == ((IPEndPoint)receiveEndPoint).Address.ToString());
                        lock (fr.Messages)
                        {
                            fr.Messages.Add($"{fr.Username}: {response[2]}");// log private message
                            SelectedChat.UpdateUI();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void StartListening()
        {
            SendMessage(MessageType.Broadcast, ActionType.Authorize);
            listenThread = new Thread(new ThreadStart(RecieveMessage));
            listenThread.IsBackground = true;
            IsListening = true;
            listenThread.Start();
        }

        private void RemoveFriend(IPAddress address)
        {
            var toRem = Friends.Where(x => x.IP == address).First();
            if (toRem != null)
            {
                Friends.Remove(toRem);
                UpdateFriendList();
            }
        }

        private void AddFriend(Friend friend)
        {
            Friends.Add(friend);
            UpdateFriendList();
        }

        private void UpdateFriendList()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                FriendList.Items.Refresh();
            });
        }

        private string FriendUsername(IPAddress ip)
        {
            var fr = Friends.First(x => x.IP.ToString() == ip.ToString());
            return fr.Username;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                var selectedFriend = e.AddedItems[0] as Friend;
                if (selectedFriend != null)
                {
                    SelectedChat.NewMessage = "";
                    SelectedChat.Friend = selectedFriend;

                    void SendPublicMessage(object param)
                    {
                        SendMessage(MessageType.Broadcast, ActionType.None, SelectedChat.NewMessage, 0);
                        SelectedChat.NewMessage = "";
                    }

                    void SendPrivateMessage(object param)
                    {
                        SendMessage(MessageType.Private, ActionType.None, SelectedChat.NewMessage, Friends.IndexOf(selectedFriend));
                        SelectedChat.NewMessage = "";
                    }

                    bool CanSendMessage(object param)
                    {
                        return !string.IsNullOrEmpty(SelectedChat.NewMessage);
                    }

                    if (selectedFriend == Friends[0])
                    {
                        SelectedChat.SendMessageCommand = new BaseCommand(SendPublicMessage, CanSendMessage);
                    }
                    else
                    {
                        SelectedChat.SendMessageCommand = new BaseCommand(SendPrivateMessage, CanSendMessage);
                    }
                }
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            IsListening = false;
            socket.Close();
        }
    }
}