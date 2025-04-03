using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Web;
using DevExpress.ExpressApp.Web.Templates;
using PDNOriginacion.Module.BusinessObjects;
using System;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class MapControllerDetailV : ViewController, IXafCallbackHandler
    {
        private DetailView v;

        public MapControllerDetailV()
        {
            InitializeComponent();

            string getPositionClientScript = @"
function idContains(selector, text) {
  var elements = document.querySelectorAll(selector);
  return [].filter.call(elements, function(element){
    return RegExp(text).test(element.id);
  });
}

function setLocalizedValueToInput(input, value) {
    var curValue = input.value.toString();
    var newValue = value.toString();    
    var isCommaAsFloatPoint = curValue.indexOf(',') > -1;

    if (isCommaAsFloatPoint)
        newValue = newValue.replace('.', ',');

    input.value = newValue;
}

function showMap(position) {
  var latitude = position.coords.latitude;
  var latInputs = idContains('input', 'Latitude_Edit(?!.*Info$)');  
  if (latInputs.length == 1) { setLocalizedValueToInput(latInputs[0], latitude); }

  var longitude = position.coords.longitude;
  var lonInputs = idContains('input', 'Longitude_Edit(?!.*Info$)'); 
  if (lonInputs.length == 1) { setLocalizedValueToInput(lonInputs[0], longitude); 
  RaiseXafCallback(globalCallbackControl, 'MyScript', '' + position.coords.latitude + ',' + position.coords.longitude, '', false)}
}

navigator.geolocation.getCurrentPosition(showMap);
";

            PosicionActual.SetClientScript(getPositionClientScript, false);
        }

        private void DetailView_ViewEditModeChanged(object sender, EventArgs e) => UpdateActionState();
        private void UpdateActionState() => PosicionActual.Enabled["EditMode"] = v.ViewEditMode == ViewEditMode.Edit;

        protected override void OnActivated()
        {
            base.OnActivated();
            v = (DetailView)View;

            v.ViewEditModeChanged += DetailView_ViewEditModeChanged;
            UpdateActionState();
        }
        protected override void OnDeactivated()
        {
            v.ViewEditModeChanged -= DetailView_ViewEditModeChanged;
            base.OnDeactivated();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            CallbackManager.RegisterHandler("MyScript", this);
        }

        protected XafCallbackManager CallbackManager => ((ICallbackManagerHolder)WebWindow.CurrentRequestPage).CallbackManager;

        public void ProcessAction(string parameter)
        {
            GeoLocalizacion geo = (GeoLocalizacion)View.CurrentObject;
            CallbackManager.NeedEndResponse = false;

            string[] vcord = parameter.Split(',');
            bool blat = double.TryParse(vcord[0], out double lat);
            bool blon = double.TryParse(vcord[1], out double lon);

            if(!blat || !blon)
            {
                return;
            }
            geo.Latitude = lat;
            geo.Longitude = lon;
            geo.Origen = OrigenGeolocalizacion.GPS;
            geo.Save();
            View.Refresh();
        }
    }
}
