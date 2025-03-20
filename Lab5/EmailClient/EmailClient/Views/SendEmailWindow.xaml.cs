using Microsoft.Win32;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EmailClient.Views
{
    /// <summary>
    /// Interaction logic for SendEmailWindow.xaml
    /// </summary>
    public partial class SendEmailWindow : Window
    {
        public string SenderName { get; private set; }
        public string RecipientEmail { get; private set; }
        public string EmailSubject { get; private set; }
        public string EmailBody { get; private set; }
        public ObservableCollection<string> Attachments { get; private set; }


        public SendEmailWindow()
        {
            InitializeComponent();
            Attachments = new ObservableCollection<string>();
            AttachmentsListBox.ItemsSource = Attachments;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SenderName = SenderTextBox.Text;
            RecipientEmail = RecipientTextBox.Text;
            EmailSubject = SubjectTextBox.Text;
            EmailBody = BodyTextBox.Text;

            DialogResult = true;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (var filename in openFileDialog.FileNames)
                {
                    Attachments.Add(filename);
                }
            }
        }
    }
}
