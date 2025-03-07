﻿using Core.Application.Interfaces.Email;
using Core.Domain.DTOs.Configurations;
using Core.Domain.DTOs.Others;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using NetCore.AutoRegisterDi;

namespace Infrastructure.Integration.Email {
    [DoNotAutoRegister]
    public class MailkitMailer : IEmailService {
        private readonly EmailParam _param;
        public MailkitMailer(IOptionsMonitor<SystemVariables> config) {
            _param = config.CurrentValue.EmailParam;
        }
        public async Task<bool> send(MailEnvelope envelope) {
            try {
                MimeMessage message = new MimeMessage();
                MailboxAddress from = new MailboxAddress(_param.fromName,
                _param.fromAddress);
                message.From.Add(from);
                for (int i = 0; i < envelope.toAddress.Length; i++) {
                    try {
                        MailboxAddress to = new MailboxAddress(envelope.toName[i],
                    envelope.toAddress[i].Trim());
                        message.To.Add(to);
                    } catch {
                        MailboxAddress to = new MailboxAddress("User",
                    envelope.toAddress[i].Trim());
                        message.To.Add(to);
                    }
                }
                message.Subject = envelope.subject;
                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = envelope.body;
                if (envelope.attachment != null) {
                    for (int i = 0; i < envelope.attachment.Length; i++) {
                        bodyBuilder.Attachments.Add(envelope.attachment[i]);
                    }
                }
                message.Body = bodyBuilder.ToMessageBody();
                SmtpClient client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(_param.smtpServer, _param.smtpPort, SecureSocketOptions.Auto);
                if (_param.smtpPort == 465) {
                    await client.AuthenticateAsync(_param.username, _param.password);
                }
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                client.Dispose();
                return true;
            } catch (Exception e) {
                throw e;
            }
        }
    }
}
