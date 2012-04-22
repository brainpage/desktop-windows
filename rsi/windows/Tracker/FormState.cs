using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;

namespace Tracker
{
    class FormState
    {
        private static FormState stateInstance = null;
        public static FormState GetInstance()
        {
            if (stateInstance == null)
            {
                throw new Exception("Call SetForm before trying to get instance");
            }
            return stateInstance;
        }

        public static void SetForm(KeyRecord form)
        {
            if (stateInstance == null)
            {
                stateInstance = new FormState(form);
            }
        }

        private ActivityTracker tracker;
        private KeyRecord targetForm;
        private AppData appData;

        private FormState(KeyRecord form)
        {
            tracker = ActivityTracker.GetInstance();
            targetForm = form;
            appData = AppData.GetInstance();
        }

        public bool IsMaximized = false;

        public void Maximize()
        {
            if (!IsMaximized)
            {
                targetForm.Maximize();
            }
        }

        public void Restore()
        {
            targetForm.Restore();
            IsMaximized = false;
        }

        public void BeginNotify()
        {
            this.Maximize();
        }

        public void UpdateActivity(string percent)
        {
            appData.ActivityPercent = percent;
            targetForm.UpdateActivity();
        }

        public void ShowScreenSaver(string url)
        {
            appData.ScreenSaverUrl = url;
            targetForm.ShowScreenSaver();
        }
    }
}
