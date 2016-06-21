using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Web.Configuration;
using System.Net.Http.Headers;
using WX.Common;

namespace WX.Core
{
    public class WXApi
    {

        /// <summary>
        /// 微信api token
        /// </summary>
        public static string AccessToken { get; private set; }

        /// <summary>
        /// 微信api token 失效时间，通常为2分钟
        /// </summary>
        public static int AccessTokenExpireTime { get; private set; }

        /// <summary>
        /// 微信api token 的最后更新时间
        /// </summary>
        public static DateTime LastTime { get; private set; }

        /// <summary>
        /// 获取Access Token
        /// </summary>
        /// <param name="appId">应用Id</param>
        /// <param name="appSecret">应用密钥</param>
        /// <returns></returns>
        public async static Task GetAccessTokenAsync(string appId, string appSecret)
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
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["CREATEMENU"], accessToken),
                result = await HttpHelpers.PostAsync(strUrl, postJson);
            return result;
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static async Task<string> GetMenuAsync(string accessToken)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["LOADMENU"], accessToken),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 删除菜单
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public static async Task<string> DeleteMenuAsync(string accessToken)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["DELETEMENU"], accessToken),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 设置所属行业
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="postJson">
        /// {"industry_id1":"1","industry_id2":"4"}
        /// </param>
        /// <returns></returns>
        public async static Task<string> SetIndustry(string accessToken, string postJson)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["SETINDUSTRY"], accessToken),
                result = await HttpHelpers.PostAsync(strUrl, postJson);
            return result;
        }

        /// <summary>
        /// 获取设置的行业信息
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async static Task<string> GetIndustry(string accessToken)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["GETINDUSTRY"], accessToken),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 获得模板ID
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="postJson">
        /// {"template_id_short":"TM00015"}
        /// </param>
        /// <returns></returns>
        public async static Task<string> GetTemplateId(string accessToken, string postJson)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["GETTEMPLATEID"], accessToken),
                result = await HttpHelpers.PostAsync(strUrl, postJson);
            return result;
        }

        /// <summary>
        /// 获取模板列表，非模板库的列表
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        public async static Task<string> GetPrivateTemplates(string accessToken)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["GETPRIVATETEMPLATE"], accessToken),
               result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="postJson"></param>
        /// <returns></returns>
        public async static Task<string> DeletePrivateTemplate(string accessToken,string postJson)
        {
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["DeletePRIVATETEMPLATE"], accessToken),
                result = await HttpHelpers.PostAsync(strUrl, postJson);
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
            string strUrl = string.Format("{0}?access_token={1}", WebConfigurationManager.AppSettings["SENDTEMPLATEMESSAGE"], accessToken),
                result = await HttpHelpers.PostAsync(strUrl, postJson);
            return result;
        }

    }
}
