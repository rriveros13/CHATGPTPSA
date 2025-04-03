using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Xpo;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace PDNOriginacion.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SolicitudPersonaController : ViewController
    {
        public SolicitudPersonaController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated() => base.OnActivated();

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated() => base.OnDeactivated();

        private void SolicitudPersonaController_Activated(object sender, EventArgs e)
        {
            if (!View.Model.Id.Contains("ListView"))
            {
                string UsarCustomDV = ConfigurationManager.AppSettings["UsarCustomDetailView"];

                if (UsarCustomDV != null && UsarCustomDV == "S")
                {
                    string codigoInstancia = ConfigurationManager.AppSettings["CodigoInstancia"];
                    IModelList<IModelView> modelViews = View.Model.Application.Views;
                    IModelView myViewNode = modelViews["SolicitudPersona_DetailView_" + codigoInstancia];
                    if(myViewNode != null)
                        View.SetModel(myViewNode);
                }
            }
        }

        private void TraerEmpleos_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SolicitudPersona solPer = (SolicitudPersona)View.CurrentObject;

            solPer.Session.Delete(solPer.Ingresos);
            solPer.Session.Save(solPer.Ingresos);

            if (solPer.Persona == null)
            {
                throw new Exception("Debe cargar a la persona asociada a la solicitud");
            }

            IngresoConcepto concepto = solPer.Session.FindObject<IngresoConcepto>(CriteriaOperator.Parse("Nombre = 'SUELDO'"));

            if (solPer.Persona.Empleos.Where(em => em.Actual).Count() == 0)
            {
                throw new Exception("No existe ingresos laborales asociados a ésta persona.");
            }

            foreach (var item in solPer.Persona.Empleos.Where(em => em.Actual))
            {
                bool existe = solPer.Ingresos.Where(i => i.PersonaEmpleo == item).Count() > 0;

                if (!existe)
                {
                    SolicitudPersonaIngreso ing = new SolicitudPersonaIngreso(solPer.Session);
                    ing.Concepto = concepto;
                    ing.Monto = item.Salario;
                    ing.Observacion = "Dato extraído de empleos de la persona";
                    ing.PersonaEmpleo = item;
                    ing.SolicitudPersona = solPer;
                    ing.ObjectSpace = solPer.ObjectSpace;
                    ing.Save();

                    solPer.Save();
                    solPer.Solicitud.Save();
                    solPer.ObjectSpace.CommitChanges();
                    // solPer.ObjectSpace.Refresh();
                }
            }
            solPer.ObjectSpace.Refresh();
        }

        private void TraerRefPersonales_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            SolicitudPersona solPer = (SolicitudPersona)View.CurrentObject;

            if (solPer.Persona == null)
            {
                throw new Exception("Debe cargar a la persona asociada a la solicitud");
            }

            var todasMenosActual = solPer.Persona.Solicitudes.Where(x => x.Solicitud.Oid != solPer.Solicitud.Oid && x.TipoPersona.Codigo == "SOL");

            SolicitudPersona ultimaSolicitud = todasMenosActual.Where(s => s.Solicitud.Oid == todasMenosActual.Max(x => x.Solicitud.Oid)).FirstOrDefault();

            if (ultimaSolicitud == null || ultimaSolicitud.ReferenciasPersonales.Count() == 0)
            {
                throw new Exception("No existe referencias personales asociados a ésta persona.");
            }

            foreach (SolicitudPersonaRefPer solicitudPersonaRefPer in ultimaSolicitud.Solicitud.SolicitudPersonaTitular.ReferenciasPersonales)
            {
                SolicitudPersonaRefPer refPer = new SolicitudPersonaRefPer(solPer.Session);
                refPer.SolicitudPersona = solPer;
                refPer.NombreCompleto = solicitudPersonaRefPer.NombreCompleto;
                refPer.Parentezco = solicitudPersonaRefPer.Parentezco;
                refPer.Telefono = solicitudPersonaRefPer.Telefono;
                refPer.Save();
            }

            solPer.ObjectSpace.CommitChanges();
            solPer.ObjectSpace.Refresh();
        }
    }
}
