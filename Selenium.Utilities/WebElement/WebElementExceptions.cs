using System;

namespace Selenium.Utilities.WebElement
{
    public class WebElementNotFoundException : Exception
    {
        public WebElementNotFoundException(string message) : base(message)
        {
        }
    }
}
