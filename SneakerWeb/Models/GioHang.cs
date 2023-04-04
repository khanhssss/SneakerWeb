using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SneakerWeb.Models;

namespace SneakerWeb.Models
{
    public class GioHang
    {
        //Tao doi tuong data chua dữ liệu từ model dbBansach đã tạo. 
        SneakerManagerDataContext context = new SneakerManagerDataContext();
        public int iMaSanPham { set; get; }
        public string sTenSanPham { set; get; }
        public string sImages { set; get; }
        public Double dDongia { set; get; }
        public int iSoluong { set; get; }
        public Double dThanhtien
        {
            get { return iSoluong * dDongia; }

        }
        //Khoi tao gio hàng theo Masach duoc truyen vao voi Soluong mac dinh la 1
        public GioHang(int MaSanPham)
        {
            iMaSanPham = MaSanPham;
            SanPham sanPham = context.SanPhams.Single(n => n.MaSanPham == iMaSanPham);
            sTenSanPham = sanPham.TenSanPham;
            sImages = sanPham.Images;
            dDongia = double.Parse(sanPham.Gia.ToString());
            iSoluong = 1;
        }
    }
}