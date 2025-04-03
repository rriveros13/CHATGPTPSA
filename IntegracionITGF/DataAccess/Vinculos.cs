using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegracionITGF.DataAccess
{
    public static class Vinculos
    {
        public static List<INT_BA_VINCULOS> CreateVinculos(Solicitud Solicitud, Session SessionIT)
        {
            List<INT_BA_VINCULOS> lista = new List<INT_BA_VINCULOS>();
            foreach (var item in Solicitud.Personas.Where(x => x.TipoPersona.Codigo != "SOL"))
            {
                lista.Add(CreateVinculo(item, SessionIT));
            }
            return lista;
        }

        private static INT_BA_VINCULOS CreateVinculo(SolicitudPersona solPersonaOri, Session SessionIT)
        {
            //Pasar la persona
            var per = Personas.CreatePersona(solPersonaOri, SessionIT);
            //IngresosYEgresos.CreateIngresosEgrs(solPersonaOri, SessionIT);

            INT_BA_VINCULOS vinculo = new INT_BA_VINCULOS(SessionIT);

            vinculo.ACTIVO = "S";
            //vinculo.COD_PERFIL
            //vinculo.COD_PERSONA
            //vinculo.COD_PER_VINCULO
            vinculo.ESTADO = "A";
            //vinculo.FEC_VENCIMIENTO
            vinculo.NRO_DOCUMENTO = solPersonaOri.Solicitud.Titular.Documento.Replace(".","").Trim();
            vinculo.NRO_DOC_VINCULO = solPersonaOri.Persona.Documento.Replace(".", "").Trim();
            vinculo.PER_ADMINISTRADOR = "N";
            vinculo.TIPO = solPersonaOri.TipoPersona.CodigoVinculo;
            vinculo.OID = Guid.NewGuid().ToString().ToUpper();
            vinculo.ID_VINCULO = solPersonaOri.Oid.ToString().ToUpper();
            vinculo.PROCESADO = "N";
            vinculo.ID_PER_VINCULO = per.ID_PERSONA;

            vinculo.Save();
            return vinculo;
        }
    }
}
