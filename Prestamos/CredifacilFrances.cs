using Microsoft.VisualBasic;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestamos
{
    public class CredifacilFrances : IPrestamo
    {
        public Prestamo GetPrestamo(decimal capital, decimal tasa, int cantCuotas, SistemasPrestamos sistema, decimal montoCuotaOriginal)
        {
            decimal diferencia;
            decimal interes;
            decimal amortizacion;
            decimal totalAmortizacion;
            decimal montoCuota = montoCuotaOriginal;

            //Calculo para conseguir el interes a partir del monto de la cuota.
            double rate = Financial.Rate((double)cantCuotas, -(double)montoCuotaOriginal, (double)capital);
            
            //Calculo de la cuota
            Prestamo p = new Prestamo(capital, tasa, cantCuotas, sistema);
            p.Cuotas = new List<Cuota>();
            decimal capitalAux = capital;

            for (int i = 1; i <= cantCuotas; i++)
            {
                interes = Decimal.Round(capitalAux * (decimal)rate);
                amortizacion = montoCuota - interes;

                if (i == cantCuotas)
                {
                    //Ajuste de la ultima cuota

                    totalAmortizacion = (p.Cuotas.Sum(x => x.Amortizacion) + amortizacion);

                    if (totalAmortizacion > capital)
                    {
                        diferencia = (totalAmortizacion - capital);
                        interes += diferencia;
                        amortizacion -= diferencia;
                    } else
                    {
                        diferencia = capital - totalAmortizacion;
                        amortizacion += diferencia;
                        montoCuota += diferencia;
                    }
                }

                Cuota c = new Cuota(i, capitalAux, montoCuota, amortizacion, interes);
                p.Cuotas.Add(c);

                //Para siguiente cuota
                capitalAux = c.Capital - c.Amortizacion;
            }
            return p;
        }

        public Prestamo GetPrestamo(decimal capital, decimal tasa, int cantCuotas, SistemasPrestamos sistema)
        {
            throw new NotImplementedException();
        }
    }
}
