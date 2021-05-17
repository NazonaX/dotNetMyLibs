using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Entity
{
    public class Zone
    {
        #region private property
        DAL.Zone _zone = new DAL.Zone();

        // for random color strategy
        private static int R = 0;
        private static int G = 0;
        private static int B = 0;

        #endregion
        public static string NextRandomColor()
        {
            R += 64;
            if (R == 256)
            {
                R = 0;
                G += 64;
            }
            if (G == 256)
            {
                G = 0;
                B += 64;
            }
            if (B == 128 && G == 128 && R == 128)
            {
                //for a color loop
                R = 0;
                G = 0;
                B = 0;
            }
            int R_Reverse = 255 - R;
            int G_Reverse = 255 - G;
            int B_Reverse = 255 - B;
            string empty = ToHexColor(R_Reverse, G_Reverse, B_Reverse);
            return empty;
        }
        public static void ResetRrandomColor()
        {
            R = 0;
            G = 0;
            B = 0;
        }
        private static string ToHexColor(int cR, int cG, int cB)
        {
            string R = Convert.ToString(cR, 16);
            if (R == "0")
                R = "00";
            string G = Convert.ToString(cG, 16);
            if (G == "0")
                G = "00";
            string B = Convert.ToString(cB, 16);
            if (B == "0")
                B = "00";
            string HexColor = "#" + R + G + B;
            return HexColor;
        }
        #region public property
        public string Name
        {
            get { return _zone.Name; }
            set { _zone.Name = value; }
        }
        public int Id
        {
            get { return _zone.Id; }
            private set { _zone.Id = value; }
        }
        public string Color
        {
            get { return _zone.Color; }
            set { _zone.Color = value; }
        }
        #endregion

        public void DAL_SetZone(DAL.Zone zone)
        {
            this._zone = zone;
        }
        public DAL.Zone DAL_GetZone()
        {
            return _zone;
        }
    }
}
