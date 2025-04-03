namespace PDNOriginacion.Module.Web.Controllers
{
    partial class TareasController
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
            this.AsignarTarea = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.ReasignarTarea = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.LiberarTarea = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FinalizarTarea = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TomarTarea = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.TomarTareaDV = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AsignarTareaDV = new DevExpress.ExpressApp.Actions.PopupWindowShowAction(this.components);
            this.LiberarTareaDV = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.FinalizarTareaDV = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AsignarTarea
            // 
            this.AsignarTarea.AcceptButtonCaption = null;
            this.AsignarTarea.CancelButtonCaption = null;
            this.AsignarTarea.Caption = "Asignar";
            this.AsignarTarea.ConfirmationMessage = null;
            this.AsignarTarea.Id = "AsignarTarea";
            this.AsignarTarea.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.AsignarTarea.ToolTip = null;
            this.AsignarTarea.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.AsignarTarea_CustomizePopupWindowParams);
            this.AsignarTarea.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.AsignarTarea_Execute);
            // 
            // ReasignarTarea
            // 
            this.ReasignarTarea.AcceptButtonCaption = null;
            this.ReasignarTarea.CancelButtonCaption = null;
            this.ReasignarTarea.Caption = "Reasignar Tarea";
            this.ReasignarTarea.ConfirmationMessage = null;
            this.ReasignarTarea.Id = "ReasignarTarea";
            this.ReasignarTarea.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.ReasignarTarea.ToolTip = null;
            this.ReasignarTarea.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.ReasignarTarea_Execute);
            // 
            // LiberarTarea
            // 
            this.LiberarTarea.Caption = "Liberar";
            this.LiberarTarea.ConfirmationMessage = "¿Confirma la liberación de la tarea?";
            this.LiberarTarea.Id = "LiberarTarea";
            this.LiberarTarea.ToolTip = null;
            this.LiberarTarea.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LiberarTarea_Execute);
            // 
            // FinalizarTarea
            // 
            this.FinalizarTarea.Caption = "Finalizar";
            this.FinalizarTarea.ConfirmationMessage = "¿Confirma la finalización de la tarea?";
            this.FinalizarTarea.Id = "FinalizarTarea";
            this.FinalizarTarea.ToolTip = null;
            this.FinalizarTarea.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FinalizarTarea_Execute);
            // 
            // TomarTarea
            // 
            this.TomarTarea.Caption = "Tomar";
            this.TomarTarea.ConfirmationMessage = "¿Confirma que quiere tomar la tarea seleccionada?";
            this.TomarTarea.Id = "TomarTarea";
            this.TomarTarea.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireMultipleObjects;
            this.TomarTarea.ToolTip = null;
            this.TomarTarea.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TomarTarea_Execute);
            // 
            // TomarTareaDV
            // 
            this.TomarTareaDV.Caption = "Tomar";
            this.TomarTareaDV.Category = "PopupActions";
            this.TomarTareaDV.ConfirmationMessage = null;
            this.TomarTareaDV.Id = "TomarTareaDV";
            this.TomarTareaDV.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.TomarTareaDV.ToolTip = null;
            this.TomarTareaDV.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.TomarTarea_Execute);
            // 
            // AsignarTareaDV
            // 
            this.AsignarTareaDV.AcceptButtonCaption = null;
            this.AsignarTareaDV.CancelButtonCaption = null;
            this.AsignarTareaDV.Caption = "Asignar";
            this.AsignarTareaDV.Category = "PopupActions";
            this.AsignarTareaDV.ConfirmationMessage = null;
            this.AsignarTareaDV.Id = "AsignarTareaDV";
            this.AsignarTareaDV.SelectionDependencyType = DevExpress.ExpressApp.Actions.SelectionDependencyType.RequireSingleObject;
            this.AsignarTareaDV.ToolTip = null;
            this.AsignarTareaDV.CustomizePopupWindowParams += new DevExpress.ExpressApp.Actions.CustomizePopupWindowParamsEventHandler(this.AsignarTarea_CustomizePopupWindowParams);
            this.AsignarTareaDV.Execute += new DevExpress.ExpressApp.Actions.PopupWindowShowActionExecuteEventHandler(this.AsignarTarea_Execute);
            // 
            // LiberarTareaDV
            // 
            this.LiberarTareaDV.Caption = "Liberar";
            this.LiberarTareaDV.Category = "PopupActions";
            this.LiberarTareaDV.ConfirmationMessage = null;
            this.LiberarTareaDV.Id = "LiberarTareaDV";
            this.LiberarTareaDV.ToolTip = null;
            this.LiberarTareaDV.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.LiberarTarea_Execute);
            // 
            // FinalizarTareaDV
            // 
            this.FinalizarTareaDV.Caption = "Finalizar";
            this.FinalizarTareaDV.Category = "PopupActions";
            this.FinalizarTareaDV.ConfirmationMessage = null;
            this.FinalizarTareaDV.Id = "FinalizarTareaDV";
            this.FinalizarTareaDV.ToolTip = null;
            this.FinalizarTareaDV.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.FinalizarTarea_Execute);
            // 
            // TareasController
            // 
            this.Actions.Add(this.AsignarTarea);
            this.Actions.Add(this.ReasignarTarea);
            this.Actions.Add(this.LiberarTarea);
            this.Actions.Add(this.FinalizarTarea);
            this.Actions.Add(this.TomarTarea);
            this.Actions.Add(this.TomarTareaDV);
            this.Actions.Add(this.AsignarTareaDV);
            this.Actions.Add(this.LiberarTareaDV);
            this.Actions.Add(this.FinalizarTareaDV);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Tarea);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.PopupWindowShowAction AsignarTarea;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction ReasignarTarea;
        private DevExpress.ExpressApp.Actions.SimpleAction LiberarTarea;
        private DevExpress.ExpressApp.Actions.SimpleAction FinalizarTarea;
        private DevExpress.ExpressApp.Actions.SimpleAction TomarTarea;
        private DevExpress.ExpressApp.Actions.SimpleAction TomarTareaDV;
        private DevExpress.ExpressApp.Actions.PopupWindowShowAction AsignarTareaDV;
        private DevExpress.ExpressApp.Actions.SimpleAction LiberarTareaDV;
        private DevExpress.ExpressApp.Actions.SimpleAction FinalizarTareaDV;
    }
}
