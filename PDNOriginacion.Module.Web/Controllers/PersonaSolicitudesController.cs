using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Web.UI.WebControls;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using PDNOriginacion.Module.BusinessObjects;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class PersonaSolicitudesController : ViewController
    {
        public PersonaSolicitudesController()
        {
            InitializeComponent();
            TargetViewId = "Persona_Solicitudes_ListView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            ListViewProcessCurrentObjectController listProcessController = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (listProcessController != null)
                listProcessController.CustomProcessSelectedItem += ProcessContactListViewRowController_CustomProcessSelectedItem;
            
        }

        void ProcessContactListViewRowController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            SolicitudPersona solicitudPersona = (SolicitudPersona)e.InnerArgs.CurrentObject;
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Solicitud));
            Solicitud solicitud = objectSpace.GetObjectByKey<Solicitud>(solicitudPersona.Solicitud.Oid);

            DetailView dv = Application.CreateDetailView(objectSpace, "Solicitud_DetailView_CREDIFACIL", true, solicitud);
            Application.MainWindow.SetView(dv);
            e.InnerArgs.ShowViewParameters.CreatedView = dv;
            e.Handled = true;
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            Frame.GetController<ListViewProcessCurrentObjectController>().CustomProcessSelectedItem -= ProcessContactListViewRowController_CustomProcessSelectedItem;

            base.OnDeactivated();
        }
    }
}
