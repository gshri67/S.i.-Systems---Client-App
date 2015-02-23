using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace SiSystems.ClientApp.Web.Domain.Repositories.Search
{
    /// <summary>
    /// Used to generate an expression that can be used
    /// in a SQL Server Full-Text query
    /// </summary>
    public class FullTextSearchExpression
    {
        private static readonly HashSet<string> ReservedWords =
            new HashSet<string> { "AND", "OR"};
 
        public static string Create(string query)
        {
            var tokens = Regex.Split(query ?? string.Empty, "\\s+");

            //Remove reserved words that could break the query
            var finalTokens = tokens.Where(t => !ReservedWords.Contains(t.ToUpperInvariant()));

            //Add wildcard so that prefix matches
            var prefixTokens = finalTokens.Select(t => "\"" + t + "*\"");

            //If there are multiple tokens, combine them with AND
            return prefixTokens.Aggregate((a, b) => a + " AND " + b);
        }

    }
}
