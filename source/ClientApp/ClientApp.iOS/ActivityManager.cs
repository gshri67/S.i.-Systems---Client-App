using System;
using UIKit;
using ClientApp.Core;
using System.Threading;

namespace ClientApp.iOS
{
    public class ActivityManager : IActivityManager
    {
        private static int _tasks;
        private object synchronize = new object();

        public void StartActivity()
        {
            StartActivity(CancellationToken.None);
        }

        public Guid StartActivity(CancellationToken cancellationToken)
        {
            lock (synchronize)
            {
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    UIApplication.SharedApplication.NetworkActivityIndicatorVisible = ++_tasks > 0;
                });
            }

            return Guid.Empty;
        }

        public void StopActivity()
        {
            StopActivity(Guid.Empty);
        }

        public void StopActivity(Guid activityId)
        {
            lock(synchronize)
            {
                UIApplication.SharedApplication.InvokeOnMainThread(() =>
                {
                    UIApplication.SharedApplication.NetworkActivityIndicatorVisible = --_tasks > 0;
                });
            }
        }
    }
}