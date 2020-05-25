using System;
using System.Collections.Generic;
using System.Text;

namespace AnyTest.Schedule
{
    public class Calculator
    {
        /// <summary>
        /// 对外提供时间花费的计算，主要提供LogicsService使用。
        /// 通过已知的最高速度、恒定加速度、恒定减速度和距离，算出时间花费。
        /// </summary>
        /// <param name="threshold">公式变换使用阈值，该阈值可以直接通过最高速度、恒定加速度和恒定减速度来计算，此处传入是为了在外围控制重复计算。</param>
        /// <param name="distance">行驶的距离</param>
        /// <param name="max_speed">最大速度</param>
        /// <param name="acc">恒定加速度</param>
        /// <param name="dec">恒定减速度</param>
        /// <returns>时间花费</returns>
        public static double CalculateTime(double threshold, double distance, double max_speed, double acc, double dec)
        {
            if (distance <= threshold)
                return Math.Sqrt(2 * distance * acc / ((acc + dec) * dec))
                + Math.Sqrt(2 * distance * dec / ((acc + dec) * acc));
            else
                return distance / max_speed + max_speed / (2 * acc) + max_speed / (2 * dec);
        }
        /// <summary>
        /// 通过传入的最大速度、恒定加速度和恒定减速度计算一个距离阈值，该阈值用于控制时间花费计算的公式选择。
        /// </summary>
        /// <param name="max_speed">最大速度</param>
        /// <param name="acc">恒定加速度</param>
        /// <param name="dec">恒定减速度</param>
        /// <returns></returns>
        public static double CalculateThreshold(double max_speed, double acc, double dec)
        {
            return 0.5 * max_speed * (max_speed / acc + max_speed / dec);
        }
    }
}
