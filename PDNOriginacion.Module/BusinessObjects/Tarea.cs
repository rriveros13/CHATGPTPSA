using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule.Notifications;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Solicitud")]
    public class Tarea : BaseObject, ISupportNotifications, IObjectSpaceLink
    {
        static FieldsClass _Fields;
        private DateTime? alarmTime;
        /*
        private DateTime? reservadaEn;
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        [ModelDefault("AllowEdit", "False")]
        public DateTime? ReservadaEn
        {
        get => reservadaEn;
        set => SetPropertyValue(nameof(ReservadaEn), ref reservadaEn, value);
        }*/
        private Usuario completadaPor;
        /*private DateTime completadaEn;
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        [ModelDefault("AllowEdit", "False")]
        public DateTime CompletadaEn
        {
        get => completadaEn;
        set => SetPropertyValue(nameof(CompletadaEn), ref completadaEn, value);
        }*/
        private bool escalada;
        private EstadoTarea estado;
        private DateTime fechaCreacion;
        private DateTime fechaEntrega;
        private IList<PostponeTime> postponeTimes;
        private TimeSpan? remindIn;
        private Usuario reservadaPor;
        private Solicitud solicitud;
        private WFTarea tipoTarea;
        private string descripcion;
        private PrioridadTarea prioridadTarea;
        private bool _manual;

        public Tarea(Session session) : base(session)
        {
        }

        private IList<PostponeTime> CreatePostponeTimes()
        {
            //IList<PostponeTime> result = PostponeTime.CreateDefaultPostponeTimesList();

            IList<PostponeTime> result = new List<PostponeTime>
            {
                new PostponeTime("None", null, "None"),
                new PostponeTime("AtStartTime", TimeSpan.Zero, "At Start Time"),
                new PostponeTime("5", new TimeSpan(0, 5, 0), "5 minutos")
            };
            PostponeTime.SortPostponeTimesList(result);
            return result;
        }
        private void SetAlarmTime(DateTime? startDate, TimeSpan remindTime) => alarmTime =
            ((startDate - DateTime.MinValue) > remindTime) ? startDate - remindTime : DateTime.MinValue;

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            fechaCreacion = DateTime.Now;
            estado = EstadoTarea.Nueva;
            escalada = false;
            prioridadTarea = PrioridadTarea.Baja;
        }

        [Browsable(false)]
        public DateTime? AlarmTime
        {
            get => alarmTime;
            set
            {
                SetPropertyValue(nameof(AlarmTime), ref alarmTime, value);
                if(value == null)
                {
                    remindIn = null;
                    IsPostponed = false;
                }
            }
        }

        [PersistentAlias("Iif(!(IsNull(FechaCierre) or IsNull(FechaEntrega)), FechaCierre > FechaEntrega , false)")]
        public bool? CierreVencida => EvaluateAlias(nameof(CierreVencida)) as bool?;

        [ModelDefault("AllowEdit", "False")]
        [Association("TareasFinalizadas-Usuario")]
        public Usuario CompletadaPor
        {
            get => completadaPor;
            set => SetPropertyValue(nameof(CompletadaPor), ref completadaPor, value);
        }

        [ModelDefault("AllowEdit", "False")]
        public bool Escalada
        {
            get => escalada;
            set => SetPropertyValue(nameof(Escalada), ref escalada, value);
        }

        public EstadoTarea Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        public PrioridadTarea Prioridad
        {
            get => prioridadTarea;
            set
            {
                SetPropertyValue(nameof(Prioridad), ref prioridadTarea, value);

                if (value == PrioridadTarea.Alta)
                    this.AlarmTime = DateTime.Now;

            }
        }

        [PersistentAlias("Iif(Historial[Estado == 3 Or Estado == 4].Count == 1,Historial[Estado == 3 Or Estado == 4].Single(), null)")]
        public HistorialTarea EstadoCierre => EvaluateAlias(nameof(EstadoCierre)) as HistorialTarea;

        [PersistentAlias("Iif(IsNull(EstadoCierre), null, EstadoCierre.Fecha)")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        public DateTime? FechaCierre => EvaluateAlias(nameof(FechaCierre)) as DateTime?;

        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        [ModelDefault("AllowEdit", "False")]
        public DateTime FechaCreacion
        {
            get => fechaCreacion;
            set => SetPropertyValue(nameof(FechaCreacion), ref fechaCreacion, value);
        }

        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        [ModelDefault("AllowEdit", "false")]
        public DateTime FechaEntrega
        {
            get => fechaEntrega;
            set => SetPropertyValue(nameof(FechaEntrega), ref fechaEntrega, value);
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

        [Association("Tarea-Historial")]
        [ModelDefault("AllowEdit", "false")]
        public XPCollection<HistorialTarea> Historial => GetCollection<HistorialTarea>(nameof(Historial));

        [Browsable(false)]
        public bool IsPostponed
        {
            get;
            set;
        }

        [Browsable(false)]
        public string NotificationMessage => string.Concat(solicitud.Oid, " - ", descripcion);

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [Browsable(false)]
        [NonPersistent]
        public IEnumerable<PostponeTime> PostponeTimeList
        {
            get
            {
                if(postponeTimes == null)
                {
                    postponeTimes = CreatePostponeTimes();
                }
                return postponeTimes;
            }
        }

        [ImmediatePostData]
        [NonPersistent]
        [ModelDefault("AllowClear", "False")]
        [DataSourceProperty(nameof(PostponeTimeList))]
        [SearchMemberOptions(SearchMemberMode.Exclude)]
        public PostponeTime ReminderTime
        {
            get
            {
                if(RemindIn.HasValue)
                {
                    return PostponeTimeList.FirstOrDefault(x => remindIn != null &&
                        (x.RemindIn != null && x.RemindIn.Value == remindIn.Value));
                }
                else
                {
                    return PostponeTimeList.FirstOrDefault(x => x.RemindIn == null);
                }
            }
            set
            {
                if(!IsLoading)
                {
                    if(value.RemindIn.HasValue)
                    {
                        RemindIn = value.RemindIn.Value;
                    }
                    else
                    {
                        RemindIn = null;
                    }
                }
            }
        }

        [Browsable(false)]
        public TimeSpan? RemindIn
        {
            get => remindIn;
            set
            {
                SetPropertyValue(nameof(RemindIn), ref remindIn, value);
                if(!IsLoading)
                {
                    if(value != null)
                    {
                        SetAlarmTime(FechaCreacion, value.Value);
                    }
                    else
                    {
                        alarmTime = null;
                    }
                }
            }
        }

        [ModelDefault("AllowEdit", "False")]
        [Association("TareaReservada-Usuario")]
        public Usuario ReservadaPor
        {
            get => reservadaPor;
            set => SetPropertyValue(nameof(ReservadaPor), ref reservadaPor, value);
        }

        [Association("Solicitud-Tareas")]
        public Solicitud Solicitud
        {
            get => solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref solicitud, value);
        }

        public string Descripcion
        {
            get => descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref descripcion, value);
        }


        [PersistentAlias("Historial[Estado == 2].Sum(TiempoUtilizado)")]
        [ModelDefault("DisplayFormat", @"{0:d\.hh\:mm\:ss}")]
        public TimeSpan? TiempoAsignada => EvaluateAlias(nameof(TiempoAsignada)) as TimeSpan?;

        [ModelDefault("DisplayFormat", "{0:n0}")]
        [Browsable(false)]
        [VisibleInDashboards]
        [PersistentAlias("Historial[Estado == 2].Sum(TiempoUtilizadoSegundos)")]
        public decimal? TiempoAsignadaSegundos => EvaluateAlias(nameof(TiempoAsignadaSegundos)) as decimal?;

        [PersistentAlias("Iif(IsNull(FechaCierre), Now() - FechaCreacion, FechaCierre - FechaCreacion)")]
        [ModelDefault("DisplayFormat", @"{0:d\.hh\:mm\:ss}")]
        public TimeSpan? TiempoTotal => EvaluateAlias(nameof(TiempoTotal)) as TimeSpan?;

        [ModelDefault("DisplayFormat", "{0:n0}")]
        [Browsable(false)]
        [VisibleInDashboards]
        [PersistentAlias("Iif(IsNull(FechaCierre), ToDecimal(DateDiffSecond(FechaCreacion, Now())), ToDecimal(DateDiffSecond(FechaCreacion, FechaCierre)))")]
        public decimal? TiempoTotalSegundos => EvaluateAlias(nameof(TiempoTotalSegundos)) as decimal?;

        public WFTarea TipoTarea
        {
            get => tipoTarea;
            set => SetPropertyValue(nameof(TipoTarea), ref tipoTarea, value);
        }
        public bool Manual
        {
            get => _manual;
            set => SetPropertyValue(nameof(Manual), ref _manual, value);
        }

        [NonPersistent]
        [Browsable(false)]
        public object UniqueId => Oid;

        [PersistentAlias("Iif(!IsNull(FechaEntrega), (FechaEntrega < Now()) and (IsNull(FechaCierre)) , false)")]
        public bool? Vencida => EvaluateAlias(nameof(Vencida)) as bool?;

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty AlarmTime
            {
                get
                {
                    return new OperandProperty(GetNestedName("AlarmTime"));
                }
            }

            public OperandProperty CierreVencida
            {
                get
                {
                    return new OperandProperty(GetNestedName("CierreVencida"));
                }
            }

            public Usuario.FieldsClass CompletadaPor
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("CompletadaPor"));
                }
            }

            public OperandProperty Escalada
            {
                get
                {
                    return new OperandProperty(GetNestedName("Escalada"));
                }
            }

            public OperandProperty Estado
            {
                get
                {
                    return new OperandProperty(GetNestedName("Estado"));
                }
            }

            public HistorialTarea.FieldsClass EstadoCierre
            {
                get
                {
                    return new HistorialTarea.FieldsClass(GetNestedName("EstadoCierre"));
                }
            }

            public OperandProperty FechaCierre
            {
                get
                {
                    return new OperandProperty(GetNestedName("FechaCierre"));
                }
            }

            public OperandProperty FechaCreacion
            {
                get
                {
                    return new OperandProperty(GetNestedName("FechaCreacion"));
                }
            }

            public OperandProperty FechaEntrega
            {
                get
                {
                    return new OperandProperty(GetNestedName("FechaEntrega"));
                }
            }

            public OperandProperty Historial
            {
                get
                {
                    return new OperandProperty(GetNestedName("Historial"));
                }
            }

            public OperandProperty IsPostponed
            {
                get
                {
                    return new OperandProperty(GetNestedName("IsPostponed"));
                }
            }

            public OperandProperty NotificationMessage
            {
                get
                {
                    return new OperandProperty(GetNestedName("NotificationMessage"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public OperandProperty RemindIn
            {
                get
                {
                    return new OperandProperty(GetNestedName("RemindIn"));
                }
            }

            public Usuario.FieldsClass ReservadaPor
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("ReservadaPor"));
                }
            }

            public Solicitud.FieldsClass Solicitud
            {
                get
                {
                    return new Solicitud.FieldsClass(GetNestedName("Solicitud"));
                }
            }

            public OperandProperty TiempoAsignada
            {
                get
                {
                    return new OperandProperty(GetNestedName("TiempoAsignada"));
                }
            }

            public OperandProperty TiempoAsignadaSegundos
            {
                get
                {
                    return new OperandProperty(GetNestedName("TiempoAsignadaSegundos"));
                }
            }

            public OperandProperty TiempoTotal
            {
                get
                {
                    return new OperandProperty(GetNestedName("TiempoTotal"));
                }
            }

            public OperandProperty TiempoTotalSegundos
            {
                get
                {
                    return new OperandProperty(GetNestedName("TiempoTotalSegundos"));
                }
            }

            public WFTarea.FieldsClass TipoTarea
            {
                get
                {
                    return new WFTarea.FieldsClass(GetNestedName("TipoTarea"));
                }
            }

            public OperandProperty Vencida
            {
                get
                {
                    return new OperandProperty(GetNestedName("Vencida"));
                }
            }
        }
    }

    public enum EstadoTarea
    {
        Nueva = 0,
        Disponible = 1,
        Asignada = 2,
        Finalizada = 3,
        Cancelada = 4
    }
}

namespace PDNOriginacion.Module.BusinessObjects
{
    public enum PrioridadTarea
    {
        Baja,
        Media,
        Alta
    }
}