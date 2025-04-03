using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Estado")]
    public class HistorialTarea : BaseObject
    {
        static FieldsClass _Fields;
        private DateTime _fecha;
        private Tarea _tarea;
        private TimeSpan? _tiempo;
        private decimal? _tiempoSegundos;
        private Usuario _usuario;
        private EstadoTarea estado;

        public HistorialTarea(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            Fecha = DateTime.Now;
        }

        public EstadoTarea Estado
        {
            get => estado;
            set => SetPropertyValue(nameof(Estado), ref estado, value);
        }

        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Fecha
        {
            get => _fecha;
            set => SetPropertyValue(nameof(Fecha), ref _fecha, value);
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
        public Tarea Tarea
        {
            get => _tarea;
            set => SetPropertyValue(nameof(Tarea), ref _tarea, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [ModelDefault("DisplayFormat", @"{0:d\.hh\:mm\:ss}")]
        public TimeSpan? TiempoUtilizado
        {
            get => _tiempo;
            set => SetPropertyValue(nameof(TiempoUtilizado), ref _tiempo, value);
        }

        [Browsable(false)]
        [VisibleInDashboards]
        public decimal? TiempoUtilizadoSegundos
        {
            get => _tiempoSegundos;
            set => SetPropertyValue(nameof(TiempoUtilizadoSegundos), ref _tiempoSegundos, value);
        }

        public Usuario Usuario
        {
            get => _usuario;
            set => SetPropertyValue(nameof(Usuario), ref _usuario, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Estado
            {
                get
                {
                    return new OperandProperty(GetNestedName("Estado"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public Tarea.FieldsClass Tarea
            {
                get
                {
                    return new Tarea.FieldsClass(GetNestedName("Tarea"));
                }
            }

            public OperandProperty TiempoUtilizado
            {
                get
                {
                    return new OperandProperty(GetNestedName("TiempoUtilizado"));
                }
            }

            public OperandProperty TiempoUtilizadoSegundos
            {
                get
                {
                    return new OperandProperty(GetNestedName("TiempoUtilizadoSegundos"));
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