using PDNOriginacion.Module.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace PDNOriginacion.Module.Web.Helpers
{
    class Helper
    {
        public static string GenerateUrlParam(object currentObject)
        {
            if (currentObject is Persona)
            {
                var oid = ((Persona)currentObject).Oid;

                return HttpUtility.UrlEncode(EncryptData(string.Concat(oid.ToString(), "|",
                    DateTime.UtcNow.ToString(new CultureInfo("es-PY")), "|", GetUserIp(), "|", "PERSONA")));
            }

            if (currentObject is Solicitud)
            {
                var oid = ((Solicitud)currentObject).Oid;

                return HttpUtility.UrlEncode(EncryptData(string.Concat(oid.ToString(), "|",
                    DateTime.UtcNow.ToString(new CultureInfo("es-PY")), "|", GetUserIp(), "|", "SOLICITUD")));
            }


            if (currentObject is InmuebleTasacion)
            {
                var oid = ((InmuebleTasacion)currentObject).Oid;

                return HttpUtility.UrlEncode(EncryptData(string.Concat(oid.ToString(), "|",
                    DateTime.UtcNow.ToString(new CultureInfo("es-PY")), "|", GetUserIp(), "|", "TASACION")));
            }

            return string.Empty;
        }

        private static string GetUserIp()
        {
            HttpContext context = HttpContext.Current;

            string ipList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            return !string.IsNullOrEmpty(ipList) ? ipList.Split(',')[0] : context.Request.ServerVariables["REMOTE_ADDR"];
        }

        private static string EncryptData(string plain)
        {
            string aesPassword = string.Concat("ytfILO!655577vvcvd333", System.Configuration.ConfigurationManager.AppSettings["aesPassword"]);

            string encrypted = CipherUtility.Encrypt<AesManaged>(plain, aesPassword, "77654");

            return encrypted;
        }
    }
}
