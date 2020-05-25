using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.RealProblems
{
    public class EncodeString
    {
        public static void Solve(string original)
        {
            int length = original.Length;
            StringBuilder sb = new StringBuilder();
            int counter = -1;
            char indexer = '\0';
            char pre = '\0';
            for(int i = 0; i < length; i++)
            {
                indexer = original[i];
                counter++;
                if (pre == '\0')
                    pre = indexer;
                else if(indexer != pre)
                {
                    sb.Append(counter).Append(pre);
                    pre = indexer;
                    counter = 0;
                }
            }
            sb.Append(counter + 1).Append(pre);
            Console.WriteLine(sb.ToString());
            Console.ReadKey();
        }
    }
}
