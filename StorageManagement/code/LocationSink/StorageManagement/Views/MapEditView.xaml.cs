using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Core.Input;

namespace wpfSimulation.Views
{
    /// <summary>
    /// Interaction logic for MapEditView.xaml
    /// </summary>
    public partial class MapEditView : Window
    {
        KeyModifierCollection keyModifiers = new KeyModifierCollection();
        public MapEditView()
        {
            InitializeComponent();
            //解决滚动条父级和子级的嵌套问题
            zoom.PreviewMouseWheel += (sender, e) =>
            {
                var eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                this.zoom.RaiseEvent(eventArg);
            };
        } 
        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
           //this.ScrollViewer1 = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(ScrollViewer1, 0), 0) as ScrollViewer;
           // this.ScrollViewer2 = VisualTreeHelper.GetChild(VisualTreeHelper.GetChild(ScrollViewer2, 0), 0) as ScrollViewer;
           // this.ScrollViewer1.ScrollChanged += new ScrollChangedEventHandler(sv1_ScrollChanged);
           // this.ScrollViewer2.ScrollChanged += new ScrollChangedEventHandler(sv2_ScrollChanged);

        }
        private void sv2_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = e.OriginalSource as ScrollViewer;

            if (sv != null)
            {
                ScrollViewer1.ScrollToHorizontalOffset(sv.HorizontalOffset);
                ScrollViewer3.ScrollToVerticalOffset(sv.VerticalOffset);
            }
        }

        private void sv1_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = e.OriginalSource as ScrollViewer;

            if (sv != null)
            {
                ScrollViewer2.ScrollToHorizontalOffset(sv.HorizontalOffset);
            }
        }

        private void sv3_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            ScrollViewer sv = e.OriginalSource as ScrollViewer;

            if (sv != null)
            {
                ScrollViewer2.ScrollToVerticalOffset(sv.VerticalOffset);
            }
        }
        /// <summary>
        /// 鼠标滚动放大缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Zoom_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            
            if (e.Delta > 0)
            {
                if (zoom.Scale > 2)
                {
                    return;
                }
                this.sli.Value = this.sli.Value + 20;
            }
            else
            {
                if (zoom.Scale < 1)
                {
                    return;
                }
                this.sli.Value = this.sli.Value - 20;
            }
        }
        /// <summary>
        /// 滑动条拉动放大缩小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Sli_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            zoom.Scale = 1 + e.NewValue / 100;
        }
        /// <summary>
        /// 单击鼠标可拖拽，同时鼠标光标设置为手掌状
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            zoom.DragModifiers.Clear();
            keyModifiers.Add(KeyModifier.None);
            zoom.DragOnPreview = true;
            zoom.DragModifiers = keyModifiers;
            zoom.Cursor = Cursors.Hand;
        }
        /// <summary>
        /// 单击鼠标不可拖拽，同时鼠标光标设置为箭头状
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            zoom.DragModifiers.Clear();
            keyModifiers.Add(KeyModifier.Ctrl);
            zoom.DragModifiers = keyModifiers;
            zoom.DragOnPreview = false;
            zoom.Cursor = Cursors.Arrow;
        }
    }
}
