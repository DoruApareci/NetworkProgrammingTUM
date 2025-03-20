using EmailClient.Services;
using MimeKit;
using EmailClient.Models;
using System;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Runtime.CompilerServices;
using EmailClient.Views;

namespace EmailClient.ViewModels
{
    public class EmailViewModel : INotifyPropertyChanged
    {
        private readonly EmailService _emailService;
        private MimeMessage _selectedEmail;
        private int _currentPage;
        private const int EmailsPerPage = 25;

        public ObservableCollection<MimeMessage> Emails { get; }
        public ObservableCollection<MimeEntity> Attachments { get; }
        public ICommand LoadEmailsCommand { get; }
        public ICommand NextPageCommand { get; }
        public ICommand PreviousPageCommand { get; }
        public ICommand OpenSendEmailWindowCommand { get; }

        public MimeMessage SelectedEmail
        {
            get => _selectedEmail;
            set
            {
                _selectedEmail = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(EmailBody));
                OnPropertyChanged(nameof(EmailBodyHtml));
                UpdateAttachments();
            }
        }

        public string EmailBody => SelectedEmail?.TextBody;
        public string EmailBodyHtml => SelectedEmail?.HtmlBody ?? SelectedEmail?.TextBody;

        public event PropertyChangedEventHandler PropertyChanged;

        public EmailViewModel()
        {
            _emailService = new EmailService();
            Emails = new ObservableCollection<MimeMessage>();
            Attachments = new ObservableCollection<MimeEntity>();
            LoadEmailsCommand = new RelayCommand(async () => await LoadEmailsAsync(_currentPage));
            NextPageCommand = new RelayCommand(async () => await LoadEmailsAsync(_currentPage + 1));
            PreviousPageCommand = new RelayCommand(async () => await LoadEmailsAsync(_currentPage - 1));
            OpenSendEmailWindowCommand = new RelayCommand(OpenSendEmailWindow);
            _currentPage = 1;
        }

        private async Task LoadEmailsAsync(int page)
        {
            if (page < 1) return;

            var emails = await _emailService.LoadEmailsAsync(page, EmailsPerPage);
            if (emails.Count == 0 && page > 1) return; // No more emails to load

            _currentPage = page;
            Emails.Clear();
            foreach (var email in emails)
            {
                Emails.Add(email);
            }
        }

        private void UpdateAttachments()
        {
            Attachments.Clear();
            if (SelectedEmail != null)
            {
                var attachments = SelectedEmail.Attachments;
                foreach (var attachment in attachments)
                {
                    Attachments.Add(attachment);
                }
            }
        }

        private async Task OpenSendEmailWindow()
        {
            var sendEmailWindow = new SendEmailWindow();
            if (sendEmailWindow.ShowDialog() == true)
            {
                var senderName = sendEmailWindow.SenderName;
                var recipientEmail = sendEmailWindow.RecipientEmail;
                var subject = sendEmailWindow.EmailSubject;
                var body = sendEmailWindow.EmailBody;
                var attachments = sendEmailWindow.Attachments;
                _emailService.SendEmailAsync(senderName, recipientEmail, subject, body, null, attachments);
            }
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
