using MessagePack;
using Microsoft.Research.DynamicDataDisplay;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
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

namespace DataCalculation
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private static HOSMQTT mqtt = null;
        private static string gateway_mac;
        private static PointList<Point> heardSoundList1 = new PointList<Point>();
        private static PointList<Point> heardSoundList2 = new PointList<Point>();
        private static PointList<Point> heardSoundList3 = new PointList<Point>();
        private static int[][] dictionary = new int[3][];
        private List<int>[] data = new List<int>[] { new List<int>(), new List<int>(), new List<int>() };
        private static string[] gateway = new string[] { "24:6F:28:2E:B0:6C", "FC:F5:C4:15:30:24", "24:62:AB:D0:B4:E8" };
        private static Window1 window = new Window1();
        private static int x = 0;
        public MainWindow()
        {
            InitializeComponent();

            window.Show();

            mqtt = new HOSMQTT();
            mqtt.ChangeName += new ChangedHandler(m_ChangeName);

            cptEcg1.Children.RemoveAll(typeof(LineGraph));
            heardSoundList1.Count = 30;
            heardSoundList1.Collection.RemoveAll(typeof(Point));
            cptEcg1.AddLineGraph(heardSoundList1, Color.FromArgb(0xFF, 0x00, 0x00, 0x00), 1, "网关1");
            cptEcg1.AxisGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 255));
            cptEcg1.Viewport.FitToView();
            cptEcg2.Children.RemoveAll(typeof(LineGraph));
            heardSoundList2.Count = 30;
            heardSoundList2.Collection.RemoveAll(typeof(Point));
            cptEcg2.AddLineGraph(heardSoundList2, Color.FromArgb(0xFF, 0xFF, 0x00, 0xFF), 1, "网关2");
            cptEcg2.AxisGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 255));
            cptEcg2.Viewport.FitToView();
            cptEcg3.Children.RemoveAll(typeof(LineGraph));
            heardSoundList3.Count = 30;
            heardSoundList3.Collection.RemoveAll(typeof(Point));
            cptEcg3.AddLineGraph(heardSoundList3, Color.FromArgb(0xFF, 0xF0, 0x0F, 0x0F), 1, "网关3");
            cptEcg3.AxisGrid.BorderBrush = new SolidColorBrush(Color.FromArgb(0, 0, 0, 255));
            cptEcg3.Viewport.FitToView();
        }

        private void m_ChangeName(object sender, object v)
        {
            //Dispatcher.Invoke(new Action(() =>
            //{
            try
            {
                byte[] b = (byte[])v;
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

            //}));
        }
        private void RssiAdd(int rssi)
        {
            //Dispatcher.Invoke(new Action(() =>
            //{
            int i;
            //     for (i = 0; gateway[i] != gateway_mac; i++);
            i = gateway.ToList().IndexOf(gateway_mac);
            data[i].Add(rssi);
            if (data[0].Count >= 80 && data[2].Count >= 80 && data[1].Count >= 80)
            {
                dataFun();
                Calculation.CalculateTheDistance(dictionary);
                Point po = Calculation.threePoints();
                Point pi = Calculation.calculaton();
                DynamicGraph(Calculation.rssi);

                //Write(System.Text.Encoding.Default.GetBytes("1:" + Calculation.rssi[0] + ";2:" + Calculation.rssi[1] + ";3:" + Calculation.rssi[2] + "\r\n"));
                window.AppPoint(po, pi);
                PersonD personD = new PersonD();
                
                data = new List<int>[] { new List<int>(), new List<int>(), new List<int>() };
               // Write(System.Text.Encoding.Default.GetBytes("\r" + po.X + "," + po.Y + "\n" + pi.X + "," + pi.Y + "\r\n\n\n"));
            }
            //}));
        }
        private void dataFun()
        {
            for (int j = 0; j < 3; j++)
            {
                int n = 0;
                int[] b = new int[data[j].Count];
                //string str = null;
                foreach (int m in data[j])
                {
                    b[n] = m;
                    n++;
                }
                dictionary[j] = b;
                //str = (j + 1).ToString() + ": ";
                //str += string.Join(",", b) + "\r\n\n";
                //byte[] d = System.Text.Encoding.Default.GetBytes(str);
                //Write(d);
            }
        }
        public static void Write(byte[] Byte)
        {
            FileStream F = new FileStream("D:\\1.txt", FileMode.Append, FileAccess.Write, FileShare.Write);
            F.Write(Byte, 0, Byte.Length);
            F.Close();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="r"></param>
        private void DynamicGraph(int[] r)
        {
            x++;
            Dispatcher.Invoke(new Action(() =>
            {
                heardSoundList1.Add(new Point(x, r[0]));
                heardSoundList2.Add(new Point(x, r[1]));
                heardSoundList3.Add(new Point(x, r[2]));
            }));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
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
        }/// <summary>
         /// 
         /// </summary>
         /// <param name="s"></param>
         /// <returns></returns>
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
                if (ladel_mac == Variable.getBluetoothLabel())
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            if (button.Content.ToString() == "获取")
            {
                mqtt.Start();
                button.Content = "停止";
            }
            else
            {
                mqtt.Stop();
                button.Content = "获取";
            }
        }
    }
}
