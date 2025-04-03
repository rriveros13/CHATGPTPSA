using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Codigo")]
    public class MotivoRechazo : BaseObject
    {
        static FieldsClass _Fields;
        private string _codigo;
        private string _descripcion;

        public MotivoRechazo(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [Size(25)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue]
        [XafDisplayName("Código")]
        public string Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        [Size(250)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName("Descripción")]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
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

            public OperandProperty Codigo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Codigo"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }
        }
    }
}
