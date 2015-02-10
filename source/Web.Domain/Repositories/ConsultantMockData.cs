using System;
using System.Collections.Generic;
using System.Linq;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class ConsultantMockData
    {
        private static readonly Specialization PmSpecialization = new Specialization
        {
            Name = "Project Manager",
            Skills = new List<Skill> { new Skill { Name = "7 Sigma" } }
        };

        private static readonly Specialization SwDevSpecialization = new Specialization
        {
            Name = "Software Developer",
            Skills = new List<Skill> { new Skill { Name = ".NET" }, new Skill { Name = "Java" } }
        };

        private static Contract CreateMockContract(int consultantId, int clientId)
        {
            return new Contract
            {
                ConsultantId = consultantId,
                ClientId = clientId,
                Title = "Lead Guy",
                StartDate = ToFormattedDate(new DateTime(2011, 11, 4)),
                EndDate = ToFormattedDate(new DateTime(2012, 11, 4)),
                Rate = 120,
                Contact = new Contact
                {
                    FirstName = "Chris",
                    LastName = "Henderson",
                    EmailAddress = "chris.henderson@email.com"
                }
            };
        }

        private static readonly Resume AResume = new Resume
        {
            Text = "Some resume text."
        };

        private static DateTime ToFormattedDate(DateTime date)
        {
            return date;
        }


        public static readonly IQueryable<Consultant> Contractors = new List<Consultant>
        {
            new Consultant
            {
                Id = 12345, 
                FirstName = "Giacomo", LastName = "Guilizzoni",
                Rating = MatchGuideConstants.ResumeRating.AboveStandard,
                Specializations = new List<Specialization>{PmSpecialization}, 
                ResumeText = AResume.Text, Contracts = new List<Contract>{ CreateMockContract(12345, 1) }
            },
            new Consultant
            {
                Id = 23456,
                FirstName = "Bobby", LastName = "Ichnamius",
                Rating = MatchGuideConstants.ResumeRating.Standard,
                Specializations = new List<Specialization>{PmSpecialization}, 
                ResumeText = AResume.Text, Contracts = new List<Contract>{ CreateMockContract(23456, 1) }
            },
            new Consultant
            {
                Id = 34567, 
                FirstName = "Rob", LastName = "Richardson",
                Rating = MatchGuideConstants.ResumeRating.NotChecked,
                Specializations = new List<Specialization>{SwDevSpecialization}, 
                ResumeText = AResume.Text, Contracts = new List<Contract>{ CreateMockContract(34567, 1) }
            },
            new Consultant
            {
                Id = 45678, 
                FirstName = "Ronald", LastName = "Herzl",
                Rating = MatchGuideConstants.ResumeRating.AlsoNotChecked,
                Specializations = new List<Specialization>{SwDevSpecialization, PmSpecialization}, 
                ResumeText = AResume.Text, Contracts = new List<Contract>{ CreateMockContract(45678, 1) }
            },
            new Consultant
            {
                Id = 34567, 
                FirstName = "Tim", LastName = "Thompson",
                Rating = MatchGuideConstants.ResumeRating.BelowStandard,
                Specializations = new List<Specialization>{SwDevSpecialization}, 
                ResumeText = AResume.Text, Contracts = new List<Contract>{ CreateMockContract(34567, 1) }
            }
        }.AsQueryable();
    }
}
