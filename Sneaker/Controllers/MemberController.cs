using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sneaker.Models;

namespace Sneaker.Controllers
{
    public class MemberController : Controller
    {
        // GET: NguoiDung
        DataYikesDataContext data = new DataYikesDataContext();
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Register(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["HoTenKH"];
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            var matkhaunhaplai = collection["MatKhauNhapLai"];
            var diachi = collection["DiaChi"];
            var email = collection["Email"];
            var dienthoai = collection["DienThoai"];
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["loi1"] = "Họ tên khách hàng không được để trống";

            }
            else if (string.IsNullOrEmpty(tendn))
            {
                ViewData["loi2"] = "Phải nhập tên đăng nhập";

            }
            else if (string.IsNullOrEmpty(matkhau))
            {
                ViewData["loi3"] = "Phải nhập mật khẩu";
            }
            else if (string.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["loi4"] = "Phải nhập lại mật khẩu";
            }
            if (string.IsNullOrEmpty(diachi))
            {
                ViewData["loi5"] = "Địa chỉ không được bỏ trống";
            }
            if (string.IsNullOrEmpty(email))
            {
                ViewData["loi6"] = "Email không được bỏ trống";
            }
            if (string.IsNullOrEmpty(dienthoai))
            {
                ViewData["loi7"] = "Phải nhập điện thoại";
            }
            else
            {
                kh.HoTen = hoten;
                kh.TaiKhoan = tendn;
                kh.MatKhau = matkhau;
                kh.Email = email;
                kh.DiaChiKH = diachi;
                kh.DienThoaiKH = dienthoai;
                data.KhachHangs.InsertOnSubmit(kh);
                data.SubmitChanges();
                return RedirectToAction("Login");
            }
            return this.Register();
        }
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                KhachHang kh = data.KhachHangs.SingleOrDefault(n => n.TaiKhoan == tendn && n.MatKhau == matkhau);
                if (kh != null)
                {
                    Session["TaiKhoan"] = kh;
                    Session["HoTen"] = kh.HoTen; // Thêm thông tin tên người dùng vào Session
                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewBag.Thongbao = "TÊN ĐĂNG NHẬP HOẶC MẬT KHẨU KHÔNG ĐÚNG";
            }
            return View();
        }
        public ActionResult Logout()
        {
            Session["TaiKhoan"] = null;
            Session["Cart"] = null;
            return RedirectToAction("Index", "Home");
        }

    }
}