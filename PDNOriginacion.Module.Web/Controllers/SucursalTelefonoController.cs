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
    public partial class SucursalTelefonoController : ViewController
    {
        private NewObjectViewController controller;
        public SucursalTelefonoController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }

        private void controller_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            NestedFrame nestedFrame = Frame as NestedFrame;
            if (nestedFrame == null)
            {
                return;
            }

            if (e.CreatedObject.GetType().Name == nameof(SucursalTelefono))
            {
                SucursalTelefono createdItem = (SucursalTelefono)e.CreatedObject;
                object parent = ((NestedFrame)Frame).ViewItem.CurrentObject;
                if (parent.GetType().Name == nameof(Sucursal))
                {
                    Sucursal Sucursal = (Sucursal)((NestedFrame)Frame).ViewItem.CurrentObject;
                    createdItem.Sucursal = createdItem.Session.GetObjectByKey<Sucursal>(Sucursal.Oid);
                }
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
            {
                controller.ObjectCreated += controller_ObjectCreated;
            }
        }

        protected override void OnDeactivated()
        {
            if (controller != null)
            {
                controller.ObjectCreated -= controller_ObjectCreated;
            }

            base.OnDeactivated();
        }
    }
}
