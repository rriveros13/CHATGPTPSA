using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(TipoPersona))]
    public class WFTipoPersona : BaseObject
    {
        static FieldsClass _Fields;
        private int _cantidad;
        private EstadoSolicitud _estadoDestino;
        private bool _procesarMotor;
        private Producto _producto;
        private TipoPersona _tipoPersona;

        public WFTipoPersona(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        public int Cantidad
        {
            get => _cantidad;
            set => SetPropertyValue(nameof(Cantidad), ref _cantidad, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public EstadoSolicitud EstadoDestino
        {
            get => _estadoDestino;
            set => SetPropertyValue(nameof(EstadoDestino), ref _estadoDestino, value);
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
                if (ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
            }
        }

        public bool ProcesarMotor
        {
            get => _procesarMotor;
            set => SetPropertyValue(nameof(ProcesarMotor), ref _procesarMotor, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [Association("Producto-Personas")]
        public Producto Producto
        {
            get => _producto;
            set => SetPropertyValue(nameof(Producto), ref _producto, value);
        }

        public TipoPersona TipoPersona
        {
            get => _tipoPersona;
            set => SetPropertyValue(nameof(TipoPersona), ref _tipoPersona, value);
        }

        [Association("WFTipoPersona-Modelos")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<WFTipoPersonaModelo> Modelos=> GetCollection<WFTipoPersonaModelo>(nameof(Modelos));

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
