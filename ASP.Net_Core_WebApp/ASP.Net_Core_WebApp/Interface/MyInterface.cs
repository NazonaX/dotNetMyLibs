using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_WebApp.Interface
{
    interface MyInterface
    {
        string Method1();
        void DoSomething();
        string Method2(string str1, int number);
        int GetInumer(string need, int? start, int? end);
    }
}
