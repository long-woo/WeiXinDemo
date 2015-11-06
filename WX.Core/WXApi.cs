using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Web.Configuration;
using System.Net.Http.Headers;

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

            string strUrl = string.Format(WebConfigurationManager.AppSettings["ACCESStOKEN"] + "?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret);
            var strUri = new Uri(strUrl);
            HttpResponseMessage response = await httpClient.GetAsync(strUri);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return JObject.Parse(result);
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="postJson"></param>
        /// <returns></returns>
        public static async Task<JObject> CreateMenuAsync(string accessToken, string postJson)
        {
            HttpClient httpClient = new HttpClient();

            string strUrl = string.Format(WebConfigurationManager.AppSettings["CREATEMENU"] + "?access_token={0}", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var strUri = new Uri(strUrl);
            HttpContent postContent = new StringContent(postJson);
            HttpResponseMessage response = await httpClient.PostAsync(strUri, postContent);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return JObject.Parse(result);
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static async Task<JObject> GetMenuAsync(string accessToken)
        {
            HttpClient httpClient = new HttpClient();

            string strUrl = string.Format(WebConfigurationManager.AppSettings["LOADMENU"] + "?access_token={0}", accessToken);
            var strUri = new Uri(strUrl);
            HttpResponseMessage response = await httpClient.GetAsync(strUri);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return JObject.Parse(result);
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static async Task<JObject> DeleteMenuAsync(string accessToken)
        {
            HttpClient httpClient = new HttpClient();

            string strUrl = string.Format(WebConfigurationManager.AppSettings["DELETEMENU"] + "?access_token={0}", accessToken);
            var strUri = new Uri(strUrl);
            HttpResponseMessage response = await httpClient.GetAsync(strUri);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return JObject.Parse(result);
        }
    }
}
