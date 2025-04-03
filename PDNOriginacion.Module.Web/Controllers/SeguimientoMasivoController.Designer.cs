namespace PDNOriginacion.Module.Web.Controllers
{
    partial class SeguimientoMasivoController
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
            this.ImportarSeguimientos = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // ImportarSeguimientos
            // 
            this.ImportarSeguimientos.AcceptButtonCaption = null;
            this.ImportarSeguimientos.CancelButtonCaption = null;
            this.ImportarSeguimientos.Caption = "Importar Seguimientos";
            this.ImportarSeguimientos.ConfirmationMessage = null;
            this.ImportarSeguimientos.Id = "ImportarSeguimientosAction";
            this.ImportarSeguimientos.TargetObjectsCriteria = "IsNull(Archivo)";
            this.ImportarSeguimientos.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.SeguimientoMasivo);
            this.ImportarSeguimientos.TargetViewId = "Any";
            this.ImportarSeguimientos.ToolTip = null;
            this.ImportarSeguimientos.TypeOfView = typeof(DevExpress.ExpressApp.View);
            this.ImportarSeguimientos.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ImportarSeguimientos_CustomizePopupWindowParams);
            this.ImportarSeguimientos.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ImportarSeguimientos_Execute);
            // 
            // SeguimientoMasivoController
            // 
            this.Actions.Add(this.ImportarSeguimientos);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.SeguimientoMasivo);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ImportarSeguimientos;
    }
}
