using DevExpress.Data.Filtering;
using DevExpress.DataAccess.UI.XPObjectSource;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Utils.Extensions;
using DevExpress.Xpo;
using Fasterflect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("PrestamoNumero")]
    [XafDisplayName("Prestamos")]
    public class PrestamoCabecera : BaseObject
    {
        public PrestamoCabecera(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        string producto;
        decimal saldoInteres;
        decimal saldoCapital;
        decimal saldoCuotas;
        Persona titular;
        Persona codeudor;
        Persona conyuge;
        int prestamoNumero;
        decimal montoPrestamo;
        int cantidadCuotas;
        DateTime fechaTransaccion;
        DateTime fechaVencimiento;
        Estado estado;
        Judicial judicial;
        int promedioAtraso;

        [ModelDefault("AllowEdit", "false")]
        [Association("Persona-PrestamoCabecera")]
        public Persona Titular
        {
            get => titular;
            set => SetPropertyValue(nameof(Titular), ref titular, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [Association("Persona-PrestamoCodeudor")]
        public Persona Codeudor
        {
            get => codeudor;
            set => SetPropertyValue(nameof(Codeudor), ref codeudor, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public Persona Conyuge
        {
            get => conyuge;
            set => SetPropertyValue(nameof(Conyuge), ref conyuge, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public int PrestamoNumero
        {
            get => prestamoNumero;
            set => SetPropertyValue(nameof(PrestamoNumero), ref prestamoNumero, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public decimal MontoPrestamo
        {
            get => montoPrestamo;
            set => SetPropertyValue(nameof(MontoPrestamo), ref montoPrestamo, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public decimal SaldoCapital
        {
            get => saldoCapital;
            set => SetPropertyValue(nameof(SaldoCapital), ref saldoCapital, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public decimal SaldoInteres
        {
            get => saldoInteres;
            set => SetPropertyValue(nameof(SaldoInteres), ref saldoInteres, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public decimal SaldoCuotas
        {
            get => saldoCuotas;
            set => SetPropertyValue(nameof(SaldoCuotas), ref saldoCuotas, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public int CantidadCuotas
        {
            get => cantidadCuotas;
            set => SetPropertyValue(nameof(CantidadCuotas), ref cantidadCuotas, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public DateTime FechaTransaccion
        {
            get => fechaTransaccion;
            set => SetPropertyValue(nameof(FechaTransaccion), ref fechaTransaccion, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public DateTime FechaVencimiento
        {
            get => fechaVencimiento;
            set => SetPropertyValue(nameof(FechaVencimiento), ref fechaVencimiento, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public Estado Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public Judicial Judicial
        {
            get => judicial;
            set => SetPropertyValue(nameof(Judicial), ref judicial, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public int PromedioAtraso
        {
            get => promedioAtraso;
            set => SetPropertyValue(nameof(PromedioAtraso), ref promedioAtraso, value);
        }

        
        [Size(50)]
        public string Producto
        {
            get => producto;
            set => SetPropertyValue(nameof(Producto), ref producto, value);
        }

        [Association("PrestamoDetalle-PrestamoCabecera")]
        public XPCollection<PrestamoDetalle> Cuotas => GetCollection<PrestamoDetalle>(nameof(Cuotas));

        [PersistentAlias("Cuotas[FechaPago = ^.Cuotas.Max(FechaPago)].Max(FechaPago)")]
        public DateTime? FechaUltimoPago => EvaluateAlias(nameof(FechaUltimoPago)) as DateTime?;

        [PersistentAlias("Iif(!IsNull(FechaUltimoPago), Cuotas[FechaPago = ^.Cuotas.Max(FechaPago)].Max(CuotaNumero), null)")]
        public int? UltimaCuotaPagada => EvaluateAlias(nameof(UltimaCuotaPagada)) as int?;
    }
}

namespace PDNOriginacion.Module.BusinessObjects
{
    public enum Estado
    {
        Activo,
        Cancelado
    }
}

namespace PDNOriginacion.Module.BusinessObjects
{
    public enum Judicial
    {
        Si,
        No
    }
}