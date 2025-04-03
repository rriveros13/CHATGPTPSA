using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Oid")]
    public class SolicitudMobile : XPObject, IObjectSpaceLink
    {
        static FieldsClass _Fields;
        private int _antiguedad;
        private byte[] _archivo;
        private Usuario _creadaPor;
        private string _documento;
        private int _edad;
        private EstadoSolicitud _estadoSolicitud;
        private DateTime _fecha;
        private decimal _monto;
        private Pais _paisdocumento;
        private Person _Seguimiento;
        //private string _nombreCompleto;
        //[ModelDefault("AllowEdit","false")]
        ////[RuleRequiredField(DefaultContexts.Save)]
        //public string NombreCompleto
        //{
        //    get => _nombreCompleto;
        //    set => SetPropertyValue("NombreCompleto", ref _nombreCompleto, value);
        //}
        private Persona _solicitante;
        private TipoDocumento _tipodocumento;
        private Producto producto;

        public SolicitudMobile(Session session) : base(session)
        {
        }

        [Action(Caption = "Procesar Motor", ConfirmationMessage = "¿Esta Seguro?", AutoCommit = true)]
        public void ActionProcesar() => ObjectSpace?.CommitChanges();
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _fecha = DateTime.Now;
            _estadoSolicitud = Session.FindObject<EstadoSolicitud>(CriteriaOperator.Parse("Descripcion='CREADA'"));
            _creadaPor = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
            _tipodocumento = Session.FindObject<TipoDocumento>(CriteriaOperator.Parse("Codigo = 'CI'"));
            _paisdocumento = Session.FindObject<Pais>(CriteriaOperator.Parse("Codigo = 'PY'"));
        }

        [RuleValueComparison("MRAntiguedad", DefaultContexts.Save, ValueComparisonType.GreaterThanOrEqual, 0, SkipNullOrEmptyValues = false, CustomMessageTemplate = "Antiguedad debe ser mayor o igual que cero")]
        public int Antiguedad
        {
            get => _antiguedad;
            set => SetPropertyValue(nameof(Antiguedad), ref _antiguedad, value);
        }

        public byte[] Archivo
        {
            get => _archivo;
            set => SetPropertyValue(nameof(Archivo), ref _archivo, value);
        }

        public Usuario CreadaPor
        {
            get => _creadaPor;
            set => SetPropertyValue(nameof(CreadaPor), ref _creadaPor, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public string Documento
        {
            get => _documento;
            set => SetPropertyValue(nameof(Documento), ref _documento, value);
        }

        [RuleValueComparison("MREdad", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false, CustomMessageTemplate = "Edad debe ser mayor que cero")]
        public int Edad
        {
            get => _edad;
            set => SetPropertyValue(nameof(Edad), ref _edad, value);
        }

        public EstadoSolicitud Estado
        {
            get => _estadoSolicitud;
            set => SetPropertyValue(nameof(Estado), ref _estadoSolicitud, value);
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

        [PersistentAlias(nameof(Oid))]
        [XafDisplayName(nameof(Solicitud))]
        public string IdSolicitud => EvaluateAlias(nameof(IdSolicitud)).ToString();

        [RuleValueComparison("MRMonto", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, SkipNullOrEmptyValues = false, CustomMessageTemplate = "Monto debe ser mayor que cero")]
        public decimal Monto
        {
            get => _monto;
            set => SetPropertyValue(nameof(Monto), ref _monto, value);
        }

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        //[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("País emisor del documento de identidad")]
        public Pais PaisDocumento
        {
            get => _paisdocumento;
            set => SetPropertyValue(nameof(PaisDocumento), ref _paisdocumento, value);
        }

        [ImmediatePostData(true)]
        //[Association("SolicitudMobile-Producto")]
        public Producto Producto
        {
            get => producto;
            set => SetPropertyValue(nameof(Producto), ref producto, value);
        }

        public Person Seguimiento
        {
            get => _Seguimiento;
            set => SetPropertyValue(nameof(Seguimiento), ref _Seguimiento, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public Persona Solicitante
        {
            get => _solicitante;
            set => SetPropertyValue(nameof(Solicitante), ref _solicitante, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("Tipo de documento de identidad")]
        public TipoDocumento TipoDocumento
        {
            get => _tipodocumento;
            set => SetPropertyValue(nameof(TipoDocumento), ref _tipodocumento, value);
        }

        public new class FieldsClass : XPObject.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Antiguedad
            {
                get
                {
                    return new OperandProperty(GetNestedName("Antiguedad"));
                }
            }

            public OperandProperty Archivo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Archivo"));
                }
            }

            public Usuario.FieldsClass CreadaPor
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("CreadaPor"));
                }
            }

            public OperandProperty Documento
            {
                get
                {
                    return new OperandProperty(GetNestedName("Documento"));
                }
            }

            public OperandProperty Edad
            {
                get
                {
                    return new OperandProperty(GetNestedName("Edad"));
                }
            }

            public EstadoSolicitud.FieldsClass Estado
            {
                get
                {
                    return new EstadoSolicitud.FieldsClass(GetNestedName("Estado"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public OperandProperty IdSolicitud
            {
                get
                {
                    return new OperandProperty(GetNestedName("IdSolicitud"));
                }
            }

            public OperandProperty Monto
            {
                get
                {
                    return new OperandProperty(GetNestedName("Monto"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public Pais.FieldsClass PaisDocumento
            {
                get
                {
                    return new Pais.FieldsClass(GetNestedName("PaisDocumento"));
                }
            }

            public Producto.FieldsClass Producto
            {
                get
                {
                    return new Producto.FieldsClass(GetNestedName("Producto"));
                }
            }

            public PersistentBase.FieldsClass Seguimiento
            {
                get
                {
                    return new PersistentBase.FieldsClass(GetNestedName("Seguimiento"));
                }
            }

            public Persona.FieldsClass Solicitante
            {
                get
                {
                    return new Persona.FieldsClass(GetNestedName("Solicitante"));
                }
            }

            public TipoDocumento.FieldsClass TipoDocumento
            {
                get
                {
                    return new TipoDocumento.FieldsClass(GetNestedName("TipoDocumento"));
                }
            }
        }

        //[Action(Caption = "Aprobar", ConfirmationMessage = "¿Esta Seguro?", AutoCommit = true)]
        //public void ActionAprobar()
        //{
        //    Aprobada = true;
        //    Estado = Session.FindObject<EstadoSolicitud>(CriteriaOperator.Parse("Descripcion='APROBADA'"));
        //    ObjectSpace?.CommitChanges();
        //}
    }
}