using DevExpress.ExpressApp.Security;

namespace PDNOriginacion.Module
{
    public class ExportPermissionRequest : IPermissionRequest
    {
        public object GetHashObject() => GetType().FullName;
    }
}