using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataCalculation
{
    class PersonD 
    {
        public double Factor { get; set; }
        public int rssi { get; set; }
        public TimeSpan time { get; set; }
        public List<int> RssiA { get; set; }
        public List<int> RssiB { get; set; }
        public List<int> RssiC { get; set; }
        public int DistanceA { get; set; }
        public int DistanceB { get; set; }
        public int DistanceC { get; set; }
        public Point point { get; set; }
    }
}

