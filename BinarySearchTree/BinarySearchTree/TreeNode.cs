using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinarySearchTree
{
    class TreeNode<T> : ICloneable
        where T: struct, IComparable<T>
    {
        private T value;
        private TreeNode<T> leftChild;
        private TreeNode<T> rightChild;

        public T Value
        {
            get { return this.value; }
            set { this.value = value; } //there is a security breach here, if someone decides to change the value, they will
                                        //disrupt the order of the tree. However, the set needs to be available for the
                                        //binary search tree class. Not sure how to fix this
        }

        internal TreeNode<T> LeftChild
        {
            get { return leftChild; }   //we don't want anyone to be able to change the children, because they might not observe
            set { leftChild = value; }  //the correct order. The user has to use the Insert method instead. This is why we will
                                        //use the internal accessibility.
        }

        internal TreeNode<T> RightChild
        {
            get { return rightChild; } //as above
            set { rightChild = value; }
        }

        public TreeNode(T value)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return this.value.ToString();
        }

        //we build this around the idea that it is the values that are important, not the wrapper that is the node
        public override bool Equals(object obj)
        {
            if (this == null && obj == null)
            {
                return true;
            }

            return this.Value.Equals((obj as TreeNode<T>).Value);
        }

        public override int GetHashCode()
        {
            return this.Value.GetHashCode();
        }

        public object Clone()
        {
            TreeNode<T> newNode = new TreeNode<T>(this.Value); //value will be struct only so this will work
            
            if(this.LeftChild != null)
            {
                newNode.LeftChild = (TreeNode<T>)this.LeftChild.Clone();
            }

            if (this.RightChild != null)
            {
                newNode.RightChild = (TreeNode<T>)this.RightChild.Clone();
            }
            return newNode;
        }
    }
}
