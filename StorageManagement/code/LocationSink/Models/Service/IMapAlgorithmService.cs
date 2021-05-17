using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service
{
    internal interface IMapAlgorithmService
    {
        /// <summary>
        /// 计算封装CargoWays，对CargoWays.AvgTimeCost赋值。
        /// 区别于CargoWays.Left/Right-In/Out-Points, 这个封装的AvgTimeCost计算更为细节的单货道平均时间花费，每一个栅格都会被计算。
        /// 扫描每一个合法栅格，Rail-Storage-StorageRail。
        /// </summary>
        /// <param name="_map">全局唯一的地图引用</param>
        void WrapCargoWaysWithDetailAvgCost(Models.Entity.Map _map);
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
        double CalculateTime(double threshold, double distance, double max_speed, double acc, double dec);
        /// <summary>
        /// 通过传入的最大速度、恒定加速度和恒定减速度计算一个距离阈值，该阈值用于控制时间花费计算的公式选择。
        /// </summary>
        /// <param name="max_speed">最大速度</param>
        /// <param name="acc">恒定加速度</param>
        /// <param name="dec">恒定减速度</param>
        /// <returns></returns>
        double CalculateThreshold(double max_speed, double acc, double dec);
    }
}
