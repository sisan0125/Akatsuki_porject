using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Controllers
{
    public class ProductsController : Controller
    {
        WMSEntities2 db = new WMSEntities2();

        public ActionResult Index(string SelectedCategoryId)
        {
            var todos = db.Products.OrderByDescending(m => m.bDate).ToList();

            if (!string.IsNullOrEmpty(SelectedCategoryId))
            {
                if (SelectedCategoryId == "01" || SelectedCategoryId == "02" || SelectedCategoryId == "03" || SelectedCategoryId == "04")
                {
                    todos = todos.Where(p => p.Categories.CID == SelectedCategoryId).ToList();
                }
            }

            List<Categories> categories = db.Categories.ToList();
            ViewBag.CategoryList = new SelectList(categories, "CID", "Name");

            return View(todos);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(string CID, string PID, string Name, string Description, string Adescription, decimal Price, decimal Qty, decimal SafeQty, DateTime bDate)
        {
            Products product = new Products();
            product.CID = CID;
            product.PID = PID;
            product.Name = Name;
            product.Description = Description;
            product.Adescription = Adescription;
            product.Price = Price;
            product.Qty = Qty;
            product.SafeQty = SafeQty;
            product.bDate = bDate;
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            var todo = db.Products.Where(m => m.PID == id).FirstOrDefault();
            db.Products.Remove(todo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(string id)
        {
            var todo = db.Products.Where(m => m.PID == id).FirstOrDefault();
            return View(todo);
        }

        [HttpPost]
        public ActionResult Edit(string PID, string Name, string Description, string Adescription, decimal Price, decimal Qty, decimal SafeQty, DateTime bDate)
        {
            var todo = db.Products.FirstOrDefault(m => m.PID == PID);
            if (todo != null)
            {
                todo.Name = Name;
                todo.Description = Description;
                todo.Adescription = Adescription;
                todo.Price = Price;
                todo.Qty = Qty;
                todo.SafeQty = SafeQty;
                todo.bDate = bDate;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View("Error");
        }

        public ActionResult CreatePurchaseOrder(string id)
        {
            var product = db.Products.FirstOrDefault(p => p.PID == id);
            if (product == null)
            {
                return HttpNotFound();
            }

            TempData["ProductDataForPOM"] = product;
            return RedirectToAction("Create", "POM");
        }
    }
}
