using System.ComponentModel;
using System.ServiceProcess;
using System.Configuration.Install;

namespace TicketWindowsService
{
    [RunInstaller(true)]
    public class ProjectInstaller : Installer //se crea esta clase ProjectInstaller para que se pueda instalar en los servicios de windows de la maquina, si ella el proyecto no 
                                              // puede ser instalado y no se ve reflejado para su activacion en los servicios de windows de la maquina 
                                              //se debe agregar referencia a "using System.Configuration.Install;" en los assemblies del proyecto
                                              //se debe ejecutar como administrador el visual studio para que pueda instalarse en los servicios de windows con "installutil
                                              //si no, no se instala tampoco
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
