using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    class BinarySearchTest
    {
        static void Main(string[] args)
        {
            TreeNode<int> eight = new TreeNode<int>(8);
            TreeNode<int> three = new TreeNode<int>(3);
            TreeNode<int> ten = new TreeNode<int>(10);
            TreeNode<int> one = new TreeNode<int>(1);
            TreeNode<int> six = new TreeNode<int>(6);
            TreeNode<int> fourteen = new TreeNode<int>(14);
            TreeNode<int> four = new TreeNode<int>(4);
            TreeNode<int> seven = new TreeNode<int>(7);
            TreeNode<int> thirteen = new TreeNode<int>(13);

            //test the insertion
            BinarySearchTree<int> testTree = new BinarySearchTree<int>();
            testTree.InsertNode(eight);
            testTree.InsertNode(three, ten, one, six, fourteen, four, seven, thirteen);

            foreach(TreeNode<int> node in testTree)
            {
                Console.WriteLine(node + " left: " + node.LeftChild + " right: " + node.RightChild);
            }

            Console.WriteLine();
            //testing the search
            TreeNode<int> searchTest = testTree.SearchNode(6);
            if (searchTest != null)
            {
                Console.WriteLine("Node {0} was found with children: {1} and {2}", searchTest, searchTest.LeftChild, searchTest.RightChild);
            }
            else
            {
                Console.WriteLine("The node was not found.");
            }

            Console.WriteLine();
            //test the deletion
            testTree.DeleteNode(seven);
            foreach (TreeNode<int> node in testTree)
            {
                Console.WriteLine(node + " left: " + node.LeftChild + " right: " + node.RightChild);
            }

            Console.WriteLine();
            testTree.DeleteNode(six);
            foreach (TreeNode<int> node in testTree)
            {
                Console.WriteLine(node + " left: " + node.LeftChild + " right: " + node.RightChild);
            }

            Console.WriteLine();
            testTree.DeleteNode(three);
            foreach (TreeNode<int> node in testTree)
            {
                Console.WriteLine(node + " left: " + node.LeftChild + " right: " + node.RightChild);
            }

            Console.WriteLine();
            testTree.DeleteNode(eight);
            foreach (TreeNode<int> node in testTree)
            {
                Console.WriteLine(node + " left: " + node.LeftChild + " right: " + node.RightChild);
            }

            Console.WriteLine();
            //test ToString
            Console.WriteLine(testTree.ToString());

            Console.WriteLine();
            //test Equals
            BinarySearchTree<int> newTree = new BinarySearchTree<int>();
            newTree.InsertNode(four);
            //you might notice there is a little problem here. If you enter the same node from an old tree into a new tree,
            //the whole subtree of that node gets automatically transferred to the new tree, because the node itself carries
            //the references to its children

            Console.WriteLine(testTree.ToString());
            Console.WriteLine(newTree.ToString());
            Console.WriteLine(newTree.Equals(testTree));

            BinarySearchTree<int> newNewTree = new BinarySearchTree<int>();
            newNewTree.InsertNode(new TreeNode<int>(4));
            newNewTree.InsertNode(new TreeNode<int>(1), new TreeNode<int>(10), new TreeNode<int>(14), new TreeNode<int>(13));
            Console.WriteLine(newNewTree);
            Console.WriteLine(newNewTree.Equals(newTree));

            Console.WriteLine();
            //test hashcode
            Console.WriteLine(newNewTree.GetHashCode());

            //testClone
            Console.WriteLine(testTree.Clone());
        }
    }
}
