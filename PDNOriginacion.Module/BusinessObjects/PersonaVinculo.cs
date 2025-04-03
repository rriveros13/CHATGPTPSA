using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Vinculo")]
    public class PersonaVinculo : BaseObject
    {
        static FieldsClass _Fields;
        private Persona _vinculo;
        private Persona _persona;
        private Parentezco _parentezco;

        public PersonaVinculo(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();

            _parentezco = Session.FindObject<Parentezco>(Parentezco.Fields.Nombre == "Esposo/a");
        }

        protected override void OnDeleted()
        {
            base.OnDeleted();

            if (Parentezco.Nombre == "Esposo/a")
            {
                PersonaVinculo personaVinculada = Session.FindObject<PersonaVinculo>(PersonaVinculo.Fields.Persona == Vinculo);
                if (personaVinculada != null)
                    personaVinculada.Delete();
            }
        }

        [Association("Persona-PersonaVinculo")]
        public Persona Persona
        {
            get => _persona;
            set => SetPropertyValue(nameof(Persona), ref _persona, value);
        }

        public Persona Vinculo
        {
            get => _vinculo;
            set => SetPropertyValue(nameof(Vinculo), ref _vinculo, value);
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
                if (ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
            }
        }

        [XafDisplayName("Parentesco")]
        public Parentezco Parentezco
        {
            get => _parentezco;
            set => SetPropertyValue(nameof(Parentezco), ref _parentezco, value);
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
