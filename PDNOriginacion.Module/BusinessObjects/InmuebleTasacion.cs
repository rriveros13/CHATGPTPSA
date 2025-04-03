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
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Fecha")]
    public class InmuebleTasacion : BaseObject
    {
        static FieldsClass _Fields;
        private DateTime _creado;
        private DateTime _fecha;
        private Inmueble _inmueble;
        private decimal _superficieConstruidaM2;
        private decimal _superficieM2;
        private decimal _totalConstruccion;
        private decimal _totalInmueble;
        private decimal _totalTerreno;
        private Usuario _usuario;
        private decimal _valorM2Construidos;
        private decimal _valorM2Terreno;
        private Tasador _tasador;
        private bool _esValorizacion;
        private string _descripcionVecindad;
        private string _descripcionInmueble;
        private string _descripcionAcceso;
        private string _descripcionAdquisicion;
        private string _descripcionHabitantes;
        private string _motivoCredito;

        public InmuebleTasacion(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            Creado = DateTime.Now;
            Usuario = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
            base.AfterConstruction();
        }

        #region Propiedades
        //[RuleRequiredField(DefaultContexts.Save)]
        public Inmueble Inmueble
        {
            get => _inmueble;
            set => SetPropertyValue(nameof(Inmueble), ref _inmueble, value);
        }


        [Appearance("", Enabled = false, Context = nameof(DetailView))]
        public DateTime Creado
        {
            get => _creado;
            set => SetPropertyValue(nameof(Creado), ref _creado, value);
        }

        [Appearance("", Enabled = false, Context = nameof(DetailView))]
        public Usuario Usuario
        {
            get => _usuario;
            set => SetPropertyValue(nameof(Usuario), ref _usuario, value);
        }

        public DateTime Fecha
        {
            get => _fecha;
            set => SetPropertyValue(nameof(Fecha), ref _fecha, value);
        }

        [Appearance("", TargetItems = nameof(Tasador), Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "EsValorizacion = True")]
        public Tasador Tasador
        {
            get => _tasador;
            set => SetPropertyValue(nameof(Tasador), ref _tasador, value);
        }

        [ImmediatePostData]
        public decimal SuperficieM2
        {
            get => _superficieM2;
            set
            {
                SetPropertyValue(nameof(SuperficieM2), ref _superficieM2, value);
                if (!IsLoading && !IsSaving)
                {
                    TotalTerreno = ValorM2Terreno * value;
                    OnChanged(nameof(TotalTerreno));
                }
            }
        }

        [ImmediatePostData]
        public decimal ValorM2Terreno
        {
            get => _valorM2Terreno;
            set
            {
                SetPropertyValue(nameof(ValorM2Terreno), ref _valorM2Terreno, value);
                if (!IsLoading && !IsSaving)
                {
                    TotalTerreno = SuperficieM2 * value;
                }
            }
        }

        [ImmediatePostData]
        public decimal TotalTerreno
        {
            get => _totalTerreno;
            set
            {
                SetPropertyValue(nameof(TotalTerreno), ref _totalTerreno, value);
                if (!IsLoading && !IsSaving)
                {
                    TotalInmueble = value + TotalConstruccion;
                }
            }
        }

        [ImmediatePostData]
        public decimal SuperficieConstruidaM2
        {
            get => _superficieConstruidaM2;
            //set => SetPropertyValue(nameof(SuperficieConstruidaM2), ref _superficieConstruidaM2, value);
            set
            {
                SetPropertyValue(nameof(SuperficieConstruidaM2), ref _superficieConstruidaM2, value);
                if (!IsLoading && !IsSaving)
                {
                    TotalConstruccion = ValorM2Construidos * value;
                }
            }
        }

        [ImmediatePostData]
        public decimal ValorM2Construidos
        {
            get => _valorM2Construidos;
            set
            {
                SetPropertyValue(nameof(ValorM2Construidos), ref _valorM2Construidos, value);
                if (!IsLoading && !IsSaving)
                {
                    TotalConstruccion = SuperficieConstruidaM2 * value;
                }
            }
        }

        [ImmediatePostData]
        public decimal TotalConstruccion
        {
            get => _totalConstruccion;
            set
            {
                SetPropertyValue(nameof(TotalConstruccion), ref _totalConstruccion, value);
                if (!IsLoading && !IsSaving)
                {
                    TotalInmueble = value + TotalTerreno;
                }
            }
        }

        public decimal TotalInmueble
        {
            get => _totalInmueble;
            set => SetPropertyValue(nameof(TotalInmueble), ref _totalInmueble, value);
        }

        [Size(500)]
        [Appearance("", TargetItems = nameof(DescripcionVecindad), Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "EsValorizacion = True")]
        public string DescripcionVecindad
        {
            get => _descripcionVecindad;
            set => SetPropertyValue(nameof(DescripcionVecindad), ref _descripcionVecindad, value);
        }

        [Size(500)]
        [Appearance("", TargetItems = nameof(DescripcionInmueble), Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "EsValorizacion = True")]
        public string DescripcionInmueble
        {
            get => _descripcionInmueble;
            set => SetPropertyValue(nameof(DescripcionInmueble), ref _descripcionInmueble, value);
        }

        [Size(500)]
        [Appearance("", TargetItems = nameof(DescripcionAcceso), Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "EsValorizacion = True")]
        public string DescripcionAcceso
        {
            get => _descripcionAcceso;
            set => SetPropertyValue(nameof(DescripcionAcceso), ref _descripcionAcceso, value);
        }

        [Size(500)]
        [Appearance("", TargetItems = nameof(DescripcionAdquisicion), Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "EsValorizacion = True")]
        public string DescripcionAdquisicion
        {
            get => _descripcionAdquisicion;
            set => SetPropertyValue(nameof(DescripcionAdquisicion), ref _descripcionAdquisicion, value);
        }

        [Size(500)]
        [Appearance("", TargetItems = nameof(DescripcionHabitantes), Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "EsValorizacion = True")]
        public string DescripcionHabitantes
        {
            get => _descripcionHabitantes;
            set => SetPropertyValue(nameof(DescripcionHabitantes), ref _descripcionHabitantes, value);
        }

        [Size(500)]
        [Appearance("", TargetItems = nameof(DescripcionHabitantes), Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "EsValorizacion = True")]
        [XafDisplayName("Motivo del Crédito")]
        public string MotivoCredito
        {
            get => _motivoCredito;
            set => SetPropertyValue(nameof(MotivoCredito), ref _motivoCredito, value);
        }

        [NonPersistent]
        [Appearance("", Enabled = false, Context = nameof(DetailView))]
        public bool Ultimo
        {
            get
            {
                 InmuebleTasacion ult;
                 if (this.EsValorizacion)
                     ult = Inmueble.Valorizaciones
                                         .Where(x => x.Creado == Inmueble.Valorizaciones.Max(y => y.Creado))
                                         .FirstOrDefault();
                 else
                     ult = Inmueble.Tasaciones
                     .Where(x => x.Creado == Inmueble.Tasaciones.Max(y => y.Creado))
                     .FirstOrDefault();

                 return ult == null || ult.Oid == Oid; 
            }
        }

        [NonPersistent]
        [Appearance("", Enabled = false)]
        [XafDisplayName("25% del valor del inmueble")]
        public decimal Valor25
        {
            get
            {
                return TotalInmueble * 0.25m;
            }
        }

        [NonPersistent]
        [Appearance("", Enabled = false)]
        [XafDisplayName("28% del valor del inmueble")]
        public decimal Valor28
        {
            get
            {
                return TotalInmueble * 0.28m;
            }
        }

        [NonPersistent]
        [Appearance("", Enabled = false)]
        [XafDisplayName("30% del valor del inmueble")]
        public decimal Valor30
        {
            get
            {
                return TotalInmueble * 0.3m;
            }
        }

        [VisibleInDetailView(false)]
        public bool EsValorizacion
        {
            get => _esValorizacion;
            set => SetPropertyValue(nameof(EsValorizacion), ref _esValorizacion, value);
        }
        #endregion Propiedades

        #region Asociociones
        [Association("Nota-Tasacion")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<NotaTasacion> Notas => GetCollection<NotaTasacion>(nameof(Notas));

        [Association("InmuebleTasacion-Adjuntos")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<Adjunto> Adjuntos => GetCollection<Adjunto>(nameof(Adjuntos));
        #endregion Asociaciones

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

            public Inmueble.FieldsClass Inmueble
            {
                get
                {
                    return new Inmueble.FieldsClass(GetNestedName("Inmueble"));
                }
            }

            public OperandProperty Creado
            {
                get
                {
                    return new OperandProperty(GetNestedName("Creado"));
                }
            }

            public Usuario.FieldsClass Usuario
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("Usuario"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public OperandProperty Tasador
            {
                get
                {
                    return new OperandProperty(GetNestedName("Tasador"));
                }
            }

            public OperandProperty SuperficieM2
            {
                get
                {
                    return new OperandProperty(GetNestedName("SuperficieM2"));
                }
            }

            public OperandProperty ValorM2Terreno
            {
                get
                {
                    return new OperandProperty(GetNestedName("ValorM2Terreno"));
                }
            }

            public OperandProperty TotalTerreno
            {
                get
                {
                    return new OperandProperty(GetNestedName("TotalTerreno"));
                }
            }

            public OperandProperty SuperficieConstruidaM2
            {
                get
                {
                    return new OperandProperty(GetNestedName("SuperficieConstruidaM2"));
                }
            }

            public OperandProperty ValorM2Construidos
            {
                get
                {
                    return new OperandProperty(GetNestedName("ValorM2Construidos"));
                }
            }

            public OperandProperty TotalConstruccion
            {
                get
                {
                    return new OperandProperty(GetNestedName("TotalConstruccion"));
                }
            }

            public OperandProperty TotalInmueble
            {
                get
                {
                    return new OperandProperty(GetNestedName("TotalInmueble"));
                }
            }

            public OperandProperty DescripcionVecindad
            {
                get
                {
                    return new OperandProperty(GetNestedName("DescripcionVecindad"));
                }
            }

            public OperandProperty DescripcionInmueble
            {
                get
                {
                    return new OperandProperty(GetNestedName("DescripcionInmueble"));
                }
            }

            public OperandProperty DescripcionAcceso
            {
                get
                {
                    return new OperandProperty(GetNestedName("DescripcionAcceso"));
                }
            }

            public OperandProperty DescripcionAdquisicion
            {
                get
                {
                    return new OperandProperty(GetNestedName("DescripcionAdquisicion"));
                }
            }

            public OperandProperty DescripcionHabitantes
            {
                get
                {
                    return new OperandProperty(GetNestedName("DescripcionHabitantes"));
                }
            }

            public OperandProperty EsValorizacion
            {
                get
                {
                    return new OperandProperty(GetNestedName("EsValorizacion"));
                }
            }

            public OperandProperty Notas
            {
                get
                {
                    return new OperandProperty(GetNestedName("Notas"));
                }
            }

            public OperandProperty Adjuntos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Adjuntos"));
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
    }
}