using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Web.Templates.ActionContainers;
using DevExpress.Web;
using System.Configuration;
using System.Linq;
//...
public class WebCustomizeNavBarController : WindowController
{
    public WebCustomizeNavBarController()
    {
        TargetWindowType = WindowType.Main;
    }
    protected override void OnActivated()
    {
        base.OnActivated();
        Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction.CustomizeControl +=
ShowNavigationItemAction_CustomizeControl;
    }
    private void ShowNavigationItemAction_CustomizeControl(object sender,
  CustomizeControlEventArgs e)
    {
        string codigoInstancia = ConfigurationManager.AppSettings["CodigoInstancia"];
        ASPxNavBar navBar = e.Control as ASPxNavBar;
        if (navBar != null)
        {
            #region Solicitudes_General
            NavBarItem itemSolGeneral = navBar.Groups.Where(g => g.Name == "Gestión").FirstOrDefault().Items.Where(i => i.Name.Contains("Solicitudes_General")).FirstOrDefault();

            if (itemSolGeneral != null) 
            {
                itemSolGeneral.ClientVisible = codigoInstancia != "SERSA";
            }
            #endregion Solicitudes_General

            #region Solicitudes_SERSA
            NavBarItem itemSolSersa = navBar.Groups.Where(g => g.Name == "Gestión").FirstOrDefault().Items.Where(i => i.Name.Contains("Solicitudes_SERSA")).FirstOrDefault();

            if (itemSolSersa != null)
            {
                itemSolSersa.ClientVisible = codigoInstancia == "SERSA";
            }
            #endregion Solicitudes_SERSA
        }
        else
        {
            ASPxTreeView mainTreeView = e.Control as ASPxTreeView;
            if (mainTreeView != null)
            {
                // Customize the main ASPxTreeView control.
                mainTreeView.ShowExpandButtons = false;
            }
        }
    }
    protected override void OnDeactivated()
    {
        base.OnDeactivated();
        Frame.GetController<ShowNavigationItemController>().ShowNavigationItemAction.CustomizeControl -=
ShowNavigationItemAction_CustomizeControl;
    }
}