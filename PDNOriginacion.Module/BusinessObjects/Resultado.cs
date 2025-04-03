using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Descripcion")]
    public class Resultado : BaseObject
    {
        static FieldsClass _Fields;
        private string _accion;
        private string _apellidos;
        private Consulta _consulta;
        private string _documento;
        private string _explicacion;
        private DateTime _fecha;
        private string _nombres;
        private Persona _persona;
        private Solicitud _solicitud;
        private Modelo _modelo;
        private bool _procesado;

        public Resultado(Session session) : base(session)
        {
        }

        [Size(50)]
        [ToolTip("Acción recomendada por el modelo")]
        public string Accion
        {
            get => _accion;
            set => SetPropertyValue(nameof(Accion), ref _accion, value);
        }

        [Size(101)]
        public string Apellidos
        {
            get => _apellidos;
            set => SetPropertyValue(nameof(Apellidos), ref _apellidos, value);
        }

        [Association("Consulta-Resultados")]
        public Consulta Consulta
        {
            get => _consulta;
            set => SetPropertyValue(nameof(Consulta), ref _consulta, value);
        }

        [PersistentAlias("Concat('(',Documento,')', ' ', Accion)")]
        public string Descripcion => (string)EvaluateAlias(nameof(Descripcion));

        [Size(30)]
        [ToolTip("Número de documento de la persona")]
        public string Documento
        {
            get => _documento;
            set => SetPropertyValue(nameof(Documento), ref _documento, value);
        }

        [Size(1000)]
        [ToolTip("Explicación de la acción recomendada")]
        public string Explicacion
        {
            get => _explicacion;
            set => SetPropertyValue(nameof(Explicacion), ref _explicacion, value);
        }
        /*private Cheque _cheque;
        [Association("Cheque-Resultados")]
        public Cheque Cheque
        {
        get => _cheque;
        set => SetPropertyValue(nameof(Cheque), ref _cheque, value);
        }*/

        public DateTime Fecha
        {
            get => _fecha;
            set => SetPropertyValue(nameof(Fecha), ref _fecha, value);
        }

        public Modelo Modelo
        {
            get => _modelo;
            set => SetPropertyValue(nameof(Modelo), ref _modelo, value);
        }

        public bool Procesado
        {
            get => _procesado;
            set => SetPropertyValue(nameof(Procesado), ref _procesado, value);
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

        [Size(101)]
        public string Nombres
        {
            get => _nombres;
            set => SetPropertyValue(nameof(Nombres), ref _nombres, value);
        }

        [Association("Persona-Resultados")]
        public Persona Persona
        {
            get => _persona;
            set => SetPropertyValue(nameof(Persona), ref _persona, value);
        }

        [Association("Solicitud-Resultados")]
        public Solicitud Solicitud
        {
            get => _solicitud;
            set => SetPropertyValue(nameof(Solicitud), ref _solicitud, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Accion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Accion"));
                }
            }

            public OperandProperty Apellidos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Apellidos"));
                }
            }

            public Consulta.FieldsClass Consulta
            {
                get
                {
                    return new Consulta.FieldsClass(GetNestedName("Consulta"));
                }
            }

            public OperandProperty Descripcion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Descripcion"));
                }
            }

            public OperandProperty Documento
            {
                get
                {
                    return new OperandProperty(GetNestedName("Documento"));
                }
            }

            public OperandProperty Explicacion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Explicacion"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public OperandProperty Nombres
            {
                get
                {
                    return new OperandProperty(GetNestedName("Nombres"));
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
        }
    }
}
