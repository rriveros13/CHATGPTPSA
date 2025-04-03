﻿namespace PDNOriginacion.Module.Web.Controllers
{
    partial class PresupuestoPrestamoController
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
            this.AceptadoCliente = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AceptadoCliente
            // 
            this.AceptadoCliente.Caption = "Aceptado Cliente";
            this.AceptadoCliente.Category = "PopupActions";
            this.AceptadoCliente.ConfirmationMessage = "¿Desea marcar el préstamo como aceptado por el cliente?";
            this.AceptadoCliente.Id = "AceptadoCliente";
            this.AceptadoCliente.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.AceptadoCliente.TargetObjectsCriteria = "Presupuesto.EsPropuesta = True";
            this.AceptadoCliente.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.PresupuestoPrestamo);
            this.AceptadoCliente.ToolTip = null;
            this.AceptadoCliente.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.AceptadoCliente_Execute);
            // 
            // PresupuestoPrestamoController
            // 
            this.Actions.Add(this.AceptadoCliente);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.PresupuestoPrestamo);
            this.Activated += new System.EventHandler(this.PresupuestoPrestamoController_Activated);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AceptadoCliente;
    }
}
