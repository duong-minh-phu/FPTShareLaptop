using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using Service.IService;

namespace Service.Service
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _config;

        public EmailService(IConfiguration config)
        {
            _config = config;
        }

        public async Task SendUserResetPassword(string fullName, string userEmail, string newPassword)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config.GetSection("SmtpSettings:Username").Value));
            email.To.Add(MailboxAddress.Parse(userEmail));
            email.Subject = "[FPT E-Laptop] - Mật khẩu mới của bạn";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
        <!DOCTYPE html>
        <html lang='vi'>
        <head>
            <meta charset='UTF-8'>
            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
            <title>Đặt lại mật khẩu</title>
        </head>
        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333;'>
            <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #ffffff; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
                <h2 style='color: #007bff; text-align: center;'>Đặt lại mật khẩu</h2>
                <p>Xin chào <strong>{fullName}</strong>,</p>
                <p>Bạn đã yêu cầu đặt lại mật khẩu cho tài khoản trên <strong>FPT E-Laptop</strong>. Đây là mật khẩu mới của bạn:</p>
                <p style='font-size: 20px; font-weight: bold; color: #d9534f; text-align: center;'>{newPassword}</p>
                <p>Vui lòng đăng nhập lại bằng mật khẩu này và thay đổi nó ngay trong phần cài đặt tài khoản.</p>
                <p>Nếu bạn không yêu cầu đặt lại mật khẩu, vui lòng bỏ qua email này.</p>
                <p>Trân trọng,</p>
                <p><strong>Đội ngũ FPT E-Laptop</strong></p>
            </div>
        </body>
        </html>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config.GetSection("SmtpSettings:Host").Value, 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(
                _config.GetSection("SmtpSettings:Username").Value,
                _config.GetSection("SmtpSettings:Password").Value
            );
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

    }
}
