using System;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Actions;

namespace PDNOriginacion.Module.Web.Controllers
{
    partial class PresupuestoController
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
            this.generarCuotas = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.aprobar = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.imprimir = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // generarCuotas
            // 
            this.generarCuotas.AcceptButtonCaption = null;
            this.generarCuotas.CancelButtonCaption = null;
            this.generarCuotas.Caption = "Generar Cuotas";
            this.generarCuotas.Category = "PopupActions";
            this.generarCuotas.ConfirmationMessage = null;
            this.generarCuotas.Id = "generarCuotas";
            this.generarCuotas.TargetObjectsCriteria = "UsuarioEnRol(\'PCA_GenerarCuotas\')";
            this.generarCuotas.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Presupuesto);
            this.generarCuotas.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.generarCuotas.ToolTip = null;
            this.generarCuotas.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.generarCuotas.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.GenerarCuotas_CustomizePopupWindowParams);
            this.generarCuotas.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.generarCuotas_Execute);
            // 
            // aprobar
            // 
            this.aprobar.Caption = "Aprobar";
            this.aprobar.Category = "PopupActions";
            this.aprobar.ConfirmationMessage = "Se aprobará el presupuesto.";
            this.aprobar.Id = "aprobarPrespuesto";
            this.aprobar.TargetObjectsCriteria = "EsPropuesta = False and Aprobado = False and UsuarioEnRol(\'PCA_AprobarPresupuesto" +
    "\')";
            this.aprobar.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Presupuesto);
            this.aprobar.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.aprobar.ToolTip = null;
            this.aprobar.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);
            this.aprobar.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.aprobar_Execute);
            // 
            // imprimir
            // 
            this.imprimir.Caption = "Imprimir";
            this.imprimir.Category = "PopupActions";
            this.imprimir.ConfirmationMessage = null;
            this.imprimir.Id = "cead2feb-8fb0-4613-9319-593fcb0f6954";
            this.imprimir.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Presupuesto);
            this.imprimir.ToolTip = null;
            this.imprimir.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.imprimir_Execute);
            // 
            // PresupuestoController
            // 
            this.Actions.Add(this.generarCuotas);
            this.Actions.Add(this.aprobar);
            this.Actions.Add(this.imprimir);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Presupuesto);

        }


        private DevExpress.ExpressApp.Actions.PopupWindowShowAction generarCuotas;
        private DevExpress.ExpressApp.Actions.SimpleAction aprobar;


        #endregion

        private SimpleAction imprimir;

    }
}
