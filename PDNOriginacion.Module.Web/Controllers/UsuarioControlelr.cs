using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using PDNOriginacion.Module.BusinessObjects;
using PDNOriginacion.Module.Helpers;

namespace PDNOriginacion.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class UsuarioControlelr : ViewController
    {
        public UsuarioControlelr()
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

        private void ReasignarTareas_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SeleccionarUsuarios selUsuario = (SeleccionarUsuarios)e.PopupWindowViewCurrentObject;
            Usuario usuarioOrigen = (Usuario)View.CurrentObject;
            Usuario usuarioDestino = selUsuario.Usuario;

            var tareas = (new XPCollection<Tarea>(usuarioOrigen.Session)).Where(t =>
                          t.ReservadaPor == usuarioOrigen && t.FechaCierre == null);


            foreach (var item in tareas)
            {
                WFHelper.InsertHistorialTarea(item,
                              item.Session.GetObjectByKey<Usuario>(usuarioDestino.Oid),
                              EstadoTarea.Asignada);
            }

            usuarioOrigen.Save();
            usuarioOrigen.Session.CommitTransaction();
        }

        private void ReasignarTareas_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            SeleccionarUsuarios obj = os.CreateObject<SeleccionarUsuarios>();
            DetailView dv = Application.CreateDetailView(os, obj, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.View = dv;
        }


    }

    [NonPersistent]
    public class SeleccionarUsuarios : BaseObject
    {
        private Usuario _usuario;

        public SeleccionarUsuarios(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [XafDisplayName("Usuario al que se asignarán las tareas")]
        public Usuario Usuario
        {
            get => _usuario;
            set => SetPropertyValue(nameof(Usuario), ref _usuario, value);
        }
    }
}
