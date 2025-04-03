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

namespace PDNOriginacion.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InmuebleTasacionController : ViewController
    {
        private NewObjectViewController controller;

        public InmuebleTasacionController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
            {
               // controller.ObjectCreated += Controller_ObjectCreated;
                if (View != null && View.CurrentObject != null && View.CurrentObject.GetType() == typeof(InmuebleTasacion))
                {
                    InmuebleTasacion pre = (InmuebleTasacion)View.CurrentObject;
                    if (pre != null && pre.EsValorizacion)
                        this.View.Caption = "VALORIZACIÓN DEL INMUEBLE";
                    else
                        this.View.Caption = "TASACIÓN DEL INMUEBLE";

                }
            }
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

        private void Controller_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
           /* if (!(Frame is NestedFrame nestedFrame))
                return;
            if (!(e.CreatedObject is Presupuesto createdItem))
                return;
            if (((NestedFrame)Frame).ViewItem.CurrentObject is Solicitud parent)
            {
                createdItem.IdPresupuesto = string.Concat("PRE-",
                                                          parent.Oid,
                                                          "-",
                                                          (parent.Presupuestos.Count + 1).ToString());
            }  */

        }
    }
}
