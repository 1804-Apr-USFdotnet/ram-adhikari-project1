using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        
        public ActionResult About()
        {
            ViewBag.AboutMessage = "This website is currently under construction; however, when it's done, it will provide " +
                                "information realted to restaurants and the quality of food they provide.";

            return View();
            
        }
    }
}