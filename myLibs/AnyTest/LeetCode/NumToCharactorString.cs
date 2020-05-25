using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class NumToCharactorString
    {
        private Dictionary<char, List<char>> NumDict = new Dictionary<char, List<char>>();
        //private char[][] NumChars = new char[8][] {
        //    new char[] { 'a', 'b', 'c' },
        //    new char[] { 'd', 'e', 'f' },
        //    new char[] { 'g', 'h', 'i' },
        //    new char[] { 'j', 'k', 'l' },
        //    new char[] { 'm', 'n', 'o' },
        //    new char[] { 'p', 'q', 'r', 's' },
        //    new char[] { 't', 'u', 'v' },
        //    new char[] { 'w', 'x', 'y', 'z' }
        //};
        public NumToCharactorString()
        {
            NumDict.Add('2', new List<char>() { 'a', 'b', 'c' });
            NumDict.Add('3', new List<char>() { 'd', 'e', 'f' });
            NumDict.Add('4', new List<char>() { 'g', 'h', 'i' });
            NumDict.Add('5', new List<char>() { 'j', 'k', 'l' });
            NumDict.Add('6', new List<char>() { 'm', 'n', 'o' });
            NumDict.Add('7', new List<char>() { 'p', 'q', 'r', 's' });
            NumDict.Add('8', new List<char>() { 't', 'u', 'v' });
            NumDict.Add('9', new List<char>() { 'w', 'x', 'y', 'z' });
        }
        public IList<string> LetterCombinations(string digits)
        {
            List<string> res = new List<string>();
            char[] theStr = new char[digits.Length];
            Cal(theStr, 0, digits, res);
            return res;
        }
        private void Cal(char[] theString, int index, string original, List<string> res)
        {
            if (index == original.Length)
            {
                if (theString.Length != 0)
                    res.Add(new string(theString));
                return;
            }
            List<char> lst = NumDict[original[index]];
            for(int i = 0; i < lst.Count; i++)
            {
                theString[index] = lst[i];
                Cal(theString, index + 1, original, res);
            }
        }
    }
}
