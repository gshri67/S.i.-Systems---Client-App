
namespace SiSystems.SharedModels
{
    public class Skill
    {
        public int Id { get; set; }

        public int SpecializationId { get; set; }
        
        public string Name { get; set; }

        public MatchGuideConstants.YearsOfExperience YearsOfExperience { get; set; }
    }
}
