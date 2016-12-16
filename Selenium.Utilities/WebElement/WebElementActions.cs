using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Selenium.Utilities.Tags;

namespace Selenium.Utilities.WebElement
{
    internal enum SelectTypes
    {
        ByValue,
        ByText
    }

    public partial class WebElement
    {
        #region Common properties

        public int Count
        {
            get { return FindIWebElements().Count; }
        }

        public bool Enabled
        {
            get { return FindSingle().Enabled; }
        }

        public bool Displayed
        {
            get { return FindSingle().Displayed; }
        }

        public bool Selected
        {
            get { return FindSingle().Selected; }
        }

        public string Text
        {
            set
            {
                var element = FindSingle();
                if (element.TagName == EnumHelper.GetEnumDescription(TagNames.Input) || element.TagName == EnumHelper.GetEnumDescription(TagNames.TextArea))
                {
                    //element.Clear();
                }
                else
                {
                    element.SendKeys(Keys.LeftControl + "a");
                    element.SendKeys(Keys.Delete);
                }
                if (string.IsNullOrEmpty(value)) return;
                Browser.ExecuteJavaScript(string.Format("arguments[0].value = \"{0}\";", value), element);
                //element.SendKeys(value); 

                WaitHelper.Try(() => FireJQueryEvent(JavaScriptEvents.KeyUp));
                WaitHelper.Try(() => FireJQueryEvent(JavaScriptEvents.Change));
                log.Debug($"        ('{FirstSelector}').Text('{value}')");
            }
            get
            {
                string text;
                try
                {
                    var element = FindSingle();
                    text = !string.IsNullOrEmpty(element.Text) ? element.Text : element.GetAttribute(EnumHelper.GetEnumDescription(TagAttributes.Value));
                }
                catch (StaleElementReferenceException)
                {
                    text = FindSingle().Text;
                }
                return text;
            }
        }

        public int TextInt
        {
            set { Text = value.ToString(CultureInfo.InvariantCulture); }
            get { return Text.ToInt(); }
        }

        public string InnerHtml
        {
            get { return Browser.ExecuteJavaScript("return arguments[0].innerHTML;", FindSingle()).ToString(); }
        }

        #endregion

        #region Common methods

        public bool Exists()
        {
            //log.Debug($"        ('{FirstSelector}').Exists()");
            return FindIWebElements().Any();
        }

        public bool Exists(TimeSpan timeSpan)
        {
            return WaitHelper.SpinWait(Exists, timeSpan, TimeSpan.FromMilliseconds(200));
        }

        public bool Exists(int seconds)
        {
            return WaitHelper.SpinWait(Exists, TimeSpan.FromSeconds(seconds), TimeSpan.FromMilliseconds(200));
        }

        public void ClickUseJQuery()
        {
            var element = FindSingle();
            FireJQueryEvent(element, JavaScriptEvents.Click);
            log.Debug($"        ('{FirstSelector}').ClickUseJQuery()");
        }

        public void Click(bool useJQuery = false)
        {
            var element = FindSingle();
            if (useJQuery && element.TagName != EnumHelper.GetEnumDescription(TagNames.Link))
            {
                FireJQueryEvent(element, JavaScriptEvents.Click);
            }
            else
            {
                try
                {
                    element.Click();
                }
                catch (InvalidOperationException e)
                {
                    if (e.Message.Contains("Element is not clickable"))
                    {
                        Thread.Sleep(2000);
                        element.Click();
                    }
                }
                catch (Exception e)
                {
                    if (e.Message.Contains("Unexpected error. Element is not clickable at point"))
                    {
                        Browser.ExecuteJavaScript(string.Format("window.scrollTo({0}, {1} - window.innerHeight/2);", element.Location.X, element.Location.Y));
                        element.Click();
                    }
                }
                log.Debug($"        ('{FirstSelector}').Click()");
            }
        }

        public void SendKeys(string keys)
        {
            FindSingle().SendKeys(keys);
            log.Debug($"        ('{FirstSelector}').SendKeys('{keys}')");
        }

