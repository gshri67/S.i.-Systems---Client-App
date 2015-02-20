using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain
{
    public interface ISessionContext
    {
        User CurrentUser { get; }
    }
}
