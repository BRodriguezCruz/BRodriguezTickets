using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;

namespace TicketWindowsService
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer
    {
        private ServiceInstaller serviceInstaller;
        private ServiceProcessInstaller processInstaller;

        public ProjectInstaller()
        {
            // Instantiate the installers
            processInstaller = new ServiceProcessInstaller();
            serviceInstaller = new ServiceInstaller();

            // Set the account type (e.g., LocalService, NetworkService, etc.)
            processInstaller.Account = ServiceAccount.LocalSystem;

            // Set the service details
            serviceInstaller.StartType = ServiceStartMode.Manual;
            serviceInstaller.ServiceName = "TicketWindService";
            serviceInstaller.DisplayName = "Tickets Win Service";

            // Add the installers to the Installers collection
            Installers.Add(processInstaller);
            Installers.Add(serviceInstaller);
        }
    }
}
