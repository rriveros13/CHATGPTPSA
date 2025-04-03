using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Web.Editors.ASPx;
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
    [DefaultProperty("Descripcion")]
    public class InmuebleReferencia : BaseObject
    {
        static FieldsClass _Fields;
        private string _descripcion;
        private Inmueble _inmueble;
        private decimal _superficieConstruidaM2;
        private decimal _superficieM2;
        private string _telefono;
        private decimal _totalConstruccion;
        private decimal _totalInmueble;
        private decimal _totalTerreno;
        private decimal _valorM2Construidos;
        private decimal _valorM2Terreno;
        private TipoInmuebleReferencia _tipo;
        private string _observacion;
        Direccion direccion;
        private string _linkReferencia;

        public InmuebleReferencia(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [Size(100)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        public Direccion Direccion
        {
            get => direccion;
            set => SetPropertyValue(nameof(Direccion), ref direccion, value);
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

        [Association("Inmueble-Referencias")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public Inmueble Inmueble
        {
            get => _inmueble;
            set => SetPropertyValue(nameof(Inmueble), ref _inmueble, value);
        }

        public string Telefono
        {
            get => _telefono;
            set => SetPropertyValue(nameof(Telefono), ref _telefono, value);
        }

        [ImmediatePostData]
        [XafDisplayName("Superficie del terreno en M2")]
        public decimal SuperficieM2
        {
            get => _superficieM2;
            set
            {
                SetPropertyValue(nameof(SuperficieM2), ref _superficieM2, value);
                if (!IsLoading && !IsSaving)
                {
                    TotalTerreno = ValorM2Construidos * value;
                }
            }
        }

        [XafDisplayName("Valor del terreno por M2")]
        public decimal ValorM2Terreno
        {
            get => _valorM2Terreno;
            set
            {
                SetPropertyValue(nameof(ValorM2Terreno), ref _valorM2Terreno, value);
            }
        }

        [ImmediatePostData]
        [XafDisplayName("Valor total del terreno")]
        public decimal TotalTerreno
        {
            get => _totalTerreno;
            set
            {
                SetPropertyValue(nameof(TotalTerreno), ref _totalTerreno, value);
                if (!IsLoading && !IsSaving)
                {
                    if (SuperficieM2 != 0)
                        ValorM2Terreno = TotalTerreno / SuperficieM2;

                    TotalInmueble = value + TotalConstruccion;
                }
            }
        }

        [ImmediatePostData]
        [XafDisplayName("Superficie construida en M2")]
        public decimal SuperficieConstruidaM2
        {
            get => _superficieConstruidaM2;
            set
            {
                SetPropertyValue(nameof(SuperficieConstruidaM2), ref _superficieConstruidaM2, value);
                if (!IsLoading && !IsSaving)
                {
                    TotalConstruccion = ValorM2Construidos * value;
                }
            }
        }
        
        [XafDisplayName("Valor por M2 construido")]
        public decimal ValorM2Construidos
        {
            get => _valorM2Construidos;
            set
            {
                SetPropertyValue(nameof(ValorM2Construidos), ref _valorM2Construidos, value);
            }
        }

        [ImmediatePostData]
        [XafDisplayName("Valor total de la construcción")]
        public decimal TotalConstruccion
        {
            get => _totalConstruccion;
            set
            {
                SetPropertyValue(nameof(TotalConstruccion), ref _totalConstruccion, value);
                if(!IsLoading && !IsSaving)
                {
                    if (SuperficieConstruidaM2 != 0)
                        ValorM2Construidos = TotalConstruccion / SuperficieConstruidaM2;
                    TotalInmueble = value + TotalTerreno;
                }
            }
        }

        [XafDisplayName("Valor total de la propiedad")]
        public decimal TotalInmueble
        {
            get => _totalInmueble;
            set => SetPropertyValue(nameof(TotalInmueble), ref _totalInmueble, value);
        }

        [XafDisplayName("Tipo de Referencia")]
        public TipoInmuebleReferencia Tipo
        {
            get => _tipo;
            set => SetPropertyValue(nameof(Tipo), ref _tipo, value);
        }

        [XafDisplayName("Link de Referencia")]
        [EditorAlias("HyperLinkStringPropertyEditor")]
        [Size(250)]
        public string LinkReferencia
        {
            get => _linkReferencia;
            set => SetPropertyValue(nameof(LinkReferencia), ref _linkReferencia, value);
        }


        [XafDisplayName("Observación")]
        [Size(500)]
        public string Observacion
        {
            get => _observacion;
            set => SetPropertyValue(nameof(Observacion), ref _observacion, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public Direccion.FieldsClass Direccion
            {
                get
                {
                    return new Direccion.FieldsClass(GetNestedName("Direccion"));
                }
            }

            public Inmueble.FieldsClass Inmueble
            {
                get
                {
                    return new Inmueble.FieldsClass(GetNestedName("Inmueble"));
                }
            }

            public OperandProperty SuperficieConstruidaM2
            {
                get
                {
                    return new OperandProperty(GetNestedName("SuperficieConstruidaM2"));
                }
            }

            public OperandProperty SuperficieM2
            {
                get
                {
                    return new OperandProperty(GetNestedName("SuperficieM2"));
                }
            }

            public OperandProperty Telefono
            {
                get
                {
                    return new OperandProperty(GetNestedName("Telefono"));
                }
            }

            public OperandProperty TotalConstruccion
            {
                get
                {
                    return new OperandProperty(GetNestedName("TotalConstruccion"));
                }
            }

            public OperandProperty TotalInmueble
            {
                get
                {
                    return new OperandProperty(GetNestedName("TotalInmueble"));
                }
            }

            public OperandProperty TotalTerreno
            {
                get
                {
                    return new OperandProperty(GetNestedName("TotalTerreno"));
                }
            }

            public OperandProperty ValorM2Construidos
            {
                get
                {
                    return new OperandProperty(GetNestedName("ValorM2Construidos"));
                }
            }

            public OperandProperty ValorM2Terreno
            {
                get
                {
                    return new OperandProperty(GetNestedName("ValorM2Terreno"));
                }
            }
        }
    }
}