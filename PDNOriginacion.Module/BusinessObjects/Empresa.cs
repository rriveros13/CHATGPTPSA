//using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("RazonSocial")]
    [RuleCombinationOfPropertiesIsUnique("UnicidadEmpresa", DefaultContexts.Save, nameof(Ruc))]
    public class Empresa : BaseObject
    {
        static FieldsClass _Fields;
        private string _comentarios;
        private Direccion _direccion;
        private string _razonsocial;
        private string _ruc;

        public Empresa(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [Association("Adjunto-Empresa")]
        public XPCollection<Adjunto> Adjuntos => GetCollection<Adjunto>(nameof(Adjuntos));

        [Size(SizeAttribute.Unlimited)]
        public string Comentarios
        {
            get => _comentarios;
            set => SetPropertyValue(nameof(Comentarios), ref _comentarios, value);
        }

        public Direccion Direccion
        {
            get => _direccion;
            set => SetPropertyValue(nameof(Direccion), ref _direccion, value);
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

        [Association("Persona-Empresa")]
        public XPCollection<Persona> Firmantes => GetCollection<Persona>(nameof(Firmantes));

        [Size(60)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string RazonSocial
        {
            get => _razonsocial;
            set => SetPropertyValue(nameof(RazonSocial), ref _razonsocial, value);
        }

        [Size(50)]
        public string Ruc
        {
            get => _ruc;
            set => SetPropertyValue(nameof(Ruc), ref _ruc, value);
        }

        [Association("Empresa-EmpresaTelefono")]
        [Aggregated]
        public XPCollection<EmpresaTelefono> Telefonos => GetCollection<EmpresaTelefono>(nameof(Telefonos));

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

            public OperandProperty Adjuntos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Adjuntos"));
                }
            }

            public OperandProperty Comentarios
            {
                get
                {
                    return new OperandProperty(GetNestedName("Comentarios"));
                }
            }

            public Direccion.FieldsClass Direccion
            {
                get
                {
                    return new Direccion.FieldsClass(GetNestedName("Direccion"));
                }
            }

            public OperandProperty Firmantes
            {
                get
                {
                    return new OperandProperty(GetNestedName("Firmantes"));
                }
            }

            public OperandProperty RazonSocial
            {
                get
                {
                    return new OperandProperty(GetNestedName("RazonSocial"));
                }
            }

            public OperandProperty Ruc
            {
                get
                {
                    return new OperandProperty(GetNestedName("Ruc"));
                }
            }

            public OperandProperty Telefonos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Telefonos"));
                }
            }
        }
    }
}