using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using Fasterflect;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Display")]
    public class SolicitudPersona : BaseObject, IObjectSpaceLink
    {
        static FieldsClass _Fields;
        private Persona _persona;
        private Solicitud _solicitud;
        private TipoPersona _tipoPersona;
        private Parentezco _parentezco;
        private int _promedioAtraso;
        private Segmento _segmento;

        //Carga rapida de Ingresos y Egresos
        private bool crie_esegreso;
        private IngresoConcepto crie_concepto;
        private decimal crie_monto;
        private bool crie_acancelar;
        private string crie_observacion;


        public SolicitudPersona(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            CRIE_EsEgreso = false;
            _tipoPersona = Session.FindObject<TipoPersona>(TipoPersona.Fields.Codigo == "COD");
        }

/*        [PersistentAlias("Concat(Solicitud.Oid, '-', Persona.NombreCompleto, '-', TipoPersona.Descripcion)")]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public string Display => (string)EvaluateAlias(nameof(Display));*/

        [Association("SolicitudPersona-SolicitudPersonaEgreso")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<SolicitudPersonaEgreso> Egresos => GetCollection<SolicitudPersonaEgreso>(nameof(Egresos));

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

        [Association("SolicitudPersona-SolicitudPersonaIngreso")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<SolicitudPersonaIngreso> Ingresos => GetCollection<SolicitudPersonaIngreso>(nameof(Ingresos));

        [NonPersistent]
        public double MontoAhorro => TotalIngresos - TotalEgresos;

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [Association("SolicitudPersona-Persona")]
        public Persona Persona
        {
            get => _persona;
            set => SetPropertyValue(nameof(Persona), ref _persona, value);
        }

        [XafDisplayName("Días de atraso promedio")]
        [ModelDefault("AllowEdit", "False")]
        public int PromedioAtraso
        {
            get => _promedioAtraso;
            set => SetPropertyValue(nameof(PromedioAtraso), ref _promedioAtraso, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public Segmento Segmento
        {
            get => _segmento;
            set => SetPropertyValue(nameof(Segmento), ref _segmento, value);
        }

        [NonPersistent]
        public double PorcentajeAhorro
        {
            get
            {
                if(TotalIngresos > 0)
                {
                    return MontoAhorro / TotalIngresos * 100;
                }
                else
                {
                    return 0;
                }
            }
        }

        #region CargaRapidaIngresoEgreso
        [NonPersistent]
        [XafDisplayName("Es egreso")]
        [ImmediatePostData]
        public bool CRIE_EsEgreso
        {
            get => crie_esegreso;
            set => SetPropertyValue(nameof(CRIE_EsEgreso), ref crie_esegreso, value);
        }

        [NonPersistent]
        [XafDisplayName("A Cancelar")]
        [Appearance("", TargetItems = nameof(CRIE_ACancelar), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!CRIE_EsEgreso")]
        public bool CRIE_ACancelar
        {
            get => crie_acancelar;
            set => SetPropertyValue(nameof(CRIE_ACancelar), ref crie_acancelar, value);
        }

        [NonPersistent]
        [XafDisplayName("Monto")]
        public decimal CRIE_Monto
        {
            get => crie_monto;
            set => SetPropertyValue(nameof(CRIE_Monto), ref crie_monto, value);
        }

        [NonPersistent]
        [XafDisplayName("Concepto")]
        [DataSourceCriteria("EsIngreso = !'@This.CRIE_EsEgreso'")]
        public IngresoConcepto CRIE_Concepto
        {
            get => crie_concepto;
            set => SetPropertyValue(nameof(CRIE_Concepto), ref crie_concepto, value);
        }

        [NonPersistent]
        [XafDisplayName("Observacion")]
        public string CRIE_Observacion
        {
            get => crie_observacion;
            set => SetPropertyValue(nameof(CRIE_Observacion), ref crie_observacion, value);
        }
        #endregion region CargaRapidaIngresoEgreso

        [Association("SolicitudPersona-SolicitudPersonaRefCom")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<SolicitudPersonaRefCom> ReferenciasComerciales => GetCollection<SolicitudPersonaRefCom>(nameof(ReferenciasComerciales));

        [Association("SolicitudPersona-SolicitudPersonaRefPer")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<SolicitudPersonaRefPer> ReferenciasPersonales => GetCollection<SolicitudPersonaRefPer>(nameof(ReferenciasPersonales));

        [Association("SolicitudPersona-SolicitudPersonaRefLab")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<SolicitudPersonaRefLab> ReferenciasLaborales => GetCollection<SolicitudPersonaRefLab>(nameof(ReferenciasLaborales));

        [Association("Solicitud-SolicitudPersona")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public Solicitud Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public TipoPersona TipoPersona
        {
            get => _tipoPersona;
            set => SetPropertyValue(nameof(TipoPersona), ref _tipoPersona, value);
        }

        [XafDisplayName("Parentesco")]
        [Appearance("Parentesco", Enabled = false, Criteria = "TipoPersona.Codigo == 'SOL'", Context = nameof(DetailView))]
        public Parentezco Parentezco
        {
            get => _parentezco;
            set => SetPropertyValue(nameof(Parentezco), ref _parentezco, value);
        }

        [NonPersistent]
        public double TotalEgresos => Egresos.Where(e => !e.ACancelar).Sum(x => x.Monto);

        [NonPersistent]
        public double TotalIngresos => Ingresos.Sum(x => x.Monto);

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

        public static void GenerarPromedioAtraso(SolicitudPersona solicitudPersona)
        {

            if (solicitudPersona != null)
            {
                double promedioDiasAtrasados = 0;

                // int verificarPrestamos = solicitudPersona.Persona.CuotasPagadas.Where(t => t.FechaTransaccion.Year >= (DateTime.Today.Year - 3)).Count();
                List<PrestamoCabecera> prestamos = solicitudPersona.Persona.PrestamosCabecera.Where(t => t.FechaTransaccion.Year >= (DateTime.Today.Year - 3)).ToList();

                if (prestamos.Count() > 0)
                {
                    // promedioDiasAtrasados = solicitudPersona.Persona.CuotasPagadas.Where(t => t.FechaTransaccion.Year >= (DateTime.Today.Year - 3)).Average(x => x.Diferencia);

                    promedioDiasAtrasados = prestamos.Average(x => x.PromedioAtraso);
                }

                solicitudPersona.PromedioAtraso = (int)Math.Round(promedioDiasAtrasados, MidpointRounding.AwayFromZero);

                solicitudPersona.Save();
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

            public OperandProperty Display
            {
                get
                {
                    return new OperandProperty(GetNestedName("Display"));
                }
            }

            public OperandProperty Egresos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Egresos"));
                }
            }

            public OperandProperty Ingresos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Ingresos"));
                }
            }

            public Modelo.FieldsClass Modelo
            {
                get
                {
                    return new Modelo.FieldsClass(GetNestedName("Modelo"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public Persona.FieldsClass Persona
            {
                get
                {
                    return new Persona.FieldsClass(GetNestedName("Persona"));
                }
            }

            public OperandProperty ProcesadoMotor
            {
                get
                {
                    return new OperandProperty(GetNestedName("ProcesadoMotor"));
                }
            }

            public OperandProperty ProcesarMotor
            {
                get
                {
                    return new OperandProperty(GetNestedName("ProcesarMotor"));
                }
            }

            public OperandProperty ReferenciasComerciales
            {
                get
                {
                    return new OperandProperty(GetNestedName("ReferenciasComerciales"));
                }
            }

            public OperandProperty ReferenciasPersonales
            {
                get
                {
                    return new OperandProperty(GetNestedName("ReferenciasPersonales"));
                }
            }

            public OperandProperty ResultadoMotor
            {
                get
                {
                    return new OperandProperty(GetNestedName("ResultadoMotor"));
                }
            }

            public Solicitud.FieldsClass Solicitud
            {
                get
                {
                    return new Solicitud.FieldsClass(GetNestedName("Solicitud"));
                }
            }

            public TipoPersona.FieldsClass TipoPersona
            {
                get
                {
                    return new TipoPersona.FieldsClass(GetNestedName("TipoPersona"));
                }
            }
        }
    }
}

namespace PDNOriginacion.Module
{
    public enum Segmento
    {
        NoDefinido= 0,
        Excelentes = 1,
        MuyBuenos= 2,
        Buenos =3,
        Regulares = 4,
        Malos = 5,
        Error = -1
    }
}