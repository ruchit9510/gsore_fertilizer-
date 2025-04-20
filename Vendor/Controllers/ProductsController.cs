using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vendor.Models;

namespace Vendor.Controllers
{
    public class ProductsController : Controller
    {
        AppFoodDbContext db = new AppFoodDbContext();
        public DbSet<Order> Orders { get; set; }
        public DbSet<InvoiceModel> InvoiceModels { get; set; }
        public DbSet<Products> Products { get; set; }
        public DbSet<Category> Categories { get; set; }
        // GET: Products
        public ActionResult Index()
        {
            List<Products> products = db.Products.ToList();
            ViewBag.Categories = db.Categories.ToList();
            return View(products);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            Products products = db.Products.Find(id);
            db.Products.Remove(products);
            db.SaveChanges();
            return RedirectToAction("ViewProductsAdmin", "Products");
        }

        [HttpGet]
        public ActionResult CreateNewProduct()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                ViewBag.Categories = db.Categories.ToList();
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

        [HttpPost]
        public ActionResult CreateNewProduct(HttpPostedFileBase file, Products products)
        {
            if (ModelState.IsValid && file != null && file.ContentLength > 0)
            {
                try
                {
                    string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    products.ProductPicture = "Images/" + file.FileName;
                    db.Products.Add(products);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Product added successfully!";
                    return RedirectToAction("CreateNewProduct");
                }
                catch (Exception ex)
                {
                    ViewBag.Message = "ERROR: " + ex.Message;
                }
            }
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                Products products = db.Products.Find(id);
                ViewBag.CategoryList = db.Categories.ToList();
                return View(products);
            }
            return RedirectToAction("LoginAdmin", "Admin");
        }

        [HttpPost]
        public ActionResult EditProduct(HttpPostedFileBase file, Products products, int CategoryId)
        {
            if (ModelState.IsValid)
            {
                if (file != null && file.ContentLength > 0)
                {
                    string path = Path.Combine(Server.MapPath("~/Images"), Path.GetFileName(file.FileName));
                    file.SaveAs(path);
                    products.ProductPicture = "Images/" + file.FileName;
                }
                products.CategoryId = CategoryId;
                db.Entry(products).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Product updated successfully.";
                return RedirectToAction("ViewProductsAdmin");
            }
            ViewBag.CategoryList = db.Categories.ToList();
            return View(products);
        }

        [HttpGet]
        public ActionResult EditCartItem(int id)
        {
            var cart = TempData["cart"] as List<Cart>;
            var item = cart?.FirstOrDefault(x => x.productId == id);
            if (item != null)
            {
                TempData.Keep(); // preserve cart for next request
                return View(item);
            }
            return RedirectToAction("Checkout");
        }

        [HttpPost]
        public ActionResult EditCartItem(int productId, int qty)
        {
            var cart = TempData["cart"] as List<Cart>;
            var item = cart?.FirstOrDefault(x => x.productId == productId);
            if (item != null)
            {
                item.qty = qty;
                item.bill = item.price * qty;
                TempData["cart"] = cart;
            }
            TempData.Keep();
            return RedirectToAction("Checkout");
        }

        public ActionResult ViewProductsAdmin(string searchTerm, int? categoryId)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var products = db.Products.AsQueryable();

                if (!string.IsNullOrEmpty(searchTerm))
                {
                    products = products.Where(p => p.ProductName.Contains(searchTerm));
                }

                if (categoryId.HasValue && categoryId != 0)
                {
                    products = products.Where(p => p.CategoryId == categoryId);
                }

