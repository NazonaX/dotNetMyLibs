using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ASP.Net_Core_WebApp.Classes;
using ASP.Net_Core_WebApp.Interface;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ASP.Net_Core_WebApp.Controllers
{
    [Route("api/my")]
    public class MyController : Controller
    {
        // GET: /<controller>/
        [Route("string")]
        public string GetStringTest()
        {
            //显示实现接口和直接实现接口的区别，
            //在于显示实现接口强制用户使用接口引用对象
            InterfaceTest it = new InterfaceTest();
            MyInterface mit = it;
            int index = it.GetInumer("this", end: 200, start: 0);
            List<InterfaceTest> listTest = new List<InterfaceTest>();

            return "Tea pot...";
        }

        [Route("string2")]
        public string GetStringTest2()
        {
            return "Tea pot2...";
        }

        [Route("return")]
        public string getstring(string str1 = "NULL")
        {
            return "Get String::" + str1;
        }

        [Route("return2")]
        public string getstring2(string str1 = "NULL1", string str2 = "NULL2")
        {
            return "Get String " + str1 + "-->" + str2;
        }
        //[Route("ApiIndex")]
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
