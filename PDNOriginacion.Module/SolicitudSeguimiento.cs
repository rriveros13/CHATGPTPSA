using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Display")]
    public class SolicitudSeguimiento : BaseObject, IObjectSpaceLink
    {
        static FieldsClass _Fields;
        private Solicitud _solicitud;
        private Persona _persona;
        private Escribania _escribania;
        private EstadoEscribania _estado;
        private Usuario _encargadoJunior;
        private Usuario _encargadoSenior;
        private NoConcretadoMotivo _noConcretadoMotivo;
        private string _observacion;
        private DateTime _fechaEntrega;
        private DateTime _fechaCierre;
        private DateTime _fechaConcretado;
        private DateTime _fechaProgramado;
        private DateTime _fechaEscribania;
        private DateTime _fechaLevantamiento;
        private DateTime _fechaEntregaLevantamiento;
        private DateTime _fechaCierreLevantamiento;

        public SolicitudSeguimiento(Session session) : base(session)
        {
        }

        [PersistentAlias("Concat(Solicitud.Oid, '-', Escribania.Nombre)")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public string Display => (string)EvaluateAlias(nameof(Display));

        /* [Association("SolicitudPersona-SolicitudPersonaEgreso")]
         [Aggregated]
         public XPCollection<SolicitudPersonaEgreso> Egresos => GetCollection<SolicitudPersonaEgreso>(nameof(Egresos));*/

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

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Escribania = Session.FindObject<Escribania>(CriteriaOperator.Parse("Default=true"));
            Estado = Session.FindObject<EstadoEscribania>(CriteriaOperator.Parse("Default=true"));
            FechaEntrega = DateTime.Today;
        }

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [Association("Persona-SolicitudSeguimiento")]
        [VisibleInDetailView(false)]
        public Persona Persona
        {
            get => _persona;
            set => SetPropertyValue(nameof(Persona), ref _persona, value);
        }

        [Association("Solicitud-SolicitudSeguimiento")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public Solicitud Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        [Association("Escribania-SolicitudSeguimiento")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public Escribania Escribania
        {
            get => _escribania;
            set => SetPropertyValue(nameof(Escribania), ref _escribania, value);
        }

        public EstadoEscribania Estado
        {
            get => _estado;
            set => SetPropertyValue(nameof(Estado), ref _estado, value);
        }

        public Usuario EncargadoJunior
        {
            get => _encargadoJunior;
            set => SetPropertyValue(nameof(EncargadoJunior), ref _encargadoJunior, value);
        }

        public Usuario EncargadoSenior
        {
            get => _encargadoSenior;
            set => SetPropertyValue(nameof(EncargadoSenior), ref _encargadoSenior, value);
        }

        public NoConcretadoMotivo NoConcretadoMotivo
        {
            get => _noConcretadoMotivo;
            set => SetPropertyValue(nameof(NoConcretadoMotivo), ref _noConcretadoMotivo, value);
        }

        [Size(500)]
        public string Observacion
        {
            get => _observacion;
            set => SetPropertyValue(nameof(Observacion), ref _observacion, value);
        }

        [ImmediatePostData]
        public DateTime FechaEntrega
        {
            get => _fechaEntrega;
            set
            {
                SetPropertyValue(nameof(FechaEntrega), ref _fechaEntrega, value);
                this.FechaProgramado = FechaEntrega.AddDays(25);
            }
        }

        public DateTime FechaProgramado
        {
            get => _fechaProgramado;
            set => SetPropertyValue(nameof(_fechaProgramado), ref _fechaProgramado, value);
        }

        [ImmediatePostData]
        public DateTime FechaCierre
        {
            get => _fechaCierre;
            set => SetPropertyValue(nameof(_fechaCierre), ref _fechaCierre, value);
        }

        [NonPersistent]
        public int DiasEntregaACierre
        {
            get
            {
                DateTime fecha = FechaCierre;
                if (fecha == DateTime.MinValue)
                    fecha = DateTime.Today;

                return (fecha - FechaEntrega).Days;
                  
            }
        }

        [ImmediatePostData]
        public DateTime FechaConcretado
        {
            get => _fechaConcretado;
            set => SetPropertyValue(nameof(_fechaConcretado), ref _fechaConcretado, value);
        }

        [NonPersistent]
        public int DiasEntregaAConcretado
        {
            get
            {
                DateTime fecha = FechaConcretado;
                DateTime fechaCierre = FechaCierre;
                if (fecha == DateTime.MinValue)
                    fecha = DateTime.Today;

                if (fechaCierre == DateTime.MinValue)
                    fechaCierre = DateTime.Today;

                return (fecha - fechaCierre).Days;

            }
        }

        [ImmediatePostData]
        public DateTime FechaEscribania
        {
            get => _fechaEscribania;
            set => SetPropertyValue(nameof(_fechaEscribania), ref _fechaEscribania, value);
        }

        [NonPersistent]
        public int DiasConcretadoAEscribania
        {
            get
            {
                DateTime fecha = FechaEscribania;
                DateTime fechaConcretado = FechaConcretado;
                if (fecha == DateTime.MinValue)
                    fecha = DateTime.Today;

                if (fechaConcretado == DateTime.MinValue)
                    fechaConcretado= DateTime.Today;

                return (fecha - fechaConcretado).Days;

            }
        }

        public DateTime FechaLevantamiento
        {
            get => _fechaLevantamiento;
            set => SetPropertyValue(nameof(_fechaLevantamiento), ref _fechaLevantamiento, value);
        }

        public DateTime FechaEntregaLevantamiento
        {
            get => _fechaEntregaLevantamiento;
            set => SetPropertyValue(nameof(_fechaEntregaLevantamiento), ref _fechaEntregaLevantamiento, value);
        }

        public DateTime FechaCierreLevantamiento
        {
            get => _fechaCierreLevantamiento;
            set => SetPropertyValue(nameof(_fechaCierreLevantamiento), ref _fechaCierreLevantamiento, value);
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            this.Persona = Solicitud.Titular;
        }

        [Association("SolicitudSeguimiento-SeguimientoGasto")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<SeguimientoGasto> Gastos => GetCollection<SeguimientoGasto>(nameof(Gastos));

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Display
            {
                get
                {
                    return new OperandProperty(GetNestedName("Display"));
                }
            }

            public OperandProperty Egresos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Egresos"));
                }
            }

            public OperandProperty Ingresos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Ingresos"));
                }
            }

            public Modelo.FieldsClass Modelo
            {
                get
                {
                    return new Modelo.FieldsClass(GetNestedName("Modelo"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public Persona.FieldsClass Persona
            {
                get
                {
                    return new Persona.FieldsClass(GetNestedName("Persona"));
                }
            }

            public OperandProperty ProcesadoMotor
            {
                get
                {
                    return new OperandProperty(GetNestedName("ProcesadoMotor"));
                }
            }

            public OperandProperty ProcesarMotor
            {
                get
                {
                    return new OperandProperty(GetNestedName("ProcesarMotor"));
                }
            }

            public OperandProperty ReferenciasComerciales
            {
                get
                {
                    return new OperandProperty(GetNestedName("ReferenciasComerciales"));
                }
            }

            public OperandProperty ReferenciasPersonales
            {
                get
                {
                    return new OperandProperty(GetNestedName("ReferenciasPersonales"));
                }
            }

            public OperandProperty ResultadoMotor
            {
                get
                {
                    return new OperandProperty(GetNestedName("ResultadoMotor"));
                }
            }

            public Solicitud.FieldsClass Solicitud
            {
                get
                {
                    return new Solicitud.FieldsClass(GetNestedName("Solicitud"));
                }
            }

            public TipoPersona.FieldsClass TipoPersona
            {
                get
                {
                    return new TipoPersona.FieldsClass(GetNestedName("TipoPersona"));
                }
            }
        }
    }
}