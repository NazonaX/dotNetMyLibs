using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class InsertInterval
    {
        public IList<Interval> Insert(IList<Interval> intervals, Interval newInterval)
        {
            if (intervals.Count == 0)
            {
                intervals.Add(newInterval);
                return intervals;
            }
            int length = intervals.Count;
            if (intervals[0].start > newInterval.end)
                intervals.Insert(0, newInterval);
            else if (intervals[length - 1].end < newInterval.start)
                intervals.Add(newInterval);
            else
            {
                int indexInsert = int.MinValue;
                int indexMerge = int.MinValue;
                for (int i = 0; i < length; i++)
                {
                    if (newInterval.start <= intervals[i].start && newInterval.end >= intervals[i].start
                        || newInterval.start >= intervals[i].start && newInterval.end <= intervals[i].end
                        || newInterval.start <= intervals[i].end && newInterval.end >= intervals[i].end)
                    {
                        indexMerge = i;
                        break;
                    }
                    if (i + 1 < length && newInterval.start > intervals[i].end && newInterval.end < intervals[i + 1].start)
                    {
                        indexInsert = i + 1;
                        break;
                    }
                }
                if (indexInsert != int.MinValue)
                    intervals.Insert(indexInsert, newInterval);
                else
                {
                    bool found = true;
                    bool first = true;
                    while (found)
                    {
                        found = false;
                        intervals[indexMerge].start = intervals[indexMerge].start < newInterval.start ? intervals[indexMerge].start : newInterval.start;
                        intervals[indexMerge].end = intervals[indexMerge].end > newInterval.end ? intervals[indexMerge].end : newInterval.end;

                        if (first)
                        {
                            first = false;
                        }
                        else
                        {
                            intervals.RemoveAt(indexInsert);//unsupported
                        }
                        if (indexMerge == 0 && indexMerge != intervals.Count - 1)
                        {
                            //next 1
                            if (intervals[indexMerge].start <= intervals[indexMerge + 1].start && intervals[indexMerge].end >= intervals[indexMerge + 1].start
                                || intervals[indexMerge].start >= intervals[indexMerge + 1].start && intervals[indexMerge].end <= intervals[indexMerge + 1].end
                                || intervals[indexMerge].start <= intervals[indexMerge + 1].end && intervals[indexMerge].end >= intervals[indexMerge + 1].end)
                            {
                                indexInsert = indexMerge + 1;
                                newInterval = intervals[indexInsert];
                                found = true;
                            }
                        }
                        else if (indexMerge == intervals.Count - 1 && indexMerge != 0)
                        {
                            //pre 1
                            if (intervals[indexMerge].start <= intervals[indexMerge - 1].start && intervals[indexMerge].end >= intervals[indexMerge - 1].start
                                || intervals[indexMerge].start >= intervals[indexMerge - 1].start && intervals[indexMerge].end <= intervals[indexMerge - 1].end
                                || intervals[indexMerge].start <= intervals[indexMerge - 1].end && intervals[indexMerge].end >= intervals[indexMerge - 1].end)
                            {
                                indexInsert = indexMerge - 1;
                                indexMerge = indexMerge - 1;
                                newInterval = intervals[indexInsert];
                                found = true;
                            }
                        }
                        else if (indexMerge == 0 && indexMerge == intervals.Count - 1)
                        {
                            //stun
                            break;
                        }
                        else
                        {
                            //double direction
                            if (intervals[indexMerge].start <= intervals[indexMerge + 1].start && intervals[indexMerge].end >= intervals[indexMerge + 1].start
                                || intervals[indexMerge].start >= intervals[indexMerge + 1].start && intervals[indexMerge].end <= intervals[indexMerge + 1].end
                                || intervals[indexMerge].start <= intervals[indexMerge + 1].end && intervals[indexMerge].end >= intervals[indexMerge + 1].end)
                            {
                                indexInsert = indexMerge + 1;
                                newInterval = intervals[indexInsert];
                                found = true;
                            }
                            else if (intervals[indexMerge].start <= intervals[indexMerge - 1].start && intervals[indexMerge].end >= intervals[indexMerge - 1].start
                                || intervals[indexMerge].start >= intervals[indexMerge - 1].start && intervals[indexMerge].end <= intervals[indexMerge - 1].end
                                || intervals[indexMerge].start <= intervals[indexMerge - 1].end && intervals[indexMerge].end >= intervals[indexMerge - 1].end)
                            {
                                indexInsert = indexMerge - 1;
                                indexMerge = indexMerge - 1;
                                newInterval = intervals[indexInsert];
                                found = true;
                            }
                        }
                    }
                }
            }
            return intervals;
        }

        public IList<Interval> Insert2(IList<Interval> intervals, Interval newInterval)
        {
            IList<Interval> res = new List<Interval>();
            if (intervals.Count == 0)
            {
                res.Add(newInterval);
                return res;
            }
            int length = intervals.Count;
            if (intervals[0].start > newInterval.end)
            {
                res.Add(newInterval);
                foreach (Interval t in intervals)
                    res.Add(t);
            }
            else if (intervals[length - 1].end < newInterval.start)
            {
                foreach (Interval t in intervals)
                    res.Add(t);
                res.Add(newInterval);
            }
            else
            {
                bool index_new = true;
                int index_nowI = 0;
                int index_res = 0;
                for (index_nowI = 0; index_nowI < length; index_nowI++)
                {
                    if (index_new)
                    {
                        if (intervals[index_nowI].end < newInterval.start)
                        {
                            res.Add(intervals[index_nowI]);
                        }
                        else if (intervals[index_nowI].start > newInterval.end)
                        {
                            res.Add(newInterval);
                            res.Add(intervals[index_nowI]);
                            index_new = false;
                        }
                        else
                        {
                            res.Add(intervals[index_nowI]);
                            index_res = index_nowI;
                            intervals[index_res].start = intervals[index_res].start < newInterval.start ? intervals[index_res].start : newInterval.start;
                            intervals[index_res].end = intervals[index_res].end > newInterval.end ? intervals[index_res].end : newInterval.end;
                            index_new = false;
                        }
                    }
                    else
                    {
                        if (intervals[index_nowI].start <= intervals[index_res].start && intervals[index_nowI].end >= intervals[index_res].start
                            || intervals[index_nowI].start >= intervals[index_res].start && intervals[index_nowI].end <= intervals[index_res].end
                            || intervals[index_nowI].start <= intervals[index_res].end && intervals[index_nowI].end >= intervals[index_res].end)
                        {
                            intervals[index_res].start = intervals[index_res].start < intervals[index_nowI].start ? intervals[index_res].start : intervals[index_nowI].start;
                            intervals[index_res].end = intervals[index_res].end > intervals[index_nowI].end ? intervals[index_res].end : intervals[index_nowI].end;
                        }
                        else if (intervals[index_res].end < intervals[index_nowI].start)
                        {
                            res.Add(intervals[index_nowI]);
                            index_res = index_nowI;
                        }
                    }
                }
            }
            return res;
        }
    }
}
