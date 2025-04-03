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
    public class PrestamoDetalle : BaseObject
    {
        public PrestamoDetalle(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        PrestamoCabecera prestamo;
        int cuotaNumero;
        decimal montoCuota;
        decimal saldoCuota;
        DateTime fechaVencimiento;
        DateTime fechaPago;
        int diasAtraso;

        [Association("PrestamoDetalle-PrestamoCabecera")]
        public PrestamoCabecera Prestamo
        {
            get => prestamo;
            set => SetPropertyValue(nameof(PrestamoCabecera), ref prestamo, value);
        }

        public int CuotaNumero
        {
            get => cuotaNumero;
            set => SetPropertyValue(nameof(CuotaNumero), ref cuotaNumero, value);
        }

        public decimal MontoCuota
        {
            get => montoCuota;
            set => SetPropertyValue(nameof(MontoCuota), ref montoCuota, value);
        }

        public decimal SaldoCuota
        {
            get => saldoCuota;
            set => SetPropertyValue(nameof(SaldoCuota), ref saldoCuota, value);
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

        public int DiasAtraso
        {
            get => diasAtraso;
            set => SetPropertyValue(nameof(DiasAtraso), ref diasAtraso, value);
        }
    }
}