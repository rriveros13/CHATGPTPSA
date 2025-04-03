using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Nombre")]
    [DeferredDeletion(Enabled = false)]
    public class Escribania : BaseObject
    {
        string observacion;
        static FieldsClass _Fields;
        bool activo;
        bool @default;
        string nombre;

        public Escribania(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            activo = true;
            @default = false;
        }

        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        public bool Default
        {
            get => @default;
            set => SetPropertyValue(nameof(Default), ref @default, value);
        }

        [Indexed(nameof(Activo), Unique = true)]
        public string Nombre
        {
            get => nombre;
            set => SetPropertyValue(nameof(Nombre), ref nombre, value);
        }

        [Size(500)]
        public string Observacion
        {
            get => observacion;
            set => SetPropertyValue(nameof(Observacion), ref observacion, value);
        }

        [Size(30)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [XafDisplayName("Código")]
        [ToolTip("Código externo de la Escribanía")]
        public string CodigoExterno
        {
            get => _codigoExterno;
            set => SetPropertyValue(nameof(CodigoExterno), ref _codigoExterno, value);
        }

        [Association("Escribania-SolicitudSeguimiento")]
        public XPCollection<SolicitudSeguimiento> Seguimientos=> GetCollection<SolicitudSeguimiento>(nameof(Seguimientos));

        private XPCollection<AuditDataItemPersistent> auditoria;
        private string _codigoExterno;

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

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public OperandProperty Observacion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Observacion"));
                }
            }
        }
    }
}