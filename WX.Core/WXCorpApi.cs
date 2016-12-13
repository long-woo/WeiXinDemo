using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WX.Common;

namespace WX.Core
{
    public class WXCorpApi
    {
        /// <summary>
        /// 微信api token （调用接口凭证）
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

        public WXCorpApi(string corpId, string corpSecret)
        {
            DateTime expireTime = LastTime.AddSeconds(AccessTokenExpireTime);
            if (DateTime.Now >= expireTime || string.IsNullOrEmpty(AccessToken))
            {
                GetAccessToken(corpId, corpSecret);
            }
        }

        /// <summary>
        /// 获取Access Token （同步）
        /// </summary>
        /// <param name="corpId">企业Id</param>
        /// <param name="corpSecret">企业管理组的密钥</param>
        /// <returns></returns>
        public void GetAccessToken(string corpId, string corpSecret)
        {
            string strUrl = string.Format("{0}?corpid={1}&corpsecret={2}", "https://qyapi.weixin.qq.com/cgi-bin/gettoken", corpId, corpSecret),
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
        /// 获取Access Token （异步）
        /// </summary>
        /// <param name="corpId">企业Id</param>
        /// <param name="corpSecret">企业管理组的密钥</param>
        /// <returns></returns>
        public async Task GetAccessTokenAsync(string corpId, string corpSecret)
        {
            string strUrl = string.Format("{0}?corpid={1}&corpsecret={2}", "https://qyapi.weixin.qq.com/cgi-bin/gettoken", corpId, corpSecret),
                    result = await HttpHelpers.GetAsync(strUrl);

            if (!result.Contains("errcode"))
            {
                JObject jObj = JObject.Parse(result);
                AccessToken = jObj["access_token"].ToString();
                AccessTokenExpireTime = int.Parse(jObj["expires_in"].ToString());
                LastTime = DateTime.Now;
            }
        }

        /// <summary>
        /// 获取通讯录标签列表
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetTagListAsyc()
        {
            string strUrl = string.Format("{0}?access_token={1}", "https://qyapi.weixin.qq.com/cgi-bin/tag/list", AccessToken),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 获取标签成员
        /// </summary>
        /// <param name="tagId">标签ID</param>
        /// <returns></returns>
        public async Task<string> GetMemberListByTagAsync(string tagId)
        {
            string strUrl = string.Format("{0}?access_token={1}&tagid={2}", "https://qyapi.weixin.qq.com/cgi-bin/tag/get", AccessToken, tagId),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <param name="departId">部门Id</param>
        /// <returns></returns>
        public async Task<string> GetDepartmentListAsync(string departId = "1")
        {
            string strUrl = string.Format("{0}?access_token={1}&id={2}", "https://qyapi.weixin.qq.com/cgi-bin/department/list", AccessToken, departId),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 获取部门成员（详细）列表
        /// </summary>
        /// <param name="isFetchChild">是否递归获取子部门下面的成员</param>
        /// <param name="departId">部门id</param>
        /// <param name="status">0获取全部成员，1获取已关注成员列表，2获取禁用成员列表，4获取未关注成员列表。status可叠加，未填写则默认为4</param>
        /// <returns></returns>
        public async Task<string> GetMemberDetailListByDepartAsync(int isFetchChild, string departId = "1", int status = 4)
        {
            string strUrl = string.Format("{0}?access_token={1}&department_id={2}&fetch_child={3}&status={4}", "https://qyapi.weixin.qq.com/cgi-bin/user/list", AccessToken, departId, isFetchChild, status),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 获取成员信息
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task<string> GetMemberDetailAsync(string userId)
        {
            string strUrl = string.Format("{0}?access_token={1}&userid={2}", "https://qyapi.weixin.qq.com/cgi-bin/user/get", AccessToken, userId),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 用户授权
        /// </summary>
        /// <param name="code">通过成员授权获取到的code，每次成员授权带上的code将不一样，code只能使用一次，10分钟未被使用自动过期</param>
        /// <returns></returns>
        public async Task<string> GetUserAuthAsync(string code)
        {
            string strUrl = string.Format("{0}?access_token={1}&code={2}", "https://qyapi.weixin.qq.com/cgi-bin/user/getuserinfo", AccessToken, code),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }

        /// <summary>
        /// 发送消息：
        /// 消息型应用支持文本、图片、语音、视频、文件、图文等消息类型
        /// </summary>
        /// <param name="jsonMsg">消息内容为json格式</param>
        /// <returns>如果无权限或收件人不存在，则本次发送失败；如果未关注，发送仍然执行。两种情况下均返回无效的部分（注：由于userid不区分大小写，返回的列表都统一转为小写）。</returns>
        public async Task<string> SendMessageAsync(string jsonMsg)
        {
            string strUrl = string.Format("{0}?access_token={1}", "https://qyapi.weixin.qq.com/cgi-bin/message/send", AccessToken),
                result = await HttpHelpers.PostAsync(strUrl, jsonMsg);
            return result;
        }

        /// <summary>
        /// 获取企业号应用列表：
        /// 用于获取secret所在管理组内的应用概况
        /// </summary>
        /// <returns>返回管理组内应用的id及名称、头像等信息。</returns>
        public async Task<string> GetAppListAsync()
        {
            string strUrl = string.Format("{0}?access_token={1}", "https://qyapi.weixin.qq.com/cgi-bin/agent/list", AccessToken),
                result = await HttpHelpers.GetAsync(strUrl);
            return result;
        }
    }
}
