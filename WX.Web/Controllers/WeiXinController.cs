using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml;
using WX.Core;
using System.Xml.Linq;

namespace WX.Web.Controllers
{
    public class WeiXinController : Controller
    {

        private static readonly string TOKEN = WebConfigurationManager.AppSettings["WXTOKEN"];
        private static readonly string ENCODINGAESKEY = WebConfigurationManager.AppSettings["WXENCODINGAESKEY"];
        private static readonly string APPID = WebConfigurationManager.AppSettings["WXAPPID"];

        // GET: WeiXin
        [ActionName("Index")]
        public Task<ActionResult> Get(string signature, string timestamp, string nonce, string echostr)
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

        [HttpPost]
        [ActionName("Index")]
        public Task<ActionResult> Post(string signature, string timestamp, string nonce, string echostr)
        {
            return Task.Factory.StartNew(() =>
            {
                if (!CheckSignature.ValidateSignature(signature, timestamp, nonce, echostr, TOKEN))
                {
                    return "参数错误";
                }

                StreamReader stream = new StreamReader(Request.InputStream, System.Text.Encoding.UTF8);
                XDocument xmlDoc = XDocument.Load(stream);
                ReceiveMessage.HandleWXMessage(xmlDoc);

                return "Token验证失败";
            }).ContinueWith<ActionResult>(t => Content(t.Result));
        }
    }
}