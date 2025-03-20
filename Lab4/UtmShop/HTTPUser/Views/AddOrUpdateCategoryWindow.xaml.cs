using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace HTTPUser.Views
{
    /// <summary>
    /// Interaction logic for AddOrUpdateCategoryWindow.xaml
    /// </summary>
    public partial class AddOrUpdateCategoryWindow : Window, INotifyPropertyChanged
    {
        public AddOrUpdateCategoryWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public string CatTitle { get; set; }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
