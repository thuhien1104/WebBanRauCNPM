using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebBanRauProject.Models;

namespace WebBanRauProject.Controllers
{
    public class AdminController : Controller
    {
        dbQLBANRAUCUDataContext data = new dbQLBANRAUCUDataContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Rau()
        {
            return View(data.SANPHAMs.ToList());
        }
        public ActionResult KhachHang()
        {
            return View(data.KHACHHANGs.ToList());
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(FormCollection collection)
        {
            var tendn = collection["username"];
            var matkhau = collection["password"];
            if (String.IsNullOrEmpty(tendn) || String.IsNullOrEmpty(matkhau))
            {
                ViewData["Loi"] = "Không được bỏ trống tên đăng nhập,mật khẩu";
            }
            else
            {
                Admin ad = data.Admins.SingleOrDefault(n => n.UserAdmin == tendn && n.PasswordAdmin == matkhau);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("Rau", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không chính xác";
                }
            }
            return View();
        }

        [HttpGet]
        public ActionResult ThemmoiRau()
        {
            //Dua du lieu vao dropdownlist
            //sap xep tang dan theo ten loai rau va ten ncc de lay
            ViewBag.MaLoai = new SelectList(data.LOAIRAUs.ToList().OrderBy(n => n.TENLOAI), "MaLoai", "TenLoai");

            ViewBag.MaNCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(n => n.TENCC), "MaNCC", "TenCC");
            return View();
        }
        [HttpPost]
        public ActionResult ThemmoiRau(SANPHAM sp, HttpPostedFileBase fileupload)
        {
            ViewBag.MaLoai = new SelectList(data.LOAIRAUs.ToList().OrderBy(n => n.TENLOAI), "MaLoai", "TenLoai");

            ViewBag.MaNCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(n => n.TENCC), "MaNCC", "TenCC");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh cho sản phẩm";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan File
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiem tra hinh da ton tai chua\
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                        fileupload.SaveAs(path);//Luu file vao duong dan

                    sp.ANHSP = fileName;

                    data.SANPHAMs.InsertOnSubmit(sp);
                    data.SubmitChanges();
                }
                return RedirectToAction("Rau");
            }
            return View();
        }

        public ActionResult Chitietrau(int id)
        {
            //lay doi tuong
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            ViewBag.MaSP = sp.MASP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        public ActionResult ChitietKhachHang(int makh)
        {
            //lay doi tuong
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MAKH == makh);
            ViewBag.MaSP = kh.MAKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpGet]
        public ActionResult DeleteKhachHang(int makh)

        {
            //lay doi tuong
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MAKH == makh);
            ViewBag.MaSP = kh.MAKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kh);
        }

        [HttpPost, ActionName("DeleteKhachHang")]

        public ActionResult DeleteKH(int makh)
        {
            //lay doi tuong
            KHACHHANG kh = data.KHACHHANGs.SingleOrDefault(n => n.MAKH == makh);
            ViewBag.MaKH = kh.MAKH;
            if (kh == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.KHACHHANGs.DeleteOnSubmit(kh);
            data.SubmitChanges();

            return RedirectToAction("KhachHang");
        }
        public ActionResult Lienhe()
        {
            return null;
        }
        
        public ActionResult SuaRau(int id)
        {
            ViewBag.MaLoai = new SelectList(data.LOAIRAUs.ToList().OrderBy(n => n.TENLOAI), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(n => n.TENCC), "MaNCC", "TenCC");
            var rau = data.SANPHAMs.First(k => k.MASP == id);
            return View(rau);
        }
        [HttpPost]
        public ActionResult SuaRau(int id, FormCollection collection, HttpPostedFileBase fileupload)
        {
            var sp = data.SANPHAMs.First(k => k.MASP == id);
            ViewBag.MaLoai = new SelectList(data.LOAIRAUs.ToList().OrderBy(n => n.TENLOAI), "MaLoai", "TenLoai");
            ViewBag.MaNCC = new SelectList(data.NHACUNGCAPs.ToList().OrderBy(n => n.TENCC), "MaNCC", "TenCC");
            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh cho sản phẩm";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    //Luu ten file
                    var fileName = Path.GetFileName(fileupload.FileName);
                    //Luu duong dan File
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    //Kiem tra hinh da ton tai chua\
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                        fileupload.SaveAs(path);//Luu file vao duong dan

                    sp.ANHSP = fileName;
                }
            }

            
            string tenRau = collection["TENSP"];
            decimal giaBan = decimal.Parse(collection["GIABAN"]);
            string moTa = collection["MOTA"];
            DateTime ngayNhap = DateTime.Parse(collection["NGAYCAPNHAT"]);
            double soLuongTon = double.Parse(collection["SOLUONGTON"]);
            int maLoai = int.Parse(collection["MALOAI"]);
            int maNCC = int.Parse(collection["MANCC"]);

            sp.TENSP = tenRau;
            sp.GIABAN = giaBan;
            sp.MOTA = moTa;
            sp.NGAYCAPNHAT = ngayNhap;
            sp.SOLUONGTON = soLuongTon;
            sp.MALOAI = maLoai;
            sp.MANCC = maNCC;

            UpdateModel(sp);
            data.SubmitChanges();

            return RedirectToAction("Chitietrau",new { id = id});
        }

        //Admin co the xem sua don dat hang

        public ActionResult DanhSachDonHang()
        {
            ViewBag.DaThanhToan = "Đã thanh toán";
            ViewBag.ChuaThanhToan = "Chưa thanh toán";
            List<DONDATHANG> lstDonHang = data.DONDATHANGs.ToList();
            return View(lstDonHang);
        }
        public ActionResult XemDonDatHang(int id)
        {
            var donhang = data.DONDATHANGs.First(d => d.MADH == id);
            return View(donhang);
        }
        [HttpGet]
        public ActionResult SuaDonHang(int id)
        {
            var donhang = data.DONDATHANGs.First(d => d.MADH == id);
            return View(donhang);
        }
        [HttpPost]
        public ActionResult SuaDonHang(int id,FormCollection collection)
        {
            var donhang = data.DONDATHANGs.First(d => d.MADH == id);

            bool daThanhToan = bool.Parse(collection["DATHANHTOAN"]);
            bool tinhTrangGiaoHang = bool.Parse(collection["TINHTRANGGIAOHANG"]);

            UpdateModel(donhang);
            data.SubmitChanges();

            return RedirectToAction("DanhSachDonHang");
        }
    }

}
