using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing;

namespace Brainpage
{
    [Serializable]
    class AppData
    {
        public string IpAddress { get; private set; }

        public string SensorUUID { get; private set; }
        public string AuthToken { get; private set; }
        public bool FirstTimeLaunch { get; set; }
        public string SettingUrl { get; set; }
        public string ChartUrl { get; set; }
        public Icon StatusIcon { get; set; }
        public string EnergyLeft { get; set; }
        public string ConnectionStatus { get; set; }
        public bool Connected { get; set; }
        public string DebugInfo { get; set; }
        public Stopwatch FromLastBreak { get; set; }
        public string ScreenSaverUrl { get; set; }

        private static AppData actInstance = null;
        public static AppData GetInstance()
        {
            if (actInstance == null)
            {
                if (actInstance == null)
                {
                    actInstance = new AppData();

                    string uuid = (string)Application.UserAppDataRegistry.GetValue("sensor_uuid");
                   
                    if (uuid == null || "".Equals(uuid))
                    {
                        uuid = Utils.Sha1Encrypt(Utils.GetMacAddress());
                        Application.UserAppDataRegistry.SetValue("sensor_uuid", uuid);
                    }
                    actInstance.SensorUUID = uuid;

                    string token = (string)Application.UserAppDataRegistry.GetValue("auth_token");
                    if (token == null || "".Equals(token))
                    {
                        token = Utils.Sha1Encrypt(Utils.GetMacAddress() + "brainpage");
                        Application.UserAppDataRegistry.SetValue("auth_token", token);
                    }
                    actInstance.AuthToken = token;

                    actInstance.FirstTimeLaunch = (Application.UserAppDataRegistry.GetValue("first_time") == null);
                }

            }
            return actInstance;
        }

        public void SetLaunched()
        {
            this.FirstTimeLaunch = false;
            Application.UserAppDataRegistry.SetValue("first_time", "launched");
        }

       
        private AppData()
        {
        }

    }
}
