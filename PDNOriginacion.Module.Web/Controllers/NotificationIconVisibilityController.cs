using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class NotificationIconVisibilityController : WindowController
    {
        public NotificationIconVisibilityController()
        {
            InitializeComponent();
            TargetWindowType = WindowType.Main;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            bool notificationIconVisible = SecuritySystem.CurrentUserName == "Admin";
            Frame.GetController<NotificationsController>().ShowNotificationsAction.Active["ByUser"] = notificationIconVisible;
        }
        protected override void OnDeactivated()
        {
            base.OnDeactivated();
        }
    }
}
