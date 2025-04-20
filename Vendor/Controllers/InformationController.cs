using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Vendor.Models;

namespace Vendor.Controllers
{
    public class InformationController : Controller
    {
        AppFoodDbContext db = new AppFoodDbContext();

        // GET: Information
        public ActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ContactUs(ContactModel contact)
        {
            if (ModelState.IsValid)
            {
                db.contactModels.Add(contact);
                db.SaveChanges();

                // Clear ModelState to reset form fields
                ModelState.Clear();

                // Optionally show a success message
                ViewBag.Message = "Your message has been sent successfully!";

                return View(new ContactModel()); // Return empty model
            }

            return View(contact); // If validation fails, return with input values
        }

        public ActionResult MessageList()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                List<ContactModel> contacts = db.contactModels.ToList<ContactModel>();
                return View(contacts);

            }
            else
            {
                var userInCookie = Request.Cookies["UserInfo"];
                if (userInCookie != null)
                {
                    return RedirectToAction("Products", "Index");

                }
                else
                {
                    return RedirectToAction("LoginAdmin", "Admin");
                }
            }
        }

        [HttpPost]
        public JsonResult DeleteMessage(int id)
        {
            var message = db.contactModels.Find(id);
            if (message != null)
            {
                db.contactModels.Remove(message);
                db.SaveChanges();
                return Json(new { success = true });
            }
            return Json(new { success = false });
        }

        public ActionResult ViewMessage(int id)
        {
            var message = db.contactModels.FirstOrDefault(x => x.contactId == id);
            if (message == null)
            {
                return HttpNotFound();
            }
            return View(message);
        }

        public ActionResult AboutUs()
        {

            return View();
        }

        // GET: Blog List (Only for Listing & Deletion)
        public ActionResult BlogList()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var blogs = db.blogModels.ToList();
                return View(blogs);
            }
            else
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
        }

        // GET: Create Blog Page
        [HttpGet]
        public ActionResult AddAdminBlog()
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
        }

        // POST: Create Blog
        [HttpPost]
        public ActionResult AddAdminBlog(BlogModel blog)
        {
            if (ModelState.IsValid)
            {
                db.blogModels.Add(blog);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Blog added successfully!";
                return RedirectToAction("BlogList");
            }
            return View(blog);
        }

        // GET: Edit Blog Page (Fetch Data)
        [HttpGet]
        public ActionResult EditBlog(int id)
        {
            var adminInCookie = Request.Cookies["AdminInfo"];
            if (adminInCookie != null)
            {
                var blog = db.blogModels.Find(id);
                if (blog != null)
                {
                    return View(blog);
                }
                return RedirectToAction("BlogList");
            }
            else
            {
                return RedirectToAction("LoginAdmin", "Admin");
            }
        }

        // GET: Blog Details
        public ActionResult BlogDetails(int id)
        {
            var blog = db.blogModels.Find(id);
            if (blog != null)
            {
                return View(blog);  // ✅ This is correct (passing a single model)
            }
            return RedirectToAction("UserBlogs");
        }



        // GET: User Blog Page
        public ActionResult UserBlogs(string search, int page = 1)
        {
            int pageSize = 6; // Number of blogs per page
            var blogs = db.blogModels.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                blogs = blogs.Where(b => b.BlogTitle.Contains(search) || b.BlogAutherName.Contains(search));
            }

            var paginatedBlogs = blogs.OrderByDescending(b => b.BlogDate)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToList();

            ViewBag.SearchQuery = search;
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)blogs.Count() / pageSize);
            return View(paginatedBlogs);

        }


        // POST: Edit Blog (Update)
        [HttpPost]
        public ActionResult EditBlog(BlogModel blog)
        {
            if (ModelState.IsValid)
            {
                var existingBlog = db.blogModels.Find(blog.BlogId);
                if (existingBlog != null)
                {
                    existingBlog.BlogTitle = blog.BlogTitle;
                    existingBlog.BlogDate = blog.BlogDate;
                    existingBlog.BlogAutherName = blog.BlogAutherName;
                    existingBlog.BlogDescription = blog.BlogDescription;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Blog updated successfully!";
                    return RedirectToAction("BlogList");
                }
            }
            return View(blog);
        }

        // POST: Delete Blog
        [HttpPost]
        public ActionResult DeleteBlog(int blogId)
        {
            var blog = db.blogModels.Find(blogId);
            if (blog != null)
            {
                db.blogModels.Remove(blog);
                db.SaveChanges();
                TempData["SuccessMessage"] = "Blog deleted successfully!";
            }
            return RedirectToAction("BlogList");
        }
    }
}
