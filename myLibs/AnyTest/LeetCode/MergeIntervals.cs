using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AnyTest.LeetCode
{
    public class MergeIntervals
    {
        public IList<Interval> Merge(IList<Interval> intervals)
        {
            IList<Interval> res = new List<Interval>();
            Dictionary<int, Interval> dict = new Dictionary<int, Interval>();
            List<int> keys = new List<int>();
            int[] headers = new int[intervals.Count];
            for (int i = 0; i < intervals.Count; i++)
            {
                headers[i] = intervals[i].start;
                if (!keys.Contains(intervals[i].start))
                {
                    keys.Add(headers[i]);
                    dict.Add(headers[i], intervals[i]);
                }
                else
                {
                    dict[headers[i]].end = dict[headers[i]].end > intervals[i].end ? dict[headers[i]].end : intervals[i].end;
                }
            }
            Array.Sort(headers);
            int m = 0;int n = m + 1;
            while(m < headers.Length && n < headers.Length)
            {
                while (n < headers.Length && headers[m] == headers[n])
                    n++;
                if (n == headers.Length)
                    break;
                if(dict[headers[m]].end >= dict[headers[n]].start)
                {
                    dict[headers[m]].end = dict[headers[m]].end < dict[headers[n]].end ?
                        dict[headers[n]].end : dict[headers[m]].end;
                    dict.Remove(headers[n]);
                    while(n < headers.Length && n + 1 < headers.Length && headers[n + 1] == headers[n])
                        n++;
                    n++;
                }
                else
                {
                    m = n;
                    n++;
                }
            }
            for (int i = 0; i < headers.Length; i++)
                if (dict.ContainsKey(headers[i]) && !res.Contains(dict[headers[i]]))
                    res.Add(dict[headers[i]]);
            return res;
        }

        public IList<Interval> BesrSolution(IList<Interval> intervals)
        {
            List<Interval> sortedIntervals = intervals.OrderBy(p => p.start).ToList();
            IList<Interval> res = new List<Interval>();
            if (intervals.Count <= 1)
                return intervals;
            int counter = 0;
            res.Add(sortedIntervals[0]);
            for(int i = 1; i < sortedIntervals.Count; i++)
            {
                if(sortedIntervals[i].start <= res[counter].end)
                {
                    res[counter].end = res[counter].end < sortedIntervals[i].end ?
                        sortedIntervals[i].end : res[counter].end;
                }
                else
                {
                    res.Add(sortedIntervals[i]);
                    counter++;
                }
            }
            return res;
        }
        //注意上述两个算法返回的IList对象中的Interval对象都是原引用，是原来传入列表中的对象，有些会改变
    }
}
