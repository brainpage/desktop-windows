using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker
{
    class AppConfig
    {
        public const string SecretKey = "ea1020da-cea9-4cef-848e-6a5121af11d4";
        public const string ServerUrl = "http://192.168.96.175:3000/rsi/portals";
        public const string SocketUrl = "ws://192.168.96.175:8080";
        public const int EventInterval = 10 * 1000;
        public const int MaxBatchSize = 1024;
        public const int MaxQueueCount = 1000;
        public const int ReconnectMinInterval = 2 * 1000;
        public const int ReconnectMaxInterval = 3600 * 1000;
        public const int BreakTime = 15 * 1000;
        public const string TipTitle = "Take a break now";
        public const string TipContent = "Your screen will be locked.\r\nClick this to skip it.";
        public const string WelcomeTitle = "Welcome to RSI";
        public const string WelcomeContent = "Click here to view you data and change settings";
    }
}
