using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("CuentaCatastral")]

    [RuleCriteria("DelInmueble", DefaultContexts.Delete, "Solicitudes.Count == 0 and Propietarios.Count == 0", "No se puede eliminar el inumeble porque ya se encuentra asociado a solicitudes o personas.!", SkipNullOrEmptyValues = true)]
    public class Inmueble : BaseObject, IObjectSpaceLink
    {
        static FieldsClass _Fields;
        private string _cuentaCatastral;
        private Direccion _direccion;
        private decimal _superficieConstruidaM2;
        private decimal _superficieM2;
        private TipoCamino _tipoCamino;
        private TipoInmueble _tipoInmueble;
        private EstadoTitulo _estadoTitulo;
        decimal valorAproximado;
        private string _nroFinca;
        private int _nroFolio;
        private int _nroInscripcion;
        private DateTime _fechaInscripcion;
        private bool _impuestoAlDia;
        private string _observaciones;
        private DateTime _fechaEscritura;
        private string _serie;

        public Inmueble(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        #region Propiedades
        [Size(20)]
        public string CuentaCatastral
        {
            get => _cuentaCatastral;
            set => SetPropertyValue(nameof(CuentaCatastral), ref _cuentaCatastral, value);
        }

        public Direccion Direccion
        {
            get => _direccion;
            set
            {
                SetPropertyValue(nameof(Direccion), ref _direccion, value);
                if(!IsSaving && !IsLoading)
                {
                    if(_direccion != null)
                    {
                        _direccion.Inmueble = this;
                        _direccion.Save();
                    }
                }
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

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        public decimal SuperficieConstruidaM2
        {
            get => _superficieConstruidaM2;
            set => SetPropertyValue(nameof(SuperficieConstruidaM2), ref _superficieConstruidaM2, value);
        }

        public decimal SuperficieM2
        {
            get => _superficieM2;
            set => SetPropertyValue(nameof(SuperficieM2), ref _superficieM2, value);
        }

        public TipoCamino TipoCamino
        {
            get => _tipoCamino;
            set => SetPropertyValue(nameof(TipoCamino), ref _tipoCamino, value);
        }

        public TipoInmueble TipoInmueble
        {
            get => _tipoInmueble;
            set => SetPropertyValue(nameof(TipoInmueble), ref _tipoInmueble, value);
        }

        [PersistentAlias("Iif(Tasaciones.Count > 0, Tasaciones[Fecha=^.Tasaciones[].Max(Fecha)].Max(TotalInmueble), ValorAproximado)")]
        public decimal Valor => (decimal)EvaluateAlias(nameof(Valor));

        [Appearance("HideValorAproximado", TargetItems = nameof(ValorAproximado), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "Tasaciones.Count > 0")]
        public decimal ValorAproximado
        {
            get => valorAproximado;
            set => SetPropertyValue(nameof(ValorAproximado), ref valorAproximado, value);
        }

        [XafDisplayName("Estado del título")]
        public EstadoTitulo EstadoTitulo
        {
            get => _estadoTitulo;
            set => SetPropertyValue(nameof(EstadoTitulo), ref _estadoTitulo, value);
        }

        [Size(20)]
        public string NumeroFinca
        {
            get => _nroFinca;
            set => SetPropertyValue(nameof(NumeroFinca), ref _nroFinca, value);
        }

        public int NumeroFolio
        {
            get => _nroFolio;
            set => SetPropertyValue(nameof(NumeroFolio), ref _nroFolio, value);
        }

        public int NumeroInscripcion
        {
            get => _nroInscripcion;
            set => SetPropertyValue(nameof(NumeroInscripcion), ref _nroInscripcion, value);
        }

        public DateTime FechaInscripcion
        {
            get => _fechaInscripcion;
            set => SetPropertyValue(nameof(FechaInscripcion), ref _fechaInscripcion, value);
        }

        [Size(5)]
        public string Serie
        {
            get => _serie;
            set => SetPropertyValue(nameof(Serie), ref _serie, value);
        }

        [XafDisplayName("Tiene impuesto al día")]
        public bool ImpuestoAlDia
        {
            get => _impuestoAlDia;
            set => SetPropertyValue(nameof(ImpuestoAlDia), ref _impuestoAlDia, value);
        }

        [Size(500)]
        public string Observaciones
        {
            get => _observaciones;
            set => SetPropertyValue(nameof(Observaciones), ref _observaciones, value);
        }

        public DateTime FechaEscritura
        {
            get => _fechaEscritura;
            set => SetPropertyValue(nameof(FechaEscritura), ref _fechaEscritura, value);
        }
        #endregion Propiedades

        protected override void OnSaved()
        {
            base.OnSaved();

            foreach (var item in this.Propietarios)
            {
                Persona.CambiarEstadoPersona(item, true);
            }
           
        }

        [PersistentAlias("Iif(!IsNull(Direccion), Direccion.Ciudad, null)")]
        [VisibleInDashboards(false), VisibleInReports(false)]
        public Ciudad Ciudad=> EvaluateAlias(nameof(Ciudad)) as Ciudad;

        #region Asosociaciones
        [Association("Inmueble-Propietarios")]
        public XPCollection<Persona> Propietarios => GetCollection<Persona>(nameof(Propietarios));

        [Association("Inmueble-Referencias")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<InmuebleReferencia> Referencias => GetCollection<InmuebleReferencia>(nameof(Referencias));

        [Association("Inmuebles-Solicitudes")]
        public XPCollection<Solicitud> Solicitudes => GetCollection<Solicitud>(nameof(Solicitudes));

        [Association("Inmueble-Adjuntos")]
        public XPCollection<Adjunto> Adjuntos=> GetCollection<Adjunto>(nameof(Adjuntos));

        public XPCollection<InmuebleTasacion> Valorizaciones
        {
            get
            {
                return new XPCollection<InmuebleTasacion>(this.Session, CriteriaOperator.Parse($"EsValorizacion = True and Inmueble = '{this.Oid}'"));
            }
        }

        public XPCollection<InmuebleTasacion> Tasaciones
        {
            get
            {
                return new XPCollection<InmuebleTasacion>(this.Session, CriteriaOperator.Parse($"EsValorizacion = False and Inmueble = '{this.Oid}'"));
            }
        }

        //public string Title => "Inmueble";

        //[PersistentAlias("Iif(IsNull(Direccion), null, Iif(IsNull(Direccion.Localizacion), null, Direccion.Localizacion.Latitude))")]
        //public double Latitude => (double)EvaluateAlias(nameof(Latitude));

        //[PersistentAlias("Iif(IsNull(Direccion), null, Iif(IsNull(Direccion.Localizacion), null, Direccion.Localizacion.Longitude))")]
        //public double Longitude => (double)EvaluateAlias(nameof(Longitude));

        #endregion Asosociaciones

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
        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty CuentaCatastral
            {
                get
                {
                    return new OperandProperty(GetNestedName("CuentaCatastral"));
                }
            }

            public Direccion.FieldsClass Direccion
            {
                get
                {
                    return new Direccion.FieldsClass(GetNestedName("Direccion"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public OperandProperty SuperficieConstruidaM2
            {
                get
                {
                    return new OperandProperty(GetNestedName("SuperficieConstruidaM2"));
                }
            }

            public OperandProperty SuperficieM2
            {
                get
                {
                    return new OperandProperty(GetNestedName("SuperficieM2"));
                }
            }

            public TipoCamino.FieldsClass TipoCamino
            {
                get
                {
                    return new TipoCamino.FieldsClass(GetNestedName("TipoCamino"));
                }
            }

            public OperandProperty Valor
            {
                get
                {
                    return new OperandProperty(GetNestedName("Valor"));
                }
            }

            public OperandProperty ValorAproximado
            {
                get
                {
                    return new OperandProperty(GetNestedName("ValorAproximado"));
                }
            }

            public OperandProperty Propietarios
            {
                get
                {
                    return new OperandProperty(GetNestedName("Propietarios"));
                }
            }

            public OperandProperty Referencias
            {
                get
                {
                    return new OperandProperty(GetNestedName("Referencias"));
                }
            }

            public OperandProperty Solicitudes
            {
                get
                {
                    return new OperandProperty(GetNestedName("Solicitudes"));
                }
            }

            public OperandProperty Valorizaciones
            {
                get
                {
                    return new OperandProperty(GetNestedName("Valorizaciones"));
                }
            }

            public OperandProperty Tasaciones
            {
                get
                {
                    return new OperandProperty(GetNestedName("Tasaciones"));
                }
            }
        }
    }
}

namespace PDNOriginacion.Module.BusinessObjects
{
    public enum EstadoTitulo
    {
        SiTiene,
        NoTiene,
        EnGestion,
        Padron
    }
}