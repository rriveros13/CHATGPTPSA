using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Prestamos
{
    public class Credifacil : IPrestamo
    {
        public Prestamo GetPrestamo(decimal capital, decimal tasa, int cantCuotas, SistemasPrestamos sistema)
        {
            Prestamo p = new Prestamo(capital, tasa, cantCuotas, sistema);
            p.Cuotas = new System.Collections.Generic.List<Cuota>();
            decimal capitalAux = capital;

            //Calculo de la cuota
            double interes = (double)((capital * tasa) / 100);
            double payment = ((interes * cantCuotas) + (double)capital) / cantCuotas;

            for (int i = 1; i <= cantCuotas; i++)
            {
                var montoCuota = Decimal.Round((decimal)payment);
                var montoInteres = Decimal.Round((decimal)interes);
                var amortizacion = montoCuota - montoInteres;

                if (i == cantCuotas)
                {
                    //Ajuste de la ultima cuota
                    decimal diferencia = capital - (p.Cuotas.Sum(x => x.Amortizacion) + amortizacion);
                    amortizacion += diferencia;
                    montoCuota += diferencia;
                }

                Cuota c = new Cuota(i, capitalAux, montoCuota, amortizacion, montoInteres);
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
