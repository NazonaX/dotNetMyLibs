using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class FourSums
    {
        public IList<IList<int>> FourSum(int[] nums, int target)
        {
            List<IList<int>> res = new List<IList<int>>();
            if (nums == null || nums.Length < 4)
                return res;
            Array.Sort(nums);
            int index1 = 0, index2 = 0, index3 = 0, index4 = 0;
            int length = nums.Length;
            while(index1 < length - 3)
            {
                index2 = index1 + 1;
                while(index2 < length - 2)
                {
                    index3 = index2 + 1;
                    index4 = length - 1;
                    while(index3 < index4)
                    {
                        int sum = nums[index1] + nums[index2] + nums[index3] + nums[index4];
                        if(sum < target)
                        {
                            while (index3 < index4 && nums[index3] == nums[index3 + 1]) index3++;
                            index3++;
                        }
                        else if(sum > target)
                        {
                            while (index3 < index4 && nums[index4] == nums[index4 - 1]) index4--;
                            index4--;
                        }
                        else
                        {
                            List<int> tmp = new List<int>() { nums[index1], nums[index2], nums[index3], nums[index4] };
                            res.Add(tmp);
                            while (index3 < index4 && nums[index4] == nums[index4 - 1]) index4--;
                            index4--;
                            while (index3 < index4 && nums[index3] == nums[index3 + 1]) index3++;
                            index3++;
                        }
                    }
                    while (index2 < length - 2 && nums[index2] == nums[index2 + 1]) index2++;
                    index2++;
                }
                while (index1 < length - 3 && nums[index1] == nums[index1 + 1]) index1++;
                index1++;
            }
            return res;
        }
    }
}
