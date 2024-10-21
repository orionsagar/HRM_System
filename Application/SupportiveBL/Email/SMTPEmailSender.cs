using Domains.ViewModels;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Application.SupportiveBL.Email
{
    public class SMTPEmailSender : IEmailSender
    {
        private readonly IOptions<SMTPOptions> smtp;

        public SMTPEmailSender(IOptions<SMTPOptions> smtp)
        {
            this.smtp = smtp;
        }

        public async Task<bool> SendEmailUsingFile(string filePath, string subject, Dictionary<string, string> keyValuePairs, List<string> tos)
        {
            using StreamReader stream = new(filePath, Encoding.UTF8);
            StringBuilder contents = new (await stream.ReadToEndAsync());
            if(keyValuePairs != null)
            {
                foreach (var kv in keyValuePairs)
                {
                    contents.Replace($"{{{{{kv.Key}}}}}", kv.Value);
                }
            }            
            await SendEmail(contents.ToString(), subject, tos);
            return false;
        }       

        private async Task SendEmail(string body, string subject, List<string> tos)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtp.Value.Name, smtp.Value.Email));
            tos.ForEach(t => message.To.Add(new MailboxAddress("", t)));
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtp.Value.Host, smtp.Value.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtp.Value.Email, smtp.Value.Password);
            // send email here
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
        public async Task<bool> SendEmailUsingFile(string filePath, string subject, Dictionary<string, string> keyValuePairs, List<DomainEventVM> domainEvent)
        {
            using StreamReader stream = new(filePath, Encoding.UTF8);
            StringBuilder contents = new(await stream.ReadToEndAsync());
            if (keyValuePairs != null)
            {
                foreach (var kv in keyValuePairs)
                {
                    contents.Replace($"{{{{{kv.Key}}}}}", kv.Value);
                }
            }
            await SendEmail(contents.ToString(), subject, domainEvent);
            return false;
        }
        private async Task SendEmail(string body, string subject, List<DomainEventVM> domainEvent)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtp.Value.Name, smtp.Value.Email));
            if(domainEvent != null)
            {
                foreach (var de in domainEvent)
                {
                    if(de.Email != null)
                    {
                        message.To.Add(new MailboxAddress("", de.Email));
                    }
                    
                }
                //domainEvent.ForEach(t => message.To.Add(new MailboxAddress("", t.Email)));
            }            
            message.Subject = subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = body;
            message.Body = builder.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(smtp.Value.Host, smtp.Value.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(smtp.Value.Email, smtp.Value.Password);
            // send email here
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

    }
}
