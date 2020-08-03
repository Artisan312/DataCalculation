using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCalculation
{
    class MongodbHandler
    {
        private static string MongodbDefaultUrl;
        private static string MongodbDefaultDBName; 
        public MongodbHandler()
        {
            MongodbDefaultUrl = Variable.getconnStr();
            MongodbDefaultDBName = Variable.getdbname();
    }
        public bool Add(PersonD p)
        {
            try
            {
                var client = new MongoClient("mongodb://127.0.0.1:27017");
                var database = client.GetDatabase("foo");
                var collection = database.GetCollection<PersonD>("PersonD");
                collection.InsertOne(p);
                return true;
            }
            catch
            {
                return false;
            }
        }


    }
}
