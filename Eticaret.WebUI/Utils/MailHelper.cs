using Eticaret.Core.Entities;
using System.Net;
using System.Net.Mail;

namespace Eticaret.WebUI.Utils
{
    public class MailHelper
    {
        public static async Task<bool> SendMailAsync(Contact contact)
        {
            SmtpClient smtpClient = new SmtpClient("mail.siteadi.com",587);

            smtpClient.Credentials = new NetworkCredential("info@siteadi.com","mailşifre");
            smtpClient.EnableSsl = false;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("info@siteadi.com");
            mailMessage.To.Add("gonderilece@siteadi.com");
            mailMessage.Subject = "Konu Başlığı";
            mailMessage.Body = $"İsim: {contact.Name} - soyisim: {contact.Name}";
            mailMessage.IsBodyHtml = true;
            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                smtpClient.Dispose();
                return true;
            }
            catch (Exception)
            {

               return false;
            }
            
        }

        public static async Task<bool> SendMailAsync(string email,string subject , string mailBody)
        {
            SmtpClient smtpClient = new SmtpClient("mail.siteadi.com",587);

            smtpClient.Credentials = new NetworkCredential("info@siteadi.com","mailşifre");
            smtpClient.EnableSsl = false;
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = new MailAddress("info@siteadi.com");
            mailMessage.To.Add(email);
            mailMessage.Subject = subject;
            mailMessage.Body = mailBody;
            mailMessage.IsBodyHtml = true;
            try
            {
                await smtpClient.SendMailAsync(mailMessage);
                smtpClient.Dispose();
                return true;
            }
            catch (Exception)
            {

               return false;
            }
            
        }
    }
}
