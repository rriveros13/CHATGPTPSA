using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
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
    [DefaultProperty("Nombre")]
    public class Modelo : BaseObject
    {
        static FieldsClass _Fields;
        private string _cliente;
        private string _codigo;
        private string _descripcion;
        private DateTime _fechavigencia;
        private string _nombre;
        private TipoProducto _tipoproducto;
        private int _version;
        private bool _vigente;
        private string _xmlInput;

        public Modelo(Session session) : base(session)
        {
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public string Cliente
        {
            get => _cliente;
            set => SetPropertyValue(nameof(Cliente), ref _cliente, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [Size(20)]
        [Appearance("Inhabilitar-Codigo", AppearanceItemType = "ViewItem", Criteria = "Vigente = true", TargetItems = nameof(Codigo), Context = "Any", Enabled = false)]
        public string Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        [Size(200)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        [Appearance("Inhabilitar-FechaVigencia", AppearanceItemType = "ViewItem", TargetItems = nameof(FechaVigencia), Context = "Any", Enabled = false)]
        public DateTime FechaVigencia
        {
            get => _fechavigencia;
            set => SetPropertyValue(nameof(Version), ref _fechavigencia, value);
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

        //[RuleRequiredField(DefaultContexts.Save)]
        public string Nombre
        {
            get => _nombre;
            set => SetPropertyValue(nameof(Nombre), ref _nombre, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [Association("TipoProductos-Modelo")]
        public TipoProducto TipoProducto
        {
            get => _tipoproducto;
            set => SetPropertyValue(nameof(Cliente), ref _tipoproducto, value);
        }

        [Appearance("Inhabilitar-Version", AppearanceItemType = "ViewItem", TargetItems = nameof(Version), Context = "Any", Enabled = false)]
        public int Version
        {
            get => _version;
            set => SetPropertyValue(nameof(Version), ref _version, value);
        }

        [Appearance("Inhabilitar-Vigente", AppearanceItemType = "ViewItem", TargetItems = nameof(Vigente), Context = "Any", Enabled = false)]
        public bool Vigente
        {
            get => _vigente;
            set => SetPropertyValue(nameof(Vigente), ref _vigente, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string XmlInput
        {
            get => _xmlInput;
            set => SetPropertyValue(nameof(XmlInput), ref _xmlInput, value);
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

            public OperandProperty Cliente
            {
                get
                {
                    return new OperandProperty(GetNestedName("Cliente"));
                }
            }

            public OperandProperty Codigo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Codigo"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public OperandProperty FechaVigencia
            {
                get
                {
                    return new OperandProperty(GetNestedName("FechaVigencia"));
                }
            }

            public OperandProperty Nombre
            {
                get
                {
                    return new OperandProperty(GetNestedName("Nombre"));
                }
            }

            public TipoProducto.FieldsClass TipoProducto
            {
                get
                {
                    return new TipoProducto.FieldsClass(GetNestedName("TipoProducto"));
                }
            }

            public OperandProperty Version
            {
                get
                {
                    return new OperandProperty(GetNestedName("Version"));
                }
            }

            public OperandProperty Vigente
            {
                get
                {
                    return new OperandProperty(GetNestedName("Vigente"));
                }
            }

            public OperandProperty XmlInput
            {
                get
                {
                    return new OperandProperty(GetNestedName("XmlInput"));
                }
            }
        }
    }
}

