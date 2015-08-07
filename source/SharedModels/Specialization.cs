using System.Collections.Generic;

namespace SiSystems.SharedModels
{
    public class Specialization
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public IEnumerable<Skill> Skills { get; set; }
    }
}
