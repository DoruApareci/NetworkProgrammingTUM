using System.Net.Sockets;
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

namespace NTPClientApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string timeZoneInput = TimeZoneTextBox.Text.Trim();
            if (!IsValidTimeZoneFormat(timeZoneInput))
            {
                MessageBox.Show("Format invalid! Utilizați GMT+X sau GMT-X.");
                return;
            }

            DateTime utcTime = GetNetworkTime();
            if (utcTime == DateTime.MinValue)
            {
                MessageBox.Show("Nu s-a putut obține ora de la serverul NTP.");
                return;
            }

            int offset = ParseTimeZoneOffset(timeZoneInput);
            DateTime localTime = utcTime.AddHours(offset);
            TimeDisplayTextBlock.Text = $"Ora exactă pentru {timeZoneInput} este: {localTime}";
        }

        private bool IsValidTimeZoneFormat(string timeZone)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(timeZone, @"^GMT[+-]\d{1,2}$");
        }

        private int ParseTimeZoneOffset(string timeZone)
        {
            string offsetString = timeZone.Substring(3);
            int offset = int.Parse(offsetString);
            return offset;
        }

        private DateTime GetNetworkTime()
        {
            const string ntpServer = "time.windows.com";
            byte[] ntpData = new byte[48];
            ntpData[0] = 0x1B;

            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(ntpServer);
                IPEndPoint ipEndPoint = new IPEndPoint(addresses[0], 123);

                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp))
                {
                    socket.Connect(ipEndPoint);
                    socket.ReceiveTimeout = 3000;
                    socket.Send(ntpData);
                    socket.Receive(ntpData);
                }

                ulong intPart = BitConverter.ToUInt32(ntpData, 40);
                ulong fracPart = BitConverter.ToUInt32(ntpData, 44);
                intPart = SwapEndianness(intPart);
                fracPart = SwapEndianness(fracPart);

                ulong milliseconds = (intPart * 1000) + ((fracPart * 1000) / 0x100000000L);
                DateTime networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long)milliseconds);

                return networkDateTime;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        private static uint SwapEndianness(ulong x)
        {
            return (uint)(((x & 0x000000ff) << 24) + ((x & 0x0000ff00) << 8) +
                          ((x & 0x00ff0000) >> 8) + ((x & 0xff000000) >> 24));
        }
    }
}