        public void SetCheck(bool value, bool useJQuery = true)
        {
            var element = FindSingle();
            const int tryCount = 10;

            for (var i = 0; i < tryCount; i++)
            {
                element = FindSingle();

                Set(value, useJQuery);

                if (element.Selected == value)
                {
                    log.Debug($"        ('{FirstSelector}').SetCheck('{value}')");
                    return;
                }
            }
        }

        public void Select(string optionValue)
        {
            SelectCommon(optionValue, SelectTypes.ByValue);
            log.Debug($"        ('{FirstSelector}').Select(string '{optionValue}')");
        }

        public void Select(int optionValue)
        {
            SelectCommon(optionValue.ToString(CultureInfo.InvariantCulture), SelectTypes.ByValue);
            log.Debug($"        ('{FirstSelector}').Select(int '{optionValue}')");
        }

        public void SelectByText(string optionText)
        {
            SelectCommon(optionText, SelectTypes.ByText);
            log.Debug($"        ('{FirstSelector}').Select(optionText '{optionText}')");
        }

        public string GetAttribute(TagAttributes tagAttribute)
        {
            log.Debug($"        ('{FirstSelector}').GetAttribute('{tagAttribute}')");
            return FindSingle().GetAttribute(EnumHelper.GetEnumDescription(tagAttribute));
        }

        #endregion

        #region Additional methods

        public void SwitchContext()
        {
            var element = FindSingle();
            Browser.SwitchToFrame(element);
            log.Debug($"        Browser.SwitchToFrame('{FirstSelector}')");
        }

        public void CacheSearchResult()
        {
            _searchCache = FindIWebElements();
        }

        public void ClearSearchResultCache()
        {
            _searchCache = null;
        }

        public void DragAndDrop(WebElement destination)
        {
            var source = FindSingle();
            var dest = destination.FindSingle();

            Browser.DragAndDrop(source, dest);
        }

        public void FireJQueryEvent(JavaScriptEvents javaScriptEvent)
        {
            var element = FindSingle();
            FireJQueryEvent(element, javaScriptEvent);
        }

        public void ForEach(Action<WebElement> action)
        {
            CacheSearchResult();
            Enumerable.Range(0, Count).ToList().ForEach(i => action(ByIndex(i)));
            ClearSearchResultCache();
        }

        public List<T> Select<T>(Func<WebElement, T> action)
        {
            var result = new List<T>();
            ForEach(e => result.Add(action(e)));
            return result;
        }

        public List<WebElement> Where(Func<WebElement, bool> action)
        {
            var result = new List<WebElement>();
            ForEach(e =>
                {
                    if (action(e)) result.Add(e);
                });
            return result;
        }

        public WebElement Single(Func<WebElement, bool> action)
        {
            return Where(action).Single();
        }

        #endregion

        #region Helpers

        private void Set(bool value, bool useJQuery = true)
        {
            if (Selected ^ value)
            {
                Click(useJQuery);
            }
        }

        private void SelectCommon(string option, SelectTypes selectType)
        {
            var element = FindSingle();
            switch (selectType)
            {
                case SelectTypes.ByValue:
                    new SelectElement(element).SelectByValue(option);
                    return;
                case SelectTypes.ByText:
                    new SelectElement(element).SelectByText(option);
                    return;
                default:
                    throw new Exception(string.Format("Unknown select type: {0}.", selectType));
            }
        }

        private void FireJQueryEvent(IWebElement element, JavaScriptEvents javaScriptEvent)
        {
            var eventName = EnumHelper.GetEnumDescription(javaScriptEvent);
            Browser.ExecuteJavaScript(string.Format("$(arguments[0]).{0}();", eventName), element);
            log.Debug($"        FireJQueryEvent('{FirstSelector}') - $(arguments[0]).{eventName}();");
        }

        #endregion
    }

    public enum JavaScriptEvents
    {
        [Description("keyup")]
        KeyUp,

        [Description("click")]
        Click,

        [Description("mouseover")]
        Mouseover,

        [Description("change")]
        Change
    }
}
