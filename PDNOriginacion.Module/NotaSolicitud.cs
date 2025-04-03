using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultProperty("Descripcion")]
    [ImageName("BO_Note")]
    public class NotaSolicitud : Nota
    {
        private Solicitud _solicitud;

        public NotaSolicitud(Session session) : base(session)
        {
        }

        [Association("Nota-Solicitud")]
        //[VisibleInDetailView(false)]
        public Solicitud Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        public new class FieldsClass : Nota.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public Persona.FieldsClass Persona
            {
                get
                {
                    return new Persona.FieldsClass(GetNestedName("Persona"));
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

