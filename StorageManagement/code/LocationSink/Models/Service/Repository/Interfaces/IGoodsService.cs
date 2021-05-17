using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service.Repository
{
    public interface IGoodsService
    {
        List<Entity.Goods> LoadGoods();
        void DeleteGoods(List<Entity.Goods> GoodList);
        void InsertGoods(List<Entity.Goods> GoodList);
        void UpdateGoods();
        void DeleteAllGoods();
    }
}
