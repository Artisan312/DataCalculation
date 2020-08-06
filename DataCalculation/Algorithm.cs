using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataCalculation
{
    class Algorithm
    {
        public double[] SideChief;
        public Point Side;
        public double SideS;
        public void GetSide(Point[] p)
        {
            double x = 0,y=0;
            foreach(Point point in p)
            {
                if (x < point.X)
                    x = point.X;
                if (y < point.Y)
                    y = point.Y;
            }
            SideS = x + y;
            Side = (new Point(x, y));
        }
        public int[] LongDouble(int[] s)
        {
            int n = 0;
            for (int i = 0; i < s.Length; i++)
                n += s[i];
            for (int i = 0; i < s.Length; i++)
               s[i]*=(n/3);
            return s;

        }

    }
}
