using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using Newtonsoft.Json;
using PDNOriginacion.Module.BusinessObjects;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;

namespace PDNOriginacion.Module.Helpers
{
    public static class WFHelper
    {
        private static bool InsertarAdjuntos(Solicitud solicitud)
        {
            try
            {
                List<WFAdjunto> adjuntos = solicitud.Producto.Adjuntos
                    .Where(a => a.EstadoDestino == solicitud.Estado)
                    .ToList<WFAdjunto>();

                foreach (WFAdjunto a in adjuntos)
                {
                    Adjunto adjunto = new Adjunto(solicitud.Session)
                    {
                        Descripcion = a.TipoAdjunto?.Descripcion,
                        TipoAdjunto = a.TipoAdjunto,
                        Persona = solicitud.Titular,
                        Solicitud = solicitud,
                        Fecha = DateTime.Now
                        //ObjectSpace   = solicitud.ObjectSpace
                    };
                    adjunto.Save();
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
        private static bool InsertarTareas(Solicitud solicitud)
        {
            try
            {
                List<WFTarea> tareas = solicitud.Producto.Tareas
                    .Where(t => t.EstadoDestino.Oid == solicitud.Estado.Oid)
                    .ToList();

                //seleccionar el usuario para la asignacion de las tareas
                if (solicitud.UsuarioParaTareas == null)
                    solicitud.UsuarioParaTareas = solicitud.ObjectSpace.GetObjectByKey<Usuario>(((Usuario)SecuritySystem.CurrentUser).Oid);
                else
                    solicitud.UsuarioParaTareas = solicitud.ObjectSpace.GetObjectByKey<Usuario>(solicitud.UsuarioParaTareas.Oid); //para que no salga el error de que el objeto es de una sesion distinta

                foreach (WFTarea t in tareas)
                {
                    Tarea tarea = new Tarea(solicitud.Session)
                    {
                        TipoTarea = t,
                        Descripcion = t.Descripcion,
                        Solicitud = solicitud,
                        FechaCreacion = DateTime.Now,
                        Prioridad = PrioridadTarea.Baja
                    };

                    if(tarea.TipoTarea.SLA != null)
                    {
                        tarea.FechaEntrega = AddWithinWorkingHours(tarea.FechaCreacion, (TimeSpan)tarea.TipoTarea.SLA);
                        tarea.RemindIn = new TimeSpan(((TimeSpan)tarea.TipoTarea.SLA).Ticks / 2);
                    }
                    tarea.ObjectSpace = solicitud.ObjectSpace;

                    tarea.Save();

                    var tieneRolDeTarea = solicitud.UsuarioParaTareas.RolesUsuario.Where(r => r.Name == t.RolUsuario.Name).Count() > 0;

                    if(tieneRolDeTarea)
                        InsertHistorialTarea(tarea, solicitud.UsuarioParaTareas, EstadoTarea.Asignada);                   
                }

                solicitud.UsuarioParaTareas = null;

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        private static bool ReactivarTareas(WFTransicion transicion,Solicitud solicitud)
        {
            try
            {
                //Las tareas que debian insertarse en el estado destino y que estan canceladas se van a reactivar.
                var tarReactivar = solicitud.Tareas.Where(t => t.Estado == EstadoTarea.Cancelada && t.TipoTarea.EstadoDestino == solicitud.Estado).ToList();

                foreach (Tarea t in tarReactivar)
                {
                    WFHelper.InsertHistorialTarea(t,
                                              t.ReservadaPor,
                                              EstadoTarea.Asignada);
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        private static void ProcesarLibradoresCheque(Solicitud sol) => throw new NotImplementedException();

        public static DateTime AddWithinWorkingHours(DateTime start, TimeSpan offset)
        {
            const int hoursPerDay = 10;
            const int startHour = 8;

            // Don't start counting hours until start time is during working hours
            if(start.TimeOfDay.TotalHours > startHour + hoursPerDay)
            {
                start = start.Date.AddDays(1).AddHours(startHour);
            }

            if(start.TimeOfDay.TotalHours < startHour)
            {
                start = start.Date.AddHours(startHour);
            }

            switch(start.DayOfWeek)
            {
                case DayOfWeek.Saturday:
                    start = start.AddDays(2);
                    break;
                case DayOfWeek.Sunday:
                    start = start.AddDays(1);
                    break;
            }

            // Calculate how much working time already passed on the first day
            TimeSpan firstDayOffset = start.TimeOfDay.Subtract(TimeSpan.FromHours(startHour));

            // Calculate number of whole days to add
            int wholeDays = (int)(offset.Add(firstDayOffset).TotalHours / hoursPerDay);

            // How many hours off the specified offset does this many whole days consume?
            TimeSpan wholeDaysHours = TimeSpan.FromHours(wholeDays * hoursPerDay);

            // Calculate the final time of day based on the number of whole days spanned and the specified offset
            TimeSpan remainder = offset - wholeDaysHours;

            // How far into the week is the starting date?
            int weekOffset = ((int)(start.DayOfWeek + 7) - (int)DayOfWeek.Monday) % 7;

            // How many weekends are spanned?
            int weekends = (wholeDays + weekOffset) / 5;

            // Calculate the final result using all the above calculated values
            return start.AddDays(wholeDays + weekends * 2).Add(remainder);
        }
        public static Buscame BuscarPersona(string documento)
        {
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            RestClient client = new RestClient($"https://pca.itti.digital/buscame/{documento}");
            RestRequest request = new RestRequest(Method.GET);
            request.AddHeader("cache-control", "no-cache");
            client.Authenticator = new HttpBasicAuthenticator("B10App", "BApp!");

            IRestResponse response = client.Execute(request);

            if(response.IsSuccessful)
            {
                Buscame b = JsonConvert.DeserializeObject<Buscame>(response.Content);
                return b;
            }
            return null;
        }
        public static Retorno CambiarEstado(Solicitud solicitud, WFTransicion transicion)
        {
            Retorno retorno = new Retorno();

            try
            {
                if(solicitud == null)
                {
                    throw new Exception("Solicitud no encontrada");
                }

                if(!transicion.EsReversion && transicion.RolUsuario != null &&
                    ((Usuario)SecuritySystem.CurrentUser).RolesUsuario.All(r => r.Name != transicion.RolUsuario.Name))
                {
                    throw new Exception("Permisos insuficientes para realizar la transición");
                }

                //Finalizar tareas que cumplen criterios y marcadas para finalizar automaticamente. Si es reversion no se finalizan las tareas

                if(!transicion.EsReversion)
                    SolicitarFinTareas(solicitud);

                //Cancelar las tareas pendientes si es que la configuracion del estado lo establece o si se esta yendo al estado anterior
                if (transicion.EstadoDestino.CancelarTareas || transicion.EsReversion)
                {
                    var tareaPendientes = solicitud.Tareas.Where(t => t.Estado != EstadoTarea.Finalizada && t.Estado != EstadoTarea.Cancelada);

                    foreach (var item in tareaPendientes)
                    {
                        WFHelper.InsertHistorialTarea(item,
                                                  item.Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId),
                                                  EstadoTarea.Cancelada);
                    }
                }

                //Verificar que no queden tareas pendientes
                if (solicitud.Tareas.Any(t => t.Estado != EstadoTarea.Finalizada && t.Estado != EstadoTarea.Cancelada))
                {
                    string mensaje = "Debe finalizar todas las tareas:";

                    foreach (var tarea in solicitud.Tareas.Where(x => x.Estado != EstadoTarea.Finalizada && x.Estado != EstadoTarea.Cancelada))
                    {                      
                        if (tarea.TipoTarea == null)
                        {
                            mensaje += "\n - " + tarea.Descripcion;
                        }
                        else
                        {
                            mensaje += "\n " + tarea.TipoTarea.Descripcion.ToUpper() + ":";

                            foreach (var criterio in tarea.TipoTarea.Criterios)
                            {
                                if (!(bool)solicitud.Evaluate(CriteriaOperator.Parse(criterio.Criterio)))
                                    mensaje += "\n - " + criterio.Mensaje;
                            }
                        }
                    }

                    throw new Exception(mensaje);
                }

                if (!transicion.EsReversion)
                {
                    foreach (TransicionCriterio c in transicion.Criterios)
                    {
                        if (!(bool)solicitud.Evaluate(CriteriaOperator.Parse(c.Criterio)))
                        {
                            throw new Exception(c.Mensaje);
                        }
                    }
                }

                if (!transicion.EsReversion)
                {
                    //verificar que exista al menos un seguimiento con fecha futura
                    if (solicitud.Producto.TransicionEvalSeguimientos && (solicitud.Seguimientos == null || solicitud.Seguimientos.Where(x => x.ProximoSegFecha > DateTime.Today).Count() == 0))
                    {
                        if (solicitud.Oid != -1)
                            throw new Exception("Debe existir al menos un seguimiento al solicitante con fecha futura.");
                    }
                }

                XPCollection<RegistroEstadoSolicitud> busquedaEstadoAnterior = new XPCollection<RegistroEstadoSolicitud>(solicitud.Session,
                                                                                                                         CriteriaOperator.Parse("Solicitud.Oid=?",
                                                                                                                                                solicitud.Oid),
                                                                                                                         new SortProperty("Fecha",
                                                                                                                                          SortingDirection.Descending))
                { TopReturnedObjects = 1 };

                if(busquedaEstadoAnterior.Any())
                {
                    RegistroEstadoSolicitud estadoanterior = busquedaEstadoAnterior[0];
                    estadoanterior.TiempoUtilizado = DateTime.Now.Subtract(estadoanterior.Fecha);
                    estadoanterior.Save();
                } 

                //solicitud.Estado = transicion.EstadoDestino;

                RegistroEstadoSolicitud reg = new RegistroEstadoSolicitud(solicitud.Session)
                { Solicitud = solicitud, Transicion = transicion, Fecha = DateTime.Now , EsReversion = transicion.EsReversion};
                reg.Save();

                if (transicion.EsReversion)
                {
                    if (!ReactivarTareas(transicion,solicitud))
                    {
                        throw new Exception("No se reactivaron las tareas en la reversión de estado");
                    }
                }
                else
                {
                    if (!InsertarTareas(solicitud))
                    {
                        throw new Exception("No se pudieron insertar las tareas correspondientes a la transición");
                    }

                    if (!InsertarAdjuntos(solicitud))
                    {
                        throw new Exception("No se pudieron insertar los adjuntos correspondientes a la transición");
                    }

                    if (!InsertarPersonas(solicitud))
                    {
                        throw new Exception("No se pudieron insertar las personas correspondientes a la transición");
                    }
                }

                //solicitud.Save();
            }
            catch(Exception e)
            {
                retorno.Error = true;
                retorno.Mensaje = e.Message;
            }
            return retorno;
        }

        public static void SolicitarFinTareas(Solicitud solicitud)
        {
            foreach (Tarea t in solicitud.Tareas.Where(t => t.Estado != EstadoTarea.Finalizada && t.Estado != EstadoTarea.Cancelada))
            {
                if (t.TipoTarea != null && t.TipoTarea.FinAutomatico)
                {
                    SolicitarFinTarea(t, t.Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId));
                }
            }
        }

        public static void CambiarEstado(Session session, int solicitudOid, string estado)
        {
            if(session == null)
            {
                return;
            }
            //session.BeginTransaction();
            Solicitud sol = session.GetObjectByKey<Solicitud>(solicitudOid);
            EstadoSolicitud e = session.FindObject<EstadoSolicitud>(CriteriaOperator.Parse("Descripcion=?", estado));
            RegistroEstadoSolicitud re = new RegistroEstadoSolicitud(session)
            {
                Solicitud = sol,
                //Estado = e,
                Fecha = DateTime.Now
            };
            if(e != null)
            {
                sol.Estados.Add(re);
            }

            //t.save();
            //session.committransaction();
        }

        public static void CrearTarea(Session session, int solicitudOid, string tipoTarea)
        {
            if(session == null)
            {
                return;
            }

            Tarea t = new Tarea(session);

            t.TipoTarea = session.FindObject<WFTarea>(CriteriaOperator.Parse("Descripcion='" + tipoTarea + "'"));
            t.Descripcion = t.TipoTarea.Descripcion;
            t.Solicitud = session.GetObjectByKey<Solicitud>(solicitudOid);
            t.Prioridad = PrioridadTarea.Baja;
            
            t.Save();
            //session.committransaction();
        }

        public static string GetParametro(this BaseObject o, string value)
        {
            try
            {
                Parametro p = o.Session.FindObject<Parametro>(CriteriaOperator.Parse("Nombre=?", value));
                return p?.Valor;
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException(string.Concat("¡Error al tratar de obtener el valor del parámetro ",
                                                                  value,
                                                                  " - Mensaje: ",
                                                                  ex.Message));
            }
        }

        public static string GetParametro(this XPObject o, string value)
        {
            try
            {
                Parametro p = o.Session.FindObject<Parametro>(CriteriaOperator.Parse("Nombre=?", value));

                if(p != null)
                {
                    return p.Valor;
                }
                else
                {
                    throw new InvalidOperationException(string.Concat("¡Error: parámetro ", value, " no encontrado!!"));
                }
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException(string.Concat("¡Error al tratar de obtener el valor del parámetro ",
                                                                  value,
                                                                  " - Mensaje: ",
                                                                  ex.Message));
            }
        }
        public static List<WFTransicion> GetTrancisionesSolicitud(Solicitud solicitud)
        {
            List<WFTransicion> retorno = null;
            try
            {
                if(solicitud == null)
                {
                    return null;
                }

                retorno = solicitud.Producto.Transiciones
                    .Where(t => t.EstadoOrigen == solicitud.Estado)
                    .ToList<WFTransicion>();

                return retorno;
            }
            catch
            {
                return retorno;
            }
        }
        public static bool InsertarPersonas(Solicitud solicitud)
        {
            try
            {
                List<WFTipoPersona> personas = solicitud.Producto.Personas
                    .Where(a => a.EstadoDestino == solicitud.Estado)
                    .ToList<WFTipoPersona>();

                foreach(WFTipoPersona p in personas)
                {
                    for(int i = 0; i < p.Cantidad; i++)
                    {
                        TipoPersona tipoSolicitante = solicitud.Session
                            .FindObject<TipoPersona>(CriteriaOperator.Parse("Codigo = 'SOL'"));
                        SolicitudPersona solPersona;

                        if (p.TipoPersona != tipoSolicitante)
                        {
                            solPersona = new SolicitudPersona(solicitud.Session)
                            {
                                TipoPersona = p.TipoPersona,
                                Solicitud = solicitud,

                            };

                        }
                        else
                        {
                            solPersona = solicitud.Personas.Where(x => x.TipoPersona == tipoSolicitante).FirstOrDefault();
                        }

                        if (solPersona != null)
                        {
                            foreach (var item in p.Modelos)
                            {
                                Resultado resultado = new Resultado(p.Session);
                                resultado.Modelo = item.Modelo;
                                resultado.Procesado = false;
                                resultado.Solicitud = solicitud;
                                resultado.Apellidos = solPersona.Persona.PrimerApellido + " " + solPersona.Persona.SegundoApellido;
                                resultado.Documento = solPersona.Persona.Documento;
                                resultado.Nombres = solPersona.Persona.PrimerNombre + " " + solPersona.Persona.SegundoNombre;
                                resultado.Persona = solPersona.Persona;

                                resultado.Save();
                            }

                            solPersona.Save();
                        }
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static void InsertHistorialTarea(Tarea tarea, Usuario usuario, EstadoTarea estado)
        {
            if(estado == EstadoTarea.Asignada)
            {
                tarea.ReservadaPor = usuario;
                if (tarea.TipoTarea.RolUsuario == tarea.Solicitud.Producto.RolEjecutivo)
                    tarea.Solicitud.EjecutivoAsignado = usuario;

            }
            else if(estado == EstadoTarea.Disponible)
            {
                tarea.ReservadaPor = null;
            }
            else if(estado == EstadoTarea.Finalizada)
            {
                if (tarea.TipoTarea != null)  //si no viene del WF
                {
                    foreach (TipoTareaCriterio c in tarea.TipoTarea.Criterios)
                    {
                        if (!(bool)tarea.Solicitud.Evaluate(CriteriaOperator.Parse(c.Criterio)))
                        {
                            throw new Exception(c.Mensaje);
                        }
                    }
                }
                tarea.CompletadaPor = usuario;
                tarea.RemindIn = null;
            }

            XPCollection<HistorialTarea> busquedaEstadoAnterior = new XPCollection<HistorialTarea>(tarea.Session,
                                                                                                   CriteriaOperator.Parse("Tarea.Oid=?",
                                                                                                                          tarea.Oid),
                                                                                                   new SortProperty("Fecha",
                                                                                                                    SortingDirection.Descending))
            { TopReturnedObjects = 1 };

            if(busquedaEstadoAnterior.Any())
            {
                HistorialTarea estadoanterior = busquedaEstadoAnterior[0];
                estadoanterior.TiempoUtilizado = DateTime.Now.Subtract(estadoanterior.Fecha);
                estadoanterior.Save();
            }

            HistorialTarea h = new HistorialTarea(tarea.Session) { Tarea = tarea, Usuario = usuario, Estado = estado };
            h.Save();

            tarea.Estado = estado;
            tarea.Save();
        }

        public static void GetSolicitante(Solicitud sol)
        {
            if (string.IsNullOrEmpty(sol.Documento))
            {
                throw new InvalidOperationException("Número de Documento no debe estar vacío");
            }

            if (sol.TipoDocumento == null)
            {
                sol.TipoDocumento = sol.Session.FindObject<TipoDocumento>(CriteriaOperator.Parse("Codigo='CI'"));
            }

            Persona p = Persona.GetOrCreate(sol.Documento, sol.TipoDocumento, null, null, null, null, sol.Session);
            p.Save();

            SolicitudPersona sp = sol.Personas.FirstOrDefault(solp => solp.TipoPersona.Codigo == "SOL");

            if (sp == null)
            {
                sp = new SolicitudPersona(sol.Session)
                {
                    Persona = p,
                    Solicitud = sol,
                    TipoPersona = sol.Session.FindObject<TipoPersona>(CriteriaOperator.Parse("Codigo = 'SOL'"))
                };
            }
            else
            {
                sp.Persona = p;
            }
            sp.Save();
        }

        public static bool ProcesarLibradores(Session session, int solicitudOid)
        {
            if(session == null)
            {
                return false;
            }
            try
            {
                Solicitud sol = session.GetObjectByKey<Solicitud>(solicitudOid);
                //ProcesarLibradoresCheque(sol);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static Retorno SolicitarCambioEstado(Solicitud solicitud)
        {
            Retorno retorno = new Retorno();
            List<WFTransicion> transiciones = GetTrancisionesSolicitud(solicitud);

            WFTransicion transicionCambio = transiciones.Where<WFTransicion>(t => t.Etiqueta == "DESEMBOLSADO").FirstOrDefault();

            if (transicionCambio != null && transicionCambio.Etiqueta == "DESEMBOLSADO")
                return CambiarEstado(solicitud, transicionCambio);

            if (transiciones != null && transiciones.Count() == 1)
            {
                return CambiarEstado(solicitud, transiciones[0]);
            }
            else
            {
                retorno.Error = true;
                retorno.Mensaje = "No se pudo encontrar el estado destino de la transición o no es único";
                return retorno;
            }
        }
        public static void SolicitarFinTarea(Tarea tarea, Usuario usuario)
        {
            foreach(TipoTareaCriterio c in tarea.TipoTarea.Criterios)
            {
                if(!(bool)tarea.Solicitud.Evaluate(CriteriaOperator.Parse(c.Criterio)))
                {
                    return;
                }
            }

            tarea.CompletadaPor = tarea.Session.GetObjectByKey<Usuario>(usuario.Oid);
            tarea.RemindIn = null;

            XPCollection<HistorialTarea> busquedaEstadoAnterior = new XPCollection<HistorialTarea>(tarea.Session,
                                                                                                   CriteriaOperator.Parse("Tarea.Oid=?",
                                                                                                                          tarea.Oid),
                                                                                                   new SortProperty("Fecha",
                                                                                                                    SortingDirection.Descending))
            { TopReturnedObjects = 1 };

            if(busquedaEstadoAnterior.Any())
            {
                HistorialTarea estadoanterior = busquedaEstadoAnterior[0];
                estadoanterior.TiempoUtilizado = DateTime.Now.Subtract(estadoanterior.Fecha);
                estadoanterior.TiempoUtilizadoSegundos = ((TimeSpan)estadoanterior.TiempoUtilizado).Seconds;
                estadoanterior.Save();
            }

            HistorialTarea h = new HistorialTarea(tarea.Session)
            { Tarea = tarea, Usuario = tarea.CompletadaPor, Estado = EstadoTarea.Finalizada, Fecha = DateTime.Now };
            h.Save();

            tarea.Estado = EstadoTarea.Finalizada;
            tarea.Save();
        }
    }

    public class Retorno
    {
        public Retorno()
        {
            Error = false;
            Mensaje = string.Empty;
        }

        public bool Error { get; set; }

        public string Mensaje { get; set; }
    }


}
