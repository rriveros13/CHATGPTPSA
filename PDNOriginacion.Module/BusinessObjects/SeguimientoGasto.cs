using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class SeguimientoGasto : BaseObject
    {
        #region variables
        private SolicitudSeguimiento _seguimiento;
        private string _descripcion;
        private decimal _cantidad;
        private decimal _monto;
        #endregion variables

        public SeguimientoGasto(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        #region campos
        [Association("SolicitudSeguimiento-SeguimientoGasto")]
        public SolicitudSeguimiento Seguimiento
        {
            get => _seguimiento;
            set => SetPropertyValue(nameof(Seguimiento), ref _seguimiento, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        [ImmediatePostData]
        public decimal Cantidad
        {
            get => _cantidad;
            set
            {
                SetPropertyValue(nameof(Cantidad), ref _cantidad, value);

            }
        }
        public decimal Monto
        {
            get => _monto;
            set => SetPropertyValue(nameof(Monto), ref _monto, value);
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

            public PresupuestoPrestamo.FieldsClass PresupuestoPrestamo
            {
                get
                {
                    return new PresupuestoPrestamo.FieldsClass(GetNestedName("PresupuestoPrestamo"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public OperandProperty Cantidad
            {
                get
                {
                    return new OperandProperty(GetNestedName("Cantidad"));
                }
            }

            public OperandProperty Monto
            {
                get
                {
                    return new OperandProperty(GetNestedName("Monto"));
                }
            }

            public OperandProperty MontoCriteria
            {
                get
                {
                    return new OperandProperty(GetNestedName("MontoCriteria"));
                }
            }

            public OperandProperty PuedeEliminarse
            {
                get
                {
                    return new OperandProperty(GetNestedName("PuedeEliminarse"));
                }
            }

            public OperandProperty PuedeEditarMonto
            {
                get
                {
                    return new OperandProperty(GetNestedName("PuedeEditarMonto"));
                }
            }

            public Inmueble.FieldsClass Inmueble
            {
                get
                {
                    return new Inmueble.FieldsClass(GetNestedName("Inmueble"));
                }
            }

            public Presupuesto.FieldsClass Presupuesto
            {
                get
                {
                    return new Presupuesto.FieldsClass(GetNestedName("Presupuesto"));
                }
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

        static FieldsClass _Fields;

        #endregion campos


        // Created/Updated: XPS15-RB\rodol on XPS15-RB at 21/5/2019 12:43

    }
}