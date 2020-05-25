
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AnyTest
{
    public static class Extension
    {
        public static bool ExtentionTest(this String str)
        {
            Console.WriteLine("Extension TeST: " + str);

            return true;
        }
    }
}