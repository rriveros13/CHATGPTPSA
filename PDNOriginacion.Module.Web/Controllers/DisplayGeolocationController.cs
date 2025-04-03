using System.Collections.Generic;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Maps.Web;
using DevExpress.ExpressApp.Maps.Web.Helpers;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.Persistent.Base;
using PDNOriginacion.Module.BusinessObjects;
using Newtonsoft.Json;

namespace PDNOriginacion.Module.Web.Controllers {
    public class DisplayGeolocationController : ObjectViewController<ListView, DeviceLocation> {
        protected override void OnActivated() {
            base.OnActivated();

            ResetViewSettingsController resetViewSettingsController = Frame.GetController<ResetViewSettingsController>();
            if(resetViewSettingsController != null) {
                resetViewSettingsController.ResetViewSettingsAction.Active["EnabledInNestedMaps"] = false;
            }
        }
        protected override void OnDeactivated() {
            ResetViewSettingsController resetViewSettingsController = Frame.GetController<ResetViewSettingsController>();
            if(resetViewSettingsController != null) {
                resetViewSettingsController.ResetViewSettingsAction.Active["EnabledInNestedMaps"] = true;
            }

            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated() {
            base.OnViewControlsCreated();

            WebMapsListEditor webMapsListEditor = View.Editor as WebMapsListEditor;
            if(webMapsListEditor != null)
                webMapsListEditor.MapViewer.ClientSideEvents.Customize = GetCustomizeScript();
        }
        private IEnumerable<MapPoint> GetPolylinePoints() {
            List<MapPoint> polylinePoints = new List<MapPoint>();
            foreach(IMapsMarker marker in View.CollectionSource.List) {
                polylinePoints.Add(new MapPoint(marker.Latitude, marker.Longitude));
            }
            return polylinePoints;
        }
        private string GetCustomizeScript() {
            var polylines = JsonConvert.SerializeObject(GetPolylinePoints());
            return string.Format(
    @"function(sender, map) {{
    map.on('ready', function(e) {{
        var googleMap = e.originalMap;
        var clientPathCoordinates = {0};
        
        var iconSettings = {{
            path: google.maps.SymbolPath.FORWARD_CLOSED_ARROW
        }};

        var clientPath = new google.maps.Polyline({{
            path: clientPathCoordinates,
            strokeColor: '#FF0000',
            strokeOpacity: 1.0,
            strokeWeight: 2,
            icons: [{{
                icon: iconSettings,
                repeat: '35px',
                offset: '100%'}}]
        }});

        window.clientPath = clientPath;
        clientPath.setMap(googleMap);
    }});
}}", polylines);
        }
    }
}
