using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WX.Common
{
    public class HttpHelpers
    {
        private static readonly HttpClient httpClient = new HttpClient();

        /// <summary>
        /// Http Get 请求 （同步）
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetSync(string url)
        {
            try
            {
                Task<HttpResponseMessage> taskResponse = null;
                Uri uri = new Uri(url);
                taskResponse = httpClient.GetAsync(uri);
                string result = taskResponse.Result.Content.ReadAsStringAsync().Result.ToString();
                return result;
            }
            catch (Exception ex)
            {
                return string.Format("错误：{0}！", ex.Message);
            }
        }

        /// <summary>
        /// Http Get 请求
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static async Task<string> GetAsync(string url)
        {
            try
            {
                Uri uri = new Uri(url);
                HttpResponseMessage response = await httpClient.GetAsync(uri);
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                return string.Format("错误：{0}！", ex.Message);
            }
        }

        /// <summary>
        /// Http Post 请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="strParams">json格式的参数</param>
        /// <returns></returns>
        public static async Task<string> PostAsync(string url, string strParams)
        {
            try
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Uri uri = new Uri(url);
                HttpContent content = new StringContent(strParams);
                content.Headers.ContentType.CharSet = "UTF-8";
                HttpResponseMessage response = await httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();
                content = response.Content;
                string result = await content.ReadAsStringAsync();
                return result;
            }
            catch (Exception ex)
            {
                return string.Format("错误：{0}！",ex.Message);
            }
        }
    }
}
