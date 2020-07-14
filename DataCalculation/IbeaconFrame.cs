using MessagePack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataCalculation
{
    [MessagePackObject]
    public class IbeaconFrame
    {
        /*
         * 
         *v - 
         * firmware version 
         * mid - message ID 
         * time - 启动时长，以秒计算 
         * ip - gateway 的 IP 
         * mac - gateway 的 mac address 
         * devices - 一个由 BLE 广播包组成的数组, 这些广播包都是 gateway 收集到的 
         */

        // Key is serialization index, it is important for versioning.
        [Key("v")]
        public string v { get; set; }

        [Key("mid")]
        public long mid { get; set; }

        [Key("time")]
        public long time { get; set; }

        [Key("ip")]
        public string ip { get; set; }

        [Key("mac")]
        public string mac { get; set; }

        [Key("devices")]
        public List<byte[]> devices { get; set; }
    }

}