                ViewBag.Categories = new SelectList(db.Categories.ToList(), "CategoryId", "CategoryName");
                return View(products.ToList());
            }
            return RedirectToAction("LoginAdmin", "Admin");
        }


        public ActionResult addToCart(int? Id)
        {
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null)
            {
                Products products = db.Products.Find(Id);
                return View(products);
            }
            return RedirectToAction("Login", "User");
        }

        List<Cart> li = new List<Cart>();

        [HttpPost]
        public ActionResult addToCart(int Id, string number)
        {
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie != null)
            {
                Products products = db.Products.Find(Id);
                Cart cart = new Cart()
                {
                    productId = products.id,
                    productName = products.ProductName,
                    productPic = products.ProductPicture,
                    price = products.ProductPrice,
                    qty = Convert.ToInt32(number),
                    bill = products.ProductPrice * Convert.ToInt32(number)
                };

                if (TempData["cart"] == null)
                {
                    li.Add(cart);
                    TempData["cart"] = li;
                }
                else
                {
                    li = TempData["cart"] as List<Cart>;
                    li.Add(cart);
                    TempData["cart"] = li;
                }
                TempData.Keep();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Login", "User");
        }

        public ActionResult Checkout()
        {
            if (TempData["cart"] != null)
            {
                List<Cart> li2 = TempData["cart"] as List<Cart>;
                float total = li2.Sum(item => item.bill);
                TempData["total"] = total;
            }
            TempData.Keep();
            return View();
        }

        public ActionResult Remove(int? id)
        {
            List<Cart> li2 = TempData["cart"] as List<Cart>;
            Cart c = li2.SingleOrDefault(x => x.productId == id);
            if (c != null)
            {
                li2.Remove(c);
            }
            TempData["cart"] = li2;
            TempData.Keep();
            return RedirectToAction("Checkout");
        }

        // Add this action for order confirmation view
        public ActionResult OrderConfirmation(int invoiceId)
        {
            var invoice = db.invoiceModel // Corrected to InvoiceModels
                .Include(i => i.user)
                .Include(i => i.Orders.Select(o => o.prodcts))
                .FirstOrDefault(i => i.ID == invoiceId);

            if (invoice == null)
            {
                return RedirectToAction("Index");
            }

            return View(invoice);
        }

        [HttpPost]
        public ActionResult PlaceOrder()
        {
            var userInCookie = Request.Cookies["UserInfo"];
            if (userInCookie == null)
            {
                return RedirectToAction("Login", "User");
            }

            if (TempData["cart"] == null || !(TempData["cart"] is List<Cart> cart) || !cart.Any())
            {
                TempData["ErrorMessage"] = "Your cart is empty!";
                return RedirectToAction("Checkout");
            }

            try
            {
                int userId = int.Parse(userInCookie["idUser"]);
                var invoice = new InvoiceModel
                {
                    DateInvoice = DateTime.Now,
                    Total_Bill = cart.Sum(item => item.bill),
                    FKUserID = userId
                };

                db.invoiceModel.Add(invoice);
                db.SaveChanges();

                foreach (var item in cart)
                {
                    var order = new Order
                    {
                        Qty = item.qty,
                        Unit_Price = (int)item.price,
                        Order_Bill = item.bill,
                        Order_Date = DateTime.Now,
                        FkProdId = item.productId,
                        FkInvoiceID = invoice.ID
                    };
                    db.orders.Add(order);
                }

                db.SaveChanges();

                TempData["cart"] = null;
                TempData["SuccessMessage"] = "Order placed successfully!";
                
                // Redirect to delivery information page instead of order confirmation
                return RedirectToAction("Create", "Delivery", new { invoiceId = invoice.ID });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error placing order: {ex.Message}";
                TempData.Keep("cart");
                return RedirectToAction("Checkout");
            }
        }

        // New action to download invoice as PDF
        public ActionResult DownloadInvoice(int invoiceId)
        {
            var invoice = db.invoiceModel
                .Include(i => i.user)
                .Include(i => i.Orders.Select(o => o.prodcts))
                .Include(i => i.DeliveryInfo)
                .FirstOrDefault(i => i.ID == invoiceId);

            if (invoice == null)
            {
                return HttpNotFound();
            }

            // Create PDF document
            using (MemoryStream memoryStream = new MemoryStream())
            {
                Document document = new Document(PageSize.A4, 25, 25, 30, 30);
                PdfWriter writer = PdfWriter.GetInstance(document, memoryStream);
                document.Open();

                // Add title
                Paragraph title = new Paragraph($"Invoice #{invoice.ID}", FontFactory.GetFont("Arial", 18, Font.BOLD));
                title.Alignment = Element.ALIGN_CENTER;
                document.Add(title);

                document.Add(new Paragraph(" ")); // Empty line

                // Add invoice details
                Paragraph details = new Paragraph(
                    $"Date: {invoice.DateInvoice?.ToString("MMMM dd, yyyy")}\n" +
                    $"Customer: {invoice.user?.Name}\n" +
                    $"Total Amount: ₹{invoice.Total_Bill}",
                    FontFactory.GetFont("Arial", 12)
                );
                document.Add(details);

                // Add delivery information if available
                if (invoice.DeliveryInfo != null)
                {
                    document.Add(new Paragraph(" ")); // Empty line
                    Paragraph deliveryInfo = new Paragraph(
                        $"Delivery Information:\n" +
                        $"Name: {invoice.DeliveryInfo.FullName}\n" +
                        $"Phone: {invoice.DeliveryInfo.PhoneNumber}\n" +
                        $"Address: {invoice.DeliveryInfo.StreetAddress}\n" +
                        $"City: {invoice.DeliveryInfo.City}, {invoice.DeliveryInfo.State} {invoice.DeliveryInfo.PostalCode}",
                        FontFactory.GetFont("Arial", 12)
                    );
                    document.Add(deliveryInfo);
                    
                    if (!string.IsNullOrEmpty(invoice.DeliveryInfo.AdditionalNotes))
                    {
                        document.Add(new Paragraph($"Additional Notes: {invoice.DeliveryInfo.AdditionalNotes}", 
                            FontFactory.GetFont("Arial", 12)));
                    }
                }

                document.Add(new Paragraph(" ")); // Empty line

                // Add table for order items
                PdfPTable table = new PdfPTable(4);
                table.WidthPercentage = 100;
                table.SetWidths(new float[] { 3f, 1f, 1f, 1f });

                // Table headers
                table.AddCell(new PdfPCell(new Phrase("Product", FontFactory.GetFont("Arial", 12, Font.BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Quantity", FontFactory.GetFont("Arial", 12, Font.BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Unit Price", FontFactory.GetFont("Arial", 12, Font.BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });
                table.AddCell(new PdfPCell(new Phrase("Subtotal", FontFactory.GetFont("Arial", 12, Font.BOLD))) { BackgroundColor = BaseColor.LIGHT_GRAY });

                // Table data
                foreach (var order in invoice.Orders)
                {
                    table.AddCell(new PdfPCell(new Phrase(order.prodcts?.ProductName ?? "N/A")));
                    table.AddCell(new PdfPCell(new Phrase(order.Qty.ToString())));
                    table.AddCell(new PdfPCell(new Phrase($"₹{order.Unit_Price}")));
                    table.AddCell(new PdfPCell(new Phrase($"₹{order.Order_Bill}")));
                }

                // Total row
                PdfPCell totalLabelCell = new PdfPCell(new Phrase("Total", FontFactory.GetFont("Arial", 12, Font.BOLD))) { Colspan = 3, HorizontalAlignment = Element.ALIGN_RIGHT };
                PdfPCell totalValueCell = new PdfPCell(new Phrase($"₹{invoice.Orders.Sum(o => o.Order_Bill)}"));
                table.AddCell(totalLabelCell);
                table.AddCell(totalValueCell);

                document.Add(table);

                document.Close();

                // Return PDF file
                byte[] bytes = memoryStream.ToArray();
                return File(bytes, "application/pdf", $"Invoice_{invoice.ID}.pdf");
            }
        }


    }
}