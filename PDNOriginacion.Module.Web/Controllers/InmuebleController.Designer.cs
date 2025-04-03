namespace PDNOriginacion.Module.Web.Controllers
{
    partial class InmuebleController
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
            this.agregarTasacionPU = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.agregarTasacion = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.agregarValoracion = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AgregarValorizacionPU = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AgregarASolicitud = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // agregarTasacionPU
            // 
            this.agregarTasacionPU.Caption = "Agregar Tasación";
            this.agregarTasacionPU.Category = "PopupActions";
            this.agregarTasacionPU.ConfirmationMessage = null;
            this.agregarTasacionPU.Id = "b887d145-4763-427a-991b-7012f30c9dbf";
            this.agregarTasacionPU.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.agregarTasacionPU.TargetObjectsCriteria = "UsuarioEnRol(\'PCA_CrearTasacion\')";
            this.agregarTasacionPU.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Inmueble);
            this.agregarTasacionPU.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.agregarTasacionPU.ToolTip = null;
            this.agregarTasacionPU.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.agregarTasacionPU.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.agregarTasacionPU_Execute);
            // 
            // agregarTasacion
            // 
            this.agregarTasacion.Caption = "Agregar Tasación";
            this.agregarTasacion.ConfirmationMessage = null;
            this.agregarTasacion.Id = "b887d145-4763-427a-991b-7012f30c9dbg";
            this.agregarTasacion.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.agregarTasacion.TargetObjectsCriteria = "IsCurrentUserInRole(\'PCA_CrearTasacion\')";
            this.agregarTasacion.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Inmueble);
            this.agregarTasacion.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.agregarTasacion.ToolTip = null;
            this.agregarTasacion.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.agregarTasacion.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.agregarTasacion_Execute);
            // 
            // agregarValoracion
            // 
            this.agregarValoracion.Caption = "Agregar Valorización";
            this.agregarValoracion.ConfirmationMessage = null;
            this.agregarValoracion.Id = "b887d145-4763-427a-991b-7012f30c9dbh";
            this.agregarValoracion.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.agregarValoracion.TargetObjectsCriteria = "IsCurrentUserInRole(\'PCA_CrearValorizacion\')";
            this.agregarValoracion.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Inmueble);
            this.agregarValoracion.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.agregarValoracion.ToolTip = null;
            this.agregarValoracion.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.agregarValoracion.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.agregarValoracion_Execute);
            // 
            // AgregarValorizacionPU
            // 
            this.AgregarValorizacionPU.Caption = "Agregar Valorizacion";
            this.AgregarValorizacionPU.Category = "PopupActions";
            this.AgregarValorizacionPU.ConfirmationMessage = null;
            this.AgregarValorizacionPU.Id = "b887d145-4763-427a-991b-7012f30c9dbk";
            this.AgregarValorizacionPU.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.AgregarValorizacionPU.TargetObjectsCriteria = "UsuarioEnRol(\'PCA_CrearValorizacion\')";
            this.AgregarValorizacionPU.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Inmueble);
            this.AgregarValorizacionPU.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AgregarValorizacionPU.ToolTip = null;
            this.AgregarValorizacionPU.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AgregarValorizacionPU.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AgregarValorizacionPU_Execute);
            // 
            // AgregarASolicitud
            // 
            this.AgregarASolicitud.Caption = "Agregar a Solicitud";
            this.AgregarASolicitud.ConfirmationMessage = null;
            this.AgregarASolicitud.Id = "agregar_a_solicitud";
            this.AgregarASolicitud.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Inmueble);
            this.AgregarASolicitud.ToolTip = null;
            this.AgregarASolicitud.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AgregarASolicitud_Execute);
            // 
            // InmuebleController
            // 
            this.Actions.Add(this.agregarTasacionPU);
            this.Actions.Add(this.agregarTasacion);
            this.Actions.Add(this.agregarValoracion);
            this.Actions.Add(this.AgregarValorizacionPU);
            this.Actions.Add(this.AgregarASolicitud);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Inmueble);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction agregarTasacionPU;
        private DevExpress.ExpressApp.Actions.SimpleAction agregarTasacion;
        private DevExpress.ExpressApp.Actions.SimpleAction agregarValoracion;
        private DevExpress.ExpressApp.Actions.SimpleAction AgregarValorizacionPU;
        private DevExpress.ExpressApp.Actions.SimpleAction AgregarASolicitud;
    }
}
