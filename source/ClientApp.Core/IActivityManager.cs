using System;
using System.Threading;

namespace ClientApp.Core
{
    public interface IActivityManager
    {
        void StartActivity();

        void StopActivity();
    }
}
