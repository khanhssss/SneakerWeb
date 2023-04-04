
using SneakerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sneaker.Controllers
{
    public class UserController : Controller
    {
        SneakerManagerDataContext context = new SneakerManagerDataContext();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Dangky()
        {
            return View();
        }
        // POST: Hàm Dangky(post) Nhận dữ liệu từ trang Dangky và thực hiện việc tạo mới dữ liệu
        [HttpPost]
        public ActionResult Dangky(FormCollection collection, KhachHang dangNhap)
        {
            // Gán các giá tị người dùng nhập liệu cho các biến 
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            var matkhaunhaplai = collection["Matkhaunhaplai"];
            var hoten = collection["HoTen"];
            var diachi = collection["Diachi"];
            var dienthoai = collection["Dienthoai"];
            var email = collection["Email"];
            var ngaysinh = String.Format("{0:MM/dd/yyyy}", collection["Ngaysinh"]);
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else if (String.IsNullOrEmpty(matkhaunhaplai))
            {
                ViewData["Loi3"] = "Phải nhập lại mật khẩu";
            }
            else if (String.IsNullOrEmpty(hoten))
            {
                ViewData["Loi4"] = "Họ tên khách hàng không được để trống";
            }
            if (String.IsNullOrEmpty(dienthoai))
            {
                ViewData["Loi5"] = "Phải nhập điện thoai";
            }

            if (String.IsNullOrEmpty(email))
            {
                ViewData["Loi6"] = "Email không được bỏ trống";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)

                dangNhap.TenDangNhap = tendn;
                dangNhap.MatKhau = matkhau;
                dangNhap.NhapLaiMatKhau = matkhaunhaplai;
                dangNhap.HoTen = hoten;
                dangNhap.DiaChi = diachi;
                dangNhap.SDT = dienthoai;
                dangNhap.Email = email;
                dangNhap.NgaySinh = DateTime.Parse(ngaysinh);
                context.KhachHangs.InsertOnSubmit(dangNhap);
                context.SubmitChanges();
                return RedirectToAction("Dangnhap");
            }
            return this.Dangky();
        }
        [HttpGet]
        public ActionResult Dangnhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Dangnhap(FormCollection collection)
        {
            // Gán các giá trị người dùng nhập liệu cho các biến 
            var tendn = collection["TenDN"];
            var matkhau = collection["Matkhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Phải nhập tên đăng nhập";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Phải nhập mật khẩu";
            }
            else
            {
                //Gán giá trị cho đối tượng được tạo mới (kh)

                KhachHang dangNhap = context.KhachHangs.SingleOrDefault(n => n.TenDangNhap == tendn && n.MatKhau == matkhau);
                if (dangNhap != null)
                {

                    Session["TenDangNhap"] = dangNhap;
                    return RedirectToAction("Index", "Home");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
    }
}