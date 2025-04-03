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
    public class Parametro : BaseObject
    {
        static FieldsClass _Fields;
        private string _categoria;
        private string _descripcion;
        private string _nombre;
        private string _valor;
        private Configuracion conf;

        public Parametro(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [Size(30)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string Categoria
        {
            get => _categoria;
            set => SetPropertyValue(nameof(Categoria), ref _categoria, value);
        }

        [Association("Parametros")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public Configuracion Configuracion
        {
            get => conf;
            set => SetPropertyValue(nameof(Configuracion), ref conf, value);
        }

        [Size(255)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
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

        [Size(30)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [Indexed(Unique = true)]
        public string Nombre
        {
            get => _nombre;
            set => SetPropertyValue(nameof(Nombre), ref _nombre, value);
        }

        [Size(255)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string Valor
        {
            get => _valor;
            set => SetPropertyValue(nameof(Valor), ref _valor, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Categoria
            {
                get
                {
                    return new OperandProperty(GetNestedName("Categoria"));
                }
            }

            public Configuracion.FieldsClass Configuracion
            {
                get
                {
                    return new Configuracion.FieldsClass(GetNestedName("Configuracion"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public OperandProperty Nombre
            {
                get
                {
                    return new OperandProperty(GetNestedName("Nombre"));
                }
            }

            public OperandProperty Valor
            {
                get
                {
                    return new OperandProperty(GetNestedName("Valor"));
                }
            }
        }
    }
}