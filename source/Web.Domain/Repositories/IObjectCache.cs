using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public interface IObjectCache
    {
        void AddItem(string key, object value);
        object GetItem(string key);
    }
}
