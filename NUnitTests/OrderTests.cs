using GophotowebAT.AdminSite.Selenium.WebPages;
using GophotowebAT.CustomerSite.Selenium.WebPages;
using NUnit.Framework;
using OpenQA.Selenium;
using Selenium.Utilities;
using Selenium.WebPages;
using System;
using System.Linq;
using TestHttpWebRequest.NUnitTests;

namespace GophotowebAT.NUnitTests
{
    [TestFixture]
    public class OrderTests : BaseTest
    {
        private string customerSiteUrl = "";
        private ClientSites clientSites;

        [SetUp]
        public void AdminStartSite()
        {
            Pages.Homepage.Open();
            var clientarea = Pages.Homepage.LogInClick();
            clientSites = clientarea.LogIn();
            customerSiteUrl = clientSites.GetCustomerSiteUrl();
        }

        [Test]
        public void PlaceOrderTest()
        {
            Browser.Navigate(new Uri(customerSiteUrl));
            Navigate.LinkShop.Click();
            var productPriceShopPage = Pages.Shop.GetPriceProduct();
            Shop.LinkProduct.Click();
            var productPriceProductPage = Pages.ProductPage.GetPriceProduct();
            Assert.AreEqual(productPriceShopPage, productPriceProductPage, "productPriceProductPage");

            Pages.ProductPage.ClickButtonAddToCart();
            Navigate.LinkShopCart.Click();
            var productPriceCartPage = Pages.Cart.GetPriceProduct(Cart.LabelPrice);
            Assert.AreEqual(productPriceShopPage, productPriceCartPage, "productPriceProductPage");

            var date = DateTime.Now.ToString("dd/MM/yyyy_hh:mm:ss");
            Pages.Cart.FillCustomerData(date);
            Cart.ButtonSubmit.Click();
            StringAssert.Contains("Спасибо за покупку!", OrderResultPage.LabelMessage.Text, "OrderResultPage.LabelMessage.Text");
        }

        [Test]
        public void ChangeQtyAndPlaceOrderTest()
        {
            int newQty = 5;

            Browser.Navigate(new Uri(customerSiteUrl));
            Navigate.LinkShop.Click();
            var productPriceShopPage = Pages.Shop.GetPriceProduct();
            Shop.LinkProduct.Click();
            var productPriceProductPage = Pages.ProductPage.GetPriceProduct();
            Assert.AreEqual(productPriceShopPage, productPriceProductPage, "productPriceProductPage");

            Pages.ProductPage.ClickButtonAddToCart();
            Navigate.LinkShopCart.Click();
            var productPriceCartPage = Pages.Cart.GetPriceProduct(Cart.LabelPrice);
            Pages.Cart.ChangeQtyProduct(newQty);
            Assert.AreEqual(productPriceShopPage, productPriceCartPage, "productPriceProductPage");

            var productPriceTotalCartPage = Pages.Cart.GetPriceProduct(Cart.LabelTotalPrice);
            Assert.AreEqual(productPriceShopPage * 5, productPriceTotalCartPage, "productPriceTotalCartPage");
            Assert.AreEqual(productPriceShopPage, productPriceCartPage, "productPriceProductPage");

            var date = DateTime.Now.ToString("dd/MM/yyyy_hh:mm:ss");
            Pages.Cart.FillCustomerData(date);
            Cart.ButtonSubmit.Click();
            StringAssert.Contains("Спасибо за покупку!", OrderResultPage.LabelMessage.Text, "OrderResultPage.LabelMessage.Text");
        }

        [Test]
        public void CustomPaymentTest()
        {
            var uniqueDate = DateTime.Now.ToString("dd/MM/yyyy_hh:mm:ss");

            clientSites.ClickEditSite();
            NavigateAdmin.LinkShop.Click();
            NavigateAdmin.LinkShopSettings.Click();
            NavigateAdmin.LinkShopSettingsPayment.Click();
            var oldPaymentCount = Payment.RowPaymentMethod.Count;
            Pages.Payment.AddCustomPayment(uniqueDate);
            NavigateAdmin.LinkShopSettingsPayment.Click();
            Pages.Payment.SetPaymentVisible(uniqueDate);
            Assert.AreEqual(oldPaymentCount + 1, Payment.RowPaymentMethod.Count, "Payment.RowPaymentMethod.Count");

            Browser.Navigate(new Uri(customerSiteUrl));
            Navigate.LinkShop.Click();
            var productPriceShopPage = Pages.Shop.GetPriceProduct();
            Shop.LinkProduct.Click();
            var productPriceProductPage = Pages.ProductPage.GetPriceProduct();
            Assert.AreEqual(productPriceShopPage, productPriceProductPage, "productPriceProductPage");

            Pages.ProductPage.ClickButtonAddToCart();
            Navigate.LinkShopCart.Click();
            var productPriceCartPage = Pages.Cart.GetPriceProduct(Cart.LabelPrice);
            Assert.AreEqual(productPriceShopPage, productPriceCartPage, "productPriceProductPage");

            Pages.Cart.SelectPaymentMethod(uniqueDate);
            Pages.Cart.FillCustomerData(uniqueDate);
            Cart.ButtonSubmit.Click();
            StringAssert.Contains("Спасибо за покупку!", OrderResultPage.LabelMessage.Text, "OrderResultPage.LabelMessage.Text");

            Pages.Homepage.Open();
            var clientarea = Pages.Homepage.LogInClick();
            clientSites.ClickEditSite();
            NavigateAdmin.LinkShop.Click();
            NavigateAdmin.LinkShopSettings.Click();
            NavigateAdmin.LinkShopSettingsPayment.Click();
            oldPaymentCount = Payment.RowPaymentMethod.Count;
            Pages.Payment.DeletePayment(uniqueDate);
            Assert.AreEqual(oldPaymentCount - 1, Payment.RowPaymentMethod.Count, "Payment.RowPaymentMethod.Count");
        }

