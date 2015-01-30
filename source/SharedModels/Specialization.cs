using System.Collections.Generic;

namespace SiSystems.ClientApp.SharedModels
{
    public class Specialization
    {
        public string Name { get; set; }
        public IEnumerable<Skill> Skills { get; set; } 
    }
}
