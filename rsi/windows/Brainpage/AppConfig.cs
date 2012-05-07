using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brainpage
{
    class AppConfig
    {
        public const string SettingsUrl = RootUrl + "settings";
        public const string ViewAnalysisUrl = RootUrl + "charts";
        public const string PortalUrl = RootUrl + "portals";
        public const string ScreenSaverUrl = RootUrl + "screen_saver";
        public const string SocketUrl = "ws://sensocol.brainpage.com:8080/sensocol";
        public const int EventInterval = 60 * 1000;
        public const int MaxBatchSize = 1024;
        public const int MaxQueueCount = 1000;
        public const int ReconnectMinInterval = 2 * 1000;
        public const int ReconnectMaxInterval = 3600 * 1000;
        public const string StrStopBreak = "Stop Break";
        public const string StrEnergyLeft = "Energy left: ";
        public const string TipTitle = "Take a break now";

        private const string RootUrl = "http://www.brainpage.com/rsi/";
    }
}
