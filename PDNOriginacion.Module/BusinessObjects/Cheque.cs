using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Numero")]
    public class Cheque : BaseObject
    {
        static FieldsClass _Fields;
        private string _cuenta;
        private Entidad _entidad;
        private DateTime _fechaEmision;
        private DateTime _fechaVencimiento;
        private byte[] _imagenFrente;
        private byte[] _imagenReverso;
        private Persona _librador;
        private MonedaEnum _moneda;
        private decimal _monto;
        private MotivoRechazoCheque _motivoRechazo;
        private string _numero;
        private bool _rechazado;
        private bool _recibido;
        private Resultado _resultado;
        private string _serie;
        private Solicitud _solicitud;

        public Cheque(Session session) : base(session)
        {
        }

        protected override void OnSaving()
        {
            base.OnSaving();

            bool any = false;

            if(Solicitud != null)
            {
                foreach(SolicitudPersona p in Solicitud.Personas)
                {
                    if(ReferenceEquals(p?.Persona, Librador))
                    {
                        any = true;
                        break;
                    }
                }

                if(Solicitud.Personas.Count > 0 && !any)
                {
                    SolicitudPersona sp = new SolicitudPersona(Session)
                    {
                        Persona = Librador,
                        Solicitud = Solicitud,
                        TipoPersona = Session.FindObject<TipoPersona>(TipoPersona.Fields.Codigo == "LIB")
                    };

                    WFTipoPersona confPersona = (Solicitud.Producto.Personas
                        .Where(wftp => wftp.Cantidad == 0 && wftp.TipoPersona == sp.TipoPersona)
                        .ToList())[0];
                    if(confPersona != null)
                    {
                        //TODO sp.ProcesarMotor = confPersona.ProcesarMotor;
                        //TODO sp.Modelo = confPersona.Modelo;
                    }

                    sp.Save();
                }
            }

            Save();
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _rechazado = false;
        }

        public string Cuenta
        {
            get => _cuenta;
            set => SetPropertyValue(nameof(Cuenta), ref _cuenta, value);
        }

        public Entidad Entidad
        {
            get => _entidad;
            set => SetPropertyValue(nameof(Entidad), ref _entidad, value);
        }

        public DateTime FechaEmision
        {
            get => _fechaEmision;
            set => SetPropertyValue(nameof(FechaEmision), ref _fechaEmision, value.Date);
        }

        public DateTime FechaVencimiento
        {
            get => _fechaVencimiento;
            set => SetPropertyValue(nameof(FechaVencimiento), ref _fechaVencimiento, value.Date);
        }

        private XPCollection<AuditDataItemPersistent> auditoria;
        public XPCollection<AuditDataItemPersistent> Auditoria
        {
            get
            {
                if (auditoria == null)
                {
                    auditoria = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditoria;
            }
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

        public byte[] ImagenFrente
        {
            get => _imagenFrente;
            set => SetPropertyValue(nameof(ImagenFrente), ref _imagenFrente, value);
        }

        public byte[] ImagenReverso
        {
            get => _imagenReverso;
            set => SetPropertyValue(nameof(ImagenReverso), ref _imagenReverso, value);
        }

        public Persona Librador
        {
            get => _librador;
            set => SetPropertyValue(nameof(Librador), ref _librador, value);
        }

        public MonedaEnum Moneda
        {
            get => _moneda;
            set => SetPropertyValue(nameof(Moneda), ref _moneda, value);
        }

        public decimal Monto
        {
            get => _monto;
            set => SetPropertyValue(nameof(Monto), ref _monto, value);
        }

        public MotivoRechazoCheque MotivoRechazo
        {
            get => _motivoRechazo;
            set => SetPropertyValue(nameof(MotivoRechazo), ref _motivoRechazo, value);
        }

        public string Numero
        {
            get => _numero;
            set => SetPropertyValue(nameof(Numero), ref _numero, value);
        }

        public bool Rechazado
        {
            get => _rechazado;
            set => SetPropertyValue(nameof(Rechazado), ref _rechazado, value);
        }

        public bool Recibido
        {
            get => _recibido;
            set => SetPropertyValue(nameof(Recibido), ref _recibido, value);
        }

        [DevExpress.Xpo.DisplayName("Resultado Motor")]
        public Resultado Resultado
        {
            get => _resultado;
            set => SetPropertyValue(nameof(Resultado), ref _resultado, value);
        }

        public string Serie
        {
            get => _serie;
            set => SetPropertyValue(nameof(Serie), ref _serie, value);
        }

        //[Association("Solicitud-Cheque")]
        public Solicitud Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Cuenta
            {
                get
                {
                    return new OperandProperty(GetNestedName("Cuenta"));
                }
            }

            public Entidad.FieldsClass Entidad
            {
                get
                {
                    return new Entidad.FieldsClass(GetNestedName("Entidad"));
                }
            }

            public OperandProperty FechaEmision
            {
                get
                {
                    return new OperandProperty(GetNestedName("FechaEmision"));
                }
            }

            public OperandProperty FechaVencimiento
            {
                get
                {
                    return new OperandProperty(GetNestedName("FechaVencimiento"));
                }
            }

            public OperandProperty ImagenFrente
            {
                get
                {
                    return new OperandProperty(GetNestedName("ImagenFrente"));
                }
            }

            public OperandProperty ImagenReverso
            {
                get
                {
                    return new OperandProperty(GetNestedName("ImagenReverso"));
                }
            }

            public Persona.FieldsClass Librador
            {
                get
                {
                    return new Persona.FieldsClass(GetNestedName("Librador"));
                }
            }

            public OperandProperty Moneda
            {
                get
                {
                    return new OperandProperty(GetNestedName("Moneda"));
                }
            }

            public OperandProperty Monto
            {
                get
                {
                    return new OperandProperty(GetNestedName("Monto"));
                }
            }

            public MotivoRechazoCheque.FieldsClass MotivoRechazo
            {
                get
                {
                    return new MotivoRechazoCheque.FieldsClass(GetNestedName("MotivoRechazo"));
                }
            }

            public OperandProperty Numero
            {
                get
                {
                    return new OperandProperty(GetNestedName("Numero"));
                }
            }

            public OperandProperty Rechazado
            {
                get
                {
                    return new OperandProperty(GetNestedName("Rechazado"));
                }
            }

            public OperandProperty Recibido
            {
                get
                {
                    return new OperandProperty(GetNestedName("Recibido"));
                }
            }

            public Resultado.FieldsClass Resultado
            {
                get
                {
                    return new Resultado.FieldsClass(GetNestedName("Resultado"));
                }
            }

            public OperandProperty Serie
            {
                get
                {
                    return new OperandProperty(GetNestedName("Serie"));
                }
            }

            public Solicitud.FieldsClass Solicitud
            {
                get
                {
                    return new Solicitud.FieldsClass(GetNestedName("Solicitud"));
                }
            }
        }

        /*[PersistentAlias("Resultados[Fecha = ^.Resultados[].Max(Fecha)].Accion)")]
        public Resultado ResultadoActual => EvaluateAlias(nameof(ResultadoActual)) as Resultado;*/
        /*[Association("Cheque-Resultados")]
        public XPCollection<Resultado> Resultados => GetCollection<Resultado>(nameof(Resultados));*/
    }
}