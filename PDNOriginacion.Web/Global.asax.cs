using System;
using System.Configuration;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Web;
using DevExpress.Web;
using PDNOriginacion.Module;
using System.Collections.Generic;
using DevExpress.ExpressApp.Web.Editors.ASPx;
using PDNOriginacion.Module.BusinessObjects;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Web;
using System.Web.Routing;
using GdPicture14;

namespace PDNOriginacion.Web {
    public class Global : System.Web.HttpApplication {
        public Global() {
            InitializeComponent();
        }
        protected void Application_Start(Object sender, EventArgs e) {
            DevExpress.ExpressApp.FrameworkSettings.DefaultSettingsCompatibilityMode = DevExpress.ExpressApp.FrameworkSettingsCompatibilityMode.v20_1;
            /*WebApplication.Instance.SetLanguage("es");
            WebApplication.Instance.SetFormattingCulture("es");*/
            //WebApplication.Instance.CustomizeLanguage += Instance_CustomizeLanguage;
            SecurityAdapterHelper.Enable();
            ASPxWebControl.CallbackError += Application_Error;
            Module.GetClienteUser.Register();
            RouteTable.Routes.RegisterXafRoutes();
            WebApplication.OptimizationSettings.AllowFastProcessListViewRecordActions = false;
            WebApplication.OptimizationSettings.AllowFastProcessObjectsCreationActions = false;

#if EASYTEST
            DevExpress.ExpressApp.Web.TestScripts.TestScriptsManager.EasyTestEnabled = true;
#endif

#if DEBUG
            LicenseManager lm = new LicenseManager();
            lm.RegisterKEY("");
#else
            var lm = new LicenseManager();
            var key = ConfigurationManager.AppSettings["GDPKey"];
            bool act = lm.RegisterKEY(key);
            Tracing.Tracer.LogText($"GDP Activation: {act.ToString()}"); 
#endif
        }

        //private void Instance_CustomizeLanguage(object sender, CustomizeLanguageEventArgs e) {
        //    var          currentLanguage    = e.LanguageName;
        //    List<string> availableLanguages = new List<string>();
        //    string       languagesValue     = ConfigurationManager.AppSettings["Languages"];
        //    if(languagesValue != null) {
        //        availableLanguages.AddRange(languagesValue.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries));
        //    }
        //    if(!availableLanguages.Contains(currentLanguage)) {
        //        e.LanguageName = "es";
        //    }
        //}

        static void Instance_CustomizeFormattingCulture(
            object sender, CustomizeFormattingCultureEventArgs e) {
            e.FormattingCulture.NumberFormat.CurrencySymbol = "Gs";
        }

