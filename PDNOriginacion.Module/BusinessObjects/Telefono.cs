using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(TelefonoCompleto))]
    
    public class Telefono : BaseObject
    {
        static FieldsClass _Fields;
        private string _numero;
        private Persona _persona;
        private bool _preferido;
        private Prefijo _prefijo;
        private string _telefonoCompleto;
        private TipoTelefono _tipoTelefono;
        private bool _whatsapp;
        private GrupoTelefono _tipo;

        public Telefono(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _whatsapp = true;
            _tipoTelefono = Session.FindObject<TipoTelefono>(TipoTelefono.Fields.Codigo == "P");
            _preferido = true;
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

        //[RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData]
        public GrupoTelefono Tipo
        {
            get => _tipo;
            set
            {
                SetPropertyValue(nameof(TipoTelefono), ref _tipo, value);
                if (IsLoading)
                {
                    return;
                }

                _whatsapp = false;
                OnChanged(nameof(Whatsapp));
            }
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

        [Size(10)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string Numero
        {
            get => _numero;
            set
            {
                bool cambio = SetPropertyValue(nameof(Numero), ref _numero, value);
                if(IsLoading || IsSaving || !cambio)
                {
                    return;
                }

                _telefonoCompleto = string.Concat(_prefijo?.Codigo, _numero);
                OnChanged(nameof(_telefonoCompleto));
            }
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public TipoTelefono TipoTelefono
        {
            get => _tipoTelefono;
            set => SetPropertyValue(nameof(TipoTelefono), ref _tipoTelefono, value);
        } 

        [Association("Persona-Telefono")]
        public Persona Persona
        {
            get => _persona;
            set => SetPropertyValue(nameof(Persona), ref _persona, value);
        }

        public bool Preferido
        {
            get => _preferido;
            set
            {
                SetPropertyValue(nameof(Preferido), ref _preferido, value);             
            }
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            Persona.CambiarEstadoPersona(this.Persona, true);
        }


        protected override void OnSaving()
        {
            //Validar que no exista otra persona con el mismo teléfono
            //XPCollection<Telefono> telefonos = new XPCollection<Telefono>(this.Session);

            //if (telefonos.Where(t => t.Persona != this.Persona && t.TelefonoCompleto == this.TelefonoCompleto).Count() > 0)
            //    throw new System.Exception("Ya existe un cliente con este número de teléfono."); ;
            
            if (this.Preferido)
            {
                foreach (Telefono pt in Persona.Telefonos)
                {
                    if (pt.Oid == Oid)
                    {
                        continue;
                    }

                    pt.Preferido = false;
                    pt.Save();
                }
            }

            base.OnSaving();
        }

        [ModelDefault("AllowEdit", "false")]
        [Size(50)]
        //[RuleUniqueValue("TelDuplicado", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues = true)]
        public string TelefonoCompleto
        {
            get => _telefonoCompleto;
            set => SetPropertyValue(nameof(TelefonoCompleto), ref _telefonoCompleto, value);
        }



        public bool Whatsapp
        {
            get => _whatsapp;
            set => SetPropertyValue(nameof(Whatsapp), ref _whatsapp, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Numero
            {
                get
                {
                    return new OperandProperty(GetNestedName("Numero"));
                }
            }

            public OperandProperty ParticularoLaboral
            {
                get
                {
                    return new OperandProperty(GetNestedName("ParticularoLaboral"));
                }
            }

            public Persona.FieldsClass Persona
            {
                get
                {
                    return new Persona.FieldsClass(GetNestedName("Persona"));
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

            public OperandProperty TelefonoCompleto
            {
                get
                {
                    return new OperandProperty(GetNestedName("TelefonoCompleto"));
                }
            }

            public OperandProperty Tipo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Tipo"));
                }
            }

            public OperandProperty Whatsapp
            {
                get
                {
                    return new OperandProperty(GetNestedName("Whatsapp"));
                }
            }
        }
    }
}