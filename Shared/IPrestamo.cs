using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public interface IPrestamo
    {
        Prestamo GetPrestamo(decimal capital, decimal tasa, int cantCuotas, SistemasPrestamos sistema);

        Prestamo GetPrestamo(decimal capital, decimal tasa, int cantCuotas, SistemasPrestamos sistema, decimal montoCuota);
    }
}
