using Fizzler.Systems.HtmlAgilityPack;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestForParsing
{
    class WebDriverAndFizzlerMegaBonusParsing
    {
        private const string Url = "https://megabonus.com/feed";

        public List<Shop> Parsing()
        {
            ConcurrentBag<Shop> shops = new ConcurrentBag<Shop>();
            IWebDriver driver = new ChromeDriver();
            try
            {
                driver.Navigate().GoToUrl(Url);
                var button = driver.FindElement(By.ClassName("see-more"));
                button.Click();
                while (button.Displayed)
                {
                    try
                    {
                        button.Click();
                        button = driver.FindElement(By.ClassName("see-more"));
                    }
                    catch (Exception e)
                    {
                        continue;
                    }
                }
                var ul = driver.FindElement(By.ClassName("cacheback-block-list")).GetAttribute("outerHTML");
                var html = new HtmlDocument();
                html.LoadHtml(ul);
                var document = html.DocumentNode.ChildNodes;
                var webElements = document[0].SelectNodes("li");
                Parallel.ForEach(webElements, webElement =>
                {
                    var shop = ParseElements(webElement);
                    if (shop != null)
                    {
                        shops.Add(shop);
                    }
                });
                return shops.ToList();
            }
            finally
            {
                driver.Close();
            }
        }

        private Shop ParseElements(HtmlNode element)
        {
            String name = GetName(element);
            String fullDiscount = GetFullDiscount(element);
            Double discount = GetDiscount(fullDiscount);
            String label = GetLabel(fullDiscount);
            String url = GetUrl(element);
            String image = GetImage(element);
            if (name != null && !Double.IsNaN(discount) && label != null && url != null && image != null)
            {
                return new Shop(name, discount, label, image, url);
            }
            return null;
        }

        private String GetName(HtmlNode html)
        {
            try
            {
                return html.QuerySelector("div.holder-more > a").InnerText.Trim();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private String GetFullDiscount(HtmlNode html)
        {
            try
            {
                return html.QuerySelector("div.percent_cashback").InnerText.Trim();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private String GetUrl(HtmlNode html)
        {
            try
            {
                return Url + html.QuerySelector("div.activate-hover-block > div.holder-more > a").GetAttributeValue("href", "");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private String GetImage(HtmlNode html)
        {
            try
            {
                return html.QuerySelector("div.holder-img > a > img").GetAttributeValue("src", "");
            }
            catch (Exception e)
            {
                return null;
            }
        }

        private String GetLabel(String fullDiscount)
        {
            if (fullDiscount == null)
            {
                return null;
            }
            Regex regex = new Regex("[$%€]|руб|(р.)|cent|р|Р|RUB|USD|EUR|SEK|UAH|INR|BRL|GBP|CHF|PLN");
            Match matcher = regex.Match(fullDiscount);
            if (matcher.Success)
            {
                return matcher.Value;
            }
            return null;
        }

        private Double GetDiscount(String fullDiscount)
        {
            if (fullDiscount == null)
            {
                return Double.NaN;
            }
            Regex regex = new Regex("\\d+[.|,]*\\d*");
            String discount = "";
            Match matcher = regex.Match(fullDiscount);
            if (matcher.Success)
            {
                discount = matcher.Value;
            }
            if (Double.TryParse(discount.Replace('.', ','), out double result))
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
