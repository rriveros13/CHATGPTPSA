using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(Entidad))]
    public class Referencia : BaseObject
    {
        static FieldsClass _Fields;
        private string _comentarios;
        private string _entidad;
        private DateTime _fecha;
        private bool _verificada;
        private Usuario _verificadaPor;

        public Referencia(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _verificada = false;
        }

        [Size(SizeAttribute.Unlimited)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string Comentarios
        {
            get => _comentarios;
            set => SetPropertyValue(nameof(Comentarios), ref _comentarios, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public string Entidad
        {
            get => _entidad;
            set => SetPropertyValue(nameof(Entidad), ref _entidad, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [NonCloneable]
        public DateTime Fecha
        {
            get => _fecha;
            set => SetPropertyValue(nameof(Fecha), ref _fecha, value);
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

        public bool Verificada
        {
            get => _verificada;
            set => SetPropertyValue(nameof(Verificada), ref _verificada, value);
        }

        [ModelDefault("Editable", "false")]
        public Usuario VerificadaPor
        {
            get => _verificadaPor;
            set => SetPropertyValue(nameof(VerificadaPor), ref _verificadaPor, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Comentarios
            {
                get
                {
                    return new OperandProperty(GetNestedName("Comentarios"));
                }
            }

            public OperandProperty Entidad
            {
                get
                {
                    return new OperandProperty(GetNestedName("Entidad"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public OperandProperty Verificada
            {
                get
                {
                    return new OperandProperty(GetNestedName("Verificada"));
                }
            }

            public Usuario.FieldsClass VerificadaPor
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("VerificadaPor"));
                }
            }
        }
    }
}