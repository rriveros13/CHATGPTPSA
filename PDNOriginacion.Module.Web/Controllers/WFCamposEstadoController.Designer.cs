namespace PDNOriginacion.Module.Web.Controllers
{
    partial class WFCamposEstadoController
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
            this.Campo_AgregarAProducto = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.popupWindowShowAction1 = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.popupWindowShowAction2 = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // Campo_AgregarAProducto
            // 
            this.Campo_AgregarAProducto.Caption = "Agregar a Producto";
            this.Campo_AgregarAProducto.ConfirmationMessage = null;
            this.Campo_AgregarAProducto.Id = "Campo_AgregarAProducto";
            this.Campo_AgregarAProducto.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Campo);
            this.Campo_AgregarAProducto.ToolTip = null;
            this.Campo_AgregarAProducto.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.simpleAction1_Execute);
            // 
            // popupWindowShowAction1
            // 
            this.popupWindowShowAction1.AcceptButtonCaption = null;
            this.popupWindowShowAction1.CancelButtonCaption = null;
            this.popupWindowShowAction1.Caption = "Extender a otros Estados";
            this.popupWindowShowAction1.Category = "PopupActions";
            this.popupWindowShowAction1.ConfirmationMessage = null;
            this.popupWindowShowAction1.Id = "popupWindowShowAction1";
            this.popupWindowShowAction1.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.popupWindowShowAction1.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.CampoProductoExcep);
            this.popupWindowShowAction1.ToolTip = null;
            this.popupWindowShowAction1.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupWindowShowAction1_CustomizePopupWindowParams);
            this.popupWindowShowAction1.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupWindowShowAction1_Execute);
            // 
            // popupWindowShowAction2
            // 
            this.popupWindowShowAction2.AcceptButtonCaption = null;
            this.popupWindowShowAction2.CancelButtonCaption = null;
            this.popupWindowShowAction2.Caption = "Extender a otras Tareas";
            this.popupWindowShowAction2.Category = "PopupActions";
            this.popupWindowShowAction2.ConfirmationMessage = null;
            this.popupWindowShowAction2.Id = "popupWindowShowAction2";
            this.popupWindowShowAction2.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.popupWindowShowAction2.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.CampoProductoExcep);
            this.popupWindowShowAction2.ToolTip = null;
            this.popupWindowShowAction2.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.popupWindowShowAction2_CustomizePopupWindowParams);
            this.popupWindowShowAction2.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.popupWindowShowAction2_Execute);
            // 
            // WFCamposEstadoController
            // 
            this.Actions.Add(this.Campo_AgregarAProducto);
            this.Actions.Add(this.popupWindowShowAction1);
            this.Actions.Add(this.popupWindowShowAction2);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction Campo_AgregarAProducto;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupWindowShowAction1;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction popupWindowShowAction2;
    }
}
