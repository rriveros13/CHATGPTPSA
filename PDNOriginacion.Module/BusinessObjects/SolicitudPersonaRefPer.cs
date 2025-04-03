using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    // [DefaultProperty(nameof(Display))]
    public class SolicitudPersonaRefPer : BaseObject, IObjectSpaceLink
    {
        static FieldsClass _Fields;
        private string _nombreCompleto;
        private Parentezco _parentezco;
        private SolicitudPersona _solicitudPersona;
        private string _telefono;
        private string _observacion;

        public SolicitudPersonaRefPer(Session session) : base(session)
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

        public string NombreCompleto
        {
            get => _nombreCompleto;
            set => SetPropertyValue(nameof(NombreCompleto), ref _nombreCompleto, value);
        }

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [XafDisplayName("Parentesco")]
        public Parentezco Parentezco
        {
            get => _parentezco;
            set => SetPropertyValue(nameof(Parentezco), ref _parentezco, value);
        }

        [Association("SolicitudPersona-SolicitudPersonaRefPer")]
        public SolicitudPersona SolicitudPersona
        {
            get => _solicitudPersona;
            set => SetPropertyValue(nameof(SolicitudPersona), ref _solicitudPersona, value);
        }

        [Size(20)]
        public string Telefono
        {
            get => _telefono;
            set => SetPropertyValue(nameof(Telefono), ref _telefono, value);
        }

        [Size(250)]
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

            public OperandProperty NombreCompleto
            {
                get
                {
                    return new OperandProperty(GetNestedName("NombreCompleto"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public Parentezco.FieldsClass Parentezco
            {
                get
                {
                    return new Parentezco.FieldsClass(GetNestedName("Parentezco"));
                }
            }

            public SolicitudPersona.FieldsClass SolicitudPersona
            {
                get
                {
                    return new SolicitudPersona.FieldsClass(GetNestedName("SolicitudPersona"));
                }
            }

            public Telefono.FieldsClass Telefono
            {
                get
                {
                    return new Telefono.FieldsClass(GetNestedName("Telefono"));
                }
            }
        }
    }
}