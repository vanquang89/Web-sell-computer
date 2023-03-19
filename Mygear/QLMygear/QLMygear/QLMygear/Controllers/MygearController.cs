using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QLMygear.Models;
using PagedList;
using PagedList.Mvc;

namespace QLMygear.Controllers
{
    public class MygearController : Controller
    {
        // GET: Mygear
        dbQLMygearDataContext data = new dbQLMygearDataContext();
        private List<SANPHAM> LaySPmoi(int count)
        {
            return data.SANPHAMs.OrderByDescending(b => b.MaSP).Take(count).ToList();
        }

        public ActionResult PC()
        {
            var PC = data.SANPHAMs.Where(n => n.MaLSP == 1).Take(3).ToList();
            return PartialView(PC);
        }
        public ActionResult Laptop()
        {
            var laptop = data.SANPHAMs.Where(n => n.MaLSP == 12).Take(3).ToList();
            return PartialView(laptop);
        }
        public ActionResult ManHinh()
        {
            var ManHinh = data.SANPHAMs.Where(n => n.MaLSP == 13).Take(3).ToList();
            return PartialView(ManHinh);
        }
        public ActionResult Chuot()
        {
            var chuot = data.SANPHAMs.Where(n => n.MaLSP == 9).Take(3).ToList();
            return PartialView(chuot);
        }
        public ActionResult BanPhim()
        {
            var banphim = data.SANPHAMs.Where(n => n.MaLSP == 10).Take(3).ToList();
            return PartialView(banphim);
        }

        public ActionResult Index()
        {
            var spm = LaySPmoi(3);
            return View(spm);
        }
        private List<SANPHAM> LaySP()
        {
            return data.SANPHAMs.OrderByDescending(a => a.MaSP).ToList();
        }
        public ActionResult SanPham(int? page)
        {
            int pageSize = 9;
            int PageNum = (page ?? 1);
            var spm = LaySP();
            return View(spm.ToPagedList(PageNum, pageSize));
        }
        public ActionResult NSX()
        {
            var LSP = from nh in data.LOAISPs select nh;
            return PartialView(LSP);
        }
        public ActionResult SPtheoLSP(int id, int? page)
        {
            int pageSize = 6;// so sp tren moi trang
            int pageNum = (page ?? 1); //tao so trang

            var sanpham = from s in data.SANPHAMs where s.MaLSP == id select s;
            return View(sanpham.ToPagedList(pageNum, pageSize));
        }

        public ActionResult Details(int id)
        {
            var sanpham = from s in data.SANPHAMs
                          where s.MaSP == id
                          select s;
            return View(sanpham.Single());
        }
        public ActionResult DetailsPC(int id)
        {
            var PC = from d in data.CHITIETPCs
                      where d.MaSP == id 
                      select d;
            return View(PC.Single());
        }

        public ActionResult DetailsLAPTOP(int id)
        {
            var LP = from d in data.CHITIETPCs
                     where d.MaSP == id
                     select d;
            return View(LP.Single());
        }

        public ActionResult DetailsCPU(int id)
        {
            var LP = from d in data.CHITIETPCs
                     where d.MaSP == id
                     select d;
            return View(LP.Single());
        }

        private List<SANPHAM> Related(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.MaLSP).Take(count).ToList();
        }
        public ActionResult SPLienQuan()
        {
            var splq = Related(3);
            return View(splq);
        }

        public ActionResult BaoHanh()
        {
            return View();
        }
        public ActionResult ThanhToan()
        {
            return View();
        }
    }
}
