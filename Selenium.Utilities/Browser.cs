using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Interactions;
using Selenium.Utilities.Properties;
using System.Configuration;
using OpenQA.Selenium.Support.Events;
using log4net;
using GophotowebAT;

namespace Selenium.Utilities
{
    [Serializable]
    public enum Browsers
    {
        [Description("Windows Internet Explorer")]
        InternetExplorer,

        [Description("Mozilla Firefox")]
        Firefox,

        [Description("Google Chrome")]
        Chrome
    }

    public class Browser
    {
        private const string _webDriverExceptionMessage = "Unable to connect to the remote server";
        protected static readonly ILog log = LogManager.GetLogger(typeof(Browser));

        public static bool IsExist()
        {
            return _current != null;
        }

        private static Browser _current;

        private Browser() { }

        public static Browser Current
        {
            get
            {
                if (_current == null)
                {
                    _current = new Browser();
                }

                return _current;
            }
        }

        #region Public properties

        public static Browsers SelectedBrowser
        {
            get { return Settings.Default.Browser; }
        }

        public static Uri Url
        {
            get
            {
                Uri uri;
                WaitAjax();
                try
                {
                    uri = new Uri(WebDriver.Url);
                }
                catch (WebDriverException ex)
                {
                    NUnit.Framework.Assert.Fail($"@@@  WebDriverException  @@@  WebDriver.Url{Environment.NewLine}ex.Message: {ex.Message}");
                    uri = new Uri(WebDriver.Url);
                }
                return uri;
            }
        }

        public string Title
        {
            get
            {
                WaitAjax();
                return string.Format("{0} - {1}", WebDriver.Title, EnumHelper.GetEnumDescription(SelectedBrowser));
            }
        }

        public static string PageSource
        {
            get { WaitAjax(); return WebDriver.PageSource; }
        }

        #endregion

        #region Public methods

        public static void Start()
        {
            _webDriver = StartWebDriver();
        }

        public static void Navigate(Uri url)
        {
            WebDriver.Navigate().GoToUrl(url);
            log.Debug("    Navigated to " + url);
        }

        public static void Quit()
        {
            if (_webDriver == null) return;

            _webDriver.Quit();
            _webDriver = null;
        }

        public static void WaitReadyState()
        {
            var ready = new Func<bool>(() => (bool)ExecuteJavaScript("return document.readyState == 'complete'"));
        }

        public static void WaitAjax()
        {
            var ready = new Func<bool>(() => (bool)ExecuteJavaScript("return (typeof($) === 'undefined') ? true : !$.active;"));
        }

        public static void SwitchToFrame(IWebElement inlineFrame)
        {
            WebDriver.SwitchTo().Frame(inlineFrame);
        }

        public static void SwitchToPopupWindow()
        {
            foreach (var handle in WebDriver.WindowHandles.Where(handle => handle != _mainWindowHandler)) // TODO:
            {
                WebDriver.SwitchTo().Window(handle);
            }
        }

        public static void SwitchToMainWindow()
        {
            WebDriver.SwitchTo().Window(_mainWindowHandler);
        }

        public static void SwitchToDefaultContent()
        {
            WebDriver.SwitchTo().DefaultContent();
        }

        public static void AcceptAlert()
        {
            var accept = WaitHelper.MakeTry(() => WebDriver.SwitchTo().Alert().Accept());

            WaitHelper.SpinWait(accept, TimeSpan.FromSeconds(5));
        }

        public static IEnumerable<IWebElement> FindElements(By selector)
        {
            IEnumerable<IWebElement> elementCollection;
            try
            {
                elementCollection = WebDriver.FindElements(selector);
            }
            catch (WebDriverException ex)
            {
                if (ex.Message.Contains(_webDriverExceptionMessage))
                {
                    NUnit.Framework.Assert.Fail($"@@@  WebDriverException  @@@  WebDriver.FindElements({selector}){Environment.NewLine}ex.Message: {ex.Message}");
                }
                elementCollection = FindElements(selector);
            }
            return elementCollection;
        }

        public static Screenshot GetScreenshot()
        {
            WaitReadyState();

            return ((ITakesScreenshot)WebDriver).GetScreenshot();
        }

