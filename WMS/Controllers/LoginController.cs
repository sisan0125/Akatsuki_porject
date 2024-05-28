using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using WMS.Models;

namespace WMS.Controllers
{
    public class LoginController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string 帳號, string 密碼)
        {
            WMSEntities2 db = new WMSEntities2();
            var member = db.AccountPassword.Where(m => m.Username == 帳號 && m.Password == 密碼).FirstOrDefault();

            if (member != null)
            {
                FormsAuthentication.RedirectFromLoginPage(member.Username, true);
                var customer = db.Customers.FirstOrDefault(c => c.CUID == 帳號);
                if (customer != null)
                {
                    Session["CustomerName"] = customer.Name;
                    Session["CustomerID"] = customer.CUID;
                }
                if (User.IsInRole("Admin"))
                {
                    return RedirectToAction("Index", "Login");
                }
                else
                {
                    ViewBag.IsLogin = true;
                    ViewBag.ShowLinks = false;
                    return RedirectToAction("Index", "SOM", new { SelectedCustomerId = 帳號 });
                }
            }

            ViewBag.IsLogin = true;
            return View("Index");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Clear();
            return RedirectToAction("Index", "SOM");
        }

    }
}
