using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Collections.Generic;

namespace IntegracionITGF.DataAccess
{
    public static class ReferenciasLaborales
    {
        public static List<INT_CR_REF_PERSONALES> CreateReferenciasLaborales(SolicitudPersona SolPersonaOri, Session SessionIT)
        {
            List<INT_CR_REF_PERSONALES> lista = new List<INT_CR_REF_PERSONALES>();

            foreach (var item in SolPersonaOri.ReferenciasLaborales)
            {
                INT_CR_REF_PERSONALES referencia = new INT_CR_REF_PERSONALES(SessionIT);

                //referencia.CAR_PER_CERTIFICO
                //referencia.CAR_PER_INFORMO
                //referencia.CNF_DAT_LABORALES
                //referencia.CNF_DAT_PARTICULARES
                //referencia.COD_INF_LABORAL
                //referencia.COD_PERSONA
                //referencia.COD_REFERENCIA
                //referencia.COD_USU_VERIFICADOR
                //referencia.CONOCE
                //referencia.EST_VERIFICACION
                //referencia.FEC_INSERCION
                //referencia.FEC_MODIFICACION
                //referencia.FEC_VERIFICACION
                referencia.LUG_TRABAJO = item.LugarTrabajo;
                referencia.NOM_PERSONA = item.NombreCompleto;
                //referencia.NOM_PER_CERTIFICO
                //referencia.NOM_PER_INFORMO
                //referencia.NRO_SECUENCIA
                referencia.OBSERVACION = item.Observacion;
                //referencia.OBS_VERIFICACION
                referencia.PUESTO = item.Puesto;
                //referencia.REC_MENSAJE
                //referencia.REL_PERSONA
                referencia.TELEFONOS = item.Telefono;
                referencia.TIP_REFERENCIA = "L";
                //referencia.USU_INSERCION
                //referencia.USU_MODIFICACION
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
