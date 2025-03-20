using Server.BL;
using Server.UI.Commands;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Server.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public string Logs { get; set; } = "";
        public string Messages { get; set; } = "";

        private readonly ILogger _logger;
        private readonly IMessager _messager;

        public ServerBL Server { get; set; }

        public ICommand StartServerCommand { get; set; }
        public bool CanServerStart(object p)
        { return !Server.IsServerRunning; }

        public ICommand StopServerCommand { get; set; }
        public bool CanServerStop(object p)
        { return Server.IsServerRunning; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            _logger = new ServerLogger(this);
            _messager = new ServerMessager(this);
            this.Server = new ServerBL(this._logger, this._messager);
            StartServerCommand = new BaseCommand(new Action<object>(Server.StartServer),
                                                  new Predicate<object>(CanServerStart));
            StopServerCommand = new BaseCommand(new Action<object>(Server.StopServer),
                                                new Predicate<object>(CanServerStop));
        }
    }
}