using System.Collections.Generic;

namespace SiSystems.ClientApp.SharedModels
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<Skill> Skills { get; set; }

        public Specialization()
        {
            Skills = new List<Skill>();
        }
    }
}
