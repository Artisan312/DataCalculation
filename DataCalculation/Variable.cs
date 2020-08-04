using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataCalculation
{
    class Variable
    {
        /// <summary>
        /// MQTT地址
        /// </summary>
        /// <returns></returns>
        public static string getServerUrl()
        {
            return ConfigurationManager.ConnectionStrings["ServerUrl"].ToString();
        }
        /// <summary>
        /// MQTT端口
        /// </summary>
        /// <returns></returns>
        public static int getPort()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        }
        /// <summary>
        /// 用户名
        /// </summary>
        /// <returns></returns>
        public static string getUserId()
        {
            return ConfigurationManager.AppSettings["UserId"].ToString();
        }
        /// <summary>
        /// 密码
        /// </summary>
        /// <returns></returns>
        public static string getPassword()
        {
            return ConfigurationManager.AppSettings["Password"].ToString();
        }
        /// <summary>
        /// 主题
        /// </summary>
        /// <returns></returns>
        public static string getTopic()
        {
            return ConfigurationManager.AppSettings["Topic"].ToString();
        }
        /// <summary>
        /// 一米rssi
        /// </summary>
        /// <returns></returns>
        public static int getCalibration()
        {
            int n = Convert.ToInt32(ConfigurationManager.AppSettings["Calibration"]);
            return Convert.ToInt32(ConfigurationManager.AppSettings["Calibration"]);
        }
        /// <summary>
        /// 误差
        /// </summary>
        /// <returns></returns>
        public static int getError()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["Error"]);
        }
        /// <summary>
        /// 允许偏差
        /// </summary>
        /// <returns></returns>
        public static int getDeviation()
        {
            return Convert.ToInt32(ConfigurationManager.AppSettings["Deviation"]);
        }
        /// <summary>
        /// 环境因素
        /// </summary>
        /// <returns></returns>
        public static double getFactor()
        {
            return Convert.ToDouble(ConfigurationManager.AppSettings["Factor"]);
        }
        /// <summary>
        /// 方差
        /// </summary>
        /// <returns></returns>
        public static double getVariance()
        {
            return Convert.ToDouble(ConfigurationManager.AppSettings["Variance"]);
        }
        /// <summary>
        /// 预定方差
        /// </summary>
        /// <returns></returns>
        public static double getPredeterminedVariance()
        {
            return Convert.ToDouble(ConfigurationManager.AppSettings["PredeterminedVariance"]);
        }
        /// <summary>
        ///蓝牙标签
        /// </summary>
        /// <returns></returns>
        public static string getBluetoothLabel()
        {
            return ConfigurationManager.AppSettings["BluetoothLabel"].ToString();
        }
        /// <summary>
        /// 网关坐标
        /// </summary>
        /// <returns></returns>
        public static Point[] getGateway()
        {
            Point a = new Point();
            a.X = Convert.ToInt32(ConfigurationManager.AppSettings["Gateway1X"]);
            a.Y = Convert.ToInt32(ConfigurationManager.AppSettings["Gateway1Y"]);
            Point b = new Point();
            b.X = Convert.ToInt32(ConfigurationManager.AppSettings["Gateway2X"]);
            b.Y = Convert.ToInt32(ConfigurationManager.AppSettings["Gateway2Y"]);
            Point c = new Point();
            c.X = Convert.ToInt32(ConfigurationManager.AppSettings["Gateway3X"]);
            c.Y = Convert.ToInt32(ConfigurationManager.AppSettings["Gateway3Y"]);
            return new Point[] { a, b, c };
        }
        public static string getconnStr()
        {
            return ConfigurationManager.ConnectionStrings["connStr"].ToString();
        }
        public static string getdbname()
        {
            return ConfigurationManager.AppSettings["dbname"].ToString();
        }
    }
}
