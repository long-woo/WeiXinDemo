namespace WX.Web.Controllers
{
    using System.Web.Mvc;
    using WX.Core;

    public class HomeController : Controller
    {
        public ActionResult About()
        {
            ((dynamic) base.ViewBag).Message = "Your application description page.";
            return base.View();
        }

        public ActionResult Contact()
        {
            ((dynamic) base.ViewBag).Message = "Your contact page.";
            return base.View();
        }

        public ActionResult Index()
        {
            WXLog.WriteLog("111111");
            return base.View();
        }
    }
}

