using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Controllers
{
    public class ImportController : Controller
    {
        WMSEntities2 db = new WMSEntities2();
        // GET: Import
        public ActionResult Index()
        {
            var todos = db.Import.OrderByDescending(m => m.bDate).ToList();
            return View(todos);

        }
    }
}
