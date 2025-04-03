using DevExpress.ExpressApp;
using DevExpress.ExpressApp.SystemModule;
using PDNOriginacion.Module.BusinessObjects;

namespace PDNOriginacion.Module.Controllers
{
    public partial class DeactivateDeleteController : ViewController
    {
        public DeactivateDeleteController()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            DeleteObjectsViewController deleteController =
                Frame.GetController<DeleteObjectsViewController>();

            ExportController exportControler =
                Frame.GetController<ExportController>();

            ResetViewSettingsController resetViewSettingsController =
                Frame.GetController<ResetViewSettingsController>();

            RefreshController refreshController =
                Frame.GetController<RefreshController>();

            NewObjectViewController newObjectViewController =
                Frame.GetController<NewObjectViewController>();

            ObjectViewController objViewController = Frame.GetController<ObjectViewController>();


            if (deleteController != null)
            {
                deleteController.Active["Deactivation in code"] =
                    !(View.ObjectTypeInfo.Type == typeof(Consulta) && View is ListView);

                exportControler.Active["Deactivation in code"] =
                    !(View.ObjectTypeInfo.Type == typeof(Consulta) && View is ListView);

                //resetViewSettingsController.Active["Deactivation in code"] =
                //    !(View.ObjectTypeInfo.Type == typeof(Solicitud) && View is ListView);

                //refreshController.Active["Deactivation in code"] =
                //    !(View.ObjectTypeInfo.Type == typeof(Solicitud) && View is ListView);

                newObjectViewController.Active["Deactivation in code"] =
                    !(View.ObjectTypeInfo.Type == typeof(Consulta) && View is ListView);

            }
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

        private void ContactoPersona_Execute(object sender, DevExpress.ExpressApp.Actions.SimpleActionExecuteEventArgs e)
        {

        }
    }
}
