using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Xml.Linq;

namespace WXTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            XDocument xml = XDocument.Parse("<xml><ToUserName><![CDATA[toUser]]></ToUserName><FromUserName><![CDATA[FromUser]]></FromUserName><CreateTime>123456789</CreateTime><MsgType><![CDATA[event]]></MsgType><Event><![CDATA[subscribe]]></Event></xml>");
            //WX.Core.ReceiveMessage.GetWXMessage1(xml);
        }
    }
}
