using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Descripcion")]
    [RuleCriteria("DelProducto", DefaultContexts.Delete, "Solicitudes.Count == 0", "No se puede eliminar el PRODUCTO porque tiene SOLICITUDES asociadas!", SkipNullOrEmptyValues = true)]
    public class Producto : BaseObject, IObjectSpaceLink
    {
        decimal montoSolicitarInformconf;
        bool tieneElectrodomestico;
        bool tieneComercio;
        int diasBeneficio;
        static FieldsClass _Fields;
        private bool activo;
        private string codigo;
        private string descripcion;
        private bool pordefecto;
        private TipoProducto tipoProducto;
        private int version;
        private bool vigente;
        private PrestamoConfiguracion prestamoConfiguracion;
        private bool transicionEvalCriterios;
        private Rol rolGeneracion;
        private string codigoExterno;
        private string codigoTipoMontoExterno;
        private bool transicionEvalSeguimientos;
        private bool presupuestoGenSoloPrestamosDefault;
        private bool presupuestoAceptadoClienteAuto;
        private bool controlarLimitePrimerVencimiento;
        private bool permitirVariosPresupuestos;
        private Rol rolEjecutivo;
        private int mesesPrimerVencimiento;
        public Producto(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            activo = true;
            tipoProducto = Session.FindObject<TipoProducto>(TipoProducto.Fields.Descripcion == "CONSUMO");
        }

        [Association("Producto-Acciones")]
        [DevExpress.Xpo.Aggregated]
        [ImmediatePostData]
        public XPCollection<WFAccion> Acciones => GetCollection<WFAccion>(nameof(Acciones));

        [NonCloneable]
        public bool Activo
        {
            get => activo;
            set => SetPropertyValue(nameof(Activo), ref activo, value);
        }

        [Association("Producto-Adjuntos")]
        [DevExpress.Xpo.Aggregated]
        [ImmediatePostData]
        public XPCollection<WFAdjunto> Adjuntos => GetCollection<WFAdjunto>(nameof(Adjuntos));

        [Association("Producto-CampoProducto")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<CampoProducto> Campos => GetCollection<CampoProducto>(nameof(Campos));

        public XPCollection<Campo> CamposNoUsados
        {
            get
            {
                List<Guid> idList = new List<Guid>();
                foreach (CampoProducto xc in Campos)
                {
                    if (xc.Campo != null)
                    {
                        idList.Add(xc.Campo.Oid);
                    }
                }

                XPCollection<Campo> collection = new XPCollection<Campo>(Session)
                {
                    Criteria = new NotOperator(new InOperator(nameof(Oid), idList))
                };
                return collection;
            }
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Size(20)]
        public string Codigo
        {
            get => codigo;
            set => SetPropertyValue(nameof(Codigo), ref codigo, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        [Size(20)]
        public string CodigoExterno
        {
            get => codigoExterno;
            set => SetPropertyValue(nameof(CodigoExterno), ref codigoExterno, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [Size(20)]
        public string CodigoTipoMontoExterno
        {
            get => codigoTipoMontoExterno;
            set => SetPropertyValue(nameof(CodigoTipoMontoExterno), ref codigoTipoMontoExterno, value);
        }

        [NonCloneable]
        public bool Default
        {
            get => pordefecto;
            set => SetPropertyValue(nameof(Default), ref pordefecto, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [RuleUniqueValue("", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction)]
        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }

        public PrestamoConfiguracion PrestamoConfiguracion
        {
            get => prestamoConfiguracion;
            set => SetPropertyValue(nameof(PrestamoConfiguracion), ref prestamoConfiguracion, value);
        }

        [XafDisplayName("Rol para asignar tareas al generar solicitud")]
        public Rol RolGeneracion
        {
            get => rolGeneracion;
            set => SetPropertyValue(nameof(RolGeneracion), ref rolGeneracion, value);
        }

        [XafDisplayName("Rol de los Ejecutivos")]
        public Rol RolEjecutivo
        {
            get => rolEjecutivo;
            set => SetPropertyValue(nameof(RolEjecutivo), ref rolEjecutivo, value);
        }

        [XafDisplayName("Evaluar criterios para mostrar Transición.")]
        public bool TransicionEvalCriterios
        {
            get => transicionEvalCriterios;
            set => SetPropertyValue(nameof(TransicionEvalCriterios), ref transicionEvalCriterios, value);
        }

        [XafDisplayName("Evaluar que exista al menos un seguimiento a fecha futura para el solicitante")]
        public bool TransicionEvalSeguimientos
        {
            get => transicionEvalSeguimientos;
            set => SetPropertyValue(nameof(TransicionEvalSeguimientos), ref transicionEvalSeguimientos, value);
        }

        [XafDisplayName("El presupuesto genera sólo préstamos default")]
        public bool PresupuestoGenSoloPrestamosDefault
        {
            get => presupuestoGenSoloPrestamosDefault;
            set => SetPropertyValue(nameof(PresupuestoGenSoloPrestamosDefault), ref presupuestoGenSoloPrestamosDefault, value);
        }

        [XafDisplayName("Marcar presupuesto como Aceptado por el cliente automáticamente")]
        public bool PresupuestoAceptadoClienteAuto
        {
            get => presupuestoAceptadoClienteAuto;
            set => SetPropertyValue(nameof(PresupuestoAceptadoClienteAuto), ref presupuestoAceptadoClienteAuto, value);
        }

        public int DiasBeneficio
        {
            get => diasBeneficio;
            set => SetPropertyValue(nameof(DiasBeneficio), ref diasBeneficio, value);
        }


        [XafDisplayName("Controlar el límite de días para el primer vencimiento")]
        public bool ControlarLimitePrimerVencimiento
        {
            get => controlarLimitePrimerVencimiento;
            set => SetPropertyValue(nameof(ControlarLimitePrimerVencimiento), ref controlarLimitePrimerVencimiento, value);
        }

        [XafDisplayName("Comercio")]
        public bool TieneComercio
        {
            get => tieneComercio;
            set => SetPropertyValue(nameof(TieneComercio), ref tieneComercio, value);
        }

        [XafDisplayName("Electrodoméstico")]
        public bool TieneElectrodomestico
        {
            get => tieneElectrodomestico;
            set => SetPropertyValue(nameof(TieneElectrodomestico), ref tieneElectrodomestico, value);
        }

        [XafDisplayName("Permitir generación de varios Presupuestos")]
        public bool PermitirVariosPresupuestos
        {
            get => permitirVariosPresupuestos;
            set => SetPropertyValue(nameof(PermitirVariosPresupuestos), ref permitirVariosPresupuestos, value);
        }

        public decimal MontoSolicitarInformconf
        {
            get => montoSolicitarInformconf;
            set => SetPropertyValue(nameof(MontoSolicitarInformconf), ref montoSolicitarInformconf, value);
        }

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public XPCollection<EstadoSolicitud> EstadosUsados
        {
            get
            {
                List<Guid> idList = new List<Guid>();

                foreach(WFTransicion xc in Transiciones)
                {
                    idList.Add(xc.EstadoDestino.Oid);
                }

                XPCollection<EstadoSolicitud> collection = new XPCollection<EstadoSolicitud>(Session)
                {
                    Criteria = new InOperator(nameof(Oid), idList)
                };

                return collection;
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

        [Association("Producto-GastoAdministrativo")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<GastoAdministrativo> GastosAdministrativos => GetCollection<GastoAdministrativo>(nameof(GastosAdministrativos));

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace
        {
            get;
            set;
        }

        [Association("Producto-Personas")]
        [DevExpress.Xpo.Aggregated]
        [ImmediatePostData]
        //[Appearance("VTransiciones", TargetItems = "Transiciones", AppearanceItemType = "ViewItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "!CampoEnProducto('Cheques', Producto)")]
        public XPCollection<WFTipoPersona> Personas => GetCollection<WFTipoPersona>(nameof(Personas));

        [Association("Solicitud-Producto")]
        public XPCollection<Solicitud> Solicitudes => GetCollection<Solicitud>(nameof(Solicitudes));

        [Association("Producto-Tareas")]
        [DevExpress.Xpo.Aggregated]
        [ImmediatePostData]
        //[Appearance("VTransiciones", TargetItems = "Transiciones", AppearanceItemType = "ViewItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "!CampoEnProducto('Cheques', Producto)")]
        public XPCollection<WFTarea> Tareas => GetCollection<WFTarea>(nameof(Tareas));

        [Association("Producto-GenSolicitudCriterios")]
        [DevExpress.Xpo.Aggregated]
        [ImmediatePostData]
        //[Appearance("VTransiciones", TargetItems = "Transiciones", AppearanceItemType = "ViewItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "!CampoEnProducto('Cheques', Producto)")]
        public XPCollection<GenSolicitudCriterio> GeneracionSolicitud => GetCollection<GenSolicitudCriterio>(nameof(GeneracionSolicitud));

        [Association("TipoProductos-Producto")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public TipoProducto TipoProducto
        {
            get => tipoProducto;
            set => SetPropertyValue(nameof(TipoProducto), ref tipoProducto, value);
        }

        [Association("Producto-Transiciones")]
        [DevExpress.Xpo.Aggregated]
        [ImmediatePostData]
        //[Appearance("VTransiciones", TargetItems = "Transiciones", AppearanceItemType = "ViewItem", Context = "DetailView", Visibility = ViewItemVisibility.Hide, Criteria = "!CampoEnProducto('Cheques', Producto)")]
        public XPCollection<WFTransicion> Transiciones => GetCollection<WFTransicion>(nameof(Transiciones));

        [NonCloneable]
        public int Version
        {
            get => version;
            set => SetPropertyValue(nameof(Version), ref version, value);
        }

        [NonCloneable]
        public bool Vigente
        {
            get => vigente;
            set => SetPropertyValue(nameof(Vigente), ref vigente, value);
        }

        [XafDisplayName("Cantidad de meses por defecto para el primer vencimiento")]
        public int MesesPrimerVencimiento
        {
            get => mesesPrimerVencimiento;
            set => SetPropertyValue(nameof(MesesPrimerVencimiento), ref mesesPrimerVencimiento, value);
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

            public OperandProperty Acciones
            {
                get
                {
                    return new OperandProperty(GetNestedName("Acciones"));
                }
            }

            public OperandProperty Activo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Activo"));
                }
            }

            public OperandProperty Adjuntos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Adjuntos"));
                }
            }

            public OperandProperty Campos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Campos"));
                }
            }

            public OperandProperty CamposNoUsados
            {
                get
                {
                    return new OperandProperty(GetNestedName("CamposNoUsados"));
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

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public PrestamoConfiguracion.FieldsClass PrestamoConfiguracion
            {
                get
                {
                    return new PrestamoConfiguracion.FieldsClass(GetNestedName("PrestamoConfiguracion"));
                }
            }

            public OperandProperty EstadosUsados
            {
                get
                {
                    return new OperandProperty(GetNestedName("EstadosUsados"));
                }
            }

            public OperandProperty GastosAdministrativos
            {
                get
                {
                    return new OperandProperty(GetNestedName("GastosAdministrativos"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public OperandProperty Personas
            {
                get
                {
                    return new OperandProperty(GetNestedName("Personas"));
                }
            }

            public OperandProperty Solicitudes
            {
                get
                {
                    return new OperandProperty(GetNestedName("Solicitudes"));
                }
            }

            public OperandProperty Tareas
            {
                get
                {
                    return new OperandProperty(GetNestedName("Tareas"));
                }
            }

            public OperandProperty GeneracionSolicitud
            {
                get
                {
                    return new OperandProperty(GetNestedName("GeneracionSolicitud"));
                }
            }

            public TipoProducto.FieldsClass TipoProducto
            {
                get
                {
                    return new TipoProducto.FieldsClass(GetNestedName("TipoProducto"));
                }
            }

            public OperandProperty Transiciones
            {
                get
                {
                    return new OperandProperty(GetNestedName("Transiciones"));
                }
            }

            public OperandProperty Version
            {
                get
                {
                    return new OperandProperty(GetNestedName("Version"));
                }
            }

            public OperandProperty Vigente
            {
                get
                {
                    return new OperandProperty(GetNestedName("Vigente"));
                }
            }

            public OperandProperty DiasBeneficio
            {
                get
                {
                    return new OperandProperty(GetNestedName("DiasBeneficio"));
                }
            }
        }
    }
}

