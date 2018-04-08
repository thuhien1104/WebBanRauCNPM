using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanRauProject.Models;

using PagedList;
using PagedList.Mvc;
using WebBanRauProject.Controllers;

namespace WebBanRauProject.Controllers
{
    public class RauShopController : Controller
    {
        //Tao doi tuong chua toan bo CSDL
        dbQLBANRAUCUDataContext data = new dbQLBANRAUCUDataContext();
        private List<SANPHAM> LaySanPhamMoiNhap(int count)
        {
            return data.SANPHAMs.OrderByDescending(a => a.NGAYCAPNHAT).Where(s=>s.TRANGTHAI == true).Take(count).ToList();
        }

        private List<SPBanChay> LaySanPhamBanChay(int count)
        {
            return data.SPBanChays.OrderByDescending(a => a.SOLUOMHMUA).Take(count).ToList();
        }
      
        // GET: RauShop
       
       
        public ActionResult Spbanchay()
        {
            //Lay 4 Rau moi nhap
            var raumoi = LaySanPhamBanChay(4);
            return PartialView(raumoi);
        }
        public ActionResult Index()
        {
            var raumoi = LaySanPhamMoiNhap(4);

            return View(raumoi);
        }

        //Dua loai rau vao menu list
        public ActionResult LoaiSanPham(int ? page)
        {
            //Lay 4 Rau moi nhap
            var loai = from sp in data.LOAIRAUs select sp;
            return PartialView(loai);
        }
        public ActionResult SanPhamTheoLoai(int id,int ? page)
        {
            //Lay 4 Rau moi nhap
            var rau = from sp in data.SANPHAMs where sp.MALOAI == id &&sp.TRANGTHAI == true select sp;
            int pageSize = 4;
            int pageNum = (page ?? 1);
   
            return View(rau.ToPagedList(pageNum, pageSize));
        }
        //Chi tiet 1 loai Rau
        public ActionResult Details(int id)
        {
            var rau = from sp in data.SANPHAMs where sp.MASP == id && sp.TRANGTHAI == true select sp;
            return View(rau.Single());
        }
        //Hien thi tat ca san pham
        public ActionResult ShowAll(int ? page)
        {
            var rau = from sp in data.SANPHAMs.Where(s => s.TRANGTHAI == true) select sp;
            int pageSize = 8;
            int pageNum = (page ?? 1);
            return View(rau.ToPagedList(pageNum, pageSize));
        }
        public ActionResult LienHe()
        {
            return View();
        }
        public ActionResult ThongTin()
        {
            return View();

        }
        public ActionResult NhaCC(int ? page)
        {
            var ncc = from sp in data.NHACUNGCAPs select sp; //sp
            int pageSize = 4;
            int pageNum = (page ?? 1);
            return View(ncc.ToPagedList(pageNum, pageSize));
        }
        public ActionResult CTNhaCC(int id)
        {
            var ncc = from m in data.NHACUNGCAPs where m.MANCC == id select m;
            return View(ncc.Single());
        }
        //-------------------------------------------------------
        public ActionResult MonAn(int ? page)
        {
            var mon = from m in data.QuanLyGoiYMonAns select m;
            int pageSize = 4;
            int pageNum = (page ?? 1);
            return View(mon.ToPagedList(pageNum, pageSize));
        }
        public ActionResult CTMonan(int id)
        {
            var mon = from m in data.QuanLyGoiYMonAns where m.MASO == id select m;
            return View(mon.Single());
        }



    }
}