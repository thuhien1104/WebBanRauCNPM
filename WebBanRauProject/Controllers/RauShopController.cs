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
            return data.SANPHAMs.OrderByDescending(a => a.NGAYCAPNHAT).Take(count).ToList();
        }

        private List<SPBanChay> LaySanPhamBanChay(int count)
        {
            return data.SPBanChays.OrderByDescending(a => a.SOLUOMHMUA).Take(count).ToList();
        }
      
        // GET: RauShop
        public ActionResult Index(int ? page)
        {
            //Lay 4 Rau moi nhap
            var raumoi = LaySanPhamMoiNhap(4);
            int pageSize = 4;
            int pageNum = (page ?? 1);
           
            return View(raumoi.ToPagedList(pageNum, pageSize));
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
            var rau = from sp in data.SANPHAMs where sp.MALOAI == id select sp;
            int pageSize = 4;
            int pageNum = (page ?? 1);
   
            return View(rau.ToPagedList(pageNum, pageSize));
        }
        //Chi tiet 1 loai Rau
        public ActionResult Details(int id)
        {
            var rau = from sp in data.SANPHAMs where sp.MASP == id select sp;
            return View(rau.Single());
        }
        //Hien thi tat ca san pham
        public ActionResult ShowAll(int ? page)
        {
            var rau = from sp in data.SANPHAMs select sp;
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
        public ActionResult NhaCC()
        {
            var ncc = from sp in data.NHACUNGCAPs select sp; //sp
            return View(ncc);
        }
       



    }
}