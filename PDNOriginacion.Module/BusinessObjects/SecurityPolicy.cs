using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultProperty(nameof(Name))]
    public class SecurityPolicy : BaseObject
    {
        static FieldsClass _Fields;
        private string bannedPasswords;
        private Configuracion configuracion;
        private bool enforcePasswordHistory;
        private int maxPasswordAge;
        private int minPasswordAge;
        private int minPasswordLenght;
        private bool passwordComplexity;
        private bool permitirAutenticacionAD;

        public SecurityPolicy(Session session) : base(session)
        {
        }

        protected override void OnDeleting() => throw new UserFriendlyException("No se puede eliminar la Política de Seguridad!");

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            minPasswordLenght = 8;
            enforcePasswordHistory = true;
            maxPasswordAge = 60;
            minPasswordAge = 1;
            passwordComplexity = true;
        }

        [Size(SizeAttribute.Unlimited)]
        public string BannedPasswords
        {
            get => bannedPasswords;
            set => SetPropertyValue(nameof(BannedPasswords), ref bannedPasswords, value);
        }

        [Browsable(false)]
        public Configuracion Configuracion
        {
            get => configuracion;
            set => SetPropertyValue(nameof(Configuracion), ref configuracion, value);
        }

        [XafDisplayName("Aplicar Historial de Contraseñas")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public bool EnforcePasswordHistory
        {
            get => enforcePasswordHistory;
            set => SetPropertyValue(nameof(EnforcePasswordHistory), ref enforcePasswordHistory, value);
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

        [XafDisplayName("Edad Máxima de Contraseña")]
        [RuleRange("", DefaultContexts.Save, 0, 999)]
        public int MaxPasswordAge
        {
            get => maxPasswordAge;
            set => SetPropertyValue(nameof(MaxPasswordAge), ref maxPasswordAge, value);
        }

        [XafDisplayName("Edad Mínima de Contraseña")]
        [RuleRange("", DefaultContexts.Save, 0, 999)]
        public int MinPasswordAge
        {
            get => minPasswordAge;
            set => SetPropertyValue(nameof(MinPasswordAge), ref minPasswordAge, value);
        }

        [XafDisplayName("Largo Mínimo de Contraseña")]
        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleRange("", DefaultContexts.Save, 0, 250)]
        [ImmediatePostData(true)]
        public int MinPasswordLenght
        {
            get => minPasswordLenght;
            set
            {
                SetPropertyValue(nameof(MinPasswordLenght), ref minPasswordLenght, value);
                if(!IsLoading && !IsSaving)
                {
                    if(minPasswordLenght < 3)
                    {
                        passwordComplexity = false;
                    }

                    OnChanged(nameof(PasswordComplexity));
                }
            }
        }

        public string Name => "POLÍTICA DE SEGURIDAD";

        [XafDisplayName("Exigir Complejidad de Contraseña")]
        //[RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData(true)]
        public bool PasswordComplexity
        {
            get => passwordComplexity;
            set
            {
                SetPropertyValue(nameof(PasswordComplexity), ref passwordComplexity, value);
                if(!IsLoading && !IsSaving)
                {
                    if(minPasswordLenght < 3)
                    {
                        minPasswordLenght = 3;
                    }

                    OnChanged(nameof(MinPasswordLenght));
                }
            }
        }

        [ImmediatePostData(true)]
        public bool PermitirAutenticacionAD
        {
            get => permitirAutenticacionAD;
            set
            {
                bool cambio = SetPropertyValue(nameof(PermitirAutenticacionAD), ref permitirAutenticacionAD, value);
                if(cambio && !IsLoading && !IsSaving)
                {
                    if (permitirAutenticacionAD == false)
                    {
                        autenticacionADPorDefecto = false;
                        OnChanged("AutenticacionADPorDefecto");
                    }
                }
            }
        }

        private bool autenticacionADPorDefecto;
        [Appearance("", Enabled = false, Criteria = "!PermitirAutenticacionAD", Context = nameof(DetailView))]
        public bool AutenticacionADPorDefecto
        {
            get => autenticacionADPorDefecto;
            set => SetPropertyValue(nameof(AutenticacionADPorDefecto), ref autenticacionADPorDefecto, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty BannedPasswords
            {
                get
                {
                    return new OperandProperty(GetNestedName("BannedPasswords"));
                }
            }

            public Configuracion.FieldsClass Configuracion
            {
                get
                {
                    return new Configuracion.FieldsClass(GetNestedName("Configuracion"));
                }
            }

            public OperandProperty EnforcePasswordHistory
            {
                get
                {
                    return new OperandProperty(GetNestedName("EnforcePasswordHistory"));
                }
            }

            public OperandProperty MaxPasswordAge
            {
                get
                {
                    return new OperandProperty(GetNestedName("MaxPasswordAge"));
                }
            }

            public OperandProperty MinPasswordAge
            {
                get
                {
                    return new OperandProperty(GetNestedName("MinPasswordAge"));
                }
            }

            public OperandProperty MinPasswordLenght
            {
                get
                {
                    return new OperandProperty(GetNestedName("MinPasswordLenght"));
                }
            }

            public OperandProperty Name
            {
                get
                {
                    return new OperandProperty(GetNestedName("Name"));
                }
            }

            public OperandProperty PasswordComplexity
            {
                get
                {
                    return new OperandProperty(GetNestedName("PasswordComplexity"));
                }
            }
        }
    }
}