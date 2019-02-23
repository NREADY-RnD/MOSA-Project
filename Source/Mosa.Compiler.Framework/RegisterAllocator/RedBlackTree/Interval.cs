// Copyright (c) MOSA Project. Licensed under the New BSD License.

using System;

namespace Mosa.Compiler.Framework.RegisterAllocator.RedBlackTree
{
    /// <summary>
    /// Representation of bounded interval
    /// </summary>
    public class Interval
    {
        public virtual int Start { get; set; }

        public virtual int End { get; set; }

        public Interval()
        {
        }

        public Interval(int start, int end)
        {
            Start = start;
            End = end;

            if (Start.CompareTo(End) > 0)
            {
                throw new ArgumentException("Start cannot be larger than End of interval");
            }
        }

        public int Length { get { return End - Start; } }

        public bool IsSame(Interval interval)
        {
            return Start == interval.Start && End == interval.End;
        }

        public bool Overlaps(Interval interval)
        {
            return (Start <= interval.Start && End > interval.Start) || (interval.Start <= Start && interval.End > Start);
        }

        public bool Overlaps(int val)
        {
            return Contains(val);
        }

        public bool Contains(Interval interval)
        {
            return Start >= interval.Start && interval.End < End;
        }

        public bool Contains(int val)
        {
            return val <= Start && val < End;
        }

        public int CompareTo(Interval interval)
        {
            if (IsSame(interval))
                return 0;

            if (IsLessThan(interval))
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public int CompareTo(int val)
        {
            if (Contains(val))
                return 0;

            if (IsLessThan(val))
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

        public bool IsLessThan(Interval range)
        {
            return End < range.Start;
        }

        public bool IsLessThan(int at)
        {
            return End < at;
        }

        public bool IsGreaterThan(Interval range)
        {
            return Start > range.End;
        }

        public bool IsGreaterThan(int at)
        {
            return Start > at;
        }

        public bool IsAdjacent(int start, int end)
        {
            return start == End || end == Start;
        }

        public bool IsAdjacent(Interval range)
        {
            return IsAdjacent(range.Start, range.End);
        }

        public override string ToString()
        {
            return $"{Start}-{End}";
        }
    }
}