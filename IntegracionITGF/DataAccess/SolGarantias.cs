using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IntegracionITGF.DataAccess
{
    public class SolGarantias
    {
        internal static List<INT_PR_SOL_GARANTIAS> CreateGarantias(Solicitud SolicitudOri, Session SessionIT, bool Hipotecario)
        {
            List<INT_PR_SOL_GARANTIAS> lista = new List<INT_PR_SOL_GARANTIAS>();
            lista.Add(CreateGarantiaInmueble(SolicitudOri, SessionIT,Hipotecario));

            foreach (var item in SolicitudOri.Personas.Where(x => x.TipoPersona.Codigo != "SOL"))
            {
                lista.Add(CreateGarantiaCodeudor(item, SessionIT));
            }
            return lista;
        }

        internal static INT_PR_SOL_GARANTIAS CreateGarantiaInmueble(Solicitud SolicitudOri, Session SessionIT, bool Hipotecario)
        {
            if ((SolicitudOri.Inmuebles == null || SolicitudOri.Inmuebles.Count() == 0) && Hipotecario)
                throw new Exception("La solicitud no posee inmuebles");

            INT_PR_SOL_GARANTIAS garantia = new INT_PR_SOL_GARANTIAS(SessionIT);
            garantia.OID = Guid.NewGuid().ToString().ToUpper();
            garantia.PROCESADO = "N";
            garantia.ID_SOLICITUD = SolicitudOri.Oid.ToString().ToUpper();
            if (Hipotecario)
                garantia.ID_GARANTIA = SolicitudOri.Inmuebles[0].Oid.ToString().ToUpper();
            garantia.NRO_SOLICITUD = 0;

            if (Hipotecario)
                garantia.COD_GARANTIA = 3;
            else
                garantia.COD_GARANTIA = 1;

            garantia.MTO_APLICADO = SolicitudOri.MontoPresupuesto;

            garantia.Save();
            return garantia;
        }

        internal static INT_PR_SOL_GARANTIAS CreateGarantiaCodeudor(SolicitudPersona SolPersonaOri, Session SessionIT)
        {
            if (SolPersonaOri.Persona == null)
                throw new Exception("No existe persona codeudora asignada");

            INT_PR_SOL_GARANTIAS garantia = new INT_PR_SOL_GARANTIAS(SessionIT);
            garantia.OID = Guid.NewGuid().ToString().ToUpper();
            garantia.PROCESADO = "N";
            garantia.ID_SOLICITUD = SolPersonaOri.Solicitud.Oid.ToString().ToUpper();
            garantia.ID_GARANTIA = SolPersonaOri.Oid.ToString().ToUpper();
            garantia.NRO_SOLICITUD = 0;
            garantia.COD_GARANTIA = 2;
            garantia.MTO_APLICADO = 0;
            garantia.ID_PERSONA = SolPersonaOri.Persona.Oid.ToString().ToUpper();

            garantia.Save();
            return garantia;
        }
    }
}
