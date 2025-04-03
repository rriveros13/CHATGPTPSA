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
    [DefaultProperty(nameof(Accion))]
    public class WFAccion : BaseObject
    {
        static FieldsClass _Fields;
        private Acciones _accion;
        private EstadoSolicitud _estadoDestino;
        private Producto _producto;
        private WFAccionConfig _configuracion;

        public WFAccion(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        public Acciones Accion
        {
            get => _accion;
            set => SetPropertyValue(nameof(Accion), ref _accion, value);
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

        [XafDisplayName("Configuración Específica")]
        public WFAccionConfig Configuracion
        {
            get => _configuracion;
            set => SetPropertyValue(nameof(Configuracion), ref _configuracion, value);
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

        //[RuleRequiredField(DefaultContexts.Save)]
        [Association("Producto-Acciones")]
        public Producto Producto
        {
            get => _producto;
            set => SetPropertyValue(nameof(Producto), ref _producto, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Accion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Accion"));
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

            public Producto.FieldsClass Producto
            {
                get
                {
                    return new Producto.FieldsClass(GetNestedName("Producto"));
                }
            }
        }
    }

    public enum Acciones
    {
        ProcesarMotor = 0,
        ImportarCheques = 1,
        DatosPersona = 2,
        ImportarPersonas = 3,
        ProcesarMotor2 = 4,
        GenerarPropuesta = 5,
        GenerarPresupuesto = 6,
        InformeEscribania = 7,
        AgregarInmueble = 8,
        CrearTarea = 9,
        IntegracionITGF = 10,
        GenerarSolAsociada = 11,
        CalculadoraPrestamo = 12,
        ImprimirSolicitud = 13
    }
}