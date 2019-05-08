using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForParsing
{
    //1 секунда
    class RestSharpParsing : IParsing
    {

        public List<Shop> Shops { get; set; }

        public RestSharpParsing()
        {
            Shops = new List<Shop>();
        }

        public List<Shop> Parsing()
        {
            for (int i = 0; i <= 1300; i+=100)
            {
                string url = $"https://d289b99uqa0t82.cloudfront.net/sites/5/campaigns_limit_100_offset_{i}_order_popularity.json";
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
                JObject jsonParse = JObject.Parse(response.Content);
                var listOfItems = jsonParse["items"];
                foreach (var item in listOfItems)
                {
                    ParseElements(item);
                }
            }
            return Shops;
        }

        public List<Shop> AnotherParsing()
        {
            for (int i = 0; i <= 1300; i += 100)
            {
                string url = $"https://d289b99uqa0t82.cloudfront.net/sites/5/campaigns_limit_100_offset_{i}_order_popularity.json";
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
                JObject jsonParse = JObject.Parse(response.Content);
                var listOfItems = jsonParse["items"];
                Parallel.ForEach(listOfItems, ParseElements);
            }
            return Shops;
        }

        private void ParseElements(JToken jToken)
        {
            var name = GetName(jToken);
            var image = GetImage(jToken);
            var shopUrl = GetUrl(jToken);
            var discount = GetDiscount(jToken);
            var label = GetLabel(jToken);
            Shops.Add(new Shop(name, discount, label, image, shopUrl));
        }

        private String GetName(JToken token)
        {
            return token["title"].ToString();
        }

        private String GetImage(JToken token)
        {
            return token["image"]["url"].ToString();
        }

        private String GetUrl(JToken token)
        {
            return "https://www.kopikot.ru/stores/" + token["url"].ToString() + "/" + token["id"].ToString();
        }

        private Double GetDiscount(JToken token)
        {
            string discount = token["commission"]["max"]["original_amount"].ToString();
            if (double.TryParse(discount.Replace('.',','), out double result))
            {
                return result;
            }
            return Double.NaN;
        }

        private String GetLabel(JToken token)
        {
            return token["commission"]["max"]["unit"].ToString();
        }
    }
}