using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Descripcion")]
    public class CuestionarioItem : BaseObject
    {
        static FieldsClass _Fields;
        private string _codigo;
        private Cuestionario _cuestionario;
        private string _descripcion;
        private int _orden;
        private TipoItem _tipo;

        public CuestionarioItem(Session session) : base(session)
        {
        }

        [RuleUniqueValue]
        public string Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        [Association("Cuestionario-Items")]
        public Cuestionario Cuestionario
        {
            get => _cuestionario;
            set => SetPropertyValue(nameof(Cuestionario), ref _cuestionario, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
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

        public int Orden
        {
            get => _orden;
            set => SetPropertyValue(nameof(Orden), ref _orden, value);
        }

        public TipoItem Tipo
        {
            get => _tipo;
            set => SetPropertyValue(nameof(Tipo), ref _tipo, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Codigo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Codigo"));
                }
            }

            public Cuestionario.FieldsClass Cuestionario
            {
                get
                {
                    return new Cuestionario.FieldsClass(GetNestedName("Cuestionario"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public OperandProperty Orden
            {
                get
                {
                    return new OperandProperty(GetNestedName("Orden"));
                }
            }

            public OperandProperty Tipo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Tipo"));
                }
            }
        }
    }

    public enum TipoItem
    {
        PreguntaSino = 0,
        PreguntaCampoLibre = 1,
        Speech = 2
    }
}