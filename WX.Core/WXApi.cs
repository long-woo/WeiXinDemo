using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Web.Configuration;

namespace WX.Core
{
    public class WXApi
    {
        /// <summary>
        /// 获取Access Token
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <param name="appSecret">应用密钥</param>
        /// <returns></returns>
        public static async Task<JObject> GetAccessTokenAsync(string appId, string appSecret)
        {
            HttpClient httpClient = new HttpClient();

            string strUrl = string.Format(WebConfigurationManager.AppSettings["ACCESStOKEN"] + "grant_type=client_credential&appid={0}&secret={1}", appId, appSecret);
            var strUri = new Uri(strUrl);
            string result = await httpClient.GetStringAsync(strUri);

            return JObject.Parse(result);
        }
    }
}
