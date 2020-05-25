using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.LeetCode
{
    public class TextJustification
    {
        public IList<string> FullJustify(string[] words, int maxWidth)
        {
            IList<string> res = new List<string>();
            int length = words.Length;
            int counter = 0, useCount = 0;
            int totalCharactorCount = 0;
            StringBuilder sb = new StringBuilder();
            while (true)
            {
                for(useCount = 0, totalCharactorCount = 0;
                    totalCharactorCount <= maxWidth && useCount + counter <length ;
                    useCount++)
                {
                    totalCharactorCount = useCount == 0?
                        totalCharactorCount + words[useCount + counter].Length 
                        : totalCharactorCount + words[useCount + counter].Length + 1;
                }
                useCount = totalCharactorCount > maxWidth ? useCount - 1 : useCount;
                sb.Clear();
                if(counter + useCount != length)
                {
                    int space = useCount - 1;
                    totalCharactorCount -= (words[counter + useCount].Length + 1);
                    if(space == 0)
                    {
                        sb.Append(words[counter]);
                        for (int i = sb.Length; i < maxWidth; i++)
                        {
                            sb.Append(" ");
                        }
                    }
                    else
                    {
                        int spaceTotalCount = maxWidth - totalCharactorCount + space;
                        int residual = spaceTotalCount % space;
                        int spaceAvg = spaceTotalCount / space;
                        for(int i = 0; i < useCount; i++)
                        {
                            sb.Append(words[counter + i]);
                            if (i != useCount - 1)
                            {
                                for (int j = 0; j < spaceAvg; j++)
                                    sb.Append(" ");
                                if (i < residual)
                                    sb.Append(" ");
                            }
                        }
                    }
                    res.Add(sb.ToString());
                    counter += useCount;
                }
                else
                {
                    for(int i = 0; i < useCount; i++)
                    {
                        if (i == 0)
                            sb.Append(words[counter + i]);
                        else
                            sb.Append(" ").Append(words[counter + i]);
                    }
                    for(int i = sb.Length; i < maxWidth; i++)
                    {
                        sb.Append(" ");
                    }
                    res.Add(sb.ToString());
                    break;
                }
            }
            return res;
        }
    }
}
