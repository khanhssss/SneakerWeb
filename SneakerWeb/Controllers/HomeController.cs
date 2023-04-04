using SneakerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using PagedList;
using PagedList.Mvc;

namespace SneakerWeb.Controllers
{
    public class HomeController : Controller
    {
        SneakerManagerDataContext context = new SneakerManagerDataContext();
  

        private List<SanPham> TopSanPham(int count)
        {
            //Sắp xếp sách theo ngày cập nhật, sau đó lấy top @count 
            return context.SanPhams.OrderByDescending(a => a.Gia).Take(count).ToList();
        }
        public ActionResult Index(int? page)
        {
            //Tao bien quy dinh so san pham tren moi trang
            int pageSize = 8;
            //Tao bien so trang
            int pageNum = (page ?? 1);

            var sptop = TopSanPham(16);
            var sptop2 = TopSanPham(6);

            return View(sptop.ToPagedList(pageNum, pageSize));
        }
        public ActionResult SPMEN(int id)
        {
            var spmen = from s in context.SanPhams
                        where s.MaLoai == id
                        select s;
            return View(spmen);
        }
        // SẢN PHẨM WOMEN
        public ActionResult SPWOMEN(int id)
        {
            var spwomen = from s in context.SanPhams
                          where s.MaLoai == id
                          select s;
            return View(spwomen);
        }
        // CHI TIẾT SẢN PHẨM
        public ActionResult ChiTiet(int id)
        {
            var sp = from s in context.SanPhams
                     where s.MaSanPham == id
                     select s;
            return View(sp.Single());
        }

        public ActionResult Phanhoi()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Phanhoi(FormCollection collection, PhanHoi phanhoi)
        {
            // Gán các giá tị người dùng nhập liệu cho các biến 
            var hoten = collection["hoten"];
            var email = collection["email"];
            var sdt = collection["sdt"];
            var ngayphanhoi = String.Format("{0:MM/dd/yyyy}", collection["ngayphanhoi"]);
            var noidung = collection["noidung"];
            if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi1"] = "Xin mời nhập tên";
            }
            else if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi2"] = "Xin mời nhập email";
            }
            else if (String.IsNullOrEmpty(sdt))
            {
                ViewData["Loi3"] = "Xin mời nhập số điện thoại";
            }
            else if (String.IsNullOrEmpty(noidung))
            {
                ViewData["Loi4"] = "Xin mời nhập nội dung";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)

                phanhoi.Hoten = hoten;
                phanhoi.Email = email;
                phanhoi.Sdt = sdt;
                phanhoi.Date = DateTime.Parse(ngayphanhoi);
                phanhoi.Noidung = noidung;
                context.PhanHois.InsertOnSubmit(phanhoi);
                context.SubmitChanges();
            }
            return RedirectToAction("Xacnhanphanhoi");
        }
        public ActionResult Xacnhanphanhoi()
        {
            return View();
        }
    }
}