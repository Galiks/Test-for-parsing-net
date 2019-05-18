using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForParsing
{
    class RestSharpCash4Brands
    {
        public void Parsing()
        {
            var client = new RestClient("https://cash4brands.ru/cashback/");
            var request = new RestRequest(Method.POST);
            #region пример с добавление headers через Dictionary
            //Dictionary<String, String> headers = new Dictionary<string, string>()
            //{
            //    { "cache-control", "no-cache"},
            //    { "Connection", "keep-alive"},
            //    { "content-length", "21"},
            //    { "accept-encoding", "gzip, deflate"},
            //    { "cookie", "Cookie_1=value; cu_uuid=3e592ff0-4d01-4f19-9d26-7c3fd24963e4; sessionid=9wq8qalx2xjkyte29xiohch4edgl0a7u"},
            //    { "Host", "cash4brands.ru"},
            //    { "Postman-Token", "91751f4f-c0e5-48c2-9137-cbcbc5e11a0f,1e684343-5161-4efc-8482-495ef99d971b"},
            //    { "Cache-Control", "no-cache" },
            //    { "Accept", "*/*"},
            //    { "User-Agent", "PostmanRuntime/7.13.0"},
            //    { "Content-Type", "application/x-www-form-urlencoded"}
            //};
            //foreach (var item in headers)
            //{
            //    request.AddHeader(item.Key, item.Value);
            //} 
            #endregion
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("content-length", "21");
            request.AddHeader("accept-encoding", "gzip, deflate");
            request.AddHeader("cookie", "Cookie_1=value; cu_uuid=3e592ff0-4d01-4f19-9d26-7c3fd24963e4; sessionid=9wq8qalx2xjkyte29xiohch4edgl0a7u");
            request.AddHeader("Host", "cash4brands.ru");
            request.AddHeader("Postman-Token", "91751f4f-c0e5-48c2-9137-cbcbc5e11a0f,1e684343-5161-4efc-8482-495ef99d971b");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.13.0");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("undefined", "namerequest=show_page", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            JObject jsonParse = JObject.Parse(response.Content);
            var listOfItems = jsonParse["shops"];
            foreach (var item in listOfItems)
            {
                Console.WriteLine(item);
            }
        }
    }
}
