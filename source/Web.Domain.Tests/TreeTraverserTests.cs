using System.Linq;
using NUnit.Framework;
using SiSystems.ClientApp.Web.Domain.Repositories;

namespace SiSystems.ClientApp.Web.Domain.Tests
{
    [TestFixture]
    public class TreeTraverserTests
    {
        private TreeNode CreateTree()
        {
            TreeNode a = new TreeNode(1),
                b = new TreeNode(2),
                c = new TreeNode(3),
                d = new TreeNode(4);

            a.Children.Add(b);
            b.Parent = a;

            a.Children.Add(c);
            b.Parent = a;

            c.Children.Add(d);
            d.Parent = c;

            return a;
        }

        [Test]
        public void GetAllConnectedNodes_ShouldNotContainDuplicates()
        {
            var rootNode = CreateTree();

            var ids = new TreeTraverser().GetAllConnectedNodes(rootNode).ToList();

            Assert.AreEqual(ids.Count, ids.Distinct().Count());
        }

        [Test]
        public void GetAllConnectedNodes_ShouldContainExpectedIds()
        {
            var rootNode = CreateTree();

            var ids = new TreeTraverser().GetAllConnectedNodes(rootNode).Select(n=>n.Id).ToList();

            Assert.Contains(1, ids);
            Assert.Contains(2, ids);
            Assert.Contains(3, ids);
            Assert.Contains(4, ids);
        }

        [Test]
        public void GetAllConnectedNodes_WhenSingleNode_ShouldReturnSelf()
        {
            var node = new TreeNode(99);
            var ids = new TreeTraverser().GetAllConnectedNodes(node).Select(n => n.Id).ToList();

            Assert.AreEqual(1, ids.Count);
            Assert.Contains(99, ids);
        }
    }
}
