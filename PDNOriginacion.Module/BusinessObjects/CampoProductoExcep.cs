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
    [DefaultProperty("CampoProducto")]
    [DeferredDeletion(false)]
    public class CampoProductoExcep : BaseObject
    {
        static FieldsClass _Fields;
        private CampoProducto _campoProducto;
        private string _errorMessage;
        private EstadoSolicitud _estadoDestino;
        private WFTarea _wfTarea;
        private bool obligatorio;

        public CampoProductoExcep(Session session) : base(session) => ObjectType = typeof(Solicitud);

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            obligatorio = false;
            ObjectType = typeof(Solicitud);
        }

        [Association("CampoProducto-CampoProductoExcep")]
        public CampoProducto CampoProducto
        {
            get => _campoProducto;
            set => SetPropertyValue(nameof(CampoProducto), ref _campoProducto, value);
        }

        [CriteriaOptions(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        public string CriterioEditable
        {
            get => GetPropertyValue<string>(nameof(CriterioEditable));
            set => SetPropertyValue(nameof(CriterioEditable), value);
        }

        [CriteriaOptions(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        [XafDisplayName("Criterio Validación")]
        public string CriterioVal
        {
            get => GetPropertyValue<string>(nameof(CriterioVal));
            set => SetPropertyValue(nameof(CriterioVal), value);
        }

        [CriteriaOptions(nameof(ObjectType))]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.PopupCriteriaPropertyEditor)]
        [XafDisplayName("Criterio Visibilidad")]
        public string CriterioVis
        {
            get => GetPropertyValue<string>(nameof(CriterioVis));
            set => SetPropertyValue(nameof(CriterioVis), value);
        }

        [DataSourceProperty("CampoProducto.Producto.EstadosUsados")]
        public EstadoSolicitud EstadoDestino
        {
            get => _estadoDestino;
            set => SetPropertyValue(nameof(EstadoDestino), ref _estadoDestino, value);
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
                SetPropertyValue(nameof(ObjectType), value);
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

        [DataSourceProperty("CampoProducto.Producto.Tareas")]
        public WFTarea WFTarea
        {
            get => _wfTarea;
            set => SetPropertyValue(nameof(WFTarea), ref _wfTarea, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public CampoProducto.FieldsClass CampoProducto
            {
                get
                {
                    return new CampoProducto.FieldsClass(GetNestedName("CampoProducto"));
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

            public EstadoSolicitud.FieldsClass EstadoDestino
            {
                get
                {
                    return new EstadoSolicitud.FieldsClass(GetNestedName("EstadoDestino"));
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

            public WFTarea.FieldsClass WFTarea
            {
                get
                {
                    return new WFTarea.FieldsClass(GetNestedName("WFTarea"));
                }
            }
        }
    }
}