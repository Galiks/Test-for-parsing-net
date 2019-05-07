using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestForParsing
{
    class WebDriverParsing : IParsing
    {
        public List<Shop> Parsing()
        {
            IWebDriver driver = new ChromeDriver();
            driver.Navigate().GoToUrl("https://letyshops.com/shops?page=1");
            return null;
        }
    }
}
