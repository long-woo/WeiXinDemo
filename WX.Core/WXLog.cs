using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WX.Core
{
    public class WXLog
    {
        public static void WriteLog(string text)
        {
            string htmlTemp = string.Format("=========日志信息========\r", text); //string.Format("<!DOCTYPE html><html lang=\"zh-cn\"><head><meta charset=\"utf-8\"/><title>微信日志记录</title></head><body>{0}</body></html>", text);
            const string foldName = "wxlog";
            string targetName = DateTime.Now.ToString("yyyyMMdd");
            string fileName = "log";//DateTime.Now.ToString("yyyyMMddHHmmss");
            string logPath = HttpContext.Current.Server.MapPath(string.Format("~/{0}/{1}", foldName, targetName));
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            if (!File.Exists(logPath + "/" + fileName + ".txt"))
            {
                File.CreateText(logPath + "/" + fileName + ".txt").Close();
            }
            using (StreamWriter writer = new StreamWriter(logPath + "/" + fileName + ".txt", false, Encoding.UTF8))
            {
                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                writer.Write(htmlTemp);
            }
        }
    }
}
