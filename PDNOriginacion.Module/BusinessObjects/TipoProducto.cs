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
    [DefaultProperty("Descripcion")]
    [RuleCriteria("DelTipoProducto", DefaultContexts.Delete, "Modelos.Count == 0 and Productos.Count == 0", "No se puede eliminar el Tipo Producto porque tiene datos asociados (Modelos, Productos, etc...)!", SkipNullOrEmptyValues = true)]
    public class TipoProducto : BaseObject
    {
        static FieldsClass _Fields;
        private string _codigo;
        private string _descripcion;

        public TipoProducto(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [Size(3)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [XafDisplayName("Código")]
        [ToolTip("Código del tipo de documento")]
        public string Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        [Size(100)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [XafDisplayName("Descripción")]
        [ToolTip("Descripción del tipo de documento")]
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

        [Association("TipoProductos-Modelo")]
        public XPCollection<Modelo> Modelos => GetCollection<Modelo>(nameof(Modelos));

        [Association("TipoProductos-Producto")]
        public XPCollection<Producto> Productos => GetCollection<Producto>(nameof(Productos));

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

            public OperandProperty Modelos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Modelos"));
                }
            }

            public OperandProperty Productos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Productos"));
                }
            }

            public OperandProperty Auditoria
            {
                get
                {
                    return new OperandProperty(GetNestedName("Auditoria"));
                }
            }
        }
    }
}