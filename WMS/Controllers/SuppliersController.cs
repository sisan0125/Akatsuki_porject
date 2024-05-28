using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;


namespace WMS.Controllers
{
    public class SuppliersController : Controller
    {
        WMSEntities2 db = new WMSEntities2();
        public ActionResult Index()
        {
            var todos = db.Suppliers.OrderByDescending(m => m.bDate).ToList();
            return View(todos);
        }
        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(string SUID, string Name, string Abbreviation, string Address, string Phone, string VAT, string CP, string CPphone, string PIC, DateTime bDate)
        {
            Suppliers todos = new Suppliers();
            todos.SUID = SUID;
            todos.Name = Name;
            todos.Abbreviation = Abbreviation;
            todos.Address = Address;
            todos.Phone = Phone;
            todos.VAT = VAT;
            todos.CP = CP;
            todos.CPphone = CPphone;
            todos.PIC = PIC;
            todos.bDate = bDate;
            db.Suppliers.Add(todos);
            db.SaveChanges();
            return RedirectToAction("index");
        }
        public ActionResult Delete(string id)
        {
            var todo = db.Suppliers.Where(m => m.SUID == id).FirstOrDefault();
            db.Suppliers.Remove(todo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string id)
        {
            var todo = db.Suppliers.Where(m => m.SUID == id).FirstOrDefault();
            return View(todo);
        }
        [HttpPost]
        public ActionResult Edit(string SUID, string Name, string Abbreviation, string Address, string Phone, string VAT, string CP, string CPphone, string PIC, DateTime bDate)
        {
            try
            {
                var todo = db.Suppliers.FirstOrDefault(m => m.SUID == SUID);

                todo.SUID = SUID;
                todo.Name = Name;
                todo.Abbreviation = Abbreviation;
                todo.Address = Address;
                todo.Phone = Phone;
                todo.VAT = VAT;
                todo.CP = CP;
                todo.CPphone = CPphone;
                todo.PIC = PIC;
                todo.bDate = bDate;

                db.SaveChanges();
                return RedirectToAction("index");
            }
            catch (DbEntityValidationException ex)
            {
                // 遍歷驗證錯誤，並記錄或處理它們
                foreach (var error in ex.EntityValidationErrors)
                {
                    foreach (var validationError in error.ValidationErrors)
                    {
                        ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                    }
                }

                // 返回編輯視圖，以便用戶可以看到錯誤
                var editedSupplier = new Suppliers
                {
                    SUID = SUID,
                    Name = Name,
                    Abbreviation = Abbreviation,
                    Address = Address,
                    Phone = Phone,
                    VAT = VAT,
                    CP = CP,
                    CPphone = CPphone,
                    PIC = PIC,
                    bDate = bDate
                };
                return View(editedSupplier);
            }
        }
    }
}