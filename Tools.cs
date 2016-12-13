using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Selenium.Utilities;
using log4net;

namespace GophotowebAT
{
    public class Tools
    {
        public static string SiteUrl = "http://www.gophotoweb.ru/";
        public static string PathLogImages = @"c:\Logs\";
        public static string RootFolder = Environment.CurrentDirectory;
        CookieContainer _cookie = new CookieContainer();
        public readonly Random Random = new Random();
        public string Viewstate = string.Empty;
        public string Eventvalidation = string.Empty;
        public readonly List<string> UsersPassedTests = new List<string>();
        protected static readonly ILog log = LogManager.GetLogger(typeof(Tools));

        public string FindRegex(string response, string pattern, int indexGroup = 1)
        {
            var regex = new Regex(pattern);
            var result = regex.Match(response);
            var itemText = result.Groups[indexGroup].ToString();
            return itemText;
        }

        public List<string> FindListOneGroup(string cart, string pattern)
        {
            var regex = new Regex(pattern);
            var result = regex.Match(cart);
            var hrefList = new List<string>();
            while (result.Success)
            {
                var item = result.Groups[1].ToString();
                hrefList.Add(item);
                result = result.NextMatch();
            }
            return hrefList;
        }

        public Dictionary<string, string> FindListTwoGroup(string cart, string pattern)
        {
            var regex = new Regex(pattern);
            var result = regex.Match(cart);
            var hrefList = new Dictionary<string, string>();
            while (result.Success)
            {
                var key = result.Groups[1].ToString();
                var value = result.Groups[2].ToString();
                hrefList.Add(key, value);
                result = result.NextMatch();
            }
            return hrefList;
        }
        
        private string ReplaceWithRegex(string text, string pattern, string repl)
        {
            var regex = new Regex(pattern);
            return regex.Replace(text, repl);
        }
        
        public static void SignOutSelenium()
        {
            Browser.Navigate(new Uri(SiteUrl + "/signout.aspx"));
        }
        
        public static Uri UrlCombine(string parentPage, string url)
        {
            return new Uri(new Uri(parentPage), url);
        }
        
        public static string SaveScreenshotAndDescription(string expectationName, string comment)
        {
            try
            {
                log4net.Config.XmlConfigurator.Configure();
                var screenName = 
                    $"{expectationName}_{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}_{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}"
                    .Replace('(', '_').Replace(')', '_');
                log.Debug($"  @@@  Save screenshot with comment: '{comment}'{Environment.NewLine}'{screenName}.png'");
                if (!Directory.Exists(PathLogImages))
                {
                    Directory.CreateDirectory(PathLogImages);
                }
                Browser.SaveScreenshot(PathLogImages + screenName + ".png");
                Thread.Sleep(5000);
                return screenName;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "EXCEPTION IN SAVESCREENSHOTANDDESCRIPTION: " + ex.Message;
            }
        }

        public static void Log(string message, ConsoleColor color = ConsoleColor.Gray)
        {
            try
            {
                Console.ForegroundColor = color;
                Console.WriteLine(DateTime.Now.ToString("g") + " - " + message);
                if (!Directory.Exists(RootFolder + @"AutomatedTestingLog"))
                {
                    Directory.CreateDirectory(RootFolder + @"AutomatedTestingLog");
                }
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void WaitIndicator(int second)
        {
            for (var i = 0; i < second; i++)
            {
                Thread.Sleep(1000);
                Console.Write("\r{0} - please wait   ", second - i);
            }
            Console.WriteLine("\r");
        }        
    }
}
