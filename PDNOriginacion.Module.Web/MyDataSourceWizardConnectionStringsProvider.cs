using DevExpress.DataAccess.ConnectionParameters;
using DevExpress.DataAccess.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PDNOriginacion.Module.Web
{
    public class MyDataSourceWizardConnectionStringsProvider : IDataSourceWizardConnectionStringsProvider
    {
        public Dictionary<string, string> GetConnectionDescriptions()
        {
            Dictionary<string, string> connections = new Dictionary<string, string>();

            connections.Add("msSqlConnectionPCA", "PCADB");
            connections.Add("msSqlConnectionPDN", "PDNDB");
            return connections;
        }

        public DataConnectionParametersBase GetDataConnectionParameters(string name)
        {
            if (name == "msSqlConnectionPCA")
            {
                string server = System.Configuration.ConfigurationManager.AppSettings["DashboardServer"];
                string database = System.Configuration.ConfigurationManager.AppSettings["DashboardDB"];
                string usuario = System.Configuration.ConfigurationManager.AppSettings["DashboardUser"];
                string password = System.Configuration.ConfigurationManager.AppSettings["DashboardPass"];
                if(string.IsNullOrEmpty(usuario) || string.IsNullOrEmpty(password))
                {
                    return new MsSqlConnectionParameters(server, database, "", "", MsSqlAuthorizationType.Windows);
                }
                else
                {
                    return new MsSqlConnectionParameters(server, database, usuario, password, MsSqlAuthorizationType.SqlServer);
                }
                
            }

            if(name == "msSqlConnectionPDN")
            {
                return new MsSqlConnectionParameters("localhost", "PDN", "pdn_dashboard_demo", "pdnPa$$w0rd", MsSqlAuthorizationType.SqlServer);
            }
           
            throw new System.Exception("La cadena de conexión no es correcta");
        }
    }
}
