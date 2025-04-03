namespace PDNOriginacion.Module.Controllers
{
    partial class AccionesController
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
            this.CargaRapidaInmueble = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CargaRapidaDireccion = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CargaRapidaTelefono = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CargaRapidaIngresos = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CrearTarea = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // CargaRapidaInmueble
            // 
            this.CargaRapidaInmueble.Caption = "Guardar Inmueble";
            this.CargaRapidaInmueble.Category = "CargaRapidaInmueble";
            this.CargaRapidaInmueble.ConfirmationMessage = null;
            this.CargaRapidaInmueble.Id = "b89972f0-6fef-4d73-a231-69c5776d7430";
            this.CargaRapidaInmueble.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Persona);
            this.CargaRapidaInmueble.ToolTip = null;
            this.CargaRapidaInmueble.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CargaRapidaInmueble_Execute);
            // 
            // CargaRapidaDireccion
            // 
            this.CargaRapidaDireccion.Caption = "Guardar Dirección";
            this.CargaRapidaDireccion.Category = "CargaRapidaDireccion";
            this.CargaRapidaDireccion.ConfirmationMessage = null;
            this.CargaRapidaDireccion.Id = "CargaRapidaDireccion";
            this.CargaRapidaDireccion.QuickAccess = true;
            this.CargaRapidaDireccion.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Persona);
            this.CargaRapidaDireccion.ToolTip = null;
            this.CargaRapidaDireccion.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CargaRapidaDireccion_Execute);
            // 
            // CargaRapidaTelefono
            // 
            this.CargaRapidaTelefono.Caption = "Guardar Teléfono";
            this.CargaRapidaTelefono.Category = "CargaRapidaTelefono";
            this.CargaRapidaTelefono.ConfirmationMessage = null;
            this.CargaRapidaTelefono.Id = "CargaRapidaTelefono";
            this.CargaRapidaTelefono.QuickAccess = true;
            this.CargaRapidaTelefono.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Persona);
            this.CargaRapidaTelefono.ToolTip = null;
            this.CargaRapidaTelefono.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CargaRapidaTelefono_Execute);
            // 
            // CargaRapidaIngresos
            // 
            this.CargaRapidaIngresos.Caption = "Guardar";
            this.CargaRapidaIngresos.Category = "CargaRapidaIngreso";
            this.CargaRapidaIngresos.ConfirmationMessage = null;
            this.CargaRapidaIngresos.Id = "CargaRapidaIngresos";
            this.CargaRapidaIngresos.QuickAccess = true;
            this.CargaRapidaIngresos.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.SolicitudPersona);
            this.CargaRapidaIngresos.ToolTip = null;
            this.CargaRapidaIngresos.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CargaRapidaIngresos_Execute);
            // 
            // CrearTarea
            // 
            this.CrearTarea.Caption = "Crear Tarea";
            this.CrearTarea.Category = "CrearTarea";
            this.CrearTarea.ConfirmationMessage = null;
            this.CrearTarea.Id = "CrearTarea";
            this.CrearTarea.TargetObjectType = typeof(PDNOriginacion.Module.Web.Controllers.NonPersistentClasses.CrearTarea);
            this.CrearTarea.ToolTip = null;
            this.CrearTarea.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.CrearTarea_Execute);
            // 
            // AccionesController
            // 
            this.Actions.Add(this.CargaRapidaInmueble);
            this.Actions.Add(this.CargaRapidaDireccion);
            this.Actions.Add(this.CargaRapidaTelefono);
            this.Actions.Add(this.CargaRapidaIngresos);
            this.Actions.Add(this.CrearTarea);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction CargaRapidaInmueble;
        private DevExpress.ExpressApp.Actions.SimpleAction CargaRapidaDireccion;
        private DevExpress.ExpressApp.Actions.SimpleAction CargaRapidaTelefono;
        private DevExpress.ExpressApp.Actions.SimpleAction CargaRapidaIngresos;
        private DevExpress.ExpressApp.Actions.SimpleAction CrearTarea;
    }
}
