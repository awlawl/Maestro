﻿using System.ServiceProcess;
using System.Threading;
using MusicData;

namespace MaestroService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[] 
            { 
                new MaestroService() 
            };

            if (args.Length == 0)
            {
                ServiceBase.Run(ServicesToRun);
            }
            else
            {
                Log.Debug("Starting in test mode.");
                ((MaestroService)ServicesToRun[0]).Start();
                while (true)
                    Thread.Sleep(1000);
            }
        }
    }
}
