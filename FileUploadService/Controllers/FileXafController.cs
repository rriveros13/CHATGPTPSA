using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using FileUploadService.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Serilog;

namespace FileUploadService.Controllers
{
    [Route("[controller]/{nroSolicitud}")]
    [ApiController]
    [Authorize]
    public class FileXafController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile archivo, int nroSolicitud)
        {
            Log.Information(string.Concat(archivo.Name, "-", archivo.FileName, "-", archivo.Length.ToString("F0")));
            
            Request.Headers.TryGetValue("User", out StringValues userStringValues);
            Request.Headers.TryGetValue("Password", out StringValues passwordStringValues);

            if (userStringValues.Count == 0 || passwordStringValues.Count == 0)
            {
                return Unauthorized();
            }

            string userName = userStringValues[0];
            string userPassword = passwordStringValues[0];

            string regexUserName = @"^[a-z0-9_-]{3,30}$";

            if (!Regex.Match(userName, regexUserName, RegexOptions.IgnoreCase).Success)
            {
                return Unauthorized();
            }

            ReflectionDictionary dict = new ReflectionDictionary();
            dict.QueryClassInfo(typeof(XpoFileDataItti));
            string connectionString = Program.MyConfig["ConnectionString"];
            
            using (IDataLayer dataLayer = XpoDefault.GetDataLayer(connectionString, dict, AutoCreateOption.None))
            {
                using (UnitOfWork work = new UnitOfWork(dataLayer))
                {
                    //Verificar usuario y contraseña
                    SelectedData userData = work.ExecuteQuery(
                        $"Select UserName, StoredPassword, Oid from dbo.Usuario where UserName='{userName}' and GCRecord is null and IsActive=1");

                    if (userData.ResultSet[0].Rows.Length == 0)
                    {
                        return Unauthorized();
                    }
                    
                    string storedPassword = (string)userData.ResultSet[0].Rows[0].Values[1];
                    Guid usuario = (Guid)userData.ResultSet[0].Rows[0].Values[2];

                    bool passOk = Rfc2898PasswordCryptographer.VerifyHashedPassword(storedPassword, userPassword);
                    if (!passOk)
                    {
                       return Unauthorized();
                    }

                    //Verificar si existe la solicitud
                    SelectedData res = work.ExecuteQuery(
                        $"select 1 from dbo.Solicitud where Oid={nroSolicitud} and GCRecord is null");

                    if (res.ResultSet[0].Rows.Length == 0)
                    {
                        return NotFound(nroSolicitud);
                    }

                    Stream stream = archivo.OpenReadStream();
                    BinaryReader br = new BinaryReader(stream);
                    
                    XpoFileDataItti fd = new XpoFileDataItti(work)
                    {
                        FileName = archivo.FileName, Content = br.ReadBytes((int) stream.Length)
                    };
                    
                    SHA256 sha256 = SHA256.Create();
                    string hashArchivo = GetHash(sha256.ComputeHash(fd.Content));

                    XpoFileDataItti fdsearch =
                        work.FindObject<XpoFileDataItti>(CriteriaOperator.Parse("Hash=?", hashArchivo));

                    if (fdsearch != null)
                    {
                        if(work.FindObject<XpoAdjunto>(CriteriaOperator.Parse("Archivo.Oid=? and Solicitud=?", fdsearch.Oid, nroSolicitud)) != null)
                        {
                            work.DropChanges();
                            return StatusCode(StatusCodes.Status409Conflict);
                        }
                    }
                    fd.Save();

                    await work.CommitChangesAsync();

                    XpoTipoAdjunto tipoAdjunto = work.FindObject<XpoTipoAdjunto>(CriteriaOperator.Parse("Descripcion=?", "OTRO"));

                    XpoAdjunto newAdjunto = new XpoAdjunto(work)
                    {
                        Archivo = fd,
                        Fecha = DateTime.Now,
                        Adjuntado = true,
                        Solicitud = nroSolicitud,
                        TipoAdjunto = tipoAdjunto?.Oid,
                        AdjuntadoPor = usuario,
                        Descripcion = (tipoAdjunto != null ? tipoAdjunto.Descripcion : "") + " - " + (fd != null ? fd.FileName : "")
                    };
                    newAdjunto.Save();
                    await work.CommitChangesAsync();
                }
            }
            return StatusCode(StatusCodes.Status201Created);
        }

        private static string GetHash(IEnumerable<byte> data)
        {
            var sBuilder = new StringBuilder();
            foreach (byte t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }
    }
}
