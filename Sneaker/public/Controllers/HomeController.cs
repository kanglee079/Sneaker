using Sneaker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace Sneaker.Controllers
{
    public class HomeController : Controller
    {
        private DataYikesDataContext db = new DataYikesDataContext();

        private List<SanPham> LatestProducts(int count)
        {
            return db.SanPhams.OrderByDescending(a => a.MaSP).Take(count).ToList();
        }

        public ActionResult Index()
        {
            var sanpham = LatestProducts(8);
            return View(sanpham);
        }

        public ActionResult About()
        {
            ViewBag.pageTitle = "Về chúng tôi";
            ViewBag.pageSubTitle = "Giới thiệu";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.pageTitle = "Liên hệ với chúng tôi";
            ViewBag.pageSubTitle = "Liên hệ";
            return View();
        }

        [HttpPost]
        public ActionResult SendEmail(string name, string email, string subject, string message)
        {
            string receiverEmail = "yikesdoancoso@gmail.com";

            // Tạo đối tượng MailMessage để xây dựng email
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(email); 
            mail.To.Add(receiverEmail); 
            mail.Subject = subject; 
            mail.Body = $"Name: {name}<br>Email: {email}<br>Message: {message}";
            mail.IsBodyHtml = true; 

            // Cấu hình SMTP để gửi email
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            smtpClient.EnableSsl = true; 
            smtpClient.Credentials = new NetworkCredential("yikesdoancoso@gmail.com", "ltwfmuyctdyxwsho"); 

            try
            {
                // Gửi email
                smtpClient.Send(mail);

                // Gửi email thành công, chuyển hướng người dùng đến một trang thành công hoặc hiển thị thông báo thành công
                TempData["SuccessMessage"] = "Email has been sent successfully.";
                return RedirectToAction("Contact");
            }
            catch (Exception ex)
            {
                // Xử lý lỗi nếu gửi email không thành công, chẳng hạn hiển thị thông báo lỗi hoặc chuyển hướng đến trang lỗi
                TempData["ErrorMessage"] = "Failed to send email. Please try again later.";
                return RedirectToAction("Contact");
            }
        }
    }
}
