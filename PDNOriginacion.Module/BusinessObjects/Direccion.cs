using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("CalleNumero")]
    public class Direccion : BaseObject
    {
        Inmueble inmueble;
        static FieldsClass _Fields;
        private string _apartamento;
        private Barrio _barrio;
        private Ciudad _ciudad;
        private Departamento _departamento;
        private string _esquina1;
        private string _esquina2;
        private GeoLocalizacion _geoLocalizacion;
        private string _obs;
        private string _piso;
        private string _calle;
        private string _numero;
        private Pais _pais;
        private TipoInmueble _tipoInmueble;
        //private TipoDireccion _tipo;

        public Direccion(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _pais = Session.FindObject<Pais>(Pais.Fields.Codigo == "1");
            _departamento = Session.FindObject<Departamento>(Departamento.Fields.Default == (CriteriaOperator)true);
            if (_departamento != null)
            {
                _ciudad = Session.FindObject<Ciudad>(Ciudad.Fields.Departamento == _departamento & Ciudad.Fields.Default == (CriteriaOperator)true);
            }

            _calle = "Sin asignar";
            _numero = "Sin Nro";

        }
        public TipoInmueble TipoInmueble
        {
            get => _tipoInmueble;
            set => SetPropertyValue(nameof(TipoInmueble), ref _tipoInmueble, value);
        }

        [Size(5)]
        public string Apartamento
        {
            get => _apartamento;
            set => SetPropertyValue(nameof(Apartamento), ref _apartamento, value);
        }

        [ImmediatePostData]
        //[RuleRequiredField(DefaultContexts.Save)]
        public Pais Pais
        {
            get => _pais;
            set
            {
                bool cambio = SetPropertyValue(nameof(Pais), ref _pais, value);
                if (!IsLoading && !IsSaving && cambio)
                {
                    if (_pais != null)
                    {
                        _departamento = null;
                    }
                    OnChanged("Departamento");
                }
            }
        }

        [DataSourceProperty("Ciudad.Barrios")]
        public Barrio Barrio
        {
            get => _barrio;
            set => SetPropertyValue(nameof(Barrio), ref _barrio, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [Size(70)]
        public string Calle
        {
            get => _calle;
            set => SetPropertyValue(nameof(Calle), ref _calle, value);
        }

        //[PersistentAlias("Concat([Calle], ' ', [Numero], ' - ', [Ciudad.Nombre])")]
        [PersistentAlias("Concat(Calle, ' ', Numero, ' - ', Departamento.Nombre, ' - ', Ciudad.Nombre, Iif(!IsNull(Barrio), ' - ' + Barrio.Nombre, ''))")]
        [VisibleInDetailView(false)]
        public string CalleNumero => (string)EvaluateAlias(nameof(CalleNumero));

        [ImmediatePostData]
        //[RuleRequiredField(DefaultContexts.Save)]
        [DataSourceProperty("Departamento.Ciudades")]
        public Ciudad Ciudad
        {
            get => _ciudad;
            set
            {
                bool cambio = SetPropertyValue(nameof(Ciudad), ref _ciudad, value);
                if (!IsLoading && !IsSaving && cambio)
                {
                    _barrio = null;
                    OnChanged("Barrio");
                }
            }
        }

        [ImmediatePostData]
        //[RuleRequiredField(DefaultContexts.Save)]
        [DataSourceProperty("Pais.Departamentos")]
        public Departamento Departamento
        {
            get => _departamento;
            set
            {
                bool cambio = SetPropertyValue(nameof(Departamento), ref _departamento, value);
                if (!IsLoading && !IsSaving && cambio)
                {
                    if (_departamento != null)
                    {
                        _ciudad = Session.FindObject<Ciudad>(Ciudad.Fields.Departamento == _departamento & Ciudad.Fields.Default == (CriteriaOperator)true);
                    }
                    else
                    {
                        _ciudad = null;
                    }
                    OnChanged("Ciudad");
                }
            }
        }

        [Size(150)]
        public string Esquina1
        {
            get => _esquina1;
            set => SetPropertyValue(nameof(Esquina1), ref _esquina1, value);
        }

        [Size(150)]
        public string Esquina2
        {
            get => _esquina2;
            set => SetPropertyValue(nameof(Esquina2), ref _esquina2, value);
        }

        public new static FieldsClass Fields
        {
            get
            {
                if (ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
            }
        }

        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        //[Association("Direccion-GeoLocalizacion")]
        [VisibleInListView(false)]
        public GeoLocalizacion Localizacion
        {
            get => _geoLocalizacion;
            set
            {
                SetPropertyValue(nameof(Localizacion), ref _geoLocalizacion, value);
                if (IsLoading || IsSaving)
                {
                    return;
                }

                _geoLocalizacion.Title = CalleNumero;
                _geoLocalizacion.Direccion = this;
                _geoLocalizacion.Save();
            }
        }

        ////[RuleRequiredField(DefaultContexts.Save)]
        [Size(8)]
        public string Numero
        {
            get => _numero;
            set => SetPropertyValue(nameof(Numero), ref _numero, value);
        }

        [Appearance("InmuebleNulo", Visibility = ViewItemVisibility.Hide, Criteria = "IsNull(Inmueble)", Context = "DetailView")]
        public Inmueble Inmueble
        {
            get => inmueble;
            set => SetPropertyValue(nameof(Inmueble), ref inmueble, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Observaciones
        {
            get => _obs;
            set => SetPropertyValue(nameof(Observaciones), ref _obs, value);
        }

        [Association("Direccion-PersonaDireccion")]
        [Appearance("", Visibility = ViewItemVisibility.Hide)]
        public XPCollection<PersonaDireccion> Personas => GetCollection<PersonaDireccion>(nameof(Personas));

        [Size(5)]
        public string Piso
        {
            get => _piso;
            set => SetPropertyValue(nameof(Piso), ref _piso, value);
        }

        /*//[RuleRequiredField(DefaultContexts.Save)]
        public TipoDireccion Tipo
        {
            get => _tipo;
            set => SetPropertyValue(nameof(Tipo), ref _tipo, value);
        } */

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

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Apartamento
            {
                get
                {
                    return new OperandProperty(GetNestedName("Apartamento"));
                }
            }

            public Barrio.FieldsClass Barrio
            {
                get
                {
                    return new Barrio.FieldsClass(GetNestedName("Barrio"));
                }
            }

            public OperandProperty Calle
            {
                get
                {
                    return new OperandProperty(GetNestedName("Calle"));
                }
            }

            public OperandProperty CalleNumero
            {
                get
                {
                    return new OperandProperty(GetNestedName("CalleNumero"));
                }
            }

            public Ciudad.FieldsClass Ciudad
            {
                get
                {
                    return new Ciudad.FieldsClass(GetNestedName("Ciudad"));
                }
            }

            public Departamento.FieldsClass Departamento
            {
                get
                {
                    return new Departamento.FieldsClass(GetNestedName("Departamento"));
                }
            }

            public OperandProperty Esquina1
            {
                get
                {
                    return new OperandProperty(GetNestedName("Esquina1"));
                }
            }

            public OperandProperty Esquina2
            {
                get
                {
                    return new OperandProperty(GetNestedName("Esquina2"));
                }
            }

            public GeoLocalizacion.FieldsClass Localizacion
            {
                get
                {
                    return new GeoLocalizacion.FieldsClass(GetNestedName("Localizacion"));
                }
            }

            public OperandProperty Numero
            {
                get
                {
                    return new OperandProperty(GetNestedName("Numero"));
                }
            }

            public OperandProperty Observaciones
            {
                get
                {
                    return new OperandProperty(GetNestedName("Observaciones"));
                }
            }

            public OperandProperty Personas
            {
                get
                {
                    return new OperandProperty(GetNestedName("Personas"));
                }
            }

            public OperandProperty Piso
            {
                get
                {
                    return new OperandProperty(GetNestedName("Piso"));
                }
            }
        }
    }
}