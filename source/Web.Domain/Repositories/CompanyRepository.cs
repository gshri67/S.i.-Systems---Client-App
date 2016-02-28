using System.Collections.Generic;
using System.Linq;
using Dapper;
using SiSystems.ClientApp.Web.Domain.Caching;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public interface ICompanyRepository
    {
        /// <summary>
        /// Returns IDs of all associated companies/divisions.
        /// Basically the entire hierarchy.
        /// </summary>
        IEnumerable<int> GetAllAssociatedCompanyIds(int companyId);

        int GetCompanyIdByAgreementId(int agreementId);
    }

    public class CompanyRepository : ICompanyRepository
    {
        private readonly IObjectCache _cache;
        private const string IndexCacheKey = "CompanyRepository.RelationshipIndex";

        public CompanyRepository(IObjectCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Returns IDs of all associated companies/divisions.
        /// Basically the entire hierarchy.
        /// </summary>
        public IEnumerable<int> GetAllAssociatedCompanyIds(int companyId)
        {
            var index = GetCompanyRelationshipIndex();
            var companyNode = EnsureNode(companyId, index);

            var nodes = new TreeTraverser().GetAllConnectedNodes(companyNode);

            return nodes.Select(n => n.Id);
        }

        public int GetCompanyIdByAgreementId(int agreementId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string companyFromAgreementQuery = "SELECT CompanyId FROM Agreement WHERE AgreementId = @AgreementId";

                return db.Connection.Query<int>(companyFromAgreementQuery, new {AgreementId = agreementId}).FirstOrDefault();
            }
        }

        /// <summary>
        /// Fetches company relationship index from cache or builds it if it is not present
        /// </summary>
        private Dictionary<int, TreeNode> GetCompanyRelationshipIndex()
        {
            var companyIndex = _cache.GetItem(IndexCacheKey) as Dictionary<int, TreeNode>;
            if (companyIndex == null)
            {
                companyIndex = BuildCompanyRelationshipIndex();
                _cache.AddItem(IndexCacheKey, companyIndex);
            }
            return companyIndex;
        }

        /// <summary>
        /// Fetches all company relationships from the database,
        /// creates a TreeNode entity for each present company
        /// and connects nodes to define their relationships.
        /// </summary>
        /// <returns>A dictionary with entries for each company node.</returns>
        private Dictionary<int, TreeNode> BuildCompanyRelationshipIndex()
        {
            var hash = new Dictionary<int, TreeNode>();

            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                const string allCompanyRelationshipsQuery = "SELECT ParentID as ParentId, ChildID as ChildId "
                                                            + "FROM [Company_ParentChildRelationship]";

                var relationships = db.Connection.Query<dynamic>(allCompanyRelationshipsQuery);

                foreach (var relationship in relationships)
                {
                    var parent = EnsureNode(relationship.ParentId, hash);
                    var child = EnsureNode(relationship.ChildId, hash);

                    parent.Children.Add(child);
                    child.Parent = parent;
                }
            }

            return hash;
        }

        private TreeNode EnsureNode(int id, Dictionary<int, TreeNode> dictionary)
        {
            if (!dictionary.ContainsKey(id))
            {
                dictionary.Add(id, new TreeNode(id));
            }
            return dictionary[id];
        }
    }
}
