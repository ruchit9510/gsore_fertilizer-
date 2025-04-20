using System;
using System.Web.Mvc;
using Vendor.Models;

namespace Vendor.Controllers
{
    public class DeliveryController : Controller
    {
        private AppFoodDbContext db = new AppFoodDbContext();

        // GET: Delivery/Create
        public ActionResult Create(int? invoiceId)
        {
            if (invoiceId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.InvoiceId = invoiceId;
            return View();
        }

        // POST: Delivery/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DeliveryInfo deliveryInfo, int invoiceId)
        {
            if (ModelState.IsValid)
            {
                deliveryInfo.FkInvoiceID = invoiceId;
                deliveryInfo.DeliveryDate = DateTime.Now;
                db.DeliveryInfos.Add(deliveryInfo);
                db.SaveChanges();

                // Redirect to order confirmation
                return RedirectToAction("OrderConfirmation", "Products", new { invoiceId = invoiceId });
            }

            ViewBag.InvoiceId = invoiceId;
            return View(deliveryInfo);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
} 