using DevExpress.ExpressApp;
using DevExpress.Persistent.Validation;
using System;
using System.Linq;

namespace PDNOriginacion.Module.Web.Controllers
{
    public class RegisterScriptFixErrorMessageController : ViewController<DetailView>
    {
        public RegisterScriptFixErrorMessageController() => TargetViewNesting = Nesting.Root;

        private void RuleSet_ValidationCompleted(object sender, ValidationCompletedEventArgs e)
        {
            //((WebWindow)Frame).RegisterStartupScript("CheckErrorMessageScroll", @"
            //        CheckErrorMessageScroll();
            //    ");
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            Validator.RuleSet.ValidationCompleted += RuleSet_ValidationCompleted;
        }
        protected override void OnDeactivated()
        {
            Validator.RuleSet.ValidationCompleted -= RuleSet_ValidationCompleted;
            base.OnDeactivated();
        }
    }
}