        public static void SaveScreenshot(string path)
        {
            var imageBytes = Convert.FromBase64String(GetScreenshot().ToString());

            using (var bw = new BinaryWriter(new FileStream(path, FileMode.Append, FileAccess.Write)))
            {
                bw.Write(imageBytes);
                bw.Close();
            }
        }

        public static void DragAndDrop(IWebElement source, IWebElement destination)
        {
            (new Actions(WebDriver)).DragAndDrop(source, destination).Build().Perform();
        }

        public static void ResizeWindow(int width, int height)
        {
            ExecuteJavaScript(string.Format("window.resizeTo({0}, {1});", width, height));
        }

        public static void NavigateBack()
        {
            WebDriver.Navigate().Back();
        }

        public static void Refresh()
        {
            WebDriver.Navigate().Refresh();
        }

        public static object ExecuteJavaScript(string javaScript, params object[] args)
        {
            var javaScriptExecutor = (IJavaScriptExecutor)WebDriver;
            return javaScriptExecutor.ExecuteScript(javaScript, args);
        }

        public static void KeyDown(string key)
        {
            new Actions(WebDriver).KeyDown(key);
        }

        public static void KeyUp(string key)
        {
            new Actions(WebDriver).KeyUp(key);
        }

        #endregion

        #region Private

        private static IWebDriver _webDriver;
        private static string _mainWindowHandler;

        public static IWebDriver WebDriver
        {
            get { return _webDriver ?? StartWebDriver(); }
        }

        private static IWebDriver StartWebDriver()
        {
            log4net.Config.XmlConfigurator.Configure();
            if (_webDriver != null) return _webDriver;

            switch (SelectedBrowser)
            {
                case Browsers.InternetExplorer:
                    _webDriver = StartInternetExplorer();
                    break;
                case Browsers.Firefox:
                    _webDriver = StartFirefox();
                    break;
                case Browsers.Chrome:
                    _webDriver = StartChrome();
                    break;
                default:
                    throw new Exception(string.Format("Unknown browser selected: {0}.", SelectedBrowser));
            }
            _webDriver.Manage().Timeouts().ImplicitlyWait(TimeSpan.FromSeconds(10));
            _webDriver.Manage().Window.Maximize();
            _mainWindowHandler = _webDriver.CurrentWindowHandle;

            return WebDriver;
        }

        private static InternetExplorerDriver StartInternetExplorer()
        {
            var internetExplorerOptions = new InternetExplorerOptions
            {
                IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                InitialBrowserUrl = "about:blank",
                EnableNativeEvents = true
            };

            return new InternetExplorerDriver(Directory.GetCurrentDirectory(), internetExplorerOptions);
        }

        private static FirefoxDriver StartFirefox()
        {
            var firefoxProfile = new FirefoxProfile
            {
                AcceptUntrustedCertificates = true,
                EnableNativeEvents = true
            };
            firefoxProfile.SetPreference("browser.download.folderList", 2);
            firefoxProfile.SetPreference("browser.download.dir", Tools.RootFolder);
            firefoxProfile.SetPreference("browser.helperApps.neverAsk.saveToDisk", "application/vnd-ms.excel, application/ms.excel");

            var pathToGeckoDriver = ConfigurationManager.AppSettings["PathToGeckodriver"];
            FirefoxDriverService service = string.IsNullOrEmpty(pathToGeckoDriver)
                ? FirefoxDriverService.CreateDefaultService()
                : FirefoxDriverService.CreateDefaultService(pathToGeckoDriver);
            var firefoxDriver = new FirefoxDriver(service);

            return firefoxDriver;
        }

        private static IWebDriver StartChrome()
        {
            var service = ChromeDriverService.CreateDefaultService();
            var chromeOptions = new ChromeOptions();
            chromeOptions.AddUserProfilePreference("download.default_directory", Tools.RootFolder);
            chromeOptions.AddUserProfilePreference("intl.accept_languages", "nl");
            chromeOptions.AddUserProfilePreference("disable-popup-blocking", "true");

            //var firingDriver = ReturnFiringDriver(new ChromeDriver(service));
            return new ChromeDriver(service, chromeOptions);//firingDriver;
        }

        public static EventFiringWebDriver ReturnFiringDriver(IWebDriver driver)
        {
            var eventDriver = new EventFiringWebDriver(driver);

            return eventDriver;
        }
        #endregion
    }
}
