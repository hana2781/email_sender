# Email Sender Console Application

## Overview

The **Email Sender Console Application** is a C# project designed to explore and learn about C# programming and .NET technologies. This console-based application allows users to send emails with features such as scheduling and attachments.

## Purpose

The **Email Sender Console Application** was inspired by a sophisticated email sender system I encountered while working with a banking web application in Indonesia. Although the system I observed was built using Java, I was fascinated by its ability to handle critical communications, such as transaction alerts and important notifications.

Driven by a desire to deepen my understanding of C# and .NET technologies, I decided to create a similar application using C#. This project represents my personal journey to explore and master C#, while implementing features that I admired in the Java-based system I worked with.

By developing this console-based email sender application, I aimed to translate the concepts and functionalities of the original system into C#, thereby gaining hands-on experience with email handling, scheduling, and HTML templating in a .NET environment.
## Lessons Learned

Building this project provided valuable insights into handling email operations and scheduling tasks programmatically. I gained practical experience with the MailKit library for sending emails, as well as implementing scheduling functionality to manage email delivery times effectively.

A significant lesson learned was the importance of **separation of concerns** in application design. By organizing the code into distinct components for email handling, scheduling, and user interaction, I found it easier to maintain and extend the application. This approach not only improved the code’s readability but also made debugging and enhancing specific features more manageable.

Additionally, I learned how to **divide large problems into smaller tasks**. By breaking down complex requirements into manageable pieces, I was able to tackle each part systematically and implement solutions more effectively. This practice also extended to my code structure, where I applied modular design principles to ensure each component had a clear, focused responsibility.

The project presented challenges such as integrating the scheduling feature and ensuring reliable email delivery. Overcoming these challenges required careful research, testing, and a deeper understanding of both C# and the libraries used. Each obstacle was an opportunity to refine my skills and apply best practices in software development.

## Authors

- [hana2781](https://www.github.com/hana2781)


## Technologies Used

- [**C# and .NET Core:**](https://dotnet.microsoft.com/en-us/) The primary programming language and framework used for building the application.
- [**MailKit:**](https://github.com/jstedfast/MailKit) A cross-platform mail client library for .NET, used for composing and sending emails.
- [**Spectre.Console:**](https://spectreconsole.net/) A library for creating rich and interactive console applications with styled output.
- **Configuration Management:** Uses `appsettings.json` for managing SMTP server settings and other configurations.

## Features

- **Send Emails:** Send emails with support for HTML content, attachments, and CC recipients.
- **Schedule Emails:** Schedule emails to be sent at a later time.
- **Template-Based:** Use HTML templates to format email content.

## Run Locally

To get started with the Email Sender Console Application, follow these steps:

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/yourusername/email-sender-console.git

2. **Change appsettings.json**
   ```
   {
       "MailSettings": {
           "SmtpServer": "smtp.gmail.com",
           "SmtpPort": 587,
           "Username": "your@mail.com",
           "Password": "Password"
       }
   }
   ```
3. **Run Application**
   ```bash 
   dotnet run
   ```

    
