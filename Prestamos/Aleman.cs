using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestamos
{
    public class Aleman : IPrestamo
    {
        public Prestamo GetPrestamo(decimal capital, decimal tasa, int cantCuotas, SistemasPrestamos sistema)
        {
            Prestamo p = new Prestamo(capital, tasa, cantCuotas, sistema);
            p.Cuotas = new List<Cuota>();
            decimal capitalAux = capital;
            decimal amortizacion = capital / cantCuotas;
            decimal rate = tasa / 100;

            for (int i = 1; i <= cantCuotas; i++)
            {
                decimal interes = Decimal.Round(capitalAux * (decimal)rate);
                decimal montoCuota = amortizacion + interes;

                Cuota c = new Cuota(i, capitalAux, montoCuota, amortizacion, interes);
                p.Cuotas.Add(c);

                //Para siguiente cuota
                capitalAux = capitalAux - amortizacion;
            }

            return p;
        }

        public Prestamo GetPrestamo(decimal capital, decimal tasa, int cantCuotas, SistemasPrestamos sistema, decimal montoCuota)
        {
            throw new NotImplementedException();
        }
    }
}
