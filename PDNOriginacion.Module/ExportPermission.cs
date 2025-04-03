using DevExpress.ExpressApp.Security;

namespace PDNOriginacion.Module
{
    public class ExportPermission : IOperationPermission
    {
        public string Operation => "Export";
    }
}
