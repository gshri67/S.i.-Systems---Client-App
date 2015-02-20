
namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    /// <summary>
    /// Simple Cache
    /// </summary>
    public interface IObjectCache
    {
        void AddItem(string key, object value);
        object GetItem(string key);
    }
}
