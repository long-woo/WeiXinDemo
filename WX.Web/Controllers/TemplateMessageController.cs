using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using WX.Core;

namespace WX.Web.Controllers
{
    
    public class TemplateMessageController : Controller
    {
        private static readonly string APPID = WebConfigurationManager.AppSettings["WXAPPID"];
        private static readonly string APPSECRET = WebConfigurationManager.AppSettings["WXAPPSECRET"];
        private static readonly string TOKEN = WebConfigurationManager.AppSettings["WXTOKEN"];

        public ActionResult Index()
        {
            return base.View();
        }

        [ValidateAntiForgeryToken, HttpPost]
        public async Task<ActionResult> SendTemplateMessage(string jsonTPMessage)
        {
            await WXApi.RefreshAccessToken(APPID, APPSECRET);
            string content = await WXApi.SendTemplateMessageAsync(WXApi.AccessToken, jsonTPMessage);
            return this.Content(content);
        }

    }
}

