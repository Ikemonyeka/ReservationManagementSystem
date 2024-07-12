using MailKit.Security;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using ReservationManagementSystem.Application.Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReservationManagementSystem.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task AdminVerifcationEmail(string email, string token)
        {
            try
            {
                var Hosturl = _configuration.GetSection("EmailDetails:EmailLocalHost").Value;
                var Curl = _configuration.GetSection("EmailDetails:AdminEmailConfirmation").Value;
                var Template = _configuration.GetSection("EmailDetails:EmailVerification").Value;

                if (Template == null)
                {
                    return;
                }

                string HtmlBody = "";
                StreamReader reader = new StreamReader(Template);
                HtmlBody = reader.ReadToEnd();
                HtmlBody = HtmlBody.Replace("{Link}", string.Format(Hosturl + Curl, email, token));

                var mail = new MimeMessage();
                mail.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailDetails:DefaultEmail").Value));
                mail.To.Add(MailboxAddress.Parse(email));
                mail.Subject = "Email Verification";
                mail.Body = new TextPart(TextFormat.Html) { Text = HtmlBody };

                using var smtp = new SmtpClient();
                smtp.Connect(_configuration.GetSection("EmailDetails:EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration.GetSection("EmailDetails:EmailUsername").Value, _configuration.GetSection("EmailDetails:EmailPassword").Value);
                smtp.Send(mail);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                return;
            }
        }

        public async Task UserVerifcationEmail(string email, string token)
        {
            try
            {
                var Hosturl = _configuration.GetSection("EmailDetails:EmailLocalHost").Value;
                var Curl = _configuration.GetSection("EmailDetails:UserEmailConfirmation").Value;
                var Template = _configuration.GetSection("EmailDetails:EmailVerification").Value;

                if (Template == null)
                {
                    return ;
                }

                string HtmlBody = "";
                StreamReader reader = new StreamReader(Template);
                HtmlBody = reader.ReadToEnd();
                HtmlBody = HtmlBody.Replace("{Link}", string.Format(Hosturl + Curl, email, token));

                var mail = new MimeMessage();
                mail.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailDetails:DefaultEmail").Value));
                mail.To.Add(MailboxAddress.Parse(email));
                mail.Subject = "Email Verification";
                mail.Body = new TextPart(TextFormat.Html) { Text = HtmlBody };

                using var smtp = new SmtpClient();
                smtp.Connect(_configuration.GetSection("EmailDetails:EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration.GetSection("EmailDetails:EmailUsername").Value, _configuration.GetSection("EmailDetails:EmailPassword").Value);
                smtp.Send(mail);
                smtp.Disconnect(true);
            }
            catch (Exception ex) 
            {
                return;
            }
        }
    }
}
