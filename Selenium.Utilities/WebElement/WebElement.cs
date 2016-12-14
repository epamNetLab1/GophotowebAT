using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;
using log4net;
using GophotowebAT;

namespace Selenium.Utilities.WebElement
{
    public partial class WebElement : ICloneable
    {
        public By FirstSelector { get; private set; }
        private IList<IWebElement> _searchCache;
        protected static readonly ILog log = LogManager.GetLogger(typeof(WebElement));

        private IWebElement FindSingle()
        {
            return TryFindSingle();
        }

        private IWebElement TryFindSingle()
        {
            try
            {
                return FindSingleIWebElement();
            }
            catch (StaleElementReferenceException)
            {
                ClearSearchResultCache();

                return FindSingleIWebElement();
            }
            catch (InvalidSelectorException)
            {
                throw;
            }
            catch (WebDriverException)
            {
                throw;
            }
            catch (WebElementNotFoundException)
            {
                throw;
            }
            catch (Exception ex)
            {
                log.Error($"@@@   ex.GetType(): {ex.GetType().ToString()}{Environment.NewLine}ex.Message: {ex.Message}");
                throw WebElementNotFoundException;
            }
        }

        private IWebElement FindSingleIWebElement()
        {
            var elements = FindIWebElements();

            if (!elements.Any()) throw WebElementNotFoundException;

            var element = elements.Count() == 1
                ? elements.Single()
                : _index == -1
                    ? elements.Last()
                    : elements.ElementAt(_index);

            return element;
        }

        private IList<IWebElement> FindIWebElements()
        {
            log4net.Config.XmlConfigurator.Configure();
            if (_searchCache != null)
            {
                return _searchCache;
            }

            Browser.WaitReadyState();
            Browser.WaitAjax();

            var resultEnumerable = Browser.FindElements(FirstSelector);

            try
            {
                resultEnumerable = FilterByVisibility(resultEnumerable).ToList();
                resultEnumerable = FilterByTagNames(resultEnumerable).ToList();
                resultEnumerable = FilterByText(resultEnumerable).ToList();
                resultEnumerable = FilterByTagAttributes(resultEnumerable).ToList();
                resultEnumerable = resultEnumerable.ToList();
            }
            catch (StaleElementReferenceException)
            {
                return FindIWebElements();
            }
            catch (Exception e)
            {
                log.Debug(e.Message);

                return new List<IWebElement>();
            }

            var resultList = resultEnumerable.ToList();

            //log.Debug($"        FindIWebElements: List<IWebElement>({FirstSelector}).Count = {resultList.Count}");
            return resultList;
        }

        private WebElementNotFoundException WebElementNotFoundException
        {
            get
            {
                log.Debug($"        WebElementNotFoundException: {FirstSelector} on page {Browser.Url.ToString()}");
                NUnit.Framework.Assert.Fail($"@@@  WebDriverException  @@@  WebElementNotFoundException{Environment.NewLine}ex.Message: Can't find single element on page '{Browser.Url}' with given search criteria: '{SearchCriteriaToString()}'.");
                return new WebElementNotFoundException(string.Format(
                    $"Can't find single element on page '{Browser.Url}' with given search criteria: '{SearchCriteriaToString()}'."));
            }
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public WebElement Clone()
        {
            return (WebElement)MemberwiseClone();
        }
    }
}
