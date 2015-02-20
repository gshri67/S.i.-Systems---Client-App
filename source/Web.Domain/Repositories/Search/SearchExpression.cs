using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace SiSystems.ClientApp.Web.Domain.Repositories.Search
{
    public class FullTextSearchExpression
    {
        private static readonly HashSet<string> ReservedWords =
            new HashSet<string> { "AND", "OR"};
 
        public static string Create(string query)
        {
            var tokens = Regex.Split(query, "\\s+");

            var finalTokens = tokens.Where(t => !ReservedWords.Contains(t.ToUpperInvariant()));

            var prefixTokens = finalTokens.Select(t => "\"" + t + "*\"");

            return prefixTokens.Aggregate((a, b) => a + " AND " + b);
        }

    }
}
