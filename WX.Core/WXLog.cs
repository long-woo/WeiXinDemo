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
        public void WriteLog(string text)
        {
            string htmlTemp="<!DOCTYPE html><html lang=\"zh-cn\"><head><meta charset=\"utf-8\"/><title>微信日志记录</title></head><body><ul></ul></body></html>";
            const string foldName = "wxlog";
            string targetName = DateTime.Now.ToString("yyyy-MM-dd");
            string fileName = DateTime.Now.ToString("yyyyMMddHH:mm:ss");
            string logPath = HttpContext.Current.Server.MapPath(string.Format("~/{0}/{1}", foldName, targetName));
            if (!Directory.Exists(logPath))
            {
                Directory.CreateDirectory(logPath);
            }

            if (!File.Exists(fileName + ".html"))
            {
                File.CreateText(fileName + ".html").Close();
            }
            using (StreamWriter writer = new StreamWriter(logPath + fileName + ".html", false, Encoding.UTF8))
            {
                writer.BaseStream.Seek(0, SeekOrigin.Begin);
                writer.Write(text);
            }
        }
    }
}
