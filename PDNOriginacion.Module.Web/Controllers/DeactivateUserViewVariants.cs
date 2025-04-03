using System;
using System.ComponentModel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ViewVariantsModule;
using PDNOriginacion.Module.BusinessObjects;
using UserViewVariants;

namespace PDNOriginacion.Module.Web.Controllers
{
    public partial class DeactivateUserViewVariants : ViewController
    {
        public DeactivateUserViewVariants()
        {
            InitializeComponent();
        }
        protected override void OnActivated()
        {
            base.OnActivated();

            View.ModelSaving += View_ModelSaving;

            UserViewVariantsController userViewVariantsController = Frame.GetController<UserViewVariantsController>();
            ChangeVariantController changeVariantController = Frame.GetController<ChangeVariantController>();

            if (userViewVariantsController != null)
            {
                int countCrear = (int)(((Usuario)SecuritySystem.CurrentUser).Session.Evaluate(typeof(Usuario), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("UsuarioEnRol('PCA_PermitirCrearVistasPersonalizadas')")));
                int countVer = (int)(((Usuario)SecuritySystem.CurrentUser).Session.Evaluate(typeof(Usuario), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("UsuarioEnRol('PCA_PermitirVerVistasPersonalizadas')")));
                if (countCrear == 0) userViewVariantsController.Active["Deactivation in code"] = false;
                if (countVer == 0) changeVariantController.Active["Deactivation in code"] = false;

                if (countCrear > 0) userViewVariantsController.Active["Deactivation in code"] = true;
                if (countVer > 0) changeVariantController.Active["Deactivation in code"] = true;
            }
        }

        private void View_ModelSaving(Object sender, CancelEventArgs e)
        {
            e.Cancel = false;
            int countNoSave = (int)(((Usuario)SecuritySystem.CurrentUser).Session.Evaluate(typeof(Usuario), CriteriaOperator.Parse("Count()"), CriteriaOperator.Parse("UsuarioEnRol('PCA_NoGuardarModel')")));
            if (countNoSave > 0)
            {
                e.Cancel = true;
            }
        }

        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
        }
        protected override void OnDeactivated()
        {
            View.ModelSaving -= View_ModelSaving;
            base.OnDeactivated();
        }
    }
}
