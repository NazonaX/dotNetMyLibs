using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class RomToInt
    {
        private Dictionary<char, int> RomDict = new Dictionary<char, int>();
        public RomToInt()
        {
            RomDict.Add('I', 1);
            RomDict.Add('V', 5);
            RomDict.Add('X', 10);
            RomDict.Add('L', 50);
            RomDict.Add('C', 100);
            RomDict.Add('D', 500);
            RomDict.Add('M', 1000);
        }

        public int RomanToInt(string s)
        {
            int res = 0;
            char ch1, ch2;
            int int1, int2;
            int1 = 0;
            int2 = 1;
            int length = s.Length;
            while(int1 < length)
            {
                if (int2 >= length)
                {
                    ch1 = s[int1];
                    res += RomDict[ch1];
                    int1++;
                }
                else
                {
                    ch1 = s[int1];
                    ch2 = s[int2];
                    res += RomDict[ch1] < RomDict[ch2] ? RomDict[ch2] - RomDict[ch1] : RomDict[ch1];
                    int1 += RomDict[ch1] < RomDict[ch2] ? 2 : 1;
                    int2 += RomDict[ch1] < RomDict[ch2] ? 2 : 1;
                }
            }
            
            return res;
        }

    }
}
