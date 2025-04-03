using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System.Collections.Generic;

namespace PDNOriginacion.Module.BusinessObjects
{
    [NonPersistent]
    public class InmueblesDisponibles : BaseObject
    {
        static FieldsClass _Fields;
        private Solicitud _solicitud;

        public InmueblesDisponibles(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [Appearance("", Visibility = DevExpress.ExpressApp.Editors.ViewItemVisibility.Hide)]
        [ImmediatePostData]
        public Solicitud Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        public List<Inmueble> Inmuebles
        {
            get
            {
                List<Inmueble> inmuebles = new List<Inmueble>();
                if (this.Solicitud != null)
                {
                    if (this.Solicitud.Personas != null && this.Solicitud.Personas.Count > 0)
                    {
                        foreach (var solPersona in this.Solicitud.Personas)
                        {
                            if (solPersona.Persona != null)
                            {
                                foreach (var inmue in solPersona.Persona.Inmuebles)
                                {
                                    inmuebles.Add(inmue);
                                }
                            }
                        }
                    }
                }
                return inmuebles;
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();

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

        }
    }
}
