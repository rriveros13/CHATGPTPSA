using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Data.Filtering;
using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.Metadata;
using FileUploadService.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace FileUploadService.Controllers
{
    [Route("[controller]/{solicitud}")]
    [ApiController]
    [Authorize]
    public class FileController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(IFormFile archivo, int solicitud)
        {
            Log.Information(string.Concat(archivo.Name, "-", archivo.FileName, "-", archivo.Length.ToString("F0")));
           
            var sha256 = System.Security.Cryptography.SHA256.Create();

            ReflectionDictionary dict = new ReflectionDictionary();
            dict.QueryClassInfo(typeof(XpoFileDataItti));
            string connectionString = Program.MyConfig["ConnectionString"];
            //const string connectionString = "XpoProvider=MSSqlServer;Data Source=127.0.0.1;User ID=pcadev_user;Password=pcaPa$$w0rd;Initial Catalog=PCADEV"; //Program.myConfig["ConnectionString"];
            
            using (IDataLayer dataLayer = XpoDefault.GetDataLayer(connectionString, dict, AutoCreateOption.None))
            {
                using (UnitOfWork work = new UnitOfWork(dataLayer))
                {
                    SelectedData res = work.ExecuteQuery(
                        $"select 1 from dbo.Solicitud where Oid={solicitud} and GCRecord is null");

                    if (res.ResultSet[0].Rows.Length == 0)
                    {
                        //Si no existe la solicitud devolver estado 404 con el id de la solicitud
                        return NotFound(solicitud);
                    }

                    var stream = archivo.OpenReadStream();
                    var br = new BinaryReader(stream);
                    
                    XpoFileDataItti fd = new XpoFileDataItti(work)
                    {
                        FileName = archivo.FileName, Content = br.ReadBytes((int) stream.Length)
                    };
                    
                    string hashArchivo = GetHash(sha256.ComputeHash(fd.Content));

                    XpoFileDataItti fdsearch =
                        work.FindObject<XpoFileDataItti>(CriteriaOperator.Parse("Hash=?", hashArchivo));

                    if (fdsearch != null)
                    {
                        if(work.FindObject<XpoAdjunto>(CriteriaOperator.Parse("Archivo.Oid=? and Solicitud=?", fdsearch.Oid, solicitud)) != null)
                        {
                            work.DropChanges();
                            return StatusCode(StatusCodes.Status409Conflict);
                        }
                    }
                    fd.Save();

                    XpoAdjunto ad = new XpoAdjunto(work)
                    {
                        Archivo = fd,
                        Fecha = DateTime.Now,
                        Adjuntado = true,
                        Solicitud = solicitud,
                        TipoAdjunto = Guid.Parse("793C08EC-3DCD-46CB-82F4-8CE42744418D")
                    };
                    ad.Save();
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
