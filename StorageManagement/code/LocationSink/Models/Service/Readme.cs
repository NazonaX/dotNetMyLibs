using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service
{
    class Readme
    {
        //The Service is used for Repository Service and Business Logics Service
        //The Logics Service should be designed as Singleton
        //Models层中Service.Repository中所有接口都规范定义了各个Entity数据实体的CRUD，参数和返回都是List形式
        //-->定义的Insert操作为插入列表中的所有元素
        //-->定义的Delete操作为删除列表中的元素
        //-->定义的Update操作为更新所有引用元素，因此没有形式参数
        //-->定义的Load操作为获取数据库中的所有元祖
        //-->实际对应的实现类中，包含了所有对应DAL实体的列表和全局唯一Map的引用，Map中含有所有Entity实体的列表引用。关于Entity和DAL实体的关系可见Entity中的Readme.cs

        /// 另外三项对外（目前Algorithm除外，给LogicsService提供公式计算服务）接口和实现都写有详细的说明
        /// 整体结构图可以参照图标文档中的“基础结构.svg”
    }
}
