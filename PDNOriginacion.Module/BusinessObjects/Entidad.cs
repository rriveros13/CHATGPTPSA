using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Nombre")]
    public class Entidad : BaseObject
    {
        static FieldsClass _Fields;
        private string _telefonos;
        private string nombre;

        public Entidad(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
        public Entidad GetOrCreate(Session session, string banco)
        {
            Entidad e = Session.FindObject<Entidad>(Entidad.Fields.Nombre == banco.ToUpper());

            if(e == null)
            {
                e = new Entidad(session) { Nombre = banco };
                e.Save();
            }

            return e;
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
        public string Nombre
        {
            get => nombre;
            set
            {
                bool cambio = SetPropertyValue(nameof(Nombre), ref nombre, value);
                if(!IsSaving && !IsLoading && cambio)
                {
                    if(!string.IsNullOrEmpty(nombre))
                    {
                        nombre = nombre.ToUpper().Trim();
                    }
                }
            }
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [Size(10)]
        public string Telefonos
        {
            get => _telefonos;
            set => SetPropertyValue(nameof(Telefonos), ref _telefonos, value);
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

            public OperandProperty Nombre
            {
                get
                {
                    return new OperandProperty(GetNestedName("Nombre"));
                }
            }

            public OperandProperty Telefonos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Telefonos"));
                }
            }
        }
    }
}