using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class NuevaConsulta : BaseObject
    {
        static FieldsClass _Fields;
        private string _documento;
        private DateTime _fecha;
        private Modelo _modelo;
        private Pais _paisdocumento;
        private TipoDocumento _tipodocumento;
        private string _usuario;

        public NuevaConsulta(Session session) : base(session) => GetClienteUser.Register();

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _fecha = DateTime.Now;
            _usuario = SecuritySystem.CurrentUserName;
            _tipodocumento = Session.FindObject<TipoDocumento>(CriteriaOperator.Parse("Codigo = 'CI'"));
            _paisdocumento = Session.FindObject<Pais>(CriteriaOperator.Parse("Codigo = 'PY'"));
        }

        [Association("NuevaConsulta-AdjuntosMobile")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<AdjuntoMobile> Adjuntos => GetCollection<AdjuntoMobile>(nameof(Adjuntos));

        [Size(30)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("Número de documento de identidad")]
        public string Documento
        {
            get => _documento;
            set => SetPropertyValue(nameof(Documento), ref _documento, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [NonCloneable]
        public DateTime Fecha
        {
            get => _fecha;
            set => SetPropertyValue(nameof(Fecha), ref _fecha, value);
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

        //[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("Modelo de regla de negocios a aplicar")]
        //[DataSourceCriteria("Cliente=GetClienteUser() and TipoProducto='@This.TipoProducto.DescripcionPDN' and Vigente = true")]
        public Modelo Modelo
        {
            get => _modelo;
            set => SetPropertyValue(nameof(Modelo), ref _modelo, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("País emisor del documento de identidad")]
        public Pais PaisDocumento
        {
            get => _paisdocumento;
            set => SetPropertyValue(nameof(PaisDocumento), ref _paisdocumento, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("Tipo de documento de identidad")]
        public TipoDocumento TipoDocumento
        {
            get => _tipodocumento;
            set => SetPropertyValue(nameof(TipoDocumento), ref _tipodocumento, value);
        }

        [Size(100)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName(nameof(Usuario))]
        [ToolTip("Usuario que realiza la consulta")]
        [NonCloneable]
        public string Usuario
        {
            get => _usuario;
            set => SetPropertyValue(nameof(Usuario), ref _usuario, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Adjuntos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Adjuntos"));
                }
            }

            public OperandProperty Documento
            {
                get
                {
                    return new OperandProperty(GetNestedName("Documento"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public Modelo.FieldsClass Modelo
            {
                get
                {
                    return new Modelo.FieldsClass(GetNestedName("Modelo"));
                }
            }

            public Pais.FieldsClass PaisDocumento
            {
                get
                {
                    return new Pais.FieldsClass(GetNestedName("PaisDocumento"));
                }
            }

            public TipoDocumento.FieldsClass TipoDocumento
            {
                get
                {
                    return new TipoDocumento.FieldsClass(GetNestedName("TipoDocumento"));
                }
            }

            public OperandProperty Usuario
            {
                get
                {
                    return new OperandProperty(GetNestedName("Usuario"));
                }
            }
        }
    }
}