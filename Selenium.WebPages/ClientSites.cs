using System;
using Selenium.Utilities.WebElement;
using Selenium.WebPages;
using Selenium.Utilities;

namespace GophotowebAT.Selenium.WebPages
{
    public class ClientSites : PageBase
    {
        public readonly WebElement MenuUsername = new WebElement().ByXPath(@"//div[@id='userMenu']/span");
        public readonly WebElement LinkSiteSettings = new WebElement().ByXPath(@"//a[contains(@class, 'mod--cart-settings')]");
        private readonly WebElement LinkAddSite = new WebElement().ByXPath(@"//a[@href='createclientsite.php']");
        public readonly WebElement BlockWaitAnimation = new WebElement().ByXPath(@"//div[contains(text(), 'Один момент,')]");

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
    }
}
