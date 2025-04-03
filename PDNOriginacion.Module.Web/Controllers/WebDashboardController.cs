using System;
using System.Linq;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Web;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Dashboards.Web;
using DevExpress.Persistent.Base;

namespace PDNOriginacion.Module.Web.Controllers
{
    public class WebDashboardController : ObjectViewController<DetailView, IDashboardData>
    {
        private WebDashboardViewerViewItem dashboardViewerViewItem;
        protected override void OnActivated()
        {
            base.OnActivated();
            dashboardViewerViewItem = View.FindItem("DashboardViewer") as WebDashboardViewerViewItem;
            if (dashboardViewerViewItem != null)
            {
                if (dashboardViewerViewItem.DashboardControl != null)
                {
                    //SetHeight(dashboardViewerViewItem.DashboardControl);
                    //dashboardViewerViewItem.DashboardControl.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());
                    //dashboardViewerViewItem.DashboardControl.AllowExecutingCustomSql = true;
                    
                }
                else
                {
                    dashboardViewerViewItem.ControlCreated += DashboardViewerViewItem_ControlCreated;
                }
            }
        }

        private void DashBoardControl_CustomPrameters(object sender, CustomParametersWebEventArgs e)
        {
            var custIDParameter = e.Parameters.FirstOrDefault(p => p.Name == "pAño");
            if (custIDParameter != null)
            {
                if(((object[])(custIDParameter.Value)).Length == 0)
                {
                    custIDParameter.Value = DateTime.Now.Year;
                }
               
            }
        }

        private void DashboardViewerViewItem_ControlCreated(object sender, EventArgs e)
        {
            //Trae las connection strings del web.config
            //((WebDashboardViewerViewItem)sender).DashboardControl.SetConnectionStringsProvider(new ConfigFileConnectionStringsProvider());

            //Crea las connection strings en MyDataSourceWizardConnectionStringsProvider
            ((WebDashboardViewerViewItem)sender).DashboardControl.SetConnectionStringsProvider(new MyDataSourceWizardConnectionStringsProvider());
            ((WebDashboardViewerViewItem)sender).DashboardControl.CustomParameters += DashBoardControl_CustomPrameters;
        }

        private void SetHeight(ASPxDashboard dashboardControl)
        {
            dashboardControl.Height = 760;
        }
        protected override void OnDeactivated()
        {
            if (dashboardViewerViewItem != null)
            {
                dashboardViewerViewItem.ControlCreated -= DashboardViewerViewItem_ControlCreated;
                if(dashboardViewerViewItem.DashboardControl != null)
                {
                    dashboardViewerViewItem.DashboardControl.CustomParameters -= DashBoardControl_CustomPrameters;
                }
                dashboardViewerViewItem = null;
            }
            base.OnDeactivated();
        }
    }
}
