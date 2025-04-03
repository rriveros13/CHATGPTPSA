using System;
using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System.Linq;

namespace IntegracionITGF.DataAccess
{
    public static class Garantias
    {
        internal static INT_PR_GARANTIAS CreateGarantia(Solicitud solicitudOri, Session SessionIT)
        {
            if (solicitudOri.Inmuebles == null || solicitudOri.Inmuebles.Count() == 0 || solicitudOri.Inmuebles[0].Tasaciones == null)
                throw new Exception("El inmueble no existe o no posee tasaciones");

            InmuebleTasacion ultTasacion = solicitudOri.Inmuebles[0].Tasaciones.Where(x => x.Ultimo).First();

            if (ultTasacion.Inmueble.Direccion == null)
                throw new Exception("El inueble de garantía no tine dirección cargada");

            INT_PR_GARANTIAS garantia = new INT_PR_GARANTIAS(SessionIT);

            //garantia.NRO_GARANTIA = 0;
            garantia.COD_GARANTIA = 3;
            garantia.COD_MONEDA = "GS";
            garantia.VALOR = ultTasacion.TotalInmueble;
            garantia.VAL_TASACION = ultTasacion.TotalInmueble;
            garantia.FEC_ULT_TASACION = ultTasacion.Fecha;

            if (ultTasacion.Inmueble.FechaEscritura == null || ultTasacion.Inmueble.FechaEscritura == DateTime.MinValue)
                throw new Exception("Debe cargar una fecha de escritura al inmueble");
            else
                garantia.FEC_ESCRITURA= ultTasacion.Inmueble.FechaEscritura;

            garantia.FEC_INICIO = solicitudOri.Fecha;

            garantia.FEC_VENCIMIENTO = solicitudOri.Fecha.AddYears(1);
            //garantia.COD_ENT_ESCRIBANIA
            //garantia.COD_ENT_SEGURO
            garantia.TIPO = "A";
            garantia.ESTADO = "C";
            //garantia.COD_PERSONA = "0";

            if (ultTasacion.Inmueble.Direccion.Localizacion != null)
                garantia.OBSERVACION = ultTasacion.Inmueble.Direccion.Localizacion.Coordenadas;
            else
                garantia.OBSERVACION = "Sin Geolocalización";

            garantia.COD_MON_TASACION = "GS";
            garantia.MTO_APLICADO = ultTasacion.TotalInmueble;
            //garantia.EST_TESORERIA
            garantia.EST_VERIFICACION = "N";
            //garantia.FEC_ENT_TESORERIA
            //garantia.FEC_SAL_TESORERIA
            //garantia.COD_USU_ENTREGADo
            //garantia.COD_CAU_SALIDA
            garantia.MTO_APL_VALOR = ultTasacion.TotalInmueble;
            garantia.IND_UTI_TERCERO = "N";
            //garantia.SITUACION
            garantia.COD_OFICINA = 1;
            //garantia.NRO_ESCRITURA
            //garantia.FEC_ULT_UTILIZACION
            //garantia.OBSERVACION_HIS
            //garantia.NRO_REFERENCIA
            garantia.PERFECCIONADA = "N";
            garantia.RANGO = "1";
            //garantia.FEC_CONFIRMACION
            garantia.ID_PERSONA = solicitudOri.Titular.Oid.ToString().ToUpper();       
            garantia.OID = Guid.NewGuid().ToString().ToUpper();
            garantia.NRO_FINCA = ultTasacion.Inmueble.NumeroFinca;
            garantia.COD_DISTRITO = (int) ultTasacion.Inmueble.Direccion.Ciudad.Codigo2;

            garantia.CTA_CATASTRAL = ultTasacion.Inmueble.CuentaCatastral;
            garantia.DIRECCION = ultTasacion.Inmueble.Direccion.CalleNumero;
            garantia.VAL_TERRENO = ultTasacion.TotalTerreno;
            garantia.VAL_EDILICIO = ultTasacion.TotalConstruccion;
            //garantia.NRO_POLIZA
            //garantia.FEC_VTO_SEGURO
            garantia.SERIE = ultTasacion.Inmueble.Serie;
            garantia.NRO_INSCRIPCION = ultTasacion.Inmueble.NumeroInscripcion;
            garantia.NRO_FOLIO = (short)ultTasacion.Inmueble.NumeroFolio;
            garantia.FEC_INSCRIPCION = ultTasacion.Inmueble.FechaInscripcion;
            garantia.VAL_VTA_RAPIDA = ultTasacion.TotalInmueble;
            //garantia.VAL_SEGURO
            if (ultTasacion.Tasador == null)
                throw new Exception("Debe cargar el tasador a la última tasación del inmueble");
            else
                garantia.COD_ENT_TASADOR = (short)ultTasacion.Tasador.Codigo;
            //garantia.FEC_INI_SEGURO
            //garantia.COD_ENT_FIDUCIARIA
            garantia.ID_GARANTIA = solicitudOri.Inmuebles[0].Oid.ToString().ToUpper();
            garantia.PROCESADO = "N";
      

            garantia.Save();
            return garantia;
        }
    }
}
