using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using MetadataExtractor;
using PDNOriginacion.Module.Helpers;
using RestSharp;
using System;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
//using GdPicture14;
using System.Drawing;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultProperty("Descripcion")]
    [ImageName("BO_Note")]
    public class Adjunto : BaseObject, IObjectSpaceLink
    {
        string hash;
        GeoLocalizacion geoLocalizacion;
        static FieldsClass _fields;
        private bool _adjuntado;
        private ITTI.FileDataITTI _archivo;
        private Consulta _consulta;
        private string _descripcion;
        private Empresa _empresa;
        private DateTime _fecha = DateTime.Now;
        private Persona _persona;
        private Solicitud _solicitud;
        private InmuebleTasacion _inmuebleTasacion;
        private Inmueble _inmueble;
        private TipoAdjunto _tipoAdjunto;
        private Usuario _adjuntadoPor;


        public Adjunto(Session session) : base(session)
        {
            //_adjuntadoPor = Session.FindObject<Usuario>(CriteriaOperator.Parse("Oid=", (Guid)SecuritySystem.CurrentUserId));
            if (!session.IsObjectsLoading)
                _adjuntadoPor = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();

            if (this.Imagen == null && this.Archivo != null)
            {
                MemoryStream ms = new MemoryStream(Archivo.Content);

                //Imagen = GetDocumentPreview(ref ms);

                //if (Imagen != null)
                //{
                    this.Save();
                    this.Solicitud.Save();
                    this.ObjectSpace.CommitChanges();
                //}
            }
        }

        protected override void OnSaving()
        {
            Adjuntado = Archivo?.Content != null;

            WFAdjunto confAdjunto = Solicitud?.Producto?.Adjuntos?.Where(a => a.TipoAdjunto.Oid == TipoAdjunto.Oid)
                .FirstOrDefault();

            if (string.IsNullOrEmpty(Descripcion))
            {
                Descripcion = (this.TipoAdjunto != null ? this.TipoAdjunto.Descripcion : "") + " - " + (this.Archivo != null ? this.Archivo.FileName : "");
            }

            if (Adjuntado && confAdjunto != null)
            {
                if (TipoAdjunto.UsarValidador && confAdjunto.Validar)
                {
                    string serviceUrl = ConfigurationManager.AppSettings["CustomVisionUrl"];
                    string predictionKey = ConfigurationManager.AppSettings["Prediction-Key"];

                    RestClient client = new RestClient(serviceUrl);
                    RestRequest request = new RestRequest(RestSharp.Method.POST);
                    if (Archivo != null)
                    {
                        request.AddFileBytes("image", Archivo.Content, Archivo.FileName, "application/octet-stream");
                        request.AddHeader("Prediction-Key", predictionKey);

                        IRestResponse response = client.Execute(request);

                        if (response.IsSuccessful)
                        {
                            ClassifyResponse result = ClassifyResponse.FromJson(response.Content);

                            foreach (Prediction c in result.Predictions)
                            {
                                if (TipoAdjunto.Codigo != c.TagName)
                                {
                                    continue;
                                }

                                if (c.Probability * 100 <= TipoAdjunto.PorcentajeMatching)
                                {
                                    throw new Exception("No se cumplieron los requerimientos mínimos de la imagen.");
                                }
                                else
                                {
                                    Adjuntado = Archivo.Content != null;
                                }
                            }
                        }
                    }
                }
            }

            if (Archivo != null)
            {
                if (Archivo.Content != null)
                {
                    if (Hash != Archivo.Hash)
                    {
                        MemoryStream ms = new MemoryStream(Archivo.Content);

                        //Imagen = GetDocumentPreview(ref ms);


                        GeoLocation gl = WFImageHelper.GetGeoLocation(ref ms);
                        if (gl != null)
                        {
                            if (GeoLocalizacion == null)
                            {
                                GeoLocalizacion ngeo = new GeoLocalizacion(Session) { Longitude = gl.Longitude, Latitude = gl.Latitude, Origen = OrigenGeolocalizacion.FOTO, Title = Archivo.FileName, Adjunto = this };
                                ngeo.Save();
                                GeoLocalizacion = ngeo;
                            }
                            else
                            {
                                GeoLocalizacion.Latitude = gl.Latitude;
                                GeoLocalizacion.Longitude = gl.Longitude;
                                GeoLocalizacion.Origen = OrigenGeolocalizacion.FOTO;
                                GeoLocalizacion.Title = Archivo.FileName;
                                geoLocalizacion.Save();
                            }
                        }
                        else
                        {
                            if (GeoLocalizacion != null)
                            {
                                GeoLocalizacion.Delete();
                                GeoLocalizacion = null;
                            }
                        }
                    }
                    Hash = Archivo.Hash;
                }
                else
                {
                    Hash = null;
                    Imagen = null;
                    if (GeoLocalizacion != null)
                    {
                        GeoLocalizacion.Delete();
                        GeoLocalizacion = null;
                    }
                }
            }
            else
            {
                Hash = null;
                Imagen = null;
                if (GeoLocalizacion != null)
                {
                    GeoLocalizacion.Delete();
                    GeoLocalizacion = null;
                }
            }
            base.OnSaving();
        }

        //private byte[] GetDocumentPreview(ref MemoryStream ms)
        //{
        //    string cm_thumbnailWidth = ConfigurationManager.AppSettings["ThumbnailWidth"];
        //    string cm_thumbnailHeight = ConfigurationManager.AppSettings["ThumbnailHeight"];
        //    int thumbnailWidth = 0;
        //    int thumbnailHeight = 0;

        //    if(!string.IsNullOrEmpty(cm_thumbnailWidth))
        //    {
        //        if (int.TryParse(cm_thumbnailWidth, out int tp_thumbnailWidth))
        //             thumbnailWidth = tp_thumbnailWidth;
        //    }

        //    if(!string.IsNullOrEmpty(cm_thumbnailHeight))
        //    {
        //        if (int.TryParse(cm_thumbnailHeight, out int tp_thumbnailHeight))
        //             thumbnailHeight = tp_thumbnailHeight;
        //    }

        //    if(thumbnailWidth == 0) thumbnailWidth = 360;
        //    if(thumbnailHeight == 0) thumbnailHeight = 510;

        //    Color thumbnailBackgroundColor = Color.Transparent;

        //    GdPicture14.DocumentFormat documentFormat = GdPicture14.DocumentFormat.DocumentFormatUNKNOWN;
        //    int thumbnailId = 0;
        //    int pageCount = 0;
        //    GdPictureStatus status = GdPictureDocumentUtilities.GetDocumentPreview(ms, "", thumbnailWidth,
        //        thumbnailHeight, thumbnailBackgroundColor.ToArgb(), ref documentFormat, ref thumbnailId,
        //        ref pageCount);

        //    if (status != GdPictureStatus.OK) return null;

        //    using (GdPictureImaging gdpictureImaging = new GdPictureImaging())
        //    {
        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            if (gdpictureImaging.SaveAsStream(thumbnailId, memoryStream,
        //                    GdPicture14.DocumentFormat.DocumentFormatPNG, 6) != GdPictureStatus.OK) return null;
        //            byte[] content = memoryStream.ToArray();
        //            return content;
        //        }
        //    }
        //}

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _fecha = DateTime.Now;
            if (Solicitud != null)
            {
                _persona = Solicitud.Titular;
            }
        }

        [Appearance("Adjunto-Adjuntado", AppearanceItemType = nameof(ViewItem), TargetItems = nameof(Adjuntado), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide)]
        public bool Adjuntado
        {
            get => _adjuntado;
            set
            {
                SetPropertyValue(nameof(Adjuntado), ref _adjuntado, value);
            }
        }

        [ImmediatePostData]
        //[EditorAlias(EditorAliases.ImagePropertyEditor)]
        //[FileTypeFilter("Archivos PDF", 1, "*.pdf")]
        public ITTI.FileDataITTI Archivo
        {
            get => _archivo;
            set
            {
                SetPropertyValue(nameof(Archivo), ref _archivo, value);
                OnChanged("GeoLocalizacion");
            }
        }

        [Association("Consulta-Adjuntos")]
        public Consulta Consulta
        {
            get => _consulta;
            set => SetPropertyValue(nameof(Consulta), ref _consulta, value);
        }

        [Size(250)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        [Association("Adjunto-Empresa")]
        [VisibleInDetailView(false)]
        public Empresa Empresa
        {
            get => _empresa;
            set => SetPropertyValue(nameof(Empresa), ref _empresa, value);
        }

        [Appearance("Adjunto-Fecha", AppearanceItemType = nameof(ViewItem), TargetItems = nameof(Fecha), Context = nameof(DetailView), Enabled = false)]
        public DateTime Fecha
        {
            get => _fecha;
            set => SetPropertyValue(nameof(Fecha), ref _fecha, value);
        }

        [ModelDefault("Allow Edit", "false")]
        public GeoLocalizacion GeoLocalizacion
        {
            get => geoLocalizacion;
            set => SetPropertyValue(nameof(GeoLocalizacion), ref geoLocalizacion, value);
        }

        [Size(128)]
        [VisibleInListView(false), VisibleInDetailView(false)]
        public string Hash
        {
            get => hash;
            set => SetPropertyValue(nameof(Hash), ref hash, value);
        }

        private XPCollection<AuditDataItemPersistent> auditoria;
        public XPCollection<AuditDataItemPersistent> Auditoria
        {
            get
            {
                if (auditoria == null)
                {
                    auditoria = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditoria;
            }
        }


        public new static FieldsClass Fields
        {
            get
            {
                if (ReferenceEquals(_fields, null))
                {
                    _fields = new FieldsClass();
                }

                return _fields;
            }
        }

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [Association("Adjunto-Persona")]
        //[VisibleInDetailView(false)]
        public Persona Persona
        {
            get => _persona;
            set => SetPropertyValue(nameof(Persona), ref _persona, value);
        }

        [Association("Solicitud-Adjuntos")]
        public Solicitud Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        [Association("InmuebleTasacion-Adjuntos")]
        public InmuebleTasacion InmuebleTasacion
        {
            get => _inmuebleTasacion;
            set => SetPropertyValue(nameof(InmuebleTasacion), ref _inmuebleTasacion, value);
        }

        [Association("Inmueble-Adjuntos")]
        public Inmueble Inmueble
        {
            get => _inmueble;
            set => SetPropertyValue(nameof(Inmueble), ref _inmueble, value);
        }

        [ImmediatePostData(true)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public TipoAdjunto TipoAdjunto
        {
            get => _tipoAdjunto;
            set => SetPropertyValue(nameof(TipoAdjunto), ref _tipoAdjunto, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public Usuario AdjuntadoPor
        {
            get => _adjuntadoPor;
            set => SetPropertyValue(nameof(AdjuntadoPor), ref _adjuntadoPor, value);
        }

        [Delayed, ImageEditor, VisibleInListView(true), VisibleInDetailView(true), ModelDefault("AllowEdit", "false")]
        [Appearance(nameof(Imagen), Visibility = ViewItemVisibility.Hide, Criteria = "IsNull(Imagen)", Context = "any")]

        public byte[] Imagen {
            get => GetDelayedPropertyValue<byte[]>("Imagen");
            set => SetDelayedPropertyValue("Imagen", value);
        }


        [Action( Caption = "Asignar Tipo de Adjunto", AutoCommit = true)]
        public void AsignarTipoAdjuntoAction(AsignarTipoAdjunto parameters)
        {
            this.TipoAdjunto = this.Session.GetObjectByKey<TipoAdjunto>(parameters.TipoAdjunto.Oid);
        }

       /* [Action(Caption = "Asignar a Persona", AutoCommit = true)]
        public void AsignarPersonaAction(AsignarPersona parameters)
        {
            /*parameters = new AsignarPersona(this.Solicitud);
            this.Persona = this.Session.GetObjectByKey<Persona>(parameters.Persona.Oid);   
        }  */

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Adjuntado => new OperandProperty(GetNestedName("Adjuntado"));

            public PersistentBase.FieldsClass Archivo => new PersistentBase.FieldsClass(GetNestedName("Archivo"));

            public Consulta.FieldsClass Consulta => new Consulta.FieldsClass(GetNestedName("Consulta"));

            public OperandProperty Descripcion => new OperandProperty(GetNestedName("Descripcion"));

            public Empresa.FieldsClass Empresa => new Empresa.FieldsClass(GetNestedName("Empresa"));

            public OperandProperty Fecha => new OperandProperty(GetNestedName("Fecha"));

            public OperandProperty ObjectSpace => new OperandProperty(GetNestedName("ObjectSpace"));

            public Persona.FieldsClass Persona => new Persona.FieldsClass(GetNestedName("Persona"));

            public Solicitud.FieldsClass Solicitud => new Solicitud.FieldsClass(GetNestedName("Solicitud"));

            public InmuebleTasacion.FieldsClass InmuebleTasacion => new InmuebleTasacion.FieldsClass(GetNestedName("InmuebleTasacion"));

            public TipoAdjunto.FieldsClass TipoAdjunto => new TipoAdjunto.FieldsClass(GetNestedName("TipoAdjunto"));
        }

        [NonPersistent]
        public class AsignarTipoAdjunto
        {
            public AsignarTipoAdjunto() { }
            public TipoAdjunto TipoAdjunto { get; set; }
        }

        [NonPersistent]
        public class AsignarPersona: BaseObject
        {
            public AsignarPersona(Session session) : base(session)
            {
            }

            public override void AfterConstruction() => base.AfterConstruction();

            [Browsable(false)]
            public Solicitud Solicitud{ get; set; }

            [DataSourceCriteria("Solicitudes[Solicitud='@This.Solicitud']")]
            public Persona Persona{ get; set; }
        }

        [NonPersistent]
        public class AsignarTasacion : BaseObject
        {
            public AsignarTasacion(Session session) : base(session)
            {
            }

            public override void AfterConstruction() => base.AfterConstruction();

            [Browsable(false)]
            public Inmueble Inmueble{ get; set; }

            [DataSourceCriteria("Inmueble='@This.Inmueble' and EsValorizacion = false")]
            public InmuebleTasacion Tasacion { get; set; }
        }

        [NonPersistent]
        public class AsignarInmueble : BaseObject
        {
            public AsignarInmueble(Session session) : base(session)
            {
            }

            public override void AfterConstruction() => base.AfterConstruction();

            [Browsable(false)]
            public Solicitud Solicitud { get; set; }

            [DataSourceCriteria("Solicitudes[Oid = '@This.Solicitud.Oid']")]
            public Inmueble Inmueble{ get; set; }
        }
    }
}

