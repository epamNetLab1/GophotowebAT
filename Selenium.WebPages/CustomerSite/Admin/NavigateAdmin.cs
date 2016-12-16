using Selenium.Utilities.WebElement;
using Selenium.WebPages;
using System;

namespace GophotowebAT.CustomerSite.Selenium.WebPages
{
    public class NavigateAdmin : PageBase
    {
        public static readonly WebElement LinkSite = new WebElement().ByXPath(@"//li/a[contains(@href, '/admin/site')]");
        public static readonly WebElement LinkShop = new WebElement().ByXPath(@"//li/a[contains(@href, '/admin/shop')]");
        public static readonly WebElement LinkDesign = new WebElement().ByXPath(@"//li/a[contains(@href, '/admin/design')]");
        public static readonly WebElement LinkSettings = new WebElement().ByXPath(@"//li/a[contains(@href, '/admin/settings')]");
        public static readonly WebElement LinkStatistics = new WebElement().ByXPath(@"//li/a[contains(@href, '/admin/statistics')]");

        public static readonly WebElement LinkShopProducts = new WebElement().ByXPath(@"//li/a[contains(@href, '/admin/shop/products')]");
        public static readonly WebElement LinkShopOrders = new WebElement().ByXPath(@"//li/a[contains(@href, '/admin/shop/orders')]");
        public static readonly WebElement LinkShopCategories = new WebElement().ByXPath(@"//li/a[contains(@href, '/admin/shop/categories')]");
        public static readonly WebElement LinkShopPromocodes = new WebElement().ByXPath(@"//li/a[contains(@href, '/admin/shop/promocodes')]");
        public static readonly WebElement LinkShopSettings = new WebElement().ByXPath(@"//li/a[contains(@href, '/admin/shop/settings')]");

        public static readonly WebElement LinkShopSettingsPayment = new WebElement().ByXPath(@"//li/a[contains(@href, '/admin/shop/settings/payment')]");
        
    }
}
