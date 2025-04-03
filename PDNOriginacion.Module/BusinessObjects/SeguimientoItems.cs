using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(CuestionarioItem))]
    public class SeguimientoItems : BaseObject
    {
        static FieldsClass _Fields;
        private string _comentarios;
        private CuestionarioItem _item;
        private bool _respuesta;
        DateTime fechaAgenda;

        public SeguimientoItems(Session session) : base(session)
        {
        }

        [Size(500)]
        public string Comentarios
        {
            get => _comentarios;
            set => SetPropertyValue(nameof(Comentarios), ref _comentarios, value);
        }

        [NonPersistent]
        public DateTime FechaAgenda
        {
            get => fechaAgenda;
            set => SetPropertyValue(nameof(FechaAgenda), ref fechaAgenda, value);
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

        //[Association("Seguimiento-Items")]
        //public Seguimiento Seguimiento
        //{
        //    get => seguimiento;
        //    set => SetPropertyValue(nameof(Seguimiento), ref seguimiento, value);
        //}
        [Appearance("", Enabled = false, Context = "Any")]
        public CuestionarioItem Item
        {
            get => _item;
            set => SetPropertyValue(nameof(Item), ref _item, value);
        }

        [CaptionsForBoolValues("S", "N")]
        public bool Respuesta
        {
            get => _respuesta;
            set => SetPropertyValue(nameof(Respuesta), ref _respuesta, value);
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

            public CuestionarioItem.FieldsClass Item
            {
                get
                {
                    return new CuestionarioItem.FieldsClass(GetNestedName("Item"));
                }
            }

            public OperandProperty Respuesta
            {
                get
                {
                    return new OperandProperty(GetNestedName("Respuesta"));
                }
            }
        }
    }
}