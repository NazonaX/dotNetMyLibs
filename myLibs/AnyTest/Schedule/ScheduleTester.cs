using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.Schedule
{
    public class ScheduleTester
    {
        public static void Main(string[] args)
        {
            Debugger.SetDebugMode(false);
            Debugger.SetWriteConsole(true);
            Scheduler schedulerTest = Scheduler.DefaultTestScheduler1();
            //schedulerTest.OutPutOriginalSequence();
            OptimizedResult result = null;
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            //result = schedulerTest.OptimizeRecursion();
            //Console.WriteLine("Finally time cost: " + result.TimeCost);
            //Console.WriteLine("Task sequence is : " + result.ToString());
            ////schedulerTest.OutputSequence(result.Tasks);
            //sw.Stop();
            //Console.WriteLine("Calculation taked " + sw.ElapsedMilliseconds);
            //Console.WriteLine(".................................");
            //sw.Restart();
            double[] xs = new double[100];
            for(int i = 0; i < 100; i++)
                xs[i] = schedulerTest.Optimize().TimeCost;
            double avg = xs.Average();
            double s = Math.Sqrt(xs.Sum(d => Math.Pow(d - avg, 2)) / 100);
            Console.WriteLine(avg + "________" + s);
            result = schedulerTest.Optimize();
            Console.WriteLine("Finally time cost: " + result.TimeCost);
            Console.WriteLine("Task sequence is : " + result.ToString());
            //schedulerTest.OutputSequence(result.Tasks);
            sw.Stop();
            Console.WriteLine("Calculation taked " + sw.ElapsedMilliseconds);
            Console.WriteLine("Testing Schedule Algorithm...");
            Console.ReadKey();
        }
    }
}
