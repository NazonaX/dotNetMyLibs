using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAndSimulation.Map
{
    [Serializable]
    public class Good
    {
        private string goodName="";
        private string orderNumber="";
        private string specification="";

        public string OrderNumber { get => orderNumber; set => orderNumber = value; }
        public string Specification { get => specification; set => specification = value; }
        public string GoodName { get => goodName; set => goodName = value; }


        public Good Copy()
        {
            Good g = new Good();
            g.orderNumber = this.orderNumber;
            g.specification = this.specification;
            g.goodName = this.goodName;
            return g;
        }
        public bool Equals(Good g)
        {
            return g.goodName == this.goodName
                && g.orderNumber == this.orderNumber
                && g.specification == this.specification;
        }
    }
}
