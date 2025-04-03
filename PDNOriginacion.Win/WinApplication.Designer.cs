namespace PDNOriginacion.Win {
    partial class PDNOriginacionWindowsFormsApplication {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if(disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.module1 = new DevExpress.ExpressApp.SystemModule.SystemModule();
            this.module2 = new DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule();
            this.module3 = new PDNOriginacion.Module.PDNOriginacionModule();
            this.module4 = new PDNOriginacion.Module.Win.PDNOriginacionWindowsFormsModule();
            this.securityModule1 = new DevExpress.ExpressApp.Security.SecurityModule();
            this.securityStrategyComplex1 = new DevExpress.ExpressApp.Security.SecurityStrategyComplex();
            this.authenticationStandard1 = new DevExpress.ExpressApp.Security.AuthenticationStandard();
            this.auditTrailModule = new DevExpress.ExpressApp.AuditTrail.AuditTrailModule();
            this.objectsModule = new DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule();
            this.cloneObjectModule = new DevExpress.ExpressApp.CloneObject.CloneObjectModule();
            this.conditionalAppearanceModule = new DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule();
            this.dashboardsModule = new DevExpress.ExpressApp.Dashboards.DashboardsModule();
            this.dashboardsWindowsFormsModule = new DevExpress.ExpressApp.Dashboards.Win.DashboardsWindowsFormsModule();
            this.notificationsModule = new DevExpress.ExpressApp.Notifications.NotificationsModule();
            this.notificationsWindowsFormsModule = new DevExpress.ExpressApp.Notifications.Win.NotificationsWindowsFormsModule();
            this.reportsModuleV2 = new DevExpress.ExpressApp.ReportsV2.ReportsModuleV2();
            this.reportsWindowsFormsModuleV2 = new DevExpress.ExpressApp.ReportsV2.Win.ReportsWindowsFormsModuleV2();
            this.schedulerModuleBase = new DevExpress.ExpressApp.Scheduler.SchedulerModuleBase();
            this.schedulerWindowsFormsModule = new DevExpress.ExpressApp.Scheduler.Win.SchedulerWindowsFormsModule();
            this.validationModule = new DevExpress.ExpressApp.Validation.ValidationModule();
            this.validationWindowsFormsModule = new DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule();
            this.workflowModule = new DevExpress.ExpressApp.Workflow.WorkflowModule();
            this.workflowWindowsFormsModule = new DevExpress.ExpressApp.Workflow.Win.WorkflowWindowsFormsModule();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // securityStrategyComplex1
            // 
            this.securityStrategyComplex1.Authentication = this.authenticationStandard1;
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
            // dashboardsModule
            // 
            this.dashboardsModule.DashboardDataType = typeof(DevExpress.Persistent.BaseImpl.DashboardData);
            // 
            // dashboardsWindowsFormsModule
            // 
            this.dashboardsWindowsFormsModule.DesignerFormStyle = DevExpress.XtraBars.Ribbon.RibbonFormStyle.Ribbon;
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
            // validationModule
            // 
            this.validationModule.AllowValidationDetailsAccess = true;
            this.validationModule.IgnoreWarningAndInformationRules = false;
            // 
            // workflowModule
            // 
            this.workflowModule.RunningWorkflowInstanceInfoType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoRunningWorkflowInstanceInfo);
            this.workflowModule.StartWorkflowRequestType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoStartWorkflowRequest);
            this.workflowModule.UserActivityVersionType = typeof(DevExpress.ExpressApp.Workflow.Versioning.XpoUserActivityVersion);
            this.workflowModule.WorkflowControlCommandRequestType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoWorkflowInstanceControlCommandRequest);
            this.workflowModule.WorkflowDefinitionType = typeof(DevExpress.ExpressApp.Workflow.Xpo.XpoWorkflowDefinition);
            this.workflowModule.WorkflowInstanceKeyType = typeof(DevExpress.Workflow.Xpo.XpoInstanceKey);
            this.workflowModule.WorkflowInstanceType = typeof(DevExpress.Workflow.Xpo.XpoWorkflowInstance);
            // 
            // PDNOriginacionWindowsFormsApplication
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
            this.Modules.Add(this.workflowModule);
            this.Modules.Add(this.module3);
            this.Modules.Add(this.dashboardsWindowsFormsModule);
            this.Modules.Add(this.notificationsWindowsFormsModule);
            this.Modules.Add(this.reportsWindowsFormsModuleV2);
            this.Modules.Add(this.schedulerWindowsFormsModule);
            this.Modules.Add(this.validationWindowsFormsModule);
            this.Modules.Add(this.workflowWindowsFormsModule);
            this.Modules.Add(this.module4);
            this.Modules.Add(this.securityModule1);
            this.Security = this.securityStrategyComplex1;
            this.UseOldTemplates = false;
            this.DatabaseVersionMismatch += new System.EventHandler<DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs>(this.PDNOriginacionWindowsFormsApplication_DatabaseVersionMismatch);
            this.CustomizeLanguagesList += new System.EventHandler<DevExpress.ExpressApp.CustomizeLanguagesListEventArgs>(this.PDNOriginacionWindowsFormsApplication_CustomizeLanguagesList);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }

        #endregion

        private DevExpress.ExpressApp.SystemModule.SystemModule module1;
        private DevExpress.ExpressApp.Win.SystemModule.SystemWindowsFormsModule module2;
        private PDNOriginacion.Module.PDNOriginacionModule module3;
        private PDNOriginacion.Module.Win.PDNOriginacionWindowsFormsModule module4;
        private DevExpress.ExpressApp.Security.SecurityModule securityModule1;
        private DevExpress.ExpressApp.Security.SecurityStrategyComplex securityStrategyComplex1;
        private DevExpress.ExpressApp.Security.AuthenticationStandard authenticationStandard1;
        private DevExpress.ExpressApp.AuditTrail.AuditTrailModule auditTrailModule;
        private DevExpress.ExpressApp.Objects.BusinessClassLibraryCustomizationModule objectsModule;
        private DevExpress.ExpressApp.CloneObject.CloneObjectModule cloneObjectModule;
        private DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule conditionalAppearanceModule;
        private DevExpress.ExpressApp.Dashboards.DashboardsModule dashboardsModule;
        private DevExpress.ExpressApp.Dashboards.Win.DashboardsWindowsFormsModule dashboardsWindowsFormsModule;
        private DevExpress.ExpressApp.Notifications.NotificationsModule notificationsModule;
        private DevExpress.ExpressApp.Notifications.Win.NotificationsWindowsFormsModule notificationsWindowsFormsModule;
        private DevExpress.ExpressApp.ReportsV2.ReportsModuleV2 reportsModuleV2;
        private DevExpress.ExpressApp.ReportsV2.Win.ReportsWindowsFormsModuleV2 reportsWindowsFormsModuleV2;
        private DevExpress.ExpressApp.Scheduler.SchedulerModuleBase schedulerModuleBase;
        private DevExpress.ExpressApp.Scheduler.Win.SchedulerWindowsFormsModule schedulerWindowsFormsModule;
        private DevExpress.ExpressApp.Validation.ValidationModule validationModule;
        private DevExpress.ExpressApp.Validation.Win.ValidationWindowsFormsModule validationWindowsFormsModule;
        private DevExpress.ExpressApp.Workflow.WorkflowModule workflowModule;
        private DevExpress.ExpressApp.Workflow.Win.WorkflowWindowsFormsModule workflowWindowsFormsModule;
    }
}
