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
    [DefaultProperty("Evento")]
    public class LogonAudit : BaseObject
    {
        static FieldsClass _Fields;
        private string evento;
        private DateTime fecha;
        private string status;
        private string sysname;
        private string usuario;
        private string workstation;

        public LogonAudit(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [Size(20)]
        public string Evento
        {
            get => evento;
            set => SetPropertyValue(nameof(Evento), ref evento, value);
        }

        [ModelDefault("DisplayFormat", @"{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTime Fecha
        {
            get => fecha;
            set => SetPropertyValue(nameof(Fecha), ref fecha, value);
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

        public string Status
        {
            get => status;
            set => SetPropertyValue(nameof(Status), ref status, value);
        }

        public string SysName
        {
            get => sysname;
            set => SetPropertyValue(nameof(SysName), ref sysname, value);
        }

        public string Usuario
        {
            get => usuario;
            set => SetPropertyValue(nameof(Usuario), ref usuario, value);
        }

        public string Workstation
        {
            get => workstation;
            set => SetPropertyValue(nameof(Workstation), ref workstation, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Evento
            {
                get
                {
                    return new OperandProperty(GetNestedName("Evento"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public OperandProperty Status
            {
                get
                {
                    return new OperandProperty(GetNestedName("Status"));
                }
            }

            public OperandProperty SysName
            {
                get
                {
                    return new OperandProperty(GetNestedName("SysName"));
                }
            }

            public OperandProperty Usuario
            {
                get
                {
                    return new OperandProperty(GetNestedName("Usuario"));
                }
            }

            public OperandProperty Workstation
            {
                get
                {
                    return new OperandProperty(GetNestedName("Workstation"));
                }
            }
        }
    }
}
