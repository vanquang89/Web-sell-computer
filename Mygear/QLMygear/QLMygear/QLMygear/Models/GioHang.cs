using System;
using System.Collections.Generic;
using System.Linq;
using QLMygear.Models;
using System.Web;

namespace QLMygear.Models
{
    public class Giohang
    {
        dbQLMygearDataContext data = new dbQLMygearDataContext();

        public int iMaSP { set; get; }
        public string sTenSP { set; get; }
        public string sAnhbia { set; get; }
        public int iSoluong { set; get; }
        public double dDongia { set; get; }
        public double dThanhTien
        {
            get
            {
                return iSoluong * dDongia;
            }
        }

        public Giohang(int MaSP)
        {
            iMaSP = MaSP;
            SANPHAM SP = data.SANPHAMs.Single(n => n.MaSP == iMaSP);
            sTenSP = SP.TenSP;
            sAnhbia = SP.Anhbia;
            dDongia = double.Parse(SP.Giaban.ToString());
            iSoluong = 1;
        }


    }
}