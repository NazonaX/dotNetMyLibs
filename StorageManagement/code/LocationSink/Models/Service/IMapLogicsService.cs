using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service
{
    //outer class as order
    /// <summary>
    /// 作为外接的订单类，该类用于模拟入库。
    /// </summary>
    public class LogicsOrder
    {
        #region public properties
        /// <summary>
        /// 品托数列表，一个元素代表一品托，数值代表该品托中的货物数量。
        /// </summary>
        public List<int> Units;
        /// <summary>
        /// 是否支持混放
        /// </summary>
        public bool Mixable;
        /// <summary>
        /// 是否异层优先
        /// </summary>
        public bool DifferLayersFirst;
        /// <summary>
        /// 货物名称
        /// </summary>
        public string GoodName;
        /// <summary>
        /// 货物类别
        /// </summary>
        public string GoodModel;
        /// <summary>
        /// 货物批次
        /// </summary>
        public string GoodBatch;
        /// <summary>
        /// 货物产品编号
        /// </summary>
        public string ProductId;
        /// <summary>
        /// 货物条形码
        /// </summary>
        public string BarCode;
        public string Model_Batch
        {
            get { return GoodModel + "_" + GoodBatch; }
        }

        #endregion

        #region methods
        /// <summary>
        /// 增加一品托
        /// </summary>
        /// <param name="goodCountForUnit">该品托的货物数量</param>
        public void AddUnit(int goodCountForUnit)
        {
            if (Units == null)
                Units = new List<int>();
            Units.Add(goodCountForUnit);
        }
        #endregion

    }
    /// <summary>
    /// 对外锁定信息类。
    /// 注意如果确认锁定，在进行Confirm的时候，传回的该类实例一定是通过计算传出的，因为包含了internal属性。
    /// </summary>
    public class LockLocations
    {
        /// <summary>
        /// 所在行
        /// </summary>
        public int Rack { get; set; }
        /// <summary>
        /// 所在层
        /// </summary>
        public int Layer { get; set; }
        /// <summary>
        /// 锁定货道ID
        /// </summary>
        public int CargoWayId { get; set; }
        /// <summary>
        /// 锁定起始位置
        /// </summary>
        public int LockStart { get; set; }
        /// <summary>
        /// 锁定结束位置
        /// </summary>
        public int LockEnd { get; set; }
        /// <summary>
        /// 锁定使用的轨道列号
        /// </summary>
        public int RailColumn { get; set; }
        /// <summary>
        /// 锁定使用的入库点Id
        /// </summary>
        internal int InPointId { get; set; }
    }
    public interface IMapLogicsService
    {
        /// <summary>
        /// 扫描栅格地图并生成裸货道信息。
        /// 生成之后记得保存货道信息。
        /// </summary>
        List<Models.Entity.CargoWays> InitializeCargoPaths();
        List<Models.Entity.Rails> InitializeRails();
        /// <summary>
        /// 结合当前货物信息和锁定信息，初始化货道的其他附加基础信息。
        /// </summary>
        void InitializeIndexer();
        /// <summary>
        /// 计算并获取锁定信息。
        /// </summary>
        /// <param name="order">订单，必定为单种类货物</param>
        /// <param name="inPoint">入库点列表，空代表都可以</param>
        /// <param name="destArea">区域列表，空代表都可以</param>
        /// <returns></returns>
        List<LockLocations> GetLockLocations(LogicsOrder order, Entity.MapItems inPoint, List<int> destArea = null);
        /// <summary>
        /// 确认并保存货物锁定信息。
        /// </summary>
        /// <param name="additionList">通过GetLockLocations函数计算获得的LockLocations列表</param>
        /// <param name="order">通过GetLockLocations函数传入的LogicsOrder</param>
        void ConfirmAndSetCargoWaysLocks(List<LockLocations> additionList, LogicsOrder order);
        /// <summary>
        /// 使用更为细节的单货平均周专利装货道，封装之后的GetLockLocation计算将使用更为细致的货道平均周转率。
        /// </summary>
        void WrapCargoWaysWithAvgDetailedCost();

        
    }
}
