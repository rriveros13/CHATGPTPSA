using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Layout;
using DevExpress.ExpressApp.Maps.Web;
using DevExpress.ExpressApp.Maps.Web.Helpers;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class MapControllerListV : ViewController<ListView>
    {
        public MapControllerListV()
        {
            InitializeComponent();
            TargetObjectType = typeof(IMapsMarker);
            SimpleAction CenterMap = new SimpleAction(this, "Centrar", PredefinedCategory.Edit);
            CenterMap.Execute += CenterMap_Execute;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            // Perform various tasks depending on the target View.
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            // Unsubscribe from previously subscribed events and release other references and resources.
            base.OnDeactivated();
        }

        private void CenterMap_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            //View.CollectionSource
            DevExpress.ExpressApp.Maps.Web.WebMapsListEditor listEditor = View.Editor as WebMapsListEditor;
            if (listEditor != null)
            {
                MapPoint mp = new MapPoint(-25.284243, -57.564085);
                listEditor.MapViewer.MapSettings.Center = mp;
            }
        }
    }
}
