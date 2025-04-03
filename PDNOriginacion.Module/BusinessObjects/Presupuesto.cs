using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
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
    [DefaultProperty("Capital")]
    [RuleCriteria("AceptadoClienteUnico", DefaultContexts.Save, "Solicitud.Presupuestos[AceptadoCliente = True].Count() <= 1", "No puede existir más de un Presupuesto aceptado por el cliente", SkipNullOrEmptyValues = false)]
    public class Presupuesto : BaseObject, IObjectSpaceLink
    {
        static FieldsClass _Fields;

        public Presupuesto(Session session) : base(session)
        {
        }

        public static void CrearPresupuesto(Solicitud solicitud, bool esPropuesta)
        {
            Presupuesto prop = new Presupuesto(solicitud.Session);
            prop.ObjectSpace = solicitud.ObjectSpace;
            prop.Solicitud = solicitud;

            if (esPropuesta)
            {
                if (solicitud.MontoPropuesta <= 0)
                    throw new Exception("Debe establecer el monto de la propuesta");
                else
                    prop.Capital = solicitud.MontoPropuesta;
            }
            else
            {
                if (solicitud.MontoPresupuesto <= 0)
                    throw new Exception("Debe establecer el monto para el presupuesto");
                else
                    prop.Capital = solicitud.MontoPresupuesto;
            }

            prop.EsPropuesta = esPropuesta;
            prop.Fecha = DateTime.Now;

            if (!prop.Solicitud.Producto.PresupuestoGenSoloPrestamosDefault)
            {
                PresupuestoPrestamo.CrearPrestamo(prop, solicitud.TasaAnual / 12, solicitud.Plazo, prop.Solicitud.Producto.PrestamoConfiguracion.SistemaDefault);
            }

            if(prop.Solicitud.Producto.PresupuestoAceptadoClienteAuto) prop.AceptadoCliente = true;

            GenerarPrestamosDefault(prop);

            prop.Save();
        }

        private static void GenerarPrestamosDefault(Presupuesto presupuesto)
        {
            foreach (var item in presupuesto.Solicitud.Producto.PrestamoConfiguracion.PlazosDefault)
            {
                PresupuestoPrestamo.CrearPrestamo(presupuesto, item.Tasa/12, item.Plazo, presupuesto.Solicitud.Producto.PrestamoConfiguracion.SistemaDefault);
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _fecha = DateTime.Now;
            Creador = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
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

        [Association("Presupuesto-PrespuestoPrestamo")]
        [Aggregated]
        public XPCollection<PresupuestoPrestamo> Prestamos => GetCollection<PresupuestoPrestamo>(nameof(Prestamos));

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

            public OperandProperty Prestamos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Prestamos"));
                }
            }

            public OperandProperty CantPresupuestos
            {
                get
                {
                    return new OperandProperty(GetNestedName("CantPresupuestos"));
                }
            }

            public OperandProperty Capital
            {
                get
                {
                    return new OperandProperty(GetNestedName("Capital"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public Usuario.FieldsClass Creador
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("Creador"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public OperandProperty Aprobado
            {
                get
                {
                    return new OperandProperty(GetNestedName("Aprobado"));
                }
            }

            public OperandProperty FechaAprobacion
            {
                get
                {
                    return new OperandProperty(GetNestedName("FechaAprobacion"));
                }
            }

            public Usuario.FieldsClass Aprobador
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("Aprobador"));
                }
            }

            public Solicitud.FieldsClass Solicitud
            {
                get
                {
                    return new Solicitud.FieldsClass(GetNestedName("Solicitud"));
                }
            }

            public OperandProperty EsPropuesta
            {
                get
                {
                    return new OperandProperty(GetNestedName("EsPropuesta"));
                }
            }

            public OperandProperty AceptadoCliente
            {
                get
                {
                    return new OperandProperty(GetNestedName("AceptadoCliente"));
                }
            }
        }

        private decimal _capital;
        private Solicitud _solicitud;
        private Usuario _creador;
        private DateTime _fecha;
        private bool _esPropuesta;
        private bool _aprobado;
        private DateTime _fechaAprobacion;
        private Usuario _aprobador;
        private bool _aceptadoCliente;

        [PersistentAlias("Solicitud.Presupuestos.Count()")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public int CantPresupuestos => (int)EvaluateAlias(nameof(CantPresupuestos));

        //[RuleRequiredField(DefaultContexts.Save)]
        [ImmediatePostData]
        [Appearance("", Enabled = false, Criteria = "Prestamos.Count() > 0", Context = nameof(DetailView))]
        public decimal Capital
        {
            get => _capital;
            set
            {
                SetPropertyValue(nameof(Capital), ref _capital, value);
            }
        }

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [ModelDefault("AllowEdit", "false")]
        [NonCloneable]
        public Usuario Creador
        {
            get => _creador;
            set => SetPropertyValue(nameof(Creador), ref _creador, value);
        }

        [NonCloneable]
        [ModelDefault("AllowEdit", "false")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy hh:mm tt}")]
        public DateTime Fecha
        {
            get => _fecha;
            set => SetPropertyValue(nameof(Fecha), ref _fecha, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [Appearance("", TargetItems = nameof(Aprobado), Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "EsPropuesta = True")]
        public bool Aprobado
        {
            get => _aprobado;
            set => SetPropertyValue(nameof(Aprobado), ref _aprobado, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [Appearance("", TargetItems = nameof(FechaAprobacion), Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "EsPropuesta = True")]
        public DateTime FechaAprobacion
        {
            get => _fechaAprobacion;
            set => SetPropertyValue(nameof(FechaAprobacion), ref _fechaAprobacion, value);
        }                       

        [ModelDefault("AllowEdit", "false")]
        [Appearance("", TargetItems = nameof(Aprobador), Visibility = ViewItemVisibility.Hide, Criteria = "EsPropuesta = True")]
        public Usuario Aprobador
        {
            get => _aprobador;
            set => SetPropertyValue(nameof(Aprobador), ref _aprobador, value);
        }

        public Solicitud Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public bool EsPropuesta
        {
            get => _esPropuesta;
            set => SetPropertyValue(nameof(EsPropuesta), ref _esPropuesta, value);
        }


        [NonPersistent]
        [Appearance("", Enabled = false, Context = nameof(DetailView))]
        public bool Ultimo
        {
            get
            {
                Presupuesto ult;
                if (this.EsPropuesta)
                    ult = this.Solicitud.PropuestasComerciales
                                        .Where(x => x.Fecha == this.Solicitud.PropuestasComerciales.Max(y => y.Fecha) && x.EsPropuesta)
                                        .FirstOrDefault();
                else
                    ult = this.Solicitud.Presupuestos
                                        .Where(x => x.Fecha == this.Solicitud.Presupuestos.Max(y => y.Fecha) && !x.EsPropuesta)
                                        .FirstOrDefault();

                return ult == null || ult.Oid == Oid;
            }
        }

        [Appearance("", TargetItems = nameof(AceptadoCliente), Visibility = ViewItemVisibility.Hide, Criteria = "EsPropuesta = True")]
        [ImmediatePostData]        
        public bool AceptadoCliente
        {
            get => _aceptadoCliente;
            set => SetPropertyValue(nameof(AceptadoCliente), ref _aceptadoCliente, value);
        }
    }
}