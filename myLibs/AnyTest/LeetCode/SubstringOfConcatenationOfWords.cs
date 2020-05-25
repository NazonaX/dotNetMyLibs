using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class SubstringOfConcatenationOfWords
    {
        public IList<int> FindSubstring(string s, string[] words)
        {
            List<int> res = new List<int>();
            if (words == null || words.Length == 0 || s.Length == 0)
                return res;
            int lengthString = words.Length * words[0].Length;
            int wordLength = words[0].Length;
            int length = s.Length;
            if (lengthString > length)
                return res;
            //to store all the indecies of sub strings in the string s
            int[][] indexers = new int[words.Length][];
            for(int i = 0; i < words.Length; i++)
            {
                indexers[i] = new int[length - wordLength + 1];
                int counter = 0;
                int k = 0;
                while (k < length)
                {
                    int index = s.IndexOf(words[i], k);
                    if (index != -1)
                    {
                        k = index + 1;
                        indexers[i][counter++] = index;
                    }
                    else
                        break;
                }
                for (; counter < indexers[i].Length; counter++)
                    indexers[i][counter] = -1;
            }
            List<int>[] vars = new List<int>[length];
            for (int i = 0; i < length; i++)
                vars[i] = new List<int>();
            for(int i = 0; i < indexers.Length; i++)
            {
                for (int j = 0; j < indexers[i].Length && indexers[i][j] != -1; j++)
                    vars[indexers[i][j]].Add(i);
            }
            bool[] usedWords = new bool[words.Length];int usedCount = 0;
            for(int i = 0; i < vars.Length; i++)
            {
                if (vars[i].Count == 0)
                    continue;
                for (int j = 0; j < usedWords.Length; j++)
                    usedWords[j] = false;
                usedCount = 0;bool found = false;
                for(int j = 0; j < words.Length && i + wordLength * j < length; j++)
                {
                    List<int> location = vars[i + wordLength * j];
                    if(location.Count != 0)
                    {
                        for(int k = 0; k < location.Count; k++)
                            if (!usedWords[location[k]])
                            {
                                usedWords[location[k]] = true;
                                usedCount++;
                                found = true;
                                break;
                            }
                    }
                    if (!found)
                        break;
                    else
                        found = false;
                }
                if (usedCount == words.Length)
                    res.Add(i);
            }
            return res;
        }
    }
}
