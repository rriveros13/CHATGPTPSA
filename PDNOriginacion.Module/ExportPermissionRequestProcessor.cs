using DevExpress.ExpressApp.Security;

namespace PDNOriginacion.Module
{
    public class ExportPermissionRequestProcessor : PermissionRequestProcessorBase<ExportPermissionRequest>
    {
        private IPermissionDictionary permissions;

        public ExportPermissionRequestProcessor(IPermissionDictionary permissions) => this.permissions = permissions;

        public override bool IsGranted(ExportPermissionRequest permissionRequest) => (permissions.FindFirst<ExportPermission>() !=
            null);
    }
}