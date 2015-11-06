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
        public static void HandleWXMessage(XDocument xmlDoc)
        {
            var message = GetWXMessage(xmlDoc);
            WXLog.WriteLog("消息类型：" + message.MsgType + "事件：" + message.Event);
            if (message.MsgType == "event")
            {
                if (message.Event == "subscribe")
                {
                    string content = "欢迎关注Huba！";
                    string result = SendTextMessage(message, content);
                    HttpContext.Current.Response.Write(result);
                }
            }
            else
            {

            }
        }

        /// <summary>
        /// 获取微信消息
        /// </summary>
        /// <param name="xmlDoc"></param>
        public static WXReceiveMessageModel GetWXMessage(XDocument xmlDoc)
        {
            var message = (from m in xmlDoc.Element("xml").Elements()
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
