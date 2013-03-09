using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    class BinarySearchTree<T> : IEnumerable<TreeNode<T>>, ICloneable
        where T: struct, IComparable<T> //T is limited to a struct, because I can't think how to make cloning work otherwise
    {
        private TreeNode<T> root;

        public TreeNode<T> Root
        {
            get { return root; }
            set { root = value; }
        }

        public BinarySearchTree()
        {
        }

        #region Deletion

        public void DeleteNode(TreeNode<T> nodeToDelete)
        {
            if (nodeToDelete.RightChild == null && nodeToDelete.LeftChild == null) //the node is a leaf and we just delete the reference to it
            {
                TreeNode<T> parent = FindParentOfNode(nodeToDelete);
                if (parent != null)
                {
                    if (nodeToDelete == parent.LeftChild)
                    {
                        parent.LeftChild = null;
                    }
                    else //parent.RightChild == nodetoDelete
                    {
                        parent.RightChild = null;
                    }
                }
                else //there is only one node in the tree and it is a root
                {
                    this.root = null;
                }
            }

            if (nodeToDelete.RightChild != null && nodeToDelete.LeftChild == null) //the node has one child only, we put the child in its place
            {
                TreeNode<T> parent = FindParentOfNode(nodeToDelete);
                if (parent != null)
                {
                    if (parent.LeftChild == nodeToDelete)
                    {
                        parent.LeftChild = nodeToDelete.RightChild;
                    }
                    else //parent.RightChild == nodeToDelete
                    {
                        parent.RightChild = nodeToDelete.RightChild;
                    }
                }
                else //we are trying to delete the root that has one child
                {
                    this.root = nodeToDelete.RightChild;
                }
            }

            if (nodeToDelete.RightChild == null && nodeToDelete.LeftChild != null) //as above, just it is the other child this time
            {
                TreeNode<T> parent = FindParentOfNode(nodeToDelete);
                if (parent != null)
                {
                    if (parent.LeftChild == nodeToDelete)
                    {
                        parent.LeftChild = nodeToDelete.LeftChild;
                    }
                    else //parent.RightChild == nodeToDelete
                    {
                        parent.RightChild = nodeToDelete.LeftChild;
                    }
                }
                else //we are trying to delete the root that has one child
                {
                    this.root = nodeToDelete.LeftChild;
                }
            }

            if (nodeToDelete.RightChild != null && nodeToDelete.LeftChild != null) //the tricky part: the node has two children
            {
                TreeNode<T> replacement = FindRightMostNodeInTree(nodeToDelete.LeftChild); //see http://en.wikipedia.org/wiki/Binary_search_tree#Deletion
                ReplaceNode(replacement, nodeToDelete);
            }
        }

        private TreeNode<T> FindParentOfNode(TreeNode<T> node)
        {
            if (this.root == node)
            {
                return null;
            }
            else
            {
                return FindParentHelper(this.root, node);
            }
        }

        private TreeNode<T> FindParentHelper(TreeNode<T> currrentNode, TreeNode<T> nodeSearched)
        {
            if (currrentNode.LeftChild == nodeSearched || currrentNode.RightChild == nodeSearched)
            {
                return currrentNode;
            }
            else
            {
                if (nodeSearched.Value.CompareTo(currrentNode.Value) < 0)
                {
                    return FindParentHelper(currrentNode.LeftChild, nodeSearched);
                }
                else
                {
                    return FindParentHelper(currrentNode.RightChild, nodeSearched);
                }
            }
        }

        private TreeNode<T> FindRightMostNodeInTree(TreeNode<T> root)
        {
            while(root.RightChild != null)
            {
                root = root.RightChild;
            }

            return root;
        }

        private void ReplaceNode(TreeNode<T> replacement, TreeNode<T> replaced)
        {
            //first of all, if the replacement has a child (it cannot possibly have two), we need to give it to its parent
            //moreover, as we are looking at the rightmost node, we know that if it has a child, it will be a left child
            if (replacement.LeftChild != null)
            {
                TreeNode<T> replacementParent = FindParentOfNode(replacement);
                if (replacement == replacementParent.LeftChild)
                {
                    replacementParent.LeftChild = replacement.LeftChild;
                }
                else //replacement == replacementParent.RightChild
                {
                    replacementParent.RightChild = replacement.LeftChild;
                }
            }
            else //even if it doesn't have a child, still, we need to set its parent reference to it to null
            {
                TreeNode<T> replacementParent = FindParentOfNode(replacement);
                if (replacement == replacementParent.LeftChild)
                {
                    replacementParent.LeftChild = null;
                }
                else //replacement == replacementParent.RightChild
                {
                    replacementParent.RightChild = null;
                }
            }

            //now we can get to replacing the replaced with the replacement
            // we replace the references that the parent has
            TreeNode<T> replacedParent = FindParentOfNode(replaced);
            if (replacedParent != null)
            {
                if (replacedParent.LeftChild == replaced)
                {
                    replacedParent.LeftChild = replacement;
                }
                else//secondParent.RightChild == second
                {
                    replacedParent.RightChild = replacement;
                }
            }

            //then we replace the references to the children
            replacement.LeftChild = replaced.LeftChild;
            replacement.RightChild = replaced.RightChild;

            //it might, however, turn out that the replacement was a direct child of our replaced and then we get in trouble
            //because then the replacement will have a reference to itself, we check that below
            if (replacement.LeftChild == replacement)
            {
                replacement.LeftChild = null;
            }

            if (replacement.RightChild == replacement)
            {
                replacement.RightChild = null;
            }

            //if one of the nodes is a root, we need to replace the reference to this
            if (this.root == replaced)
            {
                this.root = replacement;
            }
        }

        #endregion

        #region Searching

        /// <summary>
        /// Returns the node that has the value that we are looking for.
        /// </summary>
        /// <param name="nodeValue">The value of the node we are searching.</param>
        /// <returns>Returns the node with the specified value or null if such node doesn't exist.</returns>
        public TreeNode<T> SearchNode(T nodeValue)
        {
            if (this.root.Value.CompareTo(nodeValue) == 0)
            {
                return this.Root;
            }
            else
            {
                return SearchHelper(nodeValue, this.root);
            }
        }

        private TreeNode<T> SearchHelper(T nodeValue, TreeNode<T> currentNode)
        {
            if (nodeValue.CompareTo(currentNode.Value) < 0)
            {
                if (currentNode.LeftChild != null && nodeValue.CompareTo(currentNode.LeftChild.Value) == 0)
                {
                    return currentNode.LeftChild;
                }
                else if (currentNode.LeftChild != null && nodeValue.CompareTo(currentNode.LeftChild.Value) != 0)
                {
                    return SearchHelper(nodeValue, currentNode.LeftChild);
                }
            }
            else //nodeValue.CompareTo(currentNode.Value) > 0 this is the only option, as we wouldn't be here if they were equal
            {
                if (currentNode.RightChild != null && nodeValue.CompareTo(currentNode.RightChild.Value) == 0)
                {
                    return currentNode.RightChild;
                }
                else if (currentNode.RightChild != null && nodeValue.CompareTo(currentNode.RightChild.Value) != 0)
                {
                    return SearchHelper(nodeValue, currentNode.RightChild);
                }
            }

            return null;
        }

        #endregion

        #region Insertion

        public void InsertNode(params TreeNode<T>[] nodes)
        {
            foreach(TreeNode<T> node in nodes)
            {
                InsertNode(node);
            }
        }

        public void InsertNode(TreeNode<T> node)
        {
            if (this.root == null)
            {
                root = node;
            }
            else
            {
                InsertHelper(this.root, node);
            }
        }

        private void InsertHelper(TreeNode<T> currentNode, TreeNode<T> nodeToInsert)
        {
            if (nodeToInsert.Value.CompareTo(currentNode.Value) < 0)
            {
                if (currentNode.LeftChild == null)
                {
                    currentNode.LeftChild = nodeToInsert;
                }
                else
                {
                    InsertHelper(currentNode.LeftChild, nodeToInsert);
                }
            }
            else if (nodeToInsert.Value.CompareTo(currentNode.Value) > 0)
            {
                if (currentNode.RightChild == null)
                {
                    currentNode.RightChild = nodeToInsert;
                }
                else
                {
                    InsertHelper(currentNode.RightChild, nodeToInsert);
                }
            }
            else
            {
                throw new ArgumentException("This node has a value that already exists in the tree.");
            }
        }

        #endregion

        #region Enumeration

        /// <summary>
        /// Performs a BFS enumeration of the tree.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            TreeNode<T> currentNode;
            Queue<TreeNode<T>> bfsQueue = new Queue<TreeNode<T>>();
            if (this.root != null)
            {
                bfsQueue.Enqueue(this.root);
            }
            while (bfsQueue.Count != 0)
            {
                currentNode = bfsQueue.Dequeue();
                if (currentNode.LeftChild != null)
                {
                    bfsQueue.Enqueue(currentNode.LeftChild);
                }
                if (currentNode.RightChild != null)
                {
                    bfsQueue.Enqueue(currentNode.RightChild);
                }
                yield return currentNode;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

#endregion

        #region Equals, GetHashCode, ToString

        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            foreach (TreeNode<T> node in this)
            {
                result.Append(node);
                result.Append(" ");
            }

            return result.ToString();
        }

        public override bool Equals(object obj)
        {
            BinarySearchTree<T> compared = obj as BinarySearchTree<T>;

            if (!this.root.Equals(compared.root))
            {
                return false;
            }

            if(this.root == null) //both are null
            {
                return true;
            }

            Queue<TreeNode<T>> bfsThis = new Queue<TreeNode<T>>();
            Queue<TreeNode<T>> bfsCompared = new Queue<TreeNode<T>>();

            bfsThis.Enqueue(this.root);
            bfsCompared.Enqueue(compared.root);

            while (bfsThis.Count > 0 && bfsCompared.Count > 0)
            {
                //note: Equals compares the values of teh nodes, == and != compare their references; not sure if a good idea
                TreeNode<T> currentNodeThis = bfsThis.Dequeue();
                TreeNode<T> currentNodeCompared = bfsCompared.Dequeue();
                if (!currentNodeCompared.Equals(currentNodeThis))
                {
                    return false;
                }

                if (currentNodeThis.LeftChild != null)
                {
                    bfsThis.Enqueue(currentNodeThis.LeftChild);
                }
                if (currentNodeThis.RightChild != null)
                {
                    bfsThis.Enqueue(currentNodeThis.RightChild);
                }
                if (currentNodeCompared.LeftChild != null)
                {
                    bfsCompared.Enqueue(currentNodeCompared.LeftChild);
                }
                if (currentNodeCompared.RightChild != null)
                {
                    bfsCompared.Enqueue(currentNodeCompared.RightChild);
                }
            }

            if(bfsThis.Count != bfsCompared.Count)
            {
                return false;
            }

            return true;
        }

        public override int GetHashCode()
        {
            int hash = 21;
            foreach(TreeNode<T> node in this)
            {
                hash = hash * 31 + node.GetHashCode();
            }

            return hash;
        }

        #endregion

        public object Clone()
        {
            BinarySearchTree<T> newTree = new BinarySearchTree<T>();
            newTree.Root = (TreeNode<T>)this.Root.Clone();
            return newTree;
        }
    }
}
