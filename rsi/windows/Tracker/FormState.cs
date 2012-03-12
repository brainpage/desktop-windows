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

        public int lockTime;
        private int warnTime;
        private System.Timers.Timer warnTimer;
        private Stopwatch restTime;

        private FormState(KeyRecord form)
        {
            tracker = ActivityTracker.GetInstance();

            targetForm = form;

        }

        public bool IsMaximized = false;

        public void Maximize()
        {
            if (!IsMaximized)
            {
                targetForm.Maximize();
            }
        }

        public void Restore(string eventName)
        {
            if (warnTimer != null)
                warnTimer.Enabled = false;

            targetForm.Restore();
            IsMaximized = false;

            restTime.Stop();

            Dictionary<string, object> data = new Dictionary<string, object>();
            data.Add("dur", restTime.Elapsed.Seconds);

            SensocolSocket.GetInstance().SendEvent(eventName, data);
        }

        public void BeginNotify(int warnTime, int lockTime)
        {
            this.warnTime = warnTime;
            this.lockTime = lockTime;

            warnTimer = new System.Timers.Timer();
            warnTimer.Interval = warnTime * 1000;
            warnTimer.Elapsed += new ElapsedEventHandler(this.warnTimer_Elapsed);
            warnTimer.AutoReset = false;
            warnTimer.Start();

            restTime = new Stopwatch();
            restTime.Start();

            targetForm.PopupNotification(warnTime);
        }

        void warnTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            this.Maximize();
        }
    }
}
