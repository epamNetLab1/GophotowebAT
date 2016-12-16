using Selenium.Utilities.WebElement;
using Selenium.WebPages;

namespace GophotowebAT.AdminSite.Selenium.WebPages
{
    public class Homepage : PageBase
    {
        private static readonly WebElement LinkLogin = new WebElement().ByXPath(@"//*[@id='last']");

        internal Clientarea LogInClick()
        {
            LinkLogin.Click();
            return new Clientarea();
        }
    }
}
