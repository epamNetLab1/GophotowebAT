using Selenium.Utilities.WebElement;
using Selenium.WebPages;
using System;

namespace GophotowebAT.CustomerSite.Selenium.WebPages
{
    public class Cart : PageBase
    {
        public static readonly WebElement InputFirstName = new WebElement().ByXPath(@"//div[label[contains(text(), 'Имя')]]/input");
        public static readonly WebElement InputLastName = new WebElement().ByXPath(@"//div[label[contains(text(), 'Фамилия')]]/input");
        public static readonly WebElement InputEmail = new WebElement().ByXPath(@"//div[label[contains(text(), 'Email')]]/input");
        public static readonly WebElement InputPhone = new WebElement().ByXPath(@"//div[label[contains(text(), 'Телефон')]]/input");
        public static readonly WebElement InputAddress = new WebElement().ByXPath(@"//div[label[contains(text(), 'Улица')]]/input");
        public static readonly WebElement InputCity = new WebElement().ByXPath(@"//div[label[contains(text(), 'Город')]]/input");
        public static readonly WebElement InputZip = new WebElement().ByXPath(@"//div[label[contains(text(), 'индекс')]]/input");
        public static readonly WebElement InputState = new WebElement().ByXPath(@"//div[label[contains(text(), 'Регион')]]/input");
        public static readonly WebElement InputComment = new WebElement().ByXPath(@"//div[div[label[contains(text(), 'комментарий')]]]/textarea");
        public static readonly WebElement ButtonSubmit = new WebElement().ByXPath(@"//button[@name='data[btn-submit]']");
        public static readonly WebElement LabelPrice = new WebElement().ByXPath(@"//td[contains(@class, 'totalProductPrice')]");
        
        public void FillCustomerData(string date)
        {
            InputFirstName.Text = $"FirstName-{date}";
            InputLastName.Text = $"LfstName-{date}";
            InputEmail.Text = $"test@test.com";
            InputPhone.Text = $"Phone-{date}";
            InputAddress.Text = $"Address-{date}";
            InputCity.Text = $"City-{date}";
            InputZip.Text = $"Zip-{date}";
            InputState.Text = $"State-{date}";
            InputComment.Text = $"Comment-{date}";
        }

        internal double GetPriceProduct()
        {
            var price = Tools.GetPriceFromText(LabelPrice.Text);
            return price;
        }
    }
}
