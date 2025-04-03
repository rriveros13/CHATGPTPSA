using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Descripcion")]
    public class EstadoSolicitud : BaseObject
    {
        static FieldsClass _Fields;
        private string codigo;
        private string descripcion;
        private bool noPermitirCambios;
        private bool cancelarTareas;

        public EstadoSolicitud(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            noPermitirCambios = false;
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue("Codigo-Unique", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue(nameof(Codigo), ref codigo, value);
        }

        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
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

        public bool NoPermitirCambios
        {
            get => noPermitirCambios;
            set => SetPropertyValue(nameof(NoPermitirCambios), ref noPermitirCambios, value);
        }

        public bool CancelarTareas
        {
            get => cancelarTareas;
            set => SetPropertyValue(nameof(CancelarTareas), ref cancelarTareas, value);
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

            public OperandProperty NoPermitirCambios
            {
                get
                {
                    return new OperandProperty(GetNestedName("NoPermitirCambios"));
                }
            }

            public OperandProperty CancelarTareas
            {
                get
                {
                    return new OperandProperty(GetNestedName("CancelarTareas"));
                }
            }
        }
    }
}