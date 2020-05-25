using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NazonaX.MyLibs.Extends;

namespace AnyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

            //LeetCode.ListNodeClass[] heads = new LeetCode.ListNodeClass[1];
            //for (int k = 0; k < 1; k++)
            //{
            //    heads[k] = new LeetCode.ListNodeClass(0);
            //    LeetCode.ListNodeClass pt = heads[k];
            //    for (int i = 0; i < (k + 1) * 2333; i++)
            //    {
            //        pt.next = new LeetCode.ListNodeClass(i + 1); pt = pt.next;
            //    }
            //}
            //LeetCode.ListNodeClass head = new LeetCode.ListNodeClass(2);
            //LeetCode.ListNodeClass p = head;
            //p.next = new LeetCode.ListNodeClass(5); p = p.next;
            //p.next = new LeetCode.ListNodeClass(3); p = p.next;
            //p.next = new LeetCode.ListNodeClass(4); p = p.next;
            //p.next = new LeetCode.ListNodeClass(6); p = p.next;
            //p.next = new LeetCode.ListNodeClass(2); p = p.next;
            //p.next = new LeetCode.ListNodeClass(2); p = p.next;

            stopwatch.Start();

            //LeetCode.SolveSodoku vs = new LeetCode.SolveSodoku();
            //char[][] chars = new char[][] {new char[]{'5', '3', '.', '.', '7', '.', '.', '.', '.' },
            //                          new char[]{'6', '.', '.', '1', '9', '5', '.', '.', '.'},
            //                          new char[]{'.', '9', '8', '.', '.', '.', '.', '6', '.'},
            //                          new char[]{'8', '.', '.', '.', '6', '.', '.', '.', '3'},
            //                          new char[]{'4', '.', '.', '8', '.', '3', '.', '.', '1'},
            //                          new char[]{'7', '.', '.', '.', '2', '.', '.', '.', '6'},
            //                          new char[]{'.', '6', '.', '.', '.', '.', '2', '8', '.'},
            //                          new char[]{'.', '.', '.', '4', '1', '9', '.', '.', '5'},
            //                          new char[]{'.', '.', '.', '.', '8', '.', '.', '7', '9'}};
            //vs.Solution(chars);

            LeetCode.DeepCopyListWithRandomPointer d = new LeetCode.DeepCopyListWithRandomPointer();
            LeetCode.DeepCopyListWithRandomPointer.Node res = d.CopyRandomList(d.TestHead);

            stopwatch.Stop();
            Console.WriteLine(res);
            Console.WriteLine("Total run...." + stopwatch.ElapsedMilliseconds + "ms");
            Console.WriteLine("Press any key to get out of here...");
            
            Console.ReadKey();
        }
        struct person
        {
            public int age;
            public string name;
            public person(int g, string n)
            {
                age = g;
                name = n;
            }
        }
        
    }
}
