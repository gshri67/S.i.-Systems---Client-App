using System.Collections.Generic;
using System.Linq;

namespace SiSystems.ClientApp.Web.Domain.Repositories
{
    public class TreeNode
    {
        public int Id { get; set; }

        public TreeNode Parent { get; set; }
        public IList<TreeNode> Children { get; private set; }

        public TreeNode(int id)
        {
            Id = id;
            Children = new List<TreeNode>();
        }
    }

    public class TreeTraverser
    {
        public IEnumerable<TreeNode> GetAllConnectedNodes(TreeNode companyNode)
        {
            return ExtractTreeIds(companyNode, new HashSet<int>());
        }

        private IEnumerable<TreeNode> ExtractTreeIds(TreeNode companyNode, HashSet<int> visited)
        {
            if (visited.Contains(companyNode.Id))
                return Enumerable.Empty<TreeNode>();

            visited.Add(companyNode.Id);

            var ids = new List<TreeNode> { companyNode };

            if (companyNode.Parent != null)
            {
                var extractedIds = ExtractTreeIds(companyNode.Parent, visited);
                ids.AddRange(extractedIds);
            }

            foreach (var item in companyNode.Children)
            {
                var extractedIds = ExtractTreeIds(item, visited);
                ids.AddRange(extractedIds);
            }
            return ids;
        }

    }
}
