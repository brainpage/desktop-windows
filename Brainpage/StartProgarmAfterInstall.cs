using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;
using System.Diagnostics;

namespace Brainpage
{
    // Set 'RunInstaller' attribute to true.
    [RunInstaller(true)]
    public class StartProgarmAfterInstall : Installer
    {
        public StartProgarmAfterInstall()
            : base()
        {
            this.AfterInstall += new InstallEventHandler(AfterInstallEventHandler);
        }

        private void AfterInstallEventHandler(object sender, InstallEventArgs e)
        {
            Process p = new Process();
            InstallContext cont = this.Context;

            ProcessStartInfo inf = new ProcessStartInfo(cont.Parameters["assemblypath"]);
            p.StartInfo = inf;
            p.Start();
        }

        // Override the 'Install' method.
        public override void Install(IDictionary savedState)
        {
            base.Install(savedState);
        }
        // Override the 'Commit' method.
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
        }
        // Override the 'Rollback' method.
        public override void Rollback(IDictionary savedState)
        {
            base.Rollback(savedState);
        }
    }

}



