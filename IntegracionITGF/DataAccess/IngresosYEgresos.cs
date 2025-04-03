using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegracionITGF.DataAccess
{
    public static class IngresosYEgresos
    {
        public static List<INT_CR_PER_ING_EGRESOS> CreateIngresosEgrs(SolicitudPersona SolPersonaOri, Session SessionIT)
        {
            List<INT_CR_PER_ING_EGRESOS> lista = new List<INT_CR_PER_ING_EGRESOS>();
            IngresoConcepto conceptoActual = null;
            INT_CR_PER_ING_EGRESOS ingreso = null;

            foreach (var item in SolPersonaOri.Ingresos.OrderBy(x => x.Concepto))
            {
                if (item.Concepto == null)
                    throw new Exception("Existe un Ingreso sin Concepto");

                if (conceptoActual != item.Concepto)
                {
                    lista.Add(ingreso);
                    conceptoActual = item.Concepto;
                    ingreso = new INT_CR_PER_ING_EGRESOS(SessionIT);

                    //ingreso.COD_PERSONA = "0";
                    ingreso.COD_CONCEPTO = (Byte)item.Concepto.Codigo;
                    ingreso.ING_EGRESO = "I";
                    ingreso.IMPORTE = 0;
                    //ingreso.MTO_SAL_PENDIENTE
                    //ingreso.MTO_EXONERADO
                    ingreso.ID_PERSONA = item.SolicitudPersona.Persona.Oid.ToString().ToUpper();
                    ingreso.OID = Guid.NewGuid().ToString().ToUpper();
                    ingreso.ID_INGRESO = item.Oid.ToString().ToUpper();
                    ingreso.PROCESADO = "N";
                }

                ingreso.IMPORTE += (decimal)item.Monto;
                ingreso.Save();               
            }

            conceptoActual = null;
            ingreso = null;
            foreach (var item in SolPersonaOri.Egresos.Where(e => !e.ACancelar).OrderBy(x => x.Concepto))
            {
                if (item.Concepto == null)
                    throw new Exception("Existe un Egreso sin Concepto");

                if (conceptoActual != item.Concepto)
                {
                    lista.Add(ingreso);
                    conceptoActual = item.Concepto;
                    ingreso = new INT_CR_PER_ING_EGRESOS(SessionIT);

                    //ingreso.COD_PERSONA = "0";
                     ingreso.COD_CONCEPTO = (Byte)item.Concepto.Codigo;
                    ingreso.ING_EGRESO = "E";
                    ingreso.IMPORTE = 0;
                    //ingreso.MTO_SAL_PENDIENTE
                    //ingreso.MTO_EXONERADO
                    ingreso.ID_PERSONA = item.SolicitudPersona.Persona.Oid.ToString().ToUpper();
                    ingreso.OID = Guid.NewGuid().ToString().ToUpper();
                    ingreso.ID_INGRESO = item.Oid.ToString().ToUpper();
                    ingreso.PROCESADO = "N";
                }

                ingreso.IMPORTE += (decimal)item.Monto;
                ingreso.Save();
            }

            return lista;
        }                  
    }
}
