using DevExpress.Data.Filtering;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Xpo;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using DevExpress.Persistent.BaseImpl;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(Name))]
    public class Rol : PermissionPolicyRoleBase, IPermissionPolicyRoleWithUsers
    {
        private bool usableEnWorkflow;

        public Rol(Session session) : base(session) { }

        //[Association("RolUsuario-WFTarea")]
        //public XPCollection<WFTarea> TipoTareas => GetCollection<WFTarea>(nameof(TipoTareas));
        public bool CanExport
        {
            get => GetPropertyValue<bool>(nameof(CanExport));
            set => SetPropertyValue<bool>(nameof(CanExport), value);
        }

        public bool UsableEnWorkflow
        {
            get => usableEnWorkflow;
            set => SetPropertyValue(nameof(UsableEnWorkflow), ref usableEnWorkflow, value);
        }

        [Association("Usuario-Rol")]
        public XPCollection<Usuario> Usuarios => GetCollection<Usuario>("Usuarios");

        IEnumerable<IPermissionPolicyUser> IPermissionPolicyRoleWithUsers.Users
        {
            get { return Usuarios.OfType<IPermissionPolicyUser>(); }
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

            public OperandProperty CanExport
            {
                get
                {
                    return new OperandProperty(GetNestedName("CanExport"));
                }
            }

            public OperandProperty UsableEnWorkflow
            {
                get
                {
                    return new OperandProperty(GetNestedName("UsableEnWorkflow"));
                }
            }

            public OperandProperty Usuarios
            {
                get
                {
                    return new OperandProperty(GetNestedName("Usuarios"));
                }
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

        static FieldsClass _Fields;
    }
}
