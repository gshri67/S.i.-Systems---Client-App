using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Context
{
    /// <summary>
    /// Represents state of the current request,
    /// including information about the current authenticated user.
    /// </summary>
    public interface ISessionContext
    {
        User CurrentUser { get; }
    }
}
