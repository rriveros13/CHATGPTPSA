using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.Security;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
//using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices.Protocols;
using System.Linq;
using System.Net;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty(nameof(UserName))]
    public class Usuario : BaseObject, INotifyPropertyChanged, IPermissionPolicyUser, ISecurityUser, IAuthenticationStandardUser, IObjectSpaceLink, ICloneable
    {
        public const string ruleId_RoleRequired = "Rol Requerido";
        public const string ruleId_UserNameIsUnique = "Nombre de usuario es unico";
        public const string ruleId_UserNameRequired = "Nombre de usuario requerido";

        private Sucursal sucursal;
        private Cliente cliente;
        private DateTime lastLogon;
        private bool passwordNuncaExpira;
        private bool changePasswordOnFirstLogon;
        private bool validarPassAD;
        private string usuExterno;

        public Usuario(Session session) : base(session)
        {
            string adDefault = ConfigurationManager.AppSettings["ADDefault"];
            validarPassAD = adDefault == "S";
        }

        public event PropertyChangedEventHandler PropertyChanged;

        bool IAuthenticationStandardUser.ComparePassword(string password)
        {
            if (!ValidarPassAD)
                return PasswordCryptographer.VerifyHashedPasswordDelegate(StoredPassword, password);
            else
                return ValidarPasswordAD(password);

        }

        private bool ValidarPasswordAD(string password)
        {
            bool validation;
            try
            {
                string adDomain = ConfigurationManager.AppSettings["ADDomain"];
                string adServer = ConfigurationManager.AppSettings["ADServer"];
                if (string.IsNullOrEmpty(adDomain) || string.IsNullOrEmpty(adServer))
                    return false;
                LdapConnection lcon = new LdapConnection
                        (new LdapDirectoryIdentifier(adServer, false, false));
                NetworkCredential nc = new NetworkCredential(UserName, password, adDomain);
                lcon.Credential = nc;
                lcon.AuthType = AuthType.Kerberos;
                lcon.Bind(nc);
                validation = true;
            }
            catch (LdapException e)
            {
                validation = false;
            }
            return validation;
        }

        private bool ValidarPasswordADv2(string password)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, "itc.ad"))
            {
                return pc.ValidateCredentials(UserName, password);
            }
        }

        void IAuthenticationStandardUser.SetPassword(string password)
        {
            bool pUsed = false;

            foreach (LPassword userLPassword in LPasswords)
            {
                if (LPassword.ComparePassword(userLPassword.LastPassword, password))
                {
                    pUsed = true;
                }
            }

            if (!pUsed)
            {
                Session.BeginTransaction();
                LPassword lp = new LPassword(Session)
                {
                    PWChangeDate = DateTime.Now,
                    Usuario = this,
                    LastPassword = PasswordCryptographer.HashPasswordDelegate(password)
                };
                lp.Save();
                LPasswords.Add(lp);
                StoredPassword = PasswordCryptographer.HashPasswordDelegate(password);
                Session.CommitTransaction();
            }
            OnChanged(nameof(StoredPassword));
        }

        public static Usuario GetUsuarioLibre(string rol, Session sesion)
        {
            XPCollection<Usuario> usuarios = new XPCollection<Usuario>(sesion);
            var usuario = usuarios.Where(u => u.RolesUsuario.Where(r => r.Name == rol).Count() > 0)
                                                    .OrderBy(o => o.TotalTareasAsignadas).FirstOrDefault();
            return usuario;
        }

        IEnumerable<IPermissionPolicyRole> IPermissionPolicyUser.Roles => RolesUsuario.OfType<IPermissionPolicyRole>();

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            passwordNuncaExpira = false;
        }

        public object Clone() => this.MemberwiseClone();

        public void SetPassword(string password) => StoredPassword = PasswordCryptographer.HashPasswordDelegate(password);

        [Appearance("", Visibility = ViewItemVisibility.Hide, Criteria = "ValidarPassAD", Context = nameof(DetailView))]
        public bool ChangePasswordOnFirstLogon
        {
            get => changePasswordOnFirstLogon;
            set
            {
                changePasswordOnFirstLogon = value;
                if (PropertyChanged != null)
                {
                    PropertyChangedEventArgs args = new PropertyChangedEventArgs("ChangePasswordOnFirstLogon");
                    PropertyChanged(this, args);
                }
            }
        }

        [Association("Cliente-Usuario", typeof(Cliente))]
        public Cliente Cliente
        {
            get => cliente;
            set => SetPropertyValue(nameof(Cliente), ref cliente, value);
        }

        [Association("Sucursal-Usuario", typeof(Sucursal))]
        public Sucursal Sucursal
        {
            get => sucursal;
            set => SetPropertyValue(nameof(Sucursal), ref sucursal, value);
        }

        [ImmediatePostData(true)]
        public bool ValidarPassAD
        {
            get => validarPassAD;
            set => SetPropertyValue(nameof(ValidarPassAD), ref validarPassAD, value);
        }

        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [NonCloneable]
        public int ID { get; protected set; }

        public bool IsActive { get; set; }

        [NonCloneable]
        [ModelDefault("DisplayFormat", @"{0:dd/MM/yyyy HH:mm:ss}")]
        [XafDisplayName("Último Inicio de Sesión")]
        [ModelDefault("AllowEdit", "false")]
        public DateTime LastLogon
        {
            get => lastLogon;
            set => SetPropertyValue(nameof(LastLogon), ref lastLogon, value);
        }

        private string nombreCompleto;
        [NonCloneable]
        public string NombreCompleto
        {
            get => nombreCompleto;
            set => SetPropertyValue(nameof(NombreCompleto), ref nombreCompleto, value);
        }

        private string correo;
        [NonCloneable]
        public string Correo
        {
            get => correo;
            set => SetPropertyValue(nameof(Correo), ref correo, value);
        }

        private string telefono;
        [NonCloneable]
        public string Telefono
        {
            get => telefono;
            set => SetPropertyValue(nameof(Correo), ref telefono, value);
        }

       // [RuleRequiredField]
        [Size(50)]
        public string UsuarioExterno
        {
            get => usuExterno;
            set => SetPropertyValue(nameof(UsuarioExterno), ref usuExterno, value);
        }

        [NonCloneable]
        [Browsable(false), SecurityBrowsable]
        [Association("Usuario-LPasswords")]
        public XPCollection<LPassword> LPasswords => GetCollection<LPassword>(nameof(LPasswords));

        [Browsable(false), VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace
        {
            get;
            set;
        }

        [Appearance("", Visibility = ViewItemVisibility.Hide, Criteria = "ValidarPassAD", Context = nameof(DetailView))]
        public bool PasswordNuncaExpira
        {
            get => passwordNuncaExpira;
            set => SetPropertyValue(nameof(PasswordNuncaExpira), ref passwordNuncaExpira, value);
        }

        public virtual IList<PermissionPolicyRole> Roles { get; set; }

        [Association("Usuario-Rol")]
        [RuleRequiredField("UsuarioNewRoleIsRequired", DefaultContexts.Save, TargetCriteria = nameof(IsActive), CustomMessageTemplate = "Los usuarios activos deben tener por lo menos un RolUsuario asignado")]
        public XPCollection<Rol> RolesUsuario => GetCollection<Rol>(nameof(RolesUsuario));

        [NonCloneable]
        [Browsable(false), SecurityBrowsable]
        public string StoredPassword { get; set; }

        [ModelDefault("AllowEdit", "False")]
        [Association("TareasFinalizadas-Usuario")]
        [Browsable(false), SecurityBrowsable]
        public XPCollection<Tarea> TareasFinalizadas => GetCollection<Tarea>(nameof(TareasFinalizadas));

        [NonCloneable]
        [ModelDefault("AllowEdit", "False")]
        [Association("TareaReservada-Usuario")]
        [Browsable(false), SecurityBrowsable]
        public XPCollection<Tarea> TareasReservadas => GetCollection<Tarea>(nameof(TareasReservadas));

        [PersistentAlias("TareasReservadas[Estado = 2 and IsNull(FechaCierre)].Count()")]
        public int? TotalTareasAsignadas => EvaluateAlias(nameof(TotalTareasAsignadas)) as int?;

        [PersistentAlias("TareasReservadas[Vencida And FechaCierre=null].Count()")]
        public int? TotalTareasVencidas => EvaluateAlias(nameof(TotalTareasVencidas)) as int?;

        [RuleRequiredField(ruleId_UserNameRequired, "Save", "The user name must not be empty")]
        [RuleUniqueValue(ruleId_UserNameIsUnique, "Save", "The login with the entered UserName was already registered within the system")]
        public string UserName { get; set; }

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

            public OperandProperty ruleId_RoleRequired
            {
                get
                {
                    return new OperandProperty(GetNestedName("ruleId_RoleRequired"));
                }
            }

            public OperandProperty ruleId_UserNameIsUnique
            {
                get
                {
                    return new OperandProperty(GetNestedName("ruleId_UserNameIsUnique"));
                }
            }

            public OperandProperty ruleId_UserNameRequired
            {
                get
                {
                    return new OperandProperty(GetNestedName("ruleId_UserNameRequired"));
                }
            }

            public OperandProperty ChangePasswordOnFirstLogon
            {
                get
                {
                    return new OperandProperty(GetNestedName("ChangePasswordOnFirstLogon"));
                }
            }

            public Cliente.FieldsClass Cliente
            {
                get
                {
                    return new Cliente.FieldsClass(GetNestedName("Cliente"));
                }
            }

            public OperandProperty ID
            {
                get
                {
                    return new OperandProperty(GetNestedName("ID"));
                }
            }

            public OperandProperty IsActive
            {
                get
                {
                    return new OperandProperty(GetNestedName("IsActive"));
                }
            }

            public OperandProperty LastLogon
            {
                get
                {
                    return new OperandProperty(GetNestedName("LastLogon"));
                }
            }

            public OperandProperty LPasswords
            {
                get
                {
                    return new OperandProperty(GetNestedName("LPasswords"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public OperandProperty PasswordNuncaExpira
            {
                get
                {
                    return new OperandProperty(GetNestedName("PasswordNuncaExpira"));
                }
            }

            public OperandProperty Roles
            {
                get
                {
                    return new OperandProperty(GetNestedName("Roles"));
                }
            }

            public OperandProperty RolesUsuario
            {
                get
                {
                    return new OperandProperty(GetNestedName("RolesUsuario"));
                }
            }

            public OperandProperty StoredPassword
            {
                get
                {
                    return new OperandProperty(GetNestedName("StoredPassword"));
                }
            }

            public OperandProperty TareasFinalizadas
            {
                get
                {
                    return new OperandProperty(GetNestedName("TareasFinalizadas"));
                }
            }

            public OperandProperty TareasReservadas
            {
                get
                {
                    return new OperandProperty(GetNestedName("TareasReservadas"));
                }
            }

            public OperandProperty TotalTareasAsignadas
            {
                get
                {
                    return new OperandProperty(GetNestedName("TotalTareasAsignadas"));
                }
            }

            public OperandProperty TotalTareasVencidas
            {
                get
                {
                    return new OperandProperty(GetNestedName("TotalTareasVencidas"));
                }
            }

            public OperandProperty UserName
            {
                get
                {
                    return new OperandProperty(GetNestedName("UserName"));
                }
            }

            public OperandProperty Auditoria
            {
                get
                {
                    return new OperandProperty(GetNestedName("Auditoria"));
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
