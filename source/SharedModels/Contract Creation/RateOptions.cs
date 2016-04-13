using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels.Contract_Creation
{
    public class RateOptions
    {
        public IEnumerable<string> RateTypes { get; set; }

        public RateOptions()
        {
            RateTypes = Enumerable.Empty<string>();
        }
    }
}