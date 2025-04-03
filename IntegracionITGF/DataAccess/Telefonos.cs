using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Collections.Generic;

namespace IntegracionITGF.DataAccess
{
    public static class Telefonos
    {
        public static List<INT_BA_TELEFONOS> CreateTelefonos(Persona Persona, Session SessionIT)
        {
            List<INT_BA_TELEFONOS> lista = new List<INT_BA_TELEFONOS>();
            foreach (var item in Persona.Telefonos)
            {
                lista.Add(CreateTelefono(Persona, item, SessionIT));
            }
            return lista;
        }

        private static INT_BA_TELEFONOS CreateTelefono(Persona persona, Telefono TelefonoOri, Session SessionIT)
        {
            INT_BA_TELEFONOS telefono = new INT_BA_TELEFONOS(SessionIT);

            //telefono.COD_PERSONA = "0";
            telefono.TIPO = TelefonoOri.TipoTelefono.Codigo;
            telefono.TIP_LINEA = TelefonoOri.Tipo.Codigo;
            telefono.TELEFONO = TelefonoOri.Numero;

            if (TelefonoOri.Preferido)
                telefono.PRINCIPAL = "S";
            else
                telefono.PRINCIPAL = "N";

            telefono.AREA = TelefonoOri.Prefijo.Codigo;
            //telefono.INTERNO
            telefono.OBSERVACION = "Dato del Front";
            //telefono.COD_TELEFONO = 0;
            //telefono.SMS_HABILITADO
            telefono.ID_PERSONA = persona.Oid.ToString().ToUpper();
            telefono.OID = Guid.NewGuid().ToString().ToUpper();
            telefono.ID_TELEFONO = TelefonoOri.Oid.ToString().ToUpper();
            telefono.PROCESADO = "N";
  
            telefono.Save();
            return telefono;
        }
    }
}
