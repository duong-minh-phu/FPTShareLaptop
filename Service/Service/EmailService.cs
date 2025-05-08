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

        public async Task SendUnifiedAppointmentEmailToSponsor(string sponsorName, string sponsorEmail)
        {
            var appointmentDate = DateTime.Today.AddDays(1).AddHours(9); // 9h sáng ngày hôm sau
            string location = "Lô E2a-7, Đường D1, Khu Công nghệ cao, P. Long Thạnh Mỹ, TP. Thủ Đức, TP. Hồ Chí Minh";

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["SmtpSettings:Username"]));
            email.To.Add(MailboxAddress.Parse(sponsorEmail));
            email.Subject = "[FPT E-Laptop] - Xác nhận lịch hẹn nhận máy hoặc hỗ trợ giao hàng";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                        <!DOCTYPE html>
                        <html lang='vi'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title>Xác nhận giao nhận laptop</title>
                        </head>
                        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333;'>
                            <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #ffffff; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
                                <h2 style='color: #007bff; text-align: center;'>Xác nhận giao nhận laptop</h2>
                                <p>Xin chào <strong>{sponsorName}</strong>,</p>
                                <p>Cảm ơn bạn đã đăng ký tài trợ cho dự án <strong>FPT E-Laptop</strong>.</p>
                                <p>Chúng tôi xin gửi đến bạn lịch hẹn nhận máy như sau:</p>

                                <ul>
                                    <li><strong>Thời gian:</strong> {appointmentDate:HH:mm dd/MM/yyyy}</li>
                                    <li><strong>Địa điểm:</strong> {location}</li>
                                </ul>

                                <p>Nếu bạn <strong>không thể đến trực tiếp</strong>, bạn có thể gửi máy đến cùng địa chỉ qua dịch vụ giao hàng. Chúng tôi sẽ hỗ trợ toàn bộ chi phí vận chuyển.</p>

                                <blockquote style='background-color: #f8f9fa; padding: 10px; border-left: 5px solid #007bff;'>
                                    {location}<br/>
                                    Liên hệ: 0987 654 321
                                </blockquote>

                                <p>Sau khi gửi hàng, vui lòng phản hồi email này với mã vận đơn hoặc ảnh biên nhận để chúng tôi tiện theo dõi.</p>
                                <p>Trân trọng cảm ơn sự đồng hành của bạn!</p>
                                <p><strong>Đội ngũ FPT E-Laptop</strong></p>
                            </div>
                        </body>
                        </html>"
                };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["SmtpSettings:Host"], 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["SmtpSettings:Username"], _config["SmtpSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

        public async Task SendUnifiedAppointmentEmailToStudent(string studentName, string studentEmail)
        {
            var appointmentDate = DateTime.Today.AddDays(1).AddHours(9); // 9h sáng ngày hôm sau
            string location = "Lô E2a-7, Đường D1, Khu Công nghệ cao, P. Long Thạnh Mỹ, TP. Thủ Đức, TP. Hồ Chí Minh";

            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_config["SmtpSettings:Username"]));
            email.To.Add(MailboxAddress.Parse(studentEmail));
            email.Subject = "[FPT E-Laptop] - Xác nhận lịch hẹn nhận máy hoặc hỗ trợ giao hàng";

            email.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = $@"
                        <!DOCTYPE html>
                        <html lang='vi'>
                        <head>
                            <meta charset='UTF-8'>
                            <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                            <title>Xác nhận giao nhận laptop</title>
                        </head>
                        <body style='font-family: Arial, sans-serif; background-color: #f4f4f4; color: #333;'>
                            <div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #ffffff; border-radius: 10px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
                                <h2 style='color: #007bff; text-align: center;'>Xác nhận giao nhận laptop</h2>
                                <p>Xin chào <strong>{studentName}</strong>,</p>
                                <p>Cảm ơn bạn đã mượn máy của dự án <strong>FPT E-Laptop</strong>.</p>
                                <p>Chúng tôi xin gửi đến bạn lịch hẹn nhận máy như sau:</p>

                                <ul>
                                    <li><strong>Thời gian:</strong> {appointmentDate:HH:mm dd/MM/yyyy}</li>
                                    <li><strong>Địa điểm:</strong> {location}</li>
                                </ul>


                                <blockquote style='background-color: #f8f9fa; padding: 10px; border-left: 5px solid #007bff;'>
                                    {location}<br/>
                                    Liên hệ: 0987 654 321
                                </blockquote>

                                <p>Sau khi gửi hàng, vui lòng phản hồi email này với mã vận đơn hoặc ảnh biên nhận để chúng tôi tiện theo dõi.</p>
                                <p>Trân trọng cảm ơn sự đồng hành của bạn!</p>
                                <p><strong>Đội ngũ FPT E-Laptop</strong></p>
                            </div>
                        </body>
                        </html>"
            };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_config["SmtpSettings:Host"], 587, MailKit.Security.SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_config["SmtpSettings:Username"], _config["SmtpSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }

    }
}
