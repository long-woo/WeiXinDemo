using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WX.Core;

namespace WX.Web.Controllers
{
    public class WeiXinController : Controller
    {

        private static readonly string TOKEN = WebConfigurationManager.AppSettings["WXTOKEN"];
        private static readonly string ENCODINGAESKEY = WebConfigurationManager.AppSettings["WXENCODINGAESKEY"];
        private static readonly string APPID = WebConfigurationManager.AppSettings["WXAPPID"];

        // GET: WeiXin
        public Task<ActionResult> Index(string signature, string timestamp, string nonce, string echostr)
        {
            return Task.Factory.StartNew(() =>
            {
                if (CheckSignature.ValidateSignature(signature, timestamp, nonce, echostr, TOKEN))
                {
                    return echostr;
                }
                return "接入微信失败";
            }).ContinueWith<ActionResult>(t => Content(t.Result));
        }

        //public ActionResult Index(string signature, string timestamp, string nonce, string echostr)
        //{
        //    if (CheckSignature.ValidateSignature(signature, timestamp, nonce, echostr, TOKEN))
        //    {
        //        return Content(echostr);
        //    }
        //    return Content("接入微信失败");
        //}

        //[HttpPost]
        //public Task<ActionResult> Index()
    }
}