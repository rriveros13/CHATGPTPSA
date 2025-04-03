using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(TipoPersona))]
    public class WFTipoPersonaModelo : BaseObject
    {
        static FieldsClass _Fields;
        private Modelo _modelo;
        private WFTipoPersona _wfTipoPersona;

        public WFTipoPersonaModelo(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();


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
                if (ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
            }
        }

        public Modelo Modelo
        {
            get => _modelo;
            set => SetPropertyValue(nameof(Modelo), ref _modelo, value);
        }

        [Association("WFTipoPersona-Modelos")]
        public WFTipoPersona TipoPersona
        {
            get => _wfTipoPersona;
            set => SetPropertyValue(nameof(TipoPersona), ref _wfTipoPersona, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Cantidad
            {
                get
                {
                    return new OperandProperty(GetNestedName("Cantidad"));
                }
            }

            public EstadoSolicitud.FieldsClass EstadoDestino
            {
                get
                {
                    return new EstadoSolicitud.FieldsClass(GetNestedName("EstadoDestino"));
                }
            }

            public OperandProperty Auditoria
            {
                get
                {
                    return new OperandProperty(GetNestedName("Auditoria"));
                }
            }

            public Modelo.FieldsClass Modelo
            {
                get
                {
                    return new Modelo.FieldsClass(GetNestedName("Modelo"));
                }
            }

            public OperandProperty ProcesarMotor
            {
                get
                {
                    return new OperandProperty(GetNestedName("ProcesarMotor"));
                }
            }

            public Producto.FieldsClass Producto
            {
                get
                {
                    return new Producto.FieldsClass(GetNestedName("Producto"));
                }
            }

            public TipoPersona.FieldsClass TipoPersona
            {
                get
                {
                    return new TipoPersona.FieldsClass(GetNestedName("TipoPersona"));
                }
            }
        }
    }
}
