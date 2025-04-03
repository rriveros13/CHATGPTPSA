using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Collections.Generic;

namespace IntegracionITGF.DataAccess
{
    public static class ReferenciasComerciales
    {
        public static List<INT_CR_REF_COMERCIALES> CreateReferenciasComerciales(SolicitudPersona SolPersonaOri, Session SessionIT)
        {
            List<INT_CR_REF_COMERCIALES> lista = new List<INT_CR_REF_COMERCIALES>();

            foreach (var item in SolPersonaOri.ReferenciasComerciales)
            {
                INT_CR_REF_COMERCIALES referencia = new INT_CR_REF_COMERCIALES(SessionIT);

                if (item.Entidad == null) 
                    throw new Exception("Existe una referencia comercial sin Entidad asociada");

                //referencia.ADM_TARJETA
                referencia.CAN_CUOTAS = (short)item.Plazo;
                //referencia.CAN_PAGADAS
                //referencia.COD_ENTE
                //referencia.COD_MONEDA
                //referencia.COD_PERSONA
                //referencia.COD_REFERENCIA
                //referencia.COD_TIP_CUENTA
                //referencia.COD_USU_VERIFICADOR
                referencia.DES_ENTIDAD = item.Entidad.Nombre;
                //referencia.DES_GARANTIA
                //referencia.DIA_ATRASO
                referencia.EST_VERIFICACION = "N";
                //referencia.FEC_APERTURA
                //referencia.FEC_INSERCION
                //referencia.FEC_MODIFICACION
                //referencia.FEC_VENCIMIENTO
                //referencia.FEC_VERIFICACION
                //referencia.MAX_ATRASO
                //referencia.MTO_LINEA
                //referencia.MTO_MENSUAL
                //referencia.NRO_CUENTA
                //referencia.NRO_SECUENCIA
                referencia.OBSERVACION = item.Observacion;
                //referencia.OBS_VERIFICACION
                //referencia.PRO_ATRASO
                //referencia.SAL_ADMINISTRADORA
                //referencia.SAL_CREDITO
                referencia.TELEFONO = item.Entidad != null ? item.Entidad.Telefonos : "";
                referencia.TIP_REFERENCIA = "C";
                //referencia.USU_INSERCION
                //referencia.USU_MODIFICACION
                referencia.VAL_CREDITO = (decimal)item.MontoSolicitado;
                referencia.VAL_CUOTA = (decimal)item.MontoCuota;
                referencia.ID_PERSONA = item.SolicitudPersona.Persona.Oid.ToString().ToUpper();
                referencia.OID = Guid.NewGuid().ToString().ToUpper();
                referencia.ID_REFERENCIA = item.Oid.ToString().ToUpper();
                referencia.PROCESADO = "N";
                referencia.Save();

                lista.Add(referencia);
            }

            return lista;
        }
    }
}
