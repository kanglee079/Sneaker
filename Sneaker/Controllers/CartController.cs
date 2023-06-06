using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sneaker.Models;
using System.Net.Mail;
using System.Net;
using System.Text;

namespace Sneaker.Controllers
{
    public class CartController : Controller
    {
        DataYikesDataContext data = new DataYikesDataContext();
        public List<Cart> TakeACart()
        {
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart == null)
            {
                lstCart = new List<Cart>();
                Session["Cart"] = lstCart;
            }
            return lstCart;
        }
        public ActionResult AddToCart(int iMaSP, string strUrl)
        {
            List<Cart> lstCart = TakeACart();
            Cart sanpham = lstCart.Find(n => n.iMaSP == iMaSP);
            if (sanpham == null)
            {
                sanpham = new Cart(iMaSP);
                lstCart.Add(sanpham);
                return Redirect(strUrl);
            }
            else
            {
                sanpham.iSoLuong++;
                return Redirect(strUrl);
            }
        }
        private int TotalQuantity()
        {
            int iTotalQuantity = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                iTotalQuantity = lstCart.Sum(n => n.iSoLuong);
            }
            return iTotalQuantity;
        }
        private double TotalAmount()
        {
            double iTotalAmount = 0;
            List<Cart> lstCart = Session["Cart"] as List<Cart>;
            if (lstCart != null)
            {
                iTotalAmount = lstCart.Sum(n => n.dThanhTien);
            }
            return iTotalAmount;
        }
        public ActionResult Cart()
        {
            List<Cart> lstCart = TakeACart();
            if (lstCart.Count == 0)
            {
                return RedirectToAction("Index", "Cart");
            }
            ViewBag.TotalQuantity = TotalQuantity();
            ViewBag.TotalAmount = TotalAmount();
            return View(lstCart);
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CartPartial()
        {
            ViewBag.TotalQuantity = TotalQuantity();
            ViewBag.TotalAmount = TotalAmount();
            return PartialView();
        }
        public ActionResult DeleteCart(int iMaSP)
        {
            List<Cart> lstCart = TakeACart();
            Cart sanpham = lstCart.SingleOrDefault(n => n.iMaSP == iMaSP);
            if (sanpham != null)
            {
                lstCart.RemoveAll(n => n.iMaSP == iMaSP);
                return RedirectToAction("Cart");
            }
            if (lstCart.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Cart");
        }
        public ActionResult UpdateCart(int iMaSP, FormCollection f)
        {
            List<Cart> lstCart = TakeACart();
            Cart sanpham = lstCart.SingleOrDefault(n => n.iMaSP == iMaSP);
            if (sanpham != null)
            {
                sanpham.iSoLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("Cart");
        }
        public ActionResult DeleteAllCart()
        {
            List<Cart> lstCart = TakeACart();
            lstCart.Clear();
            return RedirectToAction("Index", "Home");
        }
        [HttpGet]
        public ActionResult Order()
        {
            if (Session["TaiKhoan"] == null || Session["TaiKhoan"].ToString() == "")
            {
                return RedirectToAction("Register", "Member");
            }
            if (Session["Cart"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            // lấy giỏ hàng từ session
            List<Cart> lstCart = TakeACart();
            ViewBag.TotalQuantity = TotalQuantity();
            ViewBag.TotalAmount = TotalAmount();
            return View(lstCart);
        }
        public ActionResult Order(FormCollection collection)
        {
            DonDatHang ddh = new DonDatHang();
            KhachHang kh = (KhachHang)Session["TaiKhoan"];
            List<Cart> gh = TakeACart();
            ddh.MaKH = kh.MaKH;
            ddh.NgayDat = DateTime.Now;
            data.DonDatHangs.InsertOnSubmit(ddh);
            data.SubmitChanges();
            // thêm chi tiết đơn hàng
            foreach (var item in gh)
            {
                ChiTietDatHang ctdh = new ChiTietDatHang();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSP = item.iMaSP;
                ctdh.SoLuong = item.iSoLuong;
                ctdh.DonGia = (decimal)item.dGiaBan;
                data.ChiTietDatHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();

            // Gửi email tới người quản lý về đơn hàng mới
            SendOrderPlacedEmailToAdmin(ddh);

            // Gửi email tới khách hàng xác nhận đơn hàng
            SendOrderConfirmationEmailToCustomer(ddh, kh.Email);

            Session["Cart"] = null;
            return RedirectToAction("Payment", "Cart");

        }

        private void SendOrderPlacedEmailToAdmin(DonDatHang order)
        {
            var fromAddress = new MailAddress("yikesdoancoso@gmail.com", "Yikes Shop");
            var toAddress = new MailAddress("hykhang792002@gmail.com", "To Name");
            const string fromPassword = "ltwfmuyctdyxwsho";
            const string subject = "New order placed";
            var body = new StringBuilder();
            body.AppendLine($"Có đơn hàng mới đã được đặt. Thông tin đơn hàng:");
            body.AppendLine($"Mã đơn hàng: {order.MaDonHang}");
            body.AppendLine($"Tên khách hàng: {order.KhachHang.HoTen}");
            body.AppendLine($"Mã khách hàng: {order.MaKH}");
            body.AppendLine($"Thời gian đặt hàng: {order.NgayDat}");
            body.AppendLine("Danh sách sản phẩm:");

            foreach (var chiTiet in order.ChiTietDatHangs)
            {
                body.AppendLine($"- Tên sản phẩm: {chiTiet.SanPham.TenSP}");
                body.AppendLine($"  Số lượng: {chiTiet.SoLuong}");
                body.AppendLine($"  Đơn giá: {chiTiet.DonGia}");
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body.ToString()
            })
            {
                smtp.Send(message);
            }
        }

        private void SendOrderConfirmationEmailToCustomer(DonDatHang order, string customerEmail)
        {
            var fromAddress = new MailAddress("yikesdoancoso@gmail.com", "Yikes Shop");
            var toAddress = new MailAddress(customerEmail, "To Name");
            const string fromPassword = "ltwfmuyctdyxwsho";
            const string subject = "Order confirmation";
            var body = new StringBuilder();
            body.AppendLine($"Đơn hàng của bạn đã được đặt thành công. Thông tin đơn hàng:");
            body.AppendLine($"Mã đơn hàng: {order.MaDonHang}");
            body.AppendLine($"Tên khách hàng: {order.KhachHang.HoTen}");
            body.AppendLine($"Mã khách hàng: {order.MaKH}");
            body.AppendLine($"Thời gian đặt hàng: {order.NgayDat}");
            body.AppendLine("Danh sách sản phẩm:");

            foreach (var chiTiet in order.ChiTietDatHangs)
            {
                body.AppendLine($"- Tên sản phẩm: {chiTiet.SanPham.TenSP}");
                body.AppendLine($"  Số lượng: {chiTiet.SoLuong}");
                body.AppendLine($"  Đơn giá: {chiTiet.DonGia}");
            }

            body.AppendLine("Yikes cảm ơn bạn đã tin tưởng mua sắm tại cửa hàng.");

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body.ToString()
            })
            {
                smtp.Send(message);
            }
        }
        public ActionResult OrderSuccess()
        {
            return View();
        }
        public ActionResult Payment()
        {
            return View();
        }
        public ActionResult Momo()
        {
            return View();
        }

    }
}