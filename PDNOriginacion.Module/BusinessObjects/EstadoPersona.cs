using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Descripcion")]
    [DeferredDeletion(Enabled = false)]
    public class EstadoPersona : BaseObject
    {
        string observacion;
        static FieldsClass _Fields;
        bool activo;
        bool @default;
        string descripcion;

        public EstadoPersona(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
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
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        [Size(500)]
        public string Observacion
        {
            get => observacion;
            set => SetPropertyValue(nameof(Observacion), ref observacion, value);
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