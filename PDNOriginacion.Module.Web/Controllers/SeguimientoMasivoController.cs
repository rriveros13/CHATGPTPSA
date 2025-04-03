using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using PDNOriginacion.Module.BusinessObjects;
using DevExpress.Spreadsheet;
using DevExpress.Xpo;
using DevExpress.Persistent.AuditTrail;

namespace PDNOriginacion.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class SeguimientoMasivoController : ViewController
    {
        public SeguimientoMasivoController()
        {
            InitializeComponent();
            // Target required Views (via the TargetXXX properties) and create their Actions.
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void ImportarSeguimientos_CustomizePopupWindowParams(object sender, CustomizePopupWindowParamsEventArgs e)
        {
            IObjectSpace os = Application.CreateObjectSpace();
            SeleccionarArchivoExcel obj = os.CreateObject<SeleccionarArchivoExcel>();
            DetailView dv = Application.CreateDetailView(os, obj, true);
            dv.ViewEditMode = ViewEditMode.Edit;
            e.View = dv;
        }

        private void ImportarSeguimientos_Execute(object sender, PopupWindowShowActionExecuteEventArgs e)
        {
            IObjectSpace os = ((SeguimientoMasivo)View.CurrentObject).ObjectSpace;
            SeleccionarArchivoExcel selArchivo = (SeleccionarArchivoExcel)e.PopupWindowViewCurrentObject;
            os.CommitChanges();

            AuditTrailService.Instance.ObjectAuditingMode = ObjectAuditingMode.CreationOnly;


            if (selArchivo.Archivo == null) return;

            try
            {
                MemoryStream ms = new MemoryStream();
                selArchivo.Archivo.SaveToStream(ms);
                ms.Position = 0;
                Workbook workbook = new Workbook();
                workbook.LoadDocument(ms);
                SeguimientoMasivo sm = os.GetObjectByKey<SeguimientoMasivo>(((SeguimientoMasivo)View.CurrentObject).Oid);

                ITTI.FileDataITTI archivo = new ITTI.FileDataITTI(sm.Session);
                archivo.Content = selArchivo.Archivo.Content;
                archivo.FileName = selArchivo.Archivo.FileName;
                archivo.Save();

                sm.Archivo = archivo;

                RowCollection rows = workbook.Worksheets[0].Rows;
                int r = 1;
                Row row = rows[r];

                const string colFecha = "A";
                const string colPrefijo = "B";
                const string colNumero = "C";
                const string colMedIngreso = "D";
                const string colComentario = "E";
                const string colFecProximo = "F";
                const string colUsuario = "G";
                const string colMotSeguimiento = "H";
                NaturalezaPersona perFisica = os.FindObject<NaturalezaPersona>(CriteriaOperator.Parse("Codigo = 'F'"));

                while (row[0].Value.ToString() != string.Empty)
                {
                    //validaciones y auxiliares
                    DateTime auxFecha;
                    if(row[colFecha].Value == null) throw new Exception("Fecha inválida en la celda: " + colFecha + r.ToString());
                    if (!DateTime.TryParse(row[colFecha].Value.ToString(), out auxFecha))
                        throw new Exception("Fecha inválida en la celda: " + colFecha + r.ToString());

                    if (row[colPrefijo].Value == null || row[colPrefijo].Value.NumericValue == 0) throw new Exception("Prefijo no válido en celda: " + colPrefijo + r.ToString());
                    Prefijo prefijo = os.FindObject<Prefijo>(CriteriaOperator.Parse("Codigo= '" + row[colPrefijo].Value.ToString() + "'"));
                    if (prefijo == null) throw new Exception("Prefijo no válido en celda: "+  colPrefijo + r.ToString());
                    
                    if (row[colNumero].Value == null || row[colNumero].Value.NumericValue == 0) throw new Exception("Número no válido en celda: " + colNumero + r.ToString());
                    string numero = row[colNumero].Value.ToString();
                    if (numero == null) throw new Exception("Número no válido en celda: " + colNumero + r.ToString());

                    if (row[colMedIngreso].Value == null || row[colMedIngreso].Value.TextValue == null) throw new Exception("Medio de ingreso no válido en celda: " + colMedIngreso + r.ToString());
                    MedioIngreso medIngreso = os.FindObject<MedioIngreso>(CriteriaOperator.Parse("Codigo= '" + row[colMedIngreso].Value.ToString() + "'"));
                    if (medIngreso == null) throw new Exception("Medio de ingreso no válido en celda: " + colMedIngreso + r.ToString());

                    DateTime auxFecProximo;
                    if (row[colFecProximo].Value == null) throw new Exception("Fecha inválida en la celda: " + colFecProximo + r.ToString());
                    if (!DateTime.TryParse(row[colFecProximo].Value.ToString(), out auxFecProximo))
                        throw new Exception("Fecha inválida en la celda: " + colFecProximo + r.ToString());

                    if (row[colUsuario].Value == null || row[colUsuario].Value.TextValue == null) throw new Exception("Usuario no válido en celda: " + colUsuario + r.ToString());
                    Usuario usuario = os.FindObject<Usuario>(CriteriaOperator.Parse("UserName= '" + row[colUsuario].Value.ToString() + "'"));
                    if (usuario == null) throw new Exception("Usuario no válido en celda: " + colUsuario + r.ToString());

                    if (row[colMotSeguimiento].Value == null || row[colMotSeguimiento].Value.TextValue == null) throw new Exception("Motivo seguimiento no válido en celda: " + colMotSeguimiento + r.ToString());
                    MotivoSeguimiento motSeguimiento = os.FindObject<MotivoSeguimiento>(CriteriaOperator.Parse("Codigo= '" + row[colMotSeguimiento].Value.ToString() + "'"));
                    if (motSeguimiento == null) throw new Exception("Motivo seguimiento no válido en celda: " + colMotSeguimiento + r.ToString());

                    string telefonoCompleto = prefijo + numero;
                    #region Obtener o crear la persona      
                    Persona persona = os.FindObject<Persona>(CriteriaOperator.Parse("TelefonoPreferido.TelefonoCompleto = '"+ telefonoCompleto + "'"));

                    if (persona == null)
                    {
                        persona = new Persona(sm.Session);
                        persona.Tipo = perFisica;
                        persona.MedioIngreso = medIngreso;
                        Telefono telefono = new Telefono(sm.Session);
                        telefono.Numero = numero;
                        telefono.Persona = persona;
                        telefono.Preferido = true;
                        telefono.Prefijo = prefijo;
                        telefono.Tipo = telefono.Prefijo.Tipo;
                        telefono.TipoTelefono = sm.Session.FindObject<TipoTelefono>(CriteriaOperator.Parse("Codigo = 'P'"));
                        telefono.Whatsapp = true;
                        telefono.Save();
                        persona.Save();
                    }
                    #endregion Obtener o crear la persona

                    #region Cargar Seguimiento
                    Seguimiento seg = new Seguimiento(sm.Session);
                    if(row[colComentario].Value != null )seg.Comentarios = row[colComentario].Value.ToString();
                    seg.MotivoSeguimiento = motSeguimiento;
                    seg.ProximoSegFecha = auxFecProximo;
                    seg.Persona = persona;
                    seg.SeguimientoMasivo = sm;
                    seg.TelefonoContactado = persona.TelefonoPreferido;
                    seg.Usuario = usuario;
                    seg.Save();
                    
                    seg.Fecha = auxFecha; //La fecha se sobreescribe en el afterconstruction, por eso se vuelve  a grabar despues

                    seg.Save();
                    #endregion Cargar Seguimiento
                    r++;
                    row = rows[r];
                    os.CommitChanges();
                }


                //((SeguimientoMasivo)View.CurrentObject).Save();
               // os.CommitChanges();
                AuditTrailService.Instance.ObjectAuditingMode = ObjectAuditingMode.Full;
            }
            catch
            {
                os.Rollback();
                throw;
            }
        }
    }
}
