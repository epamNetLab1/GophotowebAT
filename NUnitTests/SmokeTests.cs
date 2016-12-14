using GophotowebAT.Selenium.WebPages;
using log4net;
using NUnit.Framework;
using Selenium.WebPages;
using TestHttpWebRequest.NUnitTests;

namespace GophotowebAT.NUnitTests
{
    [TestFixture]
    public class SmokeTests : BaseTest
    {
        private string userName = "Николай Кудряшов";
        private ClientSites clientSites;

        [SetUp]
        public void AdminStartSite()
        {
            Pages.Homepage.Open();
            var clientarea = Pages.Homepage.LogInClick();
            clientSites = clientarea.LogIn();
        }

        [Test, Timeout(600000)]
        public void TestTest()
        {
            Assert.AreEqual(1, 1, "1==1");
        }

        //[Test, Timeout(600000)]
        //public void TestSignIn()
        //{
        //    Assert.AreEqual(userName, clientSites.MenuUsername.Text, "clientSites.MenuUsername.Text");
        //}

        //[Test, Timeout(600000)]
        //public void TestAddDeleteSites()
        //{
        //    var sitesCount = clientSites.LinkSiteSettings.Count;
        //    var createclientsite = clientSites.LinkAddSiteClick();
        //    clientSites = createclientsite.CreateSite();
        //    Assert.AreEqual(1, clientSites.BlockWaitAnimation.Count, "clientSites.BlockWaitAnimation.Count");
        //    clientSites.WaitAddSite();
        //    Assert.AreEqual(sitesCount + 1, clientSites.LinkSiteSettings.Count, "clientSites.LinkSiteSettings.Count");
        //    while (clientSites.LinkSiteSettings.Count > 1)
        //    {
        //        sitesCount = clientSites.LinkSiteSettings.Count;
        //        var clientsiteedit = clientSites.LinkSiteSettingsClick();
        //        clientSites = clientsiteedit.DeleteSite();
        //        Assert.AreEqual(sitesCount - 1, clientSites.LinkSiteSettings.Count, "clientSites.LinkSiteSettings.Count");
        //    }
        //}
    }
}
