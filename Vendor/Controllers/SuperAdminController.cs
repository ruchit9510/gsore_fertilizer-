using System.Linq;
using System.Web.Mvc;
using Vendor.Models;

namespace Vendor.Controllers
{
    public class SuperAdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(SuperAdminLogin model)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppFoodDbContext())
                {
                    var superAdmin = db.SuperAdminLogins
                        .FirstOrDefault(sa => sa.Email == model.Email && sa.Password == model.Password);

                    if (superAdmin != null)
                    {
                        return RedirectToAction("SuperAdminHome");
                    }
                    else
                    {
                        TempData["InvalidLogin"] = "Invalid email or password. Please try again.";
                    }
                }
            }
            return View(model);
        }

        public ActionResult SuperAdminHome()
        {
            using (var db = new AppFoodDbContext())
            {
                var users = db.SignupLogin.ToList();
                var vendors = db.adminLogin.ToList();

                ViewBag.Users = users;
                ViewBag.Vendors = vendors;
            }

            return View();
        }

        [HttpPost]
        public ActionResult AddVendor(AdminLogin vendor)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppFoodDbContext())
                {
                    db.adminLogin.Add(vendor);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("SuperAdminHome");
        }

        [HttpPost]
        public ActionResult EditVendor(AdminLogin vendor)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AppFoodDbContext())
                {
                    var existingVendor = db.adminLogin.Find(vendor.adminid);
                    if (existingVendor != null)
                    {
                        existingVendor.Email = vendor.Email;
                        existingVendor.Password = vendor.Password;
                        db.SaveChanges();
                    }
                }
            }
            return RedirectToAction("SuperAdminHome");
        }

        [HttpPost]
        public ActionResult DeleteVendor(int adminid)
        {
            using (var db = new AppFoodDbContext())
            {
                var vendor = db.adminLogin.Find(adminid);
                if (vendor != null)
                {
                    db.adminLogin.Remove(vendor);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("SuperAdminHome");
        }

        [HttpPost]
        public ActionResult EditUser(SignupLogin user)
        {

            using (var db = new AppFoodDbContext())
            {
                var existingUser = db.SignupLogin.Find(user.userid);
                if (existingUser != null)
                {
                    existingUser.Name = user.Name;
                    existingUser.Email = user.Email;
                    existingUser.Password = user.Password;
                    existingUser.ConfirmPassword = user.Password;
                    db.SaveChanges();
                }
            }

            return RedirectToAction("SuperAdminHome");
        }

        [HttpPost]
        public ActionResult DeleteUser(int userid)
        {
            using (var db = new AppFoodDbContext())
            {
                var user = db.SignupLogin.Find(userid);
                if (user != null)
                {
                    db.SignupLogin.Remove(user);
                    db.SaveChanges();
                }
            }
            return RedirectToAction("SuperAdminHome");
        }
    }
}