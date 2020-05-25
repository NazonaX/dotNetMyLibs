using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class ArrayProblems
    {
        /// <summary>
        /// 给定一个数组，要求在时间复杂度O(n)的情况下，求出最长连续序列的长度
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int LongestConsecutive(int[] nums)
        {
            //使用一个dictionary,java-hashmap，存储每个数字对应的序列长度
            //也即每次的数查看+-1是否存在于dictionary中
            Dictionary<int, int> dict = new Dictionary<int, int>();
            int max = 0;
            for(int i = 0; i < nums.Length; i++)
            {
                if (!dict.ContainsKey(nums[i]))
                {
                    dict.Add(nums[i], 1);
                }
                else
                {
                    continue;
                }
                int left = 0;
                int right = 0;
                if(dict.ContainsKey(nums[i] - 1))
                {
                    left = dict[nums[i] - 1];
                }
                if(dict.ContainsKey(nums[i] + 1))
                {
                    right = dict[nums[i] + 1];
                }
                //同步更新前后端点值
                int addon = 1 + left + right;
                dict[nums[i] - left] = addon;
                dict[nums[i] + right] = addon;
                max = max < addon ? addon : max;
            }
            return max;
        }
        /// <summary>
        /// 给定两个数组gas和cost
        /// gas表示到达所引除可以获得gas[i]个单位的汽油
        /// cost表示从索引i到达索引i+1需要消耗cost[i]个单位的汽油
        /// 假设仅有一个唯一解，并且从起点处正好可以绕一圈，那么求出这个起始点，若无法完成一圈，则返回-1
        /// </summary>
        /// <param name="gas"></param>
        /// <param name="cost"></param>
        /// <returns></returns>
        public int CanCompleteCircuit(int[] gas, int[] cost)
        {
            int length = gas.Length;
            if (length == 0)
                return -1;
            int sum = 0;
            int[] residual = new int[length];
            for(int i = 0; i < length; i++)
            {
                residual[i] = gas[i] - cost[i];
                sum += residual[i];
            }
            if (sum < 0)
                return -1;
            int index = -1;
            for(int i = 0; i < length; i++)
            {
                sum = 0;
                for(int j = i; ;)
                {
                    sum += residual[j];
                    if(sum < 0)
                    {
                        break;
                    }
                    j++;
                    if (j == length)
                        j = 0;
                    if(j == i)
                    {
                        index = i;
                        break;
                    }
                }
                if (index != -1)
                    break;
            }
            return index;
        }
        /// <summary>
        /// 给定一个数组，要求返回一个数
        /// 1、每个ratings所对应的数值必须大于等于1
        /// 2、每个ratings所对应的数值，如果ratings[i]大于它的相邻，那么对应数值也要大于它的相邻锁对应的数值
        /// 3、返回的这个数式ratings所有元素对应数值的综合，要求最小，队列可循环
        /// </summary>
        /// <param name="ratings"></param>
        /// <returns></returns>
        public int CandyCircle(int[] ratings)
        {
            int length = ratings.Length;
            int[] candy = new int[length];
            for(int i = 0; i < length; i++)
                DoRecursionByCandyCircle(ratings, candy, i);
            int sum = 0;
            foreach(int i in candy)
            {
                sum += i;
            }
            return sum;
        }
        private void DoRecursionByCandyCircle(int[] ratings, int[] candy, int index)
        {
            int index_left = index - 1 < 0 ? (ratings.Length - 1) : (index - 1);
            int index_right = index + 1 == ratings.Length ? 0 : (index + 1);
            if(ratings[index] < ratings[index_left] && ratings[index] < ratings[index_right])
            {
                candy[index] = 1;
                return;
            }
            //单边迭代
            if(ratings[index] > ratings[index_left] && ratings[index] > ratings[index_right])
            {
                //左右最大+1
                int indexT = ratings[index_left] >= ratings[index_right] ? index_left : index_right;
                if (candy[indexT] == 0)
                    DoRecursionByCandyCircle(ratings, candy, indexT);
                candy[index] = candy[indexT] + 1;
            }
            else if(ratings[index] == Math.Min(ratings[index_left], ratings[index_right]))
            {
                //左右最小相等
                //优先判左，因为从左往右扫描
                int indexT = ratings[index] == ratings[index_left] ? index_left : index_right;
                if (candy[indexT] == 0)
                    DoRecursionByCandyCircle(ratings, candy, indexT);
                candy[index] = candy[indexT];
            }
            else
            {
                //左右最小+1
                int indexT = ratings[index_right] < ratings[index_left] ? index_right : index_left;
                if (candy[indexT] == 0)
                    DoRecursionByCandyCircle(ratings, candy, indexT);
                candy[index] = candy[indexT] + 1;
            }
        }
        /// <summary>
        /// 要求如上，但是不考虑环，仅考虑直线排列
        /// </summary>
        /// <param name="ratings"></param>
        /// <returns></returns>
        public int Candy(int[] ratings)
        {
            int length = ratings.Length;
            int[] candy = new int[length];
            Array.Fill(candy, 1);
            if (length == 0)
                return 0;
            else if (length == 1)
                return 1;
            for(int i = 1; i < length;i++)
            {
                if (ratings[i] > ratings[i - 1])
                    candy[i] = candy[i - 1] + 1;
            }
            for(int i = length - 2; i >= 0; i--)
            {
                if (ratings[i] > ratings[i + 1])
                    candy[i] = Math.Max(candy[i], candy[i + 1] + 1);
            }
            int sum = 0;
            foreach (int i in candy)
                sum += i;
            return sum;
        }
        /// <summary>
        /// 给定一个数组，保证不为空，并且数组中只有一个数是只出现一次的，其余数字都出现了两遍。要求空间复杂度为O(1)，时间复杂度为O(n)
        /// 返回这个只出现一次的数字
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int SingleNumber(int[] nums)
        {
            int x = nums[0];
            for (int i = 1; i < nums.Length; i++)
                x = x ^ nums[i];
            return x;
        }
        /// <summary>
        /// 给定一个数组，其中除了一个数字，其余都重复三遍
        /// 找出这个数字，要求空间O(1)，时间O(n)
        /// </summary>
        /// <param name="nums"></param>
        /// <returns></returns>
        public int SingleNumber2(int[] nums)
        {
            //可以考虑用位运算来控制每一位的个数，然后余3，结果就是目标当前位的值
            //考虑两个参数分别组成当前位的三种状态，出现一次、出现两次和出现三次
            //当出现三次则消为0
            int one = 0;
            int two = 0;
            int three = 0;
            for(int i = 0; i < nums.Length; i++)
            {
                two = two | (one & nums[i]);
                one = one ^ nums[i];
                //先计算出现两次的情况，因为出现一次会被异或消除
                three = one & two;
                //只有当一次和两次都满足时，三次才能生效。
                //顺序为：one=1, two=0; one=0, two=1; one=1,two=1; 
                one = one & ~three;
                two = two & ~three;
                //只有当three生效时，消除one和two的状态，也就是清0
            }
            return one;
        }
        public int SingleNumber2BestSolution(int[] nums)
        {
            int one = 0;
            int two = 0;
            for(int i = 0; i < nums.Length; i++)
            {
                two = (two ^ nums[i]) & ~one;
                one = (one ^ nums[i]) & ~two;
            }
            //依次的顺序是：one=0,two=1; one=1,two=0;one=0,two=0;
            return two;
        }
    }
}
