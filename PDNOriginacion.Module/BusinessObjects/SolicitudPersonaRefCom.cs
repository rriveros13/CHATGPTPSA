using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
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
    public class SolicitudPersonaRefCom : BaseObject, IObjectSpaceLink
    {
        private int cuotasPagadas;
        static FieldsClass _Fields;
        private Entidad _entidad;
        private double _montoCuota;
        private double _montoSolicitado;
        private int _plazo;
        private SolicitudPersona _solicitudPersona;
        private DateTime _ultimoPago;
        private string _observacion;

        public SolicitudPersonaRefCom(Session session) : base(session)
        {
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public Entidad Entidad
        {
            get => _entidad;
            set => SetPropertyValue(nameof(Entidad), ref _entidad, value);
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

        public double MontoCuota
        {
            get => _montoCuota;
            set => SetPropertyValue(nameof(MontoCuota), ref _montoCuota, value);
        }

        public double MontoSolicitado
        {
            get => _montoSolicitado;
            set => SetPropertyValue(nameof(MontoSolicitado), ref _montoSolicitado, value);
        }

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [ImmediatePostData]
        public int Plazo
        {
            get => _plazo;
            set => SetPropertyValue(nameof(Plazo), ref _plazo, value);
        }

        [ImmediatePostData]
        public int CuotasPagadas
        {
            get => cuotasPagadas;
            set => SetPropertyValue(nameof(CuotasPagadas), ref cuotasPagadas, value);
        }

        [PersistentAlias("Concat(CuotasPagadas, '/', Plazo)")]
        public string CantidadCuotasPagadas
        {
            get { return (string)EvaluateAlias(nameof(CantidadCuotasPagadas)); }
        }

        [Association("SolicitudPersona-SolicitudPersonaRefCom")]
        public SolicitudPersona SolicitudPersona
        {
            get => _solicitudPersona;
            set => SetPropertyValue(nameof(SolicitudPersona), ref _solicitudPersona, value);
        }

        public DateTime UltimoPago
        {
            get => _ultimoPago;
            set => SetPropertyValue(nameof(UltimoPago), ref _ultimoPago, value);
        }

        // [Size(1000)]
        [Size(SizeAttribute.Unlimited), VisibleInListView(true)]
        public string Observacion 
        {
            get => _observacion;
            set => SetPropertyValue(nameof(Observacion), ref _observacion, value);
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

            public Entidad.FieldsClass Entidad
            {
                get
                {
                    return new Entidad.FieldsClass(GetNestedName("Entidad"));
                }
            }

            public OperandProperty MontoCuota
            {
                get
                {
                    return new OperandProperty(GetNestedName("MontoCuota"));
                }
            }

            public OperandProperty MontoSolicitado
            {
                get
                {
                    return new OperandProperty(GetNestedName("MontoSolicitado"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public OperandProperty Plazo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Plazo"));
                }
            }

            public SolicitudPersona.FieldsClass SolicitudPersona
            {
                get
                {
                    return new SolicitudPersona.FieldsClass(GetNestedName("SolicitudPersona"));
                }
            }

            public OperandProperty UltimoPago
            {
                get
                {
                    return new OperandProperty(GetNestedName("UltimoPago"));
                }
            }

            public OperandProperty Observacion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Observacion"));
                }
            }
        }
    }
}