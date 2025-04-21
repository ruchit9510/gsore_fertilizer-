using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vendor.Models;

namespace Vendor.Controllers
{
    public class AdminController : Controller
    {
        AppFoodDbContext db = new AppFoodDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                return View();
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }

        }
        [HttpGet]
        public ActionResult LoginAdmin()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return View();
                }
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LoginAdmin(AdminLogin model)
        {
            var data = db.adminLogin.Where(s => s.Email.Equals(model.Email) && s.Password.Equals(model.Password)).ToList();
            if (data.Count() > 0)
            {
                HttpCookie cooskie = new HttpCookie("AdminInfo");
                cooskie.Values["idAdmin"] = Convert.ToString(data.FirstOrDefault().adminid);
                cooskie.Values["Email"] = Convert.ToString(data.FirstOrDefault().Email);
                cooskie.Expires = DateTime.Now.AddMonths(1);
                Response.Cookies.Add(cooskie);
                return RedirectToAction("Index", "Admin");
            }
            else
            {
                ViewBag.Message = "Login failed";
                return RedirectToAction("LoginAdmin");
            }
        }
        public ActionResult LogoutAdmin()
        {
            if (this.ControllerContext.HttpContext.Request.Cookies.AllKeys.Contains("AdminInfo"))
            {
                HttpCookie cookie = this.ControllerContext.HttpContext.Request.Cookies["AdminInfo"];
                cookie.Expires = DateTime.Now.AddDays(-1);
                this.ControllerContext.HttpContext.Response.Cookies.Add(cookie);
            }
            return RedirectToAction("LoginAdmin");
        }

        // List Orders with optional date filter
        public ActionResult ListOfOrders(DateTime? startDate, DateTime? endDate)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                float t = 0;
                var orders = db.orders.AsQueryable();

                if (startDate.HasValue && endDate.HasValue)
                {
                    orders = orders.Where(o => o.Order_Date >= startDate && o.Order_Date <= endDate);
                }

                var orderList = orders.ToList();

                foreach (var item in orderList)
                {
                    t += item.Order_Bill;
                }

                TempData["OrderTotal"] = t;
                return View(orderList);
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                return userInCookie != null
                    ? RedirectToAction("Index", "Products")
                    : RedirectToAction("LoginAdmin", "Admin");
            }
        }

        // Accept Order (just deleting for now as per your request)
        [HttpPost]
        public ActionResult AcceptOrder(int id)
        {
            var order = db.orders.Find(id);
            if (order != null)
            {
                db.orders.Remove(order); // for real-world use: update status instead
                db.SaveChanges();
            }
            return RedirectToAction("ListOfOrders");
        }

        // Delete Order
        [HttpPost]
        public ActionResult DeleteOrder(int id)
        {
            var order = db.orders.Find(id);
            if (order != null)
            {
                db.orders.Remove(order);
                db.SaveChanges();
            }
            return RedirectToAction("ListOfOrders");
        }

        public ActionResult ListOfInvoices()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                float t = 0;
                List<InvoiceModel> invoice = db.invoiceModel.ToList<InvoiceModel>();

                foreach (var item in invoice)
                {
                    t += item.Total_Bill;


                }
                TempData["InvoiceTotal"] = t;
                return View(invoice);
            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Index", "Products");
                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }

        [HttpPost]
        public ActionResult DeleteInvoice(int id)
        {
            var invoice = db.invoiceModel.Find(id);
            if (invoice != null)
            {
                // First, delete related orders
                var relatedOrders = db.orders.Where(o => o.FkInvoiceID == invoice.ID).ToList();
                db.orders.RemoveRange(relatedOrders);

                // Then delete the invoice
                db.invoiceModel.Remove(invoice);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Invoice deleted successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Invoice not found.";
            }

            return RedirectToAction("ListOfInvoices");
        }

    }
}