using System.Diagnostics;

namespace TheWorld.Services
{
    public class MailService : IMailService
    {
        public void SendMail(string to, string from, string subject, string body)
        {
            Debug.WriteLine($"Sending mail to: {to}, from {from} Subject: {subject}, body: {body}");
        }
    }
}
