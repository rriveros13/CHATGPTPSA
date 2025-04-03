using DevExpress.Persistent.Base;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultProperty("Descripcion")]
    [ImageName("BO_Note")]
    public class NotaPersona : Nota
    { 
        private Persona _persona;

        public NotaPersona(Session session) : base(session)
        {
            //if (this.Oid.Equals(Guid.Empty) && !this.IsSaving && !this.IsLoading)
            //_usuariocreacion = Session.GetObjectByKey<SecuritySystemUser>(SecuritySystem.CurrentUserId);
        }
   
        [Association("Nota-Persona")]
        //[VisibleInDetailView(false)]
        public Persona Persona
        {
            get => _persona;
            set => SetPropertyValue(nameof(Persona), ref _persona, value);
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

