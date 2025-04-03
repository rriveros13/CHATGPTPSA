namespace PDNOriginacion.Module.Web.Controllers
{
    partial class PersonaController
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
            this.DatosPersona = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DatosPersonaPU = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.GenerarSolicitud = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ImportarPersonas = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.VerAdjuntos = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // DatosPersona
            // 
            this.DatosPersona.Caption = "Datos Persona Persona";
            this.DatosPersona.ConfirmationMessage = null;
            this.DatosPersona.Id = "DatosPersonaPersona";
            this.DatosPersona.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Persona);
            this.DatosPersona.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.DatosPersona.ToolTip = null;
            this.DatosPersona.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.DatosPersona.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DatosPersona_Execute);
            // 
            // DatosPersonaPU
            // 
            this.DatosPersonaPU.Caption = "Datos Persona";
            this.DatosPersonaPU.Category = "PopupActions";
            this.DatosPersonaPU.ConfirmationMessage = null;
            this.DatosPersonaPU.Id = "DatosPersonaPersonaPU";
            this.DatosPersonaPU.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Persona);
            this.DatosPersonaPU.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.DatosPersonaPU.ToolTip = null;
            this.DatosPersonaPU.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.DatosPersonaPU.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DatosPersona_Execute);
            // 
            // GenerarSolicitud
            // 
            this.GenerarSolicitud.Caption = "Generar Solicitud";
            this.GenerarSolicitud.ConfirmationMessage = null;
            this.GenerarSolicitud.Id = "GenerarSolicitud";
            this.GenerarSolicitud.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.GenerarSolicitud.TargetObjectsCriteria = "UsuarioEnRol(\'PCA_CrearSolicitud\')";
            this.GenerarSolicitud.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Persona);
            this.GenerarSolicitud.ToolTip = null;
            this.GenerarSolicitud.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.GenerarSolicitud.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.GenerarSolicitud_Execute);
            // 
            // ImportarPersonas
            // 
            this.ImportarPersonas.AcceptButtonCaption = null;
            this.ImportarPersonas.CancelButtonCaption = null;
            this.ImportarPersonas.Caption = "Importar Personas Action";
            this.ImportarPersonas.ConfirmationMessage = null;
            this.ImportarPersonas.Id = "ImportarPersonasAction";
            this.ImportarPersonas.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Persona);
            this.ImportarPersonas.TargetViewId = "Personas_ListView";
            this.ImportarPersonas.TargetViewType = DevExpress.ExpressApp.ViewType.ListView;
            this.ImportarPersonas.ToolTip = null;
            this.ImportarPersonas.TypeOfView = typeof(DevExpress.ExpressApp.ListView);
            this.ImportarPersonas.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.Importar_CustomizePopupWindowParams);
            this.ImportarPersonas.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.Importar_Execute);
            // 
            // VerAdjuntos
            // 
            this.VerAdjuntos.Caption = "Ver Adjuntos Persona";
            this.VerAdjuntos.ConfirmationMessage = null;
            this.VerAdjuntos.Id = "VerAdjuntosPersona";
            this.VerAdjuntos.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.VerAdjuntos.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Persona);
            this.VerAdjuntos.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.VerAdjuntos.ToolTip = null;
            this.VerAdjuntos.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            // 
            // PersonaController
            // 
            this.Actions.Add(this.DatosPersona);
            this.Actions.Add(this.DatosPersonaPU);
            this.Actions.Add(this.GenerarSolicitud);
            this.Actions.Add(this.ImportarPersonas);
            this.Actions.Add(this.VerAdjuntos);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Persona);
            this.Activated += new System.EventHandler(this.PersonaController_Activated);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction DatosPersona;
        private DevExpress.ExpressApp.Actions.SimpleAction DatosPersonaPU;
        private DevExpress.ExpressApp.Actions.SimpleAction GenerarSolicitud;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ImportarPersonas;
        private DevExpress.ExpressApp.Actions.SimpleAction VerAdjuntos;
    }
}
