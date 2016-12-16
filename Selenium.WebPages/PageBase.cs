using System;
using Selenium.Utilities.WebElement;
using GophotowebAT;
using Selenium.Utilities;

namespace Selenium.WebPages
{
    public abstract class PageBase
    {
        public Uri BaseUrl
        {
            get
            {
                Browser.WaitAjax();
                return GetUriByRelativePath(RelativePath);
            }
        }

        public void Open()
        {
            Browser.WaitAjax();
            if (Browser.Url == BaseUrl) return;
            Browser.Navigate(BaseUrl);
        }

        public Type PageName()
        {
            return GetType();
        }

        public bool TextExists(string text, bool exactMach = true, int timeout = 5)
        {
            return new WebElement().ByText(text, exactMach).Exists(timeout);
        }

        protected void Navigate(Uri url)
        {
            Browser.Navigate(url);
        }

        protected static Uri GetUriByRelativePath(string relativePath)
        {
            return new Uri(new Uri(Tools.SiteUrl), relativePath.ToLower());
        }

        private string RelativePath
        {
            get
            {
                const string rootNamespaceName = "GophotowebAT.Selenium.WebPages.";
                var className = GetType().FullName;

                var path = className
                    .Replace(".AdminSite", string.Empty)
                    .Replace(".CustomerSite", string.Empty)
                    .Replace(rootNamespaceName, string.Empty)
                    .Replace(".", "/");

                path = path.Replace("Homepage", "");

                return path;
            }
        }
    }
}
