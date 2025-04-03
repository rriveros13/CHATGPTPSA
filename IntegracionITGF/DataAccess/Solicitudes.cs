using DevExpress.ExpressApp;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Linq;

namespace IntegracionITGF.DataAccess
{
    public static class Solicitudes
    {
        public static void CreateSolicitud(Solicitud SolicitudOri, Session SessionIT)
        {
            var solPersona = SolicitudOri.Personas.Where(x => x.Persona == SolicitudOri.Titular).First();
            bool esHipotecario = SolicitudOri.Producto.CodigoExterno.Equals("67");

            SessionIT.ExplicitBeginTransaction();
            string procborrar = @"PRG_BOR_TAB_INTERFACE";
            SessionIT.ExecuteSprocParametrized(procborrar);
            SessionIT.ExplicitCommitTransaction();

            SessionIT.ExplicitBeginTransaction();
            INT_PR_SOLICITUDES solicitud = new INT_PR_SOLICITUDES(SessionIT);
            Presupuesto pres = SolicitudOri.Presupuestos.Where(x => x.AceptadoCliente).First();
            //solicitud.NRO_SOLICITUD = 0;
            solicitud.DESCRIPCION = SolicitudOri.Oid + " SOLICITUD MIGRADA DEL FRONT";
            solicitud.MONTO = pres.Capital;
            solicitud.TIP_INTERES = "V";
            solicitud.TIP_AMORTIZACION = "P";
            DateTime primeraCuota = pres.Prestamos[0].Cuotas.OrderBy(x => x.NroCuota).First().FechaVencimiento;
            DateTime ultimaCuota = pres.Prestamos[0].Cuotas.OrderByDescending(x => x.NroCuota).First().FechaVencimiento;
            solicitud.DIA_PLAZO = (short)(ultimaCuota - primeraCuota).Days;
            solicitud.FEC_VENCIMIENTO = DateTime.Today;
            solicitud.AJU_ULT_CUOTA = "N";
            solicitud.MTO_INTERES = pres.Prestamos[0].Cuotas.Sum(x => x.Interes);
            solicitud.TIP_AJU_FECHA = "E";
            solicitud.PER_VTO_CAPITAL = 30;
            solicitud.PER_GRACIA = 31;
            //solicitud.COD_PERSONA = "0";
            solicitud.FEC_INICIO = SolicitudOri.FechaADesembolsar;
            if (SolicitudOri.Producto.CodigoTipoMontoExterno != "S")
            {
                solicitud.TIP_MONTO = SolicitudOri.Producto.CodigoTipoMontoExterno;
            }
            else
            {
                if (SolicitudOri.Neto)
                {
                    solicitud.TIP_MONTO = "N";
                    solicitud.MTO_CARGO = 0;
                }
                else
                {
                    solicitud.TIP_MONTO = "B";
                    solicitud.MTO_CARGO = pres.Prestamos[0].GastosAdministrativos.Sum(x => x.Monto);
                }
            }
            
            solicitud.AJU_GRACIA = "N";
            solicitud.TOT_PRESTAMO = pres.Capital;
            solicitud.ESTADO = "L"; 
            solicitud.COD_ORI_FONDO = 1;
            solicitud.COD_MODULO = 6;
            solicitud.COD_MODALIDAD = Byte.Parse(SolicitudOri.Producto.CodigoExterno);
            solicitud.COD_MONEDA = "GS";
            //solicitud.MTO_CARGO = pres.Prestamos[0].GastosAdministrativos.Sum(x => x.Monto);
            solicitud.COD_OFICINA = Byte.Parse(SolicitudOri.CreadaPor.Sucursal.Codigo);
            solicitud.FEC_PRI_VENCIMIENTO = pres.Prestamos[0].Cuotas.OrderBy(o => o.NroCuota).First().FechaVencimiento;
            //solicitud.COD_ENT_SEGURO
            //solicitud.COD_SEC_CREDITO = 701;
            //solicitud.COD_DEPARTAMENTO = 11;
            //solicitud.NRO_SOL_RENOVADA 
            solicitud.TIP_DOCUMENTO = "1";
            solicitud.TIPO = "A";
            solicitud.EXO_IMPUESTO = "N";
            solicitud.TOT_DESEMBOLSAR = pres.Prestamos[0].Neto;
            solicitud.CAN_DESEMBOLSOS = 1;
            //solicitud.NRO_SOL_ORIGINAL
            //solicitud.SAL_PRESTAMO
            //solicitud.CAN_DES_FALTANTES
            //solicitud.COD_ENT_CONCESIONARIA
            //solicitud.NRO_CTA_DEBITO
            //solicitud.DEB_AUTOMATICO
            solicitud.FEC_APROBACION = DateTime.Today;
            //solicitud.COD_USU_APROBACION
            if (SolicitudOri.EjecutivoAsignado == null || SolicitudOri.EjecutivoAsignado.UsuarioExterno == string.Empty)
                throw new Exception("No existe ejecutivo asignado a la solicitud.");
            else
                solicitud.COD_USU_PROMOTOR = SolicitudOri.EjecutivoAsignado.UsuarioExterno;
            //solicitud.USU_INSERCION
            //solicitud.FEC_INSERCION 
            //solicitud.USU_MODIFICACION
            //solicitud.FEC_MODIFICACION
            //solicitud.COD_CANAL
            //solicitud.COD_CAMPANA
            //solicitud.MTO_LIN_APLICADO
            solicitud.TIP_RENOVACION = "A";
            solicitud.TIP_DESTINO = "4";
            solicitud.EST_VERIFICACION = "S";
            solicitud.AUT_PENDIENTE = "N";
            //solicitud.NIV_APROBACION
            //solicitud.NRO_ACTA
            //solicitud.COD_BANCA
            //solicitud.COD_AUTORIZACION
            solicitud.POR_ORI_FONDO = 100;
            solicitud.TOT_DESEMBOLSAR = pres.Prestamos[0].Neto;
            //solicitud.FEC_REN_TASA
            //solicitud.COD_RUB_ECONOMICO
            solicitud.OID = Guid.NewGuid().ToString().ToUpper();
            solicitud.ID_SOLICITUD = SolicitudOri.Oid.ToString().ToUpper();
            solicitud.ID_PERSONA = SolicitudOri.Titular.Oid.ToString().ToUpper();
            solicitud.PROCESADO = "N";
            
            if(esHipotecario && SolicitudOri.Motivo != null && SolicitudOri.Motivo.Codigo != null && SolicitudOri.Motivo.Codigo != string.Empty)
                solicitud.TIP_DES_ESPECIFICO = Byte.Parse(SolicitudOri.Motivo.Codigo);
            else
                solicitud.TIP_DES_ESPECIFICO =7 ;

            solicitud.VAL_TASA = pres.Prestamos[0].Tasa; //se pasa la tasa sin iva
            solicitud.TIP_COBRO = "V";
            solicitud.PER_VENCIMIENTO = 30;
            solicitud.Save();
            
            Personas.CreatePersona(solPersona, SessionIT);
            if (SolicitudOri.Producto.CodigoExterno.Equals("67"))
                Garantias.CreateGarantia(SolicitudOri, SessionIT);

            IngresosYEgresos.CreateIngresosEgrs(SolicitudOri.Personas.Where(x => x.Persona == SolicitudOri.Titular).First(), SessionIT);
            
            ReferenciasLaborales.CreateReferenciasLaborales(SolicitudOri.Personas.Where(x => x.Persona == SolicitudOri.Titular).First(), SessionIT);
            ReferenciasPersonales.CreateReferenciasPersonales(SolicitudOri.Personas.Where(x => x.Persona == SolicitudOri.Titular).First(), SessionIT);
            ReferenciasComerciales.CreateReferenciasComerciales(SolicitudOri.Personas.Where(x => x.Persona == SolicitudOri.Titular).First(), SessionIT);

            if (!SolicitudOri.Neto)
                Cargos.CreateCargos(SolicitudOri.Presupuestos.Where(x => x.AceptadoCliente).First().Prestamos[0],SessionIT);

            Cuotas.CreateCuotas(SolicitudOri.Presupuestos.Where(x => x.AceptadoCliente).First().Prestamos[0], SessionIT);
            SolGarantias.CreateGarantias(SolicitudOri,SessionIT, esHipotecario);
            var vinculos = Vinculos.CreateVinculos(SolicitudOri, SessionIT);
            SessionIT.ExplicitCommitTransaction();

            SessionIT.ExplicitBeginTransaction();

            string proc1 = @"PRG_INS_PER_INTERFACE";

            //procesar los vinculos
            foreach (var item in vinculos)
            {
                SessionIT.ExecuteSprocParametrized(proc1, new SprocParameter("persona_id", item.ID_PER_VINCULO));
            }
           
            var a =SessionIT.ExecuteSprocParametrized(proc1, new SprocParameter("persona_id", solPersona.Persona.Oid.ToString().ToUpper()));

            string proc2 = @"PRG_INS_SOL_INTERFACE";
            var b = SessionIT.ExecuteSprocParametrized(proc2, new SprocParameter("solicitud_id", SolicitudOri.Oid.ToString()));
            SessionIT.ExplicitCommitTransaction();
        }

    }
}
