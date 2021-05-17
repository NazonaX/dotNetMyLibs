using Models.Entity;
using Models.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace wpfSimulation.Controllers
{
    /// <summary>
    /// 外围调用入库分配策略接口
    /// </summary>
   public class InputPolicyController:ApiController
    {
        /// <summary>
        /// 用于WebApi调用
        [HttpPost]
        public List<LockLocations> PostLocations([FromBody]InputInfo inputInfo)
        {
            return MapSingletonService.Instance.GetMapLogicsService().GetLockLocations(inputInfo.order, inputInfo.inPoint, inputInfo.destArea);
        }
        /// <summary>
        /// 输入信息封装类
        /// </summary>
        public class InputInfo
        {
         public   LogicsOrder order { get; set; }//传入订单
         public   MapItems inPoint { get; set; }//传入入库点
         public   List<int> destArea { get; set; }//传入目标区域
        }
    }
}
