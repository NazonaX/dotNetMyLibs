using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnyTest
{
    class ThreadingTimeCostTest
    {
        static void Main(string[] args)
        {
            Thread.Sleep(1000);
            Stopwatch swTask = new Stopwatch();
            swTask.Start();
            /*执行并行操作*/
            Parallel.Invoke(SetProcuct1_500, SetProcuct2_500, SetProcuct3_500, SetProcuct4_500);
            swTask.Stop();
            Console.WriteLine("500*4条数据 并行编程所耗时间:" + swTask.ElapsedMilliseconds);

            Thread.Sleep(1000);/*防止并行操作 与 顺序操作冲突*/
            Stopwatch sw = new Stopwatch();
            sw.Start();
            SetProcuct1_500();
            SetProcuct2_500();
            SetProcuct3_500();
            SetProcuct4_500();
            sw.Stop();
            Console.WriteLine("500*4条数据  顺序编程所耗时间:" + sw.ElapsedMilliseconds);

            Thread.Sleep(1000);
            swTask.Restart();
            /*执行并行操作*/
            Parallel.Invoke(() => SetProcuct1_10000(), () => SetProcuct2_10000(), () => SetProcuct3_10000(), () => SetProcuct4_10000());
            swTask.Stop();
            Console.WriteLine("10000*4条数据 并行编程所耗时间:" + swTask.ElapsedMilliseconds);

            Thread.Sleep(1000);
            sw.Restart();
            SetProcuct1_10000();
            SetProcuct2_10000();
            SetProcuct3_10000();
            SetProcuct4_10000();
            sw.Stop();
            Console.WriteLine("10000*4条数据 顺序编程所耗时间:" + sw.ElapsedMilliseconds);


            Console.ReadLine();
        }

        private static void SetProcuct1_500()
        {
            Stopwatch swTask = new Stopwatch();
            swTask.Start();
            List<Product> ProductList = new List<Product>();
            for (int index = 1; index < 500; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            swTask.Stop();
            Console.WriteLine("SetProcuct1 执行完成..."+ swTask.ElapsedMilliseconds);
        }
        private static void SetProcuct2_500()
        {
            Stopwatch swTask = new Stopwatch();
            swTask.Start();
            List<Product> ProductList = new List<Product>();
            for (int index = 500; index < 1000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            Console.WriteLine("SetProcuct2 执行完成..." + swTask.ElapsedMilliseconds);
        }
        private static void SetProcuct3_500()
        {
            Stopwatch swTask = new Stopwatch();
            swTask.Start();
            List<Product> ProductList = new List<Product>();
            for (int index = 1000; index < 2000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            swTask.Stop();
            Console.WriteLine("SetProcuct3 执行完成..." + swTask.ElapsedMilliseconds);
        }
        private static void SetProcuct4_500()
        {
            Stopwatch swTask = new Stopwatch();
            swTask.Start();
            List<Product> ProductList = new List<Product>();
            for (int index = 2000; index < 3000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            swTask.Stop();
            Console.WriteLine("SetProcuct4 执行完成..." + swTask.ElapsedMilliseconds);
        }
        private static void SetProcuct1_10000()
        {
            Stopwatch swTask = new Stopwatch();
            swTask.Start();
            List<Product> ProductList = new List<Product>();
            for (int index = 1; index < 10000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            swTask.Stop();
            Console.WriteLine("SetProcuct1 执行完成..." + swTask.ElapsedMilliseconds);
        }
        private static void SetProcuct2_10000()
        {
            Stopwatch swTask = new Stopwatch();
            swTask.Start();
            List<Product> ProductList = new List<Product>();
            for (int index = 10000; index < 20000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            swTask.Stop();
            Console.WriteLine("SetProcuct2 执行完成..." + swTask.ElapsedMilliseconds);
        }
        private static void SetProcuct3_10000()
        {
            Stopwatch swTask = new Stopwatch();
            swTask.Start();
            List<Product> ProductList = new List<Product>();
            for (int index = 20000; index < 30000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            swTask.Stop();
            Console.WriteLine("SetProcuct3 执行完成..." + swTask.ElapsedMilliseconds);
        }
        private static void SetProcuct4_10000()
        {
            Stopwatch swTask = new Stopwatch();
            swTask.Start();
            List<Product> ProductList = new List<Product>();
            for (int index = 30000; index < 40000; index++)
            {
                Product model = new Product();
                model.Category = "Category" + index;
                model.Name = "Name" + index;
                model.SellPrice = index;
                ProductList.Add(model);
            }
            swTask.Stop();
            Console.WriteLine("SetProcuct4 执行完成..." + swTask.ElapsedMilliseconds);
        }
    }

    class Product
    {
        public string Name { get; set; }

        public string Category { get; set; }

        public int SellPrice { get; set; }
    }

}
