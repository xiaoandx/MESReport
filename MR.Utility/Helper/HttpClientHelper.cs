using ICSharpCode.SharpZipLib.GZip;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace MR.Utility.Helper
{
    public class HttpClientHelper
    {
        public HttpClientHelper()
        {

        }

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="json">发送的参数字符串，只能用json</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> PostAsyncJson(string url, string json)
        {
            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }

        /// <summary>
        /// 使用post方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <param name="data">发送的参数字符串</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> PostAsync(string url, string data, Dictionary<string, string> header, bool Gzip = false)
        {
            HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
            HttpContent content = new StringContent(data);
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            HttpResponseMessage response = await client.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = "";
            if (Gzip)
            {
                GZipInputStream inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
                responseBody = new StreamReader(inputStream).ReadToEnd();
            }
            else
            {
                responseBody = await response.Content.ReadAsStringAsync();

            }
            return responseBody;
        }

        /// <summary>
        /// 使用get方法异步请求
        /// </summary>
        /// <param name="url">目标链接</param>
        /// <returns>返回的字符串</returns>
        public static async Task<string> GetAsync(string url, Dictionary<string, string> header, bool Gzip = false)
        {

            HttpClient client = new HttpClient(new HttpClientHandler() { UseCookies = false });
            if (header != null)
            {
                client.DefaultRequestHeaders.Clear();
                foreach (var item in header)
                {
                    client.DefaultRequestHeaders.Add(item.Key, item.Value);
                }
            }
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();//用来抛异常的
            string responseBody = "";
            if (Gzip)
            {
                GZipInputStream inputStream = new GZipInputStream(await response.Content.ReadAsStreamAsync());
                responseBody = new StreamReader(inputStream).ReadToEnd();
            }
            else
            {
                responseBody = await response.Content.ReadAsStringAsync();

            }
            return responseBody;
        }

        /// <summary>
        /// 用于SMS专用提交发送短信
        /// </summary>
        /// <param name="url">URL</param>
        /// <param name="bData">bData</param>
        /// <returns>返回的字符串</returns>
        public static string Post(string url, byte[] bData)
        {
            string strResult = string.Empty;

#pragma warning disable SYSLIB0014 // 类型或成员已过时
            System.Net.HttpWebRequest hwRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
#pragma warning restore SYSLIB0014 // 类型或成员已过时
            hwRequest.Timeout = 5000;
            hwRequest.Method = "POST";
            hwRequest.ContentType = "application/x-www-form-urlencoded";
            hwRequest.ContentLength = bData.Length;

            System.IO.Stream smWrite = hwRequest.GetRequestStream();
            smWrite.Write(bData, 0, bData.Length);
            smWrite.Close();

            System.Net.HttpWebResponse hwResponse = (HttpWebResponse)hwRequest.GetResponse();
            StreamReader srReader = new StreamReader(hwResponse.GetResponseStream(), Encoding.ASCII);
            strResult = srReader.ReadToEnd();
            srReader.Close();
            hwResponse.Close();

            return strResult;
        }
    }
}

