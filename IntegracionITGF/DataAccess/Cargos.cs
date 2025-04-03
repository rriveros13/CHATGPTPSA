using System;
using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System.Linq;
using System.Collections.Generic;

namespace IntegracionITGF.DataAccess
{
    public static class Cargos
    {
        internal static List<INT_PR_SOL_CARGOS> CreateCargos(PresupuestoPrestamo prestamo, Session SessionIT)
        {
            List<INT_PR_SOL_CARGOS> lista = new List<INT_PR_SOL_CARGOS>();

            foreach (var item in prestamo.GastosAdministrativos)
            {
                lista.Add(CreateCargo(item, SessionIT));
            }
            return lista;
        }

        internal static INT_PR_SOL_CARGOS CreateCargo(PresupuestoGasto CargoOri, Session SessionIT)
        {
            INT_PR_SOL_CARGOS cargo = new INT_PR_SOL_CARGOS(SessionIT);
            cargo.OID = Guid.NewGuid().ToString().ToUpper();
            cargo.MONTO = Decimal.Truncate(CargoOri.Monto);
            cargo.PROCESADO = "N";
            cargo.MTO_MON_PRESTAMO = CargoOri.PresupuestoPrestamo.Capital;
            cargo.ID_SOLICITUD = CargoOri.Presupuesto.Solicitud.Oid.ToString().ToUpper();
            cargo.ID_CARGO = CargoOri.Oid.ToString().ToUpper();
            //cargo.NRO_SOLICITUD = 0;
            cargo.COD_TRANSACCION = (short)CargoOri.Codigo;
            cargo.COD_MONEDA = "GS";

            if (cargo.COD_TRANSACCION == 701)
            {
                if (CargoOri.Presupuesto.Solicitud.Escribania == null || CargoOri.Presupuesto.Solicitud.Escribania.Count == 0)
                    throw new Exception("No existe ficha de escribanía");

                if(CargoOri.Presupuesto.Solicitud.Escribania[0].Escribania == null)
                    throw new Exception("No existe escribanía asignada en la ficha de escribanía");

                cargo.COD_ENT_SEGURO = (short)Int32.Parse(CargoOri.Presupuesto.Solicitud.Escribania[0].Escribania.CodigoExterno);
            } 

            cargo.Save();
            return cargo;
        }
    }
}
