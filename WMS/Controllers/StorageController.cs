using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Controllers
{
    public class StoragesController : Controller
    {
        WMSEntities2 db = new WMSEntities2();
        public ActionResult Index()
        {
            var todos = db.Storage.OrderBy(m => m.SID).ToList();
            return View(todos);
        }
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(string sId, DateTime bDate)
        {
            DateTime currentTime = DateTime.Now;

            // 顯示日期，格式為 yyyy/MM/dd
            string formattedDate = currentTime.ToString("yyyy/MM/dd");


            Storage todo = new Storage();
            todo.SID = sId;
            todo.bDate = bDate;
            todo.bDate = DateTime.Parse(formattedDate);
            db.Storage.Add(todo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Delete(string id)
        {
            var todo = db.Storage.Where(m => m.SID == id).FirstOrDefault();
            db.Storage.Remove(todo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string id)
        {
            var todo = db.Storage.Where(m => m.SID == id).FirstOrDefault();
            return View(todo);
        }
        [HttpPost]
        public ActionResult Edit(string sId, DateTime bDate)
        {
            DateTime currentTime = DateTime.Now;

            // 顯示日期，格式為 yyyy/MM/dd
            string formattedDate = currentTime.ToString("yyyy/MM/dd");

            var todo = db.Storage.Where(m => m.SID == sId).FirstOrDefault();

            todo.bDate = bDate;
            todo.bDate = DateTime.Parse(formattedDate);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}