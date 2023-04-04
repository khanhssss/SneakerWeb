using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SneakerWeb.Models;

namespace Sneaker.Controllers
{
    public class DonHangController : Controller
    {
        SneakerManagerDataContext context = new SneakerManagerDataContext();
        // GET: DonHang
        public List<DonHang> LayDonHang()
        {
            List<DonHang> lstDonHang = Session["DonHang"] as List<DonHang>;
            if (lstDonHang == null)
            {
                //Neu gio hang chua ton tai thi khoi tao listDonHang
                lstDonHang = new List<DonHang>();
                Session["DonHang"] = lstDonHang;
            }
            return lstDonHang;
        }

        //Them hang vao gio
        public ActionResult Index()
        {
            return View();
        }
    }
}