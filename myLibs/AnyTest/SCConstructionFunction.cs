
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnyTest
{
    public class SCConstructionFunction
    {
        protected String str1;
        protected Int32 int1;

        public string Str1 { get => str1; set => str1 = value; }
        public Int32 Int1 { get => int1; set => int1 = value; }

        public SCConstructionFunction()
        {
            str1 = null;
            int1 = -1;
            Console.WriteLine("ConsF 1");
        }
        public SCConstructionFunction(String str1):this()
        {
            this.Str1 = str1;
            Console.WriteLine("ConsF 2");
        }
        public SCConstructionFunction(String str1, Int32 int1) : this(str1)
        {
            this.Int1 = int1;
            Console.WriteLine("ConsF 3");
        }
        public void SealedTest(string teststr)
        {

        }
    }
}