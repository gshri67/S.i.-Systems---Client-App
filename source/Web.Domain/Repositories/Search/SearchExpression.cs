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
            return finalTokens.Where(t=>!string.IsNullOrWhiteSpace(t)).Aggregate((a, b) => a + " AND " + b);
        }

        //Special keywords or sequences that could break the full-text query
        private static readonly string[] Replacements = new []
        {
            "~", "!", "&", "|", "*", "[", "]", "(", ")", "/", "\\", "\"", ","
        };

        private static string ScrubQuery(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return string.Empty;

            //apply any replacements
            return Replacements.Aggregate(query, (current, sequence) => current.Replace(sequence, string.Empty));
        }

    }
}
