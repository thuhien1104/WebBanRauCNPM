using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using WebBanRauProject.Models;

namespace WebBanRauProject.Controllers
{
    public class ThongKeController : Controller
    {
        dbQLBANRAUCUDataContext data = new dbQLBANRAUCUDataContext();
        // GET: ThongKe
        ///////////Thong ke
        [HttpGet]
        public ActionResult DoanhThu()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoanhThu(FormCollection collection)
        {
            DateTime dateTime = DateTime.Parse(collection["Date"]);

            var doanhThu = from ct in data.DONDATHANGs.Where(x => x.NGAYGIAO == dateTime)
                            select ct;

            return View(doanhThu);
        }
    }
}