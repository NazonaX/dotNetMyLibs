using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class IntToRom
    {
        private Dictionary<int, string> RomDict = new Dictionary<int, string>();
        public IntToRom()
        {
            RomDict.Add(1, "I");
            RomDict.Add(4, "IV");
            RomDict.Add(5, "V");
            RomDict.Add(9, "IX");
            RomDict.Add(10, "X");
            RomDict.Add(40, "XL");
            RomDict.Add(50, "L");
            RomDict.Add(90, "XC");
            RomDict.Add(100, "C");
            RomDict.Add(400, "CD");
            RomDict.Add(500, "D");
            RomDict.Add(900, "CM");
            RomDict.Add(1000, "M");
        }

        public String IntToRoman(int num)
        {
            StringBuilder sb = new StringBuilder();
            int tmp = num / 1000;
            num -= tmp * 1000;
            CalCulate1(sb, tmp, 1000);
            tmp = num >= 900 ? 1 : 0;
            Calculate2(sb, tmp, 900, ref num);
            tmp = num >= 500 ? 1 : 0;
            Calculate2(sb, tmp, 500, ref num);
            tmp = num >= 400 ? 1 : 0;
            Calculate2(sb, tmp, 400, ref num);

            tmp = num / 100;
            num -= tmp * 100;
            CalCulate1(sb, tmp, 100);
            tmp = num >= 90 ? 1 : 0;
            Calculate2(sb, tmp, 90, ref num);
            tmp = num >= 50 ? 1 : 0;
            Calculate2(sb, tmp, 50, ref num);
            tmp = num >= 40 ? 1 : 0;
            Calculate2(sb, tmp, 40, ref num);

            tmp = num / 10;
            num -= tmp * 10;
            CalCulate1(sb, tmp, 10);
            tmp = num >= 9 ? 1 : 0;
            Calculate2(sb, tmp, 9, ref num);
            tmp = num >= 5 ? 1 : 0;
            Calculate2(sb, tmp, 5, ref num);
            tmp = num >= 4 ? 1 : 0;
            Calculate2(sb, tmp, 4, ref num);
            CalCulate1(sb, num, 1);

            return sb.ToString();
        }
        private void CalCulate1(StringBuilder sb, int counter, int index)
        {
            while (counter > 0)
            {
                counter--;
                sb.Append(RomDict[index]);
            }
        }
        private void Calculate2(StringBuilder sb, int counter, int index, ref int num)
        {
            if (counter > 0)
            {
                sb.Append(RomDict[index]);
                num -= index;
            }
        }
    }
}
