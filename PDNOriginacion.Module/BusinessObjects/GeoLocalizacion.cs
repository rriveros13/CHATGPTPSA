using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Globalization;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DeferredDeletion(Enabled = false)]
    [DefaultProperty("Coordenadas")]
    public class GeoLocalizacion : BaseObject, IMapsMarker
    {
        Adjunto adjunto;
        Direccion direccion;
        string coordenadas;
        static FieldsClass _Fields;
        private string _coordenadasManuales;
        private double latitude;
        private double longitude;
        private string title;
        private OrigenGeolocalizacion origen;

        public GeoLocalizacion(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        //[PersistentAlias("Iif(!IsNull(Latitude) and !IsNull(Longitude), Concat(ToStr(Latitude), ',' , ToStr(Longitude)), null)")]
        //[ModelDefault("AllowEdit", "false")]
        //public string Coordenadas => (string)EvaluateAlias(nameof(Coordenadas));

        [ModelDefault("AllowEdit", "false")]
        public string Coordenadas
        {
            get => coordenadas;
            set => SetPropertyValue(nameof(Coordenadas), ref coordenadas, value);
        }

        //[Association("Direccion-GeoLocalizacion")]
        [Appearance("OcultarDireccionNula", Visibility = ViewItemVisibility.Hide, Criteria = "IsNull(Direccion)", Context = "DetailView")]
        public Direccion Direccion
        {
            get => direccion;
            set => SetPropertyValue(nameof(Direccion), ref direccion, value);
        }

        [Appearance("OcultarAdjuntoNulo", Visibility = ViewItemVisibility.Hide, Criteria = "IsNull(Direccion)", Context = "DetailView")]
        //[Association("Adjunto-GeoLocalizacion")]
        public Adjunto Adjunto
        {
            get => adjunto;
            set => SetPropertyValue(nameof(Adjunto), ref adjunto, value);
        }

        [RuleRegularExpression("ValRuleCoordenadas", DefaultContexts.Save, @"^[-+]?([1-8]?\d(\.\d+)?|90(\.0+)?),\s*[-+]?(180(\.0+)?|((1[0-7]\d)|([1-9]?\d))(\.\d+)?)$")]
        public string CoordenadasManuales
        {
            get => string.Empty;
            set
            {
                string valant = _coordenadasManuales;
                value = value?.Trim();
                bool cambio = SetPropertyValue(nameof(CoordenadasManuales), ref _coordenadasManuales, value);
                if(IsLoading || IsSaving || !cambio)
                {
                    return;
                }

                if(!string.IsNullOrEmpty(_coordenadasManuales))
                {
                    string[] vcord = _coordenadasManuales.Split(',');
                    CultureInfo culture = new CultureInfo("en-US", true);
                    bool blat = double.TryParse(vcord[0], NumberStyles.Number, culture, out double lat);
                    bool blon = double.TryParse(vcord[1], NumberStyles.Number,culture, out double lon);

                    if(!blat || !blon)
                    {
                        _coordenadasManuales = valant;
                        return;
                    }
                    latitude = lat;
                    longitude = lon;
                    origen = OrigenGeolocalizacion.MANUAL;
                }
                else
                {
                    latitude = 0;
                    longitude = 0;
                }
                OnChanged(nameof(Latitude));
                OnChanged(nameof(Longitude));
                OnChanged(nameof(Origen));
            }
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
                if(ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
            }
        }

        [XafDisplayName("Latitud")]
        public double Latitude
        {
            get => latitude;
            set
            {
                SetPropertyValue(nameof(Latitude), ref latitude, value);
                ActualizarCoordenadas();
                OnChanged("Coordenadas");
            }
        }

        [XafDisplayName("Longitud")]
        public double Longitude
        {
            get => longitude;
            set
            {
                SetPropertyValue(nameof(Longitude), ref longitude, value);
                ActualizarCoordenadas();
                OnChanged("Coordenadas");
            }
        }

        private void ActualizarCoordenadas()
        {
            Coordenadas = string.Concat(Latitude.ToString().Replace(',','.'), ",", Longitude.ToString().Replace(',', '.'));
        }

        [XafDisplayName("Titulo")]
        public string Title
        {
            get => title;
            set => SetPropertyValue(nameof(Title), ref title, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public OrigenGeolocalizacion Origen
        {
            get => origen;
            set => SetPropertyValue(nameof(Origen), ref origen, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Coordenadas
            {
                get
                {
                    return new OperandProperty(GetNestedName("Coordenadas"));
                }
            }

            public OperandProperty CoordenadasManuales
            {
                get
                {
                    return new OperandProperty(GetNestedName("CoordenadasManuales"));
                }
            }

            public OperandProperty Latitude
            {
                get
                {
                    return new OperandProperty(GetNestedName("Latitude"));
                }
            }

            public OperandProperty Longitude
            {
                get
                {
                    return new OperandProperty(GetNestedName("Longitude"));
                }
            }

            public OperandProperty Title
            {
                get
                {
                    return new OperandProperty(GetNestedName("Title"));
                }
            }

            public OperandProperty UsoPosicionActual
            {
                get
                {
                    return new OperandProperty(GetNestedName("UsoPosicionActual"));
                }
            }
        }
    }
}