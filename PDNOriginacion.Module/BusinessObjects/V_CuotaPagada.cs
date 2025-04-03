using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("PrestamoNumero")]
    [Persistent("V_CuotaPagada")]
    [XafDisplayName("Cuota Pagada")]
    public class V_CuotaPagada : XPLiteObject
    {
        public V_CuotaPagada(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        DateTime fechaTransaccion;
        string documento;
        int prestamoNumero;
        int cantidadCuotas;
        int cuotaNumero;
        decimal montoPrestamo;
        decimal montoCuota;
        decimal saldoCuota;
        decimal saldoCapital;
        decimal saldoInteres;
        DateTime fechaVencimiento;
        DateTime fechaPago;
        int diferencia;
        Boolean activo;
        Guid oid;
        Persona persona;
        string enJuicio;

        [Size(30)]
        public string Documento
        {
            get => documento;
            set => SetPropertyValue(nameof(Documento), ref documento, value);
        }

        public int PrestamoNumero
        {
            get => prestamoNumero;
            set => SetPropertyValue(nameof(PrestamoNumero), ref prestamoNumero, value);
        }

        public int CantidadCuotas
        {
            get => cantidadCuotas;
            set => SetPropertyValue(nameof(CantidadCuotas), ref cantidadCuotas, value);
        }

        public int CuotaNumero
        {
            get => cuotaNumero;
            set => SetPropertyValue(nameof(CuotaNumero), ref cuotaNumero, value);
        }

        public DateTime FechaVencimiento
        {
            get => fechaVencimiento;
            set => SetPropertyValue(nameof(FechaVencimiento), ref fechaVencimiento, value);
        }

        public DateTime FechaPago
        {
            get => fechaPago;
            set => SetPropertyValue(nameof(FechaPago), ref fechaPago, value);
        }

        public int Diferencia
        {
            get => diferencia;
            set => SetPropertyValue(nameof(Diferencia), ref diferencia, value);
        }

        public decimal MontoPrestamo
        {
            get => montoPrestamo;
            set => SetPropertyValue(nameof(MontoPrestamo), ref montoPrestamo, value);
        }

        public decimal SaldoCuota
        {
            get => saldoCuota;
            set => SetPropertyValue(nameof(SaldoCuota), ref saldoCuota, value);
        }

        public decimal SaldoCapital
        {
            get => saldoCapital;
            set => SetPropertyValue(nameof(SaldoCapital), ref saldoCapital, value);
        }

        public decimal SaldoInteres
        {
            get => saldoInteres;
            set => SetPropertyValue(nameof(SaldoInteres), ref saldoInteres, value);
        }

        public decimal MontoCuota
        {
            get => montoCuota;
            set => SetPropertyValue(nameof(MontoCuota), ref montoCuota, value);
        }

        public Boolean Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [Association("Persona-CuotaPagada")]
        public Persona Persona
        {
            get => persona;
            set => SetPropertyValue(nameof(Persona), ref persona, value);
        }
        
        public DateTime FechaTransaccion
        {
            get => fechaTransaccion;
            set => SetPropertyValue(nameof(FechaTransaccion), ref fechaTransaccion, value);
        }

        [Key, Persistent, Browsable(false)]
        public Guid Oid
        {
            get => oid;
            set => SetPropertyValue(nameof(Oid), ref oid, value);
        }

        [Size(1)]
        public string EnJuicio
        {
            get => enJuicio;
            set => SetPropertyValue(nameof(EnJuicio), ref enJuicio, value);
        }
    }
}