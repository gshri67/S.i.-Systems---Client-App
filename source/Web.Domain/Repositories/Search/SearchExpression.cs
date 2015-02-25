using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace SiSystems.ClientApp.Web.Domain.Repositories.Search
{
    /// <summary>
    /// Used to generate an expression that can be used
    /// in a SQL Server Full-Text query
    /// 
    /// We are intentionally not supporting partial word matches using wildcards
    /// as this has a significant impact on performance.
    /// In our testing, building a wildcard query, such as "Java*" could take up to 15 seconds with test data,
    /// whereas a simple match search for "Java" is sub-second.
    /// </summary>
    public class FullTextSearchExpression
    {
        private static readonly HashSet<string> ReservedWords =
            new HashSet<string> { "AND", "OR"};
 
        public static string Create(string query)
        {
            var cleanQuery = ScrubQuery(query);

            if (string.IsNullOrWhiteSpace(cleanQuery))
                return "*";

            var tokens = Regex.Split(cleanQuery, "\\s+");

            //Remove reserved words that could break the query
            var finalTokens = tokens.Where(t => !ReservedWords.Contains(t.ToUpperInvariant()));
            
            //If there are multiple tokens, combine them with AND
            return finalTokens.Aggregate((a, b) => a + " AND " + b);
        }

        private static string ScrubQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return string.Empty;

            return query
                .Replace("\"", string.Empty)
                .Replace("*", string.Empty);
        }

    }
}
