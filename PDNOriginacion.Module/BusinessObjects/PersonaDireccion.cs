using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Direccion")]
    public class PersonaDireccion : BaseObject
    {
        static FieldsClass _Fields;
        private Direccion _direccion;
        private Persona _persona;
        private TipoDireccion _tipo;
        private bool _principal;

        public PersonaDireccion(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            _tipo = Session.FindObject<TipoDireccion>(TipoDireccion.Fields.Codigo == "P");
            _principal = true;
        }

        protected override void OnSaving()
        {
            if (this.Principal)
            {
                foreach (PersonaDireccion pd in Persona.Direcciones)
                {
                    if (pd.Oid == Oid)
                    {
                        continue;
                    }

                    pd.Principal = false;
                    pd.Save();
                }
            }

            base.OnSaving();
        }

        [Association("Direccion-PersonaDireccion")]
        public Direccion Direccion
        {
            get => _direccion;
            set => SetPropertyValue(nameof(Direccion), ref _direccion, value);
        }

        public bool Principal
        {
            get => _principal;
            set => SetPropertyValue(nameof(Principal), ref _principal, value);
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

        [Association("Persona-PersonaDireccion")]
        public Persona Persona
        {
            get => _persona;
            set => SetPropertyValue(nameof(Persona), ref _persona, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public TipoDireccion Tipo
        {
            get => _tipo;
            set => SetPropertyValue(nameof(Tipo), ref _tipo, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public Direccion.FieldsClass Direccion
            {
                get
                {
                    return new Direccion.FieldsClass(GetNestedName("Direccion"));
                }
            }

            public Persona.FieldsClass Persona
            {
                get
                {
                    return new Persona.FieldsClass(GetNestedName("Persona"));
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
