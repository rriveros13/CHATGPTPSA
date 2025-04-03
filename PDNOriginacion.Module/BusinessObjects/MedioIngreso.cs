using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Medio")]
    public class MedioIngreso : BaseObject
    {
        static FieldsClass _Fields;
        private bool _activo;
        private bool _default;
        private string _medio;
        private bool _requiereTelefono;
        private string _codigo;

        public MedioIngreso(Session session) : base(session)
        {
            _default = false;
            _activo = true;
        }

        [Indexed]
        public bool Activo
        {
            get => _activo;
            set => SetPropertyValue(nameof(Activo), ref _activo, value);
        }

        [Indexed]
        public bool Default
        {
            get => _default;
            set => SetPropertyValue(nameof(Default), ref _default, value);
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

        [RuleUniqueValue]
        public string Medio
        {
            get => _medio;
            set => SetPropertyValue(nameof(Medio), ref _medio, value);
        }

        [Indexed]
        public bool RequiereTelefono
        {
            get => _requiereTelefono;
            set => SetPropertyValue(nameof(RequiereTelefono), ref _requiereTelefono, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue]
        [Size(50)]
        public string Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Activo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Activo"));
                }
            }

            public OperandProperty Default
            {
                get
                {
                    return new OperandProperty(GetNestedName("Default"));
                }
            }

            public OperandProperty Medio
            {
                get
                {
                    return new OperandProperty(GetNestedName("Medio"));
                }
            }

            public OperandProperty RequiereTelefono
            {
                get
                {
                    return new OperandProperty(GetNestedName("RequiereTelefono"));
                }
            }
        }
    }
}