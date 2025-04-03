using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using Shared;
using System.Linq;
using System;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.DC;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class PresupuestoPrestamo : BaseObject
    {
        public PresupuestoPrestamo(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        private Presupuesto _presupuesto;
        private string _descripcion;
        private decimal _capital;
        private decimal _tasaMensual;
        private decimal _tasa;
        private int _plazo;
        private decimal _totalImponible;
        private bool _aceptadoCliente;

        [Association("Presupuesto-PrespuestoPrestamo")]
        public Presupuesto Presupuesto
        {
            get => _presupuesto;
            set => SetPropertyValue(nameof(Presupuesto), ref _presupuesto, value);
        }

        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        [Appearance("", Enabled = false, Context = "Any")]
        public decimal Capital
        {
            get => _capital;
            set => SetPropertyValue(nameof(Capital), ref _capital, value);
        }

        [Appearance("", Enabled = false, Context = "Any")]
        [XafDisplayName("Tasa mensual con IVA")]
        public decimal TasaMensual
        {
            get => _tasaMensual;
            set => SetPropertyValue(nameof(TasaMensual), ref _tasaMensual, value);
        }

        [Appearance("", Enabled = false, Context = "Any")]
        [XafDisplayName("Tasa anual con IVA")]
        public decimal Tasa
        {
            get => _tasa;
            set => SetPropertyValue(nameof(Tasa), ref _tasa, value);
        }

        [Appearance("", Enabled = false, Context = "Any")]
        public int Plazo
        {
            get => _plazo;
            set => SetPropertyValue(nameof(Plazo), ref _plazo, value);
        }

        [Appearance("", TargetItems = nameof(AceptadoCliente), Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "Presupuesto.EsPropuesta = False")]
        [Appearance("", Enabled = false, Context = "Any")]
        public bool AceptadoCliente
        {
            get => _aceptadoCliente;
            set => SetPropertyValue(nameof(AceptadoCliente), ref _aceptadoCliente, value);
        }

        private XPCollection<AuditDataItemPersistent> auditoria;
        public XPCollection<AuditDataItemPersistent> Auditoria
        {
            get
            {
                if (auditoria == null)
                {
                    auditoria = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditoria;
            }
        }

        public static void CrearPrestamo(Presupuesto presupuesto, decimal tasa, int plazo, SistemasPrestamos sistema)
        {
            IPrestamo calculadora= null;
            Prestamo p;

            if (sistema == SistemasPrestamos.SistemaFrancesCF)
            {
                calculadora = new Prestamos.CredifacilFrances();
                p = calculadora.GetPrestamo(presupuesto.Capital, tasa, plazo, sistema, presupuesto.Solicitud.Monto);
            }
            else
            {
                if (sistema == SistemasPrestamos.FrancesStandar)
                    calculadora = new Prestamos.Frances();
                else if (sistema == SistemasPrestamos.CuotasDeInteres)
                    calculadora = new Prestamos.CuotasDeInteres();
                else if (sistema == SistemasPrestamos.SistemaCF)
                    calculadora = new Prestamos.Credifacil();
                else if (sistema == SistemasPrestamos.Aleman)
                    calculadora = new Prestamos.Aleman();

                p = calculadora.GetPrestamo(presupuesto.Capital, tasa, plazo, sistema);
            }

            PresupuestoPrestamo prestamo = new PresupuestoPrestamo(presupuesto.Session);
            prestamo.Presupuesto = presupuesto;
            prestamo.CopiarPrestamo(p);
            GenerarGastos(prestamo);
            prestamo.Save();
        }

        private static void GenerarGastos(PresupuestoPrestamo prestamo)
        {
            foreach (var g in prestamo.Presupuesto.Solicitud.Producto.GastosAdministrativos)
            {
                PresupuestoGasto pg = new PresupuestoGasto(prestamo.Session);
                pg.Descripcion = g.Descripcion;
                pg.PresupuestoPrestamo = prestamo;
                pg.Cantidad = (decimal)pg.Evaluate(CriteriaOperator.Parse($"ToDecimal({g.Cantidad})"));
                pg.Monto = (decimal)pg.Evaluate(CriteriaOperator.Parse($"ToDecimal({g.Monto})"));
                pg.MontoCriteria = g.Monto;
                pg.PuedeEliminarse = g.PuedeEliminarse;
                pg.PuedeEditarMonto = g.PuedeEditarMonto;
                pg.Codigo = g.Codigo;

                pg.Save();
            }

            prestamo.Save();
        }

        [Association("PresupuestoPrestamo-PrestamoCuota")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<PrestamoCuota> Cuotas => GetCollection<PrestamoCuota>(nameof(Cuotas));

        public void CopiarPrestamo(Prestamo p)
        {
            Plazo = p.CantCuotas;
            Capital = p.Capital;
            Descripcion = p.Descripcion;
            TasaMensual = p.TasaMensual;
            Tasa = p.TasaMensual * 12; 
            TotalImponible = Capital + ((TasaMensual/100) * Capital * this.Presupuesto.Solicitud.Producto.PrestamoConfiguracion.PlazoParaGastos);

            foreach(Cuota c in p.Cuotas)
            {
                PrestamoCuota dbC = new PrestamoCuota(Session);
                dbC.Prestamo = this;
                dbC.Amortizacion = c.Amortizacion;
                dbC.Capital = c.Capital;
                dbC.Interes = c.Interes;
                dbC.MontoCuota = c.MontoCuota;
                dbC.NroCuota = c.NroCuota;
                dbC.IVA = c.IVA;
                dbC.MontoCuotaSinIVA = c.MontoCuotaSinIVA;
                dbC.Save();
            }

            Save();
        }

        [Appearance("", Enabled = false, Context = "Any")]
        public decimal TotalImponible
        {
            get => _totalImponible;
            set => SetPropertyValue(nameof(TotalImponible), ref _totalImponible, value);
        }


        [NonPersistent]
        [ImmediatePostData]
        public decimal TotalGastos
        {
            get
            {
                if (!IsLoading && !IsSaving)
                {
                    return GastosAdministrativos.Sum(x => x.Monto);
                }
                else
                {
                    return 0;
                }
            }
        }

        [NonPersistent]
        public decimal Neto => Capital - TotalGastos;

        [NonPersistent]
        public decimal TotalCuotas => Cuotas.Sum<PrestamoCuota>(c => c.MontoCuota);

        [Association("PresupuestoPrestamo-PrespuestoGasto")]
        public XPCollection<PresupuestoGasto> GastosAdministrativos => GetCollection<PresupuestoGasto>(nameof(GastosAdministrativos));

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public Presupuesto.FieldsClass Presupuesto
            {
                get
                {
                    return new Presupuesto.FieldsClass(GetNestedName("Presupuesto"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public OperandProperty Capital
            {
                get
                {
                    return new OperandProperty(GetNestedName("Capital"));
                }
            }

            public OperandProperty Tasa
            {
                get
                {
                    return new OperandProperty(GetNestedName("Tasa"));
                }
            }

            public OperandProperty Plazo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Plazo"));
                }
            }

            public OperandProperty Cuotas
            {
                get
                {
                    return new OperandProperty(GetNestedName("Cuotas"));
                }
            }

            public OperandProperty TotalImponible
            {
                get
                {
                    return new OperandProperty(GetNestedName("TotalImponible"));
                }
            }

            public OperandProperty GastosAdministrativos
            {
                get
                {
                    return new OperandProperty(GetNestedName("GastosAdministrativos"));
                }
            }
        }

        public new static FieldsClass Fields
        {
            get
            {
                if(ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
            }
        }

        static FieldsClass _Fields;

        public void CalcularVencimientos(DateTime FechaDesembolso, DateTime FechaPrimerVencimiento)
        {
            DateTime fechaActual = FechaPrimerVencimiento;
            bool primero = true;
            int cont = 0;
            foreach (var item in this.Cuotas.OrderBy(o => o.NroCuota))
            {
                if (primero)
                {
                    primero = false;
                    item.FechaVencimiento = fechaActual;

                }
                else
                    item.FechaVencimiento = fechaActual = FechaPrimerVencimiento.AddMonths(cont);
                cont++;
                /*                if (primero) 
                                {
                                    primero = false;
                                    item.FechaVencimiento = fechaActual;
                                    int diasMesActual = DateTime.DaysInMonth(FechaDesembolso.Year,FechaDesembolso.Month);// se debe contar cuantos dias tiene el mes del desembolso
                                    var difDias = (FechaPrimerVencimiento - FechaDesembolso).Days - diasMesActual; //dias desde el plazo predefinido

                                    if (difDias > 0)                    
                                    {
                                        double rate = (double)(item.Prestamo.TasaMensual / 100);
                                        var interesInicial = item.Capital * (decimal)rate;
                                        var interesPorDia = interesInicial / 30; 
                                        item.Interes = Decimal.Round(interesInicial + interesPorDia * difDias);
                                        item.IVA = Decimal.Round(item.Interes / 11);
                                        item.MontoCuota= item.Amortizacion + item.Interes;
                                        item.MontoCuotaSinIVA = item.Amortizacion + item.Interes - item.IVA;
                                    }

                                }
                                else
                                    item.FechaVencimiento = fechaActual = FechaPrimerVencimiento.AddMonths(cont);
                                cont++;*/
            }

            this.Save();
        }

        // Created/Updated: XPS15-RB\rodol on XPS15-RB at 21/5/2019 12:43
    }
}