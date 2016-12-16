using Selenium.Utilities;
using Selenium.Utilities.WebElement;
using Selenium.WebPages;
using System;

namespace GophotowebAT.CustomerSite.Selenium.WebPages
{
    public class Payment : PageBase
    {
        public static readonly WebElement RowPaymentMethod = new WebElement().ByXPath(@"//div[@id='tree-body']/div[contains(@class, 'paymentMethodItem')]");

        public static readonly WebElement ButtonAddPayment = new WebElement().ByXPath(@"//div[contains(@data-target, 'popupAddPaymentMethod')]");
        public static readonly WebElement ButtonSelectPaymentType = new WebElement().ByXPath(@"//div[select[@name='data[payment][type]']]/div/button");
        public static readonly WebElement ButtonSavePaymentMethod = new WebElement()
            .ByXPath(@"//div[@id='popupAddPaymentMethod']//form/div/input[@type='submit']");

        public static readonly WebElement InputPaymentTitle = new WebElement().ByXPath(@"//input[@name='data[payment][title]']");
        public static readonly WebElement InputPaymentDescription = new WebElement().ByXPath(@"//textarea[@name='data[payment][description]']");
        public static readonly WebElement ButtonSubmit = new WebElement().ByXPath(@"//form/div[contains(@class, 'content-body')]//input[@type='submit']");
        public static readonly WebElement ButtonConfirmDelete = new WebElement().ByXPath(@"//div[contains(@class, 'remove-widget')]//button/span");

        internal void AddCustomPayment(string name)
        {
            ButtonAddPayment.Click();
            //ButtonSelectPaymentType.Click();
            ButtonSavePaymentMethod.Click();
            InputPaymentTitle.Text = $"PaymentTitle{name}";
            InputPaymentDescription.Text = $"PaymentDescription{name}";
            ButtonSubmit.Click();
        }

        internal void SetPaymentVisible(string name)
        {
            new WebElement().ByXPath($@"//div[div/a[contains(text(), '{name}')]]/div[input]/label").Click();
        }

        internal void DeletePayment(string name)
        {
            var linkDeletePaymentMethod = new WebElement().ByXPath($@"//div[div/a[contains(text(), '{name}')]]/div/a[span]");
            linkDeletePaymentMethod.Click();
            ButtonConfirmDelete.Click();
            WaitHelper.SpinWait(() => !linkDeletePaymentMethod.Exists());
        }
    }
}
