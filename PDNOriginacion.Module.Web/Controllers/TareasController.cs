using DevExpress.Data.Filtering.Helpers;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Utils;
using PDNOriginacion.Module.BusinessObjects;
using PDNOriginacion.Module.Helpers;
using System;
using System.Web.UI.WebControls;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class TareasController : ViewController
    {
        public TareasController() => InitializeComponent();

        private void AsignarTarea_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            SeleccionarUsuario obj = os.CreateObject<SeleccionarUsuario>();
            obj.RolUsuario = ((Tarea)View.CurrentObject).TipoTarea.RolUsuario;
            DetailView dv = Application.CreateDetailView(os, obj, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.View = dv;
        }
        private void AsignarTarea_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SeleccionarUsuario selUsuario = (SeleccionarUsuario)e.PopupWindowViewCurrentObject;
            Tarea t = (Tarea)View.CurrentObject;
            if(selUsuario.Usuario == null)
            {
                return;
            }

            WFHelper.InsertHistorialTarea(t,
                                          t.Session.GetObjectByKey<Usuario>(selUsuario.Usuario.Oid),
                                          EstadoTarea.Asignada);
            t.ObjectSpace.CommitChanges();
        }
        private void FinalizarTarea_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            foreach(Tarea t in View.SelectedObjects)
            {
                if (t.Estado != EstadoTarea.Cancelada && t.Estado != EstadoTarea.Finalizada)
                {
                    WFHelper.InsertHistorialTarea(t,
                                              t.Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId),
                                              EstadoTarea.Finalizada);
                    t.ObjectSpace.CommitChanges();
                }
            }
        }
        //private void ReasignarTarea_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        //{
        //    SeleccionarUsuario selUsuario = (SeleccionarUsuario)e.PopupWindowViewCurrentObject;
        //    Tarea              t          = (Tarea) View.CurrentObject;
        //    if (selUsuario.Usuario == null)
        //    {
        //        return;
        //    }
        //    WFHelper.InsertHistorialTarea(t, t.Session.GetObjectByKey<Usuario>(selUsuario.Usuario.Oid), EstadoTarea.Asignada);
        //    t.ObjectSpace.CommitChanges();
        //}
        //private void ReasignarTarea_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        //{
        //    IObjectSpace       os  = Application.CreateObjectSpace();
        //    SeleccionarUsuario obj = os.CreateObject<SeleccionarUsuario>();
        //    obj.RolUsuario = ((Tarea)View.CurrentObject).TipoTarea.RolUsuario;
        //    DetailView dv = Application.CreateDetailView(os, obj, true);
        //    dv.ViewEditMode = ViewEditMode.Edit;
        //    e.View          = dv;
        //}
        private void LiberarTarea_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            foreach(Tarea t in View.SelectedObjects)
            {
                if (t.Estado != EstadoTarea.Cancelada && t.Estado != EstadoTarea.Finalizada)
                {
                    WFHelper.InsertHistorialTarea(t,
                                              t.Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId),
                                              EstadoTarea.Disponible);
                    t.ObjectSpace.CommitChanges();
                }
            }
        }
        private void TomarTarea_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            foreach (Tarea t in View.SelectedObjects)
            { 
                if (t.Estado != EstadoTarea.Cancelada && t.Estado != EstadoTarea.Finalizada)
                {
                    WFHelper.InsertHistorialTarea(t,
                                              t.Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId),
                                              EstadoTarea.Asignada);
                    t.ObjectSpace.CommitChanges();
                }
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            //Tarea s = (Tarea) View.CurrentObject;
            //bool a = (bool)s.Evaluate(CriteriaOperator.Parse("Iif(IsNull(ReservadaPor) or Estado = 3 or Estado = 4, false, ReservadaPor.Oid = CurrentUserId() or UsuarioEnRol('PCA_LiberarTareas'))"));

            AsignarTareaDV.TargetObjectsCriteria = AsignarTarea.TargetObjectsCriteria =
                "Estado != 3 and Estado != 4 and UsuarioEnRol('PCA_AsignarTareas')";
            //AsignarTarea.Active.SetItemValue("myReason", (bool)(new ExpressionEvaluator(null, "UsuarioEnRol('PCA_AsignarTareas')")).Evaluate(null));

            ReasignarTarea.TargetObjectsCriteria = "!IsNull(ReservadaPor) and Estado != 3 and Estado != 4 and UsuarioEnRol('PCA_ReasignarTareas')";
            ReasignarTarea.Active
                .SetItemValue("myReason",
                              (bool)(new ExpressionEvaluator(null, "UsuarioEnRol('PCA_ReasignarTareas')")).Evaluate(null));

            LiberarTareaDV.TargetObjectsCriteria = LiberarTarea.TargetObjectsCriteria =
                "Iif(IsNull(ReservadaPor) or Estado = 3 or Estado = 4, false, ReservadaPor.Oid = CurrentUserId() or UsuarioEnRol('PCA_LiberarTareas'))";

            FinalizarTareaDV.TargetObjectsCriteria = FinalizarTarea.TargetObjectsCriteria =
                "Iif(IsNull(ReservadaPor) or Estado = 3 or Estado = 4, false, ReservadaPor.Oid = CurrentUserId() or UsuarioEnRol('PCA_FinalizarTareas'))";

            TomarTareaDV.TargetObjectsCriteria = TomarTarea.TargetObjectsCriteria =
                "Estado != 3 and Estado != 4 and UsuarioEnRol(TipoTarea.RolUsuario.Name)";


/*            ListViewProcessCurrentObjectController listProcessController = Frame.GetController<ListViewProcessCurrentObjectController>();
            if (listProcessController != null)
                listProcessController.CustomProcessSelectedItem += ProcessContactListViewRowController_CustomProcessSelectedItem;*/
        }

/*        void ProcessContactListViewRowController_CustomProcessSelectedItem(object sender, CustomProcessListViewSelectedItemEventArgs e)
        {
            Tarea currentTarea = (Tarea)e.InnerArgs.CurrentObject;
            IObjectSpace objectSpace = Application.CreateObjectSpace();
            SolicitudPersona solicitudPersona = objectSpace.GetObjectByKey<SolicitudPersona>(currentTarea.Solicitud.SolicitudPersonaTitular.Oid);

            DetailView dv = Application.CreateDetailView(objectSpace, "SolicitudPersona_DetailView_CREDIFACIL", true, solicitudPersona);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.InnerArgs.ShowViewParameters.CreatedView = dv;
            e.InnerArgs.ShowViewParameters.TargetWindow = TargetWindow.NewWindow;
            e.Handled = true;
        }*/

        protected override void OnDeactivated()
        {
/*            Frame.GetController<ListViewProcessCurrentObjectController>().CustomProcessSelectedItem -= ProcessContactListViewRowController_CustomProcessSelectedItem;*/

            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        private void ReasignarTarea_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {

        }
    }
}
