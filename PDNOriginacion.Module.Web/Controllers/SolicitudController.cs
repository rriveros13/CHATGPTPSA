using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.ReportsV2;
using DevExpress.ExpressApp.Web;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Spreadsheet;
using DevExpress.Xpo;
using IntegracionITGF.DataAccess;
using PDNOriginacion.Module.BusinessObjects;
using PDNOriginacion.Module.BusinessObjects.NonPersistentClasses;
using PDNOriginacion.Module.Helpers;
using PDNOriginacion.Module.PDNService;
using PDNOriginacion.Module.Web.Controllers.NonPersistentClasses;
using PDNOriginacion.Module.Web.Helpers;
using Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Xml;

#pragma warning disable 252,253

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class SolicitudController : ViewController
    {
        public SolicitudController()
        {
            InitializeComponent();
            Transiciones.ItemType = SingleChoiceActionItemType.ItemIsOperation;

        }

        private void AgregarTransiciones()
        {
            Transiciones.Items.Clear();
            Transiciones.Caption = string.Empty;
            Transiciones.Active.SetItemValue(string.Empty, true);
            Solicitud sol = (Solicitud)View.CurrentObject;
            List<WFTransicion> transiciones = WFHelper.GetTrancisionesSolicitud(sol);
            int contEstados = 0;

            if (transiciones != null && transiciones.Any())
            {
                transiciones = transiciones.OrderBy(x => x.Orden).ToList<WFTransicion>();

                foreach (WFTransicion t in transiciones)
                {
                    bool cumpleTodos = true;

                    if (t.RolUsuario != null &&
                        ((Usuario)SecuritySystem.CurrentUser).RolesUsuario.All(r => r.Name != t.RolUsuario.Name))
                    {
                        cumpleTodos = false;
                    }
                    else
                    {
                        if (sol.Producto.TransicionEvalCriterios)
                        {
                            foreach (TransicionCriterio item in t.Criterios)
                            {
                                cumpleTodos = (bool)sol.Evaluate(item.Criterio);
                                if (!cumpleTodos)
                                {
                                    break;
                                }
                            }
                        }
                    }

                    if (cumpleTodos)
                    {
                        if (Transiciones.Caption == string.Empty)
                        {
                            Transiciones.Caption = t.Etiqueta;
                        }

                        Transiciones.Items.Add(new ChoiceActionItem(t.Etiqueta, t));
                        contEstados++;
                    }
                }
            }

            //Agregar transición a estado anterior si el usuario tiene el rol y si el estado actual no es reversado.
            bool tieneRolEstAnterior = ((Usuario)SecuritySystem.CurrentUser).RolesUsuario.Where(r => r.Name == "PCA_TransicionEstAnterior").Count() > 0;
            if (sol != null && tieneRolEstAnterior && sol.EstadoActual != null && !sol.EstadoActual.EsReversion)
            {
                if (sol.Estados.Count > 1)
                {
                    var tran = (sol.Estados.OrderByDescending(x => x.Fecha).ToList())[1].Transicion;
                    tran.EsReversion = true;
                    var item = new ChoiceActionItem("<< " + tran.EstadoDestino.Descripcion, tran);
                    Transiciones.Items.Add(item);
                    contEstados++;
                    if (Transiciones.Caption == string.Empty)
                        Transiciones.Caption = "<< " + tran.EstadoDestino.Descripcion;
                }
            }

            if (contEstados == 0)
            {
                Transiciones.Active.SetItemValue(string.Empty, false);
            }
        }

        private string AjustarXmlPDN(Resultado solResultado)
        {
            string xml = solResultado.Modelo.XmlInput;
            xml = xml.Replace("{fecha_datos}", DateTime.Today.ToString("dd/MM/yyyy"));
            xml = xml.Replace("{tipo_doc}", solResultado.Persona.TipoDocumento.Codigo);
            xml = xml.Replace("{pais_doc}", solResultado.Persona.PaisDocumento.Codigo);
            xml = xml.Replace("{documento}", solResultado.Persona.Documento);
            xml = xml.Replace("{nacionalidad}", solResultado.Persona.PaisDocumento.Codigo);
            xml = xml.Replace("{salario}", solResultado.Persona.Salario.ToString("F0"));
            xml = xml.Replace("{monto}", solResultado.Solicitud.Monto.ToString("F0"));
            xml = xml.Replace("{plazo}", solResultado.Solicitud.Plazo.ToString("F0"));
            xml = xml.Replace("{antiguedad}", solResultado.Persona.Antiguedad.ToString("F0"));
            xml = xml.Replace("{edad}", solResultado.Solicitud.Edad.ToString("F0"));
            xml = xml.Replace("{promedio_atraso}", solResultado.Solicitud.SolicitudPersonaTitular.PromedioAtraso.ToString("F0"));
            xml = xml.Replace("{faja}", solResultado.Solicitud.Faja);

            /*foreach (CampoProducto c in solicitudPersona.Solicitud.Producto.Campos)
            {
            if (!xml.Contains("{" + c.Campo.Nombre + "}")) continue;
            PropertyInfo info  = (solicitudPersona.Solicitud.GetType()).GetProperty(c.Campo.Nombre);
            string       valor = info?.GetValue(solicitudPersona.Solicitud, null).ToString();
            
            var infoCampo = (solicitudPersona?.Solicitud.GetType()).GetProperty(c?.Campo?.Nombre)?.PropertyType;
            if (infoCampo == typeof(decimal))
            {
            var dec                = info?.GetValue(solicitudPersona?.Solicitud, null);
            if (dec != null) valor = ((decimal) dec).ToString("#");
            }
            xml = xml.Replace("{" + c.Campo.Nombre + "}", valor);
            } */
            return xml.Replace(" />", "/>");
        }

        private void DatosPersona_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Solicitud sol = (Solicitud)View.CurrentObject;
            WFHelper.GetSolicitante(sol);
            //sol.Save();
        }

        private void generarPresupuesto_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            generarNuevoPresupuesto(false);
        }

        private void generarPropuestaComercial_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            generarNuevoPresupuesto(true);
        }

        private void generarNuevoPresupuesto(bool esPropuesta)
        {
            //Solicitud sol = (Solicitud)View.CurrentObject;
            IObjectSpace objectSpace = this.Application.CreateObjectSpace(typeof(Solicitud));
            Solicitud sol = objectSpace.FindObject<Solicitud>(CriteriaOperator.Parse("Oid=?", ((Solicitud)View.CurrentObject).Oid));
            Presupuesto.CrearPresupuesto(sol, esPropuesta);
            sol.ObjectSpace.CommitChanges();
            View.Refresh();
        }

        private void ImportarCheques_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            SeleccionarArchivoExcel obj = os.CreateObject<SeleccionarArchivoExcel>();
            DetailView dv = Application.CreateDetailView(os, obj, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.View = dv;
        }

        private void ImportarCheques_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            Solicitud sol = (Solicitud)View.CurrentObject;
            Application.CreateObjectSpace();
            SeleccionarArchivoExcel selArchivo = (SeleccionarArchivoExcel)e.PopupWindowViewCurrentObject;

            if (selArchivo.Archivo == null)
            {
                return;
            }

            try
            {
                MemoryStream ms = new MemoryStream();

                selArchivo.Archivo.SaveToStream(ms);
                ms.Position = 0;

                Workbook workbook = new Workbook();
                workbook.LoadDocument(ms);

                sol.Session.BeginTransaction();

                RowCollection rows = workbook.Worksheets[0].Rows;
                int r = 1;
                Row row = rows[r];

                while (row[0].Value.ToString() != string.Empty)
                {
                    string documento = row["A"].Value.ToString();
                    string nombres = row["B"].Value.ToString();
                    string apellidos = row["C"].Value.ToString();
                    string cuenta = row["D"].Value.ToString();
                    string serie = row["E"].Value.ToString();
                    string nroCheque = row["F"].Value.ToString();
                    string banco = row["G"].Value.ToString();
                    decimal importe = Convert.ToDecimal(row["H"].Value.ToString());
                    DateTime fechae = Convert.ToDateTime(row["I"].Value.ToString());
                    DateTime fechav = Convert.ToDateTime(row["J"].Value.ToString());

                    Cheque cheque = new Cheque(sol.Session);

                    Persona p = sol.Session.FindObject<Persona>(CriteriaOperator.Parse("Documento=?", documento));
                    if (p == null)
                    {
                        Persona newP = new Persona(sol.Session)
                        { Documento = documento, PrimerNombre = Persona.GetPalabraSeparada(nombres, true),
                            SegundoNombre = Persona.GetPalabraSeparada(nombres, false),
                            PrimerApellido = Persona.GetPalabraSeparada(apellidos, true),
                            SegundoApellido = Persona.GetPalabraSeparada(apellidos, false) };
                        newP.Save();
                        p = newP;
                    }

                    Entidad dBBanco = new Entidad(sol.Session);
                    cheque.Entidad = dBBanco.GetOrCreate(sol.Session, banco);
                    cheque.Librador = p;
                    cheque.Monto = importe;
                    cheque.Cuenta = cuenta;
                    cheque.Numero = nroCheque;
                    cheque.Serie = serie;
                    cheque.FechaEmision = fechae;
                    cheque.FechaVencimiento = fechav;
                    cheque.Save();
                    //sol.Cheques.Add(cheque);

                    r++;
                    row = rows[r];
                }
                sol.Session.CommitTransaction();
            }
            catch
            {
                if (sol.Session.InTransaction)
                {
                    sol.Session.RollbackTransaction();
                }
                throw;
            }
        }

        private void ImportarPersonas_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            SeleccionarArchivoExcel obj = os.CreateObject<SeleccionarArchivoExcel>();
            DetailView dv = Application.CreateDetailView(os, obj, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.View = dv;
        }

        private void ImportarPersonas_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            Solicitud sol = (Solicitud)View.CurrentObject;
            Application.CreateObjectSpace();
            SeleccionarArchivoExcel selArchivo = (SeleccionarArchivoExcel)e.PopupWindowViewCurrentObject;

            if (selArchivo.Archivo == null)
            {
                return;
            }

            try
            {
                MemoryStream ms = new MemoryStream();

                selArchivo.Archivo.SaveToStream(ms);
                ms.Position = 0;

                Workbook workbook = new Workbook();
                workbook.LoadDocument(ms);

                //sol.Session.BeginTransaction();

                RowCollection rows = workbook.Worksheets[0].Rows;
                int r = 1;
                Row row = rows[r];

                while (row[0].Value.ToString() != string.Empty)
                {
                    string documento = row["A"].Value.ToString();
                    decimal ingreso = Convert.ToDecimal(row["B"].Value.ToString());

                    TipoDocumento d = sol.Session.FindObject<TipoDocumento>(CriteriaOperator.Parse("Codigo=?", "CI"));
                    Persona p = new Persona(sol.Session);

                    SolicitudPersona sp = new SolicitudPersona(sol.Session) { Persona = p.GetOrCreate(documento, d) };
                    sp.Persona.Salario = ingreso;
                    sp.Solicitud = sol;
                    sp.TipoPersona = sol.Session.FindObject<TipoPersona>(CriteriaOperator.Parse("Codigo = 'MOT'"));

                    WFTipoPersona confPersona = (sol.Producto.Personas
                        .Where(wftp => wftp.Cantidad == 0 && wftp.TipoPersona == sp.TipoPersona)
                        .ToList())[0];

                    if (confPersona != null)
                    {
                        //TODO sp.ProcesarMotor = confPersona.ProcesarMotor;
                        //TODO sp.Modelo = confPersona.Modelo;
                    }
                    sp.Save();

                    r++;
                    row = rows[r];
                }
                sol.ObjectSpace.CommitChanges();
            }
            catch
            {
                if (sol.Session.InTransaction)
                {
                    sol.Session.RollbackTransaction();
                }
                throw;
            }
        }

        private void ObjectSpace_ModifiedChanged(object sender, EventArgs e) => UpdateActionState();

        private void AgregarInmuebles_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            var sol = (Solicitud)View.CurrentObject;
            IObjectSpace os = Application.CreateObjectSpace();
            InmueblesDisponibles obj = os.CreateObject<InmueblesDisponibles>();
            obj.Solicitud = os.GetObjectByKey<Solicitud>(sol.Oid);
            DetailView dv = Application.CreateDetailView(os, obj, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.View = dv;
        }

        private void AgregarInmuebles_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            InmueblesDisponibles inmuebles = (InmueblesDisponibles)e.PopupWindowViewCurrentObject;

            var sol = (Solicitud)View.CurrentObject;
            sol.ObjectSpace.CommitChanges();
            sol.ObjectSpace.Refresh();
        }

        private string ProcesarMotor(Resultado solResultado)
        {
            string xmlError = @"<?xml version='1.0' encoding='ISO-8859-1' ?><experto><CodError/><Error/></experto>";
            string xmlString = AjustarXmlPDN(solResultado);

            string resultado = string.Empty;
            XmlDocument doc = new XmlDocument();
            XmlDocument docResultado = new XmlDocument();
            XmlDocument docError = new XmlDocument();

            doc.LoadXml(xmlString);
            docError.LoadXml(xmlError);

            ExpertoClient client;

            string xsUser = ConfigurationManager.AppSettings["PdnUser"];
            string xsPass = ConfigurationManager.AppSettings["PdnPass"];
            string xsPdnURI = ConfigurationManager.AppSettings["PdnURI"];

            client = new ExpertoClient();
            client.Endpoint.Address = new EndpointAddress(xsPdnURI);

            try
            {
                resultado = client.PDN_ConsultaExperto(xsUser,
                                                xsPass,
                                                solResultado.Solicitud.Producto.TipoProducto.Descripcion,
                                                solResultado.Modelo.Codigo,
                                                xmlString,
                                                "N");
                client.Close();
            }
            catch (TimeoutException e)
            {
                ErrorHandling.Instance.SetPageError(e);

                XmlNode noderror = docError.SelectSingleNode("/experto");
                if (noderror != null)
                {
                    XmlNode nodeCodError = noderror.SelectSingleNode("CodError");
                    if (nodeCodError != null)
                    {
                        nodeCodError.InnerText = "100";
                    }

                    XmlNode nodeError = noderror.SelectSingleNode("Error");
                    if (nodeError != null)
                    {
                        nodeError.InnerText = "Timeout conectando son servicio de Equifax";
                    }
                }

                client.Abort();
                return "ERROR";
            }
            catch (FaultException e)
            {
                ErrorHandling.Instance.SetPageError(e);
            }
            catch (CommunicationException e)
            {
                ErrorHandling.Instance.SetPageError(e);

                XmlNode noderror = docError.SelectSingleNode("/experto");
                if (noderror != null)
                {
                    XmlNode ncod = noderror.SelectSingleNode("CodError");
                    if (ncod != null)
                    {
                        ncod.InnerText = "101";
                    }

                    XmlNode nerr = noderror.SelectSingleNode("Error");
                    if (nerr != null)
                    {
                        nerr.InnerText = "Error de comunicacion conectando son servicio de Equifax";
                    }
                }
                client.Abort();
                return "ERROR";
            }

            if (resultado != string.Empty)
            {
                try
                {
                    resultado = resultado.Replace(" />", "/>");
                    docResultado.LoadXml(resultado);
                }
                catch
                {
                    throw new InvalidOperationException("Problema de xml de salida: " + resultado);
                }

                XmlNode nodeResultados = docResultado.SelectSingleNode("experto/integrantes/datos");

                if (nodeResultados != null)
                {
                    solResultado.Session.BeginNestedUnitOfWork();

                    if (nodeResultados.SelectSingleNode("accion")?.InnerText != "SIN_INFORMACION" ||
                        nodeResultados.SelectSingleNode("accion")?.InnerText != "FALLECIDO" ||
                        nodeResultados.SelectSingleNode("accion")?.InnerText != "BLOQUEADO")
                    {
                        solResultado.Fecha = DateTime.Now;
                        solResultado.Documento = nodeResultados.SelectSingleNode("documento")?.InnerText;
                        solResultado.Nombres = nodeResultados.SelectSingleNode("nombres")?.InnerText;
                        solResultado.Apellidos = nodeResultados.SelectSingleNode("apellidos")?.InnerText;
                        solResultado.Accion = nodeResultados.SelectSingleNode("accion")?.InnerText;
                        solResultado.Explicacion = nodeResultados.SelectSingleNode("explicacion")?.InnerText;

                        //nodo de segmento TODO seleccionar el campo calculado correcto
                        XmlNode nodeSegmento = docResultado.SelectSingleNode("experto/integrantes/datos/campos_calculados/SEGMENTO/valor");

                        solResultado.Solicitud.SolicitudPersonaTitular.Segmento = (Segmento)Int32.Parse(nodeSegmento.InnerText);
                        solResultado.Solicitud.SolicitudPersonaTitular.Save();


                                /* foreach (Cheque item in solResultado.Solicitud.Cheques
                                     .Where(ch => ch.Librador == solResultado.Persona))
                                 {
                                     item.Resultado = solResultado;
                                     if (solResultado.Accion != null)
                                     {
                                         item.Rechazado = solResultado.Accion.ToUpper() == "RECHAZAR";
                                     }

                                     MotivoRechazoCheque mRechazo = new MotivoRechazoCheque(solResultado.Session);
                                     item.MotivoRechazo = mRechazo.GetOrCreate(solResultado.Session,
                                                                               "RECHAZADO POR EL MOTOR");
                                 }*/

                        
                        solResultado.Save();
                        solResultado.Session.CommitTransaction();
                        View.Refresh();
                    }
                    solResultado.Solicitud.Save();
                    return nodeResultados.SelectSingleNode("accion")?.InnerText;
                }
                else
                {
                    throw new InvalidOperationException("Problemas en la conexión con Equifax! Error: " + resultado);
                }
            }
            else
            {
                throw new InvalidOperationException("Servicio de Equifax no disponible en este momento!");
            }
        }

        private void ProcesarMotor2_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Solicitud solicitud = ((Solicitud)View.CurrentObject);
            //solicitud.AProcesar = true;
            solicitud.ObjectSpace.CommitChanges();
            solicitud.Session.ExecuteSproc("SP_LLAMAR_A_JOB");
        }

        private void ProcesarSolicitante_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Solicitud solicitud = ((Solicitud)View.CurrentObject);

            if (solicitud.MontoPresupuesto > solicitud.Producto.MontoSolicitarInformconf)
            {
                if (solicitud.Faja == null)
                {
                    solicitud.ObjectSpace.CommitChanges();
                    throw new UserFriendlyException("No se puede Procesar el motor si no se encuentra cargada la Faja!");
                }

                SolicitudPersona.GenerarPromedioAtraso(solicitud.SolicitudPersonaTitular);

                List<Resultado> resultados = solicitud.ResultadosMotor.Where(r => !r.Procesado).ToList();

                foreach (Resultado item in resultados)
                {
                    item.Accion = ProcesarMotor(item);
                    item.Procesado = !item.Accion.ToUpper().Equals("ERROR");
                    item.Save();
                }
            } else
            {
                SolicitudPersona.GenerarPromedioAtraso(solicitud.SolicitudPersonaTitular);
            }

            solicitud.ObjectSpace.CommitChanges();
        }

        private void Transiciones_Execute(object sender, SingleChoiceActionExecuteEventArgs e)
        {
            SingleChoiceAction action = (SingleChoiceAction)sender;
            WFTransicion wfTran = (WFTransicion)action.SelectedItem.Data;
            bool esReversion = wfTran.EsReversion;
            Solicitud sol = (Solicitud)View.CurrentObject;
            var solOid = sol.Oid;
            var wfTranOid = wfTran.Oid;
            sol.ObjectSpace.CommitChanges();
            sol.ObjectSpace.Refresh();  //Traer datos de colecciones que pudieron haberse cambiado
            sol = sol.ObjectSpace.GetObjectByKey<Solicitud>(solOid); //recargar el objeto
            wfTran = sol.ObjectSpace.GetObjectByKey<WFTransicion>(wfTranOid); //recargar el objeto
            wfTran.EsReversion = esReversion;
            string errorMesagge;

            if (ConfigurationManager.AppSettings["CodigoInstancia"] == "CREDIFACIL")
            {
                List<Solicitud> solicitudesCodeudores = sol.SolicitudesAsociadas.ToList();

                foreach (Solicitud solicitudCodeudor in solicitudesCodeudores)
                {
                    if (solicitudCodeudor.Estado.Codigo != "FINALIZADA" && solicitudCodeudor.Estado.Codigo != "RECHAZADA" && sol.Estado.Codigo == "PENDIENTE APROBACION")
                    {
                        throw new Exception("La Solicitud del Codeudor Nro: " + solicitudCodeudor.Oid + " debe ser aprobada o rechazada antes que la Solicitud del Titular.");
                    }
                }
            }

            if (wfTran.EstadoDestino != null)
            {
                errorMesagge = WFHelper.CambiarEstado(sol, wfTran).Mensaje;
            }
            else
            {
                throw new UserFriendlyException("No se pudo realizar la transición");
            }

            if (errorMesagge != string.Empty)
            {
                throw new UserFriendlyException(errorMesagge);
            }

            Persona.CambiarEstadoPersona(sol.Titular, true);

            if (ConfigurationManager.AppSettings["CodigoInstancia"] == "CREDIFACIL")
            {
                if (sol.SolicitudOriginal != null && sol.Producto.Codigo == "CREDIFACIL_5" && wfTran.EstadoOrigen.Codigo == "PENDIENTE APROBACION" && wfTran.EstadoDestino.Codigo != "RECHAZADA")
                {
                    WFHelper.SolicitarCambioEstado(sol);
                }

                if ((wfTran.EstadoDestino.Codigo == "PENDIENTE" || wfTran.EstadoDestino.Codigo == "PENDIENTE FECHA") && sol.EnviadoBack)
                {
                    sol.EnviadoBack = false;
                    sol.FechaEnvioBack = DateTime.MinValue;
                    sol.FechaPrimerVencimiento = DateTime.MinValue;
                }
            }

            sol.Save();
            sol.ObjectSpace.CommitChanges();
            sol.ObjectSpace.Refresh();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ModifiedChanged += ObjectSpace_ModifiedChanged;
            UpdateActionState();
        }

        protected virtual void UpdateActionState()
        {
            if (View is DashboardView) return;

            if (View == null || View.ObjectTypeInfo.Name != nameof(Solicitud))
            {
                return;
            }

            string urlVisor = System.Configuration.ConfigurationManager.AppSettings["urlVisorGallery"];

            string url = $"{urlVisor}?src={Helper.GenerateUrlParam(View.CurrentObject)}";
            VerAdjuntos.SetClientScript($"window.open('{url}', '_blank')", false);

            AgregarTransiciones();

            bool mostrarImportarChe = false;
            bool mostrarImportarPer = false;
            bool mostrarProcesarMotor = false;
            bool mostrarProcesarMotor2 = false;
            bool mostrarDatosPersona = true;
            bool mostrarGenerarPropuesta = false;
            bool mostrarGenerarPresupuesto = false;
            bool mostrarTransiciones = true;
            bool mostrarInformeEscribania = false;
            bool mostrarAgregarInmueble = false;
            bool mostrarCrearTarea = false;
            bool mostrarIntegracionITGF = false;
            bool mostrarGenerarSolAsociada = false;
            bool mostrarCalculadoraPrestamo = false;
            bool mostrarImprimirSolicitud = false;
            Solicitud sol = (Solicitud)View.CurrentObject;

            if (sol != null && sol.Oid != -1 && sol.Producto != null)
            {
                mostrarImportarChe = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.ImportarCheques} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarImportarPer = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.ImportarPersonas} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarProcesarMotor = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.ProcesarMotor} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarProcesarMotor2 = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.ProcesarMotor2} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarDatosPersona = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.DatosPersona} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarGenerarPropuesta = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.GenerarPropuesta} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarGenerarPresupuesto = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.GenerarPresupuesto} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarInformeEscribania = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.InformeEscribania} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarAgregarInmueble = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.AgregarInmueble} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarCrearTarea = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.CrearTarea} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarIntegracionITGF = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.IntegracionITGF} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarGenerarSolAsociada = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.GenerarSolAsociada} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                mostrarCalculadoraPrestamo = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.CalculadoraPrestamo} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                ProcesarSolicitante.TargetObjectsCriteria = "ResultadosMotor[Procesado = false].Exists()";
                mostrarImprimirSolicitud = (bool)sol.Evaluate(CriteriaOperator.Parse($"Producto.[Acciones][Accion = {(int)Acciones.ImprimirSolicitud} and EstadoDestino.Codigo = '{sol.Estado.Codigo}'].Exists()"));
                ProcesarSolicitante.TargetObjectsCriteria = "ResultadosMotor[Procesado = false].Exists()";

                //verificar si se puede generar mas de un prespuesto
                string criterioCantPresupuestos = "1=1";

                if (!sol.Producto.PermitirVariosPresupuestos)
                    criterioCantPresupuestos = "Presupuestos.Count() = 0";

                generarPresupuesto.TargetObjectsCriteria = $"UsuarioEnRol('PCA_GenerarPresupuesto') and {criterioCantPresupuestos}";
            }
            else
            {
                mostrarTransiciones = false;
            }

            ImportarCheques.Active.SetItemValue("myReason2", mostrarImportarChe);
            ImportarPersonas.Active.SetItemValue("myReason5", mostrarImportarPer);
            ProcesarSolicitante.Active.SetItemValue("myReason3", mostrarProcesarMotor);
            ProcesarMotor2.Active.SetItemValue("myReason6", mostrarProcesarMotor2);
            DatosPersona.Active.SetItemValue("myReason4", mostrarDatosPersona);
            Transiciones.Active.SetItemValue("mostrarTransiciones", mostrarTransiciones);
            generarPropuestaComercial.Active.SetItemValue(string.Empty, mostrarGenerarPropuesta);
            generarPresupuesto.Active.SetItemValue(string.Empty, mostrarGenerarPresupuesto);
            informeEscribania.Active.SetItemValue(string.Empty, mostrarInformeEscribania);
            AgregarInmueble.Active.SetItemValue(string.Empty, mostrarAgregarInmueble);
            CrearTarea.Active.SetItemValue(string.Empty, mostrarCrearTarea);
            IntegracionITGF.Active.SetItemValue(string.Empty, mostrarIntegracionITGF);            

            if (mostrarGenerarSolAsociada)
                GenerarSolAsociada.Caption = sol.Producto.Acciones.Where(a => a.Accion == Acciones.GenerarSolAsociada && a.EstadoDestino== sol.Estado).FirstOrDefault().Configuracion.NombreBoton;
           
            GenerarSolAsociada.Active.SetItemValue(string.Empty, mostrarGenerarSolAsociada);
            CalculadoraPrestamo.Active.SetItemValue(string.Empty, mostrarCalculadoraPrestamo);
            ImprimirSolicitud.Active.SetItemValue(string.Empty, mostrarImprimirSolicitud);
        }

        private void informeEscribania_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            CultureInfo culture = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name, true);
            culture.NumberFormat.NumberDecimalSeparator = ",";
            culture.NumberFormat.NumberGroupSeparator = ".";
            System.Threading.Thread.CurrentThread.CurrentCulture = culture;

            Solicitud sol = (Solicitud)View.CurrentObject;
            IObjectSpace space = Application.CreateObjectSpace(typeof(ReportDataV2));
            IReportDataV2 reportData = (IReportDataV2)space.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Informe Escribania"));
            CriteriaOperator co2 = CriteriaOperator.Parse("Presupuesto.Solicitud.Oid=? and Presupuesto.AceptadoCliente = True and Presupuesto.Aprobado = True", sol.Oid);
            string reportContainerHandle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
            Frame.GetController<ReportServiceController>().ShowPreview(reportContainerHandle, co2);

        }

        private void CrearTarea_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {

        }

        private void CrearTarea_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            CrearTarea obj = os.CreateObject<CrearTarea>();
            obj.Solicitud = os.GetObjectByKey<Solicitud>(((Solicitud)View.CurrentObject).Oid);
            DetailView dv = Application.CreateDetailView(os, obj, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.View = dv;
        }

        private void IntegracionITFGParam_Execute(object sender, ParametrizedActionExecuteEventArgs e)
        {
            /*Solicitud solicitud = (Solicitud)View.CurrentObject;
            solicitud.FechaADesembolsar = (DateTime)e.ParameterCurrentValue;
            //Hacer calculo de vencimientos
            PresupuestoPrestamo prestamo = solicitud.Presupuestos.Where(x => x.AceptadoCliente).First().Prestamos[0];
            prestamo.CalcularVencimientos();

            if (!solicitud.EnviadoBack)
            {
                ITGFAccess iTGFAccess = new ITGFAccess();
                OpeRespuesta resp = iTGFAccess.CreateSolicitud(solicitud);

                if (resp.Error)
                    throw new Exception(resp.Mensaje);
                else
                {
                    solicitud.EnviadoBack = true;
                    solicitud.FechaEnvioBack = DateTime.Now;
                    solicitud.Save();
                    solicitud.ObjectSpace.CommitChanges();
                    solicitud.ObjectSpace.Refresh();
                }
            }*/

        }

        private void GenerarSolAsociada_Execute(object sender, SimpleActionExecuteEventArgs e)
        {            
            IObjectSpace os = Application.CreateObjectSpace();
            Solicitud solNueva = os.CreateObject<Solicitud>();
            Solicitud solicitud = os.GetObjectByKey<Solicitud>(((Solicitud)View.CurrentObject).Oid);
            WFAccionConfig configuracion = os.GetObjectByKey<WFAccionConfig>(solicitud.Producto.Acciones.Where(a => a.Accion == Acciones.GenerarSolAsociada && a.EstadoDestino == solicitud.Estado).FirstOrDefault().Configuracion.Oid);
            Solicitud.GenerarSolAsociada(solicitud, solNueva, configuracion);
 
            e.ShowViewParameters.CreatedView = Application.CreateDetailView(os,
                                                                           "Solicitud_DetailView",
                                                                           true,
                                                                           solNueva);

        }

        private void ProcesarTareas_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Solicitud sol = (Solicitud)View.CurrentObject;
            var solOid = sol.Oid;
            sol.ObjectSpace.CommitChanges();
            sol.ObjectSpace.Refresh();  //Traer datos de colecciones que pudieron haberse cambiado
            sol = sol.ObjectSpace.GetObjectByKey<Solicitud>(solOid); //recargar el objeto

            //Finalizar tareas que cumplen criterios y marcadas para finalizar automaticamente
            WFHelper.SolicitarFinTareas(sol);

            //Verificar que no queden tareas pendientes
            if (sol.Tareas.Any(t => t.Estado != EstadoTarea.Finalizada && t.Estado != EstadoTarea.Cancelada))
            {
                string mensaje = "Debe finalizar todas las tareas:";

                foreach (var tarea in sol.Tareas.Where(x => x.Estado != EstadoTarea.Finalizada && x.Estado != EstadoTarea.Cancelada))
                {
                    mensaje += "\n " + tarea.TipoTarea.Descripcion.ToUpper() + ":";
                    foreach (var criterio in tarea.TipoTarea.Criterios)
                    {
                        if (!(bool)sol.Evaluate(CriteriaOperator.Parse(criterio.Criterio)))
                            mensaje += "\n - " + criterio.Mensaje;
                    }
                }

                throw new Exception(mensaje);
            }

            sol.Save();
            sol.ObjectSpace.CommitChanges();
            sol.ObjectSpace.Refresh();
        }

        private void IntegracionITGF_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            EnviarAlBack enviarAlBack = (EnviarAlBack)e.PopupWindowViewCurrentObject;
            Solicitud solicitud = (Solicitud)View.CurrentObject;
            DateTime ultimoDiaAño = Convert.ToDateTime("31/12");
            DateTime limiteFechaPrimerVencimiento = enviarAlBack.FechaDesembolso.AddMonths(1).AddDays(solicitud.Producto.DiasBeneficio);

            if (enviarAlBack.FechaDesembolso < DateTime.Today && DateTime.Now.Date != ultimoDiaAño.Date)
                throw new Exception("La fecha del desembolso no puede ser menor a la fecha actual.");

            if (enviarAlBack.FechaPrimerVencimiento < enviarAlBack.FechaDesembolso)
                throw new Exception("La fecha del primer vencimiento no puede ser menor a la fecha de desembolso.");

            if (solicitud.Producto.ControlarLimitePrimerVencimiento && enviarAlBack.FechaPrimerVencimiento > limiteFechaPrimerVencimiento)
                throw new Exception("La fecha del primer vencimiento no puede ser mayor a los " + solicitud.Producto.DiasBeneficio + " días de beneficio.");

            //Hacer calculo de vencimientos
            PresupuestoPrestamo prestamo = solicitud.Presupuestos.Where(x => x.AceptadoCliente).First().Prestamos[0];
            prestamo.CalcularVencimientos(enviarAlBack.FechaDesembolso, enviarAlBack.FechaPrimerVencimiento);

            if (!solicitud.EnviadoBack)
            {
                solicitud.FechaADesembolsar = enviarAlBack.FechaDesembolso;
                ITGFAccess iTGFAccess = new ITGFAccess();
                OpeRespuesta resp = iTGFAccess.CreateSolicitud(solicitud);

                if (resp.Error)
                    throw new Exception(resp.Mensaje);
                else
                {
                    solicitud.EnviadoBack = true;
                    solicitud.FechaEnvioBack = DateTime.Now;
                    solicitud.FechaPrimerVencimiento = enviarAlBack.FechaPrimerVencimiento;

                    if (ConfigurationManager.AppSettings["CodigoInstancia"] == "CREDIFACIL")
                    {
                        WFHelper.SolicitarCambioEstado(solicitud);
                    }

                    solicitud.Save();
                    solicitud.ObjectSpace.CommitChanges();
                    solicitud.ObjectSpace.Refresh();
                }
            }

        }
        private void IntegracionITGF_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            EnviarAlBack obj = os.CreateObject<EnviarAlBack>();
            Solicitud solicitud = (Solicitud)View.CurrentObject;

            obj.FechaPrimerVencimiento = obj.FechaDesembolso.AddMonths(solicitud.Producto.MesesPrimerVencimiento);
            obj.Solicitud = os.GetObjectByKey<Solicitud>(solicitud.Oid); ;

            DetailView dv = Application.CreateDetailView(os, obj, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.View = dv;
        }

        private void CalculadoraPrestamo_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            CalculadoraPrestamo obj = os.CreateObject<CalculadoraPrestamo>();
            Solicitud solicitud = (Solicitud)View.CurrentObject;

            obj.Monto = solicitud.MontoPresupuesto;
            obj.Plazo = solicitud.Plazo;
            obj.TasaMensual = solicitud.TasaAnual / 12;
            obj.MontoCuota = solicitud.Monto;

            DetailView dv = Application.CreateDetailView(os, obj, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.View = dv;
        }

        private void CalculadoraPrestamo_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            CalculadoraPrestamo calculadoraPrestamo = (CalculadoraPrestamo)e.PopupWindowViewCurrentObject;
            Solicitud solicitud = (Solicitud)View.CurrentObject;
            Presupuesto presupuestoAnterior = null;

            if (VerificarRango(calculadoraPrestamo) == 0)
            {
                throw new Exception("¡Monto del prestamo no corresponde al Rango!");
            } else if (VerificarRango(calculadoraPrestamo) == 1)
            {
                throw new Exception("¡Plazo del prestamo no corresponde al Tipo de Interes!");
            }

            if (calculadoraPrestamo.Monto <= 0 || calculadoraPrestamo.Plazo <= 0 || calculadoraPrestamo.TasaMensual <= 0 || calculadoraPrestamo.MontoCuota <= 0)
            {
                throw new Exception("¡Datos deben ser mayores a 0 (cero)!");
            }

            solicitud.MontoPresupuesto = calculadoraPrestamo.Monto;
            solicitud.Plazo = calculadoraPrestamo.Plazo;
            solicitud.TasaAnual = calculadoraPrestamo.TasaMensual * 12;
            solicitud.Monto = calculadoraPrestamo.MontoCuota;
            solicitud.Save();

            presupuestoAnterior = solicitud.Presupuestos.Where<Presupuesto>(p => p.AceptadoCliente).FirstOrDefault();

            if (presupuestoAnterior != null)
            {
                presupuestoAnterior.AceptadoCliente = false;
                presupuestoAnterior.EsPropuesta = true;
                presupuestoAnterior.Aprobado = false;
                presupuestoAnterior.Save();
                Presupuesto.CrearPresupuesto(solicitud, false);
            } 
            else
            {
                Presupuesto.CrearPresupuesto(solicitud, false);
            }

            solicitud.Save();
            solicitud.ObjectSpace.CommitChanges();

            if (solicitud.SolicitudCodeudor != null)
            {
                solicitud.SolicitudCodeudor.MontoPresupuesto = solicitud.MontoPresupuesto;
                solicitud.SolicitudCodeudor.Plazo = solicitud.Plazo;
                solicitud.SolicitudCodeudor.TasaAnual = solicitud.TasaAnual;
                solicitud.SolicitudCodeudor.Monto = solicitud.Monto;

/*                Presupuesto presupuestoTitular = solicitud.ObjectSpace.FindObject<Presupuesto>(Presupuesto.Fields.AceptadoCliente == (CriteriaOperator)true);

                Presupuesto presupuestoCodeudorAnterior = solicitud.SolicitudCodeudor.Presupuestos.Where<Presupuesto>(p => p.AceptadoCliente).FirstOrDefault();
                presupuestoCodeudorAnterior.AceptadoCliente = false;
                presupuestoCodeudorAnterior.EsPropuesta = true;
                presupuestoCodeudorAnterior.Aprobado = false;
                presupuestoCodeudorAnterior.Save();

                Presupuesto presupuestoCodeudor = solicitud.SolicitudCodeudor.ObjectSpace.CreateObject<Presupuesto>();

                presupuestoCodeudor = presupuestoTitular;
                presupuestoCodeudor.Solicitud = solicitud.SolicitudCodeudor;
                presupuestoCodeudor.Save();
                presupuestoCodeudor.ObjectSpace.CommitChanges();*/

                solicitud.SolicitudCodeudor.Save();
                solicitud.SolicitudCodeudor.ObjectSpace.CommitChanges();
            }

            solicitud.ObjectSpace.Refresh();
        }

        private int VerificarRango(CalculadoraPrestamo calculadoraPrestamo)
        {
            if (calculadoraPrestamo.InteresMensual != null
                && calculadoraPrestamo.InteresMensual.RangoMinimo != 0
                && (calculadoraPrestamo.Monto < calculadoraPrestamo.InteresMensual.RangoMinimo
                || calculadoraPrestamo.Monto > calculadoraPrestamo.InteresMensual.RangoMaximo))
            {
                return 0;
            }

            InteresMensual mensual = calculadoraPrestamo.Session.FindObject<InteresMensual>(InteresMensual.Fields.CuotaReferencia == calculadoraPrestamo.Plazo);

            if (calculadoraPrestamo.InteresMensual != null
                && mensual != null
                && mensual != calculadoraPrestamo.InteresMensual)
            {
                return 1;
            }

            return 2;
        }

        private void SolicitudController_Activated(object sender, EventArgs e)
        {
            string UsarCustomDV = ConfigurationManager.AppSettings["UsarCustomDetailView"];

            if (UsarCustomDV != null && UsarCustomDV == "S")
            {
                string codigoInstancia = ConfigurationManager.AppSettings["CodigoInstancia"];
                IModelList<IModelView> modelViews = View.Model.Application.Views;

                if (View.Model.Id.Contains("DetailView"))
                {
                    IModelView myViewNode = modelViews["Solicitud_DetailView_" + codigoInstancia];
                    if (myViewNode != null)
                        View.SetModel(myViewNode);
                }
            }

        }

        private void ImprimirSolicitud_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Solicitud solicitud = (Solicitud)View.CurrentObject;
            IObjectSpace space = Application.CreateObjectSpace(typeof(ReportDataV2));
            IReportDataV2 reportData;

            /*            CultureInfo culture = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name, true);
                        culture.NumberFormat.NumberDecimalSeparator = ",";
                        culture.NumberFormat.NumberGroupSeparator = ".";
                        System.Threading.Thread.CurrentThread.CurrentCulture = culture;*/

            reportData = (IReportDataV2)space.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Reporte Solicitud"));

            /*            if (pres.EsPropuesta)
                            reportData = (IReportDataV2)space.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Propuesta Comercial"));
                        else
                        {
                            if (pres.Prestamos.Count() > 1)
                                throw new Exception("El presupuesto no puede tener más de un préstamo");
                            reportData = (IReportDataV2)space.FindObject<ReportDataV2>(new BinaryOperator("DisplayName", "Presupuesto"));
                        }*/

            CriteriaOperator co2 = CriteriaOperator.Parse("Oid=?", solicitud.Oid);
            string reportContainerHandle = ReportDataProvider.ReportsStorage.GetReportContainerHandle(reportData);
            Frame.GetController<ReportServiceController>().ShowPreview(reportContainerHandle, co2);
        }
    }
}

