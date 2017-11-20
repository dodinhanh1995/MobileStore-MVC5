using System.Configuration;
using System.Net;
using System.Net.Mail;

namespace Common
{
    public static class MailHelper
    {
        public static void SendMail(string toEmailAddress, string subject, string body)
        {
            var fromEmailAddress = ConfigurationManager.AppSettings["MailAccount"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["MailPassword"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["MailDisplayName"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();
            var smtpPort = int.Parse(ConfigurationManager.AppSettings["SMTPPort"]);

            bool enableSSL = bool.Parse(ConfigurationManager.AppSettings["EnableSSL"].ToString());

            MailMessage message = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
            message.Subject = subject;
            message.IsBodyHtml = true;
            message.Body = body;

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.EnableSsl = enableSSL;
            client.Port = smtpPort;
            client.Send(message);
        }
    }
}
