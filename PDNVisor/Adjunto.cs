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

namespace PDNVisor
{
    [DefaultProperty("Descripcion")]
    [ImageName("BO_Note")]
    public class Adjunto : BaseObject, IObjectSpaceLink
    {
        string hash;
        GeoLocalizacion geoLocalizacion;
        static FieldsClass _Fields;
        private bool _adjuntado;
        private ITTI.FileDataITTI _archivo;
        private Consulta _consulta;
        private string _descripcion;
        private Empresa _empresa;
        private DateTime _fecha = DateTime.Now;
        private Persona _persona;
        private Solicitud _solicitud;
        private InmuebleTasacion _inmuebleTasacion;
        private TipoAdjunto _tipoAdjunto;


        public Adjunto(Session session) : base(session)
        {
            //if (this.Oid.Equals(Guid.Empty) && !this.IsSaving && !this.IsLoading)
            //_usuariocreacion = Session.GetObjectByKey<SecuritySystemUser>(SecuritySystem.CurrentUserId);
        }

        protected override void OnSaving()
        {
            Adjuntado = Archivo?.Content != null;

            WFAdjunto confAdjunto = Solicitud?.Producto?.Adjuntos?.Where(a => a.TipoAdjunto.Oid == TipoAdjunto.Oid)
                .FirstOrDefault();

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

            if(Archivo != null)
            {
                if(Archivo.Content != null)
                {
                    if (Hash != Archivo.Hash)
                    {
                        MemoryStream ms = new MemoryStream(Archivo.Content);
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
                if (GeoLocalizacion != null)
                {
                    GeoLocalizacion.Delete();
                    GeoLocalizacion = null;
                }
            }
            base.OnSaving();
        }

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
            set => SetPropertyValue(nameof(Adjuntado), ref _adjuntado, value);
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
        //[VisibleInListView(false), VisibleInDetailView(true)]
        public string Hash
        {
            get => hash;
            set => SetPropertyValue(nameof(Hash), ref hash, value);
        }
        

        public new static FieldsClass Fields
        {
            get
            {
                if(ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
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

        [ImmediatePostData(true)]
        [RuleRequiredField(DefaultContexts.Save)]
        public TipoAdjunto TipoAdjunto
        {
            get => _tipoAdjunto;
            set => SetPropertyValue(nameof(TipoAdjunto), ref _tipoAdjunto, value);
        }

     
        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Adjuntado
            {
                get
                {
                    return new OperandProperty(GetNestedName("Adjuntado"));
                }
            }

            public PersistentBase.FieldsClass Archivo
            {
                get
                {
                    return new PersistentBase.FieldsClass(GetNestedName("Archivo"));
                }
            }

            public Consulta.FieldsClass Consulta
            {
                get
                {
                    return new Consulta.FieldsClass(GetNestedName("Consulta"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public Empresa.FieldsClass Empresa
            {
                get
                {
                    return new Empresa.FieldsClass(GetNestedName("Empresa"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public Persona.FieldsClass Persona
            {
                get
                {
                    return new Persona.FieldsClass(GetNestedName("Persona"));
                }
            }

            public Solicitud.FieldsClass Solicitud
            {
                get
                {
                    return new Solicitud.FieldsClass(GetNestedName("Solicitud"));
                }
            }
            

            public InmuebleTasacion.FieldsClass InmuebleTasacion
            {
                get
                {
                    return new InmuebleTasacion.FieldsClass(GetNestedName("InmuebleTasacion"));
                }
            }

            public TipoAdjunto.FieldsClass TipoAdjunto
            {
                get
                {
                    return new TipoAdjunto.FieldsClass(GetNestedName("TipoAdjunto"));
                }
            }
        }
    }
}

