namespace PDNOriginacion.Module.Web {
    partial class PDNOriginacionAspNetModule {
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
            // 
            // PDNOriginacionAspNetModule
            // 
            this.RequiredModuleTypes.Add(typeof(PDNOriginacion.Module.PDNOriginacionModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Dashboards.Web.DashboardsAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.FileAttachments.Web.FileAttachmentsAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Maps.Web.MapsAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Notifications.Web.NotificationsAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ReportsV2.Web.ReportsAspNetModuleV2));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Scheduler.Web.SchedulerAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.Validation.Web.ValidationAspNetModule));
            this.RequiredModuleTypes.Add(typeof(DevExpress.ExpressApp.ConditionalAppearance.ConditionalAppearanceModule));
            this.RequiredModuleTypes.Add(typeof(ITTI.FileDataITTIModule));
            this.RequiredModuleTypes.Add(typeof(UserViewVariants.Web.UserViewVariantsWeb));
            this.RequiredModuleTypes.Add(typeof(UserViewVariants.UserViewVariantsModule));

        }

        #endregion
    }
}