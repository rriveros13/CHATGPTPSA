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
    [DefaultProperty(nameof(Mensaje))]
    public class TransicionCriterio : BaseObject
    {
        static FieldsClass _Fields;
        private string _mensaje;
        private WFTransicion _transicion;

        public TransicionCriterio(Session session) : base(session) => ObjectType = typeof(Solicitud);

        public override void AfterConstruction() => base.AfterConstruction();

        [CriteriaOptions(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public string Criterio
        {
            get => GetPropertyValue<string>(nameof(Criterio));
            set => SetPropertyValue(nameof(Criterio), value);
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

        public string Mensaje
        {
            get => _mensaje;
            set => SetPropertyValue(nameof(Mensaje), ref _mensaje, value);
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
                if(!IsLoading && !IsSaving && cambio)
                {
                    Criterio = string.Empty;
                    OnChanged(nameof(Criterio));
                }
            }
        }

        [Association("Transicion-Criterios")]
        public WFTransicion Transicion
        {
            get => _transicion;
            set => SetPropertyValue(nameof(Transicion), ref _transicion, value);
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

            public OperandProperty Auditoria
            {
                get
                {
                    return new OperandProperty(GetNestedName("Auditoria"));
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

            public WFTransicion.FieldsClass Transicion
            {
                get
                {
                    return new WFTransicion.FieldsClass(GetNestedName("Transicion"));
                }
            }
        }
    }
}
