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
            Skills = new List<Skill> {new Skill {Name = "Skill One"}}
        };

        private static readonly Specialization SwDevSpecialization = new Specialization
        {
            Name = "Software Developer",
            Skills = new List<Skill> {new Skill {Name = "C++"}, new Skill {Name = "Java"}}
        };

        private static readonly Contract AContract = new Contract
        {
            Title = "Lead Guy",
            StartDate = new DateTime(2011, 11, 4),
            EndDate = new DateTime(2012, 11, 4),
            Rate = 120,
            Contact = new Contact
            {
                FirstName = "Chris",
                LastName = "Henderson",
                EmailAddress = "chris.henderson@email.com"
            }
        };

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
                Id = 12345, ClientId = 1,
                FirstName = "Giacomo", LastName = "Guilizzoni",
                MostRecentContractRate = 99.45m, Specializations = new List<Specialization>{PmSpecialization}, 
                StartDate = ToFormattedDate(new DateTime(2011, 11, 4)), EndDate = ToFormattedDate(new DateTime(2012, 11, 4)),
                Resume = AResume,
                MostRecentContractRating = 4, Contracts = new List<Contract>{ AContract }
            },
            new Consultant
            {
                Id = 23456, ClientId = 2, 
                FirstName = "Bobby", LastName = "Ichnamius",
                MostRecentContractRate = 150m, Specializations = new List<Specialization>{PmSpecialization}, 
                StartDate = ToFormattedDate(new DateTime(2011, 11, 4)), EndDate = ToFormattedDate(new DateTime(2012, 11, 4)),
                Resume = AResume,
                MostRecentContractRating = 3, Contracts = new List<Contract>{ AContract }
            },
            new Consultant
            {
                Id = 34567, ClientId = 2, 
                FirstName = "Rob", LastName = "Richardson",
                MostRecentContractRate = 123.45m, Specializations = new List<Specialization>{SwDevSpecialization}, 
                StartDate = ToFormattedDate(new DateTime(2011, 11, 4)), EndDate = ToFormattedDate(new DateTime(2012, 11, 4)),
                Resume = AResume,
                MostRecentContractRating = 2, Contracts = new List<Contract>{ AContract }
            },
            new Consultant
            {
                Id = 45678, ClientId = 1,
                FirstName = "Ronald", LastName = "Herzl",
                MostRecentContractRate = 123.45m, Specializations = new List<Specialization>{SwDevSpecialization, PmSpecialization}, 
                StartDate = ToFormattedDate(new DateTime(2011, 11, 4)), EndDate = ToFormattedDate(new DateTime(2012, 11, 4)),
                Resume = AResume,
                MostRecentContractRating = 3, Contracts = new List<Contract>{ AContract }
            },
            new Consultant
            {
                Id = 34567, ClientId = 1, 
                FirstName = "Tim", LastName = "Thompson",
                MostRecentContractRate = 123.45m, Specializations = new List<Specialization>{SwDevSpecialization}, 
                StartDate = ToFormattedDate(new DateTime(2011, 11, 4)), EndDate = ToFormattedDate(new DateTime(2012, 11, 4)),
                Resume = AResume,
                MostRecentContractRating = 4, Contracts = new List<Contract>{ AContract }
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
                x.ClientId == clientId
                &&
                (x.FirstName.ToLowerInvariant().Contains(invariantQuery)
                || x.LastName.ToLowerInvariant().Contains(invariantQuery)
                || (x.FirstName.ToLowerInvariant() + " " + x.LastName.ToLowerInvariant()).Contains(invariantQuery)
                || x.Specializations.Any(s=>s.Name.ToLowerInvariant().Contains(invariantQuery)));
        }

    }
}
