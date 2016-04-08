using System.Collections.Generic;
using System.Linq;

namespace SiSystems.SharedModels.Contract_Creation
{
    public class RateOptions
    {
        public IEnumerable<string> RateTypeOptions { get; set; }
        public IEnumerable<string> RateDescriptionOptions { get; set; }

        public RateOptions()
        {
            RateTypeOptions = Enumerable.Empty<string>();
            RateDescriptionOptions = Enumerable.Empty<string>();
        }
    }
}