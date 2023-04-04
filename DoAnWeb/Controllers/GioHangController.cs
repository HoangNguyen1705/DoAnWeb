using DoAnWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWeb.Controllers
{
    public class GioHangController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();

        public List<Giohang> Laygiohang()
        {
            List<Giohang> lstGioHang = Session["Giohang"] as List<Giohang>;
            if (lstGioHang == null)
            {
                lstGioHang = new List<Giohang>();
                Session["Giohang"] = lstGioHang;
            }
            return lstGioHang;
        }
        // Them san pham vao Gio Hang
        public ActionResult ThemGioHang(int id, string strURL)
        {
            List<Giohang> lstGiohang = Laygiohang();
            Giohang sanpham = lstGiohang.Find(n => n.madoan == id);
            if (sanpham == null)
            {
                sanpham = new Giohang(id);
                lstGiohang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.soluong++;
                return Redirect(strURL);
            }

        }

        private int TongSoLuong()
        {
            int tsl = 0;
            List<Giohang> lstGioHang = Session["Giohang"] as List<Giohang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Sum(n => n.soluong);
            }
            return tsl;
        }

        private int TongSLsanpham()
        {
            int tsl = 0;
            List<Giohang> lstGioHang = Session["Giohang"] as List<Giohang>;
            if (lstGioHang != null)
            {
                tsl = lstGioHang.Count;
            }
            return tsl;
        }

        private double TongTien()
        {
            double tt = 0;
            List<Giohang> lstGioHang = Session["Giohang"] as List<Giohang>;
            if (lstGioHang != null)
            {
                tt = lstGioHang.Sum(n => n.thanhtien);
            }
            return tt;
        }
        
        public ActionResult GioHang()
        {
            List<Giohang> lstGioHang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSLsanpham();
            return View(lstGioHang);
        }
       
        public ActionResult GioHangPartial()
        {
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSLsanpham();
            return PartialView();
        }

        public ActionResult XoaGioHang(int id)
        {
            List<Giohang> lstGioHang = Laygiohang();
            Giohang sanpham = lstGioHang.SingleOrDefault(n => n.madoan == id);
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.madoan == id);
                return RedirectToAction("GioHang");
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult CapNhatGioHang(int id, FormCollection collection)
        {
            List<Giohang> lstGioHang = Laygiohang();
            Giohang sanpham = lstGioHang.SingleOrDefault(n => n.madoan == id);
            if (sanpham != null)
            {
                sanpham.soluong = int.Parse(collection["txtSoLg"].ToString());
            }
            return RedirectToAction("GioHang");
        }

        public ActionResult XoaTatcaGiohang()
        {
            List<Giohang> lstGioHang = Laygiohang();
            lstGioHang.Clear();
            return RedirectToAction("GioHang");
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(FormCollection collection, KhachHang kh)
        {
            var hoten = collection["hoten"];
            var email = collection["email"];
            var diachi = collection["diachi"];
            var dienthoai = collection["dienthoai"];
           
                    kh.hoten = hoten;
                    kh.email = email;
                    kh.diachi = diachi;
                    kh.dienthoai = dienthoai;
                    
                    data.KhachHangs.InsertOnSubmit(kh);
                    data.SubmitChanges();
                    Session["ThongTin"] = kh;
                
            
            return this.DangKy();

        }

        //Code chuc nang Dat Hang
        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["ThongTin"] == null || Session["ThongTin"].ToString() == "")
            {
                return RedirectToAction("DangKy", "GioHang");
            }
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Giohang> lstGiohang = Laygiohang();
            ViewBag.Tongsoluong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            ViewBag.TongSoLuongSanPham = TongSLsanpham();
            return View(lstGiohang);
        }
        //Đặt hàng
        public ActionResult DatHang(FormCollection collection)
        {
            DonHang dh = new DonHang();
            KhachHang kh = (KhachHang)Session["ThongTin"];
            ThucAn s = new ThucAn();

            List<Giohang> gh = Laygiohang();
            var ngaygiao = String.Format("{0:MM/dd/yyyy}", collection["NgayGiao"]);

            dh.makh = kh.makh;
            dh.ngaydat = DateTime.Now;
            dh.ngaygiao = DateTime.Parse(ngaygiao);
            dh.giaohang = false;
            dh.thanhtoan = false;

            data.DonHangs.InsertOnSubmit(dh);
            data.SubmitChanges();
            foreach (var item in gh)
            {
                ChiTietDonHang ctdh = new ChiTietDonHang();
                ctdh.madon = dh.madon;
                ctdh.madoan = item.madoan;
                ctdh.soluong = item.soluong;
                ctdh.gia = (decimal)item.giaban;
                s = data.ThucAns.Single(n => n.madoan == item.madoan);
                s.soluongton = ctdh.soluong;
                data.SubmitChanges();
                data.ChiTietDonHangs.InsertOnSubmit(ctdh);
            }
            data.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacnhanDonhang", "GioHang");
        }

        //tao view XacnhanDonhang
        public ActionResult XacnhanDonhang() { return View(); }

    }
}