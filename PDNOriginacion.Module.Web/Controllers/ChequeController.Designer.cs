namespace PDNOriginacion.Module.Web.Controllers
{
    partial class ChequeController
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
            this.RecibirTodosAction = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // RecibirTodosAction
            // 
            this.RecibirTodosAction.Caption = "Recibir todos";
            this.RecibirTodosAction.ConfirmationMessage = null;
            this.RecibirTodosAction.Id = "47f4ccba-f3c4-4891-8167-8f2c5be4f285";
            this.RecibirTodosAction.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Cheque);
            this.RecibirTodosAction.ToolTip = null;
            this.RecibirTodosAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RecibirTodosAction_Execute);
            // 
            // ChequeController
            // 
            this.Actions.Add(this.RecibirTodosAction);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.Cheque);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction RecibirTodosAction;
    }
}
