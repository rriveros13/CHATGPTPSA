using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Shared;
using System.Collections.Generic;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Producto")]
    public class PrestamoConfiguracion: BaseObject
    {
        static FieldsClass _Fields;
        private Producto _producto;
        private int _plazoParaGastos;
        private decimal _porcPrestable;
        private decimal _tasaRequiereAprobacion;
        private SistemasPrestamos _sistemaDefault;
            
        
        public PrestamoConfiguracion(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public Producto Producto
        {
            get => _producto;
            set => SetPropertyValue(nameof(Producto), ref _producto, value);
        }

        public int PlazoParaGastos
        {
            get => _plazoParaGastos;
            set => SetPropertyValue(nameof(PlazoParaGastos), ref _plazoParaGastos, value);
        }

        public decimal PorcentajePrestable
        {
            get => _porcPrestable;
            set => SetPropertyValue(nameof(PorcentajePrestable), ref _porcPrestable, value);
        }

        public decimal TasaRequiereAprobacion
        {
            get => _tasaRequiereAprobacion;
            set => SetPropertyValue(nameof(TasaRequiereAprobacion), ref _tasaRequiereAprobacion, value);
        }

        public SistemasPrestamos SistemaDefault
        {
            get => _sistemaDefault;
            set => SetPropertyValue(nameof(SistemaDefault), ref _sistemaDefault, value);
        }

        [Association("PrestamoConfiguracion-RangoTasas")]
        [Aggregated]
        [ImmediatePostData]
        public XPCollection<PrestamoConfRangoTasas> RangoTasas => GetCollection<PrestamoConfRangoTasas>(nameof(RangoTasas));

        [Association("PrestamoConfiguracion-PlazosDefault")]
        [Aggregated]
        [ImmediatePostData]
        public XPCollection<PrestamoConfPlazosDefault> PlazosDefault=> GetCollection<PrestamoConfPlazosDefault>(nameof(PlazosDefault));

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

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public Producto.FieldsClass Producto
            {
                get
                {
                    return new Producto.FieldsClass(GetNestedName("Producto"));
                }
            }

            public OperandProperty PlazoParaGastos
            {
                get
                {
                    return new OperandProperty(GetNestedName("PlazoParaGastos"));
                }
            }

            public OperandProperty PorcentajePrestable
            {
                get
                {
                    return new OperandProperty(GetNestedName("PorcentajePrestable"));
                }
            }

            public OperandProperty TasaRequiereAprobacion
            {
                get
                {
                    return new OperandProperty(GetNestedName("TasaRequiereAprobacion"));
                }
            }

            public OperandProperty RangoTasas
            {
                get
                {
                    return new OperandProperty(GetNestedName("RangoTasas"));
                }
            }

            public OperandProperty PlazosDefault
            {
                get
                {
                    return new OperandProperty(GetNestedName("PlazosDefault"));
                }
            }
        }
    }
}

