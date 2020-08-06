﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataCalculation
{
    class Calculation
    {
        private static Kalman kalman_1;
        private static Kalman kalman_2;
        private static Kalman kalman_3;
        public static int[] rssi = new int[3];
        public static Point[] gateway_coordinate;
        private static Point p = new Point(0, 0);
        private static Algorithm algorithm = new Algorithm();
        //public static Point Now;
        //private static Point before;

        public static void CalculateTheDistance(int[][] dictionary)
        {
            kalman_1 = new Kalman(Variable.getPredeterminedVariance(), Variable.getVariance());
            kalman_2 = new Kalman(Variable.getPredeterminedVariance(), Variable.getVariance());
            kalman_3 = new Kalman(Variable.getPredeterminedVariance(), Variable.getVariance());
            rssi[0] = Convert.ToInt32((kalman_count(kalman_1, dictionary[0]) * 100));
            rssi[1] = Convert.ToInt32((kalman_count(kalman_2, dictionary[1]) * 100));
            rssi[2] = Convert.ToInt32((kalman_count(kalman_3, dictionary[2]) * 100));
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
            if ((b - d) <= Variable.getDeviation())//(((d-b)<=deviation)||((b-d)<=deviation))
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
            n = Math.Abs(rssi);
            int d = Variable.getCalibration();
            n = ((n - d) / (10 * Variable.getFactor()));
            n = Math.Pow(10, n);
            n += 0.005;
            n *= 100;
            n = (int)n;
            n = n / 100;
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
            gateway_coordinate = Variable.getGateway();
            algorithm.GetSide(gateway_coordinate);
            rssi=algorithm.LongDouble(rssi);
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
            x += 0.005;
            x *= 100;
            x = (int)x;
            x = x / 100;
            y += 0.005;
            y *= 100;
            y = (int)y;
            y = y / 100;
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
        public static Point calculaton()
        {
            double a = gateway_coordinate[0].X - gateway_coordinate[2].X;
            double b = gateway_coordinate[0].Y - gateway_coordinate[2].Y;
            double c = Math.Pow(gateway_coordinate[0].X, 2) - Math.Pow(gateway_coordinate[2].X, 2) + Math.Pow(gateway_coordinate[0].Y, 2) - Math.Pow(gateway_coordinate[2].Y, 2) + Math.Pow(rssi[2], 2) - Math.Pow(rssi[0], 2);
            double d = gateway_coordinate[1].X - gateway_coordinate[2].X;
            double e = gateway_coordinate[1].Y - gateway_coordinate[2].Y;
            double f = Math.Pow(gateway_coordinate[1].X, 2) - Math.Pow(gateway_coordinate[2].X, 2) + Math.Pow(gateway_coordinate[1].Y, 2) - Math.Pow(gateway_coordinate[2].Y, 2) + Math.Pow(rssi[2], 2) - Math.Pow(rssi[1], 2);
            double x = (b * f - e * c) / (2 * b * d - 2 * a * e);
            double y = (a * f - d * c) / (2 * a * e - 2 * b * d);
            return (new Point(x, y));
        }
    }
}
