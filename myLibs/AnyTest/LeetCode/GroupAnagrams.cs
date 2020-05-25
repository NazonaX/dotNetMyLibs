using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class GroupAnagrams
    {
        public IList<IList<string>> MySolution(string[] strs)
        {
            IList<IList<string>> res = new List<IList<string>>();
            Dictionary<string, List<string>> dict = new Dictionary<string, List<string>>();
            List<string> keys = new List<string>();
            HashSet<string> keysHas = new HashSet<string>();
            for(int i = 0; i < strs.Length; i++)
            {
                char[] tmpChar = strs[i].ToCharArray();
                Array.Sort(tmpChar);
                string tmp = new string(tmpChar);
                if (!keysHas.Contains(tmp))
                {
                    keys.Add(tmp);//n
                    keysHas.Add(tmp);//1
                    dict.Add(tmp, new List<string>());
                }
                dict[tmp].Add(strs[i]);
            }
            for (int i = 0; i < keys.Count; i++)
                res.Add(dict[keys[i]]);
            return res;
        }
    }
}
