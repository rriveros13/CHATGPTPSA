namespace PDNOriginacion.Module.Web.Controllers
{
    partial class SolicitudController
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
            this.ImportarCheques = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ImportarPersonas = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.Transiciones = new DevExpress.ExpressApp.Actions.SingleChoiceAction(this.components);
            this.ProcesarSolicitante = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.DatosPersona = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ProcesarMotor2 = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.generarPropuestaComercial = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.generarPresupuesto = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.informeEscribania = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AgregarInmueble = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.VerAdjuntos = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.CrearTarea = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.GenerarSolAsociada = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.ProcesarTareas = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.IntegracionITGF = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.CalculadoraPrestamo = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ImprimirSolicitud = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // ImportarCheques
            // 
            this.ImportarCheques.AcceptButtonCaption = null;
            this.ImportarCheques.CancelButtonCaption = null;
            this.ImportarCheques.Caption = "Importar Cheques";
            this.ImportarCheques.Category = "Tools";
            this.ImportarCheques.ConfirmationMessage = null;
            this.ImportarCheques.Id = "ImportarCheques";
            this.ImportarCheques.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.ImportarCheques.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportarCheques.ToolTip = null;
            this.ImportarCheques.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportarCheques.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ImportarCheques_CustomizePopupWindowParams);
            this.ImportarCheques.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ImportarCheques_Execute);
            // 
            // ImportarPersonas
            // 
            this.ImportarPersonas.AcceptButtonCaption = null;
            this.ImportarPersonas.CancelButtonCaption = null;
            this.ImportarPersonas.Caption = "Importar Personas";
            this.ImportarPersonas.Category = "Tools";
            this.ImportarPersonas.ConfirmationMessage = null;
            this.ImportarPersonas.Id = "ImportarPersonas";
            this.ImportarPersonas.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.ImportarPersonas.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImportarPersonas.ToolTip = null;
            this.ImportarPersonas.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImportarPersonas.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ImportarPersonas_CustomizePopupWindowParams);
            this.ImportarPersonas.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ImportarPersonas_Execute);
            // 
            // Transiciones
            // 
            this.Transiciones.Caption = "Cambios";
            this.Transiciones.ConfirmationMessage = null;
            this.Transiciones.Id = "Transiciones";
            this.Transiciones.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.Transiciones.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.Transiciones.ToolTip = null;
            this.Transiciones.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.Transiciones.Execute += new DevExpress.ExpressApp.Actions.SingleChoiceActionExecuteEventHandler(this.Transiciones_Execute);
            // 
            // ProcesarSolicitante
            // 
            this.ProcesarSolicitante.Caption = "Promedio Atraso";
            this.ProcesarSolicitante.ConfirmationMessage = "Se evalurán todas las personas marcadas con \"Procesar Motor\". ¿Desea continuar?";
            this.ProcesarSolicitante.Id = "ProcesarSolicitante";
            this.ProcesarSolicitante.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ProcesarSolicitante.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.ProcesarSolicitante.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ProcesarSolicitante.ToolTip = null;
            this.ProcesarSolicitante.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ProcesarSolicitante.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProcesarSolicitante_Execute);
            // 
            // DatosPersona
            // 
            this.DatosPersona.Caption = "Datos Persona Solicitud";
            this.DatosPersona.ConfirmationMessage = null;
            this.DatosPersona.Id = "DatosPersonaSolicitud";
            this.DatosPersona.TargetObjectsCriteria = "";
            this.DatosPersona.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.DatosPersona.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.DatosPersona.ToolTip = null;
            this.DatosPersona.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.DatosPersona.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.DatosPersona_Execute);
            // 
            // ProcesarMotor2
            // 
            this.ProcesarMotor2.Caption = "ProcesarMotor";
            this.ProcesarMotor2.ConfirmationMessage = "Se evalurán todas las personas marcadas con \"Procesar Motor\". ¿Desea continuar?";
            this.ProcesarMotor2.Id = "ProcesarMotor2";
            this.ProcesarMotor2.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.ProcesarMotor2.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ProcesarMotor2.ToolTip = null;
            this.ProcesarMotor2.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ProcesarMotor2.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProcesarMotor2_Execute);
            // 
            // generarPropuestaComercial
            // 
            this.generarPropuestaComercial.Caption = "Crear Propuesta Comercial";
            this.generarPropuestaComercial.ConfirmationMessage = "Se generará una Propuesta Comercial en base a la última tasación del inmueble.";
            this.generarPropuestaComercial.Id = "generarPropuestaComercial";
            this.generarPropuestaComercial.TargetObjectsCriteria = "";
            this.generarPropuestaComercial.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.generarPropuestaComercial.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.generarPropuestaComercial.ToolTip = null;
            this.generarPropuestaComercial.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.generarPropuestaComercial.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.generarPropuestaComercial_Execute);
            // 
            // generarPresupuesto
            // 
            this.generarPresupuesto.Caption = "Generar Presupuesto";
            this.generarPresupuesto.ConfirmationMessage = "Se generará un presupuesto.";
            this.generarPresupuesto.Id = "generarPresupuesto";
            this.generarPresupuesto.TargetObjectsCriteria = "";
            this.generarPresupuesto.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.generarPresupuesto.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.generarPresupuesto.ToolTip = null;
            this.generarPresupuesto.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.generarPresupuesto.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.generarPresupuesto_Execute);
            // 
            // informeEscribania
            // 
            this.informeEscribania.Caption = "Informe Escribanía";
            this.informeEscribania.ConfirmationMessage = null;
            this.informeEscribania.Id = "a0a6b6e6-4495-4a32-89e9-2b240ec64524";
            this.informeEscribania.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.informeEscribania.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.informeEscribania.ToolTip = null;
            this.informeEscribania.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.informeEscribania.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.informeEscribania_Execute);
            // 
            // AgregarInmueble
            // 
            this.AgregarInmueble.AcceptButtonCaption = null;
            this.AgregarInmueble.CancelButtonCaption = null;
            this.AgregarInmueble.Caption = "Agregar Inmueble";
            this.AgregarInmueble.ConfirmationMessage = null;
            this.AgregarInmueble.Id = "agregar_inmueble";
            this.AgregarInmueble.TargetObjectsCriteria = "UsuarioEnRol(\'PCA_AgregarInmueble\')";
            this.AgregarInmueble.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.AgregarInmueble.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.AgregarInmueble.ToolTip = null;
            this.AgregarInmueble.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.AgregarInmueble.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.AgregarInmuebles_CustomizePopupWindowParams);
            this.AgregarInmueble.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.AgregarInmuebles_Execute);
            // 
            // VerAdjuntos
            // 
            this.VerAdjuntos.Caption = "Ver Adjuntos Solicitud";
            this.VerAdjuntos.ConfirmationMessage = null;
            this.VerAdjuntos.Id = "VerAdjuntosSolicitud";
            this.VerAdjuntos.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.VerAdjuntos.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.VerAdjuntos.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.VerAdjuntos.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.VerAdjuntos.ToolTip = null;
            this.VerAdjuntos.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            // 
            // CrearTarea
            // 
            this.CrearTarea.AcceptButtonCaption = null;
            this.CrearTarea.CancelButtonCaption = null;
            this.CrearTarea.Caption = "Crear Tarea";
            this.CrearTarea.ConfirmationMessage = null;
            this.CrearTarea.Id = "81381d2e-d4d2-4896-990b-b10f5c8ab46b";
            this.CrearTarea.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.CrearTarea.TargetObjectsCriteria = "UsuarioEnRol(\'PCA_CrearTarea\')";
            this.CrearTarea.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.CrearTarea.ToolTip = null;
            this.CrearTarea.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CrearTarea_CustomizePopupWindowParams);
            this.CrearTarea.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CrearTarea_Execute);
            // 
            // GenerarSolAsociada
            // 
            this.GenerarSolAsociada.Caption = "Generar Microcrédito";
            this.GenerarSolAsociada.ConfirmationMessage = "¿Confirma que desea generar una nueva solicitud?";
            this.GenerarSolAsociada.Id = "c4bcbb3f-aa3e-4c26-8d31-fdeb7dff7ace";
            this.GenerarSolAsociada.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.GenerarSolAsociada.ToolTip = null;
            this.GenerarSolAsociada.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.GenerarSolAsociada_Execute);
            // 
            // ProcesarTareas
            // 
            this.ProcesarTareas.Caption = "Procesar Tareas";
            this.ProcesarTareas.ConfirmationMessage = null;
            this.ProcesarTareas.Id = "ProcesarTareas";
            this.ProcesarTareas.ImageName = "BO_Task";
            this.ProcesarTareas.PaintStyle = DevExpress.ExpressApp.Templates.ActionItemPaintStyle.Image;
            this.ProcesarTareas.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.ProcesarTareas.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ProcesarTareas.ToolTip = null;
            this.ProcesarTareas.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ProcesarTareas.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ProcesarTareas_Execute);
            // 
            // IntegracionITGF
            // 
            this.IntegracionITGF.AcceptButtonCaption = null;
            this.IntegracionITGF.CancelButtonCaption = null;
            this.IntegracionITGF.Caption = "Enviar al Back";
            this.IntegracionITGF.ConfirmationMessage = null;
            this.IntegracionITGF.Id = "IntegracionITGF";
            this.IntegracionITGF.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.IntegracionITGF.TargetObjectsCriteria = "!EnviadoBack";
            this.IntegracionITGF.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.IntegracionITGF.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.IntegracionITGF.ToolTip = null;
            this.IntegracionITGF.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.IntegracionITGF.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.IntegracionITGF_CustomizePopupWindowParams);
            this.IntegracionITGF.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.IntegracionITGF_Execute);
            // 
            // CalculadoraPrestamo
            // 
            this.CalculadoraPrestamo.AcceptButtonCaption = null;
            this.CalculadoraPrestamo.CancelButtonCaption = null;
            this.CalculadoraPrestamo.Caption = "Calculadora";
            this.CalculadoraPrestamo.ConfirmationMessage = null;
            this.CalculadoraPrestamo.Id = "CalculadoraPrestamo";
            this.CalculadoraPrestamo.TargetObjectsCriteria = "UsuarioEnRol(\'PCA_CalculadoraPrestamo\')";
            this.CalculadoraPrestamo.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.CalculadoraPrestamo.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.CalculadoraPrestamo.ToolTip = null;
            this.CalculadoraPrestamo.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.CalculadoraPrestamo.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.CalculadoraPrestamo_CustomizePopupWindowParams);
            this.CalculadoraPrestamo.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.CalculadoraPrestamo_Execute);
            // 
            // ImprimirSolicitud
            // 
            this.ImprimirSolicitud.Caption = "Imprimir Solicitud";
            this.ImprimirSolicitud.ConfirmationMessage = null;
            this.ImprimirSolicitud.Id = "ImprimirSolicitud";
            this.ImprimirSolicitud.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.ImprimirSolicitud.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.ImprimirSolicitud.ToolTip = null;
            this.ImprimirSolicitud.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.ImprimirSolicitud.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ImprimirSolicitud_Execute);
            // 
            // SolicitudController
            // 
            this.Actions.Add(this.ImportarCheques);
            this.Actions.Add(this.ImportarPersonas);
            this.Actions.Add(this.Transiciones);
            this.Actions.Add(this.ProcesarSolicitante);
            this.Actions.Add(this.DatosPersona);
            this.Actions.Add(this.ProcesarMotor2);
            this.Actions.Add(this.generarPropuestaComercial);
            this.Actions.Add(this.generarPresupuesto);
            this.Actions.Add(this.informeEscribania);
            this.Actions.Add(this.AgregarInmueble);
            this.Actions.Add(this.VerAdjuntos);
            this.Actions.Add(this.CrearTarea);
            this.Actions.Add(this.GenerarSolAsociada);
            this.Actions.Add(this.ProcesarTareas);
            this.Actions.Add(this.IntegracionITGF);
            this.Actions.Add(this.CalculadoraPrestamo);
            this.Actions.Add(this.ImprimirSolicitud);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Solicitud);
            this.Activated += new System.EventHandler(this.SolicitudController_Activated);

        }

        #endregion


        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction AprobarSolicitudAction;
        //private DevExpress.ExpressApp.Actions.PopupWindowShowAction RechazarSolicitudAction;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ImportarCheques;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ImportarPersonas;
        private DevExpress.ExpressApp.Actions.SimpleAction ProcesarSolicitante;
        private DevExpress.ExpressApp.Actions.SingleChoiceAction Transiciones;
        private DevExpress.ExpressApp.Actions.SimpleAction DatosPersona;
        private DevExpress.ExpressApp.Actions.SimpleAction ProcesarMotor2;
        private DevExpress.ExpressApp.Actions.SimpleAction generarPropuestaComercial;
        private DevExpress.ExpressApp.Actions.SimpleAction generarPresupuesto;
        private DevExpress.ExpressApp.Actions.SimpleAction informeEscribania;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction AgregarInmueble;
        private DevExpress.ExpressApp.Actions.SimpleAction VerAdjuntos;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CrearTarea;
        private DevExpress.ExpressApp.Actions.SimpleAction GenerarSolAsociada;
        private DevExpress.ExpressApp.Actions.SimpleAction ProcesarTareas;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction IntegracionITGF;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction CalculadoraPrestamo;
        private DevExpress.ExpressApp.Actions.SimpleAction ImprimirSolicitud;
    }
}
