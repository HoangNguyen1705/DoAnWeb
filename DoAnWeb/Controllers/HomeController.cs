using DoAnWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;

namespace DoAnWeb.Controllers
{
    public class HomeController : Controller
    {
        MyDataDataContext data = new MyDataDataContext();
        
        public ActionResult Index(int? page)
        {
            if (page == null) page = 1;
            var all_food = (from s in data.ThucAns select s).OrderBy(m => m.madoan);
            int pageSize = 3;
            int pageNum = page ?? 1;
            return View(all_food.ToPagedList(pageNum, pageSize));
        }

        public ActionResult ListKH() 
        {
            var all_kh = from ss in data.KhachHangs select ss;
            return View(all_kh);
        }
        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}