using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Utils.Extensions;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using PDNOriginacion.Module.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [ImageName("BO_Contact")]
    [SearchClassOptions(SearchMemberMode.Exclude)]
    [DefaultProperty("Oid")]
    public class Solicitud : XPObject, IObjectSpaceLink
    {
        bool neto;
        Planes electrodomestico;
        Empresa comercio;
        decimal tasaAnual;
        string faja;
        static FieldsClass _Fields;
        // private bool aprobada;
        //private bool aProcesar;
        private Usuario creadaPor;
        private string documento;
        private DateTime fecha;
        private MedioIngreso medioIngreso;
        private MonedaEnum moneda;
        private decimal monto;
        private MotivoSolicitud motivo;
        private int plazo;
        // private int edad;
        private Producto producto;
        private TipoDocumento tipoDocumento;
        private MotivoRechazo motivoRechazo;
        private MotivoDescarte motivoDescarte;
        private bool enviadoBack;
        private DateTime fechaEnvioBack;
        private DateTime fechaPrimerVencimiento;
        private DateTime fechaADesembolsar;
        private Solicitud solicitudOriginal;
        private Solicitud solicitudCodeudor;
        private string nombreTitular;
        private Usuario ejecutivoAsignado;
        private decimal montoPresupuesto;
        private decimal montoPropuesta;
        private string observacionRechDesc;
        private DateTime fechaVisita;
        private String telefonoTitular;
        private string observacion;


        public Solicitud(Session session) : base(session)
        {
        }

        protected override void OnSaving()
        {
            if (this.Titular != null)
            {
                this.nombreTitular = this.Titular.NombreCompleto;
                this.Documento = this.DocumentoTitular;

                if (this.Titular.TelefonoPreferido != null)
                    this.telefonoTitular = Titular.TelefonoPreferido.TelefonoCompleto;
            }

            base.OnSaving();
            string errorMesagge = string.Empty;
            if (Producto != null)
            {
                foreach (CampoProducto c in Producto.Campos)
                {
                    string criterio = c?.CriterioVal;

                    if (string.IsNullOrEmpty(criterio))
                    {
                        continue;
                    }

                    if ((bool)Evaluate(c.CriterioVal))
                    {
                        continue;
                    }

                    if (string.IsNullOrEmpty(c.MensajeError))
                    {
                        errorMesagge = errorMesagge + Environment.NewLine + c.Campo.Nombre + " : " + c.CriterioVal;
                    }
                    else
                    {
                        errorMesagge = errorMesagge + Environment.NewLine + c.MensajeError;
                    }
                }
                if (Producto.Codigo == "CREDIFACIL_4")
                {
                    neto = true;
                }
            }
            if (errorMesagge != string.Empty)
            {
                throw new UserFriendlyException(errorMesagge);
            }
            if (Estado == null)
            {
                if (this.Titular == null)
                    WFHelper.GetSolicitante(this);

                errorMesagge = (WFHelper.SolicitarCambioEstado(this))?.Mensaje;
                if (errorMesagge != string.Empty)
                {
                    throw new UserFriendlyException(errorMesagge);
                }
            }
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            creadaPor = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
            fecha = DateTime.Now;
            moneda = MonedaEnum.GS;
            producto = Session.FindObject<Producto>(CriteriaOperator.Parse("Default=true"));
            neto = false;
        }

        [Association("Solicitud-Adjuntos")]
        [DevExpress.Xpo.Aggregated]
        [Appearance("V2Adjuntos", TargetItems = nameof(Adjuntos), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Adjuntos', this)")]
        [Appearance("AAdjuntos", Enabled = false, Criteria = "!CampoEditable('Adjuntos', this)", Context = nameof(DetailView))]
        public XPCollection<Adjunto> Adjuntos => GetCollection<Adjunto>(nameof(Adjuntos));

        /*[RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false, TargetCriteria = "CampoObligatorio('Aprobada', Producto)")]
        [Appearance("V2Aprobada", TargetItems = nameof(Aprobada), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Aprobada', this)")]
        [ModelDefault("AllowEdit", "false")]
        public bool Aprobada
        {
            get => aprobada;
            set => SetPropertyValue(nameof(Aprobada), ref aprobada, value);
        }  */

        /* [Appearance("", TargetItems = nameof(AProcesar), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide)]
         public bool AProcesar
         {
             get => aProcesar;
             set => SetPropertyValue(nameof(AProcesar), ref aProcesar, value);
         } */

        /*[Association("Solicitud-Cheque")]
        [DevExpress.Xpo.Aggregated]
        [NonCloneable]
        [ImmediatePostData]
        //[Appearance("V2Cheques", TargetItems = nameof(Cheques), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Cheques', this)")]
        [Appearance("", TargetItems = nameof(Cheques), AppearanceItemType = nameof(ViewItem), Visibility = ViewItemVisibility.Hide)]
        [Appearance("ACheques", Enabled = false, Criteria = "!CampoEditable('Cheques', this)", Context = nameof(DetailView))]
        public XPCollection<Cheque> Cheques => GetCollection<Cheque>(nameof(Cheques));  */

        /*[Appearance("VChequesAprobados", TargetItems = nameof(ChequesAprobados), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!CampoEnProducto('ChequesAprobados', Producto)")]
        [PersistentAlias("Cheques[Rechazado=false].Count")]
        public int ChequesAprobados => (int)EvaluateAlias(nameof(ChequesAprobados));  */

        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false, TargetCriteria = "CampoObligatorio('SolicitudOriginal', Producto)")]
        [Appearance("V2SolicitudOriginal", TargetItems = nameof(SolicitudOriginal), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('SolicitudOriginal', this)")]
        [ModelDefault("AllowEdit", "false")]
        [Association("Solicitud-SolicitudOriginal")]
        public Solicitud SolicitudOriginal
        {
            get => solicitudOriginal;
            set => SetPropertyValue(nameof(SolicitudOriginal), ref solicitudOriginal, value);
        }

        [Association("Solicitud-SolicitudOriginal")]
        [Appearance("", TargetItems = nameof(SolicitudesAsociadas), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('SolicitudesAsociadas', this)")]
        public XPCollection<Solicitud> SolicitudesAsociadas => GetCollection<Solicitud>(nameof(SolicitudesAsociadas));

        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false, TargetCriteria = "CampoObligatorio('SolicitudCodeudor', Producto)")]
        [Appearance("V2SolicitudCodeudor", TargetItems = nameof(SolicitudCodeudor), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('SolicitudCodeudor', this)")]
        [Appearance("VSolicitudCodeudor", TargetItems = nameof(SolicitudCodeudor), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "SolicitudCodeudor = null")]
        [ModelDefault("AllowEdit", "false")]
        public Solicitud SolicitudCodeudor
        {
            get => solicitudCodeudor;
            set => SetPropertyValue(nameof(SolicitudCodeudor), ref solicitudCodeudor, value);
        }

        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false, TargetCriteria = "CampoObligatorio('CreadaPor', Producto)")]
        [Appearance("V2CreadaPor", TargetItems = nameof(CreadaPor), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('CreadaPor', this)")]
        [ModelDefault("AllowEdit", "false")]
        public Usuario CreadaPor
        {
            get => creadaPor;
            set => SetPropertyValue(nameof(CreadaPor), ref creadaPor, value);
        }

        [Appearance("", TargetItems = nameof(EjecutivoAsignado), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('EjecutivoAsignado', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('EjecutivoAsignado', this)", Context = nameof(DetailView))]
        public Usuario EjecutivoAsignado
        {
            get => ejecutivoAsignado;
            set => SetPropertyValue(nameof(ejecutivoAsignado), ref ejecutivoAsignado, value);
        }


        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false, TargetCriteria = "CampoObligatorio('Documento', Producto)")]
        [SearchMemberOptions(SearchMemberMode.Include)]
        [Appearance("V2Documento", TargetItems = nameof(Documento), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Documento', this)")]
        [Appearance("ADocumento", Enabled = false, Criteria = "!CampoEditable('Documento', this)", Context = nameof(DetailView))]
        public string Documento
        {
            get => documento;
            set => SetPropertyValue(nameof(Documento), ref documento, value);
        }

        [SearchMemberOptions(SearchMemberMode.Include)]
        [Appearance("", TargetItems = nameof(NombreTitular), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('NombreTitular', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('NombreTitular', this)", Context = nameof(DetailView))]
        public string NombreTitular
        {
            get
            {
                return nombreTitular;
            }
            set
            {
                SetPropertyValue(nameof(NombreTitular), ref nombreTitular, value);
            }
        }

        [SearchMemberOptions(SearchMemberMode.Include)]
        [Appearance("", TargetItems = nameof(TelefonoTitular), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('TelefonoTitular', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('TelefonoTitular', this)", Context = nameof(DetailView))]
        public String TelefonoTitular
        {
            get
            {
                return telefonoTitular;
            }
            set
            {
                SetPropertyValue(nameof(TelefonoTitular), ref telefonoTitular, value);
            }
        }

        [PersistentAlias("Iif(!IsNull(EstadoActual), EstadoActual.Estado, null)")]
        [VisibleInDashboards(false), VisibleInReports(false)]
        public EstadoSolicitud Estado => EvaluateAlias(nameof(Estado)) as EstadoSolicitud;

        [Appearance("", TargetItems = nameof(InmuebleSolicitud), AppearanceItemType = nameof(ViewItem), Visibility = ViewItemVisibility.Hide)]
        [PersistentAlias("Iif(!IsNull(EstadoActual), EstadoActual.Estado.Descripcion, null)")]
        public string EstadoSolicitud => EvaluateAlias(nameof(EstadoSolicitud)) as string;

        [PersistentAlias("Iif(Estados[Fecha=^.FechaEstadoActual].Count() == 1,Estados[Fecha=^.FechaEstadoActual].Single(), null)")]
        [Browsable(false)]
        [Appearance("", TargetItems = nameof(InmuebleSolicitud), AppearanceItemType = nameof(ViewItem), Visibility = ViewItemVisibility.Hide)]
        public RegistroEstadoSolicitud EstadoActual => EvaluateAlias(nameof(EstadoActual)) as RegistroEstadoSolicitud;

        [Appearance("V2Estados", TargetItems = nameof(Estados), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Estados', this)")]
        [Association("Solicitud-RegistroEstadoSolicitud")]
        [DevExpress.Xpo.Aggregated]
        [ModelDefault("AllowEdit", "false")]
        public XPCollection<RegistroEstadoSolicitud> Estados => GetCollection<RegistroEstadoSolicitud>(nameof(Estados));

        //[RuleRequiredField(DefaultContexts.Save)]
        [NonCloneable]
        [ModelDefault("AllowEdit", "false")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
        }

        [PersistentAlias("GetDate(Fecha)")]
        [VisibleInDetailView(false)]
        [XafDisplayName("Fecha (día)")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateTime FechaDia => (DateTime)EvaluateAlias(nameof(FechaDia));


        [NonCloneable]
        [ModelDefault("AllowEdit", "false")]
        [Appearance("", TargetItems = nameof(FechaEnvioBack), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('FechaEnvioBack', this)")]
        public DateTime FechaEnvioBack
        {
            get => fechaEnvioBack;
            set => SetPropertyValue(nameof(FechaEnvioBack), ref fechaEnvioBack, value);
        }

        [NonCloneable]
        [ModelDefault("AllowEdit", "false")]
        [Appearance("", TargetItems = nameof(FechaPrimerVencimiento), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('FechaPrimerVencimiento', this)")]
        public DateTime FechaPrimerVencimiento
        {
            get => fechaPrimerVencimiento;
            set => SetPropertyValue(nameof(FechaPrimerVencimiento), ref fechaPrimerVencimiento, value);
        }

        [NonCloneable]
        [ModelDefault("AllowEdit", "false")]
        [Appearance("", TargetItems = nameof(FechaADesembolsar), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('FechaADesembolsar', this)")]
        public DateTime FechaADesembolsar
        {
            get => fechaADesembolsar;
            set => SetPropertyValue(nameof(FechaADesembolsar), ref fechaADesembolsar, value);
        }

        [Appearance("VFechaEstadoActual", TargetItems = nameof(FechaEstadoActual), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('FechaEstadoActual', this)")]
        [PersistentAlias("Iif([Estados].Count() > 0 ,[Estados].Max(Fecha), null)")]
        public DateTime? FechaEstadoActual => EvaluateAlias(nameof(FechaEstadoActual)) as DateTime?;

        [Appearance("Comercio", TargetItems = nameof(Comercio), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!Producto.TieneComercio")]
        // [Appearance("C", Enabled = false, Criteria = "Estado.Codigo != NULL", Context = nameof(DetailView))]
        [Appearance("C", Enabled = false, Criteria = "Estado.Codigo == 'APROBADA' || Estado.Codigo == 'DESEMBOLSO' || Estado.Codigo == 'PENDIENTE APROBACION' || Estado.Codigo == 'PENDIENTE PROCESO' || Estado.Codigo == 'PROCESAR' || Estado.Codigo == 'DESEMBOLSADO'", Context = nameof(DetailView))]
        public Empresa Comercio
        {
            get => comercio;
            set => SetPropertyValue(nameof(Comercio), ref comercio, value);
        }

        [Appearance("Electrodomestico", TargetItems = nameof(Electrodomestico), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!Producto.TieneElectrodomestico")]
        // [Appearance("E", Enabled = false, Criteria = "Estado.Codigo != NULL", Context = nameof(DetailView))]
        [Appearance("E", Enabled = false, Criteria = "Estado.Codigo == 'APROBADA' || Estado.Codigo == 'DESEMBOLSO' || Estado.Codigo == 'PENDIENTE APROBACION' || Estado.Codigo == 'PENDIENTE PROCESO' || Estado.Codigo == 'PROCESAR' || Estado.Codigo == 'DESEMBOLSADO'", Context = nameof(DetailView))]
        public Planes Electrodomestico
        {
            get => electrodomestico;
            set => SetPropertyValue(nameof(Electrodomestico), ref electrodomestico, value);
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

        [Association("Inmuebles-Solicitudes")]
        [ImmediatePostData]
        [Appearance("", TargetItems = nameof(Inmuebles), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Inmuebles', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('Inmuebles', this)", Context = nameof(DetailView))]
        public XPCollection<Inmueble> Inmuebles => GetCollection<Inmueble>(nameof(Inmuebles));

        [ToolTip("Medio de Ingreso")]
        [XafDisplayName("Medio de Ingreso")]
        [Appearance("", TargetItems = nameof(MedioIngreso), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('MedioIngreso', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('MedioIngreso', this)", Context = nameof(DetailView))]
        public MedioIngreso MedioIngreso
        {
            get => medioIngreso;
            set => SetPropertyValue(nameof(MedioIngreso), ref medioIngreso, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        //[Appearance("V2Moneda", TargetItems = nameof(Moneda), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Moneda', this)")]
        [Appearance("", TargetItems = nameof(InmuebleSolicitud), AppearanceItemType = nameof(ViewItem), Visibility = ViewItemVisibility.Hide)]
        [Appearance("AMoneda", Enabled = false, Criteria = "!CampoEditable('Moneda', this)", Context = nameof(DetailView))]
        public MonedaEnum Moneda
        {
            get => moneda;
            set => SetPropertyValue(nameof(Moneda), ref moneda, value);
        }

        [RuleValueComparison("RMonto", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "CampoObligatorio('Monto', Producto)", SkipNullOrEmptyValues = false, CustomMessageTemplate = "Monto debe ser mayor que cero")]
        [Appearance("V2Monto", TargetItems = nameof(Monto), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Monto', this)")]
        [Appearance("AMonto", Enabled = false, Criteria = "CampoEditable('Monto', this)", Context = nameof(DetailView))]
        [XafDisplayName("Monto solicitado por el cliente")]
        public decimal Monto
        {
            get => monto;
            set => SetPropertyValue(nameof(Monto), ref monto, value);
        }

        [Appearance("V2MontoPres", TargetItems = nameof(Monto), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('MontoPresupuesto', this)")]
        [Appearance("AMontoPres", Enabled = false, Criteria = "!CampoEditable('MontoPresupuesto', this)", Context = nameof(DetailView))]
        public decimal MontoPresupuesto
        {
            get => montoPresupuesto;
            set => SetPropertyValue(nameof(MontoPresupuesto), ref montoPresupuesto, value);
        }

        [Appearance("", TargetItems = nameof(MontoPropuesta), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('MontoPropuesta', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('MontoPropuesta', this)", Context = nameof(DetailView))]
        public decimal MontoPropuesta
        {
            get => montoPropuesta;
            set => SetPropertyValue(nameof(MontoPropuesta), ref montoPropuesta, value);
        }

        /*[Appearance("V2MontoChequesAprobados", TargetItems = nameof(MontoChequesAprobados), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('MontoChequesAprobados', this)")]
        [PersistentAlias("iIF(ChequesAprobados > 0, Cheques[Rechazado=false].Sum(Monto), ToDecimal(0))")]
        public decimal MontoChequesAprobados => (decimal)EvaluateAlias(nameof(MontoChequesAprobados)); */

        [ToolTip("Motivo de la solicitud de crédito")]
        [XafDisplayName(nameof(Motivo))]
        [Appearance("", TargetItems = nameof(Motivo), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Motivo', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('Motivo', this)", Context = nameof(DetailView))]
        public MotivoSolicitud Motivo
        {
            get => motivo;
            set => SetPropertyValue(nameof(Motivo), ref motivo, value);
        }

        [ToolTip("Motivo de Rechazo")]
        [XafDisplayName("Motivo de Rechazo")]
        [Appearance("", TargetItems = nameof(MotivoRechazo), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('MotivoRechazo', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('MotivoRechazo', this)", Context = nameof(DetailView))]
        public MotivoRechazo MotivoRechazo
        {
            get => motivoRechazo;
            set => SetPropertyValue(nameof(motivoRechazo), ref motivoRechazo, value);
        }

        [ToolTip("Motivo de Descarte")]
        [XafDisplayName("Motivo de Descarte")]
        [Appearance("", TargetItems = nameof(MotivoDescarte), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('MotivoDescarte', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('MotivoDescarte', this)", Context = nameof(DetailView))]
        public MotivoDescarte MotivoDescarte
        {
            get => motivoDescarte;
            set => SetPropertyValue(nameof(motivoDescarte), ref motivoDescarte, value);
        }

        [XafDisplayName("Enviado al Back")]
        [Appearance("", TargetItems = nameof(EnviadoBack), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('EnviadoBack', this)")]
        [Appearance("", Enabled = false, Criteria = "!UsuarioEnRol('PCA_EditarEnviadoBack')", Context = "Any")]
        public bool EnviadoBack
        {
            get => enviadoBack;
            set => SetPropertyValue(nameof(EnviadoBack), ref enviadoBack, value);
        }

        [XafDisplayName("Marcar para Monto SIN Seguro")]
        [Appearance("", TargetItems = nameof(Neto), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Neto', this)")]
        [Appearance("Neto", Enabled = false, Criteria = "Estado.Codigo == 'APROBADA' || Estado.Codigo == 'PENDIENTE APROBACION' || Estado.Codigo == 'PENDIENTE PROCESO' || Estado.Codigo == 'FINALIZADA' || Producto.Codigo == 'CREDIFACIL_4' || Producto.Codigo == 'DESEMBOLSADO'", Context = nameof(DetailView))]
        public bool Neto
        {
            get => neto;
            set => SetPropertyValue(nameof(Neto), ref neto, value);
        }

        [NonPersistent]
        public string LugarTrabajo
        {
            get
            {
                List<PersonaEmpleo> personasEmpleos = Titular.Empleos.Where<PersonaEmpleo>(e => e.Actual).ToList();

                if (personasEmpleos.Count() > 1)
                {
                    string lugarTrabajo = "";

                    foreach (PersonaEmpleo personaEmpleo in personasEmpleos)
                    {
                        lugarTrabajo = lugarTrabajo + personaEmpleo.Empresa + " - ";
                    }

                    lugarTrabajo = lugarTrabajo.Replace(" - ", Environment.NewLine);

                    return lugarTrabajo;
                }

                return personasEmpleos.FirstOrDefault().Empresa;
            }
        }

        [NonPersistent]
        public decimal Salario => Titular.Salario;

        [NonPersistent]
        public int Edad => Titular.Edad;

        [NonPersistent]
        public string Genero => Titular?.Genero?.Descripcion;

        [NonPersistent]
        public string EstadoCivil => Titular.EstadoCivil.Descripcion;

        [NonPersistent]
        public int PromedioAtraso => SolicitudPersonaTitular.PromedioAtraso;

/*        [NonPersistent]
        public string Sucursal => CreadaPor.Sucursal.Nombre;*/

        //ultimaSolicitud = todasMenosActual.Where(s => s.Solicitud.Oid == todasMenosActual.Max(x => x.Solicitud.Oid)).FirstOrDefault();

        [NonPersistent]
        //[Appearance("", TargetItems = nameof(UsuarioParaTareas), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('UsuarioParaTareas', this)")]
        //[Appearance("", Enabled = false, Criteria = "!CampoEditable('UsuarioParaTareas', this)", Context = nameof(DetailView))]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInReports(true), VisibleInDashboards(true)]
        public Usuario UsuarioParaTareas
        {
            get;
            set;
        }

        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false, TargetCriteria = "CampoObligatorio('ObservacionRechDesc', Producto)")]
        [Size(SizeAttribute.Unlimited)]
        [Appearance("", TargetItems = nameof(ObservacionRechDesc), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('ObservacionRechDesc', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('ObservacionRechDesc', this)", Context = nameof(DetailView))]
        [XafDisplayName("Observación Rechazo/Descarte")]
        public string ObservacionRechDesc
        {
            get => observacionRechDesc;
            set => SetPropertyValue(nameof(ObservacionRechDesc), ref observacionRechDesc, value);
        }

        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false, TargetCriteria = "CampoObligatorio('Observacion', Producto)")]
        [Size(SizeAttribute.Unlimited)]
        [Appearance("", TargetItems = nameof(Observacion), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Observacion', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('Observacion', this)", Context = nameof(DetailView))]
        [Appearance("Resumen", Enabled = false, Criteria = "Estado.Codigo == 'PENDIENTE APROBACION' || Estado.Codigo == 'APROBADA' || Estado.Codigo == 'FINALIZADA'", Context = nameof(DetailView))]
        [XafDisplayName("Resumen")]
        public string Observacion
        {
            get => observacion;
            set => SetPropertyValue(nameof(Observacion), ref observacion, value);
        }

        [Association("Nota-Solicitud")]
        [DevExpress.Xpo.Aggregated]
        [ImmediatePostData]
        [Appearance("VNotas", TargetItems = nameof(Notas), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Notas', this)")]
        [Appearance("ENotas", Enabled = false, Criteria = "!CampoEditable('Notas', this)", Context = nameof(DetailView))]
        public XPCollection<NotaSolicitud> Notas => GetCollection<NotaSolicitud>(nameof(Notas));

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [ImmediatePostData]
        [Appearance("V2Personas", TargetItems = nameof(Personas), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Personas', this)")]
        [Appearance("APersonas", Enabled = false, Criteria = "!CampoEditable('Personas', this)", Context = nameof(DetailView))]
        [Association("Solicitud-SolicitudPersona")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<SolicitudPersona> Personas => GetCollection<SolicitudPersona>(nameof(Personas));

        [CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
        [Browsable(false)]
        public XPCollection<Persona> PersonasVinculadas
        {
            get
            {
                if (IsLoading || IsSaving)
                {
                    return null;
                }

                if (Personas.Count == 0)
                {
                    return null;
                }

                Guid[] personas = Personas.Where(sp => sp.Persona != null).Select(sp => sp.Persona.Oid).ToArray();

                XPCollection<Persona> xps = new XPCollection<Persona>(Session)
                {
                    Filter = new InOperator(nameof(Oid), personas),
                    Sorting = new SortingCollection(new SortProperty(nameof(Oid), SortingDirection.Ascending))
                };

                return xps;
            }
        }

        [RuleValueComparison("RPlazo", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "CampoObligatorio('Plazo', Producto)", SkipNullOrEmptyValues = false, CustomMessageTemplate = "Plazo mayor que cero")]
        [Appearance("V2Plazo", TargetItems = nameof(Plazo), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Plazo', this)")]
        [Appearance("APlazo", Enabled = false, Criteria = "!CampoEditable('Plazo', this)", Context = nameof(DetailView))]
        public int Plazo
        {
            get => plazo;
            set => SetPropertyValue(nameof(Plazo), ref plazo, value);
        }

/*        [RuleValueComparison("REdad", DefaultContexts.Save, ValueComparisonType.GreaterThan, 0, TargetCriteria = "CampoObligatorio('Edad', Producto)", SkipNullOrEmptyValues = false, CustomMessageTemplate = "La edad debe ser mayor que cero")]
        [Appearance("V2Edad", TargetItems = nameof(Edad), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Edad', this)")]
        [Appearance("AEdad", Enabled = false, Criteria = "!CampoEditable('Edad', this)", Context = nameof(DetailView))]
        public int Edad
        {
            get => edad;
            set => SetPropertyValue(nameof(Edad), ref edad, value);
        }*/

        [ImmediatePostData(true)]
        [Association("Solicitud-Producto")]
        [Appearance("AProducto", Enabled = false, Criteria = "Oid != -1", Context = nameof(DetailView))]
        [DataSourceCriteria("Activo=true")]
        //[RuleRequiredField(DefaultContexts.Save)]
        public Producto Producto
        {
            get => producto;
            set => SetPropertyValue(nameof(Producto), ref producto, value);
        }

        [Association("Solicitud-Resultados")]
        [DevExpress.Xpo.Aggregated]
        [NonCloneable]
        [ImmediatePostData]
        [Appearance("V2ResultadosMotor", TargetItems = nameof(ResultadosMotor), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('ResultadosMotor', this)")]
        [ModelDefault("AllowEdit", "false")]
        public XPCollection<Resultado> ResultadosMotor => GetCollection<Resultado>(nameof(ResultadosMotor));

        [Association("Seguimiento-Solicitud")]
        [Appearance("VSeguimientos", TargetItems = nameof(Seguimientos), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Seguimientos', this)")]
        [Appearance("ASeguimientos", Enabled = false, Criteria = "!CampoEditable('Seguimientos', this)", Context = nameof(DetailView))]
        public XPCollection<Seguimiento> Seguimientos => GetCollection<Seguimiento>(nameof(Seguimientos));

        [Association("Solicitud-SolicitudSeguimiento")]
        [DevExpress.Xpo.Aggregated]
        [Appearance("VSolicitudEscribania", TargetItems = nameof(Escribania), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Escribania', this)")]
        [Appearance("ASolicitudEscribania", Enabled = false, Criteria = "!CampoEditable('Escribania', this)", Context = nameof(DetailView))]
        public XPCollection<SolicitudSeguimiento> Escribania => GetCollection<SolicitudSeguimiento>(nameof(Escribania));


        [PersistentAlias("Iif(Personas[TipoPersona.Codigo='SOL'].Count == 1, Personas[TipoPersona.Codigo='SOL'].Single(), null)")]
        [Appearance("", TargetItems = nameof(SolicitudPersonaTitular), AppearanceItemType = nameof(ViewItem), Visibility = ViewItemVisibility.Hide)]
        public SolicitudPersona SolicitudPersonaTitular => EvaluateAlias(nameof(SolicitudPersonaTitular)) as SolicitudPersona;

        [PersistentAlias("Iif(SolicitudOriginal.SolicitudPersonaTitular != null, SolicitudOriginal.SolicitudPersonaTitular, null)")]
        //[Appearance("", TargetItems = nameof(SolicitudPersonaTitular), AppearanceItemType = nameof(ViewItem), Visibility = ViewItemVisibility.Hide)]
        public SolicitudPersona SolicitudAsociadaPersonaTitular => EvaluateAlias(nameof(SolicitudAsociadaPersonaTitular)) as SolicitudPersona;

        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInReports(true), VisibleInDashboards(true)]
        [PersistentAlias("Iif(Personas[TipoPersona.Descripcion='Conyuge'].Count == 1, Personas[TipoPersona.Descripcion='Conyuge'].Single(), null)")]
        [Appearance("V2SolicitudPersonaConyuge", TargetItems = nameof(SolicitudPersonaConyuge), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('SolicitudPersonaConyuge', this)")]
        public SolicitudPersona SolicitudPersonaConyuge => EvaluateAlias(nameof(SolicitudPersonaConyuge)) as SolicitudPersona;

        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false), VisibleInReports(true), VisibleInDashboards(true)]
        [PersistentAlias("Iif(Personas[TipoPersona.Codigo='COD'].Count == 1, Personas[TipoPersona.Codigo='COD'].Single(), null)")]
        [Appearance("SolicitudPersonaGarante", TargetItems = nameof(SolicitudPersonaGarante), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('SolicitudPersonaGarante', this)")]
        public SolicitudPersona SolicitudPersonaGarante => EvaluateAlias(nameof(SolicitudPersonaGarante)) as SolicitudPersona;

        [Appearance("V2Tareas", TargetItems = nameof(Tareas), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Tareas', this)")]
        [Association("Solicitud-Tareas")]
        [DevExpress.Xpo.Aggregated]
        [ModelDefault("AllowEdit", "false")]
        public XPCollection<Tarea> Tareas => GetCollection<Tarea>(nameof(Tareas));

        /*[PersistentAlias("Iif(IsNull(EstadoActual), Now() - Fecha, Now() - EstadoActual.Fecha)")]
        [ModelDefault("DisplayFormat", @"{0:d\.hh\:mm\:ss}")]
        [Appearance("V2TiempoEstadoActual", TargetItems = nameof(TiempoEstadoActual), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('TiempoEstadoActual', this)")]
        public TimeSpan? TiempoEstadoActual => EvaluateAlias(nameof(TiempoEstadoActual)) as TimeSpan?; */

        ////[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("Tipo de documento de identidad")]
        [Appearance("V2TipoDocumento", TargetItems = nameof(TipoDocumento), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('TipoDocumento', this)")]
        [Appearance("ATipoDocumento", Enabled = false, Criteria = "!CampoEditable('TipoDocumento', this)", Context = nameof(DetailView))]
        public TipoDocumento TipoDocumento
        {
            get => tipoDocumento;
            set => SetPropertyValue(nameof(TipoDocumento), ref tipoDocumento, value);
        }


        [XafDisplayName("Fecha para visita")]
        [ModelDefault("DisplayFormat", @"{0: dd/MM/yyyy HH:mm:ss}")]
        [EditorAlias("ITTIDateTimeEditor")]
        [Appearance("", TargetItems = nameof(FechaVisita), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('FechaVisita', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('FechaVisita', this)", Context = nameof(DetailView))]
        public DateTime FechaVisita
        {
            get => fechaVisita;
            set => SetPropertyValue(nameof(FechaVisita), ref fechaVisita, value);
        }

        [PersistentAlias("Iif(Inmuebles[].Count == 1, Inmuebles[].Single(), null)")]
        [Appearance("", TargetItems = nameof(InmuebleSolicitud), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('InmuebleSolicitud', this)")]
        public Inmueble InmuebleSolicitud => EvaluateAlias(nameof(InmuebleSolicitud)) as Inmueble;

        [Appearance("", TargetItems = nameof(CiudadInmueble), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('CiudadInmueble', this)")]
        [PersistentAlias("Iif(!IsNull(InmuebleSolicitud), InmuebleSolicitud.Direccion.Ciudad, null)")]
        public Ciudad CiudadInmueble => EvaluateAlias(nameof(CiudadInmueble)) as Ciudad;


        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false, TargetCriteria = "CampoObligatorio('Titular', Producto)")]
        [Appearance("VTitular", TargetItems = nameof(Titular), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Titular', this)")]
        [PersistentAlias("Iif(!IsNull(SolicitudPersonaTitular), SolicitudPersonaTitular.Persona, null)")]
        [XafDisplayName("Titular del Crédito")]
        public Persona Titular => EvaluateAlias(nameof(Titular)) as Persona;

        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = true, TargetCriteria = "CampoObligatorio('AsociadaTitular', Producto)")]
        [Appearance("VAsociadaTitular", TargetItems = nameof(AsociadaTitular), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('AsociadaTitular', this)")]
        [PersistentAlias("Iif(!IsNull(SolicitudAsociadaPersonaTitular), SolicitudAsociadaPersonaTitular.Persona, null)")]
        [XafDisplayName("Titular Asociada del Crédito")]
        public Persona AsociadaTitular => EvaluateAlias(nameof(AsociadaTitular)) as Persona;

        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = true, TargetCriteria = "CampoObligatorio('AsociadaCodeudor', Producto)")]
        [Appearance("VAsociadaCodeudor", TargetItems = nameof(AsociadaCodeudor), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('AsociadaCodeudor', this)")]
        [PersistentAlias("Iif(!IsNull(SolicitudCodeudor), SolicitudCodeudor.Titular, null)")]
        [Appearance("VAsociadaCodeudor2", TargetItems = nameof(AsociadaCodeudor), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "SolicitudCodeudor.Titular = null")]
        [XafDisplayName("Codeudor Asociada del Crédito")]
        public Persona AsociadaCodeudor => EvaluateAlias(nameof(AsociadaCodeudor)) as Persona;

        /* [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false, TargetCriteria = "CampoObligatorio('TelefonoTitular', Producto)")]
         [Appearance("", TargetItems = nameof(TelefonoTitular), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('TelefonoTitular', this)")]
         [PersistentAlias("Iif(!IsNull(Titular), Titular.TelefonoPreferido.TelefonoCompleto, null)")]
         [XafDisplayName("Teléfono del Titular")]
         public string TelefonoTitular => EvaluateAlias(nameof(TelefonoTitular)) as string;*/

        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false, TargetCriteria = "CampoObligatorio('TipoTelefonoTitular', Producto)")]
        [Appearance("", TargetItems = nameof(TipoTelefonoTitular), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('TipoTelefonoTitular', this)")]
        [PersistentAlias("Iif(!IsNull(Titular) and !IsNull(Titular.TelefonoPreferido), Titular.TelefonoPreferido.TipoTelefono, null)")]
        [XafDisplayName("Tipo de Teléfono del Titular")]
        public TipoTelefono TipoTelefonoTitular => EvaluateAlias(nameof(TipoTelefonoTitular)) as TipoTelefono;

        [Appearance("VTitularLineaCredito", TargetItems = nameof(Titular), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('LineaCreditoTitular', this)")]
        [Appearance("", Enabled = false)]
        [PersistentAlias("Iif(!IsNull(Titular), Titular.LineaCredito, null)")]
        [XafDisplayName("Linea de Crédito del titular")]
        public decimal? LineaCreditoTitular => EvaluateAlias(nameof(LineaCreditoTitular)) as decimal?;

        [Appearance("", TargetItems = nameof(Titular), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('CuotaMaximaTitular', this)")]
        [Appearance("", Enabled = false)]
        [PersistentAlias("Iif(!IsNull(Titular), Titular.MaximaCuota, null)")]
        [XafDisplayName("Cuota máxima del titular")]
        public decimal? CuotaMaximaTitular => EvaluateAlias(nameof(CuotaMaximaTitular)) as decimal?;


        [Appearance("", TargetItems = nameof(DocumentoTitular), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('DocumentoTitular', this)")]
        [PersistentAlias("Iif(!IsNull(Titular), Iif(!IsNull(Titular.Documento), Titular.Documento, 'Sin Documento'), null)")]
        [XafDisplayName("Documento del Titular")]
        public string DocumentoTitular => EvaluateAlias(nameof(DocumentoTitular)) as string;

        /*[Appearance("V2TotalCheques", TargetItems = nameof(TotalCheques), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('TotalCheques', this)")]
        [PersistentAlias("Cheques.Count")]
        public int TotalCheques => (int)EvaluateAlias(nameof(TotalCheques));  */

        [Appearance("VTotalTareasPendientes", TargetItems = nameof(TotalTareasPendientes), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('TotalTareasPendientes', this)")]
        [PersistentAlias("[Tareas][Estado != 3 and Estado != 4].Count()")]
        public int? TotalTareasPendientes => EvaluateAlias(nameof(TotalTareasPendientes)) as int?;

        [ImmediatePostData]
        [Appearance("", TargetItems = nameof(PropuestasComerciales), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('PropuestasComerciales', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('PropuestasComerciales', this)", Context = nameof(DetailView))]
        public XPCollection<Presupuesto> PropuestasComerciales
        {
            get
            {
                return new XPCollection<Presupuesto>(this.Session, CriteriaOperator.Parse($"EsPropuesta = True and Solicitud = '{this.Oid}'"));
            }
        }

        [ImmediatePostData]
        [Appearance("", TargetItems = nameof(Presupuestos), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Presupuestos', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('Presupuestos', this)", Context = nameof(DetailView))]
        public XPCollection<Presupuesto> Presupuestos
        {
            get
            {
                var _presupuestos = new XPCollection<Presupuesto>(this.Session, CriteriaOperator.Parse($"(EsPropuesta = False or IsNull(EsPropuesta)) and Solicitud = '{this.Oid}'"));
                return _presupuestos;
            }
        }

        [PersistentAlias("Seguimientos.Count()")]
        [VisibleInDetailView(false)]
        [XafDisplayName("Cantidad de Seguimientos")]
        public int CantSeguimientos => (int)EvaluateAlias(nameof(CantSeguimientos));

        [Size(1)]
        [Appearance("", TargetItems = nameof(Faja), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('Faja', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('Faja', this)", Context = nameof(DetailView))]
        [Appearance("Faja", Enabled = false, Criteria = "Estado.Codigo == 'PENDIENTE APROBACION' || Estado.Codigo == 'APROBADA' || Estado.Codigo == 'FINALIZADA' || Estado.Codigo == 'DESEMBOLSADO' || Estado.Codigo == 'A DESEMBOLSAR'", Context = nameof(DetailView))]
        public string Faja
        {
            get => faja;
            set => SetPropertyValue(nameof(Faja), ref faja, value);
        }

        [Appearance("", TargetItems = nameof(TasaAnual), AppearanceItemType = nameof(ViewItem), Context = nameof(DetailView), Visibility = ViewItemVisibility.Hide, Criteria = "!EvalCriterioMostrar('TasaAnual', this)")]
        [Appearance("", Enabled = false, Criteria = "!CampoEditable('TasaAnual', this)", Context = nameof(DetailView))]
        [ModelDefault("AllowEdit", "false")]
        public decimal TasaAnual
        {
            get => tasaAnual;
            set => SetPropertyValue(nameof(TasaAnual), ref tasaAnual, value);
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

        public static void GenerarSolAsociada(Solicitud solicitud, Solicitud solNueva, WFAccionConfig configuracion) 
        {
            //datos de solicitud
            solNueva.Fecha = DateTime.Now;
            solNueva.Monto = 0;
            solNueva.Producto = configuracion.ProductoDestino;
            solNueva.SolicitudOriginal = solicitud;
            solNueva.Documento = solicitud.Documento;
            solNueva.TipoDocumento = solicitud.TipoDocumento;

            //solicitante
            SolicitudPersona solPersona = solNueva.ObjectSpace.CreateObject<SolicitudPersona>();
            solPersona.Persona = solicitud.SolicitudPersonaTitular.Persona;
            solPersona.Solicitud = solNueva;
            solPersona.TipoPersona = solicitud.SolicitudPersonaTitular.TipoPersona;
            if (solPersona.Parentezco != null)
                solPersona.Parentezco = solicitud.SolicitudPersonaTitular.Parentezco;

            //ingresos
            foreach (var ing in solicitud.SolicitudPersonaTitular.Ingresos)
            {
                SolicitudPersonaIngreso ingnuevo = solNueva.ObjectSpace.CreateObject<SolicitudPersonaIngreso>();

                if (ing.Concepto == null)
                    throw new Exception("Existe algún ingreso sin concepto cargado");
                ingnuevo.Concepto = ing.Concepto;
                ingnuevo.Demostrable = ing.Demostrable;
                ingnuevo.Monto = ing.Monto;
                ingnuevo.Observacion = ing.Observacion;
                ingnuevo.SolicitudPersona = solPersona;
                ingnuevo.Save();
            }

            //ingresos
            foreach (var egr in solicitud.SolicitudPersonaTitular.Egresos)
            {
                SolicitudPersonaEgreso egrNuevo = solNueva.ObjectSpace.CreateObject<SolicitudPersonaEgreso>();
                egrNuevo.Concepto = egr.Concepto;
                egrNuevo.ACancelar = egr.ACancelar;
                egrNuevo.Monto = egr.Monto;
                egrNuevo.Observacion = egr.Observacion;
                egrNuevo.SolicitudPersona = solPersona;
                egrNuevo.Save();
            }

            //referencias personales
            foreach (var item in solicitud.SolicitudPersonaTitular.ReferenciasPersonales)
            {
                SolicitudPersonaRefPer refPerNuevo = solNueva.ObjectSpace.CreateObject<SolicitudPersonaRefPer>();
                refPerNuevo.NombreCompleto = item.NombreCompleto;

                if (item.Parentezco != null)
                    refPerNuevo.Parentezco = item.Parentezco;

                refPerNuevo.SolicitudPersona = solPersona;
                refPerNuevo.Telefono = item.Telefono;
            }

            //referencias comerciales
            foreach (var item in solicitud.SolicitudPersonaTitular.ReferenciasComerciales)
            {
                SolicitudPersonaRefCom refComNuevo = solNueva.ObjectSpace.CreateObject<SolicitudPersonaRefCom>();

                if (item.Entidad != null)
                    refComNuevo.Entidad = item.Entidad;

                refComNuevo.MontoCuota = item.MontoCuota;
                refComNuevo.SolicitudPersona = solPersona;
                refComNuevo.MontoSolicitado = item.MontoSolicitado;
                refComNuevo.Observacion = item.Observacion;
                refComNuevo.Plazo = item.Plazo;
                refComNuevo.UltimoPago = item.UltimoPago;
            }

            //referencias laborales
            foreach (var item in solicitud.SolicitudPersonaTitular.ReferenciasLaborales)
            {
                SolicitudPersonaRefLab refLabNuevo = solNueva.ObjectSpace.CreateObject<SolicitudPersonaRefLab>();

                refLabNuevo.LugarTrabajo = item.LugarTrabajo;
                refLabNuevo.NombreCompleto = item.NombreCompleto;
                refLabNuevo.Observacion = item.Observacion;
                refLabNuevo.Puesto = item.Puesto;
                refLabNuevo.SolicitudPersona = solPersona;
                refLabNuevo.Telefono = item.Telefono;
            }

            if (configuracion.CopiarAdjuntos)
            {
                //Adjuntos
                foreach (var item in solicitud.Adjuntos)
                {
                    Adjunto adjNuevo = solNueva.ObjectSpace.CreateObject<Adjunto>();

                    adjNuevo.Adjuntado = item.Adjuntado;
                    adjNuevo.AdjuntadoPor = item.AdjuntadoPor;
                    adjNuevo.Archivo = item.Archivo;
                    adjNuevo.Descripcion = item.Descripcion;
                    adjNuevo.Fecha = item.Fecha;
                    adjNuevo.GeoLocalizacion = item.GeoLocalizacion;
                    adjNuevo.Imagen = item.Imagen;
                    adjNuevo.Inmueble = item.Inmueble;
                    adjNuevo.InmuebleTasacion = item.InmuebleTasacion;
                    adjNuevo.Persona = item.Persona;
                    adjNuevo.Solicitud = solNueva;
                    adjNuevo.TipoAdjunto = item.TipoAdjunto;
                }
            }

            if (configuracion.CopiarInmuebles)
            {
                //Adjuntos
                foreach (var item in solicitud.Inmuebles)
                {
                    solNueva.Inmuebles.Add(item);
                }
            }

            solPersona.Save();
            solNueva.Save();

            solicitud.Save();
            solicitud.ObjectSpace.CommitChanges();

        }

        public new class FieldsClass : XPObject.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Adjuntos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Adjuntos"));
                }
            }

            public OperandProperty Aprobada
            {
                get
                {
                    return new OperandProperty(GetNestedName("Aprobada"));
                }
            }

            public OperandProperty AProcesar
            {
                get
                {
                    return new OperandProperty(GetNestedName("AProcesar"));
                }
            }

            public OperandProperty Cheques
            {
                get
                {
                    return new OperandProperty(GetNestedName("Cheques"));
                }
            }

            public OperandProperty ChequesAprobados
            {
                get
                {
                    return new OperandProperty(GetNestedName("ChequesAprobados"));
                }
            }

            public Usuario.FieldsClass CreadaPor
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("CreadaPor"));
                }
            }

            public OperandProperty Documento
            {
                get
                {
                    return new OperandProperty(GetNestedName("Documento"));
                }
            }

            public EstadoSolicitud.FieldsClass Estado
            {
                get
                {
                    return new EstadoSolicitud.FieldsClass(GetNestedName("Estado"));
                }
            }

            public OperandProperty EstadoSolicitud
            {
                get
                {
                    return new OperandProperty(GetNestedName("EstadoSolicitud"));
                }
            }

            public RegistroEstadoSolicitud.FieldsClass EstadoActual
            {
                get
                {
                    return new RegistroEstadoSolicitud.FieldsClass(GetNestedName("EstadoActual"));
                }
            }

            public OperandProperty Estados
            {
                get
                {
                    return new OperandProperty(GetNestedName("Estados"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public OperandProperty FechaEstadoActual
            {
                get
                {
                    return new OperandProperty(GetNestedName("FechaEstadoActual"));
                }
            }

            public OperandProperty Inmuebles
            {
                get
                {
                    return new OperandProperty(GetNestedName("Inmuebles"));
                }
            }

            public MedioIngreso.FieldsClass MedioIngreso
            {
                get
                {
                    return new MedioIngreso.FieldsClass(GetNestedName("MedioIngreso"));
                }
            }

            public OperandProperty Moneda
            {
                get
                {
                    return new OperandProperty(GetNestedName("Moneda"));
                }
            }

            public OperandProperty Monto
            {
                get
                {
                    return new OperandProperty(GetNestedName("Monto"));
                }
            }

            public OperandProperty MontoChequesAprobados
            {
                get
                {
                    return new OperandProperty(GetNestedName("MontoChequesAprobados"));
                }
            }

            public MotivoSolicitud.FieldsClass Motivo
            {
                get
                {
                    return new MotivoSolicitud.FieldsClass(GetNestedName("Motivo"));
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

            public OperandProperty PersonasVinculadas
            {
                get
                {
                    return new OperandProperty(GetNestedName("PersonasVinculadas"));
                }
            }

            public OperandProperty Plazo
            {
                get
                {
                    return new OperandProperty(GetNestedName("Plazo"));
                }
            }

            public Producto.FieldsClass Producto
            {
                get
                {
                    return new Producto.FieldsClass(GetNestedName("Producto"));
                }
            }

            public OperandProperty ResultadosMotor
            {
                get
                {
                    return new OperandProperty(GetNestedName("ResultadosMotor"));
                }
            }

            public OperandProperty Seguimientos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Seguimientos"));
                }
            }

            public OperandProperty SeguimientosProgramados
            {
                get
                {
                    return new OperandProperty(GetNestedName("SeguimientosProgramados"));
                }
            }

            public SolicitudPersona.FieldsClass SolicitudPersonaTitular
            {
                get
                {
                    return new SolicitudPersona.FieldsClass(GetNestedName("SolicitudPersonaTitular"));
                }
            }

            public OperandProperty Tareas
            {
                get
                {
                    return new OperandProperty(GetNestedName("Tareas"));
                }
            }

            public OperandProperty TiempoEstadoActual
            {
                get
                {
                    return new OperandProperty(GetNestedName("TiempoEstadoActual"));
                }
            }

            public TipoDocumento.FieldsClass TipoDocumento
            {
                get
                {
                    return new TipoDocumento.FieldsClass(GetNestedName("TipoDocumento"));
                }
            }

            public Persona.FieldsClass Titular
            {
                get
                {
                    return new Persona.FieldsClass(GetNestedName("Titular"));
                }
            }

            public OperandProperty TotalCheques
            {
                get
                {
                    return new OperandProperty(GetNestedName("TotalCheques"));
                }
            }

            public OperandProperty TotalTareasPendientes
            {
                get
                {
                    return new OperandProperty(GetNestedName("TotalTareasPendientes"));
                }
            }

            public OperandProperty PropuestasComerciales
            {
                get
                {
                    return new OperandProperty(GetNestedName("PropuestasComerciales"));
                }
            }

            public OperandProperty Presupuestos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Presupuestos"));
                }
            }

            public OperandProperty SolicitudOriginal
            {
                get
                {
                    return new OperandProperty(GetNestedName("SolicitudOriginal"));
                }
            }
        }
    }
}
