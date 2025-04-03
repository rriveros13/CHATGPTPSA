using System;
using System.Configuration;
using System.ServiceModel;
using DevExpress.ExpressApp.Workflow.Server;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Workflow;
using DevExpress.Workflow;
using PDNOriginacion.Module;
using PDNOriginacion.Module.Web;
using PDNOriginacion.Module.Win;

namespace PDNOriginacion.WFService {
    public partial class WFServiceWorkflowServer  : System.ServiceProcess.ServiceBase {
        private WorkflowServer server;
        protected override void OnStart(string[] args) {
            if(server == null) {
                ServerApplication serverApplication = new ServerApplication();
                serverApplication.ApplicationName = "PDNOriginacion.WFService";
				serverApplication.CheckCompatibilityType = CheckCompatibilityType.DatabaseSchema;
                // The service can only manage workflows for those business classes that are contained in Modules specified by the serverApplication.Modules collection.
                // So, do not forget to add the required Modules to this collection via the serverApplication.Modules.Add method.
                serverApplication.Modules.BeginInit();
                serverApplication.Modules.Add(new WorkflowModule());
                serverApplication.Modules.Add(new PDNOriginacionModule());
                serverApplication.Modules.Add(new PDNOriginacionAspNetModule());
                serverApplication.Modules.Add(new PDNOriginacionWindowsFormsModule());
                //serverApplication.Modules.Add(new PDNOriginacionMobileModule());

                if (ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                    serverApplication.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                }
                serverApplication.Setup();
                serverApplication.Logon();

                IObjectSpaceProvider objectSpaceProvider = serverApplication.ObjectSpaceProvider;

                WorkflowCreationKnownTypesProvider.AddKnownType(typeof(DevExpress.Xpo.Helpers.IdList));

                server = new WorkflowServer("http://localhost:46232", new BasicHttpBinding(), objectSpaceProvider, objectSpaceProvider);
                server.StartWorkflowListenerService.DelayPeriod = TimeSpan.FromSeconds(15);
                server.StartWorkflowByRequestService.DelayPeriod = TimeSpan.FromSeconds(15);
                server.RefreshWorkflowDefinitionsService.DelayPeriod = TimeSpan.FromMinutes(15);

                server.CustomizeHost += delegate(object sender, CustomizeHostEventArgs e) {
                    e.WorkflowInstanceStoreBehavior.WorkflowInstanceStore.RunnableInstancesDetectionPeriod = TimeSpan.FromSeconds(15);
                };

                server.CustomHandleException += delegate(object sender, CustomHandleServiceExceptionEventArgs e) {
                    Tracing.Tracer.LogError(e.Exception);
                    e.Handled = false;
                };
            }
            server.Start();
        }
        protected override void OnStop() {
            server.Stop();
        }
        public WFServiceWorkflowServer() {
            InitializeComponent();
        }
    }
}
