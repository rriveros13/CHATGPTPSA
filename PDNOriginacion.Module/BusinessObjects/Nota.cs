using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using ITTI;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultProperty("Comentario")]
    [ImageName("BO_Note")]
    // Specify more UI options using a declarative approach (http://documentation.devexpress.com/#Xaf/CustomDocument2701).
    public class Nota : BaseObject
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).

        static FieldsClass _Fields;
        private ITTI.FileDataITTI _adjunto;
        private string _comentario;
        private DateTime _fecha = DateTime.Now;
        private Usuario _usuariocreacion;
        private Usuario _usuariodestino;

        public Nota(Session session) : base(session)
        {
            
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _usuariocreacion = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
        }


        public ITTI.FileDataITTI Adjunto
        {
            get => _adjunto;
            set => SetPropertyValue(nameof(Adjunto), ref _adjunto, value);
        }

        [Size(SizeAttribute.Unlimited)]
        public string Comentario
        {
            get => _comentario;
            set => SetPropertyValue(nameof(Comentario), ref _comentario, value);
        }

        [Appearance("Nota-Fecha", AppearanceItemType = "ViewItem", TargetItems = nameof(Fecha), Context = nameof(DetailView), Enabled = false)]
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

        [Appearance("Nota-UsuarioCreacion", AppearanceItemType = "ViewItem", TargetItems = nameof(UsuarioCreacion), Context = nameof(DetailView), Enabled = false)]
        public Usuario UsuarioCreacion
        {
            get => _usuariocreacion;
            set => SetPropertyValue(nameof(UsuarioCreacion), ref _usuariocreacion, value);
        }

        public Usuario UsuarioDestino
        {
            get => _usuariodestino;
            set => SetPropertyValue(nameof(UsuarioDestino), ref _usuariodestino, value);
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public PersistentBase.FieldsClass Adjunto
            {
                get
                {
                    return new PersistentBase.FieldsClass(GetNestedName("Adjunto"));
                }
            }

            public OperandProperty Comentario
            {
                get
                {
                    return new OperandProperty(GetNestedName("Comentario"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public Usuario.FieldsClass UsuarioCreacion
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("UsuarioCreacion"));
                }
            }

            public Usuario.FieldsClass UsuarioDestino
            {
                get
                {
                    return new Usuario.FieldsClass(GetNestedName("UsuarioDestino"));
                }
            }
        }
    }
}

