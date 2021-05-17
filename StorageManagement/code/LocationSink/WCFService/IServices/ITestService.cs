using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace WCFService.IServices
{
    #region WCF服务操作接口
    /// <summary>
    /// ServiceContract表示服务契约（表示此接口暴露给客户端Client）
    /// OperationContract表示操作契约（表示ITestService接口中的Add方法暴露给客户端Client）
    /// 不添加契约则客户端看不到该方法与接口
    /// </summary>
    [ServiceContract]
    public interface ITestService
    {
        [OperationContract]
        int Add(int a, int b);
    }
    #endregion
}
