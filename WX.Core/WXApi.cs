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

        public static string AccessToken { get; set; }

        public static int AccessTokenExpireTime { get; set; }

        public static DateTime LastTime { get; set; }

        /// <summary>
        /// 获取Access Token
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <param name="appSecret">应用密钥</param>
        /// <returns></returns>
        public async static Task GetAccessTokenAsync(string appId, string appSecret)
        {
            HttpClient httpClient = new HttpClient();
            string uriString = string.Format(WebConfigurationManager.AppSettings["ACCESStOKEN"] + "?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret);
            Uri requestUri = new Uri(uriString);
            HttpResponseMessage response = await httpClient.GetAsync(requestUri);
            response.EnsureSuccessStatusCode();
            string json = await response.Content.ReadAsStringAsync();
            if (!json.Contains("errcode"))
            {
                JObject obj2 = JObject.Parse(json);
                AccessToken = obj2["access_token"].ToString();
                AccessTokenExpireTime = int.Parse(obj2["expires_in"].ToString());
                LastTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 刷新 access token 
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public async static Task RefreshAccessToken(string appId, string appSecret)
        {
            DateTime expireTime = LastTime.AddSeconds((double)AccessTokenExpireTime);
            if (DateTime.Now >= expireTime)
            {
                await GetAccessTokenAsync(appId, appSecret);
            }
        }

        /// <summary>
        /// 创建菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="postJson"></param>
        /// <returns></returns>
        public static async Task<string> CreateMenuAsync(string accessToken, string postJson)
        {
            HttpClient httpClient = new HttpClient();

            string strUrl = string.Format(WebConfigurationManager.AppSettings["CREATEMENU"] + "?access_token={0}", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var strUri = new Uri(strUrl);
            HttpContent postContent = new StringContent(postJson);
            HttpResponseMessage response = await httpClient.PostAsync(strUri, postContent);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static async Task<string> GetMenuAsync(string accessToken)
        {
            HttpClient httpClient = new HttpClient();

            string strUrl = string.Format(WebConfigurationManager.AppSettings["LOADMENU"] + "?access_token={0}", accessToken);
            var strUri = new Uri(strUrl);
            HttpResponseMessage response = await httpClient.GetAsync(strUri);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static async Task<string> DeleteMenuAsync(string accessToken)
        {
            HttpClient httpClient = new HttpClient();

            string strUrl = string.Format(WebConfigurationManager.AppSettings["DELETEMENU"] + "?access_token={0}", accessToken);
            var strUri = new Uri(strUrl);
            HttpResponseMessage response = await httpClient.GetAsync(strUri);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

        /// <summary>
        /// 获取模板消息Id
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="postJson"></param>
        /// <returns></returns>
        public async static Task<string> GetMessageTemplateId(string accessToken, string postJson)
        {
            HttpClient httpClient = new HttpClient();
            string uriString = string.Format(WebConfigurationManager.AppSettings["GETMESSAGETEMPLATEID"] + "?access_token={0}", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Uri requestUri = new Uri(uriString);
            HttpContent content = new StringContent(postJson);
            HttpResponseMessage response = await httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

        /// <summary>
        /// 发送模板消息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="postJson"></param>
        /// <returns></returns>
        public async static Task<string> SendTemplateMessageAsync(string accessToken, string postJson)
        {
            HttpClient httpClient = new HttpClient();

            string uriString = string.Format(WebConfigurationManager.AppSettings["SENDTEMPLATEMESSAGE"] + "?access_token={0}", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Uri requestUri = new Uri(uriString);
            HttpContent content = new StringContent(postJson);
            HttpResponseMessage response = await httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

        public async static Task<string> SetMessageTemplate(string accessToken, string postJson)
        {
            HttpClient httpClient = new HttpClient();

            string uriString = string.Format(WebConfigurationManager.AppSettings["SETMESSAGETEMPLATE"] + "?access_token={0}", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            Uri requestUri = new Uri(uriString);
            HttpContent content = new StringContent(postJson);
            HttpResponseMessage response = await httpClient.PostAsync(requestUri, content);
            response.EnsureSuccessStatusCode();
            string result = await response.Content.ReadAsStringAsync();

            return result;
        }

    }
}
