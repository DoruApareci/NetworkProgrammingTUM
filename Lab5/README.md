# Network Programming Lab 5 - Email Client Application

This laboratory assignment involves creating an email client application capable of sending and receiving emails through Gmail. The solution is implemented as a Visual Studio solution with a single subproject:

- **EmailClient:**  
  Contains all the necessary functionality for sending and receiving emails.

The project is developed using **.Net**, **WPF**, **MVVM**, and **FodyWeavers**. It leverages the following libraries:
- **MailKit**
- **MimeKit**

## Project Overview

### Models

- **ConfigLoader:**  
  Loads the `appconfig.json` file containing the SMTP and IMAP settings for Gmail, as well as the app password generated from [Google App Passwords](https://myaccount.google.com/apppasswords).

- **EmailSettings:**  
  Represents the application settings related to email configuration.

- **RelayCommand:**  
  Implements the command pattern for handling UI commands.

### Services

- **EmailService:**  
  Provides the necessary functionality for sending and receiving emails through Gmail. This service encapsulates all interactions with the Gmail server.

### ViewModels

- **EmailViewModel:**  
  Represents the state of the application for the UI. It creates an instance of `EmailService` to facilitate communication with Gmail for sending and receiving emails.

### Views

- **SendEmailWindow:**  
  The view used for composing and sending new emails. This window allows users to enter the recipient, subject, reply-to details, and message content. It also supports attaching files when sending an email.

## Assignment Description

Develop an email client application that is capable of:
- **Receiving Emails:**
  - Displaying the list of emails in the inbox using the POP3 protocol.
  - Displaying the list of emails in the inbox using the IMAP protocol.
  - Downloading emails with attachments.
- **Sending Emails:**
  - Sending a plain text email.
  - Sending an email with an attachment.
  - Specifying the email subject and reply-to details during the sending process.

## Evaluation Criteria

- The application can display the list of emails from the inbox using both POP3 and IMAP protocols.
- The application is capable of downloading an email with attachments.
- The application can send a plain text email.
- The application can send an email with an attachment.
- The application allows the user to specify the email subject and reply-to details when sending an email.

## How to Use

Clone, build, and run the project from the repository. Once the application is running, use the provided UI to interact with Gmail:
- **To Send an Email:**  
  Use the **SendEmailWindow** to compose and send a new email. Fill in the recipient, subject, reply-to details, and message body. Optionally, attach files before sending.
  
- **To Receive Emails:**  
  The application will display the list of emails retrieved from Gmail using the POP3 or IMAP protocol. You can view the details of each email, including those with attachments.

## Screenshots

### Email with Attachments
<p align="center">
  <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab5/Images/EmailClient1.png" alt="Email with Attachments"/>
</p>

### Email with HTML Content
<p align="center">
  <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab5/Images/EmailClient2.png" alt="Email with HTML Content"/>
</p>

### Send New Email UI
<p align="center">
  <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab5/Images/SendEmail.png" alt="Send New Email UI"/>
</p>

### Received Email inside App
<p align="center">
  <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab5/Images/ReceivedEmail.png" alt="Received Email inside App"/>
</p>

### Received Email in Gmail
<p align="center">
  <img src="https://github.com/DoruApareci/NetworkProgrammingTUM/blob/main/Lab5/Images/ReceivedEmail_1.png" alt="Received Email in Gmail"/>
</p>

## Contributing

Feel free to star the repository if you find it useful. Pull requests and suggestions are welcome to help improve the project.
