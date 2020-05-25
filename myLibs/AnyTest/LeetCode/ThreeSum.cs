using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class ThreeSumTuple
    {
        public IList<IList<int>> ThreeSum(int[] nums)
        {
            IList<IList<int>> resList = new List<IList<int>>();
            if (nums == null || nums.Length < 3)
                return resList;
            HashSet<string> hs = new HashSet<string>();
            StringBuilder sb = new StringBuilder();
            Dictionary<int, List<int>> munToIndexDict = new Dictionary<int, List<int>>();
            Array.Sort(nums);
            for(int i = 0; i < nums.Length; i++)
            {
                if (munToIndexDict.ContainsKey(nums[i]))
                {
                    munToIndexDict[nums[i]].Add(i);
                }
                else
                {
                    List<int> lst = new List<int>();
                    lst.Add(i);
                    munToIndexDict.Add(nums[i], lst);
                }
            }
            int index1 = 0;
            int index2 = 1;
            while(true)
            {
                if(index2 == nums.Length)
                {
                    index1++;
                    index2 = index1 + 1;
                }
                if (index2 >= nums.Length)
                    break;
                int tmp = nums[index1] + nums[index2];
                int index3 = index2 + 1;
                if (tmp != 0)
                    tmp = -tmp;
                int canIndex = -1;
                if (munToIndexDict.ContainsKey(tmp))
                {
                    sb.Clear();
                    sb.Append(nums[index1]).Append(nums[index2]).Append(tmp);
                    if (!hs.Contains(sb.ToString()))
                    {
                        List<int> lst = munToIndexDict[tmp];
                        foreach (int t in lst)
                        {
                            canIndex = t >= index3 ? t : -1;
                        }
                    }
                }
                if(canIndex != -1)
                {
                    hs.Add(sb.ToString());
                    List<int> tmplist = new List<int>();
                    tmplist.Add(nums[index1]);
                    tmplist.Add(nums[index2]);
                    tmplist.Add(tmp);
                    resList.Add(tmplist);
                }
                index2++;
            }
            return resList;
        }

        public IList<IList<int>> BestSolution(int[] nums)
        {
            Array.Sort(nums);
            List<IList<int>> res = new List<IList<int>>();
            int len = nums.Length;
            for (int i = 0; i < len; i++)
            {
                int target = -nums[i];
                int left = i + 1;
                int right = len - 1;
                if (target < 0) break;
                while (left < right)
                {
                    int sum = nums[left] + nums[right];
                    if (sum < target) left++;
                    else if (sum > target) right--;
                    else
                    {
                        List<int> tmp = new List<int> { nums[i], nums[left], nums[right] };
                        res.Add(tmp);
                        //可能存在多种计算结果，因为使用的是两端靠拢的二分法。因此以下步骤可以理解为略过相同元素
                        while (left < right && nums[left] == tmp[1]) left++;
                        while (left < right && nums[right] == tmp[2]) right--;
                    }
                }
                //略过相同的主操作数
                while (i + 1 < len && nums[i + 1] == nums[i]) i++;
            }
            return res;
        }
    }
}