        [Test]
        public void DeleteCustomPaymentWithCurrentOrderTest()
        {
            var uniqueDate = DateTime.Now.ToString("dd/MM/yyyy_hh:mm:ss");

            clientSites.ClickEditSite();
            NavigateAdmin.LinkShop.Click();
            NavigateAdmin.LinkShopSettings.Click();
            NavigateAdmin.LinkShopSettingsPayment.Click();
            var oldPaymentCount = Payment.RowPaymentMethod.Count;
            Pages.Payment.AddCustomPayment(uniqueDate);
            NavigateAdmin.LinkShopSettingsPayment.Click();
            Pages.Payment.SetPaymentVisible(uniqueDate);
            Assert.AreEqual(oldPaymentCount + 1, Payment.RowPaymentMethod.Count, "Payment.RowPaymentMethod.Count");

            Browser.OpenNewTab(customerSiteUrl);
            Browser.WebDriver.SwitchTo().Window(Browser.WebDriver.WindowHandles.Last());
            Navigate.LinkShop.Click();
            var productPriceShopPage = Pages.Shop.GetPriceProduct();
            Shop.LinkProduct.Click();
            var productPriceProductPage = Pages.ProductPage.GetPriceProduct();
            Assert.AreEqual(productPriceShopPage, productPriceProductPage, "productPriceProductPage");

            Pages.ProductPage.ClickButtonAddToCart();
            Navigate.LinkShopCart.Click();
            var productPriceCartPage = Pages.Cart.GetPriceProduct(Cart.LabelPrice);
            Assert.AreEqual(productPriceShopPage, productPriceCartPage, "productPriceProductPage");

            Pages.Cart.SelectPaymentMethod(uniqueDate);
            Pages.Cart.FillCustomerData(uniqueDate);

            Browser.WebDriver.SwitchTo().Window(Browser.WebDriver.WindowHandles.First());
            oldPaymentCount = Payment.RowPaymentMethod.Count;
            Pages.Payment.DeletePayment(uniqueDate);
            Assert.AreEqual(oldPaymentCount - 1, Payment.RowPaymentMethod.Count, "Payment.RowPaymentMethod.Count");

            Browser.WebDriver.SwitchTo().Window(Browser.WebDriver.WindowHandles.Last());
            Cart.ButtonSubmitClick();

            Console.WriteLine(Cart.LabelPaymentDisableMethodError.Text);
            StringAssert.Contains("Выбранный метод оплаты в данный момент не доступен",
                Cart.LabelPaymentDisableMethodError.Text, "Cart.LabelPaymentDisableMethodError.Text");
        }

        [Test]
        public void CustomDeliveryTest()
        {
            var uniqueDate = DateTime.Now.ToString("dd/MM/yyyy_hh:mm:ss");

            clientSites.ClickEditSite();
            NavigateAdmin.LinkShop.Click();
            NavigateAdmin.LinkShopSettings.Click();
            NavigateAdmin.LinkShopSettingsDelivery.Click();
            var oldDeliveryCount = Delivery.RowDeliveryMethod.Count;
            Pages.Delivery.AddDeliveryMethod(uniqueDate);
            NavigateAdmin.LinkShopSettingsDelivery.Click();
            Pages.Delivery.SetDeliveryVisible(uniqueDate);
            Assert.AreEqual(oldDeliveryCount + 1, Delivery.RowDeliveryMethod.Count, "Delivery.RowDeliveryMethod.Count");

            Browser.Navigate(new Uri(customerSiteUrl));
            Navigate.LinkShop.Click();
            var productPriceShopPage = Pages.Shop.GetPriceProduct();
            Shop.LinkProduct.Click();
            var productPriceProductPage = Pages.ProductPage.GetPriceProduct();
            Assert.AreEqual(productPriceShopPage, productPriceProductPage, "productPriceProductPage");

            Pages.ProductPage.ClickButtonAddToCart();
            Navigate.LinkShopCart.Click();
            var productPriceCartPage = Pages.Cart.GetPriceProduct(Cart.LabelPrice);
            Assert.AreEqual(productPriceShopPage, productPriceCartPage, "productPriceProductPage");

            Pages.Cart.FillCustomerData(uniqueDate);
            Cart.ButtonSubmit.Click();
            StringAssert.Contains("Спасибо за покупку!", OrderResultPage.LabelMessage.Text, "OrderResultPage.LabelMessage.Text");

            Pages.Homepage.Open();
            var clientarea = Pages.Homepage.LogInClick();
            clientSites.ClickEditSite();
            NavigateAdmin.LinkShop.Click();
            NavigateAdmin.LinkShopSettings.Click();
            NavigateAdmin.LinkShopSettingsDelivery.Click();
            oldDeliveryCount = Delivery.RowDeliveryMethod.Count;
            Pages.Delivery.DeleteDeliveryMethod(uniqueDate);
            Assert.AreEqual(oldDeliveryCount - 1, Delivery.RowDeliveryMethod.Count, "Delivery.RowDeliveryMethod.Count");
        }
    }
}
