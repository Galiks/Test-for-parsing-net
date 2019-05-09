using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForParsing
{
    class Fizzler : IParsing
    {
        private const int defaultPageCount = 30;
        private const string addressOfSiteForMaxPage = "https://letyshops.com/shops?page=1";
        private const string addressOfSiteForParsing = "https://letyshops.com/shops?page=";
        private const string addressOfSite = "https://letyshops.com";

        private List<Shop> Shops { get; set; }

        public Fizzler()
        {
            Shops = new List<Shop>();
        }

        public List<Shop> Parsing()
        {
            Parallel.For(1, GetMaxPage() + 1, ParseElements);
            return Shops;
        }

        private void ParseElements(int i)
        {
            var htmlWeb = new HtmlWeb
            {
                OverrideEncoding = Encoding.UTF8
            };
            var document = htmlWeb.Load(addressOfSiteForParsing + i);
            var html = document.DocumentNode;
            var listOfShops = html.QuerySelectorAll("div.b-teaser > a");
            foreach (var item in listOfShops)
            {
                String name = GetName(item);
                Double discount = GetDiscount(item);
                String label = GetLabel(item);
                String url = GetUrl(item);
                String image = GetImage(item);
                if (name != null && !Double.IsNaN(discount) && label != null && url != null && image != null)
                {
                    Shops.Add(new Shop(name, discount, label, image, url));
                }
            }
        }

        private String GetName(HtmlNode html)
        {
            return html.QuerySelector("div.b-teaser__title").InnerText.Trim();
        }

        private String GetLabel(HtmlNode html)
        {
            var labelList = html.QuerySelectorAll("div.b-teaser__caption > div.b-teaser__cashback-rate > div > div > span.b-shop-teaser__label");
            return labelList.Last().InnerText.Trim();
        }

        private String GetUrl(HtmlNode html)
        {
            return html.GetAttributeValue("href", "");
        }

        private String GetImage(HtmlNode html)
        {
            return html.QuerySelector("div.b-teaser__top > div.b-teaser__cover > img").GetAttributeValue("src", "");
        }

        private Double GetDiscount(HtmlNode html)
        {
            string discount = "";
            try
            {
                discount = html.QuerySelector("div.b-teaser__caption > div.b-teaser__cashback-rate > div > div > span.b-shop-teaser__cash").InnerText.Trim();
            }
            catch (NullReferenceException e)
            {
                discount = html.QuerySelector("div.b-teaser__caption > div.b-teaser__cashback-rate > div > div > span.b-shop-teaser__new-cash").InnerText.Trim(); ;
            }
            if (Double.TryParse(discount.Replace('.',','), out double result))
            {
                return result;
            }
            return Double.NaN;
        }

        private Int32 GetMaxPage()
        {
            var htmlWeb = new HtmlWeb
            {
                OverrideEncoding = Encoding.UTF8
            };
            var document = htmlWeb.Load("https://letyshops.com/shops?page=1");
            var html = document.DocumentNode;
            string maxPage = html.QuerySelector("div.b-content.b-content--shops > ul > li:nth-child(5) > a").InnerText.Trim();
            if (Int32.TryParse(maxPage, out int result))
            {
                return result;
            }
            return 30;
        }
    }
}
