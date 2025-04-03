using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Telefono")]
    public class EmpresaTelefono : BaseObject
    {
        static FieldsClass _Fields;
        private Empresa _empresa;
        private string _horario;
        private string _interno;
        private string _numero;
        private bool _preferido;
        private Prefijo _prefijo;
        private TipoTelefono _tipoTelefono;
        private GrupoTelefono _tipo;
        private string _telefonoCompleto;

        public EmpresaTelefono(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _preferido = true;
            _tipoTelefono = Session.FindObject<TipoTelefono>(TipoTelefono.Fields.Codigo == "L");
        }

        [Association("Empresa-EmpresaTelefono")]
        public Empresa Empresa
        {
            get => _empresa;
            set => SetPropertyValue(nameof(Empresa), ref _empresa, value);
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

        [Size(50)]
        public string Horario
        {
            get => _horario;
            set => SetPropertyValue(nameof(Horario), ref _horario, value);
        }

        [Size(5)]
        public string Interno
        {
            get => _interno;
            set => SetPropertyValue(nameof(Interno), ref _interno, value);
        }

        [Size(6)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string Numero
        {
            get => _numero;
            set
            {
                bool cambio = SetPropertyValue(nameof(Numero), ref _numero, value);
                if (IsLoading || IsSaving || !cambio)
                {
                    return;
                }

                _telefonoCompleto = string.Concat(_prefijo?.Codigo, _numero);
                OnChanged(nameof(_telefonoCompleto));
            }
        }

        public bool Preferido
        {
            get => _preferido;
            set => SetPropertyValue(nameof(Preferido), ref _preferido, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [DataSourceCriteria("Tipo = '@This.Tipo'")]
        public Prefijo Prefijo
        {
            get => _prefijo;
            set
            {
                bool cambio = SetPropertyValue(nameof(Prefijo), ref _prefijo, value);
                if (IsLoading || IsSaving || !cambio)
                {
                    return;
                }

                _telefonoCompleto = string.Concat(_prefijo.Codigo, _numero);
                OnChanged(nameof(_telefonoCompleto));
            }
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData]
        public TipoTelefono TipoTelefono
        {
            get => _tipoTelefono;
            set => SetPropertyValue(nameof(TipoTelefono), ref _tipoTelefono, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData]
        public GrupoTelefono Tipo
        {
            get => _tipo;
            set => SetPropertyValue(nameof(Tipo), ref _tipo, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [Size(50)]
        public string TelefonoCompleto
        {
            get => _telefonoCompleto;
            set => SetPropertyValue(nameof(TelefonoCompleto), ref _telefonoCompleto, value);
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
        
        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public Empresa.FieldsClass Empresa
            {
                get
                {
                    return new Empresa.FieldsClass(GetNestedName("Empresa"));
                }
            }

            public OperandProperty Horario
            {
                get
                {
                    return new OperandProperty(GetNestedName("Horario"));
                }
            }

            public OperandProperty Interno
            {
                get
                {
                    return new OperandProperty(GetNestedName("Interno"));
                }
            }

            public OperandProperty Numero
            {
                get
                {
                    return new OperandProperty(GetNestedName("Numero"));
                }
            }

            public OperandProperty Preferido
            {
                get
                {
                    return new OperandProperty(GetNestedName("Preferido"));
                }
            }

            public Prefijo.FieldsClass Prefijo
            {
                get
                {
                    return new Prefijo.FieldsClass(GetNestedName("Prefijo"));
                }
            }

            public OperandProperty Tipo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Tipo"));
                }
            }
        }
    }
}