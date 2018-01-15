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
            Admin ad = (Admin)Session["Taikhoanadmin"];
            if (ad != null)
            {
                var admin = data.Admins.First(k => k.ID == ad.ID);
                return View(admin);
            }
            else
                return RedirectToAction("Login");
            
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
                    return RedirectToAction("Index", "Admin");
                }
                else
                {
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không chính xác";
                }
            }
            return View();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// ///
        public ActionResult Rau()
        {
            var query = from sp in data.SANPHAMs
                        where !(from t in data.TEMPs select t.MASP).Contains(sp.MASP)
                        select sp;
            return View(query);
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

        public ActionResult XoaRau(int id)
        {
            //lấy đối tượng :
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            ViewBag.MaSP = sp.MASP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(sp);
        }

        [HttpPost, ActionName("XoaRau")]
        public ActionResult XacNhanXoaRau(int id, TEMP temp)
        {
            //lấy ra đối tượng cần xóa theo mã:
            SANPHAM sp = data.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            ViewBag.MaSP = sp.MASP;
            temp.MASP = sp.MASP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.TEMPs.InsertOnSubmit(temp);
            data.SubmitChanges();
            return RedirectToAction("Rau");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="makh"></param>
        /// <returns></returns>
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

            return RedirectToAction("Chitietrau", new { id = id });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        public ActionResult DanhSachDonHang()
        {
            ViewBag.DaThanhToan = "Đã thanh toán";
            ViewBag.ChuaThanhToan = "Chưa thanh toán";
            ViewBag.DaGiaoHang = "Đã giao hàng";
            ViewBag.ChuaGiaoHang = "Chưa giao hàng";
            List<DONDATHANG> lstDonHang = data.DONDATHANGs.OrderByDescending(d=>d.NGAYDAT).ToList();
            return View(lstDonHang);
        }
        public ActionResult ChiTietDonHang(int id)
        {
            var chitiet = data.CHITIETDONHANGs.Where(ct => ct.MADH == id);
            return View(chitiet);
        }
        [HttpPost]
        public ActionResult ChiTietDonHang(int id, FormCollection collection)
        {
            var chitiet = data.CHITIETDONHANGs.Where(ct => ct.MADH == id);
            bool flag = true;
            foreach (var item in chitiet)
            {
                var sp = data.SANPHAMs.First(k => k.MASP == item.MASP);
                sp.SOLUONGTON -= item.SOLUONG;
                if (sp.SOLUONGTON < 0)
                {
                    ViewBag.ThongBao = "Hàng trong kho đã hết!!!Xin kiểm tra lại!!!";
                    flag = false;
                }
            }
            if (flag == true)
            {
                foreach (var item in chitiet)
                {
                    var sp = data.SANPHAMs.First(k => k.MASP == item.MASP);
                    sp.SOLUONGTON -= item.SOLUONG;
                    UpdateModel(sp);                    
                    data.SubmitChanges();
                }
                var donhang = data.DONDATHANGs.Where(dh => dh.MADH == id).First();
                donhang.TINHTRANGGIAOHANG = true;
                UpdateModel(donhang);
                data.SubmitChanges();
                return RedirectToAction("DanhSachDonHang");
            }
            return View(chitiet);

        }
        [HttpGet]
        public ActionResult SuaDonHang(int id)
        {
            var donhang = data.DONDATHANGs.First(d => d.MADH == id);
            return View(donhang);
        }
        [HttpPost]
        public ActionResult SuaDonHang(int id, FormCollection collection)
        {
            var donhang = data.DONDATHANGs.First(d => d.MADH == id);

            bool daThanhToan = bool.Parse(collection["DATHANHTOAN"]);
            bool tinhTrangGiaoHang = bool.Parse(collection["TINHTRANGGIAOHANG"]);

            UpdateModel(donhang);
            data.SubmitChanges();

            return RedirectToAction("DanhSachDonHang");
        }


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ActionResult MonAn()
        {
            return View(data.QuanLyGoiYMonAns.ToList());
        }

        [HttpGet]
        public ActionResult ThemmoiMonAn()
        {


            return View();
        }
        [HttpPost]

        public ActionResult ThemmoiMonAn(QuanLyGoiYMonAn mon, HttpPostedFileBase fileupload)
        {

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

                    mon.HINHANH = fileName;

                    data.QuanLyGoiYMonAns.InsertOnSubmit(mon);
                    data.SubmitChanges();
                }
                return RedirectToAction("MonAn");
            }
            return View();
        }

        public ActionResult XoaMonAn(int id)
        {
            //lấy đối tượng :
            QuanLyGoiYMonAn mon = data.QuanLyGoiYMonAns.SingleOrDefault(n => n.MASO == id);
            ViewBag.MaSO = mon.MASO;
            if (mon == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mon);
        }

        [HttpPost, ActionName("XoaMonAn")]
        public ActionResult XacNhanXoaMonAn(int id)
        {
            //lấy ra đối tượng cần xóa theo mã:
            QuanLyGoiYMonAn mon = data.QuanLyGoiYMonAns.SingleOrDefault(n => n.MASO == id);
            ViewBag.MaSO = mon.MASO;
            if (mon == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.QuanLyGoiYMonAns.DeleteOnSubmit(mon);
            data.SubmitChanges();
            return RedirectToAction("MonAn");
        }

        public ActionResult ChitietMonAn(int id)
        {
            //lay doi tuong
            QuanLyGoiYMonAn mon = data.QuanLyGoiYMonAns.SingleOrDefault(n => n.MASO == id);
            ViewBag.MaSO = mon.MASO;
            if (mon == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(mon);
        }

        
        public ActionResult SuaMonAn(int id)
        {
            //lay doi tuong
            var mon = data.QuanLyGoiYMonAns.First(k => k.MASO == id);
            return View(mon);
        }
        [HttpPost]
        public ActionResult SuaMonAn(int id, FormCollection collection, HttpPostedFileBase fileupload)
        {
            var mon = data.QuanLyGoiYMonAns.First(k => k.MASO == id);
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

                    mon.HINHANH = fileName;
                }
            }


            string tenMonAn = collection["TENMONAN"];
            string moTa = collection["MOTA"];


            mon.TENMONAN = tenMonAn;
            mon.MOTA = moTa;

            UpdateModel(mon);
            data.SubmitChanges();

            return RedirectToAction("ChitietMonAn", new { id = id });
        }


        //them admin:
        [HttpGet]
        public ActionResult ThemAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemAdmin(Admin ad, HttpPostedFileBase fileupload)
        {

            if (fileupload == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh cho Admin nào";
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

                    ad.Hinhanh = fileName;

                    data.Admins.InsertOnSubmit(ad);
                    data.SubmitChanges();
                }
                return RedirectToAction("Index");
            }
            return View();
        }
        
        public ActionResult XoaAdmin(int id)
        {
            //lấy đối tượng :
            Admin ad = data.Admins.SingleOrDefault(n => n.ID == id);
            ViewBag.id = ad.ID;
            if (ad == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ad);
        }

        }
        public ActionResult NCC()
        {
            return View(data.NHACUNGCAPs.ToList());
        }

        public ActionResult ChitietNCC(int id)
        {
            NHACUNGCAP ncc = data.NHACUNGCAPs.SingleOrDefault(n => n.MANCC == id);
            ViewBag.MANCC = ncc.MANCC;
            if(ncc==null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);

        }

        [HttpGet]
        public ActionResult ThemNCC()
        {
            return View();

        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemNCC(NHACUNGCAP nc, HttpPostedFileBase fileupload)
        {

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

                    nc.HINHANH = fileName;



                    data.NHACUNGCAPs.InsertOnSubmit(nc);
                    data.SubmitChanges();
                }
                return RedirectToAction("ThemNCC");
            }
            
            return View();
            
            
        }

        [HttpGet]
        public ActionResult XoaNCC(int id)
        {
            NHACUNGCAP ncc = data.NHACUNGCAPs.SingleOrDefault(n => n.MANCC == id);
            ViewBag.MANCC = ncc.MANCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc);
        }

        [HttpPost, ActionName("XoaNCC")]
        public ActionResult XacnhanxoaNCC(int id)
        {
            NHACUNGCAP ncc = data.NHACUNGCAPs.SingleOrDefault(n => n.MANCC == id);
            ViewBag.MANCC = ncc.MANCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            data.NHACUNGCAPs.DeleteOnSubmit(ncc);
            data.SubmitChanges();
            return RedirectToAction("NCC");
        }

        public ActionResult SuaNCC(int id)
        {
           
            NHACUNGCAP ncc = data.NHACUNGCAPs.SingleOrDefault(n => n.MANCC == id);
            ViewBag.MANCC = ncc.MANCC;
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
  
            return View(ncc);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaNCC(NHACUNGCAP ncc,HttpPostedFileBase fileUploat)
        {
            if (fileUploat == null)
            {
                ViewBag.Thongbao = "Vui lòng chọn ảnh";
                return View();
            }
            else
            {

                if (ModelState.IsValid)
                {
                    var fileName = Path.GetFileName(fileUploat.FileName);
                    var path = Path.Combine(Server.MapPath("~/images"), fileName);
                    if (System.IO.File.Exists(path))
                        ViewBag.Thongbao = "Hình ảnh đã tồn tại";
                    else
                    {
                        fileUploat.SaveAs(path);
                    }

                    var a = data.NHACUNGCAPs.Where(p => p.MANCC == ncc.MANCC).FirstOrDefault();
                    a.TENCC = ncc.TENCC;
                    a.MOTA = ncc.MOTA;
                    a.DIACHI = ncc.DIACHI;
                    a.DIENTHOAI = ncc.DIENTHOAI;
                    UpdateModel(a);
                    data.SubmitChanges();

                }
            }
            return RedirectToAction("NCC");
        }

    }

}
