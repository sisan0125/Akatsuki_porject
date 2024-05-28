using System;
using System.IO;
using System.Web.Mvc;
using ZXing;
using ZXing.QrCode;
using WMS.Models;
using System.Linq;
using System.Net;
using PagedList;

namespace WMS.Controllers
{
    public class SOMController : Controller
    {
        WMSEntities2 db = new WMSEntities2();

        public ActionResult Index(string SelectedCustomerId, int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var soms = db.SOM.OrderByDescending(m => m.Date)
                             .ThenByDescending(m => m.SOID)
                             .ToList();

            foreach (var som in soms)
            {
                som.IsCompleted = IsSOIDCompleted(som.SOID);
            }

            if (!string.IsNullOrEmpty(SelectedCustomerId))
            {
                soms = soms.Where(p => p.CUID == SelectedCustomerId).ToList();
            }

            var pagedSOMs = soms.ToPagedList(pageNumber, pageSize);
            var customers = db.Customers.ToList();
            ViewBag.CustomerList = new SelectList(customers, "CUID", "Name");
            ViewBag.CustomerName = Session["CustomerName"] != null ? Session["CustomerName"].ToString() : "管理員";
            return View(pagedSOMs);
        }

        public bool IsSOIDCompleted(string soid)
        {
            return db.Ship.Any(i => i.SOID == soid);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string SOID, string Date, string PMID, string EDL, string PID, decimal Price, decimal Qty, string SelectedCustomerId)
        {
            try
            {
                var CUID = (string)Session["CustomerID"];

                SOM som = new SOM
                {
                    SOID = SOID,
                    CUID = CUID,
                    Date = Convert.ToDateTime(Date),
                    PMID = PMID,
                    EDL = Convert.ToDateTime(EDL),
                    DLYaddress = "0",
                    bDate = DateTime.Now
                };

                SOD sod = new SOD
                {
                    SOID = som.SOID,
                    PID = PID,
                    Price = Price,
                    Qty = Qty,
                    bDate = DateTime.Now
                };

                db.SOM.Add(som);
                db.SOD.Add(sod);
                db.SaveChanges();

                // 生成 QR Code
                string qrCodeData = $"{SOID},{Date},{PMID},{EDL},{PID},{Price},{Qty}";
                GenerateQRCode(qrCodeData, SOID);

                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", new { SelectedCustomerId = CUID });
                }
                else
                {
                    return RedirectToAction("Index", new { SelectedCustomerId });
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "發生錯誤：" + ex.Message;
                return View();
            }
        }

        private void GenerateQRCode(string qrCodeData, string sOID)
        {
            BarcodeWriter writer = new BarcodeWriter
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new QrCodeEncodingOptions
                {
                    Width = 300,
                    Height = 300
                }
            };
            var qrCode = writer.Write(qrCodeData);

            string qrCodeFileName = $"SOM{sOID}.png";
            string qrCodePath = Path.Combine(Server.MapPath("~/QRCode"), qrCodeFileName);
            qrCode.Save(qrCodePath);
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var som = db.SOM.Find(id);

            if (som == null)
            {
                return HttpNotFound();
            }

            bool hasShippedItems = db.Ship.Any(s => s.SOID == id);

            if (hasShippedItems)
            {
                som.IsCompleted = true;
                db.SaveChanges();

                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", new { SelectedCustomerId = som.CUID });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
            else
            {
                var sodList = db.SOD.Where(p => p.SOID == som.SOID).ToList();
                foreach (var sodItem in sodList)
                {
                    db.SOD.Remove(sodItem);
                }
                db.SOM.Remove(som);
                db.SaveChanges();

                if (User.Identity.IsAuthenticated)
                {
                    return RedirectToAction("Index", new { SelectedCustomerId = som.CUID });
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }

        public JsonResult IsSOIDExists(string SOID)
        {
            bool isExists = db.SOM.Any(m => m.SOID == SOID);
            return Json(!isExists, JsonRequestBehavior.AllowGet);
        }
    }
}