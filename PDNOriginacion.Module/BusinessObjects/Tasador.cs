using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("NombreCompleto")]
    public class Tasador : BaseObject
    {
        static FieldsClass _Fields;
        private int _codigo;
        private string _nombreCompleto;

        public Tasador(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public int Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
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

        //[RuleRequiredField(DefaultContexts.Save)]
        public string NombreCompleto
        {
            get => _nombreCompleto;
            set => SetPropertyValue(nameof(NombreCompleto), ref _nombreCompleto, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Activo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Activo"));
                }
            }

            public Ciudad.FieldsClass Ciudad
            {
                get
                {
                    return new Ciudad.FieldsClass(GetNestedName("Ciudad"));
                }
            }

            public OperandProperty Codigo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Codigo"));
                }
            }

            public OperandProperty Nombre
            {
                get
                {
                    return new OperandProperty(GetNestedName("Nombre"));
                }
            }
        }
    }
}