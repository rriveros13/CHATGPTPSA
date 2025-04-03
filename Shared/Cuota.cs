using System;

namespace Shared
{
    public class Cuota
    {
        public Cuota(int nroCuota, decimal capital, decimal montoCuota, decimal amortizacion, decimal interes)
        {
            this.NroCuota = nroCuota;
            this.Capital = capital;
            this.MontoCuota = montoCuota;
            this.Amortizacion = amortizacion;
            this.Interes = interes;
            this.IVA = Decimal.Round(this.Interes / 11);
            this.MontoCuotaSinIVA = this.Amortizacion + this.Interes - this.IVA;//this.MontoCuota - this.IVA;
        }

        public int NroCuota { get; set; }
        public decimal Capital{ get; set; }
        public decimal MontoCuota { get; set; }
        public decimal Amortizacion{ get; set; }
        public decimal Interes{ get; set; }
        public decimal MontoCuotaSinIVA { get; set; }
        public decimal IVA { get; set; }
    }
}