        protected void Session_Start(Object sender, EventArgs e) {
            Tracing.Initialize();
            ASPxCriteriaPropertyEditor.EnableReadOnlyParametersEditor = true;
            WebApplication.SetInstance(Session, new PDNOriginacionAspNetApplication());
            DevExpress.ExpressApp.Web.Templates.DefaultVerticalTemplateContentNew.ClearSizeLimit();
            WebApplication.Instance.LoggedOn    += Instance_LoggedOn;
            WebApplication.Instance.LogonFailed += Instance_LogonFailed;

            WebApplication.Instance.SwitchToNewStyle();
            WebApplication.Instance.CustomizeFormattingCulture += Instance_CustomizeFormattingCulture;
            if(ConfigurationManager.ConnectionStrings["ConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            }

            XpoDefault.TrackPropertiesModifications = true;

#if EASYTEST
            if(ConfigurationManager.ConnectionStrings["EasyTestConnectionString"] != null) {
                WebApplication.Instance.ConnectionString = ConfigurationManager.ConnectionStrings["EasyTestConnectionString"].ConnectionString;
            }
#endif
#if DEBUG
            if(System.Diagnostics.Debugger.IsAttached && WebApplication.Instance.CheckCompatibilityType == CheckCompatibilityType.DatabaseSchema) {
                WebApplication.Instance.DatabaseUpdateMode = DatabaseUpdateMode.UpdateDatabaseAlways;
            }
#endif

            ((SecurityStrategy)WebApplication.Instance.Security).CustomizeRequestProcessors +=
                delegate (object s, CustomizeRequestProcessorsEventArgs args) {
                List<IOperationPermission> result = new List<IOperationPermission>();
                if (s is SecurityStrategyComplex security)
                {
                    if (security.User is Usuario user)
                    {
                        foreach (var rol in user.RolesUsuario)
                        {
                            var rolUsuario = (Rol)rol;
                            if (rolUsuario.CanExport)
                            {
                                result.Add(new ExportPermission());
                            }
                        }
                    }
                }
                IPermissionDictionary permissionDictionary = new PermissionDictionary(result);
                args.Processors.Add(typeof(ExportPermissionRequest), new ExportPermissionRequestProcessor(permissionDictionary));
            };

            WebApplication.Instance.Setup();
            WebApplication.Instance.Start();
        }

        private static void Instance_LoggedOn(object sender, LogonEventArgs e)
        {
            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            
            ReflectionDictionary dict = new ReflectionDictionary();
            dict.QueryClassInfo(typeof(LogonAudit));
            String connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (IDataLayer dataLayer = XpoDefault.GetDataLayer(connectionString, dict, AutoCreateOption.DatabaseAndSchema)) {
                using(Session session = new Session(dataLayer)) {
                    session.BeginTransaction();
                    LogonAudit LogRecord = new LogonAudit(session) { Evento = "Logon", Status = "Success", Usuario = SecuritySystem.CurrentUserName, Fecha = DateTime.Now, SysName = typeof(PDNOriginacionAspNetApplication).Name, Workstation = ip };
                    LogRecord.Save();
                    Usuario u = (Usuario)session.GetObjectByKey(typeof(Usuario), SecuritySystem.CurrentUserId);
                    u.LastLogon = DateTime.Now;
                    u.Save();
                    session.CommitTransaction();
                }
                dataLayer.Connection.Close();
            }

            //string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            //if (string.IsNullOrEmpty(ip))
            //{
            //    ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            //}

            //IObjectSpace os        = ((XafApplication)sender).CreateObjectSpace();
            //LogonAudit   LogRecord = os.CreateObject<LogonAudit>();
            //LogRecord.Evento      = "Logon";
            //LogRecord.Status      = "Success";
            //LogRecord.Usuario     = SecuritySystem.CurrentUserName;
            //LogRecord.Fecha       = DateTime.Now;
            //LogRecord.SysName     = typeof(PDNOriginacionAspNetApplication).Name;
            //LogRecord.Workstation = ip;
            //Usuario u = (Usuario)os.GetObjectByKey(typeof(Usuario), SecuritySystem.CurrentUserId);
            //u.LastLogon = DateTime.Now;
            //u.Save();
            //os.CommitChanges();
        }

        private static void Instance_LogonFailed(object sender, LogonFailedEventArgs e)
        {

            string ip = System.Web.HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (string.IsNullOrEmpty(ip))
            {
                ip = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            
            ReflectionDictionary dict = new ReflectionDictionary();
            dict.QueryClassInfo(typeof(LogonAudit));
            String connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (IDataLayer dataLayer = XpoDefault.GetDataLayer(connectionString, dict, AutoCreateOption.DatabaseAndSchema)) {
                using(Session session = new Session(dataLayer)) {
                    session.BeginTransaction();
                    LogonAudit LogRecord = new LogonAudit(session) { Evento = "Logon", Status = "Failed", Usuario = ((AuthenticationStandardLogonParameters)(e.LogonParameters)).UserName, Fecha = DateTime.Now, SysName = typeof(PDNOriginacionAspNetApplication).Name, Workstation = ip };
                    LogRecord.Save();
                    session.CommitTransaction();
                }
                dataLayer.Connection.Close();
            }
        }
      
        protected void Application_BeginRequest(Object sender, EventArgs e) {
        }
        protected void Application_EndRequest(Object sender, EventArgs e) {
        }
        protected void Application_AuthenticateRequest(Object sender, EventArgs e) {
        }
        protected void Application_Error(Object sender, EventArgs e) {
            ErrorHandling.Instance.ProcessApplicationError();
        }
        protected void Session_End(Object sender, EventArgs e) {
            WebApplication.LogOff(Session);
            WebApplication.DisposeInstance(Session);
        }
        protected void Application_End(Object sender, EventArgs e) {
        }
        #region Web Form Designer generated code
        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
        }
        #endregion
    }
}
