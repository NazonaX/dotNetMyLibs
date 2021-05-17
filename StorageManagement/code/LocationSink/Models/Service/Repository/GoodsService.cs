using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Service.Repository
{
    internal class GoodsService: IGoodsService
    {
        private Entity.Map _map = null;
        private List<DAL.Goods> _DAL_GoodList = null;


        public GoodsService(Entity.Map map)
        {
            this._map = map;
            this._DAL_GoodList = new List<DAL.Goods>();
        }

        /// <summary>
        /// load goods from database, using global one reference of entity.map
        /// </summary>
        /// <returns></returns>
        public List<Entity.Goods> LoadGoods()
        {
            DAL.GoodDA.IGoodDA goodDA = new DAL.GoodDA.GoodDAO();
            List<DAL.Goods> glist = goodDA.GetGoods();
            List<Entity.Goods> resList = new List<Entity.Goods>();
            foreach (DAL.Goods g in glist)
            {
                g.Batch = g.Batch.Trim();
                g.Model = g.Model.Trim();
                g.Name = g.Name.Trim();
                g.ProductId = g.ProductId.Trim();
                g.BarCode = g.BarCode.Trim();
                Entity.Goods tmp = new Entity.Goods();
                tmp.DAL_SetGood(g);
                resList.Add(tmp);
            }
            _map.Goods = resList;
            _DAL_GoodList = glist;
            return resList;
        }
        /// <summary>
        /// delete the goodlist inputed as O(n^2)
        /// </summary>
        /// <param name="GoodList"></param>
        public void DeleteGoods(List<Entity.Goods> GoodList)
        {
            List<DAL.Goods> todel = new List<DAL.Goods>();
            foreach (Entity.Goods g in GoodList)
                todel.Add(g.DAL_GetGood());
            //delete databse
            DAL.GoodDA.IGoodDA goodDA = new DAL.GoodDA.GoodDAO();
            goodDA.DeleteGoods(todel);
            //delete memory
            foreach(Entity.Goods g in GoodList)
            {
                _map.Goods.Remove(g);
                _DAL_GoodList.Remove(g.DAL_GetGood());
            }
        }
        public void InsertGoods(List<Entity.Goods> GoodList)
        {
            List<DAL.Goods> addition = new List<DAL.Goods>();
            foreach (Entity.Goods g in GoodList)
                addition.Add(g.DAL_GetGood());
            //insert databse
            DAL.GoodDA.IGoodDA goodDA = new DAL.GoodDA.GoodDAO();
            goodDA.InsertGoods(addition);
            //insert memory
            _map.Goods.AddRange(GoodList);
            _DAL_GoodList.AddRange(addition);
        }
        public void UpdateGoods()
        {
            DAL.GoodDA.IGoodDA goodDA = new DAL.GoodDA.GoodDAO();
            goodDA.UpdateGoods(_DAL_GoodList);
        }
        /// <summary>
        /// clear all goods in the database and in the memory as O(n)
        /// </summary>
        public void DeleteAllGoods()
        {
            DAL.GoodDA.IGoodDA goodDA = new DAL.GoodDA.GoodDAO();
            goodDA.DeleteGoods(_DAL_GoodList);
            _DAL_GoodList.Clear();
            _map.Goods = new List<Entity.Goods>();
        }
    }
}
