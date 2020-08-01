using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DataCalculation
{
    /// <summary>
    /// Window1.xaml 的交互逻辑
    /// </summary>
    public partial class Window1 : Window
    {
        private static PointList<Point> heardSoundList = new PointList<Point>();//定义函数
        
        public Window1()
        {
            InitializeComponent();

            cptEcg.Children.RemoveAll(typeof(LineGraph));
            heardSoundList.Count = 30;
            heardSoundList.Collection.RemoveAll(typeof(Point));
            cptEcg.AddLineGraph(heardSoundList, Color.FromArgb(0xFF, 0x00, 0x00, 0x00), 1, "位     置");
            cptEcg.AxisGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 255));
            cptEcg.Viewport.FitToView();

        }
        public void AppPoint(Point point, Point po)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                //   heardSoundList1.Add(point);
                heardSoundList.Add(po);
            }));
        }
    }
}
