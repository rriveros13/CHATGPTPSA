using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Notifications;
using DevExpress.Persistent.Base.General;
using PDNOriginacion.Module.BusinessObjects;
using System.ComponentModel;

namespace PDNOriginacion.Module.Web.Controllers
{
    public class DeleteOnDismissController : ObjectViewController<DetailView, NotificationsObject>
    {
        private NotificationsService service;

        private void Dismiss_Executed(object sender, ActionBaseEventArgs e) => service.ItemsProcessed -=
            Service_ItemsProcessed;
        private void Dismiss_Executing(object sender, CancelEventArgs e) => service.ItemsProcessed +=
            Service_ItemsProcessed;
        private void Service_ItemsProcessed(object sender, NotificationItemsEventArgs e)
        {
            IObjectSpace space = Application.CreateObjectSpace(typeof(Tarea));
            foreach(INotificationItem item in e.NotificationItems)
            {
                if(item.NotificationSource is Tarea)
                {
                    space.Delete(space.GetObject(item.NotificationSource));
                }
            }
            space.CommitChanges();
        }
        private void Snooze_Executed(object sender, ActionBaseEventArgs e) => service.ItemsProcessed -=
            Snooze_ItemsProcessed;
        private void Snooze_Executing(object sender, CancelEventArgs e) => service.ItemsProcessed +=
            Snooze_ItemsProcessed;
        private void Snooze_ItemsProcessed(object sender, NotificationItemsEventArgs e)
        {
            IObjectSpace space = Application.CreateObjectSpace(typeof(Tarea));
            foreach(INotificationItem item in e.NotificationItems)
            {
                if(item.NotificationSource is Tarea)
                {
                    //Tarea t = (Tarea)space.GetObject(item.NotificationSource);
                    //t.IsPostponed = true;
                    //t.AlarmTime = DateTime.Now.AddMinutes(15);
                    //DateTime? tt = t.AlarmTime;
                }
            }
            space.CommitChanges();
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            service = Application.Modules.FindModule<NotificationsModule>().NotificationsService;
            NotificationsDialogViewController notificationsDialogViewController =
                Frame.GetController<NotificationsDialogViewController>();

            if(service != null && notificationsDialogViewController != null)
            {
                notificationsDialogViewController.Dismiss.Executing += Dismiss_Executing;
                notificationsDialogViewController.Dismiss.Executed += Dismiss_Executed;
                notificationsDialogViewController.Snooze.Executing += Snooze_Executing;
                notificationsDialogViewController.Snooze.Executed += Snooze_Executed;
            }
        }
        protected override void OnDeactivated()
        {
            NotificationsDialogViewController notificationsDialogViewController = Frame.GetController<NotificationsDialogViewController>();
            if(notificationsDialogViewController != null)
            {
                notificationsDialogViewController.Dismiss.Executing -= Dismiss_Executing;
                notificationsDialogViewController.Dismiss.Executed -= Dismiss_Executed;
                notificationsDialogViewController.Snooze.Executing -= Snooze_Executing;
                notificationsDialogViewController.Snooze.Executed -= Snooze_Executed;
            }
            base.OnDeactivated();
        }
    }
}
