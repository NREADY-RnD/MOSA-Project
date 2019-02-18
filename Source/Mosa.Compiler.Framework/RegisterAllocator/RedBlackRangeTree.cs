// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

// Derived from:
// https://github.com/stemarie/redblacktree

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public class RedBlackRangeTree<T> where T : BaseRange
	{
		internal enum Color { Red = 0, Black = 1 };

		private class RedBlackNode<N>
		{
			public RedBlackNode<N> Parent { get; set; }

			public RedBlackNode<N> Lower { get; set; }

			public RedBlackNode<N> Higher { get; set; }

			public Color Color { get; set; }

			public N Data { get; set; }

			public RedBlackNode()
			{
				Color = Color.Red;
				Higher = null;
				Lower = null;
				Parent = null;
			}

			public override string ToString()
			{
				return Data.ToString();
			}
		}

		// the actual tree that holds all the data
		private RedBlackNode<T> tree;

		#region Public Methods

		/// <summary>
		/// Creates an instance of this class.
		/// </summary>
		public RedBlackRangeTree()
		{
			tree = null;
			Size = 0;
		}

		public int Size { get; private set; }

		public void Add(T key)
		{
			if (key == null)
				throw new InvalidOperationException();

			// traverse tree - find where node belongs
			int result = 0;

			// create new node
			var node = new RedBlackNode<T>();
			var temp = tree;

			while (temp != null)
			{
				// find Parent
				node.Parent = temp;

				result = key.CompareTo(temp.Data);

				if (result == 0)
				{
					throw new InvalidOperationException();
				}

				if (result > 0)
				{
					temp = temp.Lower;
				}
				else
				{
					temp = temp.Higher;
				}
			}

			// setup node
			node.Data = key;
			node.Higher = null;
			node.Lower = null;

			// insert node into tree starting at parent's location
			if (node.Parent != null)
			{
				result = node.Data.CompareTo(node.Parent.Data);

				if (result > 0)
				{
					node.Parent.Lower = node;
				}
				else
				{
					node.Parent.Higher = node;
				}
			}
			else
			{
				// first node added
				tree = node;
			}

			// restore red-black properties
			RestoreAfterInsert(node);
			Size++;
		}

		public void Clear()
		{
			tree = null;
			Size = 0;
		}

		public bool Contains(T range)
		{
			// begin at root
			var node = tree;

			// traverse tree until node is found
			while (node != null)
			{
				if (node.Data.Overlaps(range))
					return true;

				int result = range.CompareTo(node.Data);

				if (result < 0)
				{
					node = node.Higher;
				}
				else
				{
					node = node.Lower;
				}
			}

			return false;
		}

		public bool Contains(int at)
		{
			// begin at root
			var node = tree;

			// traverse tree until node is found
			while (node != null)
			{
				if (node.Data.Contains(at))
					return true;

				if (node.Data.IsLessThan(at))
				{
					node = node.Higher;
				}
				else
				{
					node = node.Lower;
				}
			}

			return false;
		}

		public T Get(T range)
		{
			int result;

			// begin at root
			var node = tree;

			// traverse tree until node is found
			while (node != null)
			{
				result = range.CompareTo(node.Data);

				if (result == 0)
				{
					return node.Data;
				}

				if (result < 0)
				{
					node = node.Higher;
				}
				else
				{
					node = node.Lower;
				}
			}

			throw new InvalidOperationException();
		}

		public T Get(int at)
		{
			var node = GetNode(at);

			if (node == null)
				return null;

			return node.Data;
		}

		public List<T> GetAll(T range)
		{
			var results = new List<T>();

			var traverse = new Stack<RedBlackNode<T>>();

			// begin at root
			traverse.Push(tree);

			// traverse tree until node is found
			while (traverse.Count != 0)
			{
				var node = traverse.Pop();

				if (range.Overlaps(node.Data))
				{
					results.Add(node.Data);

					if (node.Higher != null)
					{
						traverse.Push(node.Higher);
					}
					if (node.Lower != null)
					{
						traverse.Push(node.Lower);
					}
				}
				else
				{
					int result = range.CompareTo(node.Data);

					if (result < 0)
					{
						if (node.Higher != null)
						{
							traverse.Push(node.Higher);
						}
					}
					else
					{
						if (node.Lower != null)
						{
							traverse.Push(node.Lower);
						}
					}
				}
			}

			return results;
		}

		public T GetNextAfter(int at)
		{
			var current = GetNodeAtOrAfter(at);

			return current == null ? null : current.Data;
		}

		public int GetMax()
		{
			var node = tree;

			if (node == null || node == null)
				throw new InvalidOperationException();

			// traverse to the extreme right to find the largest key
			while (node.Lower != null)
			{
				node = node.Lower;
			}

			return node.Data.End;
		}

		public int GetMin()
		{
			var node = tree;

			if (node == null || node == null)
				throw new InvalidOperationException();

			// traverse to the extreme left to find the smallest key
			while (node.Higher != null)
			{
				node = node.Higher;
			}

			return node.Data.Start;
		}

		public bool IsEmpty()
		{
			return (tree == null) || (Size == 0);
		}

		public void Remove(T key)
		{
			if (key == null)
			{
				throw new InvalidOperationException();
			}

			// find node
			int result = -1;

			// not found, must search
			var node = tree;

			while (node != null)
			{
				result = key.CompareTo(node.Data);

				if (result == 0)
				{
					break;
				}

				if (result < 0)
				{
					node = node.Higher;
				}
				else
				{
					node = node.Lower;
				}
			}

			if (node == null)
			{
				// key not found
				return;
			}

			Delete(node);

			Size--;
		}

		#endregion Public Methods

		#region Internal Methods

		private RedBlackNode<T> First(RedBlackNode<T> root)
		{
			while (root?.Lower != null)
			{
				root = root.Lower;
			}

			return root;
		}

		private RedBlackNode<T> Next(RedBlackNode<T> p)
		{
			if (p.Lower != null)
			{
				return First(p.Lower);
			}
			while (p.Parent != null)
			{
				//if (!p.Parent.Right || p.Parent.Right != p)
				if (p.Parent.Higher == null || p.Parent.Higher != p)
				{
					return p.Parent;
				}
				else
				{
					p = p.Parent;
				}
			}
			return null;
		}

		private RedBlackNode<T> GetNode(int at)
		{
			// begin at root
			var node = tree;

			// traverse tree until node is found
			while (node != null)
			{
				if (node.Data.Contains(at))
					return node;

				if (node.Data.IsLessThan(at))
				{
					node = node.Higher;
				}
				else
				{
					node = node.Lower;
				}
			}

			return null;
		}

		private RedBlackNode<T> GetNodeAtOrBefore(int at)
		{
			// begin at root
			var node = tree;

			RedBlackNode<T> previous = null;

			// traverse tree until node is found
			while (node != null)
			{
				if (node.Data.Contains(at))
					return node;

				if (node.Data.IsLessThan(at))
				{
					if (node.Higher == null)
						return node;

					previous = node;

					node = node.Higher;
				}
				else
				{
					if (node.Lower == null)
						return previous;

					previous = node;

					node = node.Lower;
				}
			}

			return null;
		}

		private RedBlackNode<T> GetNodeAtOrAfter(int at)
		{
			// begin at root
			var node = tree;

			RedBlackNode<T> previous = null;

			// traverse tree until node is found
			while (node != null)
			{
				if (node.Data.Contains(at))
					return node;

				if (node.Data.IsLessThan(at))
				{
					if (node.Higher == null)
						return previous;

					previous = node;

					node = node.Higher;
				}
				else
				{
					if (node.Lower == null)
						return node;

					previous = node;

					node = node.Lower;
				}
			}

			return null;
		}

		///<summary>
		/// Rebalance the tree by rotating the nodes to the left.
		///</summary>
		private void RotateLeft(RedBlackNode<T> x)
		{
			// pushing node x down and to the Left to balance the tree. x's Right child (y)
			// replaces x (since y > x), and y's Left child becomes x's Right child
			// (since it's < y but > x).

			// get x's Right node, this becomes y
			var y = x.Lower;

			// set x's Right link
			// y's Left child's becomes x's Right child
			x.Lower = y.Higher;

			// modify parents
			if (y.Higher != null)
			{
				// sets y's Left Parent to x
				y.Higher.Parent = x;
			}

			if (y != null)
			{
				// set y's Parent to x's Parent
				y.Parent = x.Parent;
			}

			if (x.Parent != null)
			{
				// determine which side of it's Parent x was on
				if (x == x.Parent.Higher)
				{
					// set Left Parent to y
					x.Parent.Higher = y;
				}
				else
				{
					// set Right Parent to y
					x.Parent.Lower = y;
				}
			}
			else
			{
				// at rbTree, set it to y
				tree = y;
			}

			// link x and y
			// put x on y's Left
			y.Higher = x;
			if (x != null)
			{
				// set y as x's Parent
				x.Parent = y;
			}
		}

		///<summary>
		/// Rebalance the tree by rotating the nodes to the right.
		///</summary>
		private void RotateRight(RedBlackNode<T> x)
		{
			// pushing node x down and to the Right to balance the tree. x's Left child (y)
			// replaces x (since x < y), and y's Right child becomes x's Left child
			// (since it's < x but > y).

			// get x's Left node, this becomes y
			var y = x.Higher;

			// set x's Right link
			// y's Right child becomes x's Left child
			x.Higher = y.Lower;

			// modify parents
			if (y.Lower != null)
			{
				// sets y's Right Parent to x
				y.Lower.Parent = x;
			}

			if (y != null)
			{
				// set y's Parent to x's Parent
				y.Parent = x.Parent;
			}

			// null=rbTree, could also have used rbTree
			if (x.Parent != null)
			{
				// determine which side of it's Parent x was on
				if (x == x.Parent.Lower)
				{
					// set Right Parent to y
					x.Parent.Lower = y;
				}
				else
				{
					// set Left Parent to y
					x.Parent.Higher = y;
				}
			}
			else
			{
				// at rbTree, set it to y
				tree = y;
			}

			// link x and y
			// put x on y's Right
			y.Lower = x;
			if (x != null)
			{
				// set y as x's Parent
				x.Parent = y;
			}
		}

		/// <summary>
		/// Deletes a node from the tree and restores red black properties.
		/// </summary>
		/// <param name="node"></param>
		private void Delete(RedBlackNode<T> node)
		{
			// A node to be deleted will be:
			//		1. a leaf with no children
			//		2. have one child
			//		3. have two children
			// If the deleted node is red, the red black properties still hold.
			// If the deleted node is black, the tree needs rebalancing

			// work node to contain the replacement node
			var x = new RedBlackNode<T>();

			// work node
			RedBlackNode<T> y;

			// find the replacement node (the successor to x) - the node one with
			// at *most* one child.
			if (node.Higher == null || node.Lower == null)
			{
				// node has null as a child
				y = node;
			}
			else
			{
				// node to be deleted has two children, find replacement node which will
				// be the leftmost node greater than node to be deleted

				// traverse right subtree
				y = node.Lower;

				// to find next node in sequence
				while (y.Higher != null)
				{
					y = y.Higher;
				}
			}

			// at this point, y contains the replacement node. it's content will be copied
			// to the values in the node to be deleted

			// x (y's only child) is the node that will be linked to y's old parent.
			if (y.Higher != null)
			{
				x = y.Higher;
			}
			else
			{
				x = y.Lower;
			}

			// replace x's parent with y's parent and
			// link x to proper subtree in parent
			// this removes y from the chain
			x.Parent = y.Parent;
			if (y.Parent != null)
			{
				if (y == y.Parent.Higher)
				{
					y.Parent.Higher = x;
				}
				else
				{
					y.Parent.Lower = x;
				}
			}
			else
			{
				// make x the root node
				tree = x;
			}

			// copy the values from y (the replacement node) to the node being deleted.
			// note: this effectively deletes the node.
			if (y != node)
			{
				node.Data = y.Data;
			}

			if (y.Color == Color.Black)
			{
				RestoreAfterDelete(x);
			}
		}

		private void RestoreAfterDelete(RedBlackNode<T> x)
		{
			// maintain Red-Black tree balance after deleting node

			RedBlackNode<T> y;

			while (x != tree && x.Color == Color.Black)
			{
				// determine sub tree from parent
				if (x == x.Parent.Higher)
				{
					// y is x's sibling
					y = x.Parent.Lower;
					if (y.Color == Color.Red)
					{
						// x is black, y is red - make both black and rotate
						y.Color = Color.Black;
						x.Parent.Color = Color.Red;
						RotateLeft(x.Parent);
						y = x.Parent.Lower;
					}
					if ((y.Higher.Color == Color.Black) && (y.Lower.Color == Color.Black))
					{
						// children are both black

						// change parent to red
						y.Color = Color.Red;

						// move up the tree
						x = x.Parent;
					}
					else
					{
						if (y.Lower.Color == Color.Black)
						{
							y.Higher.Color = Color.Black;
							y.Color = Color.Red;
							RotateRight(y);
							y = x.Parent.Lower;
						}

						y.Color = x.Parent.Color;
						x.Parent.Color = Color.Black;
						y.Lower.Color = Color.Black;
						RotateLeft(x.Parent);
						x = tree;
					}
				}
				else
				{
					// right subtree - same as code above with right and left swapped
					y = x.Parent.Higher;
					if (y.Color == Color.Red)
					{
						y.Color = Color.Black;
						x.Parent.Color = Color.Red;
						RotateRight(x.Parent);
						y = x.Parent.Higher;
					}

					if ((y.Lower.Color == Color.Black) && (y.Higher.Color == Color.Black))
					{
						y.Color = Color.Red;
						x = x.Parent;
					}
					else
					{
						if (y.Higher.Color == Color.Black)
						{
							y.Lower.Color = Color.Black;
							y.Color = Color.Red;
							RotateLeft(y);
							y = x.Parent.Higher;
						}

						y.Color = x.Parent.Color;
						x.Parent.Color = Color.Black;
						y.Higher.Color = Color.Black;
						RotateRight(x.Parent);
						x = tree;
					}
				}
			}

			x.Color = Color.Black;
		}

		private void RestoreAfterInsert(RedBlackNode<T> x)
		{
			// x and y are used as variable names for brevity, in a more formal
			// implementation, you should probably change the names

			RedBlackNode<T> y;

			// maintain red-black tree properties after adding x
			while (x != tree && x.Parent.Color == Color.Red)
			{
				// determine traversal path
				// is it on the Left or Right subtree?
				if (x.Parent == x.Parent.Parent.Higher)
				{
					// get uncle
					y = x.Parent.Parent.Lower;
					if (y?.Color == Color.Red)
					{
						// uncle is red; change x's Parent and uncle to black
						x.Parent.Color = Color.Black;
						y.Color = Color.Black;

						// grandparent must be red. Why? Every red node that is not
						// a leaf has only black children
						x.Parent.Parent.Color = Color.Red;

						// continue loop with grandparent
						x = x.Parent.Parent;
					}
					else
					{
						// uncle is black; determine if x is greater than Parent
						if (x == x.Parent.Lower)
						{
							// yes, x is greater than Parent; rotate Left
							// make x a Left child
							x = x.Parent;
							RotateLeft(x);
						}

						// no, x is less than Parent:
						// make Parent black
						// make grandparent black
						// rotate right
						x.Parent.Color = Color.Black;
						x.Parent.Parent.Color = Color.Red;
						RotateRight(x.Parent.Parent);
					}
				}
				else
				{
					// x's Parent is on the Right subtree
					// this code is the same as above with "Left" and "Right" swapped
					y = x.Parent.Parent.Higher;
					if (y?.Color == Color.Red)
					{
						x.Parent.Color = Color.Black;
						y.Color = Color.Black;
						x.Parent.Parent.Color = Color.Red;
						x = x.Parent.Parent;
					}
					else
					{
						if (x == x.Parent.Higher)
						{
							x = x.Parent;
							RotateRight(x);
						}

						x.Parent.Color = Color.Black;
						x.Parent.Parent.Color = Color.Red;
						RotateLeft(x.Parent.Parent);
					}
				}
			}

			// tree should always be black
			tree.Color = Color.Black;
		}

		#endregion Internal Methods
	}
}
