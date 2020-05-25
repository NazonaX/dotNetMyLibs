using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MapAndSimulation.ControlGenerater
{
    public static class LabelGenerater
    {
        public static Label Generate(int width, int height, Point location, Color color,
            string name)
        {
            Label label = new Label();
            label.AutoSize = false;
            label.Width = width;
            label.Height = height;
            label.Location = location;
            label.BackColor = color;
            label.Name = name;
            return label;
        }

    }
}
