using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AllDataWereAnalyzed
{
    class Calculation
    {
        private static Kalman kalman_1 = new Kalman(8.2, 100.0);
        private static Kalman kalman_2 = new Kalman(8.2, 100.0);
        private static Kalman kalman_3 = new Kalman(8.2, 100.0);
        private static int Le = 60;
        private static double Eaf = 3.8;
        private static int deviation = 5;

        public static int[] rssi=new int[3];
        public static Point[] gateway_coordinate = new Point[] {new Point(0,0), new Point(0, 10), new Point(10, 10)};
        private  static Point p = new Point(0, 0);
        public static Point Now;
        private static Point before;

        public static void CalculateTheDistance(Dictionary<int, int[]> dictionary)
        {
            rssi[0] =(int)(kalman_count(kalman_1, dictionary[0]) * 100);
            rssi[1] =(int)(kalman_count(kalman_2, dictionary[1]) * 100);
            rssi[2] =(int)(kalman_count(kalman_3, dictionary[2]) * 100);
        }
        private static double kalman_count(Kalman kalman, int[] rssi)
        {
            double gap = 0;
            int d = 0;
            for (int i = 0; i < rssi.Length && rssi[i] != 0; i++)
            {
                gap = kalman.KalmanFilter(rssi[i]);
            }
            d = rssi[0];
            for (int i = 1; i < rssi.Length && rssi[i] != 0; i++)
            {
                if (d != Limiting(d, rssi[i]))
                {
                    d = Limiting(d, rssi[i]);
                    gap = kalman.KalmanFilter(d);
                }
            }
            return Count(gap);
        }
        private static int Limiting(int d, int b)
        {
            if ((b - d) <= deviation)//(((d-b)<=deviation)||((b-d)<=deviation))
                return b;
            else
                return d;
        }

        /**
         * ͨ��RSSIֵ�������
         *
         * @param rssi
         * @return
         */
        public static double Count(double rssi)
        {
            double n;
            n = (double)Math.Abs(rssi);
            n = (double)((n - Le) / (10 * Eaf));
            n = (double)Math.Pow(10, n);
            n += 0.005;
            n *= 100;
            n = (int)n;
            n = (double)n / 100;
            return n;
        }

        /**
         * 计算标签位置
         *
         * @param dis 标签离网关距离
         * @param gateway_coordinate  网关位置
         * @return
         */
        public static Point threePoints()
        {
            double x = 0, y = 0;
            double x2, y2;
            if (rssi == null || gateway_coordinate == null)
                return p;

            for (int i = 0; i < 2; i++)
            {
                if (rssi[i] < 0)
                    return p;

                for (int j = i + 1; j < 3; j++)
                {
                    x2 = Math.Pow((gateway_coordinate[i].X - gateway_coordinate[j].X), 2);
                    y2 = Math.Pow((gateway_coordinate[i].Y - gateway_coordinate[j].Y), 2);
                    double p2p = Math.Sqrt(x2 + y2);
                    if (rssi[i] + rssi[j] <= p2p)
                    {
                        x += gateway_coordinate[i].X + (gateway_coordinate[j].X - gateway_coordinate[i].X) * rssi[i] / (rssi[i] + rssi[j]);
                        y += gateway_coordinate[i].Y + (gateway_coordinate[j].Y - gateway_coordinate[i].Y) * rssi[i] / (rssi[i] + rssi[j]);
                    }
                    else
                    {
                        double dr = p2p / 2 + (rssi[i] * rssi[i] - rssi[j] * rssi[j]) / (2 * p2p);
                        x += gateway_coordinate[i].X + (gateway_coordinate[j].X - gateway_coordinate[i].X) * dr / p2p;
                        y += gateway_coordinate[i].Y + (gateway_coordinate[j].Y - gateway_coordinate[i].Y) * dr / p2p;
                    }
                }
            }
            x /= 3;
            y /= 3;
            
            return new Point(x, y);
        }

        /*
         *通过坐标位置计算距离
         * @param a
         * @param b
         */
        private static double count_distance(Point a, Point b)
        {
            double m = Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
            return m;
        }
    }
}
