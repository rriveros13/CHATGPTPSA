namespace PDNOriginacion.Module.Web.Controllers
{
    partial class MapsGoogleController
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
            this.AbrirGMaps = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            this.AbrirGMapsPopup = new DevExpress.ExpressApp.Actions.SimpleAction(this.components);
            // 
            // AbrirGMaps
            // 
            this.AbrirGMaps.Caption = "Abrir GMaps";
            this.AbrirGMaps.ConfirmationMessage = null;
            this.AbrirGMaps.Id = "AbrirGMaps";
            this.AbrirGMaps.ToolTip = "Ver en Google Maps";
            // 
            // AbrirGMapsPopup
            // 
            this.AbrirGMapsPopup.Caption = "Abrir GMaps Popup";
            this.AbrirGMapsPopup.Category = "PopupActions";
            this.AbrirGMapsPopup.ConfirmationMessage = null;
            this.AbrirGMapsPopup.Id = "AbrirGMapsPopup";
            this.AbrirGMapsPopup.ToolTip = "Ver en Google Maps";
            // 
            // MapsGoogleController
            // 
            this.Actions.Add(this.AbrirGMaps);
            this.Actions.Add(this.AbrirGMapsPopup);

        }

        #endregion

        private DevExpress.ExpressApp.Actions.SimpleAction AbrirGMaps;
        private DevExpress.ExpressApp.Actions.SimpleAction AbrirGMapsPopup;
    }
}
