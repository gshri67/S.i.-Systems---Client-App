using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Core
{
    public interface IPlatformExceptionHandler
    {
        Task<TResult> HandleAsync<TResult>(Func<Task<TResult>> action);
    }
}
