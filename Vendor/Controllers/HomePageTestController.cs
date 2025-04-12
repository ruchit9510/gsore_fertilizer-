using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vendor.Models;

namespace Vendor.Controllers
{
    public class HomePageTestController : Controller
    {
        AppFoodDbContext db = new AppFoodDbContext();
        // GET: HomePageTest
        public ActionResult Index()
        {
            List<Products> products = db.Products.ToList<Products>();
            return View(products);
        }
    }
}