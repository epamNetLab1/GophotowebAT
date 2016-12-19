using Selenium.Utilities;
using Selenium.Utilities.WebElement;
using Selenium.WebPages;
using System;

namespace GophotowebAT.CustomerSite.Selenium.WebPages
{
    public class ProductPage : PageBase
    {
        public static readonly WebElement SelectOptions = new WebElement().ByXPath(@"//div[@class='product-params']/div/select[@class='options']");
        public static readonly WebElement OptionSelectOptions = new WebElement().ByXPath(@"//div[@class='product-params']/div/select[@class='options']/option");
        public static readonly WebElement OptionDefaultSelectOptions = new WebElement().ByXPath(@"//div[@class='product-params']/div/select[@class='options']/option[@value='0']");
        public static readonly WebElement ButtonAddToCart = new WebElement().ByXPath(@"//a[@id='skuadd']");
        public static readonly WebElement LabelPrice = new WebElement().ByXPath(@"//div/span[@class='product-price-min']");

        public void ClickButtonAddToCart()
        {
            if (OptionDefaultSelectOptions.Exists() && OptionDefaultSelectOptions.Selected)
            {
                SelectOptions.Select(OptionSelectOptions.Last().Text);
            }
            ButtonAddToCart.Click();
            WaitHelper.SpinWait(() => ButtonAddToCart.Text.ToLower() == "добавлено", TimeSpan.FromSeconds(10), TimeSpan.FromMilliseconds(500));
        }

        public double GetPriceProduct()
        {
            var price = Tools.GetPriceFromText(LabelPrice.Text);
            return price;
        }
    }
}
