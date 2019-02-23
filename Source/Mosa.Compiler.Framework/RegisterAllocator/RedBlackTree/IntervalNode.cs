﻿// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree
{
    /// <summary>
    /// Node of interval Tree
    /// </summary>
    /// <typeparam name="T">type of interval bounds</typeparam>
    public class IntervalNode<T> where T : Interval
    {
        public IntervalNode<T> Left { get; set; }
        public IntervalNode<T> Right { get; set; }
        public IntervalNode<T> Parent { get; set; }

        /// <summary>
        /// Maximum "end" value of interval in node subtree
        /// </summary>
        public int MaxEnd { get; set; }

        /// <summary>
        /// The interval this node holds
        /// </summary>
        public T Interval;

        /// <summary>
        /// Color of the node used for R-B implementation
        /// </summary>
        public NodeColor Color { get; set; }

        public IntervalNode()
        {
            Parent = Left = Right = IntervalTree<T>.Sentinel;
            Color = NodeColor.BLACK;
        }

        public IntervalNode(T interval) : this()
        {
            MaxEnd = interval.End;
            Interval = interval;
        }

        /// <summary>
        /// Indicates whether the node has a parent
        /// </summary>
        public bool IsRoot
        {
            get { return Parent == IntervalTree<T>.Sentinel; }
        }

        /// <summary>
        /// Indicator whether the node has children
        /// </summary>
        public bool IsLeaf
        {
            get { return Right == IntervalTree<T>.Sentinel && Left == IntervalTree<T>.Sentinel; }
        }

        /// <summary>
        /// The direction of the parent, from the child point-of-view
        /// </summary>
        public NodeDirection ParentDirection
        {
            get
            {
                if (Parent == IntervalTree<T>.Sentinel)
                {
                    return NodeDirection.NONE;
                }

                return Parent.Left == this ? NodeDirection.RIGHT : NodeDirection.LEFT;
            }
        }

        public IntervalNode<T> GetSuccessor()
        {
            if (Right == IntervalTree<T>.Sentinel)
            {
                return IntervalTree<T>.Sentinel;
            }

            var node = Right;
            while (node.Left != IntervalTree<T>.Sentinel)
            {
                node = node.Left;
            }

            return node;
        }

        public int CompareTo(IntervalNode<T> other)
        {
            return Interval.CompareTo(other.Interval);
        }

        /// <summary>
        /// Refreshes the MaxEnd value after node manipulation
        ///
        /// This is a local operation only
        /// </summary>
        public void RecalculateMaxEnd()
        {
            int max = Interval.End;

            if (Right != IntervalTree<T>.Sentinel)
            {
                if (Right.MaxEnd.CompareTo(max) > 0)
                {
                    max = Right.MaxEnd;
                }
            }

            if (Left != IntervalTree<T>.Sentinel)
            {
                if (Left.MaxEnd.CompareTo(max) > 0)
                {
                    max = Left.MaxEnd;
                }
            }

            MaxEnd = max;

            if (Parent != IntervalTree<T>.Sentinel)
            {
                Parent.RecalculateMaxEnd();
            }
        }

        /// <summary>
        /// Return grandparent node
        /// </summary>
        /// <returns>grandparent node or IntervalTree<T>.Sentinel if none</returns>
        public IntervalNode<T> GrandParent
        {
            get
            {
                if (Parent != IntervalTree<T>.Sentinel)
                {
                    return Parent.Parent;
                }
                return IntervalTree<T>.Sentinel;
            }
        }

        /// <summary>
        /// Returns sibling of parent node
        /// </summary>
        /// <returns>sibling of parent node or IntervalTree<T>.Sentinel if none</returns>
        public IntervalNode<T> Uncle
        {
            get
            {
                var gparent = GrandParent;
                if (gparent == IntervalTree<T>.Sentinel)
                {
                    return IntervalTree<T>.Sentinel;
                }

                if (Parent == gparent.Left)
                {
                    return gparent.Right;
                }

                return gparent.Left;
            }
        }

        /// <summary>
        /// Returns sibling node
        /// </summary>
        /// <returns>sibling node or IntervalTree<T>.Sentinel if none</returns>
        public IntervalNode<T> Sibling
        {
            get
            {
                if (Parent != IntervalTree<T>.Sentinel)
                {
                    if (Parent.Right == this)
                    {
                        return Parent.Left;
                    }

                    return Parent.Right;
                }

                return IntervalTree<T>.Sentinel;
            }
        }
    }

    public enum NodeColor
    {
        RED,
        BLACK
    }

    public enum NodeDirection
    {
        LEFT,
        RIGHT,
        NONE
    }
}