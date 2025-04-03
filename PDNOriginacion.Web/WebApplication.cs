using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.AuditTrail;
using DevExpress.ExpressApp.CloneObject;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Dashboards;
using DevExpress.ExpressApp.Dashboards.Web;
using DevExpress.ExpressApp.FileAttachments.Web;
using DevExpress.ExpressApp.Maps.Web;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.Notifications.Web;
using DevExpress.ExpressApp.Objects;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.ReportsV2.Web;
using DevExpress.ExpressApp.Scheduler;
using DevExpress.ExpressApp.Scheduler.Web;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Security.ClientServer;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Validation.Web;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using DevExpress.ExpressApp.Web.SystemModule;
using DevExpress.ExpressApp.Workflow;
using DevExpress.ExpressApp.Workflow.Versioning;
using DevExpress.ExpressApp.Workflow.Xpo;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Workflow.Xpo;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using ITTI;
using PDNOriginacion.Module;
using PDNOriginacion.Module.BusinessObjects;
using PDNOriginacion.Module.Web;
using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Web;

namespace PDNOriginacion.Web
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/DevExpressExpressAppWebWebApplicationMembersTopicAll.aspx
    public partial class PDNOriginacionAspNetApplication : WebApplication
    {
        private AuditTrailModule auditTrailModule;
        private CloneObjectModule cloneObjectModule;
        private ConditionalAppearanceModule conditionalAppearanceModule;
        private DashboardsAspNetModule dashboardsAspNetModule;
        private DashboardsModule dashboardsModule;
        private FileAttachmentsAspNetModule fileAttachmentsAspNetModule;
        private FileDataITTIModule fileDataITTIModule1;
        private MapsAspNetModule mapsAspNetModule;
        private SystemModule module1;
        private SystemAspNetModule module2;
        private PDNOriginacionModule module3;
        private PDNOriginacionAspNetModule module4;
        private NotificationsAspNetModule notificationsAspNetModule;
        private NotificationsModule notificationsModule;
        private BusinessClassLibraryCustomizationModule objectsModule;
        private ReportsAspNetModuleV2 reportsAspNetModuleV2;
        private ReportsModuleV2 reportsModuleV2;
        private SchedulerAspNetModule schedulerAspNetModule;
        private SchedulerModuleBase schedulerModuleBase;
        private SecurityModule securityModule1;
        private SecurityStrategyComplex securityStrategyComplex1;
        private ValidationAspNetModule validationAspNetModule;
        private ValidationModule validationModule;
        private DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule viewVariantsModule1;
        private UserViewVariants.UserViewVariantsModule userViewVariantsModule1;
        private UserViewVariants.Web.UserViewVariantsWeb userViewVariantsWeb1;
        private AuthenticationStandard authenticationStandard1;
        private HyperLinkPropertyEditor.Web.HyperLinkPropertyEditorAspNetModule hyperLinkPropertyEditorAspNetModule1;
        private WorkflowModule workflowModule1;

        public PDNOriginacionAspNetApplication()
        {
            
            InitializeComponent();
            InitializeDefaults();
            //SafePostgreSqlConnectionProvider.Register();
        }

        private IXpoDataStoreProvider GetDataStoreProvider(string connectionString, IDbConnection connection)
        {
            HttpApplicationState application = HttpContext.Current?.Application;
            IXpoDataStoreProvider dataStoreProvider;
            if (application != null && application["DataStoreProvider"] != null)
            {
                dataStoreProvider = application["DataStoreProvider"] as IXpoDataStoreProvider;
            }
            else
            {
                dataStoreProvider = XPObjectSpaceProvider.GetDataStoreProvider(connectionString, connection, true);
                if (application != null)
                {
                    application["DataStoreProvider"] = dataStoreProvider;
                }
            }
            return dataStoreProvider;
        }

        private void InitializeComponent()
        {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule();
            this.module3 = new PDNOriginacion.Module.PDNOriginacionModule();
            this.module4 = new PDNOriginacion.Module.Web.PDNOriginacionAspNetModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.securityStrategyComplex1 = new DevExpress.ExpressApp.Security.SecurityStrategyComplex();
            this.authenticationStandard1 = new DevExpress.ExpressApp.Security.AuthenticationStandard();
            this.auditTrailModule = new DevExpress.ExpressApp.AuditTrail.AuditTrailModule();
            this.objectsModule = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.cloneObjectModule = new DevExpress.ExpressApp.CloneObject.CloneObjectModule();
            this.conditionalAppearanceModule = new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule();
            this.dashboardsModule = new DevExpress.ExpressApp.Dashboards.DashboardsModule();
            this.dashboardsAspNetModule = new DevExpress.ExpressApp.Dashboards.Web.DashboardsAspNetModule();
            this.fileAttachmentsAspNetModule = new DevExpress.ExpressApp.FileAttachments.Web.FileAttachmentsAspNetModule();
            this.mapsAspNetModule = new DevExpress.ExpressApp.Maps.Web.MapsAspNetModule();
            this.notificationsModule = new DevExpress.ExpressApp.Notifications.NotificationsModule();
            this.notificationsAspNetModule = new DevExpress.ExpressApp.Notifications.Web.NotificationsAspNetModule();
            this.reportsModuleV2 = new DevExpress.ExpressApp.ReportsV2.ReportsModuleV2();
            this.reportsAspNetModuleV2 = new DevExpress.ExpressApp.ReportsV2.Web.ReportsAspNetModuleV2();
            this.schedulerModuleBase = new DevExpress.ExpressApp.Scheduler.SchedulerModuleBase();
            this.schedulerAspNetModule = new DevExpress.ExpressApp.Scheduler.Web.SchedulerAspNetModule();
            this.validationModule = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.validationAspNetModule = new DevExpress.ExpressApp.Validation.Web.ValidationAspNetModule();
            this.workflowModule1 = new DevExpress.ExpressApp.Workflow.WorkflowModule();
            this.fileDataITTIModule1 = new ITTI.FileDataITTIModule();
            this.viewVariantsModule1 = new DevExpress.ExpressApp.ViewVariantsModule.ViewVariantsModule();
            this.userViewVariantsModule1 = new UserViewVariants.UserViewVariantsModule();
            this.userViewVariantsWeb1 = new UserViewVariants.Web.UserViewVariantsWeb();
            this.hyperLinkPropertyEditorAspNetModule1 = new HyperLinkPropertyEditor.Web.HyperLinkPropertyEditorAspNetModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // securityStrategyComplex1
            // 
            this.securityStrategyComplex1.AllowAnonymousAccess = false;
            this.securityStrategyComplex1.AssociationPermissionsMode = DevExpress.ExpressApp.Security.AssociationPermissionsMode.ExtendedAuto;
            this.securityStrategyComplex1.Authentication = this.authenticationStandard1;
            this.securityStrategyComplex1.PermissionsReloadMode = DevExpress.ExpressApp.Security.PermissionsReloadMode.NoCache;
            this.securityStrategyComplex1.RoleType = typeof(PDNOriginacion.Module.BusinessObjects.Rol);
            this.securityStrategyComplex1.SupportNavigationPermissionsForTypes = false;
            this.securityStrategyComplex1.UserType = typeof(PDNOriginacion.Module.BusinessObjects.Usuario);
            // 
            // authenticationStandard1
            // 
            this.authenticationStandard1.LogonParametersType = typeof(DevExpress.ExpressApp.Security.AuthenticationStandardLogonParameters);
            // 
            // auditTrailModule
            // 
            this.auditTrailModule.AuditDataItemPersistentType = typeof(DevExpress.Persistent.BaseImpl.AuditDataItemPersistent);
            // 
            // cloneObjectModule
            // 
            this.cloneObjectModule.ClonerType = null;
            // 
            // dashboardsModule
            // 
            this.dashboardsModule.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData);
            // 
            // mapsAspNetModule
            // 
            this.mapsAspNetModule.GoogleApiKey = "AIzaSyBs2dOwj6YIlqiAQC8yUbsnQYBbyWfFIIk";
            // 
            // notificationsModule
            // 
            this.notificationsModule.CanAccessPostponedItems = false;
            this.notificationsModule.NotificationsRefreshInterval = System.TimeSpan.Parse("00:05:00");
            this.notificationsModule.NotificationsStartDelay = System.TimeSpan.Parse("00:00:05");
            this.notificationsModule.ShowDismissAllAction = false;
            this.notificationsModule.ShowNotificationsWindow = true;
            this.notificationsModule.ShowRefreshAction = false;
            // 
            // reportsModuleV2
            // 
            this.reportsModuleV2.EnableInplaceReports = true;
            this.reportsModuleV2.ReportDataType = typeof(DevExpress.Persistent.BaseImpl.ReportDataV2);
            this.reportsModuleV2.ReportStoreMode = DevExpress.ExpressApp.ReportsV2.ReportStoreModes.XML;
            // 
            // reportsAspNetModuleV2
            // 
            this.reportsAspNetModuleV2.ReportViewerType = DevExpress.ExpressApp.ReportsV2.Web.ReportViewerTypes.HTML5;
            // 
            // validationModule
            // 
            this.validationModule.AllowValidationDetailsAccess = true;
            this.validationModule.IgnoreWarningAndInformationRules = false;
            // 
            // workflowModule1
            // 
            this.workflowModule1.RunningWorkflowInstanceInfoType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoRunningWorkflowInstanceInfo);
            this.workflowModule1.StartWorkflowRequestType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoStartWorkflowRequest);
            this.workflowModule1.UserActivityVersionType = typeof(DevExpress.ExpressApp.Workflow.Versioning.XpoUserActivityVersion);
            this.workflowModule1.WorkflowControlCommandRequestType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoWorkflowInstanceControlCommandRequest);
            this.workflowModule1.WorkflowDefinitionType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoWorkflowDefinition);
            this.workflowModule1.WorkflowInstanceKeyType = typeof(DevExpress.Workflow.Xpo.XpoInstanceKey);
            this.workflowModule1.WorkflowInstanceType = typeof(DevExpress.Workflow.Xpo.XpoWorkflowInstance);
            // 
            // PDNOriginacionAspNetApplication
            // 
            this.ApplicationName = "PDNOriginacion";
            this.CheckCompatibilityType = DevExpress.ExpressApp.CheckCompatibilityType.DatabaseSchema;
            this.Modules.Add(this.module1);
            this.Modules.Add(this.module2);
            this.Modules.Add(this.auditTrailModule);
            this.Modules.Add(this.objectsModule);
            this.Modules.Add(this.cloneObjectModule);
            this.Modules.Add(this.conditionalAppearanceModule);
            this.Modules.Add(this.dashboardsModule);
            this.Modules.Add(this.notificationsModule);
            this.Modules.Add(this.reportsModuleV2);
            this.Modules.Add(this.schedulerModuleBase);
            this.Modules.Add(this.validationModule);
            this.Modules.Add(this.workflowModule1);
            this.Modules.Add(this.hyperLinkPropertyEditorAspNetModule1);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.dashboardsAspNetModule);
            this.Modules.Add(this.fileAttachmentsAspNetModule);
            this.Modules.Add(this.mapsAspNetModule);
            this.Modules.Add(this.notificationsAspNetModule);
            this.Modules.Add(this.reportsAspNetModuleV2);
            this.Modules.Add(this.schedulerAspNetModule);
            this.Modules.Add(this.validationAspNetModule);
            this.Modules.Add(this.fileDataITTIModule1);
            this.Modules.Add(this.viewVariantsModule1);
            this.Modules.Add(this.userViewVariantsModule1);
            this.Modules.Add(this.userViewVariantsWeb1);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.securityModule1);
            this.Security = this.securityStrategyComplex1;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.PDNOriginacionAspNetApplication_DatabaseVersionMismatch);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        private void PDNOriginacionAspNetApplication_DatabaseVersionMismatch(object sender,
                                                                             DatabaseVersionMismatchEventArgs e)
        {
#if EASYTEST
            e.Updater.Update();
            e.Handled = true;
#else
            if (Debugger.IsAttached)
            {
                e.Updater.Update();
                e.Handled = true;
            }
            else
            {
                string message = "The application cannot connect to the specified database, " +
                    "because the database doesn't exist, its version is older " +
                    "than that of the application or its schema does not match " +
                    "the ORM data model structure. To avoid this error, use one " +
                    "of the solutions from the https://www.devexpress.com/kb=T367835 KB Article.";

                if (e.CompatibilityError != null && e.CompatibilityError.Exception != null)
                {
                    message += "\r\n\r\nInner exception: " + e.CompatibilityError.Exception.Message;
                }
                throw new InvalidOperationException(message);
            }
#endif
        }

        protected override void CreateDefaultObjectSpaceProvider(CreateCustomObjectSpaceProviderEventArgs args)
        {
            args.ObjectSpaceProvider = new SecuredObjectSpaceProvider((SecurityStrategyComplex)Security,
                                                                      GetDataStoreProvider(args.ConnectionString,
                                                                                           args.Connection),
                                                                      true);
            args.ObjectSpaceProviders.Add(new NonPersistentObjectSpaceProvider(TypesInfo, null));
            ((SecuredObjectSpaceProvider)args.ObjectSpaceProvider).AllowICommandChannelDoWithSecurityContext = true;
        }

        protected override void OnLoggedOn(LogonEventArgs args)
        {
            base.OnLoggedOn(args);
            //using(IObjectSpace objectSpace = CreateObjectSpace())
            //{
            Usuario user = (Usuario)(SecuritySystem.CurrentUser);
            if (user.PasswordNuncaExpira)
                return;

            Configuracion config = user.Session.FindObject<Configuracion>(null);
            if (config.SecurityPolicy.MaxPasswordAge <= 0)
                return;

            XPCollection<LPassword> ultimoPassword = new XPCollection<LPassword>(user.Session,
                                                                                 CriteriaOperator.Parse("Usuario.Oid=?",
                                                                                                        user.Oid),
                                                                                 new SortProperty("PWChangeDate",
                                                                                                  SortingDirection.Descending))
            { TopReturnedObjects = 1 };

            if (ultimoPassword.Count == 1)
            {
                if ((DateTime.Now - ultimoPassword[0].PWChangeDate) >
                    new TimeSpan(config.SecurityPolicy.MaxPasswordAge, 0, 0, 0))
                {
                    user.ChangePasswordOnFirstLogon = true;
                }
            }
            user.ObjectSpace.CommitChanges();
            //}
        }

        protected override IViewUrlManager CreateViewUrlManager()
        {
            return new ViewUrlManager();
        }


        #region Default XAF configuration options (https://www.devexpress.com/kb=T501418)
        static PDNOriginacionAspNetApplication()
        {
            EnableMultipleBrowserTabsSupport = true;
            ASPxGridListEditor.AllowFilterControlHierarchy = true;
            ASPxGridListEditor.MaxFilterControlHierarchyDepth = 3;
            ASPxCriteriaPropertyEditor.AllowFilterControlHierarchyDefault = true;
            ASPxCriteriaPropertyEditor.MaxHierarchyDepthDefault = 3;
            PasswordCryptographer.EnableRfc2898 = true;
            PasswordCryptographer.SupportLegacySha512 = false;
        }

        private void InitializeDefaults()
        {
            LinkNewObjectToParentImmediately = false;
            OptimizedControllersCreation = true;
        }
        #endregion
    }
}