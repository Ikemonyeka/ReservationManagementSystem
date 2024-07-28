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
using ReservationManagementSystem.Core.Objects;
using ReservationManagementSystem.Models.Entities;

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

        public async Task<ResponseViewModel> ReservationComplete(ReservationCompleteViewModel model)
        {
            try
            {
                var Template = _configuration.GetSection("EmailDetails:ReservationComplete").Value;

                if (Template == null)
                    return new ResponseViewModel { message = "", status = false, data = "No data available" };

                string HtmlBody = "";
                StreamReader reader = new StreamReader(Template);
                HtmlBody = reader.ReadToEnd();
                HtmlBody = HtmlBody.Replace("{CustomerName}", model.Name);
                HtmlBody = HtmlBody.Replace("{RestaurantName}", model.RestuarantName);
                HtmlBody = HtmlBody.Replace("{Date}", model.ReservationDate.ToString("MM/dd/yyyy"));
                HtmlBody = HtmlBody.Replace("{StartTime}", model.StartTime.ToString("HH:mm:tt"));
                HtmlBody = HtmlBody.Replace("{EndTime}", model.EndTime.ToString("HH:mm:tt"));
                HtmlBody = HtmlBody.Replace("{TableName}", model.TableType);
                HtmlBody = HtmlBody.Replace("{NumberOfPeople}", model.PartySize.ToString());
                HtmlBody = HtmlBody.Replace("{SpecialRequests}", model.SpecialRequests);

                var mail = new MimeMessage();
                mail.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailDetails:DefaultEmail").Value));
                mail.To.Add(MailboxAddress.Parse(model.Email));
                mail.Subject = "Reservation Completed";
                mail.Body = new TextPart(TextFormat.Html) { Text = HtmlBody };

                using var smtp = new SmtpClient();
                smtp.Connect(_configuration.GetSection("EmailDetails:EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration.GetSection("EmailDetails:EmailUsername").Value, _configuration.GetSection("EmailDetails:EmailPassword").Value);
                smtp.Send(mail);
                smtp.Disconnect(true);

                return new ResponseViewModel { message = "Email Sent", status = true, data = "No data available" };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel();
            }
        }

        public async Task<ResponseViewModel> ForgotPasswordEmail(string email, string token)
        {
            try
            {
                var Hosturl = _configuration.GetSection("EmailDetails:EmailLocalHost").Value;
                var Curl = _configuration.GetSection("EmailDetails:ForgotPasswordUrl").Value;
                var Template = _configuration.GetSection("EmailDetails:ForgotPasswordEmail").Value;

                if (Template == null)
                    return new ResponseViewModel { message = "", status = false, data = "No data available" };

                string HtmlBody = "";
                StreamReader reader = new StreamReader(Template);
                HtmlBody = reader.ReadToEnd();
                HtmlBody = HtmlBody.Replace("{Link}", string.Format(Hosturl + Curl, email, token));

                var mail = new MimeMessage();
                mail.From.Add(MailboxAddress.Parse(_configuration.GetSection("EmailDetails:DefaultEmail").Value));
                mail.To.Add(MailboxAddress.Parse(email));
                mail.Subject = "Password Reset";
                mail.Body = new TextPart(TextFormat.Html) { Text = HtmlBody };

                using var smtp = new SmtpClient();
                smtp.Connect(_configuration.GetSection("EmailDetails:EmailHost").Value, 587, SecureSocketOptions.StartTls);
                smtp.Authenticate(_configuration.GetSection("EmailDetails:EmailUsername").Value, _configuration.GetSection("EmailDetails:EmailPassword").Value);
                smtp.Send(mail);
                smtp.Disconnect(true);

                return new ResponseViewModel { message = "Email Sent", status = true, data = "No data available" };
            }
            catch (Exception ex)
            {
                return new ResponseViewModel();
            }
        }
    }
}
