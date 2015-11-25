using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive
{
    public interface IContractorRepository
    {
        Contractor GetContractorById(int id);
    }
    
    public class ContractorRepository : IContractorRepository
    {
        public Contractor GetContractorById(int id)
        {
            throw new NotImplementedException();
        }
    }

    public class MockContractorRepository : IContractorRepository
    {
        public Contractor GetContractorById(int id)
        {
            return new Contractor
            {
                Id = 1,
                FirstName = "Robert",
                LastName = "Paulson", 
                EmailAddress = "rp.consultant@email.com",
                PhoneNumber = "(555)555-1231",
                Rating = MatchGuideConstants.ResumeRating.Standard,
                ResumeText = string.Empty,
                Specializations = new List<Specialization>
                {
                    DeveloperSpecialization,
                    ProjectManagerSpecialization
                },
                Contracts = new List<ConsultantContract>
                {
                    new ConsultantContract
                    {
                        ClientName = "Cenovus",
                        ClientId = 1,
                    }
                }
            };
        }

        private Specialization DeveloperSpecialization = new Specialization
        {
            Id = 1,
            Name = "SW Developer",
            Description = "Developer",
            Skills = new List<Skill>
            {
                new Skill
                {
                    Id = 1,
                    Name = ".NET",
                    SpecializationId = 1,
                    YearsOfExperience = MatchGuideConstants.YearsOfExperience.TwoToFour
                },
                new Skill
                {
                    Id = 2,
                    Name = "C++",
                    SpecializationId = 1,
                    YearsOfExperience = MatchGuideConstants.YearsOfExperience.FiveToSeven
                },
                new Skill
                {
                    Id = 3,
                    Name = "MVC",
                    SpecializationId = 1,
                    YearsOfExperience = MatchGuideConstants.YearsOfExperience.LessThanTwo
                },
            }
        };

        private Specialization ProjectManagerSpecialization = new Specialization
        {
            Id = 2,
            Name = "Project Manager",
            Description = "Manger",
            Skills = new List<Skill>
            {
                new Skill
                {
                    Id = 4,
                    Name = "7 Sigma",
                    SpecializationId = 2,
                    YearsOfExperience = MatchGuideConstants.YearsOfExperience.TwoToFour
                },
                new Skill
                {
                    Id = 5,
                    Name = "Agile",
                    SpecializationId = 2,
                    YearsOfExperience = MatchGuideConstants.YearsOfExperience.LessThanTwo
                }
            }
        };
    }
}
