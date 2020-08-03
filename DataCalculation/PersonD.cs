using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataCalculation
{

    public interface MongoBaseEntity
    {
        double Factor { get; set; }
        int rssi { get; set; }
        TimeSpan time { get; set; }
        List<int> RssiA { get; set; }
        List<int> RssiB { get; set; }
        List<int> RssiC { get; set; }
        int DistanceA { get; set; }
        int DistanceB { get; set; }
        int DistanceC { get; set; }
        Point point { get; set; }
    }
    class PersonD : MongoBaseEntity
    {
        public double Factor { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int rssi { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public TimeSpan time { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<int> RssiA { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<int> RssiB { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public List<int> RssiC { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int DistanceA { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int DistanceB { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public int DistanceC { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public Point point { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}
