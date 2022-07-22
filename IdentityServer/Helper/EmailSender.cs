using IdentityServer.Models;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace IdentityServer.Helper
{
    public class EmailSender
    {
        private readonly string host;
        private readonly int port;
        private readonly bool enableSSL;
        private readonly string userName;
        private readonly string password;
        private readonly string senderName;
        private readonly string fromEmail;

        public EmailSender()
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            this.host = MyConfig.GetValue<string>("Email:host");
            this.port = MyConfig.GetValue<int>("Email:port");
            this.enableSSL = MyConfig.GetValue<bool>("Email:enableSSL");
            this.userName = MyConfig.GetValue<string>("Email:userName");
            this.password = MyConfig.GetValue<string>("Email:password");
            this.senderName = MyConfig.GetValue<string>("Email:senderName");
            this.fromEmail = MyConfig.GetValue<string>("Email:fromEmail");
        }
        public void ConfirmedEmail(User user)
        {
            var MyConfig = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            string url = MyConfig.GetValue<string>("applicationUrl");
            var token = TokenHelper.GenerateJSONWebToken(user);
            string link = $@"{url}/api/User/ConfirmEmail/{token}";
            var Info = new StringBuilder();
            Info.Append($"<br><Strong>Hello {user.UserName}</Strong><br>");
            Info.Append($"<br>Please confirm the registration");
            Info.Append($"<br>Click the link bellow<br>");
            Info.Append($"<a href={link}> Confirm </a>");


            var IsSended = SendEmailAsync(user.Email, "Registration Confirm", Info.ToString());
        }
        public void SendNewPassword(User user, string newPassword)
        {
            var Info = new StringBuilder();
            Info.Append($"<br><Strong>Hello {user.UserName}</Strong><br>");
            Info.Append($"<br>Your new password is");
            Info.Append($"<br>{newPassword}<br>");
            var IsSended = SendEmailAsync(user.Email, "New Password", Info.ToString());

        }
        public async Task<bool> SendEmailAsync(string toEmail, string subject, string htmlMessage, string cc = null, string bcc = null)
        {
            try
            {
                var client = new SmtpClient(host, port)
                {
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(fromEmail, password),
                    EnableSsl = enableSSL,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };
                var message = new MailMessage(fromEmail, toEmail, subject, htmlMessage)
                {
                    IsBodyHtml = true,
                    BodyEncoding = Encoding.UTF8
                };
                message.From = new MailAddress(fromEmail, senderName);
                if (!string.IsNullOrEmpty(cc))
                {
                    message.CC.Add(cc);
                }
                if (!string.IsNullOrEmpty(bcc))
                {
                    message.Bcc.Add(bcc);
                }
                client.Send(message);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
