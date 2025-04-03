using System;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using IdentityModel.Client;
using Microsoft.Win32;
using Newtonsoft.Json;
using RestSharp;
using Serilog;

namespace FileUploadClient
{
    public partial class Form1 : Form
    {
        public bool Cancelar;
        public bool Salir;
        public bool Transfiriendo;
        public string SourceDirectory;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            var mkey = Registry.CurrentUser.OpenSubKey("ITTIFileUpload", false);
            if (mkey != null)
            {
                var regPath = (string)mkey.GetValue("SelectedPath");
                var regUser = (string)mkey.GetValue("Usuario");
                txtPath.Text = !string.IsNullOrEmpty(regPath) ? regPath : ConfigurationManager.AppSettings["SelectedPath"];
                txtUsuario.Text = !string.IsNullOrEmpty(regUser) ? regUser : string.Empty;
                SourceDirectory = txtPath.Text;
            }

            if(!string.IsNullOrEmpty(txtUsuario.Text)) txtContraseña.Select();

            chElimTranExito.CheckState = CheckState.Checked;
            chElimDuplicados.CheckState = CheckState.Unchecked;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.File(
                    $@"{Path.GetDirectoryName(Assembly.GetEntryAssembly().Location)}\FileUploadClient.txt",
                    fileSizeLimitBytes: 1_000_000,
                    rollingInterval: RollingInterval.Day,
                    shared: true,
                    flushToDiskInterval: TimeSpan.FromSeconds(1))
                .CreateLogger();

            Log.Information("FileUploadCliente Iniciado!");

            SetEstadoIncial();

        }

