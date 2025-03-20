using EmailClient.Models;
using MailKit.Net.Imap;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MailKit.Security;

namespace EmailClient.Services
{
    class EmailService
    {
        private readonly EmailSettings _emailSettings;

        public EmailService()
        {
            _emailSettings = ConfigLoader.LoadConfig("appconfig.json");
        }

        public async Task<IList<MimeMessage>> LoadEmailsAsync(int page, int count)
        {
            var emails = new List<MimeMessage>();

            using (var client = new ImapClient())
            {
                await client.ConnectAsync(_emailSettings.ImapServer, _emailSettings.ImapPort, SecureSocketOptions.SslOnConnect);
                await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);

                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadOnly);

                int totalMessages = inbox.Count;
                int startIndex = totalMessages - ((page - 1) * count) - 1;
                int endIndex = Math.Max(startIndex - count + 1, 0);

                for (int i = startIndex; i >= endIndex; i--)
                {
                    var message = await inbox.GetMessageAsync(i);
                    emails.Add(message);
                }

                await client.DisconnectAsync(true);
            }

            return emails;
        }

        public async Task SendEmailAsync(string senderName, string recipientEmail, string subject, string body, string replyTo = null, IEnumerable<string> attachmentPaths = null)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(senderName, _emailSettings.Email));
            message.To.Add(new MailboxAddress("Recipient Name", recipientEmail));
            message.Subject = subject;

            var bodyPart = new TextPart("plain") { Text = body };

            if (replyTo != null)
            {
                message.ReplyTo.Add(new MailboxAddress("Reply To", replyTo));
            }

            var multipart = new Multipart("mixed");
            multipart.Add(bodyPart);

            if (attachmentPaths != null)
            {
                foreach (var attachmentPath in attachmentPaths)
                {
                    var attachment = new MimePart("application", "octet-stream")
                    {
                        Content = new MimeContent(System.IO.File.OpenRead(attachmentPath)),
                        ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                        ContentTransferEncoding = ContentEncoding.Base64,
                        FileName = System.IO.Path.GetFileName(attachmentPath)
                    };
                    multipart.Add(attachment);
                }
            }

            message.Body = multipart;

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.Email, _emailSettings.Password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }
    }
}
