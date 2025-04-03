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
using PDNOriginacion.Module.Web.Controllers.NonPersistentClasses;

namespace PDNOriginacion.Module.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class AccionesController : ViewController
    {
        public AccionesController()
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

        private void CargaRapidaInmueble_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Persona per = (Persona)View.CurrentObject;
            per.Save();

            if (per.CRI_CuentaCatastral == string.Empty)
                throw new Exception("Debe cargar la cuenta catastral.");
            else if (per.CRI_Departamento == null)
                throw new Exception("Debe cargar el Departamento.");
            else if (per.CRI_Ciudad == null)
                throw new Exception("Debe cargar la Ciudad.");

            Inmueble inm = new Inmueble(per.Session);
            inm.CuentaCatastral = per.CRI_CuentaCatastral;
            inm.SuperficieM2 = per.CRI_SuperficieM2;
            inm.TipoCamino = per.CRI_TipoCamino;
            inm.TipoInmueble = per.CRI_TipoInmueble;
            inm.EstadoTitulo = per.CRI_EstadoTitulo;
            inm.ImpuestoAlDia = per.CRI_ImpuestoAlDia;
            inm.Observaciones = per.CRI_Observaciones;
            inm.ValorAproximado = per.CRI_ValorAproximado;

            Direccion dir = new Direccion(per.Session);
            dir.Departamento = per.CRI_Departamento;
            dir.Ciudad = per.CRI_Ciudad;
            dir.Barrio = per.CRI_Barrio;
            dir.Calle = per.CRI_Calle;
            dir.Numero = per.CRI_Numero;

            dir.Save();
            inm.Direccion = dir;
            inm.Save();
            per.Inmuebles.Add(inm);
            per.Save();

            per.CRI_CuentaCatastral = DateTime.Now.ToString("dd/MM") + SecuritySystem.CurrentUserName;
            per.CRI_Departamento = null;
            per.CRI_Ciudad = null;
            per.CRI_Barrio = null;
            per.CRI_Calle = "Sin asignar";
            per.CRI_Numero = "Sin Nro";
            per.CRI_SuperficieM2 = 0;
            per.CRI_TipoCamino = null;
            per.CRI_TipoInmueble = null;
            per.CRI_EstadoTitulo = EstadoTitulo.SiTiene;
            per.CRI_ImpuestoAlDia = false;
            per.CRI_Observaciones = string.Empty;
            per.CRI_ValorAproximado = 0;

            per.ObjectSpace.CommitChanges();
            per.ObjectSpace.Refresh();
        }

        private void CargaRapidaDireccion_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Persona per = (Persona)View.CurrentObject;
            per.Save();

            if (per.CRD_Departamento == null)
                throw new Exception("Debe cargar el Departamento.");
            else if (per.CRD_Ciudad == null)
                throw new Exception("Debe cargar la Ciudad.");

            Direccion dir = new Direccion(per.Session);
            dir.Pais = per.CRD_Pais;
            dir.Departamento = per.CRD_Departamento;
            dir.Ciudad = per.CRD_Ciudad;
            dir.Barrio = per.CRD_Barrio;
            dir.Calle = per.CRD_Calle;
            dir.Numero = per.CRD_Numero;

            PersonaDireccion pd = new PersonaDireccion(per.Session);
            pd.Direccion = dir;
            pd.Persona = per;
            pd.Principal = true;

            dir.Save();
            per.Direcciones.Add(pd);
            per.Save();

            per.CRD_Pais = per.Session.FindObject<Pais>(Pais.Fields.Codigo == "1");
            per.CRD_Departamento = null;
            per.CRD_Ciudad = null;
            per.CRD_Barrio = null;
            per.CRD_Calle = "Sin Asignar";
            per.CRD_Numero = "Sin Nro";

            per.ObjectSpace.CommitChanges();
            per.ObjectSpace.Refresh();

        }

        private void CargaRapidaTelefono_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Persona per = (Persona)View.CurrentObject;
            per.Save();

            if (per.CRT_Tipo== null)
                throw new Exception("Debe cargar el tipo de teléfono.");
            else if (per.CRT_Prefijo == null)
                throw new Exception("Debe cargar el prefijo.");
            else if (per.CRT_Numero== null)
                throw new Exception("Debe cargar el número de teléfono.");

            Telefono tel = new Telefono(per.Session);
            tel.TipoTelefono = per.CRT_TipoTelefono;
            tel.Tipo = per.CRT_Tipo;
            tel.Prefijo = per.CRT_Prefijo;
            tel.Numero = per.CRT_Numero;
            tel.Whatsapp = per.CRT_Whatsapp;
            tel.Preferido = per.CRT_Preferido;

            tel.Save();
            per.Telefonos.Add(tel);
            per.Save();

            per.CRT_Tipo = null;
            per.CRT_Prefijo = null;
            per.CRT_Numero = string.Empty;

            per.ObjectSpace.CommitChanges();
            per.ObjectSpace.Refresh();
        }

        private void CargaRapidaIngresos_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SolicitudPersona solPer = (SolicitudPersona)View.CurrentObject;
            solPer.Save();

            if (solPer.CRIE_Concepto == null)
                throw new Exception("Debe cargar un concepto.");
            else if (solPer.CRIE_Monto <= 0 )
                throw new Exception("Debe cargar un monto válido.");

            if (solPer.CRIE_EsEgreso)
            {
                SolicitudPersonaEgreso egreso = new SolicitudPersonaEgreso(solPer.Session);
                egreso.ACancelar = solPer.CRIE_ACancelar;
                egreso.Concepto = solPer.CRIE_Concepto;
                egreso.Monto = (double)solPer.CRIE_Monto;
                egreso.SolicitudPersona = solPer;
                egreso.Observacion = solPer.CRIE_Observacion;
                egreso.Save();
            }
            else 
            {
                SolicitudPersonaIngreso ingreso = new SolicitudPersonaIngreso(solPer.Session);
                ingreso.Concepto = solPer.CRIE_Concepto;
                ingreso.Monto = (double)solPer.CRIE_Monto;
                ingreso.SolicitudPersona = solPer;
                ingreso.Observacion = solPer.CRIE_Observacion;
                ingreso.Save();
            }

            solPer.CRIE_ACancelar = false;
            solPer.CRIE_Concepto = null;
            solPer.CRIE_EsEgreso = true;
            solPer.CRIE_Monto = 0;
            solPer.CRIE_Observacion = string.Empty;

            solPer.ObjectSpace.CommitChanges();
            solPer.ObjectSpace.Refresh();
        }

        private void CrearTarea_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            CrearTarea crearTarea = (CrearTarea)View.CurrentObject;
            Solicitud solicitud = os.GetObjectByKey<Solicitud>(crearTarea.Solicitud.Oid);

            Tarea t = os.CreateObject<Tarea>();
            t.Descripcion = crearTarea.Descripcion;
            t.Estado = EstadoTarea.Asignada;
            t.FechaCreacion = DateTime.Now;
            t.ObjectSpace = solicitud.ObjectSpace;
            t.ReservadaPor = solicitud.Session.GetObjectByKey<Usuario>(crearTarea.Usuario.Oid);
            t.Solicitud = solicitud;
            t.Prioridad = crearTarea.Prioridad;
            t.Manual = true;
            t.TipoTarea = solicitud.Session.GetObjectByKey<WFTarea>(crearTarea.TipoTarea.Oid);
            t.Save();

            solicitud.Save();
            solicitud.ObjectSpace.CommitChanges();
            solicitud.ObjectSpace.Refresh();

            crearTarea.Descripcion = string.Empty;
            crearTarea.TipoTarea = null; 
        }
    }
}
