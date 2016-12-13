using System.IO;

namespace Selenium.Utilities
{
    public static class RandomHelper
    {
        public static string RandomString
        {
            get { return Path.GetRandomFileName().Replace(".", string.Empty); }
        }
    }
}
