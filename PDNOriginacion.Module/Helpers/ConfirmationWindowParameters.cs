using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;

namespace PDNOriginacion.Module.Helpers
{
    [DomainComponent]
    public class ConfirmationWindowParameters
    {
        public ConfirmationWindowParameters()
        {
        }

        [ModelDefault("AllowEdit", "False")]
        public string ConfirmationMessage { get; set; }
    }
}