using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebBanRauProject.Models
{
    public class DoanhThu
    {
        public int Id { get; set; }
        public string TenKhachHang { get; set; }
        public DateTime ThoiGian { get; set; }
        public float TongTien { get; set; }

        public DoanhThu()
        {

        }
    }
}