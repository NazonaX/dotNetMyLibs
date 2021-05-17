using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.GoodDA
{
    public interface IGoodDA
    {
        List<Goods> GetGoods();
        void InsertGoods(List<Goods> Goods);
        void UpdateGoods(List<Goods> Goods);
        void DeleteGoods(List<Goods> Goods);
    }
}
