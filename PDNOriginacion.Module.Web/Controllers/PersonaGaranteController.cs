using System;
using System.Linq;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.Persistent.BaseImpl;
using PDNOriginacion.Module.BusinessObjects;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class PersonaGaranteController : ViewController
    {
        public PersonaGaranteController() => InitializeComponent();
        protected override void OnActivated()
        {
            base.OnActivated();
        }
        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        private void GenerarSolicitudGarante_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SolicitudPersona solicitudPersonaGarante = (SolicitudPersona)View.CurrentObject;

            solicitudPersonaGarante.Persona.Reload();

            if (solicitudPersonaGarante.TipoPersona.Codigo == "SOL")
            {
                throw new Exception("El titular del crédito no puede ser garante!");
            }

            IObjectSpace os = Application.CreateObjectSpace();
            var producto = os.FindObject<Producto>(Producto.Fields.Codigo == "CREDIFACIL_5");

            foreach (var item in producto.GeneracionSolicitud)
            {
                if ((bool)solicitudPersonaGarante.Persona.Evaluate(item.Criterio) == false)
                    throw new System.Exception(item.Mensaje);
            }

            Solicitud solicitudTitular = os.GetObjectByKey<Solicitud>(solicitudPersonaGarante.Solicitud.Oid);

            Solicitud sol = os.CreateObject<Solicitud>();
            sol.Producto = producto;
            sol.Documento = solicitudPersonaGarante.Persona.Documento;
            sol.TipoDocumento = os.GetObjectByKey<TipoDocumento>(solicitudPersonaGarante.Persona.TipoDocumento.Oid);
            sol.UsuarioParaTareas = Usuario.GetUsuarioLibre(producto.RolGeneracion.Name, sol.Session);

            sol.Plazo = solicitudTitular.Plazo;
            sol.TasaAnual = solicitudTitular.TasaAnual;
            sol.MontoPresupuesto = solicitudTitular.MontoPresupuesto;
            sol.Monto = solicitudTitular.Monto;
            sol.SolicitudOriginal = solicitudTitular;

            if (solicitudPersonaGarante.Persona.MotivoSolicitud != null)
                sol.Motivo = os.GetObjectByKey<MotivoSolicitud>(solicitudPersonaGarante.Persona.MotivoSolicitud.Oid);

            sol.Save();

            //datos del presupuesto
            Presupuesto presupuestoTitular = os.FindObject<Presupuesto>(Presupuesto.Fields.AceptadoCliente == (CriteriaOperator)true);
            Presupuesto presupuestoGarante = os.CreateObject<Presupuesto>();
            presupuestoGarante = presupuestoTitular;
            presupuestoGarante.Solicitud = sol;
            presupuestoGarante.Save();

            //datos del garante
            SolicitudPersona spGaranteNuevaSolicitud = os.CreateObject<SolicitudPersona>();
            spGaranteNuevaSolicitud.Persona = os.GetObjectByKey<Persona>(solicitudPersonaGarante.Persona.Oid);
            spGaranteNuevaSolicitud.Solicitud = sol;
            spGaranteNuevaSolicitud.TipoPersona = os.FindObject<TipoPersona>(CriteriaOperator.Parse("Codigo = 'SOL'"));
            spGaranteNuevaSolicitud.Parentezco = os.GetObjectByKey<Parentezco>(solicitudPersonaGarante.Parentezco.Oid);
            spGaranteNuevaSolicitud.Save();

            sol.Save();
            solicitudPersonaGarante.Persona.ObjectSpace.CommitChanges();
            solicitudTitular.SolicitudCodeudor = sol;
            solicitudTitular.Save();

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(os,
                                                                            "Solicitud_DetailView",
                                                                            true,
                                                                            sol);

        }

        private void AgregarConyuge_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SolicitudPersona solicitudPersonaTitular = (SolicitudPersona)View.CurrentObject;

            SolicitudPersona verificarConyuge = solicitudPersonaTitular?.Solicitud?.Personas?.Where(p => p.TipoPersona.Codigo == "CON").FirstOrDefault();

            if (verificarConyuge != null)
            {
                throw new Exception("La solicitud ya tiene Conyuge asociado!");
            }

            PersonaVinculo conyugue = solicitudPersonaTitular?.Persona?.Vinculos?.Where(p => p.Parentezco.Codigo == "CONYUGE").FirstOrDefault();

            if (conyugue != null)
            {
                SolicitudPersona insertarConyugue = solicitudPersonaTitular.ObjectSpace.CreateObject<SolicitudPersona>();
                insertarConyugue.Persona = solicitudPersonaTitular.ObjectSpace.GetObjectByKey<Persona>(conyugue.Vinculo.Oid);
                insertarConyugue.Solicitud = solicitudPersonaTitular.Solicitud;
                insertarConyugue.TipoPersona = solicitudPersonaTitular.ObjectSpace.FindObject<TipoPersona>(CriteriaOperator.Parse("Codigo = 'CON'"));
                insertarConyugue.Parentezco = solicitudPersonaTitular.ObjectSpace.FindObject<Parentezco>(CriteriaOperator.Parse("Codigo = 'CONYUGE'"));
                insertarConyugue.Save();
                insertarConyugue.ObjectSpace.CommitChanges();
                insertarConyugue.Reload();
            }
            else
            {
                throw new Exception("El titular de la solicitud no cuenta con Conyuge!");
            }
        }
    }
}
