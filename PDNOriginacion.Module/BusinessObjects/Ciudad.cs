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
    public class Ciudad : BaseObject
    {
        bool @default;
        static FieldsClass _Fields;
        private bool activo;
        private int _codigo;
        private int _codigo2;
        private Departamento _departamento;
        private string _nombre;
        private decimal _impuestoMunicipal;

        public Ciudad(Session session) : base(session)
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

        [Association("Ciudad-Barrio")]
        [Aggregated]
        public XPCollection<Barrio> Barrios => GetCollection<Barrio>(nameof(Barrios));

        //[RuleRequiredField(DefaultContexts.Save)]
        public int Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public int Codigo2
        {
            get => _codigo2;
            set => SetPropertyValue(nameof(Codigo2), ref _codigo2, value);
        }


        public bool Default
        {
            get => @default;
            set => SetPropertyValue(nameof(Default), ref @default, value);
        }
        
        [Association("Departamento-Ciudad")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public Departamento Departamento
        {
            get => _departamento;
            set => SetPropertyValue(nameof(Departamento), ref _departamento, value);
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

        public decimal ImpuestoMunicipal
        {
            get => _impuestoMunicipal;
            set => SetPropertyValue(nameof(ImpuestoMunicipal), ref _impuestoMunicipal, value);
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

            public OperandProperty Activo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Activo"));
                }
            }

            public OperandProperty Barrios
            {
                get
                {
                    return new OperandProperty(GetNestedName("Barrios"));
                }
            }

            public OperandProperty Codigo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Codigo"));
                }
            }

            public OperandProperty Default
            {
                get
                {
                    return new OperandProperty(GetNestedName("Default"));
                }
            }

            public Departamento.FieldsClass Departamento
            {
                get
                {
                    return new Departamento.FieldsClass(GetNestedName("Departamento"));
                }
            }

            public OperandProperty Nombre
            {
                get
                {
                    return new OperandProperty(GetNestedName("Nombre"));
                }
            }

            public OperandProperty ImpuestoMunicipal
            {
                get
                {
                    return new OperandProperty(GetNestedName("ImpuestoMunicipal"));
                }
            }
        }
    }
}