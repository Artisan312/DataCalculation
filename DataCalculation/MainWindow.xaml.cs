using MessagePack;
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
        public MainWindow()
        {
            InitializeComponent();
            mqtt = new HOSMQTT();
            mqtt.ChangeName += new ChangedHandler(m_ChangeName);
        }
        private HOSMQTT mqtt=null;
        private void m_ChangeName(object sender, object v)
        {
            //Dispatcher.Invoke(new Action(() =>
            //{
            byte[] b =(byte[]) v;
       
            if (b[0] == 134)
            {
                string str = byteToHexStr(b);
                IbeaconFrame mc2 = MessagePackSerializer.Deserialize<IbeaconFrame>(b);
                foreach (byte[] d in mc2.devices)
                {
                    str = byteToHexStr(d);
                    Label(str);
                }
            }
            //}));
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
        private bool Label(String s)
        {
            try
            {
                s = s.ToUpperInvariant();
                String ladel_mac = null, uuid = null;
                int rssi;
                String str;
                str = s.Substring(16, 18);
                long qw = Convert.ToInt64(str, 16);
                uuid = s.Substring(30 + 2 * (int)qw, 62 + 2 * (int)qw);//uuid
                ladel_mac = s.Substring(2, 14);//mac
                ladel_mac = insert(ladel_mac);
                if (uuid.Equals("FDA50693A4E24FB1AFCFC6EB07647825")&&ladel_mac== "E3:52:1E:25:99:F1")
                {
                    str = s.Substring(14, 16);//
                    rssi = Convert.ToInt32(str, 16) - 256;
                    rssi = Math.Abs(rssi);
                    
                }

            }
            catch (Exception e)
            {
                e.GetBaseException();
            }
            return false;
        }

        private static String insert(String str)
        {
            // TODO Auto-generated method stub
            String s = "";
            for (int i = 0; i < str.Length; i += 2)
            {
                if (i != 0)
                    s += ":";
                s += str.Substring(i, i + 2);
            }
            return s;
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            if(button.Content.ToString()=="获取")
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
