using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WMS.Models;

namespace WMS.Controllers
{
    public class ShipController : Controller
    {
        WMSEntities2 db = new WMSEntities2();
        // GET: Ship
        public ActionResult Index()
        {
            var todos = db.Ship.OrderByDescending(m => m.bDate).ToList();
            return View(todos);

        }
    }
}
