using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Validation;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using PDNOriginacion.Module.BusinessObjects;
using PDNOriginacion.Module.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text.RegularExpressions;

namespace PDNOriginacion.Module
{
    public sealed partial class PDNOriginacionModule : ModuleBase
    {
        public static string GlbCriteriaDescPersonal;
        public PDNOriginacionModule()
        {
            InitializeComponent();
            BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
            UsuarioEnRol.Register();
            CampoEnProducto.Register();
            CampoObligatorio.Register();
            EvalCriterioMostrar.Register();
            EvalCriterioValidacion.Register();
            GetClienteUser.Register();
            CampoEditable.Register();
            GlbCriteriaDescPersonal = ConfigurationManager.AppSettings["CriteriaDescPersona"];
            if (GlbCriteriaDescPersonal == null)
                // throw new Exception("Parámetro: CriteriaDescPersona no definido");
                GlbCriteriaDescPersonal = "NombrePersona";
        }

        void application_LoggedOn(object sender, LogonEventArgs e)
        {
            NotificationsModule notificationsModule = Application.Modules.FindModule<NotificationsModule>();
            DefaultNotificationsProvider notificationsProvider = notificationsModule.DefaultNotificationsProvider;
            notificationsProvider.CustomizeNotificationCollectionCriteria += notificationsProvider_CustomizeNotificationCollectionCriteria;
        }
        void application_SetupComplete(object sender, EventArgs e)
        {
            ValidationModule module = (ValidationModule)((XafApplication)sender).Modules
                .FindModule(typeof(ValidationModule));
            module?.InitializeRuleSet();
        }
        void notificationsProvider_CustomizeNotificationCollectionCriteria(object sender,
                                                                           CustomizeCollectionCriteriaEventArgs e)
        {
            if(e.Type == typeof(Tarea))
            {
                e.Criteria = CriteriaOperator.Parse("ReservadaPor.Oid = CurrentUserId()");
            }
        }

        public override void CustomizeTypesInfo(ITypesInfo typesInfo)
        {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB)
        {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new[] { updater };
        }
        public override void Setup(XafApplication application)
        {
            base.Setup(application);
            application.LoggedOn += application_LoggedOn;
            application.SetupComplete += application_SetupComplete;
        }
        public override void Setup(ApplicationModulesManager moduleManager)
        {
            base.Setup(moduleManager);
            ValidationRulesRegistrator.RegisterRule(moduleManager,
                                                    typeof(PasswordStrengthCodeRule),
                                                    typeof(IRuleBaseProperties));
        }

        [CodeRule]
        public class PasswordStrengthCodeRule : RuleBase<ChangePasswordOnLogonParameters>
        {
            public PasswordStrengthCodeRule() : base(string.Empty, "ChangePassword") => Properties.SkipNullOrEmptyValues =
                false;
            public PasswordStrengthCodeRule(IRuleBaseProperties properties) : base(properties)
            {
            }

            private static int CalculatePasswordStrength(string pwd, Usuario user)
            {
                Configuracion conf = Configuracion.GetInstance(user.ObjectSpace);

                int weight = 0;

                if(conf.SecurityPolicy.MinPasswordLenght > 0)
                {
                    //Verificar el largo del password, si es menor que lo exigido no permitir
                    if(pwd.Length < conf.SecurityPolicy.MinPasswordLenght)
                    {
                        return weight;
                    }
                }

                //Si esta activado el chequeo de historial de contraseñas hacerlo y no permitir repetir
                if(conf.SecurityPolicy.EnforcePasswordHistory)
                {
                    foreach(LPassword userLPassword in user.LPasswords)
                    {
                        if(LPassword.ComparePassword(userLPassword.LastPassword, pwd))
                        {
                            return weight;
                        }
                    }
                }

                //Si no esta activada la complejidad de contraseña pasar la validacion
                if(!conf.SecurityPolicy.PasswordComplexity)
                {
                    return 3;
                }

                //Validar que la contraseña no centenga el nombre de usuario
                const StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase;
                if(pwd.IndexOf(user.UserName, stringComparison) >= 0)
                {
                    return weight;
                }

                //Verificar que la contraseña tenga al menos un caracter de tres de los conjuntos siguientes
                //mayúsculas, minúsculas, números, símbolos
                Regex rxUpperCase = new Regex("[A-Z]");
                Regex rxLowerCase = new Regex("[a-z]");
                Regex rxNumerals = new Regex("[0-9]");
                Regex rxSymbols = new Regex(@"[$|~=[\]'_+@.\-!%*()#?¿<>:;]");
                Regex rxInvalid = new Regex(@"[^a-zA-Z0-9$|~=[\]'_+@.\-!%*()#?¿<>:;]");

                Match match = rxInvalid.Match(pwd);

                if(match.Success)
                {
                    weight = 0;
                }
                else
                {
                    match = rxUpperCase.Match(pwd);
                    if(match.Success)
                    {
                        ++weight;
                    }

                    match = rxLowerCase.Match(pwd);
                    if(match.Success)
                    {
                        ++weight;
                    }

                    match = rxNumerals.Match(pwd);
                    if(match.Success)
                    {
                        ++weight;
                    }

                    match = rxSymbols.Match(pwd);
                    if(match.Success)
                    {
                        ++weight;
                    }
                }

                return weight;
            }
            bool MatchBanned(string newPassword, Configuracion conf)
            {
                //string ruta = HttpContext.Current.Server.MapPath("~");
                //string archivo = string.Concat(ruta, "BannedPasswords.lst");
                //if (File.Exists(archivo))
                //{
                //    var passwords = File.ReadAllLines(archivo);
                //    return passwords.Any(x=> x == newPassword);
                //}

                if(string.IsNullOrEmpty(conf.SecurityPolicy.BannedPasswords))
                {
                    return false;
                }

                List<string> list = conf.SecurityPolicy.BannedPasswords.Split((char)10).ToList();
                return list.Any(x => x == newPassword);
            }

            protected override bool IsValidInternal(ChangePasswordOnLogonParameters target,
                                                    out string errorMessageTemplate)
            {
                Usuario currentUser = (Usuario)SecuritySystem.CurrentUser;
                currentUser.LPasswords.Reload();

                if(CalculatePasswordStrength(target.NewPassword, currentUser) < 3)
                {
                    errorMessageTemplate = "La contraseña no cuenta con la complejidad necesaria o ya fue utilizada!";
                    return false;
                }

                Configuracion conf = Configuracion.GetInstance(currentUser.ObjectSpace);
                if(MatchBanned(target.NewPassword.ToLower(), conf))
                {
                    errorMessageTemplate = "Contraseña no permitida, es muy común!";
                    return false;
                }

                errorMessageTemplate = string.Empty;

                return true;
            }
        }
    }
}
