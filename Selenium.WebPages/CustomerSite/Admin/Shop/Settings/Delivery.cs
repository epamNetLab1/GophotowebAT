using Selenium.Utilities;
using Selenium.Utilities.WebElement;
using Selenium.WebPages;
using System.Threading;

namespace GophotowebAT.CustomerSite.Selenium.WebPages
{
    public class Delivery : PageBase
    {
        public static readonly WebElement RowDeliveryMethod = new WebElement().ByXPath(@"//div[@id='tree-body']/div/div/div[contains(@class, 'delivery-title')]");

        public static readonly WebElement ButtonAddDelivery = new WebElement().ByXPath(@"//a[div[contains(@class, 'button-add')]]");
        public static readonly WebElement InputDeliveryMethodTitle = new WebElement().ByXPath(@"//input[@id='delivery-method-title']");
        public static readonly WebElement InputDeliveryMethodTaxPerOrder = new WebElement().ByXPath(@"//input[@id='delivery-method-tax_per_order']");
        
        public static readonly WebElement ButtonSubmit = new WebElement().ByXPath(@"//form[contains(@action, '/delivery_method/add')]/div/input[@type='submit']");
        public static readonly WebElement ButtonConfirmDelete = new WebElement().ByXPath(@"//div[contains(@class, 'remove-widget')]//button/span");

        internal void AddDeliveryMethod(string name)
        {
            ButtonAddDelivery.Click();
            InputDeliveryMethodTitle.Text = $"DeliveryMethodTitle{name}";
            InputDeliveryMethodTaxPerOrder.Text = "0";
            ButtonSubmit.Click();
            Thread.Sleep(2000);
        }

        internal void SetDeliveryVisible(string name)
        {
            new WebElement().ByXPath($@"//div[div/a[contains(text(), '{name}')]]/div[input]/label").Click();
        }

        internal void DeleteDeliveryMethod(string name)
        {
            var linkDeleteDeliveryMethod = new WebElement().ByXPath($@"//div[div/a[contains(text(), '{name}')]]/div/a[span]");
            linkDeleteDeliveryMethod.Click();
            ButtonConfirmDelete.Click();
            WaitHelper.SpinWait(() => !linkDeleteDeliveryMethod.Exists());
        }
    }
}
