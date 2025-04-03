using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    // [DefaultProperty(nameof(Display))]
    public class SolicitudPersonaEgreso : BaseObject, IObjectSpaceLink
    {
        static FieldsClass _Fields;
        private double _monto;
        private IngresoConcepto _concepto;
        private string _observacion;
        private SolicitudPersona _solicitudPersona;
        private bool _aCancelar;

        public SolicitudPersonaEgreso(Session session) : base(session)
        {
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
        public double Monto
        {
            get => _monto;
            set => SetPropertyValue(nameof(Monto), ref _monto, value);
        }

        [DataSourceCriteria("EsIngreso = False")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public IngresoConcepto Concepto
        {
            get => _concepto;
            set => SetPropertyValue(nameof(Concepto), ref _concepto, value);
        }

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        public string Observacion
        {
            get => _observacion;
            set => SetPropertyValue(nameof(Observacion), ref _observacion, value);
        }

        [XafDisplayName("A Cancelar")]
        public bool ACancelar
        {
            get => _aCancelar;
            set => SetPropertyValue(nameof(ACancelar), ref _aCancelar, value);
        }

        [Association("SolicitudPersona-SolicitudPersonaEgreso")]
        public SolicitudPersona SolicitudPersona
        {
            get => _solicitudPersona;
            set => SetPropertyValue(nameof(SolicitudPersona), ref _solicitudPersona, value);
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

            public OperandProperty Monto
            {
                get
                {
                    return new OperandProperty(GetNestedName("Monto"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public OperandProperty Observacion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Observacion"));
                }
            }

            public SolicitudPersona.FieldsClass SolicitudPersona
            {
                get
                {
                    return new SolicitudPersona.FieldsClass(GetNestedName("SolicitudPersona"));
                }
            }
        }
    }
}