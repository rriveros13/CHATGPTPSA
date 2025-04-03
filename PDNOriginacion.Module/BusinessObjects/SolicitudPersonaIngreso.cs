using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    // [DefaultProperty(nameof(Display))]
    public class SolicitudPersonaIngreso : BaseObject, IObjectSpaceLink
    {
        static FieldsClass _Fields;
        private double _monto;
        private IngresoConcepto _concepto;
        private string _observacion;
        private SolicitudPersona _solicitudPersona;
        private PersonaEmpleo _personaEmpleo;
        private bool _demostrable;

        public SolicitudPersonaIngreso(Session session) : base(session)
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

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [DataSourceCriteria("EsIngreso = True")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public IngresoConcepto Concepto
        {
            get => _concepto;
            set => SetPropertyValue(nameof(Concepto), ref _concepto, value);
        }

        public string Observacion
        {
            get => _observacion;
            set => SetPropertyValue(nameof(Observacion), ref _observacion, value);
        }

        [Association("SolicitudPersona-SolicitudPersonaIngreso")]
        public SolicitudPersona SolicitudPersona
        {
            get => _solicitudPersona;
            set => SetPropertyValue(nameof(SolicitudPersona), ref _solicitudPersona, value);
        }

        [ModelDefault("AllowEdit", "false")]
        public PersonaEmpleo PersonaEmpleo
        {
            get => _personaEmpleo;
            set => SetPropertyValue(nameof(PersonaEmpleo), ref _personaEmpleo, value);
        }

        [NonPersistent]
        public string Empresa => PersonaEmpleo?.Empresa;

        [NonPersistent]
        public string Telefono => PersonaEmpleo?.Telefono;

        public bool Demostrable
        {
            get => _demostrable;
            set => SetPropertyValue(nameof(Demostrable), ref _demostrable, value);
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