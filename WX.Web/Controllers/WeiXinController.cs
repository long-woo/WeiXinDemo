using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Linq;
using WX.Core;

namespace WX.Web.Controllers
{
    public class WeiXinController : Controller
    {
        private static readonly string APPID = WebConfigurationManager.AppSettings["WXAPPID"];
        private static readonly string ENCODINGAESKEY = WebConfigurationManager.AppSettings["WXENCODINGAESKEY"];
        private static readonly string TOKEN = WebConfigurationManager.AppSettings["WXTOKEN"];

        [ActionName("Index")]
        public Task<ActionResult> Get(string signature, string timestamp, string nonce, string echostr)
        {
            return Task.Factory.StartNew<string>(delegate {
                if (CheckSignature.ValidateSignature(signature, timestamp, nonce, echostr, TOKEN))
                {
                    return echostr;
                }
                return "接入微信失败";
            }).ContinueWith<ActionResult>(t => base.Content(t.Result));
        }

        [HttpPost, ActionName("Index")]
        public Task<ActionResult> Post(string signature, string timestamp, string nonce, string echostr)
        {
            return Task.Factory.StartNew<string>(delegate {
                if (!CheckSignature.ValidateSignature(signature, timestamp, nonce, echostr, TOKEN))
                {
                    return "参数错误";
                }
                StreamReader textReader = new StreamReader(this.Request.InputStream, Encoding.UTF8);
                return ReceiveMessage.HandleWXMessage(XDocument.Load(textReader));
            }).ContinueWith<ActionResult>(t => base.Content(t.Result));
        }
    }
}

