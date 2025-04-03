using DevExpress.Data.Filtering;
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
    [ImageName("Action_Filter")]
    [DefaultProperty(nameof(Mensaje))]
    public class GenSolicitudCriterio : BaseObject
    {
        static FieldsClass _Fields;
        private string mensaje;
        private Producto _producto;

        public GenSolicitudCriterio(Session session) : base(session) => ObjectType = typeof(Persona);

        public override void AfterConstruction() => base.AfterConstruction();

        [CriteriaOptions(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public string Criterio
        {
            get => GetPropertyValue<string>(nameof(Criterio));
            set => SetPropertyValue(nameof(Criterio), value);
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

        public string Mensaje
        {
            get => mensaje;
            set => SetPropertyValue(nameof(Mensaje), ref mensaje, value);
        }

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
                if (!IsLoading && !IsSaving && cambio)
                {
                    Criterio = string.Empty;
                    OnChanged(nameof(Criterio));
                }
            }
        }

        [Association("Producto-GenSolicitudCriterios")]
        public Producto Producto
        {
            get => _producto;
            set => SetPropertyValue(nameof(Producto), ref _producto, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Criterio
            {
                get
                {
                    return new OperandProperty(GetNestedName("Criterio"));
                }
            }

            public OperandProperty Mensaje
            {
                get
                {
                    return new OperandProperty(GetNestedName("Mensaje"));
                }
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
        }
    }
}
