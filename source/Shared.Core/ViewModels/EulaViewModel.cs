using System.Collections.Generic;
using Newtonsoft.Json;

namespace Shared.Core.ViewModels
{
    public class EulaViewModel : ViewModelBase
    {
        private Dictionary<string, int> _eulaVersions;

        public EulaViewModel(Dictionary<string, int> eulaVersions)
        {
            _eulaVersions = eulaVersions ?? new Dictionary<string, int>();
        }

        public void AcceptEula(string username, int version)
        {
            if (_eulaVersions.ContainsKey(username))
            {
                _eulaVersions[username] = version;
            }
            else
            {
                _eulaVersions.Add(username, version);
            }
        }

        public string GetUpdatedStorageString()
        {
            return JsonConvert.SerializeObject(_eulaVersions);
        }
    }
}
