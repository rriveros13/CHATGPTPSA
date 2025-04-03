using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class PrestamoConfRangoTasas : BaseObject
    {
        public PrestamoConfRangoTasas(Session session) : base(session)
        {
        }

    public override void AfterConstruction() => base.AfterConstruction();

        private PrestamoConfiguracion _prestamoConfiguracion;
        private int _plazoDesde;
        private int _plazoHasta;
        private decimal _tasaDesde;
        private decimal _tasaHasta;
        

        [Association("PrestamoConfiguracion-RangoTasas")]
        public PrestamoConfiguracion PrestamoConfiguracion
        {
            get => _prestamoConfiguracion;
            set => SetPropertyValue(nameof(PrestamoConfiguracion), ref _prestamoConfiguracion, value);
        }

        public int PlazoDesde
        {
            get => _plazoDesde;
            set => SetPropertyValue(nameof(PlazoDesde), ref _plazoDesde, value);
        }

        public int PlazoHasta
        {
            get => _plazoHasta;
            set => SetPropertyValue(nameof(PlazoHasta), ref _plazoHasta, value);
        }

        public decimal TasaDesde
        {
            get => _tasaDesde;
            set => SetPropertyValue(nameof(TasaDesde), ref _tasaDesde, value);
        }

        public decimal TasaHasta
        {
            get => _tasaHasta;
            set => SetPropertyValue(nameof(TasaHasta), ref _tasaHasta, value);
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

            public PrestamoConfiguracion.FieldsClass PrestamoConfiguracion
            {
                get
                {
                    return new PrestamoConfiguracion.FieldsClass(GetNestedName("PrestamoConfiguracion"));
                }
            }

            public OperandProperty PlazoDesde
            {
                get
                {
                    return new OperandProperty(GetNestedName("PlazoDesde"));
                }
            }

            public OperandProperty PlazoHasta
            {
                get
                {
                    return new OperandProperty(GetNestedName("PlazoHasta"));
                }
            }

            public OperandProperty TasaDesde
            {
                get
                {
                    return new OperandProperty(GetNestedName("TasaDesde"));
                }
            }

            public OperandProperty TasaHasta
            {
                get
                {
                    return new OperandProperty(GetNestedName("TasaHasta"));
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

        // Created/Updated: XPS15-RB\rodol on XPS15-RB at 21/5/2019 12:43
    }
}