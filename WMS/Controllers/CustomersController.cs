using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Controllers
{
    public class CustomersController : Controller
    {
        WMSEntities2 db = new WMSEntities2();
        public ActionResult Index()
        {
            var todos = db.Customers.OrderByDescending(m => m.bDate).ToList();
            return View(todos);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(string CUID, string Name, string Abbreviation, string Address, string ReceiptAddress, string Phone, string VAT, string CP, string CPphone, string PIC, DateTime bDate)
        {
            Customers customers = new Customers();
            customers.CUID = CUID;
            customers.Name = Name;
            customers.Abbreviation = Abbreviation;
            customers.Address = Address;
            customers.ReceiptAddress = ReceiptAddress;
            customers.Phone = Phone;
            customers.VAT = VAT;
            customers.CP = CP;
            customers.CPphone = CPphone;
            customers.PIC = PIC;
            customers.bDate = bDate;
            db.Customers.Add(customers);
            db.SaveChanges();
            return RedirectToAction("index");

        }
        public ActionResult Delete(string id)
        {
            var todo = db.Customers.Where(m => m.CUID == id).FirstOrDefault();
            db.Customers.Remove(todo);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Edit(string id)
        {
            var todo = db.Customers.Where(m => m.CUID == id).FirstOrDefault();
            return View(todo);
        }
        [HttpPost]
        public ActionResult Edit(string CUID, string Name, string Abbreviation, string Address, string ReceiptAddress, string Phone, string VAT, string CP, string CPphone, string PIC, DateTime bDate)
        {


            var customer = db.Customers.FirstOrDefault(m => m.CUID == CUID);

            if (customer != null)
            {
                // Update customer properties
                customer.Name = Name;
                customer.Abbreviation = Abbreviation;
                customer.Address = Address;
                customer.ReceiptAddress = ReceiptAddress;
                customer.Phone = Phone;
                customer.VAT = VAT;
                customer.CP = CP;
                customer.CPphone = CPphone;
                customer.PIC = PIC;
                customer.bDate = bDate;

                // Optionally, check entity state before saving changes
                // var entry = db.Entry(customer);
                // Console.WriteLine($"Entity State: {entry.State}");

                // Save changes
                db.SaveChanges();
                db.Customers.Add(customer);
                return RedirectToAction("Index");
            }

            return View("Error");


        }


    }
}


