using Shared;
using System;

namespace Prestamos
{
    public class CuotasDeInteres : IPrestamo
    {
        public Prestamo GetPrestamo(decimal capital, decimal tasa, int cantCuotas, SistemasPrestamos sistema)
        {
            Prestamo p = new Prestamo(capital, tasa, cantCuotas, sistema);
            p.Cuotas = new System.Collections.Generic.List<Cuota>();

            //Calculo de la cuota
            double payment = (double)(capital * (tasa/100));

            for (int i = 1; i < cantCuotas; i++)
            {              
                var montoCuota = (decimal)payment;
                var interes = (decimal)payment;
                Cuota c = new Cuota(i,capital,montoCuota,0,interes);
                p.Cuotas.Add(c);
            }

            //ultima cuota            
            var montoCuota2 = capital + (decimal)payment;
            var interes2 = (decimal)payment;
            Cuota cu = new Cuota(cantCuotas, capital, montoCuota2, capital, interes2);
            p.Cuotas.Add(cu);

            return p;
        }

        public Prestamo GetPrestamo(decimal capital, decimal tasa, int cantCuotas, SistemasPrestamos sistema, decimal montoCuota)
        {
            throw new NotImplementedException();
        }
    }
}
