using GophotowebAT;
using log4net;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using Selenium.Utilities;
using System;

namespace TestHttpWebRequest.NUnitTests
{
    [TestFixture]
    public class BaseTest
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(BaseTest));

        [SetUp]
        public void SetUpTest()
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Info($"START: {TestContext.CurrentContext.Test.FullName}.");
        }

        [TearDown]
        public void TearDown()
        {
            Browser.AcceptAlert();
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed)
            {
                var screenNane = Tools.SaveScreenshotAndDescription(TestContext.CurrentContext.Test.FullName, "TearDown screen.");
                log.Error($"  @@@  StackTrace: {TestContext.CurrentContext.Result.StackTrace}");
                log.Error($"  @@@  Message: {TestContext.CurrentContext.Result.Message}");
            }
            log.Debug("Close Browser.");
            Browser.Quit();
            log.Info($"FINISH: {TestContext.CurrentContext.Test.FullName}.");
        }

        public static class AssertHelper
        {
            public delegate void Thunk();

            public static void DoesNotThrow<T>(Thunk thunk, string message = "@@@ DoesNotThrow<Exception>") where T : Exception
            {
                try
                {
                    thunk.Invoke();
                }
                catch (T ex)
                {
                    Assert.Fail($"{Environment.NewLine + message + Environment.NewLine}, type: {ex.GetType() + Environment.NewLine}," +
                        $" message: {ex.Message + Environment.NewLine}, stackTrace: {ex.StackTrace + Environment.NewLine}");
                }
            }
        }
    }
}
