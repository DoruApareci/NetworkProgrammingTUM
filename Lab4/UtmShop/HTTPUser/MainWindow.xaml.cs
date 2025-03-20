using HTTPUser.Models;
using HTTPUser.ViewModels;
using System.Reflection;
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

namespace HTTPUser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainViewModel ViewModel => DataContext as MainViewModel;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainViewModel();
        }

        private async void UpdateCategory_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedCategory != null)
            {
                await ViewModel.ShowAddOrUpdateCategoryWindow(ViewModel.SelectedCategory);
            }
            else
            {
                MessageBox.Show("Please select a category to update.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void DeleteCategory_Click(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedCategory != null)
            {
                await ViewModel.DeleteCategoryAsync(ViewModel.SelectedCategory.Id);
            }
            else
            {
                MessageBox.Show("Please select a category to delete.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void AddCategory_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.ShowAddOrUpdateCategoryWindow();
        }

        private async void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.ShowAddOrUpdateProductWindow();
        }
    }
}