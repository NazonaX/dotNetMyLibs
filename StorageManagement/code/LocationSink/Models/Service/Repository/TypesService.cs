using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL;
using Models.Entity;

namespace Models.Service.Repository
{
    internal class TypesService : ITypesService
    {
        private Entity.Map _map = null;
        private List<DAL.Types> _DAL_Types = null;

        public TypesService(Entity.Map map)
        {
            this._map = map;
            this._DAL_Types = new List<DAL.Types>();
        }

        /// <summary>
        /// load types into global one map
        /// </summary>
        /// <returns></returns>
        public List<Entity.Types> LoadTypes()
        {
            DAL.TypesDA.ITypesDA typesDA = new DAL.TypesDA.TypesDAO();
            List<DAL.Types> tl = typesDA.GetTypes();
            List<Entity.Types> res = new List<Entity.Types>();
            foreach (DAL.Types t in tl)
            {
                t.Name = t.Name.Trim();
                t.Color = t.Color.Trim();
                Entity.Types tmp = new Entity.Types();
                tmp.DAL_SetTypes(t);
                res.Add(tmp);
            }
            _DAL_Types = tl;
            _map.Types = res;
            return res;
        }
        public void DeleteTypes(List<Entity.Types> detList)
        {
            List<DAL.Types> todel = new List<DAL.Types>();
            foreach(Entity.Types t in detList)
            {
                todel.Add(t.DAL_GetTypes());
            }
            //delete database
            DAL.TypesDA.ITypesDA typesDA = new DAL.TypesDA.TypesDAO();
            typesDA.DeleteTypes(todel);
            //delete memory
            foreach(Entity.Types t in detList)
            {
                _map.Types.Remove(t);
                _DAL_Types.Remove(t.DAL_GetTypes());
            }
        }
        public void InsertTypes(List<Entity.Types> addList)
        {
            List<DAL.Types> insert = new List<DAL.Types>();
            foreach(Entity.Types t in addList)
            {
                insert.Add(t.DAL_GetTypes());
            }
            //insert databse
            DAL.TypesDA.ITypesDA typesDA = new DAL.TypesDA.TypesDAO();
            typesDA.InsertTypes(insert);
            //insert memory
            _map.Types.AddRange(addList);
            _DAL_Types.AddRange(insert);
        }
        public void UpdateTypes()
        {
            DAL.TypesDA.ITypesDA typesDA = new DAL.TypesDA.TypesDAO();
            typesDA.UpdateTypes(_DAL_Types);
        }
    }
}
