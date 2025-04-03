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
    public class PrestamoConfPlazosDefault: BaseObject
    {
        public PrestamoConfPlazosDefault(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        private PrestamoConfiguracion _prestamoConfiguracion;
        private int _plazo;
        private decimal _tasa;

        [Association("PrestamoConfiguracion-PlazosDefault")]
        public PrestamoConfiguracion PrestamoConfiguracion
        {
            get => _prestamoConfiguracion;
            set => SetPropertyValue(nameof(PrestamoConfiguracion), ref _prestamoConfiguracion, value);
        }

        public int Plazo
        {
            get => _plazo;
            set => SetPropertyValue(nameof(Plazo), ref _plazo, value);
        }

        public decimal Tasa
        {
            get => _tasa;
            set => SetPropertyValue(nameof(Tasa), ref _tasa, value);
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

            public OperandProperty Plazo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Plazo"));
                }
            }

            public OperandProperty Tasa
            {
                get
                {
                    return new OperandProperty(GetNestedName("Tasa"));
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
    }
}
