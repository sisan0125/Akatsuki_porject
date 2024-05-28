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
    public class POMController : Controller
    {
        WMSEntities2 db = new WMSEntities2();

        public ActionResult Index(int? page)
        {
            int pageSize = 20;
            int pageNumber = (page ?? 1);
            var poms = db.POM.OrderByDescending(m => m.Date)
                             .ThenByDescending(m => m.POID)
                             .ToList();

            foreach (var pom in poms)
            {
                pom.IsCompleted = IsPOIDCompleted(pom.POID);
            }

            return View(poms.ToPagedList(pageNumber, pageSize));
        }

        public bool IsPOIDCompleted(string poid)
        {
            return db.Import.Any(i => i.POID == poid);
        }

        public ActionResult Create(string PID, decimal? Price, decimal? Qty)
        {
            ViewBag.PID = PID;
            ViewBag.Price = Price;
            ViewBag.Qty = Qty;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string POID, string SUID, string Date, string PMID, string Paydate, string TAMT, string bDate, string PID, decimal? Price, decimal? Qty)
        {
            if (string.IsNullOrEmpty(POID) || string.IsNullOrEmpty(PID) || Price == null || Qty == null)
            {
                ModelState.AddModelError("", "請提供必要的參數");
                return View();
            }

            bool isPOIDExists = db.POM.Any(m => m.POID == POID);
            if (isPOIDExists)
            {
                ModelState.AddModelError("POID", "採購單編號已存在");
                return View();
            }

            POM pom = new POM
            {
                POID = POID,
                SUID = SUID,
                Date = Convert.ToDateTime(Date),
                PMID = PMID,
                Paydate = Convert.ToDateTime(Paydate),
                bDate = DateTime.Now,
                TAMT = Price.Value * Qty.Value
            };

            POD pod = new POD
            {
                POID = pom.POID,
                PID = PID,
                Price = Price.Value,
                Qty = Qty.Value,
                bDate = DateTime.Now
            };

            db.POM.Add(pom);
            db.POD.Add(pod);
            db.SaveChanges();

            // 生成QR Code
            string qrCodeData = $"{POID},{SUID},{Date},{PMID},{Paydate},{PID},{Price},{Qty}";
            GenerateQRCode(qrCodeData, POID);

            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var pom = db.POM.Find(id);

            if (pom == null)
            {
                return HttpNotFound();
            }

            if (pom != null)
            {
                var podList = db.POD.Where(p => p.POID == pom.POID).ToList();
                foreach (var podItem in podList)
                {
                    db.POD.Remove(podItem);
                }
                db.POM.Remove(pom);
                db.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        public JsonResult IsPOIDExists(string POID)
        {
            bool isExists = db.POM.Any(m => m.POID == POID);
            return Json(isExists, JsonRequestBehavior.AllowGet);
        }

        private void GenerateQRCode(string data, string POID)
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
            var qrCode = writer.Write(data);

            string qrCodeFileName = $"POM{POID}.png";
            string qrCodePath = Path.Combine(Server.MapPath("~/QRCode"), qrCodeFileName);
            qrCode.Save(qrCodePath);
        }
    }
}