using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Motivo")]
    public class MotivoRechazoCheque : BaseObject
    {
        static FieldsClass _Fields;
        string motivo;

        public MotivoRechazoCheque(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();
        public MotivoRechazoCheque GetOrCreate(Session session, string motivo)
        {
            MotivoRechazoCheque m = Session.FindObject<MotivoRechazoCheque>(MotivoRechazoCheque.Fields.Motivo ==
                motivo.ToUpper());

            if(m == null)
            {
                m = new MotivoRechazoCheque(session) { Motivo = motivo.ToUpper() };
                m.Save();
            }
            return m;
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
        public string Motivo
        {
            get => motivo;
            set
            {
                bool cambio = SetPropertyValue(nameof(Motivo), ref motivo, value);
                if(!IsSaving && !IsLoading && cambio)
                {
                    if(!string.IsNullOrEmpty(motivo))
                    {
                        motivo = motivo.ToUpper().Trim();
                    }
                }
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

            public OperandProperty Motivo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Motivo"));
                }
            }
        }
    }
}