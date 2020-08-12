using MessagePack;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AllDataWereAnalyzed
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static PointList<Point> heardSoundList1 = new PointList<Point>();
        private static PointList<Point> heardSoundList2 = new PointList<Point>();
        private static PointList<Point> heardSoundList3 = new PointList<Point>();
        private static PointList<Point> heardSoundList4 = new PointList<Point>();
        private static PointList<Point> heardSoundList5 = new PointList<Point>();
        private static PointList<Point> heardSoundList6 = new PointList<Point>();
        private static Kalman kalman1 = new Kalman(9,100);
        private static Kalman kalman2 = new Kalman(9, 100);
        private static Kalman kalman3 = new Kalman(9, 100);
        private static string[] gateway = new string[] { "24:6F:28:2E:B0:6C", "FC:F5:C4:15:30:24", "24:62:AB:D0:B4:E8" };
        private static int[] bd = new int[3] { 0,0,0};
        private static HOSMQTT mqtt;
        private static string gateway_mac;
        public MainWindow()
        {
            InitializeComponent();

            mqtt = new HOSMQTT();
            mqtt.ChangeName += new ChangedHandler(shijian);



            cptEcg1.Children.RemoveAll(typeof(LineGraph));
            heardSoundList1.Count = 80;
            heardSoundList1.Collection.RemoveAll(typeof(Point));
            cptEcg1.AddLineGraph(heardSoundList1, Color.FromArgb(0xFF, 0x00, 0x0F, 0x00), 1, "网关1");
            heardSoundList2.Count = 80;
            heardSoundList2.Collection.RemoveAll(typeof(Point));
            cptEcg1.AddLineGraph(heardSoundList2, Color.FromArgb(0xFF, 0xFF, 0x00, 0xF0), 1, "网关2");
            heardSoundList3.Count = 80;
            heardSoundList3.Collection.RemoveAll(typeof(Point));
            cptEcg1.AddLineGraph(heardSoundList3, Color.FromArgb(0xFF, 0x00, 0xFF, 0x00), 1, "网关3");
            cptEcg1.AxisGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 255));
            cptEcg1.Viewport.FitToView();

            cptEcg2.Children.RemoveAll(typeof(LineGraph));
            heardSoundList4.Count = 80;
            heardSoundList4.Collection.RemoveAll(typeof(Point));
            cptEcg2.AddLineGraph(heardSoundList4, Color.FromArgb(0xFF, 0x00, 0x0F, 0x00), 1, "网关1");
            heardSoundList5.Count = 80;
            heardSoundList5.Collection.RemoveAll(typeof(Point));
            cptEcg2.AddLineGraph(heardSoundList5, Color.FromArgb(0xFF, 0xFF, 0x00, 0xF0), 1, "网关2");
            heardSoundList6.Count = 80;
            heardSoundList6.Collection.RemoveAll(typeof(Point));
            cptEcg2.AddLineGraph(heardSoundList6, Color.FromArgb(0xFF, 0x00, 0xFF, 0x00), 1, "网关3");
            cptEcg2.AxisGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 255));
            cptEcg2.Viewport.FitToView();
        }

        private void shijian(byte[] b)
        {
            try
            {
                if (b[0] == 134)
                {
                    string str = byteToHexStr(b);
                    IbeaconFrame mc2 = MessagePackSerializer.Deserialize<IbeaconFrame>(b);//47B87A8D782719F772D4021E2384
                    gateway_mac = insert(mc2.mac);
                    foreach (byte[] d in mc2.devices)
                    {
                        str = byteToHexStr(d);
                        Label(str);
                    }
                }
            }
            catch (Exception e)
            {

                e.GetBaseException();
            }
        }
        private bool Label(String s)
        {
            try
            {
                s = s.ToUpperInvariant();
                String ladel_mac = null, uuid = null;
                int rssi;
                String str;
                str = s.Substring(16, 2);
                int qw = Convert.ToInt16(str, 16);
                uuid = s.Substring(30 + 2 * qw, 32);//uuid
                ladel_mac = s.Substring(2, 12);//mac
                ladel_mac = insert(ladel_mac);
                if (ladel_mac == "E3:52:1E:25:99:F1")
                {
                    str = s.Substring(14, 2);//
                    rssi = Convert.ToInt32(str, 16) - 256;
                    rssi = Math.Abs(rssi);
                    RssiAdd(rssi);
                }

            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        private  void RssiAdd(int rssi)
        {
            int i;
            for ( i= 0; gateway[i] != gateway_mac; i++) ;
            Dispatcher.Invoke(new Action(() =>
            {
                switch (i)
                {
                    case 0:
                        {
                            heardSoundList1.Add(new Point(bd[i], rssi));
                            heardSoundList4.Add(new Point(bd[i], kalman1.KalmanFilter(rssi)));
                            bd[i]++;
                        }; break;
                    case 1:
                        {
                            heardSoundList2.Add(new Point(bd[i], rssi));
                            heardSoundList5.Add(new Point(bd[i], kalman1.KalmanFilter(rssi)));
                            bd[i]++;
                        }; break;
                    case 2:
                        {
                            heardSoundList3.Add(new Point(bd[i], rssi));
                            heardSoundList6.Add(new Point(bd[i], kalman1.KalmanFilter(rssi)));
                            bd[i]++;
                        }; break;

                }
            }));
        }

        private void open(object sender, RoutedEventArgs e)
        {
            mqtt.Start();
        }
        private string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                }
            }
            return returnStr;
        }
        private static String insert(String str)
        {
            // TODO Auto-generated method stub
            String s = "";
            for (int i = 0; i < str.Length; i += 2)
            {
                if (i != 0)
                    s += ":";
                s += str.Substring(i, 2);
            }
            return s;
        }
    }
}
