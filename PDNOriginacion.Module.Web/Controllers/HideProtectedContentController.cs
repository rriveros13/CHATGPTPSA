using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.Editors;

namespace PDNOriginacion.Module.Controllers
{
    public class HideProtectedContentController : ViewController<ObjectView>
    {
        private AppearanceController appearanceController;

        void appearanceController_CustomApplyAppearance(object sender, ApplyAppearanceEventArgs e)
        {
            if(e.AppearanceObject.Visibility == null || e.AppearanceObject.Visibility == ViewItemVisibility.Show)
            {
                if(View is ListView)
                {
                    if(e.Item is ColumnWrapper)
                    {
                        if(!DataManipulationRight.CanRead(View.ObjectTypeInfo.Type,
                                                          ((ColumnWrapper)e.Item).PropertyName,
                                                          null,
                                                          ((ListView)View).CollectionSource,
                                                          View.ObjectSpace))
                        {
                            e.AppearanceObject.Visibility = ViewItemVisibility.Hide;
                        }
                    }
                }
                if(View is DetailView)
                {
                    if(e.Item is PropertyEditor)
                    {
                        if(!DataManipulationRight.CanRead(View.ObjectTypeInfo.Type,
                                                          ((PropertyEditor)e.Item).PropertyName,
                                                          e.ContextObjects.Length > 0 ? e.ContextObjects[0] : null,
                                                          null,
                                                          View.ObjectSpace))
                        {
                            e.AppearanceObject.Visibility = ViewItemVisibility.Hide;
                        }
                    }
                }
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            appearanceController = Frame.GetController<AppearanceController>();
            if(appearanceController != null)
            {
                appearanceController.CustomApplyAppearance += appearanceController_CustomApplyAppearance;
            }
        }
        protected override void OnDeactivated()
        {
            if(appearanceController != null)
            {
                appearanceController.CustomApplyAppearance -= appearanceController_CustomApplyAppearance;
            }
            base.OnDeactivated();
        }
    }
}
