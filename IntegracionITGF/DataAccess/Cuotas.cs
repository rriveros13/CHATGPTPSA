using System;
using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System.Linq;
using System.Collections.Generic;

namespace IntegracionITGF.DataAccess
{
    public static class Cuotas
    {
        internal static List<INT_PR_SOL_CUOTAS> CreateCuotas(PresupuestoPrestamo prestamo, Session SessionIT)
        {
            List<INT_PR_SOL_CUOTAS> lista = new List<INT_PR_SOL_CUOTAS>();

            foreach (var item in prestamo.Cuotas)
            {
                lista.Add(CreateCuota(item, SessionIT));
            }
            return lista;
        }

        internal static INT_PR_SOL_CUOTAS CreateCuota(PrestamoCuota CuotaOri, Session SessionIT)
        {
            INT_PR_SOL_CUOTAS cuota = new INT_PR_SOL_CUOTAS(SessionIT);
            cuota.OID = Guid.NewGuid().ToString().ToUpper();
            cuota.PROCESADO = "N";
            cuota.ID_SOLICITUD = CuotaOri.Prestamo.Presupuesto.Solicitud.Oid.ToString().ToUpper();
            cuota.ID_CUOTA = CuotaOri.Oid.ToString().ToUpper();
            cuota.NRO_SOLICITUD = 0;
            cuota.NRO_CUOTA = (Byte)CuotaOri.NroCuota;
            cuota.MTO_CAPITAL = Decimal.Round(CuotaOri.Amortizacion);
            cuota.FEC_VTO_EXACTO = CuotaOri.FechaVencimiento;
            cuota.VAL_INTERES = Decimal.Round(CuotaOri.Interes- CuotaOri.IVA);
            cuota.MTO_IVA = Decimal.Round(CuotaOri.IVA);
            cuota.Save();
            return cuota;
        }
    }
}
