using System.ServiceProcess;
using System.Threading;

namespace MaestroService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new MaestroService() 
            };

#if !SERVICE_DEBUG
            ServiceBase.Run(ServicesToRun);
#else
           ((MaestroService) ServicesToRun[0]).Start();
           while (true)
               Thread.Sleep(1000);
#endif
        }
    }
}
