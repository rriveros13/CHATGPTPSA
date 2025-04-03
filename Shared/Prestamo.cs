using System;
using System.Collections.Generic;
using System.Configuration;

namespace Shared
{
    public class Prestamo
    {
        public string Descripcion{ get; set; }
        public decimal Capital { get; set; }
        public decimal TasaMensual { get; set; }
        public int CantCuotas { get; set; }
        public List<Cuota> Cuotas{ get; set; }
        public static decimal IVA = Decimal.Parse(ConfigurationManager.AppSettings.Get("IVA").ToString());

        public Prestamo(decimal capital, decimal tasaMensual, int cantCuotas, SistemasPrestamos sistema)
        {
            this.Capital = capital;
            this.TasaMensual = tasaMensual;
            this.CantCuotas = cantCuotas;

            if (sistema == SistemasPrestamos.FrancesStandar)
                this.Descripcion = "Francés. ";
            else if (sistema == SistemasPrestamos.CuotasDeInteres)
                this.Descripcion = "Cuotas de Interés. ";
            else if (sistema == SistemasPrestamos.SistemaCF)
                this.Descripcion = "Sistema CF. ";
            else if (sistema == SistemasPrestamos.SistemaFrancesCF)
                this.Descripcion = "Sistema Francés CF.  ";
            else if (sistema == SistemasPrestamos.Aleman)
                this.Descripcion = "Sistema Aleman.  ";


            this.Descripcion += $"Plazo de {this.CantCuotas} cuotas.";
        }
    }

    public enum SistemasPrestamos
    {
        FrancesStandar = 0,
        CuotasDeInteres = 1,
        SistemaCF = 2,
        SistemaFrancesCF = 3,
        Aleman = 4
    }
}
