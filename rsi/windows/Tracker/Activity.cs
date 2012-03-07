using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tracker
{
    [Serializable]
    class Activity
    {
        public string IpAddress { get; private set; }

        public string SensorUUID { get; private set; }
        public string AuthToken { get; private set; }
        public string LoginUrl { get; private set; }

        private static Activity actInstance = null;
        public static Activity GetInstance()
        {
            if (actInstance == null)
            {
                actInstance = (Activity)Utils.ReadFile("data");
                if(actInstance == null)
                    actInstance = new Activity();

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

        private Activity()
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
