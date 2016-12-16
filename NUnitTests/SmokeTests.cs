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
    public class SmokeTests : BaseTest
    {
        private string userName = "Николай Кудряшов";
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
        public void SignInTest()
        {
            Assert.AreEqual(userName, clientSites.MenuUsername.Text, "clientSites.MenuUsername.Text");
        }

        [Test]
        public void AddDeleteSitesTest()
        {
            var sitesCount = clientSites.LinkSiteSettings.Count;
            clientSites.LinkAddSiteClick()
                .CreateSite();
            Assert.AreEqual(1, clientSites.BlockWaitAnimation.Count, "clientSites.BlockWaitAnimation.Count");

            clientSites.WaitAddSite();
            Assert.AreEqual(sitesCount + 1, clientSites.LinkSiteSettings.Count, "clientSites.LinkSiteSettings.Count");

            while (clientSites.LinkSiteSettings.Count > 1)
            {
                sitesCount = clientSites.LinkSiteSettings.Count;
                clientSites.LinkSiteSettingsClick()
                    .DeleteSite();
                Assert.AreEqual(sitesCount - 1, clientSites.LinkSiteSettings.Count, "clientSites.LinkSiteSettings.Count");
            }
        }
    }
}
