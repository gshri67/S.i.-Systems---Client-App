﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
