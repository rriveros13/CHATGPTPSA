using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class PrestamoCuota : BaseObject
    {
        public PrestamoCuota(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        private PresupuestoPrestamo _prestamo;
        private int _nroCuota;
        private decimal _capital;
        private decimal _montoCuota;
        private decimal _amortizacion;
        private decimal _interes;
        private decimal _iva;
        private decimal _montoCuotaSinIVA;
        private DateTime _fechaVencimiento;                                   

        [Association("PresupuestoPrestamo-PrestamoCuota")]
        public PresupuestoPrestamo Prestamo
        {
            get => _prestamo;
            set => SetPropertyValue(nameof(Prestamo), ref _prestamo, value);
        }

        public int NroCuota
        {
            get => _nroCuota;
            set => SetPropertyValue(nameof(NroCuota), ref _nroCuota, value);
        }

        public decimal Capital
        {
            get => _capital;
            set => SetPropertyValue(nameof(Capital), ref _capital, value);
        }

        public decimal MontoCuota
        {
            get => _montoCuota;
            set => SetPropertyValue(nameof(MontoCuota), ref _montoCuota, value);
        }

        public decimal Amortizacion
        {
            get => _amortizacion;
            set => SetPropertyValue(nameof(Amortizacion), ref _amortizacion, value);
        }

        public decimal Interes
        {
            get => _interes;
            set => SetPropertyValue(nameof(Interes), ref _interes, value);
        }

        public decimal IVA
        {
            get => _iva;
            set => SetPropertyValue(nameof(IVA), ref _iva, value);
        }

        public decimal MontoCuotaSinIVA
        {
            get => _montoCuotaSinIVA;
            set => SetPropertyValue(nameof(MontoCuotaSinIVA), ref _montoCuotaSinIVA, value);
        }

        public DateTime FechaVencimiento
        {
            get => _fechaVencimiento;
            set => SetPropertyValue(nameof(FechaVencimiento), ref _fechaVencimiento, value);
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

            public PresupuestoPrestamo.FieldsClass Prestamo
            {
                get
                {
                    return new PresupuestoPrestamo.FieldsClass(GetNestedName("Prestamo"));
                }
            }

            public OperandProperty NroCuota
            {
                get
                {
                    return new OperandProperty(GetNestedName("NroCuota"));
                }
            }

            public OperandProperty Capital
            {
                get
                {
                    return new OperandProperty(GetNestedName("Capital"));
                }
            }

            public OperandProperty MontoCuota
            {
                get
                {
                    return new OperandProperty(GetNestedName("MontoCuota"));
                }
            }

            public OperandProperty Amortizacion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Amortizacion"));
                }
            }

            public OperandProperty Interes
            {
                get
                {
                    return new OperandProperty(GetNestedName("Interes"));
                }
            }
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

        static FieldsClass _Fields;

        // Created/Updated: XPS15-RB\rodol on XPS15-RB at 21/5/2019 12:43
    }
}