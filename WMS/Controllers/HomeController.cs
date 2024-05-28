using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult ContentsIndex()
        {
            return View();
        }

        // 添加对其他控制器的链接
        public ActionResult NavigateToCategories()
        {
            return RedirectToAction("Index", "Categories");
        }

        public ActionResult NavigateToCustomers()
        {
            return RedirectToAction("Index", "Customers");
        }

        public ActionResult NavigateToPayment()
        {
            return RedirectToAction("Index", "Payment");
        }

        public ActionResult NavigateToProducts()
        {
            return RedirectToAction("Index", "Products");
        }

        public ActionResult NavigateToStorage()
        {
            return RedirectToAction("Index", "Storages");
        }

        public ActionResult NavigateToStorageProduct()
        {
            return RedirectToAction("Index", "StorageProduct");
        }

        public ActionResult NavigateToSuppliers()
        {
            return RedirectToAction("Index", "Suppliers");
        }
        public ActionResult NavigateToSOM()
        {
            return RedirectToAction("Index", "SOM");
        }
        public ActionResult NavigateToPOM()
        {
            return RedirectToAction("Index", "POM");
        }
    }
}
