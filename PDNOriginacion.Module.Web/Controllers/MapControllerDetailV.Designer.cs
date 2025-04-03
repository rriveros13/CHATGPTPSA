namespace PDNOriginacion.Module.Web.Controllers
{
    partial class MapControllerDetailV
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
            this.PosicionActual = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // PosicionActual
            // 
            this.PosicionActual.Caption = "Posicion Actual";
            this.PosicionActual.Category = "PopupActions";
            this.PosicionActual.ConfirmationMessage = null;
            this.PosicionActual.Id = "PosicionActual";
            this.PosicionActual.ToolTip = null;
            // 
            // MapController
            // 
            this.Actions.Add(this.PosicionActual);
            this.TargetObjectType = typeof(PDNOriginacion.Module.BusinessObjects.GeoLocalizacion);
            this.TargetViewType = DevExpress.ExpressApp.ViewType.DetailView;
            this.TypeOfView = typeof(DevExpress.ExpressApp.DetailView);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction PosicionActual;
    }
}
