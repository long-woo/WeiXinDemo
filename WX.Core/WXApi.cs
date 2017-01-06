using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Web.Configuration;
using System.Net.Http.Headers;
using System.Security.Cryptography;

namespace Loan.WebCore
{
    public class WXApi
    {

        /// <summary>
        /// 微信api token （调用接口凭证）
        /// </summary>
        public static string AccessToken { get; private set; }

        /// <summary>
        /// 微信api token失效时间，通常为2分钟
        /// </summary>
        public static int AccessTokenExpireTime { get; private set; }

        /// <summary>
        /// 微信api token的最后更新时间
        /// </summary>
        public static DateTime LastTime { get; private set; }

        /// <summary>
        /// js-api ticket
        /// </summary>
        public static string JSApiTicket { get; private set; }

        /// <summary>
        /// js-api ticket失效时间，通常为2分钟
        /// </summary>
        public static int JSApiTicketExpireTime { get; protected set; }

        /// <summary>
        /// js-api ticket的最后更新时间
        /// </summary>
        public static DateTime JSApiLastTime { get; protected set; }

        public WXApi(string appId, string appSecret)
        {
            DateTime expireTime = LastTime.AddSeconds(AccessTokenExpireTime);
            if (DateTime.Now >= expireTime || string.IsNullOrEmpty(AccessToken))
            {
                GetAccessToken(appId, appSecret);
            }
        }

        /// <summary>
        /// 获取js-api ticket
        /// </summary>
        /// <returns></returns>
        public async Task GetJSApiTicketAsync()
        {
            string url = string.Format("https://api.weixin.qq.com/cgi-bin/ticket/getticket?access_token={0}&type=jsapi", AccessToken),
                   result = await HttpHelpers.GetAsync(url);
            var json = JObject.Parse(result);

            if (json["errmsg"].ToString() == "ok")
            {
                JSApiTicket = json["ticket"].ToString();
                JSApiTicketExpireTime = Convert.ToInt32(json["expires_in"]);
                JSApiLastTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 获取js-api签名
        /// </summary>
        /// <param name="timestamp">时间戳</param>
        /// <param name="noncestr">随机字符串</param>
        /// <param name="jsapi_ticket">ticket</param>
        /// <param name="url">当前网页的URL，不包含#及其后面部分</param>
        /// <returns></returns>
        public static string GetJSApiSignature(string timestamp, string noncestr, string jsapi_ticket, string url)
        {
            var arrParam = new[] { jsapi_ticket, timestamp, noncestr, url }.OrderBy(p => p).ToArray(); // 参数排序后，转成数组
            string strArrParam = string.Join("&", arrParam);
            var sha1 = SHA1.Create();
            var btSha1 = sha1.ComputeHash(Encoding.UTF8.GetBytes(strArrParam));

            StringBuilder enText = new StringBuilder();
            foreach (var b in btSha1)
            {
                enText.AppendFormat("{0:x2}", b);
            }

            return enText.ToString();
        }

        /// <summary>
        /// 获取Access Token （同步）
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <param name="appSecret">应用密钥</param>
        /// <returns></returns>
        public void GetAccessToken(string appId, string appSecret)
        {
            string strUrl = string.Format("{0}?grant_type=client_credential&appid={1}&secret={2}", WebConfigurationManager.AppSettings["ACCESStOKEN"], appId, appSecret),
               result = HttpHelpers.GetSync(strUrl);

            if (!result.Contains("errcode"))
            {
                JObject jObj = JObject.Parse(result);
                AccessToken = jObj["access_token"].ToString();
                AccessTokenExpireTime = int.Parse(jObj["expires_in"].ToString());
                LastTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 获取Access Token 异步
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <param name="appSecret">应用密钥</param>
        /// <returns></returns>
        public async Task GetAccessTokenAsync(string appId, string appSecret)
        {
            string strUrl = string.Format("{0}?grant_type=client_credential&appid={1}&secret={2}", WebConfigurationManager.AppSettings["ACCESStOKEN"], appId, appSecret),
                    result = await HttpHelpers.GetAsync(strUrl);

            if (!result.Contains("errcode"))
            {
                JObject obj2 = JObject.Parse(result);
                AccessToken = obj2["access_token"].ToString();
                AccessTokenExpireTime = int.Parse(obj2["expires_in"].ToString());
                LastTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="postJson"></param>
        /// <returns></returns>
        public async Task<string> CreateMenuAsync(string postJson)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["CREATEMENU"], AccessToken),
                result = await HttpHelpers.PostAsync(strUrl, postJson);
            return result;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetMenuAsync()
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["LOADMENU"], AccessToken),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <returns></returns>
        public async Task<string> DeleteMenuAsync()
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["DELETEMENU"], AccessToken),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 设置所属行业
        /// </summary>
        /// <param name="postJson">
        /// {"industry_id1":"1","industry_id2":"4"}
        /// </param>
        /// <returns></returns>
        public async Task<string> SetIndustry(string postJson)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["SETINDUSTRY"], AccessToken),
                result = await HttpHelpers.PostAsync(strUrl, postJson);
            return result;
        }

        /// <summary>
        /// 获取设置的行业信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetIndustry()
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["GETINDUSTRY"], AccessToken),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 获得模板ID
        /// </summary>
        /// <param name="postJson">
        /// {"template_id_short":"TM00015"}
        /// </param>
        /// <returns></returns>
        public async Task<string> GetTemplateId(string postJson)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["GETTEMPLATEID"], AccessToken),
                result = await HttpHelpers.PostAsync(strUrl, postJson);
            return result;
        }

        /// <summary>
        /// 获取模板列表，非模板库的列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetPrivateTemplates()
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["GETPRIVATETEMPLATE"], AccessToken),
               result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="postJson"></param>
        /// <returns></returns>
        public async Task<string> DeletePrivateTemplate(string postJson)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["DeletePRIVATETEMPLATE"], AccessToken),
                result = await HttpHelpers.PostAsync(strUrl, postJson);
            return result;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="postJson"></param>
        /// <returns></returns>
        public async Task<string> SendTemplateMessageAsync(string postJson)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["SENDTEMPLATEMESSAGE"], AccessToken),
                result = await HttpHelpers.PostAsync(strUrl, postJson);
            return result;
        }

    }
}
