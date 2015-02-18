using System.Collections.Generic;
using System.Linq;
using Dapper;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class CompanyRepository
    {
        public IEnumerable<int> GetAllAssociatedCompanyIds(int companyId)
        {
            using (var db = new DatabaseContext(DatabaseSelect.MatchGuide))
            {
                string allCompanyRelationshipsQuery = "SELECT ParentID as ParentId, ChildID as ChildId "
                                                      + "FROM [Company_ParentChildRelationship]";

                var relationships = db.Connection.Query<CompanyRelationship>(allCompanyRelationshipsQuery);

                var companyNode = new TreeNode(companyId);

                var hash = new Dictionary<int, TreeNode>();
                hash.Add(companyId, companyNode);

                foreach (var relationship in relationships)
                {
                    TreeNode child;
                    if (!hash.ContainsKey(relationship.ChildId))
                    {
                        child = new TreeNode(relationship.ChildId);
                        hash.Add(relationship.ChildId, child);
                    }
                    else
                    {
                        child = hash[relationship.ChildId];
                    }

                    TreeNode parent;
                    if (!hash.ContainsKey(relationship.ParentId))
                    {
                        parent = new TreeNode(relationship.ParentId);
                        hash.Add(relationship.ParentId, parent);
                    }
                    else
                    {
                        parent = hash[relationship.ParentId];
                    }

                    parent.Children.Add(child);
                    child.Parent = parent;
                }

                var nodes = new TreeTraverser().GetAllConnectedNodes(companyNode);

                return nodes.Select(n => n.Id);
            }
        }

        private struct CompanyRelationship
        {
            public int ParentId { get; set; }
            public int ChildId { get; set; }
        }

        
    }
}
