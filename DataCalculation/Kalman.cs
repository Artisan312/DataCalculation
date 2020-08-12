using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCalculation
{
    public class Kalman
    {
        private double startValue;
        private double kalmanGain;
        private double A;
        private double H;
        private double Q;
        private double R;
        private double P;

        public Kalman(double q, double r,double s)
        {
            A = 1;
            H = 1;
            P = 8;
            Q = q;
            R = r;
            startValue = s;
        }
        public double KalmanFilter(double value)
        {
            double predictValue = A * startValue;
            P = A * A * P + Q;
            kalmanGain = P * H / (P * H * H + R);
            startValue = predictValue + (value - predictValue) * kalmanGain;
            P = (1 - kalmanGain * H) * P;
            return startValue;
        }

    }
}
