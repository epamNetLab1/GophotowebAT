using GophotowebAT.AdminSite.Selenium.WebPages;
using GophotowebAT.CustomerSite.Selenium.WebPages;
using NUnit.Framework;
using Selenium.Utilities;
using Selenium.WebPages;
using System;
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
            Console.WriteLine($"productPriceShopPage = {productPriceShopPage}; productPriceProductPage = {productPriceProductPage}");
            Assert.AreEqual(productPriceShopPage, productPriceProductPage, "productPriceProductPage");
            Pages.ProductPage.ClickButtonAddToCart();
            Navigate.LinkShopCart.Click();
            var productPriceCartPage = Pages.Cart.GetPriceProduct();
            Console.WriteLine($"productPriceShopPage={productPriceShopPage}:productPriceCartPage={productPriceCartPage}");
            Assert.AreEqual(productPriceShopPage, productPriceCartPage, "productPriceProductPage");
            var date = DateTime.Now.ToString("dd/MM/yyyy_hh:mm:ss");
            Pages.Cart.FillCustomerData(date);
            Cart.ButtonSubmit.Click();
            StringAssert.Contains("Спасибо за покупку!", OrderResultPage.LabelMessage.Text, "OrderResultPage.LabelMessage.Text");
        }
    }
}
