using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sneaker.Models;
using PagedList;
using PagedList.Mvc;

namespace Sneaker.Controllers
{
    public class ShopController : Controller
    {
        // GET: Shop
        DataYikesDataContext db = new DataYikesDataContext();
        public ActionResult Index(int? page)
        {
            int pageSize = 6; // Số sản phẩm trên 1 trang
            int pageNumber = (page ?? 1); // Trang hiện tại, mặc định là trang đầu tiên

            var products = db.SanPhams.OrderBy(p => p.TenSP); // Sắp xếp danh sách sản phẩm theo tên

            return View(products.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Brand()
        {
            var thuonghieus = from c in db.ThuongHieus select c;
            return PartialView(thuonghieus);
        }
        public ActionResult ListByBrand(int id)
        {
            var sanphams = from p in db.SanPhams where p.MaThuongHieu == id select p;
            return View(sanphams);
        }
        public ActionResult Detail(int id)
        {
            var sanphams = from p in db.SanPhams where p.MaSP == id select p;
            return View(sanphams.Single());
        }

    }
}