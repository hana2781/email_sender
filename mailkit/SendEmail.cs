using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.mailkit
{   

    internal class SendEmail
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _username;
        private readonly string _password;

        public SendEmail(string smtpServer, int smtpPort, string username, string password)
        {
            _smtpServer = smtpServer ?? throw new ArgumentNullException(nameof(smtpServer));
            _smtpPort = smtpPort;
            _username = username ?? throw new ArgumentNullException(nameof(username));
            _password = password ?? throw new ArgumentNullException(nameof(password));
        }

        public MimeMessage CreateMailMessage(string fromEmail, string toEmail, 
            string fromName, string toName,
            string subject, string htmlBody, 
            IEnumerable<string> attachmentPaths = null, string? cc = null)
        {
            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress(fromName, fromEmail));

            var recipientList = toEmail.Split(',').Select(email => email.Trim());
            foreach (var recipient in recipientList)
            {
                mailMessage.To.Add(new MailboxAddress(toName, recipient));
            }
            if (cc != null)
            { 
                var ccRecipienList = cc.Split(',').Select(cc => cc.Trim());
                foreach (var ccRecipient in ccRecipienList)
                {
                    mailMessage.Cc.Add(new MailboxAddress(cc, ccRecipient));
                }
            }
            mailMessage.Subject = subject;
            //mailMessage.Body = new TextPart("html") { Text = htmlBody };
            var bodyBuilder = new BodyBuilder { HtmlBody = htmlBody };

            if (attachmentPaths != null)
            {
                foreach (var attachmentPath in attachmentPaths)
                {
                    if (File.Exists(attachmentPath))
                    {
                        bodyBuilder.Attachments.Add(attachmentPath);
                    }
                    else
                    {
                        throw new FileNotFoundException($"Attachment file not found: {attachmentPath}");
                    }
                }
            }
            mailMessage.Body = bodyBuilder.ToMessageBody();
            return mailMessage;
        }

        public void SendEmailto(MimeMessage mailMessage)
        {
            try
            {
                using (var smtpClient = new SmtpClient())
                {
                    smtpClient.Connect(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);
                    smtpClient.Authenticate(_username, _password);
                    smtpClient.Send(mailMessage);
                    smtpClient.Disconnect(true);
                }
            }
            catch (Exception ex) 
            {
                AnsiConsole.MarkupLine("[red]Error:[/] Failed to send email execption : {0}", ex);
            }
        }

    }
}
