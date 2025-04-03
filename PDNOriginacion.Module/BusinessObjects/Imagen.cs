using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Title")]
    public class Imagen : BaseObject
    {
        static FieldsClass _Fields;
        private byte[] contenido;
        private string title;

        public Imagen(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        public byte[] Contenido
        {
            get => contenido;
            set => SetPropertyValue(nameof(Contenido), ref contenido, value);
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

        [XafDisplayName("Titulo")]
        public string Title
        {
            get => title;
            set => SetPropertyValue(nameof(Title), ref title, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Contenido
            {
                get
                {
                    return new OperandProperty(GetNestedName("Contenido"));
                }
            }

            public OperandProperty Title
            {
                get
                {
                    return new OperandProperty(GetNestedName("Title"));
                }
            }
        }
    }
}