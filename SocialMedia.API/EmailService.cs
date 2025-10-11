using MimeKit;
using MailKit.Net.Smtp;

namespace SocialMedia
{
    public class EmailService
    {
        private readonly string _from;
        private readonly string _appPassword; 

        public EmailService(IConfiguration configuration)
        {
            _from = configuration["Email:From"];
            _appPassword = configuration["Email:AppPassword"];
        }
        public async Task SendOtpAsync(string toEmail, string otp)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Social Media HoTi", _from));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "OTP Confirmation For Password Reset";

            message.Body = new TextPart("plain")
            {
                Text = $"Your OTP for password reset is: {otp}. It is valid for 5 minutes."
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_from, _appPassword);


            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }

        public async Task ConfirmEmail(string toEmail, string link)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("Social Media HoTi", _from));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = "Email Confirmation";

            message.Body = new TextPart("plain")
            {
                Text = $"Please confirm your email by clicking the following link: {link}"
            };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_from, _appPassword);
        }
    }
}
