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
using DevExpress.Xpo.DB;
using System;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(Fecha))]
    public class Seguimiento : BaseObject,  IObjectSpaceLink
    {
        static FieldsClass _Fields;
        private string comentarios;
        private DateTime fecha;
        private MedioIngreso medioIngreso;
        private MotivoSeguimiento motivoSeguimiento;
        private Persona persona;
        private Solicitud solicitud;
        private Telefono telefonoContactado;
        private Usuario usuario;
        private DateTime? proximoSegFecha;
        private Usuario proximoSegUsuario;
        private ProxSeguimientoOpciones _opciones;
        private Seguimiento _seguimientoOriginal;
        private bool _concretado;
        private SeguimientoMasivo _seguimientoMasivo;

        public Seguimiento(Session session) : base(session)
        {
           
        }

        [ModelDefault("AllowEdit", "false")]
        [XafDisplayName("Seguimiento Original")]
        [Association("Seguimiento-SeguimientoOriginal")]
        public Seguimiento SeguimientoOriginal
        {
            get => _seguimientoOriginal;
            set => SetPropertyValue(nameof(SeguimientoOriginal), ref _seguimientoOriginal, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [XafDisplayName("Seguimiento Masivo")]
        [Association("Seguimiento-SeguimientoMasivo")]
        public SeguimientoMasivo SeguimientoMasivo
        {
            get => _seguimientoMasivo;
            set => SetPropertyValue(nameof(SeguimientoMasivo), ref _seguimientoMasivo, value);
        }

        [ModelDefault("DisplayFormat", @"{0: dd/MM/yyyy HH:mm:ss}")]
        [EditorAlias("ITTIDateTimeEditor")]
        [XafDisplayName("Fecha de seguimiento")]
        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false)]
        [Appearance("", Enabled = false, Criteria = "!UsuarioEnRol('Administrator')")]
        public DateTime Fecha
        {
            get => fecha;
            set
            {
                SetPropertyValue(nameof(Fecha), ref fecha, value);
           }
        }

        [PersistentAlias("Iif(!IsNull(Fecha), GetDate(Fecha), null)")]
        [VisibleInDashboards(false), VisibleInReports(false), VisibleInDetailView(false)]
        [XafDisplayName("Fecha seguimiento (día)")]
        public DateTime? FechaSeguimientoAlias => EvaluateAlias(nameof(FechaSeguimientoAlias)) as DateTime?;

        [PersistentAlias("Iif(!IsNull(ProximoSegFecha), GetDate(ProximoSegFecha), null)")]
        [VisibleInDashboards(false), VisibleInReports(false), VisibleInDetailView(false)]
        [XafDisplayName("Próxmimo seguimiento (día)")]
        public DateTime? ProximoSegFechaAlias => EvaluateAlias(nameof(ProximoSegFechaAlias)) as DateTime?;

        [Appearance("", Enabled = false, Context = "Any")]
        [XafDisplayName("Creado Por")]
        public Usuario Usuario
        {
            get => usuario;
            set => SetPropertyValue(nameof(Usuario), ref usuario, value);
        }

        [PersistentAlias("Iif(!IsNull(Solicitud) ,Solicitud.Estado, null)")]
        public EstadoSolicitud EstadoSolicitud=> (EstadoSolicitud)EvaluateAlias(nameof(EstadoSolicitud));

        [Association("Seguimiento-Persona")]
        [ImmediatePostData]
        [DataSourceCriteria("Iif(!IsNull('@This.Solicitud'),Solicitudes[Solicitud='@This.Solicitud'], true)")]
        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false)]
        public Persona Persona
        {
            get => persona;
            set
            {
                bool cambio = SetPropertyValue(nameof(Persona), ref persona, value);
                if (IsLoading || IsSaving || !cambio)
                {
                    return;
                }

                if (persona != null)
                {
                    if (Solicitudes != null)
                    {
                        if (Solicitudes.Count > 0)
                        {
                            solicitud = Solicitudes[0];
                        }
                    }
                    telefonoContactado = persona.TelefonoPreferido;
                    OnChanged(nameof(TelefonoContactado));
                }
                else
                {
                    solicitud = null;
                    telefonoContactado = null;
                    OnChanged(nameof(TelefonoContactado));
                }
                OnChanged(nameof(Solicitud));
            }
        }

        [ModelDefault("AllowEdit", "false")]
        public bool Concretado
        {
            get => _concretado;
            set => SetPropertyValue(nameof(Concretado), ref _concretado, value);
        }

        [ImmediatePostData]
        //[Appearance(nameof(Solicitud), Visibility = ViewItemVisibility.Hide, Criteria = "Iif(IsNull(Persona), true, Persona.CantSolicitudes = 0)", Context = nameof(DetailView))]
        [DataSourceCriteria("Iif(!IsNull('@This.Persona'),Personas[Persona='@This.Persona'], true)")]
        [Association("Seguimiento-Solicitud")]
        public Solicitud Solicitud
        {
            get => solicitud;
            set
            {
                bool cambio = SetPropertyValue(nameof(Solicitud), ref solicitud, value);
                if (IsLoading || IsSaving || !cambio)
                {
                    return;
                }

                if (solicitud != null)
                {
                    if (Personas != null)
                    {
                        if (Personas.Count > 0)
                        {
                            persona = Personas[0];
                        }
                    }
                    telefonoContactado = persona.TelefonoPreferido;
                    OnChanged(nameof(TelefonoContactado));
                }
                else
                {
                    persona = null;
                    telefonoContactado = null;
                    OnChanged(nameof(TelefonoContactado));
                }
                OnChanged(nameof(Persona));
            }
        }

        [DataSourceProperty("Persona.Telefonos")]
        [CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
        public Telefono TelefonoContactado
        {
            get => telefonoContactado;
            set => SetPropertyValue(nameof(TelefonoContactado), ref telefonoContactado, value);
        }

        [DataSourceCriteria("Activo=true")]
        [XafDisplayName("Motivo de Seguimiento")]
        public MotivoSeguimiento MotivoSeguimiento
        {
            get => motivoSeguimiento;
            set => SetPropertyValue(nameof(MotivoSeguimiento), ref motivoSeguimiento, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Comentarios
        {
            get => comentarios;
            set => SetPropertyValue(nameof(Comentarios), ref comentarios, value);
        }

        [NonPersistent]
        [XafDisplayName("Opciones para próximo seguimiento")]
        [ImmediatePostData]
        public ProxSeguimientoOpciones ProxSeguimientoOpciones
        {
            get => _opciones;
            set
            {
                SetPropertyValue(nameof(ProxSeguimientoOpciones), ref _opciones, value);
                if (value == ProxSeguimientoOpciones.Ninguna)
                {
                    ProximoSegFecha = null;
                    ProximoSegUsuario = null;
                }
                else
                {
                    ProximoSegFecha = Fecha.AddDays((int)value);
                    if (proximoSegUsuario == null) ProximoSegUsuario = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
                }
            }

        }

        [XafDisplayName("Fecha para próximo seguimiento")]
        [ModelDefault("DisplayFormat", @"{0: dd/MM/yyyy HH:mm:ss}")]
        [EditorAlias("ITTIDateTimeEditor")]
        public DateTime? ProximoSegFecha
        {
            get => proximoSegFecha;
            set => SetPropertyValue(nameof(ProximoSegFecha), ref proximoSegFecha, value);
        }

        [XafDisplayName("Usuario para próximo seguimiento")]
        public Usuario ProximoSegUsuario
        {
            get => proximoSegUsuario;
            set => SetPropertyValue(nameof(ProximoSegUsuario), ref proximoSegUsuario, value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            usuario = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
            fecha = DateTime.Now;
            medioIngreso = Session.FindObject<MedioIngreso>(MedioIngreso.Fields.Default == (CriteriaOperator)true & MedioIngreso.Fields.Activo == (CriteriaOperator)true);
            MotivoSeguimiento = Session.FindObject<MotivoSeguimiento>(MotivoSeguimiento.Fields.Default ==
                (CriteriaOperator)true &
                MotivoSeguimiento.Fields.Activo == (CriteriaOperator)true);
            ProxSeguimientoOpciones = ProxSeguimientoOpciones.Ninguna;
        }


        [Association("Seguimiento-SeguimientoOriginal")]
        public XPCollection<Seguimiento> Seguimientos => GetCollection<Seguimiento>(nameof(Seguimientos));


        //[ImmediatePostData]
        ////[RuleRequiredField(DefaultContexts.Save)]
        //[Appearance(nameof(Cuestionario), Visibility = ViewItemVisibility.Hide, Criteria = "Persona.CantSeguimientos > 0", Context = nameof(DetailView))]
        //public Cuestionario Cuestionario
        //{
        //    get => cuestionario;
        //    set
        //    {
        //        bool cambio = SetPropertyValue(nameof(Cuestionario), ref cuestionario, value);
        //        if (IsLoading || IsSaving || !cambio)
        //        {
        //            return;
        //        }
        //        if (cuestionario == null)
        //        {
        //            return;
        //        }
        //        Session.Delete(Items);
        //        foreach (CuestionarioItem item in Cuestionario.Items)
        //        {
        //            SeguimientoItems ci = new SeguimientoItems(Session) { Seguimiento = this, Item = item };
        //            Items.Add(ci);
        //        }
        //        OnChanged(nameof(Items));
        //    }
        //}


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

        //[Association("Seguimiento-Items")]
        //[DevExpress.Xpo.Aggregated]
        //[Appearance(nameof(Items), Visibility = ViewItemVisibility.Hide, Criteria = "!SeguimientoInicial", Context = nameof(DetailView))]
        //public XPCollection<SeguimientoItems> Items => GetCollection<SeguimientoItems>(nameof(Items));

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace
        {
            get;
            set;
        }



        //[ImmediatePostData]
        //[CaptionsForBoolValues("S", "N")]
        //public bool SeguimientoInicial
        //{
        //    get => seguimientoInicial;
        //    set => SetPropertyValue(nameof(SeguimientoInicial), ref seguimientoInicial, value);
        //}
        /*[ModelDefault("AllowEdit", "false")]
        public SeguimientoProgramado SeguimientoProgramado
        {
            get => seguimientoProgramado;
            set => SetPropertyValue(nameof(SeguimientoProgramado), ref seguimientoProgramado, value);
        }  */



        [CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
        [Browsable(false)]
        public XPCollection<Solicitud> Solicitudes
        {
            get
            {
                if(IsLoading || IsSaving)
                {
                    return null;
                }

                if(persona == null)
                {
                    return null;
                }

                if(persona.Solicitudes.Count == 0)
                {
                    return null;
                }

                XPQuery<SolicitudPersona> solpersonas = new XPQuery<SolicitudPersona>(Session);
                solpersonas.InTransaction();

                int[] solicitudes = solpersonas.Where(sp => sp.Persona == Persona)
                    .Select(sp => sp.Solicitud.Oid)
                    .ToArray();

                XPCollection<Solicitud> xps = new XPCollection<Solicitud>(Session)
                {
                    Filter = new InOperator(nameof(Oid), solicitudes),
                    Sorting = new SortingCollection(new SortProperty(nameof(Oid), SortingDirection.Descending))
                };

                return xps;
            }
        }

        protected override void OnSaved()
        {
            base.OnSaved();
            Persona.CambiarEstadoPersona(this.persona, true);
        }

        [CollectionOperationSet(AllowAdd = false, AllowRemove = false)]
        [Browsable(false)]
        public XPCollection<Persona> Personas
        {
            get
            {
                if (IsLoading || IsSaving)
                {
                    return null;
                }

                if (solicitud == null)
                {
                    return null;
                }

                if (solicitud.Personas.Count == 0)
                {
                    return null;
                }

                XPQuery<SolicitudPersona> solpersonas = new XPQuery<SolicitudPersona>(Session);
                solpersonas.InTransaction();

                Guid[] personas = solpersonas.Where(sp => sp.Solicitud == solicitud)
                    .Select(sp => sp.Persona.Oid)
                    .ToArray();

                XPCollection<Persona> xps = new XPCollection<Persona>(Session)
                {
                    Filter = new InOperator(nameof(Oid), personas),
                    //Sorting = new SortingCollection(new SortProperty(nameof(NombreCompleto), SortingDirection.Descending))
                };

                return xps;
            }
        }

        [XafDisplayName("Rol en Solicitud")]
        [Appearance(nameof(TipoPersona), Visibility = ViewItemVisibility.Hide, Criteria = "!IsNull(Solicitud)", Context = nameof(DetailView))]
        public string TipoPersona
        {
            get
            {
                if(persona != null && solicitud != null)
                {
                    SolicitudPersona solp = solicitud.Personas.FirstOrDefault(sp => sp.Persona == persona);
                    if(solp != null)
                    {
                        return solp.TipoPersona.Descripcion;
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
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

            public OperandProperty Comentarios
            {
                get
                {
                    return new OperandProperty(GetNestedName("Comentarios"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public MotivoSeguimiento.FieldsClass MotivoSeguimiento
            {
                get
                {
                    return new MotivoSeguimiento.FieldsClass(GetNestedName("MotivoSeguimiento"));
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

            public Solicitud.FieldsClass Solicitud
            {
                get
                {
                    return new Solicitud.FieldsClass(GetNestedName("Solicitud"));
                }
            }

            public OperandProperty Solicitudes
            {
                get
                {
                    return new OperandProperty(GetNestedName("Solicitudes"));
                }
            }

            public Telefono.FieldsClass TelefonoContactado
            {
                get
                {
                    return new Telefono.FieldsClass(GetNestedName("TelefonoContactado"));
                }
            }

            public OperandProperty TipoPersona
            {
                get
                {
                    return new OperandProperty(GetNestedName("TipoPersona"));
                }
            }

            public Usuario.FieldsClass Usuario
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("Usuario"));
                }
            }
        }
    }
}

namespace PDNOriginacion.Module.BusinessObjects
{
    public enum ProxSeguimientoOpciones
    {
        Ninguna = 0,
        _1Dia = 1,
        _2Dias =2,
        _3Dias = 3,
        _5Dias = 5,
        _15Dias = 15 
    }
}