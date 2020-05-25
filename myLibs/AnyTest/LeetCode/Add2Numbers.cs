using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class Add2Numbers
    {

        //better
        public int[] Solve(int[] nums, int target)
        {
            for(int i = 0; i < nums.Length - 1; i++)
            {
                for(int j = i + 1; j < nums.Length; j++)
                {
                    if(nums[i] + nums[j] == target)
                    {
                        return new int[] { i, j };
                    }
                }
            }
            return null;
        }

        //normal, the slowest one
        public int[] Sove2(int[] nums, int target)
        {
            int[] slution = new int[2];
            bool found = false;
            for (int i = 0; i < nums.Length - 1; i++)
            {
                for (int j = i + 1; j < nums.Length; j++)
                {
                    if (nums[i] + nums[j] == target)
                    {
                        slution[0] = i;
                        slution[1] = j;
                        found = true;
                        break;
                    }
                }
                if (found)
                    break;
            }
            return slution;
        }

        //the best one
        public int[] Solve3(int[] nums, int target)
        {
            Dictionary<int, int> dict = new Dictionary<int, int>();
            for(int i = 0; i < nums.Length; i++)
            {
                if (dict.ContainsKey(target - nums[i]))
                    return new int[2] { dict[target - nums[i]], i };
                else if(!dict.ContainsKey(nums[i]))
                    dict.Add(nums[i], i);
            }
            return null;
        }

    }
}
