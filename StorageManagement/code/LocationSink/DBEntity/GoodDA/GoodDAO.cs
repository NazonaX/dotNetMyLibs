using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.GoodDA
{
    public class GoodDAO : IGoodDA
    {
        public void DeleteGoods(List<Goods> Goods)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkDelete(Goods);
                context.BulkSaveChanges();
            }
        }

        public List<Goods> GetGoods()
        {
            using(DevEntities context = new DevEntities())
            {
                return context.Goods.ToList();
            }
        }

        public void InsertGoods(List<Goods> Goods)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkInsert(Goods);
                context.BulkSaveChanges();
            }
        }

        public void UpdateGoods(List<Goods> Goods)
        {
            using(DevEntities context = new DevEntities())
            {
                context.BulkUpdate(Goods);
                context.BulkSaveChanges();
            }
        }
    }
}
