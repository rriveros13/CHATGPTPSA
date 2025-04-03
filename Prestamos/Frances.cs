using Shared;
using System;
using System.Linq;

namespace Prestamos
{
    public class Frances : IPrestamo
    {
        public Prestamo GetPrestamo(decimal capital, decimal tasa, int cantCuotas, SistemasPrestamos sistema)
        {
            Prestamo p = new Prestamo(capital,tasa,cantCuotas, sistema);
            p.Cuotas = new System.Collections.Generic.List<Cuota>();
            decimal capitalAux = capital;

            //Calculo de la cuota
            double rate = (double)(tasa / 100);
            double factor = (rate + (rate / (Math.Pow(rate + 1, cantCuotas) - 1)));
            double payment = ((double)capital * factor);

            for (int i = 1; i <= cantCuotas; i++)
            {
                
                var montoCuota = Decimal.Round((decimal)payment);
                var interes = Decimal.Round(capitalAux * (decimal)rate);
                var amortizacion = montoCuota - interes;

                if (i == cantCuotas)
                {
                    //Ajuste de la ultima cuota
                    decimal diferencia = capital - (p.Cuotas.Sum(x => x.Amortizacion) + amortizacion);
                    amortizacion += diferencia;
                    montoCuota += diferencia;
                }

                Cuota c = new Cuota(i, capitalAux, montoCuota, amortizacion, interes);               
                p.Cuotas.Add(c);

                //Para siguiente cuota
                capitalAux = c.Capital - c.Amortizacion;
            }
            return p;
        }

        public Prestamo GetPrestamo(decimal capital, decimal tasa, int cantCuotas, SistemasPrestamos sistema, decimal montoCuota)
        {
            throw new NotImplementedException();
        }
    }
}
