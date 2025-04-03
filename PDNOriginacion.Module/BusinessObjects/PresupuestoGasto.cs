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
    [RuleCriteria("", DefaultContexts.Delete, "PuedeEliminarse = True and !IsNull(PuedeEliminarse)",  "No se puede eliminar este gasto.")]
    public class PresupuestoGasto : BaseObject
    {
        #region variables
        private PresupuestoPrestamo _presupuestoPrestamo;
        private string _descripcion;
        private decimal _cantidad;
        private decimal _monto;
        private string _montoCriteria;
        private bool _puedeEliminarse;
        private bool _puedeEditarMonto;
        private int _codigo;
        #endregion variables

        public PresupuestoGasto(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            this.PuedeEditarMonto = true;
        }

        #region campos
         [Association("PresupuestoPrestamo-PrespuestoGasto")]
        public PresupuestoPrestamo PresupuestoPrestamo
        {
            get => _presupuestoPrestamo;
            set => SetPropertyValue(nameof(PresupuestoPrestamo), ref _presupuestoPrestamo, value);
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

                if (!IsLoading && !IsSaving && this.MontoCriteria != null)
                    this.Monto = (decimal)this.Evaluate($"ToDecimal({this.MontoCriteria})");
            }
        }

        [Appearance("", Enabled = false, Criteria = "PuedeEditarMonto = False", Context = nameof(DetailView),Priority = 1)]
        public decimal Monto
        {
            get => _monto;
            set => SetPropertyValue(nameof(Monto), ref _monto, value);
        }

        [Browsable(false)]
        [Size(SizeAttribute.Unlimited)]
        public string MontoCriteria
        {
            get => _montoCriteria;
            set => SetPropertyValue(nameof(MontoCriteria), ref _montoCriteria, value);
        }

        [Browsable(false)]
        public bool PuedeEliminarse
        {
            get => _puedeEliminarse;
            set => SetPropertyValue(nameof(PuedeEliminarse), ref _puedeEliminarse, value);
        }

        [Browsable(false)]
        public bool PuedeEditarMonto
        {
            get => _puedeEditarMonto;
            set => SetPropertyValue(nameof(PuedeEditarMonto), ref _puedeEditarMonto, value);
        }

        public int Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        [PersistentAlias("Iif(PresupuestoPrestamo.Presupuesto.Solicitud.Inmuebles.Count == 1,PresupuestoPrestamo.Presupuesto.Solicitud.Inmuebles[].Single(), null)")]
        public Inmueble Inmueble => (Inmueble)EvaluateAlias(nameof(Inmueble));

        [PersistentAlias("PresupuestoPrestamo.Presupuesto")]
        public Presupuesto Presupuesto => (Presupuesto)EvaluateAlias(nameof(Presupuesto));

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