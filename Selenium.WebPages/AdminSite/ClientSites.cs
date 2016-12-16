using System;
using Selenium.Utilities.WebElement;
using Selenium.WebPages;
using Selenium.Utilities;
using Selenium.Utilities.Tags;

namespace GophotowebAT.AdminSite.Selenium.WebPages
{
    public class ClientSites : PageBase
    {
        public readonly WebElement MenuUsername = new WebElement().ByXPath(@"//div[@id='userMenu']/span");
        public readonly WebElement LinkSiteSettings = new WebElement().ByXPath(@"//a[contains(@class, 'mod--cart-settings')]");
        private readonly WebElement LinkAddSite = new WebElement().ByXPath(@"//a[@href='createclientsite.php']");
        public readonly WebElement BlockWaitAnimation = new WebElement().ByXPath(@"//div[contains(text(), 'Один момент,')]");
        public readonly WebElement LinkCustomerSite = new WebElement().ByXPath(@"//a[@class='md-cart__title']");
        public readonly WebElement LinkEditSite = new WebElement().ByXPath(@"//a[contains(@class, 'mod--cart-del')]");

        internal string GetCustomerSiteUrl()
        {
            return LinkCustomerSite.GetAttribute(TagAttributes.Href);
        }

        internal Createclientsite LinkAddSiteClick()
        {
            LinkAddSite.Click();
            return new Createclientsite();
        }

        internal void WaitAddSite()
        {
            WaitHelper.SpinWait(() => !BlockWaitAnimation.Exists(), TimeSpan.FromSeconds(120), TimeSpan.FromSeconds(15));
        }

        internal Clientsiteedit LinkSiteSettingsClick()
        {
            LinkSiteSettings.Last().Click();
            return new Clientsiteedit();
        }

        internal void ClickEditSite()
        {
            LinkEditSite.Click();            
        }
    }
}
