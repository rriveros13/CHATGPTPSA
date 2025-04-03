using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Fecha")]
    public class RegistroEstadoSolicitud : BaseObject
    {
        static FieldsClass _Fields;
        private DateTime _fecha;
        private Solicitud _solicitud;
        private TimeSpan? _tiempo;
        //private EstadoSolicitud _estado;
        //[NonCloneable]
        //public EstadoSolicitud Estado
        //{
        //    get => _estado;
        //    set => SetPropertyValue(nameof(Estado), ref _estado, value);
        //}
        private WFTransicion _transicion;
        private Usuario _usuario;
        private bool _esReversion;

        public RegistroEstadoSolicitud(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _fecha = DateTime.Now;
            _usuario = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
        }

        [PersistentAlias("Iif(!IsNull(Transicion), Iif(!IsNull(Transicion.EstadoDestino),Transicion.EstadoDestino,null), null)")]
        public EstadoSolicitud Estado => EvaluateAlias(nameof(Estado)) as EstadoSolicitud;

        [ModelDefault("DisplayFormat", @"{0: dd/MM/yyyy HH:mm:ss}")]
        //[RuleRequiredField(DefaultContexts.Save)]
        [NonCloneable]
        public DateTime Fecha
        {
            get => _fecha;
            set => SetPropertyValue(nameof(Fecha), ref _fecha, value);
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

        [Association("Solicitud-RegistroEstadoSolicitud")]
        [Aggregated]
        public Solicitud Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [ModelDefault("DisplayFormat", @"{0:d\.hh\:mm\:ss}")]
        public TimeSpan? TiempoUtilizado
        {
            get => _tiempo;
            set => SetPropertyValue(nameof(TiempoUtilizado), ref _tiempo, value);
        }

        [NonCloneable]
        public WFTransicion Transicion
        {
            get => _transicion;
            set => SetPropertyValue(nameof(Transicion), ref _transicion, value);
        }

        [NonCloneable]
        public Usuario Usuario
        {
            get => _usuario;
            set => SetPropertyValue(nameof(Usuario), ref _usuario, value);
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            //Persona.CambiarEstadoPersona(this.Solicitud.Titular, true);
        }

        public bool EsReversion
        {
            get => _esReversion;
            set => SetPropertyValue(nameof(EsReversion), ref _esReversion, value);
        }


        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public EstadoSolicitud.FieldsClass Estado
            {
                get
                {
                    return new EstadoSolicitud.FieldsClass(GetNestedName("Estado"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public Solicitud.FieldsClass Solicitud
            {
                get
                {
                    return new Solicitud.FieldsClass(GetNestedName("Solicitud"));
                }
            }

            public OperandProperty TiempoUtilizado
            {
                get
                {
                    return new OperandProperty(GetNestedName("TiempoUtilizado"));
                }
            }

            public WFTransicion.FieldsClass Transicion
            {
                get
                {
                    return new WFTransicion.FieldsClass(GetNestedName("Transicion"));
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