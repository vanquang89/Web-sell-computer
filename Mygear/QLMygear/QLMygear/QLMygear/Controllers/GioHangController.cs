using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLMygear.Models;

namespace QLMygear.Controllers
{
    public class GiohangController : Controller
    {
        // GET: Giohang
        dbQLMygearDataContext data = new dbQLMygearDataContext();
        public List<Giohang> LayGiohang()
        {
            List<Giohang> lsGiohang = Session["Giohang"] as List<Giohang>;
            if (lsGiohang == null)
            {
                lsGiohang = new List<Giohang>();
                Session["Giohang"] = lsGiohang;
            }
            return lsGiohang;
        }
        public ActionResult ThemGiohang(int Masp)
        {
            List<Giohang> lsGiohang = LayGiohang();
            Giohang sp = lsGiohang.Find(n => n.iMaSP == Masp);
            if (sp == null)
            {
                sp = new Giohang(Masp);
                lsGiohang.Add(sp);
                return RedirectToAction("Giohang");
            }
            else
            {
                sp.iSoluong++;
                return RedirectToAction("Giohang");
            }
        }
        private int TongSL()
        {
            int TongSL = 0;
            List<Giohang> lsGiohang = Session["Giohang"] as List<Giohang>;
            if (lsGiohang != null)
            {
                TongSL = lsGiohang.Sum(n => n.iSoluong);
            }
            return TongSL;
        }
        private double TongTien()
        {
            double TongTien = 0;
            List<Giohang> lsGiohang = Session["Giohang"] as List<Giohang>;
            if (lsGiohang != null)
            {
                TongTien = lsGiohang.Sum(n => n.dThanhTien);
            }
            return TongTien;
        }
        public ActionResult Giohang()
        {
            List<Giohang> lsGiohang = LayGiohang();
            if (lsGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BanHang");
            }
            ViewBag.TongSL = TongSL();
            ViewBag.TongTien = TongTien();
            return View(lsGiohang);
        }
        public ActionResult GiohangPartial()
        {
            ViewBag.TongSL = TongSL();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        public ActionResult XoaGiohang(int MaSP)
        {
            List<Giohang> lsGiohang = LayGiohang();
            Giohang sp = lsGiohang.SingleOrDefault(n => n.iMaSP == MaSP);
            if (sp != null)
            {
                lsGiohang.RemoveAll(n => n.iMaSP == MaSP);
                return RedirectToAction("Giohang");
            }
            if (lsGiohang.Count == 0)
            {
                return RedirectToAction("Index", "BanHang");
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult CapNhatGiohang(int MaSP, FormCollection f)
        {
            List<Giohang> lsGiohang = LayGiohang();
            Giohang sp = lsGiohang.SingleOrDefault(n => n.iMaSP == MaSP);
            if (sp != null)
            {
                sp.iSoluong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("Giohang");
        }
        public ActionResult XoaTatCaGiohang()
        {
            List<Giohang> lsGiohang = LayGiohang();
            lsGiohang.Clear();
            return RedirectToAction("Index", "BanHang");
        }
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString() == "")
            {
                return RedirectToAction("DangNhap", "NguoiDung");
            }
            if (Session["Giohang"] == null)
            {
                return RedirectToAction("Index", "BanHang");
            }
            List<Giohang> lsGiohang = LayGiohang();
            ViewBag.TongSL = TongSL();
            ViewBag.TongTien = TongTien();
            return View(lsGiohang);
        }
        public ActionResult DatHang(FormCollection collection)
        {
            DONDATHANG ddh = new DONDATHANG();
            KHACHHANG kh = (KHACHHANG)Session["Taikhoan"];
            List<Giohang> gh = LayGiohang();

            ddh.MaKH = kh.MaKH;
            ddh.Ngaydat = DateTime.Now;
            ddh.Tinhtranggiaohang = false;
            ddh.Dathanhtoan = false;
            data.DONDATHANGs.InsertOnSubmit(ddh);
            data.SubmitChanges();

            foreach (var item in gh)
            {
                CHITIETDONTHANG ctdh = new CHITIETDONTHANG();
                ctdh.MaDonHang = ddh.MaDonHang;
                ctdh.MaSP = item.iMaSP;
                ctdh.Soluong = item.iSoluong;
                ctdh.Dongia = (decimal)item.dDongia;
                data.CHITIETDONTHANGs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Xacnhan", "Giohang");
        }


        public ActionResult Xacnhan()
        {
            return View();
        }
    }
}