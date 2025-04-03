using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using PDNOriginacion.Module.Web.Helpers;

namespace PDNOriginacion.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class TasacionController : ViewController
    {
        public TasacionController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            ObjectSpace.ModifiedChanged += ObjectSpace_ModifiedChanged;
            UpdateActionState();
        }

        private void UpdateActionState()
        {
            if (View is DashboardView) return;

            if (View == null || View.ObjectTypeInfo.Name != nameof(InmuebleTasacion))
            {
                return;
            }

            string urlVisor = System.Configuration.ConfigurationManager.AppSettings["urlVisorGallery"];

            string url = $"{urlVisor}?src={Helper.GenerateUrlParam(View.CurrentObject)}";
            VerAdjuntosPopup.SetClientScript($"window.open('{url}', '_blank')", false);
        }

        private void ObjectSpace_ModifiedChanged(object sender, EventArgs e) => UpdateActionState();

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
