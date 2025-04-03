using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Empresa")]
    [RuleObjectExists("AnotherSingletonExists", DefaultContexts.Save, "True", InvertResult = true, CustomMessageTemplate = "Ya existe un objeto configuracion!")]
    [RuleCriteria("CannotDeleteSingleton", DefaultContexts.Delete, "False", CustomMessageTemplate = "No se puede eliminar la configuración!")]
    public class Configuracion : BaseObject
    {
        static FieldsClass _Fields;
        private XPCollection<AuditDataItemPersistent> auditTrail;
        private string direccion;
        private string email;
        private string empresa;
        private string razonsocial;
        private string ruc;
        private SecurityPolicy securityPolicy;
        private string telefono1;

        public Configuracion(Session session) : base(session)
        {
        }

        protected override void OnDeleting() => throw new UserFriendlyException("No se puede eliminar la configuración!");

        public static Configuracion GetInstance(IObjectSpace objectSpace)
        {
            Configuracion result = objectSpace.FindObject<Configuracion>(null);

            return result;
        }
        [Action(Caption = "Eliminar Registros Borrados")]
        public void PurgeDeletedRecords() => Session.PurgeDeletedObjects();

        public XPCollection<AuditDataItemPersistent> AuditTrail => auditTrail ??
            (auditTrail = AuditedObjectWeakReference.GetAuditTrail(Session, this));

        [Size(400)]
        public string Direccion
        {
            get => direccion;
            set => SetPropertyValue(nameof(Direccion), ref direccion, value);
        }

        [Size(60)]
        public string Email
        {
            get => email;
            set => SetPropertyValue(nameof(Email), ref email, value);
        }

        [Size(100)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string Empresa
        {
            get => empresa;
            set => SetPropertyValue(nameof(Empresa), ref empresa, value);
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

        [Association(nameof(Parametros))]
        [Aggregated]
        public XPCollection<Parametro> Parametros
        {
            get
            {
                XPCollection<Parametro> collection = GetCollection<Parametro>(nameof(Parametros));
                return collection;
            }
        }

        [Size(100)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string RazonSocial
        {
            get => razonsocial;
            set => SetPropertyValue(nameof(RazonSocial), ref razonsocial, value);
        }

        [Size(10)]
        public string Ruc
        {
            get => ruc;
            set => SetPropertyValue(nameof(Ruc), ref ruc, value);
        }

        [Aggregated]
        [ExpandObjectMembers(ExpandObjectMembers.Never)]
        [VisibleInListView(false)]
        public SecurityPolicy SecurityPolicy
        {
            get => securityPolicy;
            set => SetPropertyValue(nameof(securityPolicy), ref securityPolicy, value);
        }

        [Size(20)]
        public string Telefono
        {
            get => telefono1;
            set => SetPropertyValue(nameof(Telefono), ref telefono1, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty AuditTrail
            {
                get
                {
                    return new OperandProperty(GetNestedName("AuditTrail"));
                }
            }

            public OperandProperty Direccion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Direccion"));
                }
            }

            public OperandProperty Email
            {
                get
                {
                    return new OperandProperty(GetNestedName("Email"));
                }
            }

            public OperandProperty Empresa
            {
                get
                {
                    return new OperandProperty(GetNestedName("Empresa"));
                }
            }

            public OperandProperty Parametros
            {
                get
                {
                    return new OperandProperty(GetNestedName("Parametros"));
                }
            }

            public OperandProperty RazonSocial
            {
                get
                {
                    return new OperandProperty(GetNestedName("RazonSocial"));
                }
            }

            public OperandProperty Ruc
            {
                get
                {
                    return new OperandProperty(GetNestedName("Ruc"));
                }
            }

            public SecurityPolicy.FieldsClass SecurityPolicy
            {
                get
                {
                    return new SecurityPolicy.FieldsClass(GetNestedName("SecurityPolicy"));
                }
            }

            public OperandProperty Telefono
            {
                get
                {
                    return new OperandProperty(GetNestedName("Telefono"));
                }
            }
        }
    }
}