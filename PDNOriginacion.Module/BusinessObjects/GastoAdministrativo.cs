using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    public class GastoAdministrativo : BaseObject
    {
        public GastoAdministrativo(Session session) : base(session) => ObjectType = typeof(PresupuestoGasto);

        public override void AfterConstruction() => base.AfterConstruction();

        private Producto _producto;
        private string _descripcion;
        private string _cantidad;
        private decimal _monto;
        private bool _puedeEliminarse;
        private bool _puedeEditarMonto;
        private int _codigo;

        [ValueConverter(typeof(TypeToStringConverter))]
        [ImmediatePostData]
        [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
        [Appearance("NoMostrarObjectType", AppearanceItemType = nameof(ViewItem), TargetItems = nameof(ObjectType), Context = "Any", Visibility = ViewItemVisibility.Hide)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public Type ObjectType
        {
            get => GetPropertyValue<Type>(nameof(ObjectType));
            set
            {
                bool cambio = SetPropertyValue(nameof(ObjectType), value);
                if(!IsLoading && !IsSaving && cambio)
                {
                    Monto = string.Empty;
                    OnChanged(nameof(Monto));
                }
            }
        }

        [Association("Producto-GastoAdministrativo")]
        public Producto Producto
        {
            get => _producto;
            set => SetPropertyValue(nameof(Producto), ref _producto, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        [CriteriaOptions(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public string Cantidad
        {
            get => GetPropertyValue<string>(nameof(_cantidad));
            set => SetPropertyValue(nameof(_cantidad), value);
        }

        [CriteriaOptions(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public string Monto
        {
            get => GetPropertyValue<string>(nameof(_monto));
            set => SetPropertyValue(nameof(_monto), value);
        }

        public bool PuedeEliminarse
        {
            get => _puedeEliminarse;
            set => SetPropertyValue(nameof(PuedeEliminarse), ref _puedeEliminarse, value);
        }
        
        public bool PuedeEditarMonto
        {
            get => _puedeEditarMonto;
            set => SetPropertyValue(nameof(PuedeEditarMonto), ref _puedeEditarMonto, value);
        }

        public int Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
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
       
        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty ObjectType
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectType"));
                }
            }

            public Producto.FieldsClass Producto
            {
                get
                {
                    return new Producto.FieldsClass(GetNestedName("Producto"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public OperandProperty Cantidad
            {
                get
                {
                    return new OperandProperty(GetNestedName("Cantidad"));
                }
            }

            public OperandProperty Monto
            {
                get
                {
                    return new OperandProperty(GetNestedName("Monto"));
                }
            }

            public OperandProperty PuedeEliminarse
            {
                get
                {
                    return new OperandProperty(GetNestedName("PuedeEliminarse"));
                }
            }

            public OperandProperty PuedeEditarMonto
            {
                get
                {
                    return new OperandProperty(GetNestedName("PuedeEditarMonto"));
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

        static FieldsClass _Fields;

        // Created/Updated: XPS15-RB\rodol on XPS15-RB at 21/5/2019 12:43
    }
}