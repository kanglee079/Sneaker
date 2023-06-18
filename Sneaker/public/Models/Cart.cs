using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sneaker.Models
{
    public class Cart
    {
        DataYikesDataContext data = new DataYikesDataContext();
        public int iMaSP { set; get; }
        public string sTenSP { set; get; }
        public double dGiaBan { set; get; }
        public string sAnhDD { set; get; }
        public int iSoLuong { set; get; }
        public double dThanhTien
        {
            get { return iSoLuong * dGiaBan; }
        }
        public Cart(int MaSP)
        {
            iMaSP = MaSP;
            SanPham sanpham = data.SanPhams.Single(n => n.MaSP == iMaSP);
            sTenSP = sanpham.TenSP;
            sAnhDD = sanpham.AnhDD;
            dGiaBan = double.Parse(sanpham.GiaBan.ToString());
            iSoLuong = 1;
        }
    }
}