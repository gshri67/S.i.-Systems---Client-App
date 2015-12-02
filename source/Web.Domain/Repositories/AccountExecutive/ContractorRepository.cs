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
        IEnumerable<Contractor> GetShortlistedContractorsByJobId(int id);
        IEnumerable<Contractor> GetProposedContractorsByJobId(int id);
        IEnumerable<Contractor> GetCalloutContractorsByJobId(int id); 
    }
    
    public class ContractorRepository : IContractorRepository
    {
        public Contractor GetContractorById(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contractor> GetShortlistedContractorsByJobId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contractor> GetProposedContractorsByJobId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Contractor> GetCalloutContractorsByJobId(int id)
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
                EmailAddresses = new List<string>(){ "rp.consultant@email.com"}.AsEnumerable(),
                PhoneNumbers = new List<string>() { "(555)555-1231", "(555)222-2212" }.AsEnumerable(),
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

        public IEnumerable<Contractor> GetShortlistedContractorsByJobId(int id)
        {
            IEnumerable<Contractor> shortlisted = new List<Contractor>
            {
                LouFerigno,
                PeterGriffin,
                SpiderMan
            }.AsEnumerable();

            return shortlisted;
        }

        public IEnumerable<Contractor> GetProposedContractorsByJobId(int id)
        {
            IEnumerable<Contractor> proposed = new List<Contractor>
            {
                LouFerigno,
                PeterGriffin
            }.AsEnumerable();

            return proposed;
        }

        public IEnumerable<Contractor> GetCalloutContractorsByJobId(int id)
        {
            IEnumerable<Contractor> callouts = new List<Contractor>
            {
                LouFerigno
            }.AsEnumerable();

            return callouts;
        }

        private Contractor LouFerigno = new Contractor
        {
            FirstName = "Lou",
            LastName = "Ferigno",
            BillRate = 176.22f,
            PayRate = 180.00f,
            GrossMargin = 30.00f,
            Markup = 100.22f
        };

        private Contractor PeterGriffin = new Contractor
        {
            FirstName = "Peter",
            LastName = "Griffin",
            BillRate = 176.22f,
            PayRate = 180.00f,
            GrossMargin = 30.00f,
            Markup = 100.22f
        };

        private Contractor SpiderMan = new Contractor
        {
            FirstName = "Spider",
            LastName = "Man",
            BillRate = 176.22f,
            PayRate = 180.00f,
            GrossMargin = 30.00f,
            Markup = 100.22f
        };

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
