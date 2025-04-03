using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Collections.Generic;

namespace IntegracionITGF.DataAccess
{
    public static class Direcciones
    {
        public static List<INT_BA_DIRECCIONES> CreateDirecciones(XPCollection<PersonaDireccion> DireccionOri, Session SessionIT)
        {
            List<INT_BA_DIRECCIONES> lista = new List<INT_BA_DIRECCIONES>();
            foreach (var item in DireccionOri)
            {
                lista.Add(CreateDireccion(item, SessionIT));
            }
            return lista;
        }

        private static INT_BA_DIRECCIONES CreateDireccion(PersonaDireccion DireccionOri, Session SessionIT)
        {
            INT_BA_DIRECCIONES direccion = new INT_BA_DIRECCIONES(SessionIT);
            //direccion.COD_PERSONA = "0";
            //direccion.COD_DIRECCION = 0;
            if (DireccionOri.Tipo == null)
                throw new Exception("Todas las direcciones deben tener un tipo cargado.");
            else
                direccion.TIPO = DireccionOri.Tipo.Codigo;

            if (DireccionOri.Principal)
                direccion.PRINCIPAL = "S";
            else
                direccion.PRINCIPAL = "N";

            direccion.DIRECCION = DireccionOri.Direccion.Calle;
            direccion.COD_PAIS = Byte.Parse(DireccionOri.Direccion.Departamento.Pais.Codigo);
            direccion.COD_CIUDAD = (Byte)DireccionOri.Direccion.Ciudad.Codigo;

            if (DireccionOri.Direccion.Barrio != null)
                direccion.COD_BARRIO = (Byte)DireccionOri.Direccion.Barrio.Codigo;

            if (DireccionOri.Direccion.Localizacion != null)
                direccion.OBSERVACION = DireccionOri.Direccion.Localizacion.Coordenadas;
            else
                direccion.OBSERVACION = "Sin Geolocalización";

            direccion.NUMERO = DireccionOri.Direccion.Numero;
            direccion.DEPARTAMENTO = DireccionOri.Direccion.Apartamento;
            direccion.PISO = DireccionOri.Direccion.Piso;
            //direccion.EDIFICIO 
            //direccion.COD_POSTAL
            direccion.ID_PERSONA = DireccionOri.Persona.Oid.ToString().ToUpper();
            direccion.OID = Guid.NewGuid().ToString().ToUpper();
            direccion.ID_DIRECCION = DireccionOri.Oid.ToString().ToUpper();
            direccion.PROCESADO = "N";

            direccion.Save();
            return direccion;
        }
    }
}
