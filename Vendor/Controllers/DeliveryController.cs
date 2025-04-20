using System;
using System.Web.Mvc;
using Vendor.Models;

namespace Vendor.Controllers
{
    public class DeliveryController : Controller
    {
        private AppFoodDbContext db = new AppFoodDbContext();

        // GET: Delivery
        public ActionResult Index()
        {
            return Content("Delivery Controller is working!");
        }

        // GET: Delivery/Test
        public ActionResult Test()
        {
            return View();
        }

        // GET: Delivery/Create
        public ActionResult Create(int? invoiceId)
        {
            if (invoiceId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            // Check if delivery info already exists for this invoice
            var invoice = db.invoiceModel.Find(invoiceId);
            if (invoice == null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (invoice.DeliveryInfo != null)
            {
                return RedirectToAction("OrderConfirmation", "Products", new { invoiceId = invoiceId });
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
                deliveryInfo.DeliveryDate = DateTime.Now;
                
                // Get the invoice and set the delivery info
                var invoice = db.invoiceModel.Find(invoiceId);
                if (invoice != null)
                {
                    invoice.DeliveryInfo = deliveryInfo;
                    db.SaveChanges();
                }

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