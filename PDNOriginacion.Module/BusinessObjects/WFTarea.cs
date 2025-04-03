using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Descripcion")]
    public class WFTarea : BaseObject
    {
        static FieldsClass _Fields;
        private string _descripcion;
        private EstadoSolicitud _estadoDestino;
        private bool _finAutomatico;
        private Producto _producto;
        private Rol _rolUsuario;
        private TimeSpan? _timeSpan;

        public WFTarea(Session session) : base(session)
        {
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _timeSpan = new TimeSpan(0, 4, 0, 0);
            //  _timeSpanAsignado = new TimeSpan(0,2,0,0);
        }
        //private TimeSpan? _timeSpanAsignado;
        //public TimeSpan? SLAAsignado
        //{
        //    get => _timeSpanAsignado;
        //    set => SetPropertyValue(nameof(SLAAsignado), ref _timeSpanAsignado, value);
        //}

        [Association("TipoTarea-Criterios")]
        [Aggregated]
        [ImmediatePostData]
        public XPCollection<TipoTareaCriterio> Criterios => GetCollection<TipoTareaCriterio>(nameof(Criterios));

        //[RuleRequiredField(DefaultContexts.Save)]
        public string Descripcion
        {
            get => _descripcion;
            set => SetPropertyValue(nameof(Descripcion), ref _descripcion, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public EstadoSolicitud EstadoDestino
        {
            get => _estadoDestino;
            set => SetPropertyValue(nameof(EstadoDestino), ref _estadoDestino, value);
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

        public bool FinAutomatico
        {
            get => _finAutomatico;
            set => SetPropertyValue(nameof(FinAutomatico), ref _finAutomatico, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [Association("Producto-Tareas")]
        public Producto Producto
        {
            get => _producto;
            set => SetPropertyValue(nameof(Producto), ref _producto, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public Rol RolUsuario
        {
            get => _rolUsuario;
            set => SetPropertyValue(nameof(RolUsuario), ref _rolUsuario, value);
        }

        public TimeSpan? SLA
        {
            get => _timeSpan;
            set => SetPropertyValue(nameof(SLA), ref _timeSpan, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Criterios
            {
                get
                {
                    return new OperandProperty(GetNestedName("Criterios"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public EstadoSolicitud.FieldsClass EstadoDestino
            {
                get
                {
                    return new EstadoSolicitud.FieldsClass(GetNestedName("EstadoDestino"));
                }
            }

            public OperandProperty Auditoria
            {
                get
                {
                    return new OperandProperty(GetNestedName("Auditoria"));
                }
            }

            public OperandProperty FinAutomatico
            {
                get
                {
                    return new OperandProperty(GetNestedName("FinAutomatico"));
                }
            }

            public Producto.FieldsClass Producto
            {
                get
                {
                    return new Producto.FieldsClass(GetNestedName("Producto"));
                }
            }

            public Rol.FieldsClass RolUsuario
            {
                get
                {
                    return new Rol.FieldsClass(GetNestedName("RolUsuario"));
                }
            }

            public OperandProperty SLA
            {
                get
                {
                    return new OperandProperty(GetNestedName("SLA"));
                }
            }
        }
    }
}