using Selenium.Utilities.WebElement;
using Selenium.WebPages;

namespace GophotowebAT.Selenium.WebPages
{
    public class Clientarea : PageBase
    {
        private readonly WebElement InputUsername = new WebElement().ByXPath(@"//input[@id='username']");
        private readonly WebElement InputPassword = new WebElement().ByXPath(@"//input[@id='pass']");
        private readonly WebElement InputSubmit = new WebElement().ByXPath(@"//input[@type='submit']");

        public ClientSites LogIn()
        {
            InputUsername.Text = "mikola-2@yandex.ru";
            InputPassword.Text = "zJwRy3yw0c";
            InputSubmit.Click();
            return new ClientSites();
        }
    }
}
