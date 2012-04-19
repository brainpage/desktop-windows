using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker
{
    class AppConfig
    {
        public const string SettingsUrl = "http://www.brainpage.com/rsi/settings";
        public const string ViewAnalysisUrl = "http://www.brainpage.com/rsi/charts";
        public const string SocketUrl = "ws://sensocol.brainpage.com:8080/sensocol";
      //  public const string SocketUrl = "ws://192.168.1.50:8080/sensocol";
        public const int EventInterval = 10 * 1000;
        public const int MaxBatchSize = 1024;
        public const int MaxQueueCount = 1000;
        public const int ReconnectMinInterval = 2 * 1000;
        public const int ReconnectMaxInterval = 3600 * 1000;
        public const string StopBreak = "Stop Break";
        public const string Activity = "Activity:";
        public const string TipTitle = "Take a break now";
        public const string TipContent = "Your screen will be locked.\r\nClick this to skip it.";
        public const string WelcomeTitle = "Welcome to RSI";
        public const string WelcomeContent = "Click here to view you data and change settings";
    }
}
