namespace PDNOriginacion.Module.Web.Controllers
{
    partial class PersonaGaranteController
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
            this.GenerarSolicitudGarante = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AgregarConyuge = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // GenerarSolicitudGarante
            // 
            this.GenerarSolicitudGarante.Caption = "Generar Solicitud Garante";
            this.GenerarSolicitudGarante.ConfirmationMessage = "¿Confirma que quiere crear una solicitud para el Garante?";
            this.GenerarSolicitudGarante.Id = "GenerarSolicitudGarante";
            this.GenerarSolicitudGarante.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.GenerarSolicitudGarante.ToolTip = null;
            this.GenerarSolicitudGarante.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.GenerarSolicitudGarante_Execute);
            // 
            // AgregarConyuge
            // 
            this.AgregarConyuge.Caption = "Agregar Conyuge";
            this.AgregarConyuge.ConfirmationMessage = "¿Confirma que quiere agregar el Conyuge a la solicitud?";
            this.AgregarConyuge.Id = "AgregarConyuge";
            this.AgregarConyuge.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.AgregarConyuge.ToolTip = null;
            this.AgregarConyuge.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AgregarConyuge_Execute);
            // 
            // PersonaGaranteController
            // 
            this.Actions.Add(this.GenerarSolicitudGarante);
            this.Actions.Add(this.AgregarConyuge);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.SolicitudPersona);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction GenerarSolicitudGarante;
        private DevExpress.ExpressApp.Actions.SimpleAction AgregarConyuge;
    }
}
