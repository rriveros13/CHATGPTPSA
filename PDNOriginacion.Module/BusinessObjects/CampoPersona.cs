using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
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
    [DefaultProperty("Campo")]
    public class CampoPersona : BaseObject
    {
        static FieldsClass _Fields;
        private string _errorMessage;
        private Campo campo;
        private string criterioEditable;
        private string criterioVal;
        private string crterioVis;
        private bool obligatorio;

        public CampoPersona(Session session) : base(session)
        {
            ObjectType = typeof(Persona);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            obligatorio = false;
            ObjectType = typeof(Persona);
        }


        //[RuleRequiredField(DefaultContexts.Save)]
        public Campo Campo
        {
            get => campo;
            set => SetPropertyValue(nameof(Campo), ref campo, value);
        }

        [CriteriaOptions(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public string CriterioEditable
        {
            get => criterioEditable;
            set => SetPropertyValue(nameof(CriterioEditable), ref criterioEditable, value);
        }

        [CriteriaOptions(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        [XafDisplayName("Criterio Validación")]
        public string CriterioVal
        {
            get => criterioVal;
            set => SetPropertyValue(nameof(CriterioVal), ref criterioVal, value);
        }

        [CriteriaOptions(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        [XafDisplayName("Criterio Visibilidad")]
        public string CriterioVis
        {
            get => crterioVis;
            set => SetPropertyValue(nameof(CriterioVis), ref crterioVis, value);
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

        [Size(250)]
        public string MensajeError
        {
            get => _errorMessage;
            set => SetPropertyValue(nameof(MensajeError), ref _errorMessage, value);
        }

        [ValueConverter(typeof(TypeToStringConverter))]
        [ImmediatePostData]
        [TypeConverter(typeof(LocalizedClassInfoTypeConverter))]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public Type ObjectType
        {
            get => GetPropertyValue<Type>(nameof(ObjectType));
            set
            {
                bool cambio = SetPropertyValue(nameof(ObjectType), value);
                if (IsSaving || IsLoading || !cambio)
                {
                    return;
                }

                CriterioVis = string.Empty;
                CriterioVal = string.Empty;
                CriterioEditable = string.Empty;
            }
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public bool Obligatorio
        {
            get => obligatorio;
            set => SetPropertyValue(nameof(Obligatorio), ref obligatorio, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public Campo.FieldsClass Campo
            {
                get
                {
                    return new Campo.FieldsClass(GetNestedName("Campo"));
                }
            }

            public OperandProperty ConfiguracionEspecial
            {
                get
                {
                    return new OperandProperty(GetNestedName("ConfiguracionEspecial"));
                }
            }

            public OperandProperty CriterioEditable
            {
                get
                {
                    return new OperandProperty(GetNestedName("CriterioEditable"));
                }
            }

            public OperandProperty CriterioVal
            {
                get
                {
                    return new OperandProperty(GetNestedName("CriterioVal"));
                }
            }

            public OperandProperty CriterioVis
            {
                get
                {
                    return new OperandProperty(GetNestedName("CriterioVis"));
                }
            }

            public OperandProperty MensajeError
            {
                get
                {
                    return new OperandProperty(GetNestedName("MensajeError"));
                }
            }

            public OperandProperty ObjectType
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectType"));
                }
            }

            public OperandProperty Obligatorio
            {
                get
                {
                    return new OperandProperty(GetNestedName("Obligatorio"));
                }
            }

        }
    }
}