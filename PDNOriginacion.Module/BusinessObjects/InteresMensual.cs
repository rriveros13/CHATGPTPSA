using System;
using System.Linq;
using System.Text;
using DevExpress.Xpo;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using System.Collections.Generic;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Descripcion")]
    public class InteresMensual : BaseObject
    {
        public InteresMensual(Session session)
            : base(session)
        {
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
        }

        static FieldsClass _Fields;
        int cuotaReferencia;
        decimal rangoMaximo;
        decimal rangoMinimo;
        string descripcion;
        decimal porcentajeInteres;

        //[RuleRequiredField(DefaultContexts.Save)]
        public decimal PorcentajeInteres
        {
            get => porcentajeInteres;
            set => SetPropertyValue(nameof(PorcentajeInteres), ref porcentajeInteres, value);
        }

        [Size(100)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public decimal RangoMinimo
        {
            get => rangoMinimo;
            set => SetPropertyValue(nameof(RangoMinimo), ref rangoMinimo, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public decimal RangoMaximo
        {
            get => rangoMaximo;
            set => SetPropertyValue(nameof(RangoMaximo), ref rangoMaximo, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public int CuotaReferencia
        {
            get => cuotaReferencia;
            set => SetPropertyValue(nameof(CuotaReferencia), ref cuotaReferencia, value);
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

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty CuotaReferencia
            {
                get
                {
                    return new OperandProperty(GetNestedName("CuotaReferencia"));
                }
            }

            public OperandProperty RangoMaximo
            {
                get
                {
                    return new OperandProperty(GetNestedName("RangoMaximo"));
                }
            }

            public OperandProperty RangoMinimo
            {
                get
                {
                    return new OperandProperty(GetNestedName("RangoMinimo"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public OperandProperty PorcentajeInteres
            {
                get
                {
                    return new OperandProperty(GetNestedName("PorcentajeInteres"));
                }
            }
        }
    }
}