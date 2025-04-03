using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("Action_Filter")]
    [DefaultProperty("Description")]
    public class FilteringCriterion : BaseObject
    {
        static FieldsClass _Fields;
        private string _descripcion;

        public FilteringCriterion(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [CriteriaOptions(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public string Criterion
        {
            get => GetPropertyValue<string>(nameof(Criterion));
            set => SetPropertyValue<string>(nameof(Criterion), value);
        }

        public string Description
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Description), ref _descripcion, value);
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

        [ValueConverter(typeof(TypeToStringConverter))]
        [ImmediatePostData]
        [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
        public Type ObjectType
        {
            get => GetPropertyValue<Type>(nameof(ObjectType));
            set
            {
                SetPropertyValue<Type>(nameof(ObjectType), value);
                Criterion = string.Empty;
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

            public OperandProperty Criterion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Criterion"));
                }
            }

            public OperandProperty Description
            {
                get
                {
                    return new OperandProperty(GetNestedName("Description"));
                }
            }

            public OperandProperty ObjectType
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectType"));
                }
            }
        }
    }
}

