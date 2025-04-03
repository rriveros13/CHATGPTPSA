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
    public class TipoAdjunto : BaseObject
    {
        static FieldsClass _Fields;
        private string _codigo;
        private string _descripcion;
        private int _porcentajeMatching;
        private bool _usarvalidador;
        private int _vigencia;

        public TipoAdjunto(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _vigencia = 0;
        }

        [Size(30)]
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

        public int PorcentajeMatching
        {
            get => _porcentajeMatching;
            set => SetPropertyValue(nameof(PorcentajeMatching), ref _porcentajeMatching, value);
        }

        public bool UsarValidador
        {
            get => _usarvalidador;
            set => SetPropertyValue(nameof(UsarValidador), ref _usarvalidador, value);
        }

        [ToolTip("Vigencia en días (0, No Expira)")]
        public int Vigencia
        {
            get => _vigencia;
            set => SetPropertyValue(nameof(Vigencia), ref _vigencia, value);
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

            public OperandProperty PorcentajeMatching
            {
                get
                {
                    return new OperandProperty(GetNestedName("PorcentajeMatching"));
                }
            }

            public OperandProperty UsarValidador
            {
                get
                {
                    return new OperandProperty(GetNestedName("UsarValidador"));
                }
            }

            public OperandProperty Vigencia
            {
                get
                {
                    return new OperandProperty(GetNestedName("Vigencia"));
                }
            }
        }
    }
}
