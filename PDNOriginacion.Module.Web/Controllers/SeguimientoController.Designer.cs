namespace PDNOriginacion.Module.Web.Controllers
{
    partial class SeguimientoController
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.RegSeguimientoPersona = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.RegSeguimientoPersona2 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ContSeguimiento = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ContSeguimientoPU = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // RegSeguimientoPersona
            // 
            this.RegSeguimientoPersona.Caption = "Seguimiento";
            this.RegSeguimientoPersona.Category = "PopupActions";
            this.RegSeguimientoPersona.ConfirmationMessage = null;
            this.RegSeguimientoPersona.Id = "regSeguimientoPersona";
            this.RegSeguimientoPersona.ToolTip = null;
            this.RegSeguimientoPersona.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RegSeguimientoPersona_Execute);
            // 
            // RegSeguimientoPersona2
            // 
            this.RegSeguimientoPersona2.Caption = "Seguimiento";
            this.RegSeguimientoPersona2.ConfirmationMessage = null;
            this.RegSeguimientoPersona2.Id = "regSeguimientoPersona2";
            this.RegSeguimientoPersona2.ToolTip = null;
            this.RegSeguimientoPersona2.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RegSeguimientoPersona2_Execute);
            // 
            // ContSeguimiento
            // 
            this.ContSeguimiento.Caption = "Continuar Seguimiento";
            this.ContSeguimiento.ConfirmationMessage = null;
            this.ContSeguimiento.Id = "e715d923-3cd0-4d88-91de-a9bcca206884";
            this.ContSeguimiento.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ContSeguimiento.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Seguimiento);
            this.ContSeguimiento.ToolTip = null;
            this.ContSeguimiento.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleAction1_Execute);
            // 
            // ContSeguimientoPU
            // 
            this.ContSeguimientoPU.Caption = "Continuar Seguimiento";
            this.ContSeguimientoPU.Category = "PopupActions";
            this.ContSeguimientoPU.ConfirmationMessage = null;
            this.ContSeguimientoPU.Id = "e715d923-3cd0-4d88-91de-a9bcca206888";
            this.ContSeguimientoPU.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ContSeguimientoPU.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Seguimiento);
            this.ContSeguimientoPU.ToolTip = null;
            this.ContSeguimientoPU.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleAction1_Execute);
            // 
            // SeguimientoController
            // 
            this.Actions.Add(this.RegSeguimientoPersona);
            this.Actions.Add(this.RegSeguimientoPersona2);
            this.Actions.Add(this.ContSeguimiento);
            this.Actions.Add(this.ContSeguimientoPU);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Seguimiento);

        }

        #endregion
        private DevExpress.ExpressApp.Actions.SimpleAction RegSeguimientoPersona;
        private DevExpress.ExpressApp.Actions.SimpleAction RegSeguimientoPersona2;
        private DevExpress.ExpressApp.Actions.SimpleAction ContSeguimiento;
        private DevExpress.ExpressApp.Actions.SimpleAction ContSeguimientoPU;
    }
}
