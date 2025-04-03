using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Reflection;

namespace PDNOriginacion.Module.Web.Controllers
{
    public class AccionesEdicionController : ViewController
    {
        private SimpleAction viewModeAction;
        private SimpleAction viewEditModeAction;
        private SimpleAction refrescarAction;

        public AccionesEdicionController()
        {
            viewModeAction = new SimpleAction(this, "Modo Vista", PredefinedCategory.View);
            viewModeAction.ImageName = "Action_ShowItemOnDashboard";
            viewModeAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewtModeAction_Execute);
            viewEditModeAction = new SimpleAction(this, "Editar", PredefinedCategory.View);
            viewEditModeAction.Category = "PopupActions";
            viewEditModeAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.ViewEditModeAction_Execute);
            viewEditModeAction.ImageName = "Action_Edit";
            refrescarAction = new SimpleAction(this, "Refrescar", PredefinedCategory.View);
            refrescarAction.Category = "PopupActions";
            refrescarAction.Execute += new DevExpress.ExpressApp.Actions.SimpleActionExecuteEventHandler(this.RefrescarObjeto_Execute);
            refrescarAction.ImageName = "Action_Refresh";
            refrescarAction.ToolTip = "Refrescar valores desde la base de datos";

            viewEditModeAction.Active["EsDetailView"] = View is DetailView;
            viewEditModeAction.ToolTip = "Cambiar modo Edicion/Visualizar";
            viewModeAction.Active["EsDetailView"] = View is DetailView;
        }

        private void RefrescarObjeto_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            XPBaseObject baseObject = (XPBaseObject)View.CurrentObject;

            baseObject.Session.Reload(View.CurrentObject);

            Object currentObject = View.CurrentObject;
            Type cot = View.CurrentObject.GetType();

            foreach(PropertyInfo propertyInfo in cot.GetProperties())
            {
                if(typeof(XPBaseObject).IsAssignableFrom(propertyInfo.PropertyType))
                {
                  var o = (XPBaseObject)currentObject.GetType().GetProperty(propertyInfo.Name).GetValue(currentObject, null);
                  if(o != null)
                    {
                      baseObject.Session.Reload(o);
                    }
                }
            }
            View.Refresh();
        }

        protected override void OnActivated()
        {                                                                 
            base.OnActivated();
            
            viewEditModeAction.Active["EsDetailView"] = View is DetailView;
            viewModeAction.Active["EsDetailView"] = View is DetailView;
            if(View is DetailView)
            {
                viewModeAction.Active["EsDetailViewModoEdit"] = ((DetailView)View).ViewEditMode == ViewEditMode.Edit;
                ((DetailView)View).ViewEditModeChanged += View_ViewEditModeChanged;
            }
        }

        private void View_ViewEditModeChanged(object sender, EventArgs e)
        {
            if (((DetailView)View).ViewEditMode != ViewEditMode.View)
            {
                viewModeAction.Active["EsDetailViewModoEdit"] = true;
            }
            else
            {
                viewModeAction.Active["EsDetailViewModoEdit"] = false;
            }

        }

        private void ViewtModeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if(View is DetailView)
            {
                if (((DetailView)View).ViewEditMode != ViewEditMode.View)
                {
                    ((DetailView)View).ViewEditMode = ViewEditMode.View;
                    View.BreakLinksToControls();
                    View.CreateControls();
                }
            }
           
        }

        private void ViewEditModeAction_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            if (((DetailView)View).ViewEditMode == ViewEditMode.Edit)
            {
                ((DetailView)View).ViewEditMode = ViewEditMode.View;
                this.viewEditModeAction.Caption = "Editar";
                this.viewEditModeAction.ImageName = "Action_Edit";
                View.ObjectSpace.CommitChanges();
                View.Refresh();
            }
            else
            {
                ((DetailView)View).ViewEditMode = ViewEditMode.Edit;
                this.viewEditModeAction.Caption = "Modo Vista";
                this.viewEditModeAction.ImageName = "Action_ShowItemOnDashboard";
            }

            View.BreakLinksToControls();
            View.CreateControls();
        }

        protected override void OnDeactivated()
        {
            if (View is DetailView)
            {
                ((DetailView)View).ViewEditModeChanged -= View_ViewEditModeChanged;
            }
            base.OnDeactivated();
        }

    }
}
