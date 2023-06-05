using Sneaker.Models;
using System.Collections.Generic;
using System.Linq;
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
    }
}