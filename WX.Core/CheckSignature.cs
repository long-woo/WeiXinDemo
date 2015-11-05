using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace WX.Core
{
    public class CheckSignature
    {
        /// <summary>
        /// 验证服务器地址的有效性
        /// </summary>
        /// <param name="signature">微信加密签名</param>
        /// <param name="timestamp">时间戳</param>
        /// <param name="nonce">随机数</param>
        /// <param name="echostr">随机字符串</param>
        /// <param name="token">服务器配置Token</param>
        /// <returns></returns>
        public static bool ValidateSignature(string signature, string timestamp, string nonce, string echostr, string token)
        {
            return signature == GetSignature(timestamp, nonce, token);
        }

        /// <summary>
        /// 获取签名
        /// </summary>
        /// <param name="timestamp"></param>
        /// <param name="nonce"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public static string GetSignature(string timestamp, string nonce, string token)
        {
            var arrParam = new[] { token, timestamp, nonce }.OrderBy(p => p).ToArray();
            string strArrParam = string.Join("", arrParam);
            var sha1 = SHA1.Create();
            var btSha1 = sha1.ComputeHash(Encoding.UTF8.GetBytes(strArrParam));
            return Convert.ToBase64String(btSha1).ToUpper();

            //StringBuilder enText = new StringBuilder();
            //foreach (var b in btSha1)
            //{
            //    enText.AppendFormat("{0:x2}", b);
            //}

            //return enText.ToString();
        }
    }
}
