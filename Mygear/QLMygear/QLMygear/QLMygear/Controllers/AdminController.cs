using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLMygear.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace QLMygear.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
        dbQLMygearDataContext data = new dbQLMygearDataContext();
        public ActionResult Index()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                return View();
            }
        }
        public ActionResult SanPham(int? page)
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 9;
                return View(data.SANPHAMs.ToList().OrderBy(a => a.MaSP).ToPagedList(pageNumber, pageSize));
            }
        }
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["TenDN"];
            var matkhau = collection["MatKhau"];
            if (String.IsNullOrEmpty(tendn))
            {
                ViewData["Loi1"] = "Tên đăng nhập không được để trống";
            }
            else if (String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi2"] = "Mật khẩu không được để trống";
            }
            else
            {
                QUANLY ad = data.QUANLies.SingleOrDefault(n => n.TenDN == tendn && n.Matkhau == matkhau);
                if (ad != null)
                {
                    ViewBag.ThongBao = "Đăng nhập thành công";
                    Session["USERNAME"] = ad;
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu không đúng";
                }
            }
            return RedirectToAction("SanPham");
        }
        [HttpGet]
        public ActionResult ThemSP()
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                ViewBag.MaLSP = new SelectList(data.LOAISPs.ToList().OrderBy(n => n.TenLSP), "MaLSP", "TenLSP");
            }
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]

        public ActionResult ThemSP(SANPHAM sp, HttpPostedFileBase fileupload)
        {

            ViewBag.MaLSP = new SelectList(data.LOAISPs.ToList().OrderBy(n => n.TenLSP), "MaLSP", "TenLSP");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh bìa";
                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/image"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sp.Anhbia = fileName;
                    data.SANPHAMs.InsertOnSubmit(sp);

                    data.SubmitChanges();
                }
                return RedirectToAction("SanPham");
            }
        }
        public ActionResult Details(int id)
        {
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            return View(sp);
        }
        [HttpGet]
        public ActionResult XoaSP(int id)
        {
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            return View(sp);
        }
        [HttpPost, ActionName("XoaSP")]
        public ActionResult XacnhanXoa(int id)
        {
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.SANPHAMs.DeleteOnSubmit(sp);
            data.SubmitChanges();
            return RedirectToAction("SanPham");
        }
        [HttpGet]
        public ActionResult SuaSP(int id)
        {
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.MaSP == id);
            ViewBag.MaSP = sp.MaSP;
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                if (sp == null)
                {
                    Response.StatusCode = 404;
                    return null;
                }
            }
            ViewBag.LoaiSP = new SelectList(data.LOAISPs.ToList().OrderBy(n => n.TenLSP), "MaLSP", "TenLSP");
            return View(sp);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaSP(SANPHAM sp, HttpPostedFileBase fileupload)
        {
            ViewBag.LoaiSP = new SelectList(data.LOAISPs.ToList().OrderBy(n => n.TenLSP), "MaLSP", "TenLSP");
            if (fileupload == null)
            {

                return View();
            }
            else
            {
                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/San_Pham"), fileName);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sp.Anhbia = fileName;
                    UpdateModel(sp);
                    data.SubmitChanges();
                }
                return RedirectToAction("SanPham");
            }
        }
        public ActionResult LoaiSP(int? page)
        {
            if (Session["USERNAME"] == null)
            {
                return RedirectToAction("Login", "Admin");
            }
            else
            {
                int pageNumber = (page ?? 1);
                int pageSize = 9;
                return View(data.NSXes.ToList().OrderBy(a => a.TenNSX).ToPagedList(pageNumber, pageSize));
            }
        }
        [HttpGet]
        public ActionResult ThemNH()
        {
            return View();
        }

    }
}