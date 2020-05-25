using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class GenerateParentheses
    {
        //my method is faster
        //but the mem cost is higher
        public IList<string> GenerateParenthesis(int n)
        {
            List<string> res = new List<string>();
            if (n == 0)
                return res;
            int[][] indexers = new int[n][];
            int[] indexOfIndexers = new int[n];
            for (int i = 0; i < n; i++)
            {
                indexers[i] = new int[i + 1];
                indexOfIndexers[i] = 0;
                for (int j = 0; j < i + 1; j++)
                {
                    indexers[i][j] = i + j;
                }
            }
            bool final = false;
            bool[] hasUp = new bool[n];
            while (!final)
            {
                char[] stringChar = new char[2 * n];
                for (int i = 0; i < n; i++)
                {
                    stringChar[indexers[i][indexOfIndexers[i]]] = '(';
                    hasUp[i] = false;
                }
                for (int i = 0; i < 2 * n; i++)
                {
                    if (stringChar[i] == '\0')
                        stringChar[i] = ')';
                }
                res.Add(new string(stringChar));
                indexOfIndexers[n - 1] += 1;
                for (int i = n - 1; i >= 1; i--)
                {
                    if (indexOfIndexers[i] == indexers[i].Length)
                    {
                        hasUp[i] = true;
                        indexOfIndexers[i - 1] += 1;
                    }
                }
                if (indexOfIndexers[0] == indexers[0].Length)
                {
                    final = true;
                }
                else
                {
                    for (int i = 1; i < n; i++)
                    {
                        if (hasUp[i])
                            indexOfIndexers[i] = indexOfIndexers[i - 1];
                    }
                }
            }
            return res;
        }

        //the best speed code is mem stable
        //but the speed is lower than mine
        public IList<string> Solution2(int n)
        {
            List<string> res = new List<string>();
            Generater(res, new char[2 * n], 0);
            return res;
        }

        private void Generater(List<string> list, char[] charArray, int pos)
        {
            if (pos == 0)
            {
                charArray[pos] = '(';
                Generater(list, charArray, pos + 1);
            }
            else if (pos == charArray.Length - 1)
            {
                charArray[pos] = ')';
                if (IsValid(charArray))
                    list.Add(new string(charArray));
            }
            else
            {
                charArray[pos] = '(';
                Generater(list, charArray, pos + 1);
                charArray[pos] = ')';
                Generater(list, charArray, pos + 1);
            }
        }

        private bool IsValid(char[] charArray)
        {
            int balance = 0;
            foreach (char c in charArray)
            {
                if (c == '(')
                {
                    balance++;
                }
                else
                {
                    balance--;
                }

                if (balance < 0)
                {
                    return false;
                }
            }

            if (balance == 0)
                return true;
            else
            {
                return false;
            }
        }
    }
}
