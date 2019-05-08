using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForParsing
{
    class RestSharpParsing : IParsing
    {
        public List<Shop> Parsing()
        {
            string url = $"https://d289b99uqa0t82.cloudfront.net/sites/5/campaigns_limit_100_offset_0_order_popularity.json";
            var client = new RestClient(url);
            var request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Connection", "keep-alive");
            request.AddHeader("accept-encoding", "gzip, deflate");
            request.AddHeader("Host", "d289b99uqa0t82.cloudfront.net");
            request.AddHeader("Postman-Token", "048aef15-143b-4f61-8c44-60467f64a33d,e85413f5-28a6-4878-b792-942c640071cc");
            request.AddHeader("Cache-Control", "no-cache");
            request.AddHeader("Accept", "*/*");
            request.AddHeader("User-Agent", "PostmanRuntime/7.11.0");
            IRestResponse response = client.Execute(request);
            Data data = JsonConvert.DeserializeObject<Data>(response.Content);
            foreach (var item in data.Items)
            {
                Console.WriteLine(item);
            }

            return null;
        }
    }

    class Data
    {
        public Item[] Items;
    }

    class Item
    {
        public string Title;
        public string Url;
        public object Image;
        public object Commission;

        public override string ToString()
        {
            return $"{Title} {Url}\n{Image}\n{Commission}";
        }
    }
}