using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Models.Classes
{
    [Serializable]
    public class Position
    {
        public int _layer;
        public int _rack;
        public int _column;

        public int Layer { get { return _layer; } set { _layer = value; } }
        public int Rack { get { return _rack; } set { _rack = value; } }
        public int Column { get { return _column; } set { _column = value; } }

        public Position(int layer, int rack, int column)
        {
            this.Layer = layer;
            this.Rack = rack;
            this.Column = column;
        }
        public Position()
        {
            Layer = -1;
            Rack = -1 ;
            Column = -1;
        }

        public Position Copy()
        {
            return IOOps.CopyMemory(this) as Position;
        }
        public bool Equals(Position p)
        {
            return p.Layer == Layer && p.Rack == Rack && p.Column == Column;
        }
    }
}
