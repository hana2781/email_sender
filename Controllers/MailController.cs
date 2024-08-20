using ConsoleApp1.Model;
using ConsoleApp1.utils;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApp1.Controllers
{
    internal class MailController
    {
        private readonly Utilities _util;
        public MailController(Utilities utilities) 
        {
            _util = utilities;
        }

        public Email collectData() 
        {
            Email email = new Email();

            email.Msg = _util.Prompt("Insert your message here: ");
            email.FromEmail = CollectValidEmail("Insert sender email address: ");

            email.ToEmails = CollectValidEmails("Insert recipient email addresses (comma-separated): ");

            if (_util.confirmation("use Cc (N/y)", "n", "y").Equals("y"))
            {
                email.Cc = CollectValidEmails("Insert Cc recipient email addresses (comma-separated): ");
            }
            email.FromName = _util.Prompt("Insert sender name: ");
            email.ToName = _util.Prompt("Insert target name: ");
            email.Subject = _util.Prompt("Email subject: ");

            email.AttachmentPaths = CollectAttachmentPaths();
            email.ScheduledSendTime = CollectScheduledSendTime();


            return email;
        }

        public Email editEMailData(Email emailData)
        {
            string editAgain = "n";
            do
            {
                AnsiConsole.MarkupLine("Edit [cyan]Email[/] details!");

                emailData.Msg = updateValue(emailData.Msg, "messages");

                emailData.FromEmail = updateValue(emailData.FromEmail, "sender email address");
                
                if (!_util.isValidEmail(emailData.FromEmail))
                {
                    AnsiConsole.MarkupLine("Error [red] invalid email[/]");
                    continue;
                }

                emailData.ToEmails = updateValue(emailData.ToEmails, "recipient email addresses (comma-separated)");

                if (!validateEmailList(emailData.ToEmails))
                {
                    AnsiConsole.MarkupLine("Error [red] invalid email[/]");
                    continue;
                }
         

                emailData.FromName = updateValue(emailData.FromName, "sender name");

                emailData.ToName = updateValue(emailData.ToName, "recipient names");

                emailData.Subject = updateValue(emailData.Subject, "email subject");

                editAgain = _util.confirmation("Do you want to edit the data (y/N): ", "n", "y").ToLower();

            } while (editAgain.Equals("y"));    
            return emailData;
        }

        private string CollectValidEmail(string prompt)
        {
            var email = _util.Prompt(prompt);
            if (!_util.isValidEmail(email))
            {
                throw new FormatException($"Invalid email format: {email}");
            }
            return email;
        }

        private string CollectValidEmails(string prompt)
        {
            var emails = _util.Prompt(prompt).Split(',').Select(email => email.Trim()).ToList();
            foreach (var email in emails)
            {
                if (!_util.isValidEmail(email))
                {
                    throw new FormatException($"Invalid email format: {email}");
                }
            }
            return string.Join(",", emails);
        }

        private List<string> CollectAttachmentPaths()
        {
            var attachmentPaths = new List<string>();
            if (_util.confirmation("Do you want to use attachment (y/N)?", "n", "y").Equals("y"))
            {
                string attachmentPath;
                do
                {
                    attachmentPath = _util.Prompt("Enter attachment file path (or leave blank to finish): ");
                    if (!string.IsNullOrEmpty(attachmentPath))
                    {
                        if (File.Exists(attachmentPath))
                        {
                            attachmentPaths.Add(attachmentPath);
                        }
                        else
                        {
                            AnsiConsole.MarkupLine("[red]Error:[/] File not found: {0}", attachmentPath);
                        }
                    }
                } while (!string.IsNullOrEmpty(attachmentPath));
            }
            return attachmentPaths;
        }

        private string updateValue(string oldValue, string description)
        {
            _util.OutValue($"Current {description} : {oldValue}");
            var newValue = _util.Prompt($"new {description} (leave blank for default): ");
            return string.IsNullOrWhiteSpace(newValue) ? oldValue : newValue;
        }

        private bool validateEmailList(string emailList)
        {
            var emails = emailList.Split(',').Select(email => email.Trim()).ToList();
            foreach (var email in emails)
            {
                if (!_util.isValidEmail(email))
                {
                    return false;
                }
            }
            return true;
        }

        private DateTime? CollectScheduledSendTime()
        {
            if (_util.confirmation("Do you want to schedule this email (N/y)?", "n", "y").Equals("y"))
            {
                while (true)
                {
                    var dateTimeString = _util.Prompt("Enter the scheduled send date and time (YYYY-MM-DD HH:MM): ");
                    if (DateTime.TryParse(dateTimeString, out var scheduledTime))
                    {
                        return scheduledTime;
                    }
                    AnsiConsole.MarkupLine("[red]Error:[/] Invalid date/time format.");
                }
            }
            return null;
        }

    }

}
