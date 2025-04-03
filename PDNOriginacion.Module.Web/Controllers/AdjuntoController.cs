using DevExpress.ExpressApp;
using PDNOriginacion.Module.BusinessObjects;
using System;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using static PDNOriginacion.Module.BusinessObjects.Adjunto;
using System.Configuration;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class AdjuntoController : ViewController
    {
        private NewObjectViewController controller;

        public AdjuntoController()
        {
            InitializeComponent();
            TargetObjectType = typeof(Adjunto);
        }

        private void ObjectSpace_ObjectChanged(object sender, ObjectChangedEventArgs e)
        {
            if (View.CurrentObject == null)
            {
                return;
            }

            if (!ReferenceEquals(e.Object, ((Adjunto)View.CurrentObject).Archivo))
            {
                return;
            }

            if (e.PropertyName == "Content")
            {
                ((Adjunto)View.CurrentObject).Fecha = DateTime.Now;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ObjectChanged += ObjectSpace_ObjectChanged;
            controller = Frame.GetController<NewObjectViewController>();
            if (controller != null)
            {
                controller.ObjectCreated += controller_ObjectCreated;
            }
        }
        protected override void OnDeactivated()
        {
            ObjectSpace.ObjectChanged -= ObjectSpace_ObjectChanged;
            if (controller != null)
            {
                controller.ObjectCreated -= controller_ObjectCreated;
            }
            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        private void controller_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            NestedFrame nestedFrame = Frame as NestedFrame;
            if (nestedFrame == null) return;
            if (e.CreatedObject.GetType().Name == nameof(Adjunto))
            {
                Adjunto createItem = (Adjunto)e.CreatedObject;
                object parent = ((NestedFrame)Frame).ViewItem.CurrentObject;
                if (parent.GetType().Name == nameof(Solicitud))
                {
                    Solicitud solicitud = (Solicitud)parent;
                    createItem.Persona = createItem.Session.GetObjectByKey<Persona>(solicitud.Titular.Oid);
                }
            }
        }

        private void AsignarAPersona_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            if (View.SelectedObjects != null && View.SelectedObjects.Count > 0)
            {
                var adj = (Adjunto)View.SelectedObjects[0];
                IObjectSpace os = Application.CreateObjectSpace();
                AsignarPersona obj = os.CreateObject<AsignarPersona>();
                obj.Solicitud = os.GetObjectByKey<Solicitud>(adj.Solicitud.Oid);
                DetailView dv = Application.CreateDetailView(os, obj, true);
                dv.ViewEditMode = ViewEditMode.Edit;
                e.View = dv;
            }
        }

        private void AsignarAPersona_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (View.SelectedObjects != null && View.SelectedObjects.Count > 0)
            {
                foreach (var item in View.SelectedObjects)
                {
                    AsignarPersona persona = (AsignarPersona)e.PopupWindowViewCurrentObject;

                    var adjunto = (Adjunto)item;
                    adjunto.Persona = adjunto.ObjectSpace.GetObjectByKey<Persona>(persona.Persona.Oid);
                    adjunto.ObjectSpace.CommitChanges();
                }
            }
        }

        private void AsignarAInmueble_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            if (View.SelectedObjects != null && View.SelectedObjects.Count > 0)
            {
                var adj = (Adjunto)View.SelectedObjects[0];
                IObjectSpace os = Application.CreateObjectSpace();
                AsignarInmueble obj = os.CreateObject<AsignarInmueble>();
                Solicitud sol = os.GetObjectByKey<Solicitud>(adj.Solicitud.Oid);
                obj.Solicitud = sol;
                DetailView dv = Application.CreateDetailView(os, obj, true);
                dv.ViewEditMode = ViewEditMode.Edit;
                e.View = dv;
            }
        }

        private void AsignarAInmueble_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (View.SelectedObjects != null && View.SelectedObjects.Count > 0)
            {
                foreach (var item in View.SelectedObjects)
                {
                    AsignarInmueble inmueble = (AsignarInmueble)e.PopupWindowViewCurrentObject;

                    var adjunto = (Adjunto)item;
                    adjunto.Inmueble = adjunto.ObjectSpace.GetObjectByKey<Inmueble>(inmueble.Inmueble.Oid);
                    adjunto.ObjectSpace.CommitChanges();
                }
            }
        }

        private void AsignarATasacion_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            if (View.SelectedObjects != null && View.SelectedObjects.Count > 0)
            {
                var adj = (Adjunto)View.SelectedObjects[0];
                IObjectSpace os = Application.CreateObjectSpace();
                AsignarTasacion obj = os.CreateObject<AsignarTasacion>();
                Solicitud sol = os.GetObjectByKey<Solicitud>(adj.Solicitud.Oid);
                if (sol.Inmuebles != null && sol.Inmuebles.Count > 0)
                {
                    obj.Inmueble = sol.Inmuebles[0];
                    DetailView dv = Application.CreateDetailView(os, obj, true);
                    dv.ViewEditMode = ViewEditMode.Edit;
                    e.View = dv;
                }
            }
        }

        private void AsignarATasacion_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            if (View.SelectedObjects != null && View.SelectedObjects.Count > 0)
            {
                foreach (var item in View.SelectedObjects)
                {
                    var adjunto = (Adjunto)item;
                    if (adjunto.Solicitud.Inmuebles != null && adjunto.Solicitud.Inmuebles.Count > 0)
                    {
                        AsignarTasacion tasacion = (AsignarTasacion)e.PopupWindowViewCurrentObject;
                        adjunto.InmuebleTasacion = adjunto.ObjectSpace.GetObjectByKey<InmuebleTasacion>(tasacion.Tasacion.Oid);
                        adjunto.ObjectSpace.CommitChanges();
                    }
                }
            }
        }

        private void AdjuntoController_Activated(object sender, EventArgs e)
        {
            if (!View.Model.Id.Contains("ListView"))
            {
                string UsarCustomDV = ConfigurationManager.AppSettings["UsarCustomDetailView"];

                if (UsarCustomDV != null && UsarCustomDV == "S")
                {
                    string codigoInstancia = ConfigurationManager.AppSettings["CodigoInstancia"];
                    IModelList<IModelView> modelViews = View.Model.Application.Views;
                    IModelView myViewNode = modelViews["Adjunto_DetailView_" + codigoInstancia];
                    if (myViewNode != null)
                        View.SetModel(myViewNode);
                }
            }
        }
    }
}
