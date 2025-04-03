using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;

namespace PDNOriginacion.Module.BusinessObjects
{
    [NonPersistent]
    public class SeleccionarUsuario : BaseObject
    {
        static FieldsClass _Fields;
        private Usuario usuario;
        public Rol RolUsuario;

        public SeleccionarUsuario(Session session) : base(session)
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

        [DataSourceCriteria("RolesUsuario[Name='@This.RolUsuario.Name']")]
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

            public Rol.FieldsClass RolUsuario
            {
                get
                {
                    return new Rol.FieldsClass(GetNestedName("RolUsuario"));
                }
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