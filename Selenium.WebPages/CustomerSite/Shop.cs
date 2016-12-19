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

        internal double GetPriceProduct(int idProduct)
        {
            var price = Tools.GetPriceFromText(new WebElement()
                .ByXPath($@"//a[contains(@class, 'product') and div[img[contains(@src, '{idProduct}')]]]//div/span[@class='product-price-min']").Text);
            return price;
        }

        internal static void LinkProductClick(int idProduct)
        {
            new WebElement().ByXPath($@"//a[contains(@class, 'product') and div[img[contains(@src, '{idProduct}')]]]").Click();
        }
    }
}
