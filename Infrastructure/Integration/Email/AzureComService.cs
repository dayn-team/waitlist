using Azure;
using Azure.Communication.Email;
using Core.Application.Interfaces.Email;
using Core.Domain.DTOs.Configurations;
using Core.Domain.DTOs.Others;
using Core.Shared;
using Microsoft.Extensions.Options;
using NetCore.AutoRegisterDi;

namespace Infrastructure.Integration.Email {
    [RegisterAsSingleton]
    public class AzureComService : IEmailService {
        private EmailClient _client;
        private readonly EmailParam _emailconfig;
        public AzureComService(IOptionsMonitor<SystemVariables> config) {
            _emailconfig = config.CurrentValue.EmailParam;
            string connectionString = string.Concat("endpoint=", _emailconfig.smtpServer, ";accesskey=", _emailconfig.password);
            _client = new EmailClient(connectionString);
        }
        public async Task<bool> send(MailEnvelope envelope) {
            EmailContent emailContent = new EmailContent(envelope.subject);
            if (envelope.bodyIsPlainText) {
                emailContent.PlainText = envelope.body;
            } else {
                emailContent.Html = envelope.body;
            }
            List<EmailAddress> emailAddresses = new List<EmailAddress>();
            for (int i = 0; i < envelope.toAddress.Length; i++) {
                try {
                    EmailAddress to = new EmailAddress(envelope.toAddress[i].Trim(), envelope.toName[i]);
                    emailAddresses.Add(to);
                } catch {
                    EmailAddress to = new EmailAddress(envelope.toAddress[i].Trim());
                    emailAddresses.Add(to);
                }
            }
            EmailRecipients emailRecipients = new EmailRecipients(emailAddresses);
            EmailMessage emailMessage = new EmailMessage(_emailconfig.fromAddress, emailRecipients, emailContent);
            addAttachments(emailMessage, envelope);
            await _client.SendAsync(WaitUntil.Started, emailMessage);
            return true;
        }

        private void addAttachments(EmailMessage emailMessage, MailEnvelope env) {
            if (env.attachmentObj == null)
                return;
            foreach (Attachment attachment in env.attachmentObj) {
                var t = getAttachment(attachment);
                if (t is null)
                    continue;
                emailMessage.Attachments.Add(t);
            }
        }

        private EmailAttachment? getAttachment(Attachment attachment) {
            if (!string.IsNullOrEmpty(attachment.fileUrl)) {
                attachment.rawAttachment = File.ReadAllBytes(attachment.fileUrl);
            }
            if (attachment.rawAttachment is null)
                return null;
            var contentBinaryData = new BinaryData(attachment.rawAttachment);
            attachment.fileName = string.IsNullOrEmpty(attachment.fileName) ? $"{Cryptography.CharGenerator.genID()}.{attachment.format}" : attachment.fileName;
            return new EmailAttachment(attachment.fileName, attachment.contentType, contentBinaryData);
        }

    }
}
