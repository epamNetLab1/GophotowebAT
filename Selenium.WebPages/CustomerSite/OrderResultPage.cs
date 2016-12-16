using Selenium.Utilities.WebElement;
using Selenium.WebPages;
using System;

namespace GophotowebAT.CustomerSite.Selenium.WebPages
{
    public class OrderResultPage : PageBase
    {
        public static readonly WebElement LabelMessage = new WebElement().ByXPath(@"//div[contains(@class, 'shop-order-title')]/p");
        
        public void Method(string date)
        {
            
        }
    }
}
