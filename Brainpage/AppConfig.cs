using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Brainpage
{
    class AppConfig
    {
        public static string SettingsUrl = Strings.rootUrl + "settings";
        public static string ViewAnalysisUrl = Strings.rootUrl + "charts";
        public static string PortalUrl = Strings.rootUrl + "portals";
        public static string ScreenSaverUrl = Strings.rootUrl + "screen_saver";
        public const string SocketUrl = "ws://sensocol.brainpage.com:8080/sensocol";
        public const int EventInterval = 60 * 1000;
        public const int MaxBatchSize = 1024;
        public const int MaxQueueCount = 1000;
        public const int ReconnectMinInterval = 2 * 1000;
        public const int ReconnectMaxInterval = 3600 * 1000;
    }
}
