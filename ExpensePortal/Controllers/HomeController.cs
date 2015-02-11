using System.Web.Mvc;

namespace ExpensePortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            
            return RedirectToAction("Index", "Expense");
            //return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    }
}