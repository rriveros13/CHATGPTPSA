using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using GdPicture14;
using ITTI;
using Serilog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace PDNVisor
{
    public class Helper
    {
        public static string GetUserIp()
        {
            HttpContext context = HttpContext.Current;
            string ipList = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string ip = !string.IsNullOrEmpty(ipList) ? ipList.Split(',')[0] : context.Request.ServerVariables["REMOTE_ADDR"];
            Log.Information($"Dirección IP del cliente: {ip}");
            return ip;
        }

        public static string DecryptData(string encData)
        {
            string aesPassword = string.Concat("ytfILO!655577vvcvd333", System.Configuration.ConfigurationManager.AppSettings["aesPassword"]);

            string decryptData = CipherUtility.Decrypt<AesManaged>(encData, aesPassword, "77654");

            return decryptData;
        }

        public static bool CompararIp(string sourceIpAddress, string actualIpAddress)
        {
            if (sourceIpAddress == actualIpAddress) return true;
            switch (sourceIpAddress)
            {
                case "::1" when actualIpAddress == "127.0.0.1":
                case "127.0.0.1" when actualIpAddress == "::1":
                    return true;
                default:
                    return false;
            }
        }

        public static void GetImage(string src, out string filename, out byte[] contenido)
        {
            ReflectionDictionary dict = new ReflectionDictionary();
            dict.QueryClassInfo(typeof(FileDataITTI));
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (IDataLayer dataLayer = XpoDefault.GetDataLayer(connectionString, dict, AutoCreateOption.None))
            {
                using (Session session = new Session(dataLayer))
                {
                    FileDataITTI d = session.GetObjectByKey<FileDataITTI>(Guid.Parse(src));
                    if (d == null)
                    {
                        throw new Exception($"Documento no encontrado en la base de datos, Oid Documento: {src}");
                    }
                    else
                    {
                        filename = d.FileName;
                        Log.Information($"Archivo obtenido {filename}, {d.Oid.ToString()}");
                        contenido = d.Content;
                    }
                }
            }
        }

        public static void GetImage(string src, out string filename, out byte[] contenido, out GdPicture14.DocumentFormat format)
        {
            if (string.IsNullOrEmpty(src))
            {
                filename = string.Empty;
                contenido = null;
                format = GdPicture14.DocumentFormat.DocumentFormatUNKNOWN;
                return;
            }
            ReflectionDictionary dict = new ReflectionDictionary();
            dict.QueryClassInfo(typeof(FileDataITTI));
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (IDataLayer dataLayer = XpoDefault.GetDataLayer(connectionString, dict, AutoCreateOption.None))
            {
                using (Session session = new Session(dataLayer))
                {
                    FileDataITTI d = session.GetObjectByKey<FileDataITTI>(Guid.Parse(src));
                    if (d == null)
                    {
                        throw new Exception($"Documento no encontrado en la base de datos, Oid Documento: {src}");
                    }
                    else
                    {
                        filename = d.FileName;
                        Log.Information($"Archivo obtenido {filename}, {d.Oid.ToString()}");
                        contenido = d.Content;
                        MemoryStream ms = new MemoryStream(contenido);
                        format = GdPictureDocumentUtilities.GetDocumentFormat((Stream)ms);
                    }
                }
            }
        }

    }
}