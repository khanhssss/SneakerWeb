using SneakerWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace SneakerWeb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        SneakerManagerDataContext context = new SneakerManagerDataContext();
        public ActionResult View1()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
                return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
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
                //gan gia tri
                Admin ad = context.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PassAdmin == matkhau);
                if (ad != null)
                {
                    ViewBag.Thongbao = " Đăng nhập thành công";
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("SanPham", "Admin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        public ActionResult SanPham(int? page)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 7;
                return View(context.SanPhams.ToList().OrderBy(n => n.MaSanPham).ToPagedList(pageNumber, pageSize));
            }
        }
        [HttpGet]
        public ActionResult Themmoisanpham()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ViewBag.MaLoai = new SelectList(context.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai");
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoisanpham(SanPham sanpham, HttpPostedFileBase fileupload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                //Kiem tra duong dan file
                if (fileupload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileupload.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/images/"), fileName);
                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileupload.SaveAs(path);
                        }
                        sanpham.Images = fileName;
                        //Luu vao CSDL
                        context.SanPhams.InsertOnSubmit(sanpham);
                        context.SubmitChanges();
                    }
                    return RedirectToAction("SanPham", "Admin");
                }
            }
        }
        public ActionResult Chitietsanpham(int Id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SanPham sanpham = context.SanPhams.SingleOrDefault(n => n.MaSanPham == Id);
                ViewBag.MaSanPham = sanpham.MaSanPham;
                if (sanpham == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(sanpham);
            }
        }
        [HttpGet]
        public ActionResult Xoasanpham(int Id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var sanpham = from s in context.SanPhams where s.MaSanPham == Id select s;
                return View(sanpham.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Xoasanpham")]
        public ActionResult Xacnhanxoa(int Id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SanPham sanpham = context.SanPhams.SingleOrDefault(n => n.MaSanPham == Id);
                context.SanPhams.DeleteOnSubmit(sanpham);
                context.SubmitChanges();
                return RedirectToAction("SanPham", "Admin");
            }
        }
        [HttpGet]
        public ActionResult Suasanpham(int Id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SanPham sanpham = context.SanPhams.SingleOrDefault(n => n.MaSanPham == Id);
                //Lay du liệu tư table Chude để đổ vào Dropdownlist, kèm theo chọn MaCD tương tưng 
                ViewBag.MaLoai = new SelectList(context.Loais.ToList().OrderBy(n => n.TenLoai), "MaLoai", "TenLoai", sanpham.MaLoai);
                return View(sanpham);
            }
        }
        [HttpPost, ActionName("Suasanpham")]
        public ActionResult Xacnhansua(int Id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                SanPham sanpham = context.SanPhams.SingleOrDefault(n => n.MaSanPham == Id);
                UpdateModel(sanpham);
                context.SubmitChanges();
                return RedirectToAction("SanPham", "Admin");
            }
        }

        public ActionResult Tkadmin()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View(context.Admins.ToList());
            }
        }
        [HttpGet]
        public ActionResult Themmoitk()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View();
            }
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Themmoitk(Admin ad, HttpPostedFileBase fileupload)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                //Kiem tra duong dan file
                if (fileupload == null)
                {
                    ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                    return View();
                }
                //Them vao CSDL
                else
                {
                    if (ModelState.IsValid)
                    {
                        //Luu ten fie, luu y bo sung thu vien using System.IO;
                        var fileName = Path.GetFileName(fileupload.FileName);
                        //Luu duong dan cua file
                        var path = Path.Combine(Server.MapPath("~/images/"), fileName);
                        //Kiem tra hình anh ton tai chua?
                        if (System.IO.File.Exists(path))
                            ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                        else
                        {
                            //Luu hinh anh vao duong dan
                            fileupload.SaveAs(path);
                        }
                        ad.Hinh = fileName;
                        //Luu vao CSDL
                        context.Admins.InsertOnSubmit(ad);
                        context.SubmitChanges();
                    }
                    return RedirectToAction("Tkadmin", "Admin");
                }
            }
        }
        public ActionResult Xoatk(string Id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                var ad = from a in context.Admins where a.UserAdmin == Id select a;
                return View(ad.SingleOrDefault());
            }
        }
        [HttpPost, ActionName("Xoatk")]
        public ActionResult Xacnhanxoa(string Id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Admin ad = context.Admins.SingleOrDefault(a => a.UserAdmin == Id);
                context.Admins.DeleteOnSubmit(ad);
                context.SubmitChanges();
                return RedirectToAction("Tkadmin", "Admin");
            }
        }
        public ActionResult Chitiettk(string Id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                Admin ad = context.Admins.SingleOrDefault(a => a.UserAdmin == Id);
                ViewBag.UserAdmin = ad.UserAdmin;
                if (ad == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ad);
            }
        }
        public ActionResult Khachhang()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View(context.KhachHangs.ToList());
            }
        }
        public ActionResult Phanhoi()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View(context.PhanHois.ToList());
            }
        }
        public ActionResult Chitietphanhoi(int Id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                PhanHoi ph = context.PhanHois.SingleOrDefault(a => a.Maph == Id);
                ViewBag.Maph = ph.Maph;
                if (ph == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ph);
            }
        }
        public ActionResult Donhang()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                return View(context.DonHangs.ToList());
            }
        }
        public ActionResult ChitietdonHang(int Id)
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ChiTietDonHang ph = context.ChiTietDonHangs.SingleOrDefault(a => a.MaDonHang == Id);
                ViewBag.MaDonHang = ph.MaDonHang;
                if (ph == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
                return View(ph);
            }
        }
        public ActionResult Thongke()
        {
            if (Session["Taikhoanadmin"] == null)
                return RedirectToAction("Login", "Admin");
            else
            {
                ViewBag.tongdoanhthu = thongkedoanhthu();
                ViewBag.ddh = thongkedonhang();
                ViewBag.kh = thongkekhachhang();
                return View();
            }
        }
        public decimal thongkedoanhthu()
        {
            decimal tongdoanhthu = context.ChiTietDonHangs.Sum(n => n.Soluong * n.Dongia).Value;
            return tongdoanhthu;
        }
        public double thongkedonhang()
        {
            double ddh = context.DonHangs.Count();
            return ddh;
        }
        public double thongkekhachhang()
        {
            double kh = context.KhachHangs.Count();
            return kh;
        }
    }
}