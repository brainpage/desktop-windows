using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker
{
    class AppConfig
    {
        public const string SecretKey = "ea1020da-cea9-4cef-848e-6a5121af11d4";
        public const string ServerUrl = "http://192.168.96.175:3000/rsi/feeds";
        public const string SocketUrl = "ws://192.168.96.175:8080/";
        public const int EventInterval = 10 * 1000;
        public const int MaxBatchSize = 1024;
        public const int MaxQueueCount = 1000;
        public const int ReconnectMinInterval = 2 * 1000;
        public const int ReconnectMaxInterval = 3600 * 1000;
    }
}
