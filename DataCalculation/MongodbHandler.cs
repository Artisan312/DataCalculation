using MongoDB.Driver;

namespace DataCalculation
{
    /*
     * 未处理System.Windows.Markup.XamlParseException
Message: “System.Windows.Markup.XamlParseException”类型的未经处理的异常在 PresentationFramework.dll 中发生 
其他信息: “对类型“DataCalculation.MainWindow”的构造函数执行符合指定的绑定约束的调用时引发了异常。”，行号为“8”，行位置为“9”。

     * 
     */
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
                var client = new MongoClient("mongodb://10.13.18.40:27017");
                var database = client.GetDatabase("foo");
                var collection = database.GetCollection<PersonD>("3，17");
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
