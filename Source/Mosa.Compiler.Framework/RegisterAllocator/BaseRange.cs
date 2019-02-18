// Copyright (c) MOSA Project. Licensed under the New BSD License.

namespace Mosa.Compiler.Framework.RegisterAllocator
{
	public abstract class BaseRange
	{
		public abstract int Start { get; }
		public abstract int End { get; }

		public int Length { get { return End - Start; } }

		public bool IsSame(BaseRange range)
		{
			return Start == range.Start && End == range.End;
		}

		public bool Overlaps(BaseRange range)
		{
			return Overlaps(range.Start, range.End);
		}

		public bool Overlaps(int start, int end)
		{
			return (Start <= start && End > start) || (start <= Start && end > Start);
		}

		public bool Contains(int at)
		{
			return at >= Start && at < End;
		}

		public bool IsLessThan(BaseRange range)
		{
			return End < range.Start;
		}

		public bool IsLessThan(int at)
		{
			return End < at;
		}

		public bool IsGreaterThan(BaseRange range)
		{
			return Start > range.End;
		}

		public bool IsGreaterThan(int at)
		{
			return Start > at;
		}

		public int CompareTo(BaseRange range)
		{
			if (IsSame(range))
				return 0;

			if (IsLessThan(range))
			{
				return 1;
			}
			else
			{
				return -1;
			}
		}

		public bool IsAdjacent(int start, int end)
		{
			return start == End || end == Start;
		}

		public bool IsAdjacent(BaseRange range)
		{
			return IsAdjacent(range.Start, range.End);
		}

		public override string ToString()
		{
			return $"{Start}-{End}";
		}
	}
}
