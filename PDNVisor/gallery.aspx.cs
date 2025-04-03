using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using GdPicture14;
using GdPicture14.WEB;
using ITTI;
using PDNOriginacion.Module.BusinessObjects;
using Serilog;
using System.Linq;
using System.Linq.Expressions;

namespace PDNVisor
{
    public partial class Gallery : System.Web.UI.Page
    {
        public static void HandleLoadAction(CustomActionEventArgs e)
        {
            string docRef = (string)e.args;
            //e.message = new DocuViewareMessage("Cargando archivo...", icon: DocuViewareMessageIcon.Information);
            Helper.GetImage(docRef, out var filename, out byte[] content);
            e.docuVieware.LoadFromStream(new MemoryStream(content), true, filename);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;

            string src = HttpUtility.UrlDecode(Request.QueryString["src"]);

            src = src?.Replace(' ', '+');

            if (string.IsNullOrEmpty(src))
            {
                throw new Exception("Invocación incorrecta, parámetro src no recibido!");
                //src = "7BB8BBB8-15C1-41BA-9B2E-18DE31DA3586";
            }

            DocuVieware1.EnableSaveButton = false;

            string param = Helper.DecryptData(src);

            string[] parray = param.Split('|');

            if (parray.Length != 4)
            {
                throw new Exception($"Cantidad de parámetros recibitos incorrecta, recibidos {parray.Length}, esperados 4!");
            }

            Log.Information($"Procesando: {parray[0]}, {parray[1]}, {parray[2]}, {parray[3]}");

            string oid = parray[0];
            DateTime fecha = Convert.ToDateTime(parray[1], new CultureInfo("es-PY"));
            string sourceIpAddress = parray[2];

            string pexpTime = ConfigurationManager.AppSettings["LinkExpiration"];
            int expTime = 60;

            if (!string.IsNullOrEmpty(pexpTime))
            {
                if (int.TryParse(pexpTime, out int time)) expTime = time;
            }

            string tipoObjeto = parray[3];

            //string oid = "5263";
            
            ReflectionDictionary dict = new ReflectionDictionary();
            dict.QueryClassInfo(typeof(Adjunto));
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (IDataLayer dataLayer = XpoDefault.GetDataLayer(connectionString, dict, AutoCreateOption.None))
            {
                using (Session session = new Session(dataLayer))
                {
                    XPQuery<Adjunto> adjuntos = session.Query<Adjunto>();
                    IQueryable<string> list = null;
                    
                    if (tipoObjeto == "SOLICITUD")
                    {
                        list = from a in adjuntos
                                                  where (a.Solicitud.Oid == Convert.ToInt32(oid)) && a.Archivo != null
                                                  orderby a.TipoAdjunto.Descripcion
                                                  select  a.Archivo.Oid.ToString() + "|" + a.TipoAdjunto.Descripcion;
                        gallery_header_title.InnerText = "Solicitud " + oid;
                    }

                    if (tipoObjeto == "PERSONA")
                    {
                        list = from a in adjuntos
                                       where (a.Persona.Oid == Guid.Parse(oid)) && a.Archivo != null
                               orderby a.TipoAdjunto.Descripcion
                                       select a.Archivo.Oid.ToString() + "|" + a.TipoAdjunto.Descripcion;

                        XPQuery<Persona> personas = session.Query<Persona>();

                        string nombreCompleto = (from a in personas
                                            where (a.Oid == Guid.Parse(oid))
                                            select a.NombreCompleto).FirstOrDefault();

                        gallery_header_title.InnerText = nombreCompleto;
                    }


                    if (tipoObjeto == "TASACION")
                    {
                        list = from a in adjuntos
                               where (a.InmuebleTasacion.Oid == Guid.Parse(oid)) && a.Archivo != null
                               orderby a.TipoAdjunto.Descripcion
                               select a.Archivo.Oid.ToString() + "|" + a.TipoAdjunto.Descripcion;
                        gallery_header_title.InnerText = "Tasación";
                    }


                    string categoriaAnterior = string.Empty;
                    bool firstDoc = true;
                    foreach (var doc in list)
                    {
                        string[] pdoc = doc.Split('|');
                        
                        Helper.GetImage(pdoc[0], out var filename, out byte[] content, out GdPicture14.DocumentFormat documentFormat);

                        if(categoriaAnterior != pdoc[1])
                        {
                            categoriaAnterior = pdoc[1];
                            using (HtmlGenericControl separador = new HtmlGenericControl("div"))
                            {
                                separador.Attributes["class"] = "separador";
                                //separador.InnerHtml = "<br />" + pdoc[1] + "<br />";
                                separador.InnerHtml = pdoc[1];

                                gallery_container.Controls.Add(separador);
                            }
                        }
                       

                        if (documentFormat != GdPicture14.DocumentFormat.DocumentFormatUNKNOWN)
                        {
                            using (HtmlGenericControl thumbnailContainer = new HtmlGenericControl("div"))
                            {
                                thumbnailContainer.Attributes["class"] = "thumbnail_container";
                                using (ImageButton btn = new ImageButton())
                                {
                                    btn.ImageUrl = "galleryThumbnails.ashx?doc=" + HttpUtility.UrlEncode(pdoc[0].ToString()) + "&fname=" + HttpUtility.UrlEncode(filename.ToString());
                                    btn.Attributes["class"] = "thumbnail_button";
                                    btn.OnClientClick = "loadDocument('" + HttpUtility.UrlEncode(pdoc[0].ToString()) + "', this);return false;";
                                    if (firstDoc)
                                    {
                                        if (DocuVieware1.LoadFromStream(new MemoryStream(content), true, filename) == GdPictureStatus.OK)
                                        {
                                            thumbnailContainer.Attributes["class"] += " item-selected";
                                            firstDoc = false;
                                        }
                                    }
                                    thumbnailContainer.Controls.Add(btn);
                                }
                                using (HtmlGenericControl titleThumbnail = new HtmlGenericControl("div"))
                                {
                                    titleThumbnail.InnerHtml = documentFormat.ToString().Replace("DocumentFormat", "") + " file<br />" + filename;
                                    thumbnailContainer.Controls.Add(titleThumbnail);
                                }
                                gallery_container.Controls.Add(thumbnailContainer);

                            }
                        }
                    }
                }
            }
            

            
        }
    }
}