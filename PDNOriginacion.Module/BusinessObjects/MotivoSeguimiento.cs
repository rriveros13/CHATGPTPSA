using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Motivo")]
    public class MotivoSeguimiento : BaseObject
    {
        static FieldsClass _Fields;
        private bool _activo;
        private bool _default;
        private string _motivo;
        private string _codigo;

        public MotivoSeguimiento(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _activo = true;
            _default = false;
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
                if (ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
            }
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue]
        public string Motivo
        {
            get => _motivo;
            set => SetPropertyValue(nameof(Motivo), ref _motivo, value);
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

            public OperandProperty Motivo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Motivo"));
                }
            }
        }
    }
}