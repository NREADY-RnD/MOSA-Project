// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;
using System.Collections.Generic;

// Derived from:
// https://github.com/stemarie/redblacktree

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public class RedBlackRangeTree<T> where T : BaseRange
	{
		public enum Color { Red = 0, Black = 1 };

		public class RedBlackNode<N>
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

			// create new node
			var newNode = new RedBlackNode<T>
			{
				Data = key
			};

			var workNode = tree;

			while (workNode != null)
			{
				// find Parent
				newNode.Parent = workNode;
				int result = key.CompareTo(workNode.Data);

				if (result == 0)
				{
					throw new InvalidOperationException();
				}

				workNode = result > 0 ? workNode.Higher : workNode.Lower;
			}

			// insert node into tree starting at parent's location
			if (newNode.Parent != null)
			{
				if (newNode.Data.CompareTo(newNode.Parent.Data) > 0)
				{
					newNode.Parent.Higher = newNode;
				}
				else
				{
					newNode.Parent.Lower = newNode;
				}
			}
			else
			{   // first node added
				tree = newNode;
			}

			// restore red-black properties
			BalanceTreeAfterInsert(newNode);

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

				node = result > 0 ? node.Higher : node.Lower;
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

				node = result < 0 ? node.Lower : node.Higher;
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

					if (result > 0)
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
			while (node.Higher != null)
			{
				node = node.Higher;
			}

			return node.Data.End;
		}

		public int GetMin()
		{
			var node = tree;

			if (node == null || node == null)
				throw new InvalidOperationException();

			// traverse to the extreme left to find the smallest key
			while (node.Lower != null)
			{
				node = node.Lower;
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

				node = result > 0 ? node.Higher : node.Lower;
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

		public RedBlackNode<T> GetNodeAtOrBefore(int at)
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

		public RedBlackNode<T> GetNodeAtOrAfter(int at)
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
		private void RotateLeft(RedBlackNode<T> rotateNode)
		{
			// pushing node rotateNode down and to the Left to balance the tree. rotateNode's Right child (_workNode)
			// replaces rotateNode (since _workNode > rotateNode), and _workNode's Left child becomes rotateNode's Right child
			// (since it's < _workNode but > rotateNode).

			// get rotateNode's Right node, this becomes _workNode
			var workNode = rotateNode.Higher;

			// set rotateNode's Right link
			// _workNode's Left child's becomes rotateNode's Right child
			rotateNode.Higher = workNode.Lower;

			// modify parents
			if (workNode.Lower != null)
			{
				// sets _workNode's Left Parent to rotateNode
				workNode.Lower.Parent = rotateNode;
			}

			if (workNode != null)
			{  // set _workNode's Parent to rotateNode's Parent
				workNode.Parent = rotateNode.Parent;
			}

			if (rotateNode.Parent != null)
			{
				// determine which side of it's Parent rotateNode was on
				if (rotateNode == rotateNode.Parent.Lower)
				{
					// set Left Parent to _workNode
					rotateNode.Parent.Lower = workNode;
				}
				else
				{
					// set Right Parent to _workNode
					rotateNode.Parent.Higher = workNode;
				}
			}
			else
			{
				// at rbTree, set it to _workNode
				tree = workNode;
			}

			// link rotateNode and _workNode
			// put rotateNode on _workNode's Left
			workNode.Lower = rotateNode;

			// set _workNode as rotateNode's Parent
			if (rotateNode != null)
			{
				rotateNode.Parent = workNode;
			}
		}

		///<summary>
		/// Rebalance the tree by rotating the nodes to the right.
		///</summary>
		private void RotateRight(RedBlackNode<T> rotateNode)
		{
			// pushing node rotateNode down and to the Right to balance the tree. rotateNode's Left child (_workNode)
			// replaces rotateNode (since rotateNode < _workNode), and _workNode's Right child becomes rotateNode's Left child
			// (since it's < rotateNode but > _workNode).

			// get rotateNode's Left node, this becomes _workNode
			var workNode = rotateNode.Lower;

			// set rotateNode's Right link
			// _workNode's Right child becomes rotateNode's Left child
			rotateNode.Lower = workNode.Higher;

			// modify parents
			if (workNode.Higher != null)
			{
				// sets _workNode's Right Parent to rotateNode
				workNode.Higher.Parent = rotateNode;
			}

			if (workNode != null)
			{
				// set _workNode's Parent to rotateNode's Parent
				workNode.Parent = rotateNode.Parent;
			}

			// null=rbTree, could also have used rbTree
			if (rotateNode.Parent != null)
			{
				// determine which side of it's Parent rotateNode was on
				if (rotateNode == rotateNode.Parent.Higher)
				{
					// set Right Parent to _workNode
					rotateNode.Parent.Higher = workNode;
				}
				else
				{       // set Left Parent to _workNode
					rotateNode.Parent.Lower = workNode;
				}
			}
			else
			{  // at rbTree, set it to _workNode
				tree = workNode;
			}

			// link rotateNode and _workNode
			// put rotateNode on _workNode's Right
			workNode.Higher = rotateNode;

			// set _workNode as rotateNode's Parent
			if (rotateNode != null)
			{
				rotateNode.Parent = workNode;
			}
		}

		/// <summary>
		/// Deletes a node from the tree and restores red black properties.
		/// </summary>
		/// <param name="node"></param>
		private void Delete(RedBlackNode<T> deleteNode)
		{
			// A node to be deleted will be:
			//		1. a leaf with no children
			//		2. have one child
			//		3. have two children
			// If the deleted node is red, the red black properties still hold.
			// If the deleted node is black, the tree needs rebalancing

			// work node
			RedBlackNode<T> workNode;

			// find the replacement node (the successor to x) - the node one with
			// at *most* one child.
			if (deleteNode.Lower == null || deleteNode.Higher == null)
			{
				// node has sentinel as a child
				workNode = deleteNode;
			}
			else
			{
				// z has two children, find replacement node which will
				// be the leftmost node greater than z
				// traverse right subtree
				workNode = deleteNode.Higher;

				// to find next node in sequence
				while (workNode.Lower != null)
				{
					workNode = workNode.Lower;
				}
			}

			// at this point, y contains the replacement node. it's content will be copied
			// to the values in the node to be deleted

			// x (y's only child) is the node that will be linked to y's old parent.
			var linkedNode = workNode.Lower != null ? workNode.Lower : workNode.Higher;

			// replace x's parent with y's parent and
			// link x to proper subtree in parent
			// this removes y from the chain
			linkedNode.Parent = workNode.Parent;

			if (workNode.Parent != null)
			{
				if (workNode == workNode.Parent.Lower)
				{
					workNode.Parent.Lower = linkedNode;
				}
				else
				{
					workNode.Parent.Higher = linkedNode;
				}
			}
			else
			{
				// make x the root node
				tree = linkedNode;
			}

			// copy the values from y (the replacement node) to the node being deleted.
			// note: this effectively deletes the node.
			if (workNode != deleteNode)
			{
				deleteNode.Data = workNode.Data;
			}

			if (workNode.Color == Color.Black)
			{
				BalanceTreeAfterDelete(linkedNode);
			}
		}

		private void BalanceTreeAfterDelete(RedBlackNode<T> linkedNode)
		{
			// maintain Red-Black tree balance after deleting node
			while (linkedNode != tree && linkedNode.Color == Color.Black)
			{
				RedBlackNode<T> workNode;

				// determine sub tree from parent
				if (linkedNode == linkedNode.Parent.Lower)
				{
					// y is x's sibling
					workNode = linkedNode.Parent.Higher;

					if (workNode.Color == Color.Red)
					{
						// x is black, y is red - make both black and rotate
						linkedNode.Parent.Color = Color.Red;
						workNode.Color = Color.Black;
						RotateLeft(linkedNode.Parent);
						workNode = linkedNode.Parent.Higher;
					}

					if (workNode.Lower.Color == Color.Black && workNode.Higher.Color == Color.Black)
					{
						// children are both black
						// change parent to red
						workNode.Color = Color.Red;

						// move up the tree
						linkedNode = linkedNode.Parent;
					}
					else
					{
						if (workNode.Higher.Color == Color.Black)
						{
							workNode.Lower.Color = Color.Black;
							workNode.Color = Color.Red;
							RotateRight(workNode);
							workNode = linkedNode.Parent.Higher;
						}

						linkedNode.Parent.Color = Color.Black;
						workNode.Color = linkedNode.Parent.Color;
						workNode.Higher.Color = Color.Black;
						RotateLeft(linkedNode.Parent);
						linkedNode = tree;
					}
				}
				else
				{   // right subtree - same as code above with right and left swapped
					workNode = linkedNode.Parent.Lower;

					if (workNode.Color == Color.Red)
					{
						linkedNode.Parent.Color = Color.Red;
						workNode.Color = Color.Black;
						RotateRight(linkedNode.Parent);
						workNode = linkedNode.Parent.Lower;
					}

					if (workNode.Higher.Color == Color.Black && workNode.Lower.Color == Color.Black)
					{
						workNode.Color = Color.Red;
						linkedNode = linkedNode.Parent;
					}
					else
					{
						if (workNode.Lower.Color == Color.Black)
						{
							workNode.Higher.Color = Color.Black;
							workNode.Color = Color.Red;
							RotateLeft(workNode);
							workNode = linkedNode.Parent.Lower;
						}

						workNode.Color = linkedNode.Parent.Color;
						linkedNode.Parent.Color = Color.Black;
						workNode.Lower.Color = Color.Black;
						RotateRight(linkedNode.Parent);
						linkedNode = tree;
					}
				}
			}

			linkedNode.Color = Color.Black;
		}

		private void BalanceTreeAfterInsert(RedBlackNode<T> insertedNode)
		{
			// x and y are used as variable names for brevity, in a more formal
			// implementation, you should probably change the names

			// maintain red-black tree properties after adding newNode
			while (insertedNode != tree && insertedNode.Parent.Color == Color.Red)
			{
				// Parent node is .Colored red;
				RedBlackNode<T> workNode;

				if (insertedNode.Parent == insertedNode.Parent.Parent.Lower)    // determine traversal path
				{
					// is it on the Left or Right subtree?
					workNode = insertedNode.Parent.Parent.Higher;            // get uncle

					if (workNode != null && workNode.Color == Color.Red)
					{
						// uncle is red; change x's Parent and uncle to black
						insertedNode.Parent.Color = Color.Black;
						workNode.Color = Color.Black;

						// grandparent must be red. Why? Every red node that is not
						// a leaf has only black children
						insertedNode.Parent.Parent.Color = Color.Red;
						insertedNode = insertedNode.Parent.Parent;  // continue loop with grandparent
					}
					else
					{
						// uncle is black; determine if newNode is greater than Parent
						if (insertedNode == insertedNode.Parent.Higher)
						{
							// yes, newNode is greater than Parent; rotate Left
							// make newNode a Left child
							insertedNode = insertedNode.Parent;
							RotateLeft(insertedNode);
						}

						// no, newNode is less than Parent
						insertedNode.Parent.Color = Color.Black;    // make Parent black
						insertedNode.Parent.Parent.Color = Color.Red;       // make grandparent black
						RotateRight(insertedNode.Parent.Parent);                    // rotate right
					}
				}
				else
				{
					// newNode's Parent is on the Right subtree
					// this code is the same as above with "Left" and "Right" swapped
					workNode = insertedNode.Parent.Parent.Lower;

					if (workNode != null && workNode.Color == Color.Red)
					{
						insertedNode.Parent.Color = Color.Black;
						workNode.Color = Color.Black;
						insertedNode.Parent.Parent.Color = Color.Red;
						insertedNode = insertedNode.Parent.Parent;
					}
					else
					{
						if (insertedNode == insertedNode.Parent.Lower)
						{
							insertedNode = insertedNode.Parent;
							RotateRight(insertedNode);
						}

						insertedNode.Parent.Color = Color.Black;
						insertedNode.Parent.Parent.Color = Color.Red;
						RotateLeft(insertedNode.Parent.Parent);
					}
				}
			}

			tree.Color = Color.Black;       // rbTree should always be black
		}

		#endregion Internal Methods
	}
}
