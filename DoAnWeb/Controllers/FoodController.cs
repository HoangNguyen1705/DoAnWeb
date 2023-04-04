using DoAnWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DoAnWeb.Controllers
{
    public class FoodController : Controller
    {
        // GET: Food
        MyDataDataContext data = new MyDataDataContext();
        [Authorize]
        public ActionResult ListFood()
        {
            var all_food = from ss in data.ThucAns select ss;
            return View(all_food);

        }

        public ActionResult Detail(int id)
        {
            var D_sach = data.ThucAns.Where(m => m.madoan == id).First();
            return View(D_sach);
        }

        public ActionResult Create()
        {
            return View();
        }


      
        [HttpPost]
        public ActionResult Create(FormCollection collection, ThucAn s)
        {
            var E_tendoan = collection["tendoan"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycapnhat"]);
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            if (string.IsNullOrEmpty(E_tendoan))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                s.tendoan = E_tendoan.ToString();
                s.hinh = E_hinh.ToString();
                s.giaban = E_giaban;
                s.ngaycapnhat = E_ngaycapnhat;
                s.soluongton = E_soluongton;
                data.ThucAns.InsertOnSubmit(s);
                data.SubmitChanges();
                return RedirectToAction("ListFood");
            }
            return this.Create();
        }

        public ActionResult Edit(int id)
        {
            var E_doan = data.ThucAns.First(m => m.madoan == id);
            return View(E_doan);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            var E_ThucAn = data.ThucAns.First(m => m.madoan == id);
            var E_tendoan = collection["tendoan"];
            var E_hinh = collection["hinh"];
            var E_giaban = Convert.ToDecimal(collection["giaban"]);
            var E_ngaycapnhat = Convert.ToDateTime(collection["ngaycatnhat"]);
            var E_soluongton = Convert.ToInt32(collection["soluongton"]);
            E_ThucAn.madoan = id;
            if (string.IsNullOrEmpty(E_tendoan))
            {
                ViewData["Error"] = "Don't empty!";
            }
            else
            {
                E_ThucAn.tendoan = E_tendoan;
                E_ThucAn.hinh = E_hinh;
                E_ThucAn.giaban = E_giaban;
                E_ThucAn.ngaycapnhat = E_ngaycapnhat;
                E_ThucAn.soluongton = E_soluongton;
                UpdateModel(E_ThucAn);
                data.SubmitChanges();
                return RedirectToAction("ListFood");
            }
            return this.Edit(id);
        }

        public ActionResult Delete(int id)
        {
            var D_ThucAn = data.ThucAns.First(m => m.madoan == id);
            return View(D_ThucAn);
        }


        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            var D_ThucAn = data.ThucAns.Where(m => m.madoan == id).First();
            data.ThucAns.DeleteOnSubmit(D_ThucAn);
            data.SubmitChanges();
            return RedirectToAction("ListFood");
        }



        public string ProcessUpload(HttpPostedFileBase file)
        {
            if (file == null)
            {
                return "";
            }
            file.SaveAs(Server.MapPath("~/Content/images/" + file.FileName));
            return "/Content/images/" + file.FileName;
        }


    }
}