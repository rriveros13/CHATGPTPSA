using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Web;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using Serilog;
using ITTI;

namespace PDNVisor
{
    public partial class Visor : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack) return;
            try
            {
                //ArrayList colCookies = new ArrayList();
                //for (int i = 0; i < Request.Cookies.Count; i++)
                //    colCookies.Add(Request.Cookies[i]);

                string src = HttpUtility.UrlDecode(Request.QueryString["src"]);

                src = src?.Replace(' ', '+');

                if (string.IsNullOrEmpty(src))
                {
                    throw new Exception("Invocación incorrecta, parámetro src no recibido!");
                    //src = "7BB8BBB8-15C1-41BA-9B2E-18DE31DA3586";
                }

                DocuVieware1.EnableSaveButton = false;

                string param = Helper.DecryptData(src);

                string[] parray = param.Split('|');

                if(parray.Length != 3)
                {
                    throw new Exception($"Cantidad de parámetros recibitos incorrecta, recibidos {parray.Length}, esperados 3!");
                }

                Log.Information($"Procesando: {parray[0]}, {parray[1]}, {parray[2]}");

                string oid = parray[0];
                DateTime fecha = Convert.ToDateTime(parray[1], new CultureInfo("es-PY"));
                string sourceIpAddress = parray[2];
                    
                string pexpTime = ConfigurationManager.AppSettings["LinkExpiration"];
                int expTime = 60;

                if (!string.IsNullOrEmpty(pexpTime))
                {
                    if (int.TryParse(pexpTime, out int time)) expTime = time;
                }

                if ((DateTime.UtcNow - fecha).Minutes < expTime && Helper.CompararIp(sourceIpAddress, Helper.GetUserIp()))
                {
                    DocuVieware1.EnableSaveButton = true;
                    Helper.GetImage(oid, out var filename, out byte[] content);
                    Title = filename;
                    DocuVieware1.LoadFromStream(new MemoryStream(content), true, filename);
                }
                else
                {
                    Log.Information($"No pasó validacion de tiempo o ip! linkIP: {sourceIpAddress}, userIP: {Helper.GetUserIp()}");
                    DocuVieware1.Close();
                }

            }
            catch (Exception ex)
            {
                DocuVieware1.Close();
                Log.Error(ex.ToString());
                Log.Error(Request.RawUrl);
            }
        }

      

       

       
    }
}