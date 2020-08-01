

using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Connecting;
using MQTTnet.Client.Disconnecting;
using MQTTnet.Client.Options;
using MQTTnet.Client.Receiving;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AllDataWereAnalyzed
{
    class HOSMQTT:Person
    {
        public static  byte[] text;
        public  event ChangedHandler ChangeName;
        public event ChangedHandler Position;
        private static MqttClient mqttClient = null;
        private static IMqttClientOptions options = null;
        private static bool runState = false;
        private static bool running = false;

        /// <summary>
        /// 服务器IP
        /// </summary>
       // public static string ServerUrl = ConfigurationManager.ConnectionStrings["ServerUrl"].ToString();
        /// <summary>
        /// 服务器端口
        /// </summary>
       // public static int Port =Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        /// <summary>
        /// 选项 - 开启登录 - 密码
        /// </summary>
       // public static string Password = ConfigurationManager.AppSettings["Password"];
        /// <summary>
        /// 选项 - 开启登录 - 用户名
        /// </summary>
        //public static string UserId = ConfigurationManager.AppSettings["UserId"];
        /// <summary>
        /// 主题
        /// <para>China/Hunan/Yiyang/Nanxian</para>
        /// <para>Hotel/Room01/Tv</para>
        /// <para>Hospital/Dept01/Room001/Bed001</para>
        /// <para>Hospital/#</para>
        /// </summary>
        //public static string Topic= ConfigurationManager.AppSettings["Topic"];
        /// <summary>
        /// 保留
        /// </summary>
        private static bool Retained = false;
        /// <summary>
        /// 服务质量
        /// <para>0 - 至多一次</para>
        /// <para>1 - 至少一次</para>
        /// <para>2 - 刚好一次</para>
        /// </summary>
        private static int QualityOfServiceLevel = 0;
        public  void Stop()
        {
            runState = false;
        }

        public  bool IsRun()
        {
            return (runState && running);
        }
        /// <summary>
        /// 启动客户端
        /// </summary>
        public  string Start()
        {
             
            try
            {
                runState = true;
                if (mqttClient == null)
                {
                    
                    System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(Work));
                    thread.Start();
                }
                return "成功";
            }
            catch (Exception exp)
            {

                return exp.Message;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        private  void Work()
        {
            running = true;
            // Console.WriteLine("Work >>Begin");
            try
            {
                var factory = new MqttFactory();
                mqttClient = factory.CreateMqttClient() as MqttClient;

                options = new MqttClientOptionsBuilder()
                    .WithTcpServer(ConfigurationManager.ConnectionStrings["ServerUrl"].ToString(), Convert.ToInt32(ConfigurationManager.AppSettings["Port"]))
                    .WithCredentials(ConfigurationManager.AppSettings["UserId"], ConfigurationManager.AppSettings["Password"])
                    .WithClientId(Guid.NewGuid().ToString().Substring(0, 5))
                    .Build();

                mqttClient.ConnectAsync(options);
                mqttClient.ConnectedHandler = new MqttClientConnectedHandlerDelegate(new Func<MqttClientConnectedEventArgs, Task>(Connected));
                mqttClient.DisconnectedHandler = new MqttClientDisconnectedHandlerDelegate(new Func<MqttClientDisconnectedEventArgs, Task>(Disconnected));
                mqttClient.ApplicationMessageReceivedHandler = new MqttApplicationMessageReceivedHandlerDelegate(new Action<MqttApplicationMessageReceivedEventArgs>(MqttApplicationMessageReceived));
                while (runState)
                {
                    System.Threading.Thread.Sleep(100);
                }


            }
            catch (Exception exp)
            {

                //exp.Message;

            }
            //   Console.WriteLine("Work >>End");
            running = false;
            runState = false;

        }


        /// <summary>
        /// 发布
        /// <paramref name="QoS"/>
        /// <para>0 - 最多一次</para>
        /// <para>1 - 至少一次</para>
        /// <para>2 - 仅一次</para>
        /// </summary>
        /// <param name="Topic">发布主题</param>
        /// <param name="Message">发布内容</param>
        /// <returns></returns>
        public  string Publish(string Topic, string Message)
        {
            try
            {
                if (mqttClient == null) return "失败";
                if (mqttClient.IsConnected == false)
                    mqttClient.ConnectAsync(options);

                if (mqttClient.IsConnected == false)
                    return "失败";


                // Console.WriteLine("Publish >>Topic: " + Topic + "; QoS: " + QualityOfServiceLevel + "; Retained: " + Retained + ";");
                //  Console.WriteLine("Publish >>Message: " + Message);
                MqttApplicationMessageBuilder mamb = new MqttApplicationMessageBuilder()
                 .WithTopic(Topic)
                 .WithPayload(Message).WithRetainFlag(Retained);
                if (QualityOfServiceLevel == 0)
                {
                    mamb = mamb.WithAtMostOnceQoS();
                }
                else if (QualityOfServiceLevel == 1)
                {
                    mamb = mamb.WithAtLeastOnceQoS();
                }
                else if (QualityOfServiceLevel == 2)
                {
                    mamb = mamb.WithExactlyOnceQoS();
                }

                mqttClient.PublishAsync(mamb.Build());
                return "成功";
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }
        /// <summary>
        /// 连接服务器并按标题订阅内容
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private  async Task Connected(MqttClientConnectedEventArgs e)
        {
            try
            {
                List<TopicFilter> listTopic = new List<TopicFilter>();
                if (listTopic.Count <= 0)
                {
                    var topicFilterBulder = new TopicFilterBuilder().WithTopic(ConfigurationManager.AppSettings["Topic"]).Build();
                    listTopic.Add(topicFilterBulder);
                    //Console.WriteLine("Connected >>Subscribe " + Topic);
                }
                await mqttClient.SubscribeAsync(listTopic.ToArray());
                Console.WriteLine("Connected >>Subscribe Success");
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
        /// <summary>
        /// 失去连接触发事件
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        private  async Task Disconnected(MqttClientDisconnectedEventArgs e)
        {
            try
            {
                Console.WriteLine("Disconnected >>Disconnected Server");
                await Task.Delay(TimeSpan.FromSeconds(5));
                try
                {
                    await mqttClient.ConnectAsync(options);
                }
                catch (Exception exp)
                {
                    Console.WriteLine("Disconnected >>Exception " + exp.Message);
                }

            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
        }
        /// <summary>
        /// 接收消息触发事件
        /// </summary>
        /// <param name="e"></param>
        private  void MqttApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
        {
            try
            {
                text =e.ApplicationMessage.Payload;
                string Topic = e.ApplicationMessage.Topic;
                string QoS = e.ApplicationMessage.QualityOfServiceLevel.ToString();
                string Retained = e.ApplicationMessage.Retain.ToString();


            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.Message);
            }
            Age = text;
        }
        object age = 0;
        public virtual object Age
        {
            get { return age; }
            set
            {
                base.OnChanged(this, value, Age, ChangeName);
                age=value ;
                OnPropertyChanged("Age");
            }     
        }
        Point p=new Point(0,0);
        public virtual Point point
        {
            get { return p; }
            set
            {
                base.OnChanged(this, value, point, Position);
                p = value;
                OnPropertyChanged("point");
            }
        }
    }
}
