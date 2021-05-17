using ASP.Net_Core_WebApp.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASP.Net_Core_WebApp.Classes
{
    public class InterfaceTest : MyInterface
    {
        void MyInterface.DoSomething()
        {
            throw new NotImplementedException();
        }

        public string Method1()
        {
            throw new NotImplementedException();
        }

        string MyInterface.Method2(string str1, int number)
        {
            throw new NotImplementedException();
        }

        string MyInterface.Method1()
        {
            throw new NotImplementedException();
        }

        public int GetInumer(string need, int? start = 0, int? end = null)
        {
            
            throw new NotImplementedException();
        }
        public (int index, string content) GetContent()
        {
            return (0, "this is a test");
        }
    }
}
