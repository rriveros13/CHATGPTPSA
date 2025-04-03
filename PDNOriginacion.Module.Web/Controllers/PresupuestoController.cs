using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.BaseImpl;
using PDNOriginacion.Module.BusinessObjects;
using Shared;
using System;
using System.Globalization;
using System.Linq;

namespace PDNOriginacion.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class PresupuestoController : ViewController
    {
        private NewObjectViewController controller;

        public PresupuestoController() => InitializeComponent();

        private void Controller_ObjectCreated(object sender, ObjectCreatedEventArgs e)
        {
            if(!(Frame is NestedFrame nestedFrame))
                return;
            if(!(e.CreatedObject is Presupuesto createdItem))
                return;
            if(((NestedFrame)Frame).ViewItem.CurrentObject is Solicitud parent)
            {
                /*createdItem.IdPresupuesto = string.Concat("PRE-",
                                                          parent.Oid,
                                                          "-",
                                                          (parent.Presupuestos.Count + 1).ToString()); */
            }

        }

        private void aprobar_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Presupuesto pre = (Presupuesto)View.CurrentObject;

            if (pre.Prestamos.Count() == 0)
                throw new Exception("Debe generar las cuotas.");

            if (pre.Prestamos.Count() > 1)
                throw new Exception("El presupuesto no puede tener más de un préstamo.");

            Usuario usu = pre.Session.FindObject<Usuario>(CriteriaOperator.Parse("Oid = '" + SecuritySystem.CurrentUserId + "'"));
            bool userTieneRol = usu.RolesUsuario.Where(x => x.Name == "PCA_AprobarTasa").Count() > 0;

            if (pre.Prestamos[0].Tasa < pre.Solicitud.Producto.PrestamoConfiguracion.TasaRequiereAprobacion)
            {                    
                if (!userTieneRol)
                    throw new System.Exception("No tiene privilegios para aprobar este Presupuesto. La tasa de uno de los préstamos es menor a la permitida.");
            }

            if (pre.Prestamos[0].Capital > pre.Solicitud.Titular.LineaCredito)
                throw new Exception("El monto del préstamo es mayor que la linea de crédito del cliente ("+ pre.Solicitud.Titular.LineaCredito + ")");

            if (pre.Prestamos[0].Cuotas[0].MontoCuota  > pre.Solicitud.Titular.MaximaCuota)
                throw new Exception("El monto de las cuotas es mayor que el establecido como cuota máxima del cliente (" + pre.Solicitud.Titular.MaximaCuota + ")");

            pre.Aprobado = true;
            pre.Aprobador = usu;
            pre.FechaAprobacion = DateTime.Now;
            pre.Solicitud.Monto = pre.Capital;
            pre.Solicitud.Plazo = pre.Prestamos[0].Plazo;
            pre.Solicitud.Save();
            pre.Save();
            pre.ObjectSpace.CommitChanges();   
        }

        private void GenerarCuotas_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            SeleccionarCuotas obj = os.CreateObject<SeleccionarCuotas>();
            DetailView dv = Application.CreateDetailView(os, obj, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.View = dv;
        }

        private void generarCuotas_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            SeleccionarCuotas selCuotas = (SeleccionarCuotas)e.PopupWindowViewCurrentObject;
            Presupuesto pre = (Presupuesto)View.CurrentObject;
            PresupuestoPrestamo.CrearPrestamo(pre, selCuotas.Tasa/12, selCuotas.Plazo, selCuotas.Sistema);
            pre.ObjectSpace.CommitChanges();          
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            controller = Frame.GetController<NewObjectViewController>();
            if(controller != null)
            {
                controller.ObjectCreated += Controller_ObjectCreated;
                if (View != null && View.CurrentObject!= null && View.CurrentObject.GetType() == typeof(Presupuesto))
                {
                    Presupuesto pre = (Presupuesto)View.CurrentObject;
                    if (pre != null && pre.EsPropuesta)
                        this.View.Caption = "PROPUESTA COMERCIAL";
                    else
                        this.View.Caption = "PRESUPUESTO";
                }
            }           
        }

        protected override void OnDeactivated()
        {
            if(controller != null)
            {
                controller.ObjectCreated -= Controller_ObjectCreated;
            }
            base.OnDeactivated();
        }

        private void imprimir_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Presupuesto pres = (Presupuesto)View.CurrentObject;
            IObjectSpace space = Application.CreateObjectSpace(typeof(ReportDataV2));
            IReportDataV2 reportData;

            CultureInfo culture = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name, true);
            culture.NumberFormat.NumberDecimalSeparator = ",";
            culture.NumberFormat.NumberGroupSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            if (pres.EsPropuesta)
                reportData = (IReportDataV2)space.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Propuesta Comercial"));
            else
            {
                if (pres.Prestamos.Count() > 1)
                    throw new Exception("El presupuesto no puede tener más de un préstamo");
                reportData = (IReportDataV2)space.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Presupuesto"));
            }

            CriteriaOperator co2 = CriteriaOperator.Parse("Presupuesto.Oid=?", pres.Oid);
            string reportContainerHandle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
            Frame.GetController<ReportServiceController>().ShowPreview(reportContainerHandle, co2);

        }

    }
}

