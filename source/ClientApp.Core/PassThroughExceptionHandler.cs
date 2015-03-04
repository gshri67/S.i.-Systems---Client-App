using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public class PassThroughExceptionHandler : IPlatformExceptionHandler
    {
        public async Task<TResult> HandleAsync<TResult>(Func<Task<TResult>> action)
        {
            return await action();
        }
    }
}
