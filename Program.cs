// See https://aka.ms/new-console-template for more information
using ConsoleApp1.Controllers;
using ConsoleApp1.mailkit;
using ConsoleApp1.Model;
using ConsoleApp1.utils;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Spectre.Console;
using System.Data;
using System.Net.Mail;
using System.Xml.Linq;

#region load config file
var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
#endregion

#region initaliaze dependencies class
var mailSettings = configuration.GetSection("MailSettings");
string smtpServer = mailSettings["SmtpServer"];
int smtpPort = int.Parse(mailSettings["SmtpPort"]);
string username = mailSettings["Username"];
string password = mailSettings["Password"];

var emailSender = new SendEmail(smtpServer, smtpPort, username, password);
var util = new Utilities();
var mailController = new MailController(util);
var emailScheduler = new TaskScheduller(emailSender);
#endregion

String sendAnotherEmail = "y";
using (var cts = new CancellationTokenSource())
{
    Task schedulingTask = emailScheduler.StartScheduling(cts.Token);
    #region greeting user
    util.figletText("Email Sender");
    AnsiConsole.MarkupLine("Welcome to the [cyan]Email Sender[/] application!");
    #endregion

    #region main logic sender email
    do
    {
        try
        {
            #region user input
            Email collectData = mailController.collectData();

            string template = util.templates();

            string htmlTemplatePath = $"template/{template}.html";
            string htmlTemplate = File.ReadAllText(htmlTemplatePath);
            util.DisplaySummary(collectData.FromEmail, collectData.ToEmails, collectData.Subject, collectData.AttachmentPaths, template);
            #endregion

            #region edit input
            string htmlBody;
            string isEdit = util.confirmation("Do you want to edit the data (y/N): ", "n", "y").ToLower();

            if (isEdit.Equals("y"))
            {
                collectData = mailController.editEMailData(collectData);
                util.DisplaySummary(collectData.FromEmail, collectData.ToEmails, collectData.Subject, collectData.AttachmentPaths, template);
            }
            #endregion

            #region load html
            htmlBody = htmlTemplate
                        .Replace("{name}", collectData.ToName)
                        .Replace("{msg}", collectData.Msg)
                        .Replace("{senderName}", collectData.FromName);
            #endregion

            #region send email
            //if (util.confirmation("Do you want to send this email now (y) or schedule it (S)? ", "y", "s").Equals("s"))
            if(collectData.ScheduledSendTime.HasValue)
            {
                // Schedule email
                collectData.Msg = htmlBody;  // Set the HTML body in the email object
                emailScheduler.ScheduleEmail(collectData);
                AnsiConsole.MarkupLine("[green]Success:[/] Email scheduled.");
            } 
            else if (util.confirmation("Do you want to send this email (y/N): ", "n", "y").Equals("y"))
            {
                var mailMessage = emailSender.CreateMailMessage(
                    collectData.FromEmail, collectData.ToEmails,
                    collectData.FromName, collectData.ToName, 
                    collectData.Subject, htmlBody, collectData.AttachmentPaths, 
                    collectData.Cc);
                try
                {
                    util.sendEmailProgress(emailSender, mailMessage);
                }
                catch (Exception ex)
                {
                    AnsiConsole.MarkupLine("[red]Failed:[/] Failed to send email: {0}", ex.Message);
                    AnsiConsole.WriteException(ex);
                }
            }
            else
            {
                AnsiConsole.MarkupLine("[yellow]Success:[/] Email sent cancelled");
            }
            #endregion

            sendAnotherEmail = util.confirmation("Do you want send another Email (Y/n): ", "y", "n").ToLower();
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine("[red]Failed:[/] Failed to send email: {0}", ex.Message);
            AnsiConsole.WriteException(ex);
            sendAnotherEmail = "n";
        }
    }
    while (sendAnotherEmail.Equals("y"));

    cts.Cancel();
    await schedulingTask;
    #endregion
}