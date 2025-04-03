namespace PDNOriginacion.Module.Web.Controllers
{
    partial class UsuarioControlelr
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
            this.ReasignarTareas = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            // 
            // ReasignarTareas
            // 
            this.ReasignarTareas.AcceptButtonCaption = null;
            this.ReasignarTareas.CancelButtonCaption = null;
            this.ReasignarTareas.Caption = "Reasignar Tareas";
            this.ReasignarTareas.ConfirmationMessage = "¿Está seguro de que desea reasignar las tareas del usuario?";
            this.ReasignarTareas.Id = "2e9df1c1-b60f-4e96-baa4-e615497e64da";
            this.ReasignarTareas.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.ReasignarTareas.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Usuario);
            this.ReasignarTareas.ToolTip = null;
            this.ReasignarTareas.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.ReasignarTareas_CustomizePopupWindowParams);
            this.ReasignarTareas.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ReasignarTareas_Execute);
            // 
            // UsuarioControlelr
            // 
            this.Actions.Add(this.ReasignarTareas);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Usuario);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ReasignarTareas;
    }
}
