using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels
{
    public class SingleGridMapItemViewModels: BaseViewModels
    {
        #region for view
        private Models.Classes.MapItem item = null;
        private bool isSelected = false;
        public Models.Classes.MapItem SingleStorage { get { return item; } set { item = value; } }
       
        public SingleGridMapItemViewModels(int w, int h, int tp, int lp, Models.Classes.MapItem.ItemTypes itemType)
        {
            item.Width = w;
            item.Height = h;
            item.TopPad = tp;
            item.LeftPad = lp;
            item.Type = itemType;
        }
        public SingleGridMapItemViewModels() {
            
        }
        
        public int Width
        {
            get { return item.Width; }
            set { item.Width = value; OnPropertyChanged("Width"); }
        }
        public int Height
        {
            get { return item.Height; }
            set { item.Height = value; OnPropertyChanged("Height"); }

        }
        public int TopPad
        {
            get { return item.TopPad; }
            set { item.TopPad = value; OnPropertyChanged("TopPad"); }
        }
        public int LeftPad
        {
            get { return item.LeftPad; }
            set { item.LeftPad = value; OnPropertyChanged("LeftPad"); }
        }
        public string Color
        {
            get { return item.Color; }
        }
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                isSelected = value;
                OnPropertyChanged("IsSelected");
            }
        }
        #endregion

        public Models.Classes.MapItem.ItemTypes Type
        {
            get { return item.Type; }
            set
            {
                item.Type = value;
                OnPropertyChanged("Color");
            }
        }


    }
}
