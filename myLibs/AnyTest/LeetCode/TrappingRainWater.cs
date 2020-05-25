using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class TrappingRainWater
    {
        public int Trap(int[] height)
        {
            int res = 0;
            int length = height.Length;
            int[] tmp = new int[length];
            Array.Copy(height, tmp, length);
            Array.Sort(tmp);
            for(int i = 1; i < length; i++)
            {
                if(tmp[i] != 0 && tmp[i] != tmp[i-1])
                {
                    int level = tmp[i];
                    int ground = tmp[i - 1];
                    int height1 = 0;int height2;
                    int indexHeight1 = 0;int indexHeight2 = 0;
                    bool first = true;
                    for(int j = 0; j < length; j++)
                    {
                        if(height[j] > ground)
                        {
                            if(first)
                            {
                                first = false;
                                height1 = height[j] < level ? height[j] - ground : level - ground;
                                indexHeight1 = j;
                            }
                            else
                            {
                                height2 = height[j] < level ? height[j] - ground : level - ground;
                                indexHeight2 = j;
                                if(indexHeight1 != indexHeight2 - 1)
                                {
                                    //calculate volume
                                    res += (indexHeight2 - indexHeight1 - 1) *
                                        (height1 < height2 ? height1 : height2);
                                }
                                //continous for reset 1 and 2
                                height1 = height2;
                                indexHeight1 = indexHeight2;
                            }
                        }
                    }
                }
            }
            return res;
        }

        public int BestSolution(int[] height)
        {
            int res = 0;
            int length = height.Length;
            int[] canLeftHeight = new int[length];
            int[] canRigthHeight = new int[length];
            for (int i = 1; i < length; i++)
                canLeftHeight[i] = canLeftHeight[i - 1] > height[i - 1] ? canLeftHeight[i - 1] : height[i - 1];
            for (int i = length - 2; i >= 0; i--)
                canRigthHeight[i] = canRigthHeight[i + 1] > height[i + 1] ? canRigthHeight[i + 1] : height[i + 1];
            for(int i =0; i < length; i++)
            {
                int tmp = canLeftHeight[i] < canRigthHeight[i] ? canLeftHeight[i] - height[i] : canRigthHeight[i] - height[i];
                res += 0 < tmp ? tmp : 0;
            }
            return res;
        }
    }
}
