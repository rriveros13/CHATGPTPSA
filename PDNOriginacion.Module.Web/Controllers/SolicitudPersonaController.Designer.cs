namespace PDNOriginacion.Module.Web.Controllers
{
    partial class SolicitudPersonaController
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
            this.TraerEmpleos = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TraerRefPersonales = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // TraerEmpleos
            // 
            this.TraerEmpleos.Caption = "Traer Empleos";
            this.TraerEmpleos.Category = "PopupActions";
            this.TraerEmpleos.ConfirmationMessage = "¿Desea copiar los empleos del cliente como ingresos?";
            this.TraerEmpleos.Id = "62dfa57e-c14c-49c3-a208-e2ebea6a6e07";
            this.TraerEmpleos.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.SolicitudPersona);
            this.TraerEmpleos.ToolTip = null;
            this.TraerEmpleos.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TraerEmpleos_Execute);
            // 
            // TraerRefPersonales
            // 
            this.TraerRefPersonales.Caption = "Traer Ref Personales";
            this.TraerRefPersonales.Category = "PopupActions";
            this.TraerRefPersonales.ConfirmationMessage = "¿Desea copiar las referencias personales del cliente?";
            this.TraerRefPersonales.Id = "TraerRefPersonales";
            this.TraerRefPersonales.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.SolicitudPersona);
            this.TraerRefPersonales.ToolTip = null;
            this.TraerRefPersonales.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TraerRefPersonales_Execute);
            // 
            // SolicitudPersonaController
            // 
            this.Actions.Add(this.TraerEmpleos);
            this.Actions.Add(this.TraerRefPersonales);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.SolicitudPersona);
            this.Activated += new System.EventHandler(this.SolicitudPersonaController_Activated);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction TraerEmpleos;
        private DevExpress.ExpressApp.Actions.SimpleAction TraerRefPersonales;
    }
}
