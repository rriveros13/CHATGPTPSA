using System;

namespace PDNOriginacion.Module.Helpers
{
    public static class Varios
    {
        public static int CalcularEdad(DateTime FechaNacimiento)
        {
            if (FechaNacimiento == null || FechaNacimiento == DateTime.MinValue)
                return 0;

            int age = DateTime.Today.Year - FechaNacimiento.Year;

            if (DateTime.Today.Month < FechaNacimiento.Month || (DateTime.Today.Month == FechaNacimiento.Month && DateTime.Today.Day < FechaNacimiento.Day))
                age--;

            return age;
        }

        public static int CalcularAñosDiferencia(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde == null || fechaDesde == DateTime.MinValue)
                return 0;

            int año = fechaHasta.Year - fechaDesde.Year;

            if (fechaHasta.Month < fechaDesde.Month || (fechaHasta.Month == fechaDesde.Month && fechaHasta.Day < fechaDesde.Day))
                año--;

            return año;
        }

        public static int CalcularMesesDiferencia(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde == null || fechaDesde == DateTime.MinValue)
                return 0;

            int meses = fechaHasta.Month - fechaDesde.Month;
            int dias = fechaHasta.Day - fechaDesde.Day;

            if (meses < 0)
            {
                meses += 12;
            }

            if (dias < 0 && meses != 0)
            {
                meses--;
            }

            return meses;
        }

        public static int CalcularDiasDiferencia(DateTime fechaDesde, DateTime fechaHasta)
        {
            if (fechaDesde == null || fechaDesde == DateTime.MinValue)
                return 0;

            int dias = fechaHasta.Day - fechaDesde.Day;

            if (dias < 0)
            {
                dias += DateTime.DaysInMonth(fechaHasta.Year, fechaHasta.Month);
            }

            return dias;
        }
    }
}
