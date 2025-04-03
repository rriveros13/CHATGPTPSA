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
    [RuleCriteria("DelPorCliente", DefaultContexts.Delete, "Usuarios.Count == 0", "No se puede eliminar el cliente porque tiene usuarios asociados!", SkipNullOrEmptyValues = true)]
    public class Cliente : BaseObject
    {
        static FieldsClass _Fields;
        private string _codigo;
        private string _nombre;

        public Cliente(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [Size(6)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public string Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
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

        [Size(100)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public string Nombre
        {
            get => _nombre;
            set => SetPropertyValue(nameof(Nombre), ref _nombre, value);
        }

        [Association("Cliente-Usuario", typeof(Usuario))]
        public XPCollection Usuarios => GetCollection(nameof(Usuarios));

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

            public OperandProperty Nombre
            {
                get
                {
                    return new OperandProperty(GetNestedName("Nombre"));
                }
            }

            public OperandProperty Usuarios
            {
                get
                {
                    return new OperandProperty(GetNestedName("Usuarios"));
                }
            }
        }
    }
}
