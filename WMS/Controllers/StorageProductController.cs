using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Controllers
{
    public class StorageProductController : Controller
    {
        WMSEntities2 db = new WMSEntities2();
        public ActionResult Index(string searchString)
        {
            var storageProducts = db.StorageProduct.OrderBy(m => m.SID).ToList();

            if (!string.IsNullOrEmpty(searchString))
            {
                storageProducts = storageProducts
                    .Where(s => s.Products != null &&
                                (!string.IsNullOrEmpty(s.Storage.SID) && s.Storage.SID.Contains(searchString) ||
                                 !string.IsNullOrEmpty(s.Products.Name) && s.Products.Name.Contains(searchString)))
                    .ToList();
            }

            return View(storageProducts);
        }
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(string sId, string pId, decimal qty)
        {
            DateTime currentTime = DateTime.Now;

            // 顯示日期，格式為 yyyy/MM/dd
            string formattedDate = currentTime.ToString("yyyy/MM/dd");


            StorageProduct todo = new StorageProduct();
            todo.SID = sId;
            todo.PID = pId;
            todo.Qty = qty;
            db.StorageProduct.Add(todo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(string id)
        {
            var todo = db.StorageProduct.Where(m => m.SID == id).FirstOrDefault();
            db.StorageProduct.Remove(todo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string id)
        {
            var todo = db.StorageProduct.Where(m => m.SID == id).FirstOrDefault();
            return View(todo);
        }
        [HttpPost]
        public ActionResult Edit(string sId, string pId, decimal qty)
        {
            DateTime currentTime = DateTime.Now;

            // 顯示日期，格式為 yyyy/MM/dd
            string formattedDate = currentTime.ToString("yyyy/MM/dd");

            var todo = db.StorageProduct.Where(m => m.SID == sId).FirstOrDefault();

            todo.SID = sId;
            todo.PID = pId;
            todo.Qty = qty;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
