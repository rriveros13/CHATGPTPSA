using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Security;
using DevExpress.ExpressApp.Updating;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using PDNOriginacion.Module.BusinessObjects;
using PDNOriginacion.Module.Helpers;
using System;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace PDNOriginacion.Module.DatabaseUpdate
{
    public class Updater : ModuleUpdater
    {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion)
        {
        }

        private Rol CreateDefaultRole()
        {
            Rol defaultRole = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "Default"));
            if(defaultRole == null)
            {
                defaultRole = ObjectSpace.CreateObject<Rol>();
                defaultRole.Name = "Default";

                defaultRole.AddObjectPermission<Usuario>(SecurityOperations.Read,
                                                         "[Oid] = CurrentUserId()",
                                                         SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<Configuracion>(SecurityOperations.Read,
                                                                         SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<SecurityPolicy>(SecurityOperations.Read,
                                                                          SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<LPassword>(SecurityOperations.Create,
                                                                     SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<LPassword>(SecurityOperations.ReadWriteAccess,
                                                                     SecurityPermissionState.Allow);
                defaultRole.AddNavigationPermission(@"Application/NavigationItems/Items/Default/Items/MyDetails",
                                                    SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<Usuario>(SecurityOperations.Write,
                                                         "ChangePasswordOnFirstLogon",
                                                         "[Oid] = CurrentUserId()",
                                                         SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<Usuario>(SecurityOperations.Write,
                                                         "StoredPassword",
                                                         "[Oid] = CurrentUserId()",
                                                         SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<Usuario>(SecurityOperations.Write,
                                                         "LPasswords",
                                                         "[Oid] = CurrentUserId()",
                                                         SecurityPermissionState.Allow);
                defaultRole.AddMemberPermission<Usuario>(SecurityOperations.Write,
                                                         "LastLogon",
                                                         "[Oid] = CurrentUserId()",
                                                         SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<Rol>(SecurityOperations.Read, SecurityPermissionState.Deny);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.ReadWriteAccess,
                                                                           SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.ReadWriteAccess,
                                                                                 SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifference>(SecurityOperations.Create,
                                                                           SecurityPermissionState.Allow);
                defaultRole.AddTypePermissionsRecursively<ModelDifferenceAspect>(SecurityOperations.Create,
                                                                                 SecurityPermissionState.Allow);
            }
            return defaultRole;
        }

        public override void UpdateDatabaseAfterUpdateSchema()
        {
            base.UpdateDatabaseAfterUpdateSchema();

            if(ObjectSpace.GetObjectsCount(typeof(Configuracion), null) == 0)
            {
                Configuracion conf = ObjectSpace.CreateObject<Configuracion>();
                conf.Empresa = "ITTI";
                conf.RazonSocial = "IT CONSULTORES S.A.";
                conf.Direccion = "Torre 1, Piso 7, Paseo la Galería, Asunción, Paraguay";
                SecurityPolicy sp = ObjectSpace.CreateObject<SecurityPolicy>();
                sp.Configuracion = conf;
                sp.EnforcePasswordHistory = true;
                sp.MaxPasswordAge = 60;
                sp.MinPasswordAge = 0;
                sp.MinPasswordLenght = 8;
                sp.PasswordComplexity = true;
                conf.SecurityPolicy = sp;
            }

            ObjectSpace.CommitChanges();

            Configuracion config = Configuracion.GetInstance(ObjectSpace);

            if(config.GetParametro("ACTUALIZAR_PROPIEDADES") != "N")
            {
                foreach(PropertyInfo propertyInfo in typeof(Solicitud).GetProperties())
                {
                    string nombre = propertyInfo.Name;

                    if(nombre.ToLower() == "oid" ||
                        nombre.ToLower() == "gcrecord" ||
                        nombre.ToLower() == "optimisticlockfield")
                    {
                        continue;
                    }

                    Campo campo = ObjectSpace.FindObject<Campo>(CriteriaOperator.Parse("Nombre=?", nombre));
                    if(campo != null)
                    {
                        continue;
                    }

                    Campo campoNuevo = ObjectSpace.CreateObject<Campo>();
                    campoNuevo.Nombre = nombre;
                    campoNuevo.Activo = true;
                }
                ObjectSpace.CommitChanges();
            }

            Rol adminRol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name",
                                                                          SecurityStrategy.AdministratorRoleName));
            if(adminRol == null)
            {
                adminRol = ObjectSpace.CreateObject<Rol>();
                adminRol.Name = SecurityStrategy.AdministratorRoleName;
                adminRol.IsAdministrative = true;
            }

            Usuario usuarioAdmin = ObjectSpace.FindObject<Usuario>(new BinaryOperator("UserName", "Admin"));

            if(usuarioAdmin == null)
            {
                usuarioAdmin = ObjectSpace.CreateObject<Usuario>();
                usuarioAdmin.UserName = "Admin";
                usuarioAdmin.SetPassword("Pa$$w0rd");
                usuarioAdmin.IsActive = true;
                usuarioAdmin.ValidarPassAD = false;
                usuarioAdmin.ChangePasswordOnFirstLogon = true;
                usuarioAdmin.RolesUsuario.Add(adminRol);
            }

            CreateDefaultRole();
            CrearOtrosRoles();
            //CargarDatosMaestros();


            ObjectSpace.CommitChanges();
        }

        private void CargarDatosMaestros()
        {
            bool cargarDatosMaestros = ConfigurationManager.AppSettings["CargarDatosMaestros"].ToString().Equals("S");

            if (cargarDatosMaestros) 
            {
                #region Naturaleza de Persona
                NaturalezaPersona regNP = ObjectSpace.FindObject<NaturalezaPersona>(CriteriaOperator.Parse("Codigo = 'F'"));
                if (regNP == null)
                {
                    regNP = ObjectSpace.CreateObject<NaturalezaPersona>();
                    regNP.Codigo = "F";
                    regNP.Descripcion = "Persona Física";
                    regNP.Save();
                }

                regNP = ObjectSpace.FindObject<NaturalezaPersona>(CriteriaOperator.Parse("Codigo = 'J'"));
                if (regNP == null)
                {
                    regNP = ObjectSpace.CreateObject<NaturalezaPersona>();
                    regNP.Codigo = "J";
                    regNP.Descripcion = "Persona Jurídica";
                    regNP.Save();
                }

                ObjectSpace.CommitChanges();

                if (ObjectSpace.GetObjects<NaturalezaPersona>().Where(n => n.Codigo != "J" && n.Codigo != "F").Count() > 0)
                    throw new Exception("Se encontró un error en registro de Maestros: Naturaleza Persona. Contactar con el Administrador de Sistemas.");
                #endregion Naturaleza de Persona

                //Agregar tipo de persona solicitud, se esta usando titular = sol
                //Agregar tipo de telefono o grupo, se usa para seleccionar prefijos.
            } 
        }

        private void CrearOtrosRoles()
        {
            Rol pCA_PermitirCrearVistasPersonalizadas = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_PermitirCrearVistasPersonalizadas"));
            if (pCA_PermitirCrearVistasPersonalizadas == null)
            {
                pCA_PermitirCrearVistasPersonalizadas = ObjectSpace.CreateObject<Rol>();
                pCA_PermitirCrearVistasPersonalizadas.Name = "PCA_PermitirCrearVistasPersonalizadas";
            }

            Rol pCA_PermitirVerVistasPersonalizadas = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_PermitirVerVistasPersonalizadas"));
            if (pCA_PermitirVerVistasPersonalizadas == null)
            {
                pCA_PermitirVerVistasPersonalizadas = ObjectSpace.CreateObject<Rol>();
                pCA_PermitirVerVistasPersonalizadas.Name = "PCA_PermitirVerVistasPersonalizadas";
            }

            Rol pCA_NoGuardarModel = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_NoGuardarModel"));
            if (pCA_NoGuardarModel == null)
            {
                pCA_NoGuardarModel = ObjectSpace.CreateObject<Rol>();
                pCA_NoGuardarModel.Name = "PCA_NoGuardarModel";
            }

            Rol pCA_CrearSolicitud = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_CrearSolicitud"));
            if (pCA_CrearSolicitud == null)
            {
                pCA_CrearSolicitud = ObjectSpace.CreateObject<Rol>();
                pCA_CrearSolicitud.Name = "PCA_CrearSolicitud";
            }

            Rol rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_TransicionEstAnterior"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_TransicionEstAnterior";
            }
           
            rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_GenerarPresupuesto"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_GenerarPresupuesto";
            }
            
            rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_CrearTasacion"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_CrearTasacion";
            }
           
            rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_CrearValorizacion"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_CrearValorizacion";
            }
            
            rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_AprobarTasa"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_AprobarTasa";
            }
            
            rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_CrearTarea"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_CrearTarea";
            }
            
            rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_AgregarInmueble"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_AgregarInmueble";
            }
            
            rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_CalculadoraPrestamo"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_CalculadoraPrestamo";
            }
            
            rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_GenerarCuotas"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_GenerarCuotas";
            }
            
            rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_AprobarPresupuesto"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_AprobarPresupuesto";
            }
            
            rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_AsignarTareas"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_AsignarTareas";
            }
            
            rol = ObjectSpace.FindObject<Rol>(new BinaryOperator("Name", "PCA_ReasignarTareas"));
            if (rol == null)
            {
                rol = ObjectSpace.CreateObject<Rol>();
                rol.Name = "PCA_ReasignarTareas";
            }
        }
        
        public override void UpdateDatabaseBeforeUpdateSchema() => base.UpdateDatabaseBeforeUpdateSchema();
    }
}
