using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Repositories.Search;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    public class FullTextSearchExpressionTests
    {

        [Test]
        public void Create_SingleWordSearch_ShouldReturnQuotedPrefixExpression()
        {
            const string query = "hello";
            const string expected = "hello";

            Assert.AreEqual(expected, FullTextSearchExpression.Create(query));
        }

        [Test]
        public void Create_ShouldRemoveReservedExpressionWords()
        {
            const string query = "hello and";
            const string expected = "hello";

            Assert.AreEqual(expected, FullTextSearchExpression.Create(query));
        }

        [Test]
        public void Create_ShouldCombineTokensWithOperator()
        {
            const string query = "Java C++";
            const string expected = "Java AND C++";

            Assert.AreEqual(expected, FullTextSearchExpression.Create(query));
        }

        [Test]
        public void Create_ShouldIgnoreQuotes()
        {
            const string query = "\"";
            const string expected = "*";

            Assert.AreEqual(expected, FullTextSearchExpression.Create(query));
        }
    }
}
