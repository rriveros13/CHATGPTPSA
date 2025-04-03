using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.Spreadsheet;
using PDNOriginacion.Module.BusinessObjects;
using PDNOriginacion.Module.Helpers;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using PDNOriginacion.Module.Web.Helpers;
using DevExpress.ExpressApp.Model;
using System.Configuration;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class PersonaController : ViewController
    {

        public PersonaController()
        {
            InitializeComponent();
        }

        private void DatosPersona_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Persona per = (Persona)View.CurrentObject;
            Buscame perBuscame = WFHelper.BuscarPersona(per.Documento);
            if(perBuscame == null)
            {
                return;
            }

            per.PrimerNombre = Persona.GetPalabraSeparada(perBuscame.Name, true);
            per.SegundoNombre = Persona.GetPalabraSeparada(perBuscame.Name, false);
            per.PrimerApellido = Persona.GetPalabraSeparada(perBuscame.Lastname,true);
            per.SegundoApellido = Persona.GetPalabraSeparada(perBuscame.Lastname, false);
        }
        private void GenerarSolicitud_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            Persona persona = (Persona)View.CurrentObject;

            persona.Save();
            persona.ObjectSpace.CommitChanges();

            IObjectSpace os = Application.CreateObjectSpace();
            var producto = os.FindObject<Producto>(Producto.Fields.Default == (CriteriaOperator)true);

            foreach (var item in producto.GeneracionSolicitud)
            {
                if ((bool)persona.Evaluate(item.Criterio) == false)
                    throw new System.Exception(item.Mensaje);
            }
                       
            Solicitud sol = os.CreateObject<Solicitud>();
            sol.Producto = producto;
            sol.Documento = persona.Documento;
            sol.TipoDocumento = os.GetObjectByKey<TipoDocumento>(persona.TipoDocumento.Oid);
            sol.UsuarioParaTareas = Usuario.GetUsuarioLibre(producto.RolGeneracion.Name, sol.Session);

            if(persona.MotivoSolicitud != null)
                sol.Motivo = os.GetObjectByKey<MotivoSolicitud>(persona.MotivoSolicitud.Oid);

            sol.Save();

            bool procesarMotor = (from wtp in sol.Producto.Personas
                where wtp.TipoPersona.Codigo == "SOL"
                select wtp.ProcesarMotor).FirstOrDefault();
            //TODO Modelo modelo = (from wtp in sol.Producto.Personas where wtp.TipoPersona.Codigo == "SOL" select wtp.Modelo).FirstOrDefault();

            SolicitudPersona sp = os.CreateObject<SolicitudPersona>();
            sp.Persona = os.GetObjectByKey<Persona>(persona.Oid);
            sp.Solicitud = sol;
            //TODO sp.ProcesarMotor = procesarMotor;
            //TODO sp.Modelo = modelo;
            sp.TipoPersona = os.FindObject<TipoPersona>(CriteriaOperator.Parse("Codigo = 'SOL'"));
            sol.Monto = persona.MontoSolicitado;

            //SolicitudPersona sp = new SolicitudPersona(sol.Session)
            //{
            //    Persona = persona,
            //    Solicitud = sol,
            //    ProcesarMotor = procesarMotor,
            //    Modelo = modelo,
            //    TipoPersona = sol.Session.FindObject<TipoPersona>(CriteriaOperator.Parse("Codigo = 'SOL'"))
            //};

            sp.Save();
            sol.Save();
            persona.ObjectSpace.CommitChanges();

            e.ShowViewParameters.CreatedView = Application.CreateDetailView(os,
                                                                            "Solicitud_DetailView",
                                                                            true,
                                                                            sol);
        }
        private void Importar_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            SeleccionarArchivoExcel obj = os.CreateObject<SeleccionarArchivoExcel>();
            DetailView dv = Application.CreateDetailView(os, obj, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.View = dv;
        }
        private void Importar_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            SeleccionarArchivoExcel selArchivo = (SeleccionarArchivoExcel)e.PopupWindowViewCurrentObject;

            if(selArchivo.Archivo == null)
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

                RowCollection rows = workbook.Worksheets[0].Rows;
                int r = 1;
                Row row = rows[r];

                while(row[0].Value.ToString() != string.Empty)
                {
                    string tipoDocumento = string.Empty;

                    string origen = row["A"].Value.ToString();
                    string nombres = row["B"].Value.ToString();
                    nombres = nombres.ToUpper();
                    string apellidos = row["C"].Value.ToString();
                    apellidos = apellidos.ToUpper();
                    // tipoDocumento = row["B"].Value.ToString();
                    string documento = row["D"].Value.ToString();
                    documento = documento.Replace(".", string.Empty).Trim();

                    string prefijo = row["E"].Value.ToString();
                    string numero = row["F"].Value.ToString();
                    string correo = row["G"].Value.ToString();
                    string usuario = row["H"].Value.ToString();

                    TipoDocumento tipoDoc;
                    if(string.IsNullOrEmpty(tipoDocumento))
                    {
                        tipoDoc = os.FindObject<TipoDocumento>(CriteriaOperator.Parse("Codigo=?", "CI"));
                    }
                    else
                    {
                        tipoDoc = os.FindObject<TipoDocumento>(CriteriaOperator.Parse("Codigo=?", tipoDocumento));
                    }

                    Persona p = Persona.GetOrCreate(documento, tipoDoc, Persona.GetPalabraSeparada(nombres,true), Persona.GetPalabraSeparada(nombres, false), 
                        Persona.GetPalabraSeparada(apellidos,true), Persona.GetPalabraSeparada(apellidos, false), os, out bool pnueva);

                    if(pnueva)
                    {
                        Telefono perTel = os.CreateObject<Telefono>();
                        perTel.Persona = p;
                        perTel.Prefijo = os.FindObject<Prefijo>(CriteriaOperator.Parse("Codigo=?", prefijo));
                        perTel.Numero = numero;
                        if(p.Telefonos.Count == 0)
                        {
                            perTel.Preferido = true;
                        }

                        perTel.Preferido = true;
                        perTel.Save();
                        p.Telefonos.Add(perTel);

                        if(!string.IsNullOrEmpty(correo))
                        {
                            try
                            {
                                MailAddress ma = new MailAddress(correo);
                                p.CorreoParticular = ma.Address;
                            }
                            catch
                            {
                                // ignored
                            }
                        }
                    }

                    p.Save();

                    Seguimiento cp = os.CreateObject<Seguimiento>();
                    cp.Persona = p;
                    cp.ProximoSegUsuario = os.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
                    if(!string.IsNullOrEmpty(usuario))
                    {
                        cp.ProximoSegUsuario = os.FindObject<Usuario>(CriteriaOperator.Parse("UserName=?", usuario));
                    }

                    if(!string.IsNullOrEmpty(origen))
                    {
                        MedioIngreso medio = os.FindObject<MedioIngreso>(CriteriaOperator.Parse("Medio=?", origen));
                        if(medio == null)
                        {
                            MedioIngreso newmedio = os.CreateObject<MedioIngreso>();
                            newmedio.Activo = true;
                            newmedio.Medio = origen;
                            newmedio.Default = false;
                            newmedio.Save();
                            medio = newmedio;
                            p.MedioIngreso = medio;
                            p.Save();
                        }
                    }

                    cp.Save();
                    //os.CommitChanges();

                    r++;
                    row = rows[r];
                }
                os.CommitChanges();
            }
            catch
            {
                os.Rollback();
                throw;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ObjectSpace.ModifiedChanged += ObjectSpace_ModifiedChanged;
            UpdateActionState();
        }

        private void ObjectSpace_ModifiedChanged(object sender, EventArgs e)
        {
            UpdateActionState();
        }

        private void UpdateActionState()
        {

            if (View is DashboardView) return;

            if (View == null  || View.ObjectTypeInfo.Name != nameof(Persona))
            {
                return;
            }

            string urlVisor = System.Configuration.ConfigurationManager.AppSettings["urlVisorGallery"];

            string url = $"{urlVisor}?src={Helper.GenerateUrlParam(View.CurrentObject)}";
            VerAdjuntos.SetClientScript($"window.open('{url}', '_blank')", false);
        }

        protected override void OnDeactivated() => base.OnDeactivated();
        protected override void OnViewControlsCreated() => base.OnViewControlsCreated();

        private void PersonaController_Activated(object sender, EventArgs e)
        {
            if (!View.Model.Id.Contains("ListView"))
            {
                string UsarCustomDV = ConfigurationManager.AppSettings["UsarCustomDetailView"];

                if (UsarCustomDV != null && UsarCustomDV == "S")
                {
                    string codigoInstancia = ConfigurationManager.AppSettings["CodigoInstancia"];
                    IModelList<IModelView> modelViews = View.Model.Application.Views;
                    IModelView myViewNode = modelViews["Persona_DetailView_" + codigoInstancia];
                    if (myViewNode != null)
                        View.SetModel(myViewNode);
                }
            }
        }
    }
}
