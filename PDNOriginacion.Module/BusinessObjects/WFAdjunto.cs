using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(Adjunto))]
    public class WFAdjunto : BaseObject
    {
        static FieldsClass _Fields;
        private EstadoSolicitud _estadoDestino;
        private Producto _producto;
        private TipoAdjunto _tipoAdjunto;
        private bool _validar;

        public WFAdjunto(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        //[RuleRequiredField(DefaultContexts.Save)]
        public EstadoSolicitud EstadoDestino
        {
            get => _estadoDestino;
            set => SetPropertyValue(nameof(EstadoDestino), ref _estadoDestino, value);
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

        //[RuleRequiredField(DefaultContexts.Save)]
        [Association("Producto-Adjuntos")]
        public Producto Producto
        {
            get => _producto;
            set => SetPropertyValue(nameof(Producto), ref _producto, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public TipoAdjunto TipoAdjunto
        {
            get => _tipoAdjunto;
            set => SetPropertyValue(nameof(TipoAdjunto), ref _tipoAdjunto, value);
        }

        public bool Validar
        {
            get => _validar;
            set => SetPropertyValue(nameof(Validar), ref _validar, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public EstadoSolicitud.FieldsClass EstadoDestino
            {
                get
                {
                    return new EstadoSolicitud.FieldsClass(GetNestedName("EstadoDestino"));
                }
            }

            public Producto.FieldsClass Producto
            {
                get
                {
                    return new Producto.FieldsClass(GetNestedName("Producto"));
                }
            }

            public TipoAdjunto.FieldsClass TipoAdjunto
            {
                get
                {
                    return new TipoAdjunto.FieldsClass(GetNestedName("TipoAdjunto"));
                }
            }

            public OperandProperty Validar
            {
                get
                {
                    return new OperandProperty(GetNestedName("Validar"));
                }
            }
        }

        //private string _adjunto;
        ////[RuleRequiredField(DefaultContexts.Save)]
        //public string Adjunto
        //{
        //    get => _adjunto;
        //    set => SetPropertyValue(nameof(Adjunto), ref _adjunto, value);
        //}
    }
}