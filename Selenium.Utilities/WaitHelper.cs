using System;
using System.Diagnostics;
using System.Threading;

namespace Selenium.Utilities
{
    public class WaitHelper
    {
        private readonly TimeSpan _timeout;
        private readonly TimeSpan _checkInterval;
        private readonly Stopwatch _stopwatch;
        private bool _isSatisfied = true;

        private WaitHelper(TimeSpan timeout) : this(timeout, TimeSpan.FromSeconds(1))
        {
        }

        private WaitHelper(TimeSpan timeout, TimeSpan checkInterval)
        {
            _timeout = timeout;
            _checkInterval = checkInterval;
            _stopwatch = Stopwatch.StartNew();
        }

        public static WaitHelper WithTimeout(TimeSpan timeout, TimeSpan pollingInterval)
        {
            return new WaitHelper(timeout, pollingInterval);
        }

        public static WaitHelper WithTimeout(TimeSpan timeout)
        {
            return new WaitHelper(timeout);
        }

        public WaitHelper WaitFor(Func<bool> condition)
        {
            if (!_isSatisfied)
            {
                return this;
            }

            while (!condition())
            {
                var sleepAmount = Min(_timeout - _stopwatch.Elapsed, _checkInterval);
                var target = new WebElement.WebElement();

                //Console.Write("\r" + _stopwatch.Elapsed.Seconds + "; " + condition.Method.Name);

                if (sleepAmount < TimeSpan.Zero)
                {
                    _isSatisfied = false;
                    break;
                }

                Thread.Sleep(sleepAmount);
            }
            //Console.WriteLine("\r" + condition() + "; " + condition.Method.Name);

            return this;
        }

        public bool IsSatisfied
        {
            get { return _isSatisfied; }
        }

        public void EnsureSatisfied()
        {
            if (!_isSatisfied)
            {
                throw new TimeoutException();
            }
        }

        public void EnsureSatisfied(string message)
        {
            if (!_isSatisfied)
            {
                throw new TimeoutException(message);
            }
        }

        public static bool SpinWait(Func<bool> condition)
        {
            return SpinWait(condition, TimeSpan.FromSeconds(20), TimeSpan.FromSeconds(1));
        }

        public static bool SpinWait(Func<bool> condition, TimeSpan timeout)
        {
            return SpinWait(condition, timeout, TimeSpan.FromSeconds(1));
        }

        public static bool SpinWait(Func<bool> condition, TimeSpan timeout, TimeSpan pollingInterval)
        {
            return WithTimeout(timeout, pollingInterval).WaitFor(condition).IsSatisfied;
        }

        public static bool Try(Action action)
        {
            Exception exception;

            return Try(action, out exception);
        }

        public static bool Try(Action action, out Exception exception)
        {
            try
            {
                action();
                exception = null;

                return true;
            }
            catch (Exception e)
            {
                exception = e;

                return false;
            }
        }

        public static Func<bool> MakeTry(Action action)
        {
            return () => Try(action);
        }

        private static T Min<T>(T left, T right) where T : IComparable<T>
        {
            return left.CompareTo(right) < 0 ? left : right;
        }
    }
}