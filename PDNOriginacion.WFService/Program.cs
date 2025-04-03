using System.ServiceProcess;

namespace PDNOriginacion.WFService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode = DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.v20_1;
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new WFServiceWorkflowServer()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
