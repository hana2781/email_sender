using ConsoleApp1.mailkit;
using ConsoleApp1.Model;
using MimeKit;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.utils
{
    internal class TaskScheduller
    {
        private readonly List<Email> _emailQueue = new List<Email>();
        private readonly SendEmail _sendEmailService;
        private readonly TimeSpan _checkInterval = TimeSpan.FromSeconds(30);

        public TaskScheduller(SendEmail sendEmailService)
        {
            _sendEmailService = sendEmailService;
        }

        public void ScheduleEmail(Email email)
        {
            _emailQueue.Add(email);
        }

        public async Task StartScheduling(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                var now = DateTime.Now;
                var emailsToSend = _emailQueue.Where(email => email.ScheduledSendTime <= now).ToList();

                foreach (var email in emailsToSend)
                {
                    try
                    {
                        var mailMessage = CreateMailMessage(email);
                        _sendEmailService.SendEmailto(mailMessage);
                        _emailQueue.Remove(email);
                    }
                    catch (Exception ex)
                    {
                        // Handle exceptions
                        AnsiConsole.WriteException(ex);
                    }
                }

                try
                {
                    await Task.Delay(_checkInterval, cancellationToken);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
            }
        }

        private MimeMessage CreateMailMessage(Email email)
        {
            return _sendEmailService.CreateMailMessage(
                email.FromEmail,
                string.Join(",", email.ToEmails),
                email.FromName,
                email.ToName,
                email.Subject,
                email.Msg,
                email.AttachmentPaths,
                email.Cc
            );
        }
    }
}
