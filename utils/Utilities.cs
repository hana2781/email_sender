using ConsoleApp1.mailkit;
using MimeKit;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.utils
{
    internal class Utilities
    {
        public Utilities()
        {
            
        }
        public string Prompt(string message)
        {
            Console.Write(message);
            return Console.ReadLine()?.Trim();
        }

        public void OutValue(string message)
        {
            Console.WriteLine(message);
        }

        public void figletText(string message) 
        {
            AnsiConsole.Write(
            new FigletText($"{message}")
                .LeftJustified()
                .Color(Color.Red));
        }

        public void DisplaySummary(string fromEmail, string toEmail, string subject, IEnumerable<string> attachmentPaths, string template)
        {
            var table = new Table();
            table.AddColumn(new TableColumn("[bold]Field[/]").Centered());
            table.AddColumn(new TableColumn("[bold]Value[/]").Centered());

            table.AddRow("Sender Email", fromEmail);
            table.AddRow("Receiver Email", toEmail);
            table.AddRow("Subject", subject);
            table.AddRow("Template", template);

            if (attachmentPaths == null || !attachmentPaths.Any())
            {
                table.AddRow("Attachments", "None");
            }
            else
            {
                int i = 1;
                foreach (var attachmentPath in attachmentPaths)
                {
                    var fileName = Path.GetFileName(attachmentPath);
                    table.AddRow($"Atttachment {i}", fileName);
                    i++;
                }
            }

            AnsiConsole.Write(table);
        }

        public string confirmation(string msg, string defaultOptions, string secondOptions)
        {
            string confirm = AnsiConsole.Prompt(
                new TextPrompt<string>(msg)
                    .DefaultValue(defaultOptions)
                    .Validate(input =>
                    {
                        return (input.ToLower() == defaultOptions || input.ToLower() == secondOptions)
                            ? ValidationResult.Success()
                            : ValidationResult.Error($"[red]Please enter '{defaultOptions}' or '{secondOptions}'[/]");
                    })
            ).ToLower();
            return confirm;
        }

        public string templates()
        {
            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Choose [green]template[/]?")
                    .PageSize(3)
                    .MoreChoicesText("[grey](Move up and down to reveal more options)[/]")
                    .AddChoices(new[] {
                            "msg", "ticket",
                        }
                    )
            );
            return choice;
        }

        public bool isValidEmail(string email)
        {
            try
            {
                var addr = new MailAddress(email);
                return addr.Address == email;
            }
            catch { return false; }
        }

        public void sendEmailProgress(SendEmail mail, MimeMessage mailBody)
        {
            AnsiConsole.Status()
            .Start("Processing...", ctx =>
            {
                // Update the status and spinner
                ctx.Status("Sending email");
                ctx.Spinner(Spinner.Known.Star);
                ctx.SpinnerStyle(Style.Parse("green"));

                //emailSender.SendEmailto(mailMessage);
                mail.SendEmailto(mailBody);
                AnsiConsole.MarkupLine("[green]Success:[/] Email sent successfully!");
            });
        }
    }
}
