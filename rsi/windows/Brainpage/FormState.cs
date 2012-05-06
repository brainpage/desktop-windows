using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;

namespace Brainpage
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

        public void AfterConnected()
        {
            if (appData.FirstTimeLaunch)
            {
                SensocolSocket.GetInstance().RequestLoginTokenFor(AppConfig.ViewAnalysisUrl);
                appData.SetLaunched();
            }
        }

        public void UpdateDebugInfo(string info)
        {
            appData.DebugInfo = info;
            targetForm.UpdateStatusIcon();
        }

        public void UpdateActivity(int percent)
        {
            if (percent < 10)
            {
                appData.StatusIcon = Properties.Resources.StatusBar0;
            }
            else if (percent < 20)
            {
                appData.StatusIcon = Properties.Resources.StatusBar1;
            }
            else if (percent < 30)
            {
                appData.StatusIcon = Properties.Resources.StatusBar2;
            }
            else if (percent < 40)
            {
                appData.StatusIcon = Properties.Resources.StatusBar3;
            }
            else if (percent < 50)
            {
                appData.StatusIcon = Properties.Resources.StatusBar4;
            }
            else if (percent < 60)
            {
                appData.StatusIcon = Properties.Resources.StatusBar5;
            }
            else if (percent < 70)
            {
                appData.StatusIcon = Properties.Resources.StatusBar6;
            }
            else if (percent < 80)
            {
                appData.StatusIcon = Properties.Resources.StatusBar7;
            }
            else if (percent < 90)
            {
                appData.StatusIcon = Properties.Resources.StatusBar8;
            }
            else if (percent < 100)
            {
                appData.StatusIcon = Properties.Resources.StatusBar9;
            }
            else
            {
                appData.StatusIcon = Properties.Resources.StatusBar10;
            }
            appData.EnergyLeft = (100 - percent).ToString() + " %";
            targetForm.UpdateStatusIcon();
        }

        public void ShowScreenSaver(string url)
        {
            appData.ScreenSaverUrl = url;
            targetForm.ShowScreenSaver();
        }
    }
}
