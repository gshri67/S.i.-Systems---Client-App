using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using ClientApp.Core;
using System.Threading;
using System.Collections.ObjectModel;

namespace ClientApp.iOS
{
    public class iOSActivityManager : IActivityManager
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
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = ++_tasks > 0;
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
                UIApplication.SharedApplication.NetworkActivityIndicatorVisible = --_tasks > 0;
            }
        }
    }
}