using DNSTools;
using PropertyChanged;
using System.ComponentModel;
using System.Net;
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

namespace DNSApp.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        [AlsoNotifyFor("ConsoleOutput")]
        public StringBuilder COutput { get; private set; }
        public string ConsoleOutput { get { return COutput.ToString(); } }
        public string CurrentDNS { get; set; }
        public string Command { get; set; }

        public DNSClient DNS { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
            Command = "";
            DNS = new DNSClient("8.8.8.8");
            CurrentDNS = "Current dns: 8.8.8.8";
            COutput = new ();
            Append("App started\n");
        }

        private void ProcessCommand()
        {
            if(Command.ToLower().StartsWith("use dns "))
            {
                UseDns();
                return;
            }
            else if(Command.ToLower().StartsWith("resolve "))
            {
                var q = Command.ToLower().Replace("resolve ", "");
                IPAddress adr;
                if(IPAddress.TryParse(q, out adr))
                {
                    Append(DNS.Resolve(adr));
                }
                else
                {
                    Append(DNS.Resolve(q));
                }
                return;
            }
            else
            {
                Append("\nUnknown command\n");
            }
        }

        private void UseDns()
        {
            var newDns = Command.ToLower().Replace("use dns ", "");
            if(DNS.UseDNS(newDns))
            {
                CurrentDNS = $"Current dns: {newDns}";
                Append($"\ndns was set to {newDns}\n");
                return;
            }
            Append($"\nunable to parse new dns\n");

        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if (string.IsNullOrEmpty(Command))
                    MessageBox.Show("No command found");
                ProcessCommand();
                Command = "";
            }
        }
        private void Append(string mess)
        {
            COutput.Append(mess);
            OnPropertyChanged(nameof(ConsoleOutput));
        }
    }
}