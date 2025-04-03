using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using PDNOriginacion.Module.BusinessObjects;
using System;

namespace PDNOriginacion.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class WFCamposEstadoController : ViewController
    {
        public WFCamposEstadoController() => InitializeComponent();

        private string GetEstadosProductosConc(Producto prod)
        {
            string cadena = string.Empty;
            foreach(EstadoSolicitud item in prod.EstadosUsados)
            {
                cadena += $"'{item.Oid.ToString()}',";
            }

            return cadena.Substring(0, cadena.Length - 1);
        }
        private string GetTareasProductosConc(Producto prod)
        {
            string cadena = string.Empty;
            foreach(WFTarea item in prod.Tareas)
            {
                cadena += $"'{item.Oid.ToString()}',";
            }

            return cadena.Substring(0, cadena.Length - 1);
        }
        private void popupWindowShowAction1_CustomizePopupWindowParams(object sender,
                                                                       CustomizePopupWindowParamsEventArgs e)
        {
            CampoProductoExcep campoProd = (CampoProductoExcep)((DetailView)View.ObjectSpace.Owner).CurrentObject;
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(EstadoSolicitud));
            string noteListViewId = Application.FindLookupListViewId(typeof(EstadoSolicitud));
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace,
                                                                                       typeof(EstadoSolicitud),
                                                                                       noteListViewId);
            collectionSource.SetCriteria("IdEstado",
                                         $@"Oid In ({GetEstadosProductosConc(campoProd.CampoProducto.Producto)})");
            e.View = Application.CreateListView(noteListViewId, collectionSource, true);
        }
        private void popupWindowShowAction1_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            CampoProductoExcep campo = (CampoProductoExcep)((DetailView)View.ObjectSpace.Owner).CurrentObject;
            CampoProductoExcep campoSel = (CampoProductoExcep)View.CurrentObject;
            foreach(object item in e.PopupWindowViewSelectedObjects)
            {
                CampoProductoExcep campoConf = new CampoProductoExcep(campoSel.Session)
                {
                    CampoProducto = campoSel.CampoProducto,
                    CriterioEditable = campoSel.CriterioEditable,
                    CriterioVal = campoSel.CriterioVal,
                    CriterioVis = campoSel.CriterioVis,
                    EstadoDestino = campoSel.Session.GetObjectByKey<EstadoSolicitud>(((EstadoSolicitud)item).Oid),
                    MensajeError = campoSel.MensajeError,
                    Obligatorio = campoSel.Obligatorio
                };
                campoConf.Save();
            }
            campoSel.CampoProducto.Producto.Save();
            campoSel.CampoProducto.Producto.ObjectSpace.CommitChanges();
        }
        private void popupWindowShowAction2_CustomizePopupWindowParams(object sender,
                                                                       CustomizePopupWindowParamsEventArgs e)
        {
            CampoProductoExcep campoProd = (CampoProductoExcep)((DetailView)View.ObjectSpace.Owner).CurrentObject;
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(WFTarea));
            string noteListViewId = Application.FindLookupListViewId(typeof(WFTarea));
            CollectionSourceBase collectionSource = Application.CreateCollectionSource(objectSpace,
                                                                                       typeof(WFTarea),
                                                                                       noteListViewId);
            collectionSource.SetCriteria("IdTarea",
                                         $@"Oid In ({GetTareasProductosConc(campoProd.CampoProducto.Producto)})");
            e.View = Application.CreateListView(noteListViewId, collectionSource, true);
        }
        private void popupWindowShowAction2_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            CampoProductoExcep campo = (CampoProductoExcep)((DetailView)View.ObjectSpace.Owner).CurrentObject;
            CampoProductoExcep campoSel = (CampoProductoExcep)View.CurrentObject;
            foreach(object item in e.PopupWindowViewSelectedObjects)
            {
                CampoProductoExcep campoConf = new CampoProductoExcep(campoSel.Session)
                {
                    CampoProducto = campoSel.CampoProducto,
                    CriterioEditable = campoSel.CriterioEditable,
                    CriterioVal = campoSel.CriterioVal,
                    CriterioVis = campoSel.CriterioVis,
                    WFTarea = campoSel.Session.GetObjectByKey<WFTarea>(((WFTarea)item).Oid),
                    EstadoDestino = campoSel.EstadoDestino,
                    MensajeError = campoSel.MensajeError,
                    Obligatorio = campoSel.Obligatorio
                };
                campoConf.Save();
            }
            campoSel.CampoProducto.Producto.Save();
            campoSel.CampoProducto.Producto.ObjectSpace.CommitChanges();
        }
        private void simpleAction1_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Producto prod = (Producto)((DetailView)View.ObjectSpace.Owner).CurrentObject;
            foreach(object item in e.SelectedObjects)
            {
                CampoProducto campoConf = new CampoProducto(prod.Session) { Campo = (Campo)item, Producto = prod };
                campoConf.Save();
            }
            prod.Save();
            prod.ObjectSpace.CommitChanges();
            prod.ObjectSpace.Refresh();
        }

        protected override void OnActivated() => base.OnActivated();
        protected override void OnDeactivated() =>
 // Unsubscribe from previously subscribed events and release other references and resources.
 base.OnDeactivated();
        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();
    }
}
