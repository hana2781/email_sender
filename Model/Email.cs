using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Model
{
    internal class Email
    {
        public string Msg { get; set; }
        public string FromEmail { get; set; }
        public string ToEmails { get; set; }
        public string FromName { get; set; }
        public string ToName { get; set; }
        public string? Cc { get; set; }
        public string Subject { get; set; }
        public List<string> AttachmentPaths { get; set; }
        public string Template { get; set; }
        public DateTime? ScheduledSendTime { get; set; }
        public Email() { }

        public Email(string msg, string fromEmail, string toEmail, string fromName, string toName, string subject, List<string> attachmentPaths, string template, string? cc, DateTime? scheduledSendTime = null)
        {
            Msg = msg;
            FromEmail = fromEmail;
            ToEmails = toEmail;
            FromName = fromName;
            ToName = toName;
            Subject = subject;
            AttachmentPaths = attachmentPaths;
            Template = template;
            Cc = cc;
            ScheduledSendTime = scheduledSendTime;
        }

        public Email(string msg, string fromEmail, string toEmail, string fromName, string toName, string subject, List<string> attachmentPaths, string? cc)
        {
            Msg = msg;
            FromEmail = fromEmail;
            ToEmails = toEmail;
            FromName = fromName;
            ToName = toName;
            Subject = subject;
            AttachmentPaths = attachmentPaths;
            Cc = cc;
        }
    }
}
