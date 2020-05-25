using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class BestTimeToBuyAndSellStock
    {
        /// <summary>
        /// 给定一个数组，代表第i天股票的售价
        /// 返回先买入后卖出的最大收益
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        public int MaxProfit(int[] prices)
        {
            if (prices.Length == 0)
                return 0;
            int max = 0;
            int min = prices[0];
            for(int i = 1; i < prices.Length; i++)
            {
                if(prices[i] < min)
                {
                    min = prices[i];
                }
                else
                {
                    max = max > (prices[i] - min) ? max : (prices[i] - min);
                }
            }
            return max;
        }

        /// <summary>
        /// 给定一个数组，代表第i天的股票售价
        /// 可以多次交易，返回最大收益
        /// 每次交易不能有重叠，也即买了之后只能先卖再买
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        public int MaxProfit2(int[] prices)
        {
            int res = 0;
            int min = 0;
            int max = 0;
            int status = 0;
            if (prices.Length == 0 || prices.Length == 1)
                return 0;
            min = prices[0];
            max = prices[0];
            for(int i = 1; i < prices.Length; i++)
            {
                if(status == 0)
                {
                    //初始状态
                    //遍历一个最小值
                    if(prices[i] < min)
                    {
                        min = prices[i];
                        max = prices[i];
                    }
                    else
                    {
                        max = prices[i];
                        status = 1;
                    }
                }
                else if(status == 1)
                {
                    //遍历一个最大值
                    if(prices[i] >= max)
                    {
                        max = prices[i];
                    }
                    else
                    {
                        res += (max - min);
                        min = prices[i];
                        max = prices[i];
                        status = 0;
                    }
                }
            }
            //遍历完毕查看状态，作尾巴处理
            if(status == 1)
            {
                res += (max - min);
            }
            return res;
        }

        /// <summary>
        /// 还是股票交易，若最多可以交易两笔，那么最大收益怎么算
        /// </summary>
        /// <param name="prices"></param>
        /// <returns></returns>
        public int MaxProfit3(int[] prices)
        {
            int firstB = int.MinValue;
            int firstS = 0;
            int secondB = int.MinValue;
            int secondS = 0;
            for(int i = 0; i < prices.Length; i++)
            {
                //时间点上的连续四个顶点，
                //先后运算的牵制决定了四个顶点的顺序，因此不必担心
                firstB = Math.Max(firstB, 0 - prices[i]);
                firstS = Math.Max(firstS, firstB + prices[i]);
                secondB = Math.Max(secondB, firstS - prices[i]);
                secondS = Math.Max(secondS, secondB + prices[i]);
            }
            return secondS;
        }

    }
}
