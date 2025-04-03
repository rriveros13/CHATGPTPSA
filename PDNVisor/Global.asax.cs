using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Reflection;
using System.Web.Security;
using System.Web.SessionState;
using GdPicture14;
using GdPicture14.WEB;
using Serilog;

namespace PDNVisor
{
    public class Global : HttpApplication
    {
        public static readonly int SESSION_TIMEOUT = 20; //Set to 20 minutes. use -1 to handle DocuVieware session timeout through asp.net session mechanism.
        private const bool STICKY_SESSION = true; //Set false to use DocuVieware on Servers Farm witn non sticky sessions.
        private const DocuViewareSessionStateMode DOCUVIEWARE_SESSION_STATE_MODE = DocuViewareSessionStateMode.InProc; //Set DocuViewareSessionStateMode.File is STICKY_SESSION is False.

        public static string GetCacheDirectory()
        {
            return HttpRuntime.AppDomainAppPath + "\\cache";
        }

        void Application_Start(object sender, EventArgs e)
        {

            Serilog.Debugging.SelfLog.Enable(Console.Error);

            string camino = HttpRuntime.AppDomainAppPath;

            Log.Logger = new LoggerConfiguration()
                .WriteTo.File($@"{camino}\logs\log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Log.Information("Iniciando aplicacion Visor...");
            try
            {
                // Code that runs on application startup
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                DocuViewareManager.SetupConfiguration(true, DocuViewareSessionStateMode.InProc,
                    HttpRuntime.AppDomainAppPath + "\\cache");



                #if DEBUG
                    DocuViewareLicensing.RegisterKEY("");
#else
                    DocuViewareLicensing.RegisterKEY("211823500702000290710001495952896");
#endif

                DocuViewareEventsHandler.CustomAction += CustomActionsHandler;

            }
            catch (Exception ex)
            {
                Log.Error(ex.ToString());
            }


        }

        private static void NewDocumentLoadedHandler(object sender, NewDocumentLoadedEventArgs e)
        {
            e.docuVieware.PagePreload = e.docuVieware.PageCount <= 50 ? PagePreloadMode.AllPages : PagePreloadMode.AdjacentPages;
        }

        private static void CustomActionsHandler(object sender, CustomActionEventArgs e)
        {
            switch (e.actionName)
            {
                case "load":
                    Gallery.HandleLoadAction(e);
                    break;
            }
        }

        public sealed class RegionOfInterest
        {
            public double Left { get; set; }
            public double Top { get; set; }
            public double Width { get; set; }
            public double Height { get; set; }
        }
    }
}