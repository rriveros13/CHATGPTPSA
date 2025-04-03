using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultProperty(nameof(NombreBoton))]
    public class WFAccionConfig : BaseObject
    {
        static FieldsClass _Fields;
        private string _nombreBoton;
        private Producto _productodestino;
        private bool _copiarAdjuntos;
        private bool _copiarInmuebles;

        public WFAccionConfig(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        [Size(30)]
        public string NombreBoton
        {
            get => _nombreBoton;
            set => SetPropertyValue(nameof(NombreBoton), ref _nombreBoton, value);
        }

        public bool CopiarAdjuntos
        {
            get => _copiarAdjuntos;
            set => SetPropertyValue(nameof(CopiarAdjuntos), ref _copiarAdjuntos, value);
        }

        public bool CopiarInmuebles
        {
            get => _copiarInmuebles;
            set => SetPropertyValue(nameof(CopiarInmuebles), ref _copiarInmuebles, value);
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

        //[RuleRequiredField(DefaultContexts.Save)]
        public Producto ProductoDestino
        {
            get => _productodestino;
            set => SetPropertyValue(nameof(Producto), ref _productodestino, value);
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

            public Producto.FieldsClass Producto
            {
                get
                {
                    return new Producto.FieldsClass(GetNestedName("Producto"));
                }
            }
        }
    }
}