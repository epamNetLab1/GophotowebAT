using Selenium.Utilities.WebElement;
using Selenium.WebPages;

namespace GophotowebAT.AdminSite.Selenium.WebPages
{
    public class Createclientsite : PageBase
    {
        private readonly WebElement BlockCreateClientSite = new WebElement().ByXPath(@"//div[contains(@class, 'col-md-6')]");

        internal ClientSites CreateSite()
        {
            BlockCreateClientSite.Click();           
            return new ClientSites();
        }
    }
}
