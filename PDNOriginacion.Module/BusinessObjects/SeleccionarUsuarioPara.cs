using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace PDNOriginacion.Module.BusinessObjects
{
    [NonPersistent]
    public class SeleccionarUsuarioPara : BaseObject
    {
        static FieldsClass _Fields;
        private Usuario usuario;

        public SeleccionarUsuarioPara(Session session) : base(session)
        {
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

        public Usuario Usuario
        {
            get => usuario;
            set => SetPropertyValue(nameof(Usuario), ref usuario, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public Usuario.FieldsClass Usuario
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("Usuario"));
                }
            }
        }
    }
}