using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Classes
{
    [Serializable]
    public class Goods
    {
        public static List<string> GoodsTypes = new List<string>()
        {
            "GoodsTypes_test1",
            "GoodsTypes_test2"
        };
        public static string Empty = "NaN";

        public string GoodsName { get; set; }
        public string GoodsCode { get; set; }
        public string OrderCode { get; set; }
        public string Specification { get; set; }
        public int GoodsCount { get; set; }
        public List<string> Types { get; set; }
        public Position _nowPosition = new Position();

        public Position NowPosition
        {
            get { return _nowPosition; }
            set { _nowPosition = value.Copy(); }
        }

        public Goods()
        {
            Clear();
        }
        public Goods(string goodsName, string goodsCode,string specification, string orderCode, int goodsCount,
            List<string> types)
        {
            Initial(goodsName, goodsCode, specification, orderCode, goodsCount, types);
        }

        private void Initial(string goodsName, string goodsCode,string specification, string orderCode, int goodsCount,
            List<string> types)
        {
            this.GoodsName = goodsName;
            this.GoodsCode = goodsCode;
            this.OrderCode = orderCode;
            this.Specification = specification;
            this.GoodsCount = goodsCount;
            Types = new List<string>();
            for (int i = 0; i < types.Count; i++)
                Types.Add(types[i]);
        }
        public void AddType(string type)
        {
            if (GoodsTypes.Contains(type) && !Types.Contains(type))
                Types.Add(type);
        }
        public void Clear()
        {
            GoodsName = Empty;
            GoodsCode = Empty;
            Specification = Empty;
            GoodsCount = 0;
            OrderCode = Empty;
            Types = new List<string>();
        }
        public bool IsEmpty()
        {
            if (IsStringNullOrEmpty(GoodsName) && Types.Count == 0 && IsStringNullOrEmpty(GoodsCode)
                && IsStringNullOrEmpty(OrderCode) && GoodsCount == 0 && IsStringNullOrEmpty(Specification))
                return true;
            else
                return false;
        }
        public static bool IsStringNullOrEmpty(string str)
        {
            return str == null || str.Equals(Empty);
        }
        public Goods Copy()
        {
            return Utils.IOOps.CopyMemory(this) as Goods;
        }
        /// <summary>
        /// two static functions for GoodsTypes List
        /// </summary>
        /// <param name="str"></param>
        public static void AddGoodsTypes(string str)
        {
            if (!GoodsTypes.Contains(str))
                GoodsTypes.Add(str);
        }
        private static void AddGoodsTypes(string str, int index)
        {
            if (!GoodsTypes.Contains(str))
                GoodsTypes.Insert(index, str);
        }
        public static void DeleteGoodsTypes(string str)
        {
            if (GoodsTypes.Contains(str))
                GoodsTypes.Remove(str);
        }
        public static void ModifyGoodsTypes(string original, string target)
        {
            if (GoodsTypes.Contains(original))
            {
                int index = GoodsTypes.IndexOf(original);
                DeleteGoodsTypes(original);
                AddGoodsTypes(target, index);
            }
        }
        /// <summary>
        /// to decide if the target is partial equals to self
        /// only check the properties which are not equals Empty
        /// except for count
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public bool PartialEquals(Goods target)
        {
            bool res = true;
            if (!IsStringNullOrEmpty(this.GoodsName) && !IsStringNullOrEmpty(target.GoodsName))
                res = res && this.GoodsName.Equals(target.GoodsName);
            if (!IsStringNullOrEmpty(this.GoodsCode) && !IsStringNullOrEmpty(target.GoodsCode))
                res = res && this.GoodsCode.Equals(target.GoodsCode);
            if (!IsStringNullOrEmpty(this.Specification) && !IsStringNullOrEmpty(target.Specification))
                res = res && this.Specification.Equals(target.Specification);
            if (!IsStringNullOrEmpty(this.OrderCode) && !IsStringNullOrEmpty(target.OrderCode))
                res = res && this.OrderCode.Equals(target.OrderCode);
            if (!(target.Types.Count == 1 && target.Types[0].Equals(Empty)) 
                && this.Types.Count > 0 && target.Types.Count > 0)
                res = res && TypeEquals(target);
            return res;
        }

        public bool TypeEquals(Goods target)
        {
            if (target.Types.Count != this.Types.Count)
                return false;
            bool res = true;
            for (int i = 0; i < this.Types.Count; i++)
            {
                res = res && target.Types.Contains(Types[i]);
                res = res && this.Types.Contains(target.Types[i]);
            }
            return res;
        }
    }
}
