﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UDPChat.Models;

namespace UDPChat.Views
{
    /// <summary>
    /// Interaction logic for ChatView.xaml
    /// </summary>
    public partial class ChatView : UserControl, INotifyPropertyChanged
    {
        public string NewMessage { get; set; }
        public Friend Friend { get; set; }

        public ICommand SendMessageCommand { get; set; }

        public void UpdateUI()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                MessagesLV.Items.Refresh();
            });
        }

        public ChatView()
        {
            InitializeComponent();
            this.DataContext = this;
        }
    }
}
