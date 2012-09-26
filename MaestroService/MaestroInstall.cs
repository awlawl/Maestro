using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.Threading.Tasks;

namespace MaestroService
{
    [RunInstaller(true)]
    public partial class MaestroInstall : System.Configuration.Install.Installer
    {
        public MaestroInstall()
        {
            InitializeComponent();

            var serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            var serviceInstaller1 = new System.ServiceProcess.ServiceInstaller();

            serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            serviceProcessInstaller1.Password = null;
            serviceProcessInstaller1.Username = null;
            serviceInstaller1.DisplayName = "Maestro";
            serviceInstaller1.ServiceName = "Maestro";
            serviceInstaller1.Description = "Music jukebox";

            serviceInstaller1.StartType = System.ServiceProcess.ServiceStartMode.Automatic;

            Installers.AddRange(new System.Configuration.Install.Installer[] {
                serviceProcessInstaller1,
                serviceInstaller1
            });
        }
    }
}
