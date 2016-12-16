using Selenium.Utilities.WebElement;
using Selenium.WebPages;
using System;

namespace GophotowebAT.CustomerSite.Selenium.WebPages
{
    public class Navigate : PageBase
    {
        public static readonly WebElement LinkPortfolio = new WebElement().ByXPath(@"//li[contains(@class, 'menu-item')]/a[contains(@href, '/portfolio')]");
        public static readonly WebElement LinkVideo = new WebElement().ByXPath(@"//li[contains(@class, 'menu-item')]/a[contains(@href, '/video')]");
        public static readonly WebElement LinkAbout = new WebElement().ByXPath(@"//li[contains(@class, 'menu-item')]/a[contains(@href, '/about')]");
        public static readonly WebElement LinkContacts = new WebElement().ByXPath(@"//li[contains(@class, 'menu-item')]/a[contains(@href, '/contacts')]");
        public static readonly WebElement LinkShop = new WebElement().ByXPath(@"//li[contains(@class, 'menu-item')]/a[contains(@href, '/shop')]");
        public static readonly WebElement LinkBlog = new WebElement().ByXPath(@"//li[contains(@class, 'menu-item')]/a[contains(@href, '/blog')]");
        public static readonly WebElement LinkShopCart = new WebElement().ByXPath(@"//li[contains(@class, 'menu-item')]/a[@id='shop-cart-widget']");
        public static readonly WebElement LabelShopCartAmount = new WebElement().ByXPath(@"//li[contains(@class, 'menu-item')]/a[@id='shop-cart-widget']");

        internal int GetCountProduct()
        {
            return Convert.ToInt32(LabelShopCartAmount.Text);
        }
    }
}
