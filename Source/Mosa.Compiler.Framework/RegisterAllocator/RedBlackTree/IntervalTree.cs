// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree
{
    /// <summary>
    /// Tree capable of adding arbitrary intervals and performing search queries on them
    /// </summary>
    public class IntervalTree<T> where T : Interval
    {
        internal static IntervalNode<T> Sentinel = new IntervalNode<T>();

        private IntervalNode<T> Root { get; set; }

        public IntervalTree()
        {
            Root = Sentinel;
            Root.Left = Sentinel;
            Root.Right = Sentinel;
            Root.Parent = Sentinel;
        }

        #region Tree searching

        public bool Contains(T interval)
        {
            return SearchSubtree(Root, interval);
        }

        public bool Contains(int val)
        {
            return SearchSubtree(Root, val);
        }

        /// <summary>
        /// Search interval tree for a given point
        /// </summary>
        /// <param name="val">value to be searched for</param>
        /// <returns>list of intervals which contain the value</returns>
        public List<T> Search(int val)
        {
            var result = new List<T>();
            SearchSubtree(Root, val, result);
            return result;
        }

        /// <summary>
        /// Search interval tree for intervals overlapping with given
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public List<T> Search(T i)
        {
            var result = new List<T>();
            SearchSubtree(Root, i, result);
            return result;
        }

        /// <summary>
        /// Searches for the first overlapping interval
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public Interval SearchFirstOverlapping(T i)
        {
            var node = Root;

            while (node != Sentinel && !node.Interval.Overlaps(i))
            {
                if (node.Left != Sentinel && node.Left.MaxEnd.CompareTo(i.Start) > 0)
                {
                    node = node.Left;
                }
                else
                {
                    node = node.Right;
                }
            }

            if (node == Sentinel)
            {
                throw new KeyNotFoundException("No overlapping interval found.");
            }

            return node.Interval;
        }

        public T SearchFirstOverlapping(int val)
        {
            var node = Root;

            while (node != Sentinel && !node.Interval.Overlaps(val))
            {
                if (node.Left != Sentinel && node.Left.MaxEnd.CompareTo(val) > 0)
                {
                    node = node.Left;
                }
                else
                {
                    node = node.Right;
                }
            }

            if (node == Sentinel)
            {
                throw new KeyNotFoundException("No overlapping interval found.");
            }

            return (T)node.Interval;
        }

        private void SearchSubtree(IntervalNode<T> node, T i, List<T> result)
        {
            if (node == Sentinel)
            {
                return;
            }

            if (node.Left != Sentinel)
            {
                SearchSubtree(node.Left, i, result);
            }

            if (i.Overlaps(node.Interval))
            {
                result.Add(node.Interval);
            }

            // Interval start is greater than largest endpoint in this subtree
            if (node.Right != Sentinel && i.Start.CompareTo(node.MaxEnd) < 0)
            {
                SearchSubtree(node.Right, i, result);
            }
        }

        private void SearchSubtree(IntervalNode<T> node, int val, List<T> result)
        {
            if (node == Sentinel)
            {
                return;
            }

            if (node.Left != Sentinel)
            {
                SearchSubtree(node.Left, val, result);
            }

            if (node.Interval.Contains(val))
            {
                result.Add(node.Interval);
            }

            // Interval start is greater than largest endpoint in this subtree
            if (node.Right != Sentinel && val.CompareTo(node.MaxEnd) < 0)
            {
                SearchSubtree(node.Right, val, result);
            }
        }

        private bool SearchSubtree(IntervalNode<T> node, T i)
        {
            if (node == Sentinel)
            {
                return false;
            }

            if (node.Left != Sentinel)
            {
                if (SearchSubtree(node.Left, i))
                    return true;
            }

            if (i.Overlaps(node.Interval))
            {
                return true;
            }

            // Interval start is greater than largest endpoint in this subtree
            if (node.Right != Sentinel && i.Start.CompareTo(node.MaxEnd) < 0)
            {
                return SearchSubtree(node.Right, i);
            }

            return false;
        }

        private bool SearchSubtree(IntervalNode<T> node, int val)
        {
            if (node == Sentinel)
            {
                return false;
            }

            if (node.Left != Sentinel)
            {
                if (SearchSubtree(node.Left, val))
                    return true;
            }

            if (node.Interval.Contains(val))
            {
                return true;
            }

            // Interval start is greater than largest endpoint in this subtree
            if (node.Right != Sentinel && val.CompareTo(node.MaxEnd) < 0)
            {
                return SearchSubtree(node.Right, val);
            }

            return false;
        }

        private IntervalNode<T> FindInterval(IntervalNode<T> tree, T i)
        {
            while (tree != Sentinel)
            {
                if (tree.Interval.CompareTo(i) > 0)
                {
                    tree = tree.Left;
                    continue;
                }

                if (tree.Interval.CompareTo(i) < 0)
                {
                    tree = tree.Right;
                    continue;
                }

                if (tree.Interval.CompareTo(i) == 0)
                {
                    return tree;
                }
            }

            return Sentinel;
        }

        #endregion Tree searching

        /// <summary>
        /// Insert new interval to interval tree
        /// </summary>
        /// <param name="interval">interval to add</param>
        public void Add(T interval)
        {
            Debug.Assert(!Contains(interval));

            var node = new IntervalNode<T>(interval);

            if (Root == Sentinel)
            {
                node.Color = NodeColor.BLACK;
                Root = node;
            }
            else
            {
                InsertInterval(interval, Root);
            }
        }

        #region Tree insertion internals

        /// <summary>
        /// Recursively descends to the correct spot for interval insertion in the tree
        /// When a free spot is found for the node, it is attached and tree state is validated
        /// </summary>
        /// <param name="interval">interval to be added</param>
        /// <param name="currentNode">subtree accessed in recursion</param>
        private void InsertInterval(T interval, IntervalNode<T> currentNode)
        {
            var addedNode = Sentinel;

            var compare = interval.CompareTo(currentNode.Interval);

            if (compare < 0)
            {
                if (currentNode.Left == Sentinel)
                {
                    addedNode = new IntervalNode<T>(interval)
                    {
                        Color = NodeColor.RED
                    };
                    currentNode.Left = addedNode;
                    addedNode.Parent = currentNode;
                }
                else
                {
                    InsertInterval(interval, currentNode.Left);
                    return;
                }
            }
            else if (compare > 0)
            {
                if (currentNode.Right == Sentinel)
                {
                    addedNode = new IntervalNode<T>(interval)
                    {
                        Color = NodeColor.RED
                    };
                    currentNode.Right = addedNode;
                    addedNode.Parent = currentNode;
                }
                else
                {
                    InsertInterval(interval, currentNode.Right);
                    return;
                }
            }
            else
            {
                return;
            }

            addedNode.Parent.RecalculateMaxEnd();

            RenewConstraintsAfterInsert(addedNode);

            Root.Color = NodeColor.BLACK;
        }

        /// <summary>
        /// Validates and applies RB-tree constraints to node
        /// </summary>
        /// <param name="node">node to be validated and fixed</param>
        private void RenewConstraintsAfterInsert(IntervalNode<T> node)
        {
            if (node.Parent == Sentinel)
            {
                return;
            }

            if (node.Parent.Color == NodeColor.BLACK)
            {
                return;
            }

            var uncle = node.Uncle;

            if (uncle != Sentinel && uncle.Color == NodeColor.RED)
            {
                node.Parent.Color = uncle.Color = NodeColor.BLACK;

                var gparent = node.GrandParent;
                if (gparent != Sentinel && !gparent.IsRoot)
                {
                    gparent.Color = NodeColor.RED;
                    RenewConstraintsAfterInsert(gparent);
                }
            }
            else
            {
                if (node.ParentDirection == NodeDirection.LEFT && node.Parent.ParentDirection == NodeDirection.RIGHT)
                {
                    RotateLeft(node.Parent);
                    node = node.Left;
                }
                else if (node.ParentDirection == NodeDirection.RIGHT && node.Parent.ParentDirection == NodeDirection.LEFT)
                {
                    RotateRight(node.Parent);
                    node = node.Right;
                }

                node.Parent.Color = NodeColor.BLACK;

                if (node.GrandParent == Sentinel)
                {
                    return;
                }
                node.GrandParent.Color = NodeColor.RED;

                if (node.ParentDirection == NodeDirection.RIGHT)
                {
                    RotateRight(node.GrandParent);
                }
                else
                {
                    RotateLeft(node.GrandParent);
                }
            }
        }

        #endregion Tree insertion internals

        /// <summary>
        /// Removes interval from tree (if present in tree)
        /// </summary>
        /// <param name="?"></param>
        public void Remove(T interval)
        {
            RemoveNode(FindInterval(Root, interval));
        }

        private void RemoveNode(IntervalNode<T> node)
        {
            if (node == Sentinel)
            {
                return;
            }

            IntervalNode<T> temp = node;
            if (node.Right != Sentinel && node.Left != Sentinel)
            {
                // Trick when deleting node with both children, switch it with closest in order node
                // swap values and delete the bottom node converting it to other cases

                temp = node.GetSuccessor();
                node.Interval = temp.Interval;

                node.RecalculateMaxEnd();
                while (node.Parent != Sentinel)
                {
                    node = node.Parent;
                    node.RecalculateMaxEnd();
                }
            }
            node = temp;
            temp = node.Left != Sentinel ? node.Left : node.Right;

            // we will replace node with temp and delete node
            temp.Parent = node.Parent;
            if (node.IsRoot)
            {
                Root = temp; // Set new root
            }
            else
            {
                // Reattach node to parent
                if (node.ParentDirection == NodeDirection.RIGHT)
                {
                    node.Parent.Left = temp;
                }
                else
                {
                    node.Parent.Right = temp;
                }

                var maxAux = node.Parent;
                maxAux.RecalculateMaxEnd();
                while (maxAux.Parent != Sentinel)
                {
                    maxAux = maxAux.Parent;
                    maxAux.RecalculateMaxEnd();
                }
            }

            if (node.Color == NodeColor.BLACK)
            {
                RenewConstraintsAfterDelete(temp);
            }
        }

        /// <summary>
        /// Ensures constraints still apply after node deletion
        /// - made with the help of algorithm from Cormen et Al. Introduction to Algorithms 2nd ed.
        /// </summary>
        /// <param name="node">The node.</param>
        private void RenewConstraintsAfterDelete(IntervalNode<T> node)
        {
            // Need to bubble up and fix
            while (node != Root && node.Color == NodeColor.BLACK)
            {
                if (node.ParentDirection == NodeDirection.RIGHT)
                {
                    var aux = node.Parent.Right;
                    if (aux.Color == NodeColor.RED)
                    {
                        aux.Color = NodeColor.BLACK;
                        node.Parent.Color = NodeColor.RED;
                        RotateLeft(node.Parent);
                        aux = node.Parent.Right;
                    }

                    if (aux.Left.Color == NodeColor.BLACK && aux.Right.Color == NodeColor.BLACK)
                    {
                        aux.Color = NodeColor.RED;
                        node = node.Parent;
                    }
                    else
                    {
                        if (aux.Right.Color == NodeColor.BLACK)
                        {
                            aux.Left.Color = NodeColor.BLACK;
                            aux.Color = NodeColor.RED;
                            RotateRight(aux);
                            aux = node.Parent.Right;
                        }

                        aux.Color = node.Parent.Color;
                        node.Parent.Color = NodeColor.BLACK;
                        aux.Right.Color = NodeColor.BLACK;
                        RotateLeft(node.Parent);
                        node = Root;
                    }
                }
                else
                {
                    var aux = node.Parent.Left;
                    if (aux.Color == NodeColor.RED)
                    {
                        aux.Color = NodeColor.BLACK;
                        node.Parent.Color = NodeColor.RED;
                        RotateRight(node.Parent);
                        aux = node.Parent.Left;
                    }

                    if (aux.Left.Color == NodeColor.BLACK && aux.Right.Color == NodeColor.BLACK)
                    {
                        aux.Color = NodeColor.RED;
                        node = node.Parent;
                    }
                    else
                    {
                        if (aux.Left.Color == NodeColor.BLACK)
                        {
                            aux.Right.Color = NodeColor.BLACK;
                            aux.Color = NodeColor.RED;
                            RotateLeft(aux);
                            aux = node.Parent.Left;
                        }

                        aux.Color = node.Parent.Color;
                        node.Parent.Color = NodeColor.BLACK;
                        aux.Left.Color = NodeColor.BLACK;
                        RotateRight(node.Parent);
                        node = Root;
                    }
                }
            }

            node.Color = NodeColor.BLACK;
        }

        /// <summary>
        /// General right rotation
        /// </summary>
        /// <param name="node">Top of rotated subtree</param>
        private void RotateRight(IntervalNode<T> node)
        {
            var pivot = node.Left;
            var dir = node.ParentDirection;
            var parent = node.Parent;
            var tempTree = pivot.Right;

            pivot.Right = node;
            node.Parent = pivot;
            node.Left = tempTree;

            if (tempTree != Sentinel)
            {
                tempTree.Parent = node;
            }

            if (dir == NodeDirection.LEFT)
            {
                parent.Right = pivot;
            }
            else if (dir == NodeDirection.RIGHT)
            {
                parent.Left = pivot;
            }
            else
            {
                Root = pivot;
            }

            pivot.Parent = parent;

            pivot.RecalculateMaxEnd();
            node.RecalculateMaxEnd();
        }

        /// <summary>
        /// General left rotation
        /// </summary>
        /// <param name="node">top of rotated subtree</param>
        private void RotateLeft(IntervalNode<T> node)
        {
            var pivot = node.Right;
            var dir = node.ParentDirection;
            var parent = node.Parent;
            var tempTree = pivot.Left;

            pivot.Left = node;
            node.Parent = pivot;
            node.Right = tempTree;

            if (tempTree != Sentinel)
            {
                tempTree.Parent = node;
            }

            if (dir == NodeDirection.LEFT)
            {
                parent.Right = pivot;
            }
            else if (dir == NodeDirection.RIGHT)
            {
                parent.Left = pivot;
            }
            else
            {
                Root = pivot;
            }

            pivot.Parent = parent;

            pivot.RecalculateMaxEnd();
            node.RecalculateMaxEnd();
        }

        #region Enumerators

        private IEnumerable<T> InOrderWalk(IntervalNode<T> node)
        {
            if (node.Left != Sentinel)
            {
                foreach (T val in InOrderWalk(node.Left))
                {
                    yield return val;
                }
            }

            if (node != Sentinel)
            {
                yield return node.Interval;
            }

            if (node.Right != Sentinel)
            {
                foreach (T val in InOrderWalk(node.Right))
                {
                    yield return val;
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var val in InOrderWalk(Root))
            {
                yield return val;
            }
        }

        #endregion Enumerators

        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var node in InOrderWalk(Root))
            {
                sb.Append("[").Append(node.Start).Append("-").Append(node.End).Append("],");
            }

            sb.Length--;

            return sb.ToString();
        }
    }
}
