
namespace SiSystems.ClientApp.Web.Domain.Caching
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
