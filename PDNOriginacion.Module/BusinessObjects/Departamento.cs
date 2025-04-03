using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Nombre")]
    public class Departamento : BaseObject
    {
        bool @default;
        static FieldsClass _Fields;
        private bool activo;
        private int _codigo;
        private string _nombre;
        private Pais _pais;

        public Departamento(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            activo = true;
            @default = false;
        }

        //[RuleRequiredField(DefaultContexts.Save)]
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

        [Association("Pais-Departamentos")]
        public Pais Pais
        {
            get => _pais;
            set => SetPropertyValue(nameof(Pais), ref _pais, value);
        }


        [Association("Departamento-Ciudad")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<Ciudad> Ciudades => GetCollection<Ciudad>(nameof(Ciudades));

        //[RuleRequiredField(DefaultContexts.Save)]
        public int Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
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

        //[RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName(nameof(Nombre))]
        [ToolTip("Nombre del Departamento")]
        public string Nombre
        {
            get => _nombre;
            set => SetPropertyValue(nameof(Nombre), ref _nombre, value);
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

            public OperandProperty Ciudades
            {
                get
                {
                    return new OperandProperty(GetNestedName("Ciudades"));
                }
            }

            public OperandProperty Codigo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Codigo"));
                }
            }

            public OperandProperty Nombre
            {
                get
                {
                    return new OperandProperty(GetNestedName("Nombre"));
                }
            }
        }
    }
}
