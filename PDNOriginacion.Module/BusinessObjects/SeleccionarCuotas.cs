using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using ITTI;
using Shared;

namespace PDNOriginacion.Module.BusinessObjects
{
    [NonPersistent]
    public class SeleccionarCuotas : BaseObject
    {
        static FieldsClass _Fields;
        private int _plazo;
        private decimal _tasa;
        private SistemasPrestamos _sistema;

        public SeleccionarCuotas(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        public int Plazo
        {
            get => _plazo;
            set => SetPropertyValue(nameof(Plazo), ref _plazo, value);
        }

        [XafDisplayName("Tasa anual con IVA")]
        [ModelDefault("EditMask", "N")]
        [ModelDefault("DisplayFormat", "N2")]
        public decimal Tasa
        {
            get => _tasa;
            set => SetPropertyValue(nameof(Tasa), ref _tasa, value);
        }

        public SistemasPrestamos Sistema
        {
            get => _sistema;
            set => SetPropertyValue(nameof(Sistema), ref _sistema, value);
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
    }
}
