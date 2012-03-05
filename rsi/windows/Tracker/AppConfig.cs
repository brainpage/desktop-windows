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
        public const int EventInterval = 60 * 1000;
    }
}
