using System;
using System.Threading;

namespace ClientApp.Core
{
    public interface IActivityManager
    {
        void StartActivity();

        Guid StartActivity(CancellationToken cancellationToken);

        void StopActivity();

        void StopActivity(Guid activityId);
    }
}
