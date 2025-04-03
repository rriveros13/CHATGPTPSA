using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Nombre")]
    public class WFTransicion : BaseObject
    {
        static FieldsClass _Fields;
        private bool _automatico;
        private EstadoSolicitud _estadoDestino;
        private EstadoSolicitud _estadoOrigen;
        private string _etiqueta;
        private string _nombre;
        private bool _esReversion;
        /*[ValueConverter(typeof(TypeToStringConverter)), ImmediatePostData]
        [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
        [Appearance("NoMostrarObjectType", AppearanceItemType = "ViewItem", TargetItems = "ObjectType", Context = "Any", Visibility = ViewItemVisibility.Hide)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public Type ObjectType
        {
        get { return GetPropertyValue<Type>("ObjectType"); }
        set
        {
        SetPropertyValue<Type>("ObjectType", typeof(Solicitud));
        Criterio = String.Empty;
        }
        }   */
        /*[CriteriaOptions("ObjectType"), Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public string Criterio
        {
        get { return GetPropertyValue<string>("Criterio"); }
        set { SetPropertyValue<string>("Criterio", value); }
        }    */
        private string _parametros;
        private Producto _producto;
        private Rol _rolUsuario;
        private int _orden;

        public WFTransicion(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        public bool Automatico
        {
            get => _automatico;
            set => SetPropertyValue(nameof(Automatico), ref _automatico, value);
        }

        [Association("Transicion-Criterios")]
        [Aggregated]
        [ImmediatePostData]
        public XPCollection<TransicionCriterio> Criterios => GetCollection<TransicionCriterio>(nameof(Criterios));

        //[RuleRequiredField(DefaultContexts.Save)]
        public EstadoSolicitud EstadoDestino
        {
            get => _estadoDestino;
            set => SetPropertyValue(nameof(EstadoDestino), ref _estadoDestino, value);
        }

        public EstadoSolicitud EstadoOrigen
        {
            get => _estadoOrigen;
            set => SetPropertyValue(nameof(EstadoOrigen), ref _estadoOrigen, value);
        }

        [Size(50)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string Etiqueta
        {
            get => _etiqueta;
            set => SetPropertyValue(nameof(Etiqueta), ref _etiqueta, value);
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
                if (ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
            }
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public string Nombre
        {
            get => _nombre;
            set => SetPropertyValue(nameof(Nombre), ref _nombre, value);
        }

        public string Parametros
        {
            get => _parametros;
            set => SetPropertyValue(nameof(Parametros), ref _parametros, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [Association("Producto-Transiciones")]
        public Producto Producto
        {
            get => _producto;
            set => SetPropertyValue(nameof(Producto), ref _producto, value);
        }

        public Rol RolUsuario
        {
            get => _rolUsuario;
            set => SetPropertyValue(nameof(RolUsuario), ref _rolUsuario, value);
        }

        public int Orden
        {
            get => _orden;
            set => SetPropertyValue(nameof(Orden), ref _orden, value);
        }

        [NonPersistent]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInReports(false), VisibleInDashboards(false)]
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

            public OperandProperty Automatico
            {
                get
                {
                    return new OperandProperty(GetNestedName("Automatico"));
                }
            }

            public OperandProperty Criterios
            {
                get
                {
                    return new OperandProperty(GetNestedName("Criterios"));
                }
            }

            public EstadoSolicitud.FieldsClass EstadoDestino
            {
                get
                {
                    return new EstadoSolicitud.FieldsClass(GetNestedName("EstadoDestino"));
                }
            }

            public EstadoSolicitud.FieldsClass EstadoOrigen
            {
                get
                {
                    return new EstadoSolicitud.FieldsClass(GetNestedName("EstadoOrigen"));
                }
            }

            public OperandProperty Etiqueta
            {
                get
                {
                    return new OperandProperty(GetNestedName("Etiqueta"));
                }
            }

            public OperandProperty Auditoria
            {
                get
                {
                    return new OperandProperty(GetNestedName("Auditoria"));
                }
            }

            public OperandProperty Nombre
            {
                get
                {
                    return new OperandProperty(GetNestedName("Nombre"));
                }
            }

            public OperandProperty Parametros
            {
                get
                {
                    return new OperandProperty(GetNestedName("Parametros"));
                }
            }

            public Producto.FieldsClass Producto
            {
                get
                {
                    return new Producto.FieldsClass(GetNestedName("Producto"));
                }
            }

            public Rol.FieldsClass RolUsuario
            {
                get
                {
                    return new Rol.FieldsClass(GetNestedName("RolUsuario"));
                }
            }
        }
    }
}