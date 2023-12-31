﻿using Application.Common.Email.Interfaces;
using Infrastructure.Emails;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;


namespace Infrastructure.Services.Email
{
    internal class EmailSenderService : IEmailSender
    {
        private readonly EmailSettings _mailSettings;
        public EmailSenderService(IOptions<EmailSettings> options)
        {
            _mailSettings = options.Value;
        }

        public async Task SendAsync(string to, string subject, string body, IEnumerable<IFormFile> attachments = null)
        {
            var mimeMessage = await GetMimeMessage(to, subject, body, attachments);

            await SendAsync(mimeMessage);
        }

        private async Task<MimeMessage> GetMimeMessage(string to, string subject, string body, IEnumerable<IFormFile> attachments)
        {
            var email = new MimeMessage
            {
                Sender = MailboxAddress.Parse(_mailSettings.UserName),
                Subject = subject
            };

            email.To.Add(MailboxAddress.Parse(to));
            email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.UserName));

            var bodyBuilder = new BodyBuilder();

            bodyBuilder.HtmlBody = body;

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    using var ms = new MemoryStream();
                    await attachment.CopyToAsync(ms);
                    var attachmentBytes = ms.ToArray();

                    bodyBuilder.Attachments.Add(attachment.FileName, attachmentBytes, ContentType.Parse(attachment.ContentType));
                }
            }

            email.Body = bodyBuilder.ToMessageBody();

            return email;
        }

        private async Task SendAsync(MimeMessage email)
        {
            using var smtpClient = new SmtpClient();
            await smtpClient.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
            await smtpClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
            await smtpClient.SendAsync(email);
            //await smtpClient.DisconnectAsync(true);
        }
    }
}
