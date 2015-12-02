using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace WX.Core
{
    /// <summary>
    /// 接收微信消息
    /// </summary>
    public class ReceiveMessage
    {

        /// <summary>
        /// 处理接收微信消息
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <returns></returns>
        public static string HandleWXMessage(XDocument xmlDoc)
        {
            string str = "";
            try
            {
                WXReceiveMessageModel wXMessage = GetWXMessage(xmlDoc);
                if (wXMessage.MsgType != "event")
                {
                    return str;
                }
                if (wXMessage.Event == "subscribe")
                {
                    string content = "谢谢你关注物盟盟测试公众号！";
                    str = SendTextMessage(wXMessage, content);
                }
                if ((wXMessage.Event == "CLICK") && (wXMessage.EventKey == "MENU_GOOD"))
                {
                    string str3 = "谢谢你的赞";
                    str = SendTextMessage(wXMessage, str3);
                }
            }
            catch (Exception exception)
            {
                WXLog.WriteLog("异常信息：" + exception.Message);
            }
            return str;
        }

        /// <summary>
        /// 获取微信消息
        /// </summary>
        /// <param name="xmlDoc"></param>
        public static WXReceiveMessageModel GetWXMessage(XDocument xmlDoc)
        {
            var message = (from m in xmlDoc.Descendants()
                           select new WXReceiveMessageModel
                           {
                               ToUserName = m.Element("ToUserName").Value,
                               FromUserName = m.Element("FromUserName").Value,
                               CreateTime = m.Element("CreateTime").Value,
                               MsgType = m.Element("MsgType").Value,
                               Event = m.Element("Event").Value
                           }).FirstOrDefault();

            return message;
        }

        /// <summary>
        /// 发送文本消息
        /// </summary>
        /// <param name="message"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static string SendTextMessage(WXReceiveMessageModel message, string content)
        {
            string textMsg = string.Format("<xml><ToUserName><![CDATA[{0}]]></ToUserName><FromUserName><![CDATA[{1}]]></FromUserName><CreateTime>{2}</CreateTime><MsgType><![CDATA[{3}]]></MsgType><Content><![CDATA[{4}]]></Content></xml>", message.FromUserName, message.ToUserName, DateTime.Now.Ticks, "text", content);

            return textMsg;
        }
    }
}
