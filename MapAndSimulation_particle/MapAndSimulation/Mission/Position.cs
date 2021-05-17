using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapAndSimulation.Mission
{
    public class Position
    {
        int layer = 0;
        int row = 0;
        int column = 0;

        public Position(int layer, int row, int column)
        {
            SetPosition(layer, row, column);
        }

        public void SetPosition(int layer, int row, int column)
        {
            this.layer = layer;
            this.row = row;
            this.column = column;
        }

        public int Row { get => row; set => row = value; }
        public int Column { get => column; set => column = value; }
        public int Layer { get => layer; set => layer = value; }

        public Position Copy()
        {
            return new Position(this.layer, this.row, this.column);
        }
        public bool Equals(Position p)
        {
            return p.layer == this.layer && p.row == this.row && p.column == this.column;
        }

        
        public override string ToString()
        {
            return "(" + layer + ", " + row + ", " + column + ")";
        }
    }
}
