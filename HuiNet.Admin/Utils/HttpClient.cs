using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using RestSharp;

namespace HuiNet.Admin.Utils
{
    public class HttpClient
    {
        public static string Post(string url, string command, Dictionary<string, object> parameters, string userId = "")
        {
            var request = CreateRestRequest(command, parameters, userId);
            var client = new RestClient(url)
            {
                Proxy = null,
                CookieContainer = null,
                FollowRedirects = false,
                Timeout = 60000
            };

            return client.Execute(request).Content;
        }

        public static async Task<string> PostAsync(string url, string command, Dictionary<string, object> parameters, string userId = "")
        {
            var request = CreateRestRequest(command, parameters, userId);
            var client = new RestClient(url)
            {
                Proxy = null,
                CookieContainer = null,
                FollowRedirects = false,
                Timeout = 60000
            };

            var respose = await client.ExecuteTaskAsync<string>(request);
            return respose.Content;
        }

        public static T Post<T>(string url, string command, Dictionary<string, object> parameters, string userId = "")
        {
            string content = Post(url, command, parameters, userId);
            return JsonConvert.DeserializeObject<T>(content);
        }

        public static async Task<T> PostAsync<T>(string url, string command, Dictionary<string, object> parameters, string userId = "")
        {
            string content = await PostAsync(url, command, parameters, userId);
            return await Task.Run<T>(() => JsonConvert.DeserializeObject<T>(content));
        }

        public static RestRequest CreateRestRequest(string command, Dictionary<string, object> parameters, string userId = "")
        {
            //- Head
            dynamic head = new ExpandoObject();
            head.SerialNumber = Guid.NewGuid().ToString();
            head.RequestHost = GetRemoteHost(HttpContext.Current.Request);
            head.RequestTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            head.Command = command;
            head.Channel = "mweb";
            head.UserId = userId;
            head.UseCache = true;

            string postHead = Base64UrlEncoder.Encode(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(head)));

            //- body
            string bodyStr = JsonConvert.SerializeObject(parameters);
            var bodyStrBits = Encoding.UTF8.GetBytes(bodyStr);
            string postBody = Base64UrlEncoder.Encode(bodyStrBits);
            string encryptBody = EncryptHelper.DESEncrypt(postBody, ConfigReader.RouteDesKey);

            var request = new RestRequest(Method.POST);
            request.AddHeader("head", postHead);
            request.AddParameter("body", encryptBody);
            return request;
        }

        /// <summary>
        /// 获取客户端IP
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetRemoteHost(HttpRequest request)
        {
            //优先取得代理IP 
            string IP = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(IP))
            {
                //没有代理IP则直接取连接客户端IP 
                IP = request.ServerVariables["REMOTE_ADDR"];
            }
            return IP;
        }
    }
}