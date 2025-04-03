
using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultProperty("Descripcion")]
    [ImageName("BO_Note")]
    public class NotaTasacion: Nota
    {
        private InmuebleTasacion _tasacion;

        public NotaTasacion(Session session) : base(session)
        {
        }

        [Association("Nota-Tasacion")]
        public InmuebleTasacion Tasacion
        {
            get => _tasacion;
            set => SetPropertyValue(nameof(Tasacion), ref _tasacion, value);
        }

        public new class FieldsClass : Nota.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public InmuebleTasacion.FieldsClass Tasacion
            {
                get
                {
                    return new InmuebleTasacion.FieldsClass(GetNestedName("Tasacion"));
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

