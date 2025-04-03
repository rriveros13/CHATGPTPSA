using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Collections.Generic;

namespace IntegracionITGF.DataAccess
{
    public static class InformacionesLaborales
    {
        public static List<INT_CR_INF_LABORALES> CreateInfoLaborales(Persona Persona, Session SessionIT)
        {
            List<INT_CR_INF_LABORALES> lista = new List<INT_CR_INF_LABORALES>();
            foreach (var item in Persona.Empleos)
            {
                lista.Add(CreateInfoLaboral(item, SessionIT));
            }
            return lista;
        }

        private static INT_CR_INF_LABORALES CreateInfoLaboral(PersonaEmpleo EmpleoOri, Session SessionIT)
        {
            INT_CR_INF_LABORALES empleo = new INT_CR_INF_LABORALES(SessionIT);
            //empleo.COD_PERSONA = "0";
            empleo.OID = Guid.NewGuid().ToString().ToUpper();
            empleo.LUG_TRABAJO = EmpleoOri.Empresa;
            empleo.PROCESADO = "N";
            empleo.FEC_INGRESO = EmpleoOri.FechaIngreso;
            empleo.FEC_SALIDA = EmpleoOri.FechaSalida;
            empleo.MTO_INGRESO = (decimal)EmpleoOri.Salario;
            empleo.COD_MONEDA = "GS";
            empleo.DES_PUESTO = EmpleoOri.Cargo;
            empleo.TIP_CARGO = "E";
            empleo.TIP_INGRESO = 1;

            if (EmpleoOri.Actual)
                empleo.ACTUAL = "S";
            else
                empleo.ACTUAL = "N";

            empleo.OBSERVACION = "Dato del Front";

            empleo.ID_INFO_LABORAL = EmpleoOri.Oid.ToString().ToUpper();
            empleo.ID_PERSONA = EmpleoOri.Persona.Oid.ToString().ToUpper();
            empleo.Save();     
            return empleo;
        }
    }
}
