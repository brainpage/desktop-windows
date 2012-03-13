using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker
{
    [Serializable]
    class AppData
    {
        public string IpAddress { get; private set; }

        public string SensorUUID { get; private set; }
        public string AuthToken { get; private set; }
        public string LoginUrl { get; private set; }
        public bool SettingClicked { get;  set; }

        private static AppData actInstance = null;
        public static AppData GetInstance()
        {
            if (actInstance == null)
            {
                actInstance = (AppData)Utils.ReadFile("data");
                if (actInstance == null)
                    actInstance = new AppData();

                actInstance.SetValues();
            }
            return actInstance;
        }

        public void Save()
        {
            Utils.WriteFile("data", this);
        }

        public void SetLoginUrl(string token)
        {
            this.LoginUrl = token;
            this.Save();
        }

        private AppData()
        {
        }

        // Initilize values
        private void SetValues()
        {
            if (this.SensorUUID == null)
                this.SensorUUID = Utils.Sha1Encrypt(Utils.GetMacAddress());

            if (this.AuthToken == null)
                this.AuthToken = Utils.Sha1Encrypt(Guid.NewGuid().ToString());

            Save();
        }

    }
}
