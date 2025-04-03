using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Linq;

namespace PDNOriginacion.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class InmuebleController : ViewController
    {

        public InmuebleController() => InitializeComponent();

        private void agregarTasacion_Execute(object sender, SimpleActionExecuteEventArgs e) => CrearNuevaTasacion();
        private void agregarTasacionPU_Execute(object sender, SimpleActionExecuteEventArgs e) => CrearNuevaTasacion();
        private void agregarValoracion_Execute(object sender, SimpleActionExecuteEventArgs e) => CrearNuevaValorizacion();
        private void AgregarValorizacionPU_Execute(object sender, SimpleActionExecuteEventArgs e) => CrearNuevaValorizacion();

        private void CrearNuevaTasacion()
        {
            Inmueble inmueble = (Inmueble)View.CurrentObject;

            InmuebleTasacion ultTasacion = inmueble.Valorizaciones.Where(x => x.Ultimo == true && x.EsValorizacion == true).FirstOrDefault();

            if (ultTasacion == null)
            {
                throw new Exception("No existe valoración al inmueble");
            }

            InmuebleTasacion it = new InmuebleTasacion(inmueble.Session);
            it.Inmueble = inmueble;
            it.SuperficieConstruidaM2 = ultTasacion.SuperficieConstruidaM2;
            it.SuperficieM2 = ultTasacion.SuperficieM2;
            it.ValorM2Construidos = ultTasacion.ValorM2Construidos;
            it.ValorM2Terreno = ultTasacion.ValorM2Terreno;
            it.EsValorizacion = false;
            it.Fecha = DateTime.Now;
            it.Save();
            inmueble.ObjectSpace.CommitChanges();
            View.Refresh();
        }

        private void CrearNuevaValorizacion()
        {
            Inmueble inmueble = (Inmueble)View.CurrentObject;
            InmuebleTasacion it = new InmuebleTasacion(inmueble.Session);
            it.Inmueble = inmueble;
            it.SuperficieConstruidaM2 = inmueble.SuperficieConstruidaM2;
            it.SuperficieM2 = inmueble.SuperficieM2;
            it.EsValorizacion = true;
            it.Fecha = DateTime.Now;
            it.Save();
            inmueble.ObjectSpace.CommitChanges();
            View.Refresh();
        }

        protected override void OnActivated()
        {
            base.OnActivated();   
        }

        private void AgregarASolicitud_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            InmueblesDisponibles parametro = (InmueblesDisponibles)((DetailView)View.ObjectSpace.Owner).CurrentObject;
            foreach (Inmueble item in e.SelectedObjects)
            {
                bool inmuebleExiste = parametro.Solicitud.Inmuebles.Where(x => x.Oid == item.Oid).Count() > 0;
                if (!inmuebleExiste)
                    parametro.Solicitud.Inmuebles.Add(item);
            }

        }

        protected override void OnDeactivated() =>
            // Unsubscribe from previously subscribed events and release other references and resources.
 base.OnDeactivated();
        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

    }
}                                             
