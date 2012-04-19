using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Tracker
{
    [Serializable]
    class AppData
    {
        public string IpAddress { get; private set; }

        public string SensorUUID { get; private set; }
        public string AuthToken { get; private set; }
        public bool SettingClicked { get; set; }
        public string SettingUrl { get; set; }
        public string ChartUrl { get; set; }
        public string ActivityPercent { get; set; }

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
                        token = Utils.Sha1Encrypt(Guid.NewGuid().ToString());
                        Application.UserAppDataRegistry.SetValue("auth_token", token);
                    }
                    actInstance.AuthToken = token;

                    object clicked = Application.UserAppDataRegistry.GetValue("setting_clicked");
                    actInstance.SettingClicked = (clicked != null && (string)clicked == "True");
                }

            }
            return actInstance;
        }

        public void ClickSetting()
        {
            this.SettingClicked = true;
            Application.UserAppDataRegistry.SetValue("setting_clicked", true);
        }

        private AppData()
        {
        }

    }
}
