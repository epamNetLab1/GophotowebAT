using System;
using Selenium.Utilities.WebElement;
using Selenium.WebPages;
using Selenium.Utilities;
using System.Threading;

namespace GophotowebAT.AdminSite.Selenium.WebPages
{
    public class Clientsiteedit : PageBase
    {
        private readonly WebElement LinkDeleteSite = new WebElement().ByXPath(@"//a[contains(@href, 'clientsitedelete.php')]");
        private readonly WebElement ButtonDeleteSite = new WebElement().ByXPath(@"//div[contains(@class, 'md-remove')]//a");
        private readonly WebElement ButtonConfirmationDeleteSite = new WebElement()
            .ByXPath(@"//div[contains(@class, 'md-remove')]//div[contains(@class, 'popup')]//a[contains(@data-popup-load-cont, '2')]");
        private readonly WebElement PopupBackground = new WebElement().ByXPath(@"//div[contains(@data-popup-close)]");

        internal ClientSites DeleteSite()
        {
            LinkDeleteSite.Click();
            ButtonDeleteSite.Click();
            ButtonConfirmationDeleteSite.Click();
            Browser.Navigate(new Uri("http://clients.gophotoweb.ru/clientsites.php"));
            Thread.Sleep(2000);
            return new ClientSites();
        }
    }
}