        private void btnUpload_Click(object sender, EventArgs e)
        {

            SourceDirectory = txtPath.Text;

            if (string.IsNullOrEmpty(txtUsuario.Text))
            {
                MessageBox.Show($@"¡Debe ingresar usuario y contraseña anstes de transmitir!", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(SourceDirectory))
            {
                MessageBox.Show($@"Directorio origen inexistente: {txtPath.Text}", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            if (!Autenticate(out string token))
            {
                MessageBox.Show(@"Error al autenticar con el servicio!", @"Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }

            int cantArchivos = ContarArchivos(SourceDirectory);

            if (cantArchivos == 0)
            {
                MessageBox.Show(@"No hay archivos para transferir en la carpeta indicada", @"Atención", MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);
                return;
            }

            SetEstadoTransfiriendo();

            Application.DoEvents();

            int cantTranOk = 0;
            int cantDuplicados = 0;
            int cantError = 0;

            bool errorAutenticación = false;

            foreach (string directorio in Directory.EnumerateDirectories(SourceDirectory))
            {
                bool archivosEliminados = true;
                string dirName = directorio.Substring(directorio.LastIndexOf(Path.DirectorySeparatorChar) + 1);
                foreach (string file in Directory.EnumerateFiles(directorio, "*.*"))
                {
                    string fileName = file.Substring(directorio.Length + 1);

                    lblTransfiriendoDetalle.Text = $@"{dirName}\{fileName}";
                    Application.DoEvents();
                    if (Cancelar)
                    {
                        SetEstadoIncial();
                        if (Salir) Application.Exit();
                        return;
                    }

                    byte[] fileContents = File.ReadAllBytes(file);
                                                       

                    //Task<string> loginToken = GetToken();

                    string serviceUrl = ConfigurationManager.AppSettings["FileUploadService"];
                    RestClient client = new RestClient(string.Concat(serviceUrl, "/", dirName));
                    RestRequest request = new RestRequest(Method.POST);
                    request.AddFileBytes("archivo", fileContents, fileName, "application/octet-stream");
                    request.AddHeader("cache-control", "no-cache");
                    request.AddHeader("Authorization", $"Bearer {token}");
                    request.AddHeader("User", txtUsuario.Text);
                    request.AddHeader("Password", txtContraseña.Text);
                    try
                    {
                        bool transferOk = false;
                        var response = client.Execute(request);

                        if (!response.IsSuccessful)
                        {
                            if (response.StatusCode == 0)
                            {
                                Log.Error(response.ErrorException, "Error al intentar contactar al servicio");
                            }
                        }

                        errorAutenticación = false;

                        switch (response.StatusCode)
                        {
                            case HttpStatusCode.Unauthorized:
                                 errorAutenticación = true;
                                 break;
                            case HttpStatusCode.NotFound:
                                Log.Error($"Solicitud no encontrada: {dirName}");
                                cantError++;
                                archivosEliminados = false;
                                break;
                            case HttpStatusCode.Conflict:
                                Log.Error($"Archivo duplicado en la solicitud, archivo: {file}");
                                cantDuplicados++;
                                if (chElimDuplicados.Checked)
                                {
                                    if (!EliminarArchivo(file))
                                    {
                                        archivosEliminados = false;
                                    }
                                }
                                else
                                {
                                    archivosEliminados = false;
                                }
                                continue;
                            case HttpStatusCode.Created:
                                transferOk = true;
                                cantTranOk++;
                                Log.Information($"Archivo {file} transferido con éxito!");
                                break;
                            default:
                                cantError++;
                                archivosEliminados = false;
                                Log.Information($"Archivo {file} no transferido, StatusCode: {response.StatusCode}, Exception: {response.ErrorException}");
                                break;
                        }

                        if(errorAutenticación) break;

                        progressBar1.PerformStep();
                        lblTranferidos.Text = $@"Tranferidos {progressBar1.Value}/{cantArchivos}";
                        Application.DoEvents();

                        if (transferOk)
                        {
                            if (chElimTranExito.Checked)
                            {
                                if (!EliminarArchivo(file))
                                {
                                    archivosEliminados = false;
                                }
                                else
                                {
                                    Log.Information($"Archivo {file} eliminado con éxito");
                                }
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, $"Error al intentar transferir el archivo {file}");
                        archivosEliminados = false;
                    }
                }

                if (chElimTranExito.Checked)
                {
                    if (archivosEliminados) EliminarDirectorio(directorio);
                }

            }

            if(errorAutenticación) lblUltimaTransfDetail.Text = "Error de autenticación, favor verificar usuario y contraseña";
            else
              lblUltimaTransfDetail.Text = $@"Con Éxito: {cantTranOk}, Con Error: {cantError}, Duplicados {cantDuplicados}";
            
            if (cantError > 0)
            {
                lblUltimaTransfDetail.ForeColor = Color.Red;
            }
            else
            {
                lblUltimaTransfDetail.ForeColor = cantDuplicados > 0 ? Color.Orange : Color.Green;
            }

            SetEstadoIncial();

            if (Salir) Application.Exit();
        }

        //private async Task<string> GetToken()
        //{
        //    var client = new HttpClient();

        //    var disco = await client.GetDiscoveryDocumentAsync("https://demo.identityserver.io");
        //    if (disco.IsError) throw new Exception(disco.Error);

        //    var response = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
        //    {
        //        Address = disco.TokenEndpoint,
        //        ClientId = "clientSersaFileUpload",
        //        ClientSecret = "secret",
        //        Scope = "api1"
        //    });

           
        //    return "";
        //}

        private void SetEstadoIncial()
        {
            salirToolStripMenuItem.Enabled = true;
            transferirToolStripMenuItem.Enabled = true;
            Transfiriendo = false;
            Cancelar = false;
            btnCancelar.Enabled = false;
            btnUpload.Enabled = true;
            btnSelectFolder.Enabled = true;
            btnSalir.Enabled = true;

            lblTranferidos.Text = @"Estado de transferencia: NO INICIADA 
(Hacer click en el botón <Transferir> para iniciar)";

            progressBar1.Minimum = 0;
            progressBar1.Maximum = 0;
            progressBar1.Step = 1;
            progressBar1.Value = 0;
            lblTransfiriendoDetalle.Text = "";
        }

        private void SetEstadoTransfiriendo()
        {
            salirToolStripMenuItem.Enabled = false;
            transferirToolStripMenuItem.Enabled = false;
            int cantArchivos = ContarArchivos(SourceDirectory);
            Transfiriendo = true;
            Salir = false;
            Cancelar = false;

            btnCancelar.Enabled = true;
            btnUpload.Enabled = false;
            btnSelectFolder.Enabled = false;
            btnSalir.Enabled = false;

            lblTranferidos.Text = $@"Tranferidos 0/{cantArchivos}";

            progressBar1.Minimum = 0;
            progressBar1.Maximum = cantArchivos;
            progressBar1.Step = 1;
            progressBar1.Value = 0;

            lblTransfiriendoDetalle.Text = "";
        }

        private static int ContarArchivos(string sourceDirectory)
        {
            if (!Directory.Exists(sourceDirectory)) return 0;

            int cantidad = 0;
            foreach (string directorio in Directory.EnumerateDirectories(sourceDirectory))
            {
                cantidad += Directory.EnumerateFiles(directorio).Count();
            }
            return cantidad;
        }

        private static void EliminarDirectorio(string directorio)
        {
            try
            {
                Directory.Delete(directorio);
                Log.Information($"Directorio {directorio} eliminado con éxito");
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error al tratar de eliminar el directorio {directorio}");
            }
        }

        private static bool EliminarArchivo(string file)
        {
            try
            {
                File.Delete(file);
                Log.Information($"Archivo {file} eliminado con éxito");
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, $"Error al tratar de eliminar el archivo {file}");
                return false;
            }

        }

        private static bool Autenticate(out string token)
        {
            try
            {
                var client = new RestClient("https://www.itti.digital/identity-server/connect/token");
                var request = new RestRequest(Method.POST);
                request.AddHeader("cache-control", "no-cache");
                request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
                request.AddParameter("undefined",
                    "client_id=clientSersaFileUpload&scope=SersaFileUploadAPI&client_secret=1298127hewuuqwausygyqwdg6363!!ww&grant_type=client_credentials&undefined=",
                    ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                string r = response.Content;
                var auth = JsonConvert.DeserializeObject<AuthResponse>(r);
                token = auth.access_token;
                return true;
            }
            catch (Exception e)
            {
                Log.Error(e, "Error al intentar obtener el access_token!");
                token = string.Empty;
                return false;
            }

        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            if (Salir) Application.Exit();

            string path;
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (Directory.Exists(txtPath.Text)) fbd.SelectedPath = txtPath.Text;
            DialogResult dr = fbd.ShowDialog();

            if (dr == DialogResult.OK)
            {
                path = fbd.SelectedPath;
                SetEstadoIncial();
            }
            else
            {
                return;
            }

            if (path == "") return;

            var mkey = Registry.CurrentUser.OpenSubKey("ITTIFileUpload", true);
            if (mkey == null)
            {
                mkey = Registry.CurrentUser.CreateSubKey("ITTIFileUpload");
                if (mkey != null)
                {
                    mkey.SetValue("SelectedPath", path);
                    mkey.Close();
                }
            }
            else
            {
                mkey.SetValue("SelectedPath", path);
                mkey.Close();

            }
            txtPath.Text = path;
            if (Salir) Application.Exit();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Cancelar = true;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtUsuario.Text))
            {
                var mkey = Registry.CurrentUser.OpenSubKey("ITTIFileUpload", true);
                if (mkey == null)
                {
                    mkey = Registry.CurrentUser.CreateSubKey("ITTIFileUpload");
                    if (mkey != null)
                    {
                        mkey.SetValue("Usuario", txtUsuario.Text);
                        mkey.Close();
                    }
                }
                else
                {
                    mkey.SetValue("Usuario", txtUsuario.Text);
                    mkey.Close();

                }
            }

            if (!Transfiriendo) return;
            if (MessageBox.Show(@"¿Desea cancelar la transferencia?", @"Salir", MessageBoxButtons.YesNo) == DialogResult.No)
            {
                e.Cancel = true;
            }
            else
            {
                Cancelar = true;
                Salir = true;
                e.Cancel = true;
            }
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Log.Information("Saliendo de la aplicación");
            Application.Exit();
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            if (FormWindowState.Minimized == WindowState)
            {
                notifyIcon1.Visible = true;
                ShowInTaskbar = false;
                //notifyIcon1.ShowBalloonTip(100);
                Hide();
            }
            else if (FormWindowState.Normal == WindowState)
            {
                notifyIcon1.Visible = false;
                ShowInTaskbar = true;
            }
        }

        private void transferirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnUpload.PerformClick();
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            btnSalir.PerformClick();
        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            notifyIcon1.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            notifyIcon1.Visible = false;
            Show();
            WindowState = FormWindowState.Normal;
        }
    }
}
