using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiSystems.ClientApp.Web.Domain.Repositories.AccountExecutive;
using SiSystems.SharedModels;
using SiSystems.Web.Domain.Context;

namespace SiSystems.ClientApp.Web.Domain.Services.AccountExecutive
{
    public class UserContactService
    {
        private readonly IUserContactRepository _repo;
        private readonly ISessionContext _context;

        public UserContactService(IUserContactRepository repo, ISessionContext context)
        {
            _repo = repo;
            _context = context;
        }

        public UserContact GetUserContactById(int id)
        {
            var contact = _repo.GetUserContactById(id);

            PopulateLinkedInUrl(contact);

            return contact;
        }

        public IEnumerable<UserContact> GetClientContacts()
        {
            var clientContacts = _repo.GetClientContacts();

            return clientContacts;
        }

        public IEnumerable<UserContact> Find(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<UserContact>();

            IEnumerable<UserContact> contacts;

            query = ScrubQuery(query);

            if (IsUsingWildCard(query))
            {
                string[] wildCardQueries = query.Split('%');

                if (query.Length >= 2)
                {
                    contacts = _repo.FindUsersWithWildCardSearch(wildCardQueries[0], wildCardQueries[1]);
                    return contacts;
                }
            }

            query = query.Trim('%');

            contacts = _repo.FindUsers(query);

            return contacts; 
        }

        private bool IsUsingWildCard(string query)
        {
            if (query.Count(ch => ch == '%') == 2)
                return true;
            return false;
        }

        //Special keywords or sequences that could break the full-text query
        private static readonly string[] Replacements = new[]
        {
            "~", "!", "&", "|", "*", "[", "]", "(", ")", "/", "\\", "\"", ",", ";"
        };

        private static string ScrubQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return string.Empty;

            //apply any replacements
            return Replacements.Aggregate(query, (current, sequence) => current.Replace(sequence, string.Empty));
        }

        private void PopulateLinkedInUrl(UserContact contact)
        {
            if (contact == null)
                return;

            contact.LinkedInUrl = string.Format("{0}{1}",
                "https://www.linkedin.com/vsearch/f?type=all&keywords=", contact.FullName.Replace(" ", "+"));
        }
    }
}
