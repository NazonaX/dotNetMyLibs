using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WCFService.IServices;

namespace WCFService.Service
{
    public class TestService : ITestService
    {
        public int Add(int a, int b)
        {
            return a + b;
        }
    }
}
