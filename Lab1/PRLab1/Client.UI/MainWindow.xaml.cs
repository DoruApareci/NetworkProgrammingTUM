using Client.BL;
using Client.UI.Commands;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Client.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public ClientBL Client { get; set; }
        public string Messages { get; set; } = "";

        private readonly IMessager _messager;

        public ICommand ConnectCommand { get; set; }
        public bool CanConnect(object p)
        { return !Client.IsConnected && Client.Username.Length!=0; }
        public ICommand DisconnecCommand { get; set; }
        public bool CanDisonnect(object p)
        { return Client.IsConnected; }
        public ICommand SendMessageCommand { get; set; }
        public bool CanSendMessage(object p)
        { return Client.IsConnected && Client.Username.Length != 0 && Client.UserMessage.Length!=0; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _messager = new ClientMessager(this);
            Client = new ClientBL(_messager);
            ConnectCommand = new BaseCommand(new Action<object>(Client.ConnectServer), new Predicate<object>(CanConnect));
            DisconnecCommand = new BaseCommand(new Action<object>(Client.DisconnectServer), new Predicate<object>(CanDisonnect));
            SendMessageCommand = new BaseCommand(new Action<object>(Client.Send), new Predicate<object>(CanSendMessage));
        }
    }
}