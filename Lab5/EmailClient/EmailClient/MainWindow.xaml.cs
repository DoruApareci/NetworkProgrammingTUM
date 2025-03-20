using EmailClient.ViewModels;
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

namespace EmailClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            {
                InitializeComponent();
                this.DataContext = new EmailViewModel();
                var viewModel = DataContext as EmailViewModel;
                if (viewModel != null)
                {
                    viewModel.PropertyChanged += ViewModel_PropertyChanged;
                }
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(EmailViewModel.EmailBodyHtml))
            {
                var viewModel = sender as EmailViewModel;
                if (viewModel != null)
                {
                    EmailBodyBrowser.NavigateToString(viewModel.EmailBodyHtml ?? string.Empty);
                }
            }
        }
    }
}