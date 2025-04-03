using DevExpress.Xpo;
using IntegracionITGF.ITGFDataModel;
using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Linq;

namespace IntegracionITGF.DataAccess
{
    public static class Personas
    {
        public static INT_BA_PERSONAS CreatePersona(SolicitudPersona SolPersonaOri, Session SessionIT)
        {
            INT_BA_PERSONAS persona = new INT_BA_PERSONAS(SessionIT);

            //persona.COD_PERSONA = "0";
            persona.NOM_COMPLETO = SolPersonaOri.Persona.NombreCompleto;
            persona.TIPO = SolPersonaOri.Persona.Tipo.Codigo;

            if (SolPersonaOri.Solicitud.EjecutivoAsignado == null || SolPersonaOri.Solicitud.EjecutivoAsignado.UsuarioExterno == string.Empty)
                throw new Exception("No existe ejecutivo asignado a la solicitud.");
            else
                persona.COD_USU_PROMOTOR = SolPersonaOri.Solicitud.EjecutivoAsignado.UsuarioExterno;

            persona.COD_TIP_DOCUMENTO = byte.Parse(SolPersonaOri.Persona.TipoDocumento.Codigo);
            persona.NRO_DOCUMENTO = SolPersonaOri.Persona.Documento.Replace(".", "").Trim();
            persona.COD_PAIS = byte.Parse(SolPersonaOri.Persona.PaisDocumento.Codigo);

            if (SolPersonaOri.Persona.CorreoParticular == null || SolPersonaOri.Persona.CorreoParticular == string.Empty)
                persona.DIR_CORREO = SolPersonaOri.Persona.CorreoLaboral;
            else
                persona.DIR_CORREO = SolPersonaOri.Persona.CorreoParticular;

           // persona.COD_ACTIVIDAD = null;
            //persona.TOT_INGRESOS = (decimal)SolPersonaOri.TotalIngresos;
            //persona.ING_PRESUNTOS 
            //persona.EGR_PRESUNTOS
            persona.EST_VERIFICACION = "N";
            //persona.NOM_PER_INFORMO
            //persona.OBS_VERIFICACION
            //persona.FEC_VERIFICACION
            //persona.TIP_RELACION 
            //persona.FEC_VTO_DOCUMENTO
            //persona.TIP_IVA
            persona.TAS_PERSONALIZADA = "N";
            persona.RECIBE_SMS = "N";
            //persona.COD_RUB_PRIMARIO = "-";
            //persona.COD_RUB_SECUNDARIO
            //persona.COD_RUB_TERCIARIO
            //persona.COD_GRUPO
            //persona.EMP_NO_GENERADORA
            persona.PRI_NOMBRE = SolPersonaOri.Persona.PrimerNombre;
            persona.PRI_APELLIDO = SolPersonaOri.Persona.PrimerApellido;

            if (SolPersonaOri.Persona.EstadoCivil != null)
                persona.EST_CIVIL = SolPersonaOri.Persona.EstadoCivil.Codigo;

            if (SolPersonaOri.Persona.Genero != null)
                persona.COD_SEXO = SolPersonaOri.Persona.Genero.Codigo;
            else
                persona.COD_SEXO = "M";

            //persona.COD_NIV_ESTUDIO
            //persona.COD_TIP_VIVIENDA
            persona.COD_PAI_NACIONALIDAD= byte.Parse(SolPersonaOri.Persona.PaisDocumento.Codigo);
            persona.SEG_NOMBRE = SolPersonaOri.Persona.SegundoNombre;
            persona.SEG_APELLIDO = SolPersonaOri.Persona.SegundoApellido;
            persona.FEC_NACIMIENTO = SolPersonaOri.Persona.FechaNacimiento;
            //persona.CAN_HIJOS
            persona.COD_PROFESION = byte.Parse(SolPersonaOri.Persona.Profesion.Codigo);

            if (persona.TIPO == "J")
            {
                persona.RAZ_SOCIAL = SolPersonaOri.Persona.NombreCompleto;
                persona.NOM_COMERCIAL = SolPersonaOri.Persona.NombreCompleto;
            }

            //persona.NATURALEZA
            //persona.FEC_APERTURA
            //persona.COD_RAMO
            //persona.GAR_TERCERO
            //persona.CAP_OPERATIVO
            //persona.COD_AREA
            persona.COD_OFICINA = 1;
            //persona.CAT_RIESGO 
            //persona.COD_GRU_ECONOMICO
            persona.EXO_IMPUESTOS = "N";
            //persona.COD_USU_OFICIAL = "-";
            persona.CLI_ESPECIAL = "N";
            persona.RESIDENTE = "S";
            persona.COD_SEC_CONTABLE = "N";
            persona.COD_TIP_DEUDOR = 2;

            if (SolPersonaOri.Persona.Direcciones != null && SolPersonaOri.Persona.Direcciones.Count > 0 && SolPersonaOri.Persona.Direcciones.Where(x => x.Principal).Count() > 0
                    && SolPersonaOri.Persona.Direcciones.Where(x => x.Principal).First().Direccion != null)
                persona.COD_DEPARTAMENTO = (byte)SolPersonaOri.Persona.Direcciones.Where(x => x.Principal).First().Direccion.Departamento.Codigo;

            //persona.NIV_CRUZAMIENTO
            //persona.GE_CLIENTE_TIPO
            //persona.TOT_DEU_REAL
            //persona.TOT_DEU_CONTINGENTE
            //persona.CAT_SUBJETIVA
            //persona.TOT_GAR_COMPUTABLES
            //persona.TOT_PREVISION
            persona.TIP_EXTRACTO = "C";
            //persona.COD_USU_GESTION
            persona.FEC_INGRESO = DateTime.Now;
            //persona.COD_SEG_CLIENTE
            persona.CLI_CONFIDENCIAL = "N";
            //persona.TOT_DEU_SISTEMA
            persona.CAT_AUTOMATICA = 1;
            //persona.CAT_SISTEMA
            //persona.CAT_PONDERADA
            //persona.CAT_GRU_ECONOMICO
            //persona.CAT_ANTERIOR
            //persona.FEC_CAT_ANTERIOR
            //persona.FEC_ULT_CLASIF
            //persona.TOT_DEU_COD_SISTEMA
            //persona.TOT_DEU_CODEUDORIA
            //persona.TOT_DEU_CON_SISTEMA
            //persona.ULT_CAT_RIESGO
            //persona.COD_PERFIL
            //persona.FEC_ULT_ACCESO
            //persona.CAT_PONDERADA
            //persona.COD_CANAL
            //persona.FEC_CAT_SUBJETIVA
            //persona.MOT_CAT_SUBJETIVA
            persona.OID = Guid.NewGuid().ToString().ToUpper();
            persona.PROCESADO = "N";
            persona.ID_PERSONA = SolPersonaOri.Persona.Oid.ToString().ToUpper();

            persona.Save();

            Telefonos.CreateTelefonos(SolPersonaOri.Persona, SessionIT);
            Direcciones.CreateDirecciones(SolPersonaOri.Persona.Direcciones, SessionIT);
            InformacionesLaborales.CreateInfoLaborales(SolPersonaOri.Persona, SessionIT);   

            return persona;
        }
    }
}
