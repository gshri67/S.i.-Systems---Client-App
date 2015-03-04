using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using ClientApp.Core;
using System.Threading;

namespace ClientApp.iOS
{
    public class iOSActivityManager : IActivityManager
    {
        public Guid StartActivity(CancellationToken cancellationToken)
        {
            return Guid.Empty;
            //throw new NotImplementedException();
        }

        public void StopActivity(Guid activityId)
        {
            //throw new NotImplementedException();
        }
    }
}