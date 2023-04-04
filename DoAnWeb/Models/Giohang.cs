using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Xml.Linq;

namespace DoAnWeb.Models
{
    public class Giohang
    {
        MyDataDataContext data = new MyDataDataContext();
        public int madoan { get; set; } 

        [Display(Name = "Tên Đồ ăn")]
        public string tendoan { get; set; }

        [Display(Name = "Hình")]
        public string hinh { get; set; }

        [Display(Name = "Giá bán")]
        public double giaban { get; set; }

        [Display(Name = "Số lượng")]
        public int soluong { get; set; }

        [Display(Name = "Thành tiền")]
        public double thanhtien
        {
            get { return soluong * giaban; }
        }


        public Giohang(int id)
        {
            madoan = id;
            ThucAn monan = data.ThucAns.Single(n => n.madoan == madoan);
            tendoan = monan.tendoan;
            hinh = monan.hinh;
            giaban = double.Parse(monan.giaban.ToString());
            soluong = 1;
        }
    }
}