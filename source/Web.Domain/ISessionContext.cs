using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain
{
    public interface ISessionContext
    {
        User CurrentUser { get; }
    }
}
