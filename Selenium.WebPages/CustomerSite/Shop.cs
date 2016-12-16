using Selenium.Utilities.WebElement;
using Selenium.WebPages;
using System;

namespace GophotowebAT.CustomerSite.Selenium.WebPages
{
    public class Shop : PageBase
    {
        public static readonly WebElement LinkProduct = new WebElement().ByXPath(@"//a[contains(@class, 'product')]");
        public static readonly WebElement LabelPrice = new WebElement().ByXPath(@"//div/span[@class='product-price-min']");
                
        internal double GetPriceProduct()
        {
            var price = Tools.GetPriceFromText(LabelPrice.Text);
            return price;
        }
    }
}
