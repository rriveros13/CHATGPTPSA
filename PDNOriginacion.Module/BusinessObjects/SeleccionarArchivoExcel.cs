using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using ITTI;

namespace PDNOriginacion.Module.BusinessObjects
{
    [NonPersistent]
    public class SeleccionarArchivoExcel : BaseObject
    {
        static FieldsClass _Fields;
        private ITTI.FileDataITTI archivo;

        public SeleccionarArchivoExcel(Session session) : base(session)
        {
        }

        public override void AfterConstruction() => base.AfterConstruction();

        public ITTI.FileDataITTI Archivo
        {
            get => archivo;
            set => SetPropertyValue(nameof(Archivo), ref archivo, value);
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

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public PersistentBase.FieldsClass Archivo
            {
                get
                {
                    return new PersistentBase.FieldsClass(GetNestedName("Archivo"));
                }
            }
        }
    }
}
