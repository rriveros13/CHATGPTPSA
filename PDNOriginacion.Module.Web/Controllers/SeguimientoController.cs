using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Linq;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class SeguimientoController : ViewController
    {
        public SeguimientoController() => InitializeComponent();

        private void ObjectSpace_ModifiedChanged(object sender, EventArgs e) => UpdateActionState();

        private void RegSeguimientoPersona_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //if (View.CurrentObject == null) return;

            IObjectSpace os = Application.CreateObjectSpace();
            Seguimiento SeguimientoNuevo = os.CreateObject<Seguimiento>();

            if(View.CurrentObject != null)
            {
                Type tipo = View.CurrentObject.GetType();

                switch(tipo.Name)
                {
                    case nameof(Persona):
                        Persona persona = (Persona)View.CurrentObject;
                        SeguimientoNuevo.Persona = SeguimientoNuevo.Session.GetObjectByKey<Persona>(persona.Oid);
                        break;
                    case nameof(Solicitud):
                        Solicitud solicitud = (Solicitud)View.CurrentObject;
                        if(solicitud.Titular != null)
                        {
                            SeguimientoNuevo.Persona = SeguimientoNuevo.Session
                                .GetObjectByKey<Persona>(solicitud.Titular.Oid);
                        }
                        if(solicitud.Oid != -1)
                        {
                            SeguimientoNuevo.Solicitud = SeguimientoNuevo.Session
                                .GetObjectByKey<Solicitud>(solicitud.Oid);
                        }
                        break;
                    case nameof(SolicitudPersona):
                        SolicitudPersona solicitudPersona = (SolicitudPersona)View.CurrentObject;
                        SeguimientoNuevo.Persona = SeguimientoNuevo.Session
                            .GetObjectByKey<Persona>(solicitudPersona.Persona.Oid);
                        SeguimientoNuevo.Solicitud = SeguimientoNuevo.Session
                            .GetObjectByKey<Solicitud>(solicitudPersona.Solicitud.Oid);
                        break;
                    case nameof(Seguimiento):
                        Seguimiento seguimiento = (Seguimiento)View.CurrentObject;
                        if(seguimiento.Persona != null)
                        {
                            SeguimientoNuevo.Persona = SeguimientoNuevo.Session
                                .GetObjectByKey<Persona>(seguimiento.Persona);
                        }

                        if(seguimiento.Solicitud != null)
                        {
                            SeguimientoNuevo.Solicitud = SeguimientoNuevo.Session
                                .GetObjectByKey<Solicitud>(seguimiento.Solicitud.Oid);
                        }

                        break;
                    /*case nameof(Seguimiento):
                        Seguimiento seguimientoProgramado = (Seguimiento)View.CurrentObject;

                        SeguimientoNuevo.Seguimiento =
                            SeguimientoNuevo.Session
                            .GetObjectByKey<Seguimiento>(seguimientoProgramado.Oid); 
                        SeguimientoNuevo.Save();
                        SeguimientoNuevo.ObjectSpace.CommitChanges();

                        seguimientoProgramado.Seguimiento = seguimientoProgramado.Session
                            .GetObjectByKey<Seguimiento>(SeguimientoNuevo.Oid);
                        seguimientoProgramado.Save();
                        seguimientoProgramado.ObjectSpace.CommitChanges();

                        SeguimientoNuevo.ObjectSpace.CommitChanges();

                        if(seguimientoProgramado.Persona != null)
                        {
                            SeguimientoNuevo.Persona = SeguimientoNuevo.Session
                                .GetObjectByKey<Persona>(seguimientoProgramado.Persona.Oid);
                        }

                        if(seguimientoProgramado.Solicitud != null)
                        {
                            SeguimientoNuevo.Solicitud = SeguimientoNuevo.Session
                                .GetObjectByKey<Solicitud>(seguimientoProgramado.Solicitud.Oid);
                        }
                        break; */
                }
            }

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(os,
                                                                            "Seguimiento_DetailView",
                                                                            true,
                                                                            SeguimientoNuevo);
        }
        private void RegSeguimientoPersona2_Execute(object sender, SimpleActionExecuteEventArgs e) => RegSeguimientoPersona_Execute(sender,
                                                                                                                                    e);
        private void UpdateActionState()
        {

            if(View == null || View.ObjectTypeInfo.Name != nameof(Solicitud))
            {
                return;
            }

            bool regSeguimiento = true;
            bool regProgramarSeg = true;

            Solicitud s = (Solicitud)View.CurrentObject;

            if(s == null || s.Oid == -1 || s.Producto == null)
            {
                regSeguimiento = false;
                regProgramarSeg = false;
            }

            RegSeguimientoPersona.Active.SetItemValue("Habilitar", regSeguimiento);
            RegSeguimientoPersona2.Active.SetItemValue("Habilitar", regSeguimiento);
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            if(!(View is DashboardView))
            {
                ObjectSpace.ModifiedChanged += ObjectSpace_ModifiedChanged;
                UpdateActionState();
            }
        }
        protected override void OnDeactivated()
        {
            ObjectSpace.ModifiedChanged -= ObjectSpace_ModifiedChanged;
            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        private void simpleAction1_Execute(object sender, SimpleActionExecuteEventArgs e)
        {           
            Seguimiento seguimiento = (Seguimiento)View.CurrentObject;

            if (seguimiento.Solicitud != null)
            {
                foreach (var item in seguimiento.Solicitud.Seguimientos)
                {
                    item.Concretado = true;
                    item.Save();
                }
            }

            if (seguimiento.Persona != null)
            {
                foreach (var item in seguimiento.Persona.Seguimientos)
                {
                    item.Concretado = true;
                    item.Save();
                }
            }

            Seguimiento createdItem = new Seguimiento(seguimiento.Session);

            createdItem.MotivoSeguimiento = seguimiento.MotivoSeguimiento;
            createdItem.Persona = seguimiento.Persona;
            createdItem.SeguimientoOriginal = seguimiento;
            createdItem.Solicitud = seguimiento.Solicitud;
            createdItem.TelefonoContactado = seguimiento.TelefonoContactado;
            createdItem.ObjectSpace = seguimiento.ObjectSpace;

            createdItem.Save();
            seguimiento.Save();

            seguimiento.ObjectSpace.CommitChanges();
            seguimiento.ObjectSpace.Refresh();
        }
    }
}
