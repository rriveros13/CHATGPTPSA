using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using PDNOriginacion.Module.Helpers;

namespace PDNOriginacion.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ConfirmationWindowController : ViewController
    {
        public ConfirmationWindowController()
        {
            InitializeComponent();
            PopupWindowShowAction showConfirmationWindowAction =
                new PopupWindowShowAction(this, "CustomConfirmationWindow", DevExpress.Persistent.Base.PredefinedCategory.View);
            showConfirmationWindowAction.ImageName = "ModelEditor_Views";
            showConfirmationWindowAction.CustomizePopupWindowParams +=
                showConfirmationWindowAction_CustomizePopupWindowParams;
        }

        private void showConfirmationWindowAction_CustomizePopupWindowParams(object                              sender,
                                                                             CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace objectSpace =
                Application.CreateObjectSpace(typeof(ConfirmationWindowParameters));
            ConfirmationWindowParameters parameters =
                objectSpace.CreateObject<ConfirmationWindowParameters>();
            parameters.ConfirmationMessage = "Confirmation message text.";
            objectSpace.CommitChanges();
            DetailView confirmationDetailView = Application.CreateDetailView(objectSpace, parameters);
            confirmationDetailView.Caption      = "Custom Confirmation Window";
            confirmationDetailView.ViewEditMode = DevExpress.ExpressApp.Editors.ViewEditMode.View;
            e.View                              = confirmationDetailView;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }
    }
}
