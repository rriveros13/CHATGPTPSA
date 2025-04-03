using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace PDNOriginacion.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class ProductoController : ViewController
    {
        public ProductoController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
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

        private void ProductoController_Activated(object sender, EventArgs e)
        {
            if (!View.Model.Id.Contains("ListView"))
            {
                string UsarCustomDV = ConfigurationManager.AppSettings["UsarCustomDetailView"];

                if (UsarCustomDV != null && UsarCustomDV == "S")
                {
                    string codigoInstancia = ConfigurationManager.AppSettings["CodigoInstancia"];
                    IModelList<IModelView> modelViews = View.Model.Application.Views;
                    IModelView myViewNode = modelViews["Producto_DetailView_" + codigoInstancia];
                    if (myViewNode != null)
                        View.SetModel(myViewNode);
                }
            }
        }
    }
}
