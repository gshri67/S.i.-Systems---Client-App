using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class CompanyRepository
    {
        public IEnumerable<int> GetAllAssociatedCompanyIds(int companyId)
        {
            EnsureIndexExists();
            var companyNode = EnsureNode(companyId, _companyIndex);

            var nodes = new TreeTraverser().GetAllConnectedNodes(companyNode);

            return nodes.Select(n => n.Id);
        }

        private static Dictionary<int, TreeNode> _companyIndex;

        private void EnsureIndexExists()
        {
            if (_companyIndex == null)
            {
                BuildCompanyRelationshipIndex();
            }
        }

        private void BuildCompanyRelationshipIndex()
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

            _companyIndex = hash;
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
