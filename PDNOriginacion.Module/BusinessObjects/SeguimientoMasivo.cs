using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using System;
using System.ComponentModel;
using System.Linq;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(Fecha))]
    public class SeguimientoMasivo : BaseObject, IObjectSpaceLink
    {
        static FieldsClass _Fields;
        private string comentarios;
        private DateTime fecha;
        private Usuario usuario;
        private ITTI.FileDataITTI archivo;

        public SeguimientoMasivo(Session session) : base(session)
        {

        }

        [ModelDefault("DisplayFormat", @"{0: dd/MM/yyyy HH:mm:ss}")]
        [EditorAlias("ITTIDateTimeEditor")]
        [XafDisplayName("Fecha de carga")]
        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false)]
        [Appearance("", Enabled = false)]
        public DateTime Fecha
        {
            get => fecha;
            set
            {
                SetPropertyValue(nameof(Fecha), ref fecha, value);
            }
        }

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        [Appearance("", Enabled = false, Context = "Any")]
        [XafDisplayName("Creado Por")]
        public Usuario Usuario
        {
            get => usuario;
            set => SetPropertyValue(nameof(Usuario), ref usuario, value);
        }
    
        [Size(SizeAttribute.Unlimited)]
        public string Comentarios
        {
            get => comentarios;
            set => SetPropertyValue(nameof(Comentarios), ref comentarios, value);
        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            usuario = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
            fecha = DateTime.Now;
        }

        [ImmediatePostData]
        [Appearance("", Enabled = false, Context = "Any")]
        public ITTI.FileDataITTI Archivo
        {
            get => archivo;
            set
            {
                SetPropertyValue(nameof(Archivo), ref archivo, value);
            }
        }

        [Association("Seguimiento-SeguimientoMasivo")]
        public XPCollection<Seguimiento> Seguimientos => GetCollection<Seguimiento>(nameof(Seguimientos));

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

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public MotivoSeguimiento.FieldsClass MotivoSeguimiento
            {
                get
                {
                    return new MotivoSeguimiento.FieldsClass(GetNestedName("MotivoSeguimiento"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
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

            public OperandProperty Solicitudes
            {
                get
                {
                    return new OperandProperty(GetNestedName("Solicitudes"));
                }
            }

            public Telefono.FieldsClass TelefonoContactado
            {
                get
                {
                    return new Telefono.FieldsClass(GetNestedName("TelefonoContactado"));
                }
            }

            public OperandProperty TipoPersona
            {
                get
                {
                    return new OperandProperty(GetNestedName("TipoPersona"));
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
