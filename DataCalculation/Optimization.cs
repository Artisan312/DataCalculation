using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataCalculation
{
    class Optimization
    {
        public Kalman kalman;
        public int[] R { get; set; }
        public int D { get; set; }
        public double[] K { get; set; }
        public double SideS { get; set; }

        public Optimization(int[] n,double s,int d)
        {
            this.kalman = new Kalman(Variable.getVariance(), Variable.getPredeterminedVariance(), Average(n));
            this.K = new double[n.Length];
            this.R = n;
            this.SideS = s;
            this.D = d;
            for (int i = 0; i < R.Length; i++)
                K[1] = R[i];
        }
        private double Average(int[] d)
        {
            double average = 0;
            for (int i = 0; i < d.Length; i++)
            {
                average += d[i];
            }
            return average / d.Length;
        }

        
        public int[] LongDouble(int[] s)
        {
            int n = 0;
            for (int i = 0; i < s.Length; i++)
                n += s[i];
            if (n < SideS)
                for (int i = 0; i < s.Length; i++)
                {
                    s[i] -= 100;
                    s[i] = Convert.ToInt32(s[i] * ((SideS - 300) / n));
                    s[i] += 100;
                }
            return s;

        }
        public  double data( )
        {
            double n = K[R.Length - 11];
            int g = 0,s=0;

            for(int i=(R.Length-10);i<R.Length;i++)
            {
                n =Math.Abs( n - K[i]);
                if (n <= D)
                    g++;
                else if (n >= 8)
                    s++;
            }
            if (g >= 4 && Max(R) < 80)
            {
                return Average(K);
            }
            else if (g >= 4 && Max(R) > 80)
            {
                return Max(K);
            }
            else if(s>=4&& Max(R) > 80)
            {
                return Max(K);
            }
            else if (s >= 4 && Max(R)< 70)
            {
                return Min(K);
            }
            else {
                if (Min(R) < 63 && Max(R) < 70)
                    return Min(K);
                else if (Min(R) < 65 && Max(R) < 70)
                    return Sort(K)[40];
                else if (Min(R) > 60 && Max(R) < 80)
                    return Sort(K)[60];
                else if (Min(R) > 70 && Max(R) < 80)
                    return Sort(K)[70];
                else if (Min(R) > 70 && Max(R) > 85)
                    return Max(K);
                else
                    return Average(K);

            }
        }
        public double[] Sort(double[] d)
        {
            double s = 0;
            for (int i = 0; i < d.Length - 1; i++)
                for (int j = i + 1; j < d.Length; j++)
                {
                    if (d[i] < d[j])
                    {
                        s = d[j];
                        d[j] = d[i];
                        d[i] = s;
                    }
                }
            return d;
        }
        public double Min(int[] d)
        {
            int min = d[0];
            for (int i = 1; i < d.Length; i++)
            {
                if (min > d[i])
                    min = d[i];
            }
            return min;
        }
        public double Max(int[] d)
        {
            int max = d[0];
            for (int i = 1; i < d.Length; i++)
            {
                if (max > d[i])
                    max = d[i];
            }
            return max;
        }
        public double Min(double[] d)
        {
            double min=d[0];
            for(int i=1;i<d.Length;i++)
            {
                if (min > d[i])
                    min = d[i];
            }
            return min;
        }
        public double Max(double[] d)
        {
            double max = d[0];
            for (int i = 1; i < d.Length; i++)
            {
                if (max > d[i])
                    max = d[i];
            }
            return max;
        }
        public double Average(double[] d)
        {
            double average = 0;
            for (int i = 0; i < d.Length; i++)
            {
                average += d[i];
            }
            return average / d.Length;
        }

    }
}
