using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using PDNOriginacion.Module.BusinessObjects;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class ChequeController : ViewController
    {
        private NewObjectViewController controller;

        public ChequeController() => InitializeComponent();

        void Controller_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            if(e.CreatedObject is Cheque cheque)
            {
                cheque.Moneda = cheque.Solicitud.Moneda;
            }
        }
        private void RecibirTodosAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            controller = Frame.GetController<NewObjectViewController>();
            controller.ObjectCreated += Controller_ObjectCreated;
        }
        protected override void OnDeactivated()
        {
            controller.ObjectCreated -= Controller_ObjectCreated;
            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();
    }
}
