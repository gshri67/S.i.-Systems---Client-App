using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SiSystems.ClientApp.SharedModels;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class ConsultantRepository
    {
        private static readonly Specialization PmSpecialization = new Specialization
        {
            Name = "Project Manager",
            Skills = new List<Skill> {new Skill {Name = "7 Sigma"}}
        };

        private static readonly Specialization SwDevSpecialization = new Specialization
        {
            Name = "Software Developer",
            Skills = new List<Skill> {new Skill {Name = ".NET"}, new Skill {Name = "Java"}}
        };

        private static Contract CreateMockContract(int consultantId, int clientId)
        {
            return new Contract
            {
                ConsultantId = consultantId, ClientId = clientId,
                Title = "Lead Guy",
                StartDate = ToFormattedDate(new DateTime(2011, 11, 4)), EndDate = ToFormattedDate(new DateTime(2012, 11, 4)),
                Rate = 120, Rating = 3,
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

        private static string ToFormattedDate(DateTime date)
        {
            return date.ToString("M/dd/yyyy");
        }


        private static readonly IQueryable<Consultant> Contractors = new List<Consultant>
        {
            new Consultant
            {
                Id = 12345, 
                FirstName = "Giacomo", LastName = "Guilizzoni",
                Specializations = new List<Specialization>{PmSpecialization}, 
                Resume = AResume, Contracts = new List<Contract>{ CreateMockContract(12345, 1) }
            },
            new Consultant
            {
                Id = 23456,
                FirstName = "Bobby", LastName = "Ichnamius",
                Specializations = new List<Specialization>{PmSpecialization}, 
                Resume = AResume, Contracts = new List<Contract>{ CreateMockContract(23456, 1) }
            },
            new Consultant
            {
                Id = 34567, 
                FirstName = "Rob", LastName = "Richardson",
                Specializations = new List<Specialization>{SwDevSpecialization}, 
                Resume = AResume, Contracts = new List<Contract>{ CreateMockContract(34567, 1) }
            },
            new Consultant
            {
                Id = 45678, 
                FirstName = "Ronald", LastName = "Herzl",
                Specializations = new List<Specialization>{SwDevSpecialization, PmSpecialization}, 
                Resume = AResume, Contracts = new List<Contract>{ CreateMockContract(45678, 1) }
            },
            new Consultant
            {
                Id = 34567, 
                FirstName = "Tim", LastName = "Thompson",
                Specializations = new List<Specialization>{SwDevSpecialization}, 
                Resume = AResume, Contracts = new List<Contract>{ CreateMockContract(34567, 1) }
            }
        }.AsQueryable();

        public Consultant Find(int id)
        {
            return Contractors.SingleOrDefault(c => c.Id == id);
        }

        public IEnumerable<ConsultantGroup> FindAlumni(string query, int clientId)
        {
            var matchingConsultants = Contractors
                .Where(FilterByNameAndSpecialization(query, clientId));

            var groups = new Dictionary<string, ConsultantGroup>();

            foreach (var consultant in matchingConsultants)
            {
                foreach (var specialization in consultant.Specializations)
                {
                    if (!groups.ContainsKey(specialization.Name))
                    {
                        groups.Add(specialization.Name, new ConsultantGroup{Specialization=specialization.Name});       
                    }
                    var group = groups[specialization.Name];
                    group.Consultants.Add(new ConsultantSummary(consultant));
                }
            }
            return groups.Select(g=>g.Value).OrderBy(g=>g.Specialization);
        }

        private Expression<Func<Consultant, bool>> FilterByNameAndSpecialization(string query, int clientId)
        {
            var invariantQuery = query.ToLowerInvariant();

            return x =>
                x.Contracts.Any(c=>c.ClientId==clientId)
                &&
                (x.FirstName.ToLowerInvariant().Contains(invariantQuery)
                || x.LastName.ToLowerInvariant().Contains(invariantQuery)
                || (x.FirstName.ToLowerInvariant() + " " + x.LastName.ToLowerInvariant()).Contains(invariantQuery)
                || x.Specializations.Any(s=>s.Name.ToLowerInvariant().Contains(invariantQuery)));
        }

    }
}
