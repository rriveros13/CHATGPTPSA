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
using Shared;
using DevExpress.ExpressApp.ConditionalAppearance;

namespace PDNOriginacion.Module.BusinessObjects.NonPersistentClasses
{
    [DefaultClassOptions]
    [NonPersistent]
    public class CalculadoraPrestamo : BaseObject
    {
        public CalculadoraPrestamo(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        InteresMensual interesMensual;
        decimal montoCuota;
        decimal tasaMensual;
        decimal monto;
        int plazo;

        [ImmediatePostData]
        [Appearance("", Enabled = false, Criteria = "InteresMensual != NULL && InteresMensual.CuotaReferencia != 0", Context = nameof(DetailView))]
        public int Plazo
        {
            get => plazo;
            set
            {
                bool cambio = SetPropertyValue(nameof(Plazo), ref plazo, value);
                if (cambio && !IsLoading && !IsSaving && plazo != 0 && monto != 0 && tasaMensual != 0)
                {
                    MontoCuota = this.GenerarMontoCuota(monto, tasaMensual, plazo);
                    OnChanged("MontoCuota");
                }
            }
        }
        [ImmediatePostData]
        public decimal Monto
        {
            get => monto;
            set
            {
                bool cambio = SetPropertyValue(nameof(Monto), ref monto, value);
                if (cambio && !IsLoading && !IsSaving && plazo != 0 && monto != 0 && tasaMensual != 0)
                {
                    MontoCuota = this.GenerarMontoCuota(monto, tasaMensual, plazo);
                    OnChanged("MontoCuota");
                }
            }
        }

        [ImmediatePostData]
        [Appearance("", Enabled = false, Criteria = "InteresMensual != NULL", Context = nameof(DetailView))]
        public decimal TasaMensual
        {
            get => tasaMensual;
            set
            {
                bool cambio = SetPropertyValue(nameof(TasaMensual), ref tasaMensual, value);
                if (cambio && !IsLoading && !IsSaving && plazo != 0 && monto != 0 && tasaMensual != 0)
                {
                    MontoCuota = this.GenerarMontoCuota(monto, tasaMensual, plazo);
                    OnChanged("MontoCuota");
                }
            }
        }

        public decimal MontoCuota
        {
            get => montoCuota;
            set => SetPropertyValue(nameof(MontoCuota), ref montoCuota, value);
        }

        [ImmediatePostData]
        public InteresMensual InteresMensual
        {
            get => interesMensual;
            set
            {
                bool cambio = SetPropertyValue(nameof(InteresMensual), ref interesMensual, value);
                if (cambio && !IsLoading && !IsSaving && interesMensual != null)
                {
                    TasaMensual = interesMensual.PorcentajeInteres;
                    OnChanged("TasaMensual");

                    if (interesMensual.CuotaReferencia != 0)
                    {
                        Plazo = interesMensual.CuotaReferencia;
                        OnChanged("Plazo");
                    }
                }
            }
        }

        private decimal GenerarMontoCuota(decimal capital, decimal tasa, int cantCuotas)
        {
            decimal interesOriginal = (capital * tasa) / 100;
            decimal montoCuotaOriginal = Decimal.Round(((interesOriginal * cantCuotas) + capital) / cantCuotas);
            return montoCuotaOriginal;
        }
    }
}