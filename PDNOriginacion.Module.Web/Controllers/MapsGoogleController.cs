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
using DevExpress.ExpressApp.SystemModule;
using DevExpress.ExpressApp.Templates;
using DevExpress.ExpressApp.Utils;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;
using PDNOriginacion.Module.BusinessObjects;

namespace PDNOriginacion.Module.Web.Controllers
{
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppViewControllertopic.aspx.
    public partial class MapsGoogleController : ViewController
    {
        public MapsGoogleController()
        {
            InitializeComponent();
            AbrirGMaps.Active["SinGeoCord"] = false;
            AbrirGMapsPopup.Active["SinGeoCord"] = false;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            View.CurrentObjectChanged += View_CurrentObjectChanged;
            View_CurrentObjectChanged(View, new EventArgs());
        }

        private void View_CurrentObjectChanged(object sender, EventArgs e)
        {
            var curObject = View.CurrentObject;
            if (curObject is Direccion || curObject is Adjunto || curObject is GeoLocalizacion)
            {
                GeoLocalizacion loc = null;

                if (curObject is Direccion) loc = ((Direccion)curObject).Localizacion;
                if (curObject is Adjunto) loc = ((Adjunto)curObject).GeoLocalizacion;
                if (curObject is GeoLocalizacion) loc = (GeoLocalizacion)curObject;

                if (loc != null)
                {
                    AbrirGMaps.Active.RemoveItem("SinGeoCord");
                    AbrirGMapsPopup.Active.RemoveItem("SinGeoCord");

                    string url = $"http://maps.google.com/maps?&z=17&mrt=yp&t=h&q={loc.Coordenadas}";
                    //https://maps.google.com/maps?&z={INSERT_MAP_ZOOM}&mrt={INSERT_TYPE_OF_SEARCH}&t={INSERT_MAP_TYPE}&q={INSERT_MAP_LAT_COORDINATES}+{INSERT_MAP_LONG_COORDINAT
                    //https://moz.com/ugc/everything-you-never-wanted-to-know-about-google-maps-parameters
                    AbrirGMaps.SetClientScript($"window.open('{url}', '_blank')", false);
                    AbrirGMapsPopup.SetClientScript($"window.open('{url}', '_blank')", false);
                }
                else
                {
                    AbrirGMaps.Active["SinGeoCord"] = false;
                    AbrirGMapsPopup.Active["SinGeoCord"] = false;
                }
            }
            else
            {
                AbrirGMaps.Active["SinGeoCord"] = false;
                AbrirGMapsPopup.Active["SinGeoCord"] = false;
            }
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            // Access and customize the target View control.
        }
        protected override void OnDeactivated()
        {
            View.CurrentObjectChanged -= View_CurrentObjectChanged;
            base.OnDeactivated();
        }
    }
}
