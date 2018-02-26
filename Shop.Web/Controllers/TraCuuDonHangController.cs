using System;
using System.Linq;
using System.Web.Mvc;
using Shop.Data.Infrastructure;
using Shop.Data.Repositories;
using Shop.Model;
using Shop.Web.Models;


namespace Shop.Web.Controllers
{
    public class TraCuuDonHangController : Controller
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}
