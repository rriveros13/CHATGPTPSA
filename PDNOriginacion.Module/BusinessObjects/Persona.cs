using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using PDNOriginacion.Module.Helpers;
using System;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using PDNOriginacion.Module;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("DescripcionPersona")]
    [SearchClassOptions(SearchMemberMode.Exclude)]
    [RuleCriteria("DelPersona", DefaultContexts.Delete, "Solicitudes.Count == 0 and Seguimientos.Count == 0", "No se puede eliminar la Persona porque tiene datos asociados (Solicitudes, Seguimientos, etc...)!", SkipNullOrEmptyValues = true)]
    public class Persona : BaseObject, IObjectSpaceLink
    {
        int cantidadPrestamosCancelados;
        int cantidadPrestamosActivos;
        TipoIngreso tipoIngreso;
        EstadoPersona estadoPersona;
        static FieldsClass _Fields;
        private int antiguedad; //en meses
        private int antiguedadAños;
        private string primerApellido;
        private bool contacto;
        private string correoLaboral;
        private string correoParticular;
        private string documento;
        private int edad;
        private EstadoCivil estadoCivil;
        private DateTime fechaAlta;
        private Usuario usuarioCreacion;
        private DateTime fechaNacimiento;
        private FormaDeSeguiminetoPreferida formadeSeguiminetoPreferida;
        private Genero genero;
        private string horarioSeguimineto;
        private bool? interesadoPrestamo;
        private MedioIngreso medioIngreso;
        private decimal montoSolicitado;
        MotivoSolicitud motivoSolicitud;
        private string descripcionPersona;
        private string nombreCompleto;
        private string primerNombre;
        private EstadoPregunta poseeMora;
        private EstadoPregunta poseeDemanda;
        private EstadoPregunta poseeInhibicion;
        private string observacionRechDesc;
        private string opeConMoraDemandasInhiDet;
        private Pais paisDocumento;
        private bool? procesoCreditoExplicado;
        private Profesion profesion;
        private PropietarioInquilino propietarioInquilino;
        private string razonSocial;
        private string ruc;
        private decimal salario;
        private bool? tieneComoDemostrarIngresos;
        private NaturalezaPersona tipo;
        private TipoDocumento tipoDocumento;
        private string _codigo;
        private string segundoNombre;
        private string segundoApellido;
        private MotivoDescarte motivoDescarte;
        private MotivoRechazo motivoRechazo;
        private Pais nacionalidad;
        private Decimal lineaCredito;
        private Decimal maximaCuota;
        private String fajaInformconf;

        //Carga Rapida de inmueble
        private string cri_cuentaCatastral;
        private decimal cri_superficieM2;
        private TipoCamino cri_tipoCamino;
        private TipoInmueble cri_tipoInmueble;
        private EstadoTitulo cri_estadoTitulo;
        decimal cri_valorAproximado;
        private Barrio cri_barrio;
        private Ciudad cri_ciudad;
        private Departamento cri_departamento;
        private string cri_calle;
        private string cri_numero;
        private bool cri_impuestoAlDia;
        private string cri_observaciones;

        //Carga rapida de direccion
        private Pais crd_pais;
        private Barrio crd_barrio;
        private Ciudad crd_ciudad;
        private Departamento crd_departamento;
        private string crd_calle;
        private string crd_numero;

        //Carga rapida de telefono
        private string crt_numero;
        private Prefijo crt_prefijo;
        private TipoTelefono crt_tipoTelefono;
        private GrupoTelefono crt_tipo;
        private bool crt_whatsapp;
        private bool crt_preferido;

        public Persona(Session session) : base(session)
        {

        }

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            fechaAlta = DateTime.Now;
            usuarioCreacion = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
            tipoDocumento = Session.FindObject<TipoDocumento>(TipoDocumento.Fields.Codigo == "1");
            paisDocumento = Session.FindObject<Pais>(Pais.Fields.Codigo == "1");
            Nacionalidad = Session.FindObject<Pais>(Pais.Fields.Codigo == "1");
            medioIngreso = Session.FindObject<MedioIngreso>(MedioIngreso.Fields.Default == (CriteriaOperator)true);
            tipo = Session.FindObject<NaturalezaPersona>(NaturalezaPersona.Fields.Codigo == "F");
            tieneComoDemostrarIngresos = true;
            contacto = true;
            estadoPersona = Session.FindObject<EstadoPersona>(EstadoPersona.Fields.Default == (CriteriaOperator)true);
            primerNombre = "S/Nombre";
            primerApellido = "S/Apellido";
            SegundoNombre = "";
            segundoApellido = "";
            //carga rapida de inmuebles           
            CRI_CuentaCatastral = DateTime.Now.ToString("dd/MM") + SecuritySystem.CurrentUserName;
            CRI_Calle = "Sin asignar";
            CRI_Numero = "Sin Nro";
            CRD_Calle = "Sin asignar";
            CRD_Numero = "Sin Nro";
            crd_pais = Session.FindObject<Pais>(Pais.Fields.Codigo == "1");
            poseeDemanda = EstadoPregunta.No;
        }


        public Persona GetOrCreate(string documento, TipoDocumento tipoDocumento)
        {
            if (string.IsNullOrEmpty(documento) || tipoDocumento == null)
            {
                throw new UserFriendlyException("¡Documento y/o Tipo Documento no deben estar vacíos!");
            }

            Persona p = Session.FindObject<Persona>(Persona.Fields.Documento == documento &
                Persona.Fields.TipoDocumento == tipoDocumento);

            if (p != null)
            {
                return p;
            }

            Session.BeginNestedUnitOfWork();

            p = new Persona(Session) { Documento = documento, TipoDocumento = tipoDocumento };

            Buscame perBuscame = WFHelper.BuscarPersona(p.Documento);
            if (perBuscame != null)
            {
                p.PrimerNombre = GetPalabraSeparada(perBuscame.Name, true);
                p.SegundoNombre = GetPalabraSeparada(perBuscame.Name, false);
                p.PrimerApellido = GetPalabraSeparada(perBuscame.Lastname, true);
                p.SegundoApellido = GetPalabraSeparada(perBuscame.Lastname, false);
                p.Save();
                Session.CommitTransaction();
                return p;
            }
            throw new UserFriendlyException("¡Persona no encontrada por documento/tipo documento!");
        }

        public static string GetPalabraSeparada(string cadena, bool primeraPalabra)
        {
            string[] palabras = cadena.Split(' ');
            string respuesta = string.Empty;

            if (palabras.Length > 0)
            {
                if (primeraPalabra)
                    respuesta = palabras[0];
                else
                {
                    if (palabras.Length > 1)
                    {
                        for (int i = 1; i < palabras.Length - 1; i++)
                        {
                            respuesta += palabras[i] + " ";
                        }
                    }
                }
            }

            return respuesta;
        }

        public static Persona GetOrCreate(string documento,
                                          TipoDocumento tipoDocumento,
                                          string primerNombre,
                                          string segundoNombre,
                                          string primerApellido,
                                          string segundoApellido,
                                          Session session)
        {
            if (!string.IsNullOrEmpty(documento))
            {
                Persona newPersona = session.FindObject<Persona>(Persona.Fields.Documento == documento &
                    Persona.Fields.TipoDocumento == tipoDocumento);

                if (newPersona != null)
                {
                    return newPersona;
                }

                newPersona = new Persona(session)
                {
                    Documento = documento,
                    TipoDocumento = tipoDocumento,
                    PrimerNombre = primerNombre,
                    SegundoNombre = segundoNombre,
                    PrimerApellido = primerApellido,
                    SegundoApellido = segundoApellido,
                    Tipo = session.FindObject<NaturalezaPersona>(NaturalezaPersona.Fields.Codigo == "F")
                };

                Buscame perBuscame = WFHelper.BuscarPersona(newPersona.Documento);
                if (perBuscame != null)
                {
                    newPersona.PrimerNombre = GetPalabraSeparada(perBuscame.Name, true);
                    newPersona.SegundoNombre = GetPalabraSeparada(perBuscame.Name, false);
                    newPersona.PrimerApellido = GetPalabraSeparada(perBuscame.Lastname, true);
                    newPersona.SegundoApellido = GetPalabraSeparada(perBuscame.Lastname, false);
                }

                newPersona.Save();
                return newPersona;
            }
            else
            {
                if (string.IsNullOrEmpty(primerNombre) || string.IsNullOrEmpty(primerApellido))
                {
                    throw new UserFriendlyException("¡Nombres y/o apellidos no deben estar vacíos!");
                }

                primerNombre = primerNombre.ToUpper();
                segundoNombre = segundoNombre.ToUpper();
                primerApellido = primerApellido.ToUpper();
                segundoApellido = segundoApellido.ToUpper();
                Persona p = session.FindObject<Persona>(Persona.Fields.PrimerNombre == primerNombre && Persona.Fields.SegundoNombre == segundoNombre &&
                    Persona.Fields.PrimerApellido == primerApellido && Persona.Fields.SegundoApellido == segundoApellido);

                if (p != null)
                {
                    return p;
                }
                p = new Persona(session)
                {
                    PrimerNombre = primerNombre,
                    SegundoNombre = segundoNombre,
                    PrimerApellido = primerApellido,
                    SegundoApellido = segundoApellido,
                    Tipo = session.FindObject<NaturalezaPersona>(NaturalezaPersona.Fields.Codigo == "F")
                };

                p.Save();
                return p;
            }
        }
        public static Persona GetOrCreate(string documento,
                                          TipoDocumento tipoDocumento,
                                          string primerNombre,
                                          string segundoNombre,
                                          string primerApellido,
                                          string segundoApellido,
                                          IObjectSpace os,
                                          out bool personaNueva)
        {
            if (!string.IsNullOrEmpty(documento))
            {
                Persona p = os.FindObject<Persona>(Persona.Fields.Documento == documento &
                    Persona.Fields.TipoDocumento == tipoDocumento);

                if (p != null)
                {
                    personaNueva = false;
                    return p;
                }
                p = os.CreateObject<Persona>();
                p.Documento = documento;
                p.TipoDocumento = tipoDocumento;
                p.PrimerNombre = primerNombre;
                p.SegundoNombre = segundoNombre;
                p.PrimerApellido = primerApellido;
                p.SegundoApellido = segundoApellido;
                p.Tipo = os.FindObject<NaturalezaPersona>(NaturalezaPersona.Fields.Codigo == "F");

                Buscame perBuscame = WFHelper.BuscarPersona(p.Documento);
                if (perBuscame != null)
                {
                    p.PrimerNombre = GetPalabraSeparada(perBuscame.Name, true);
                    p.SegundoNombre = GetPalabraSeparada(perBuscame.Name, false);
                    p.PrimerApellido = GetPalabraSeparada(perBuscame.Lastname, true);
                    p.SegundoApellido = GetPalabraSeparada(perBuscame.Lastname, false);
                }

                p.Save();
                personaNueva = true;
                return p;
            }
            else
            {
                if (string.IsNullOrEmpty(primerNombre) || string.IsNullOrEmpty(primerApellido))
                {
                    throw new UserFriendlyException("¡Nombres y/o apellidos no deben estar vacíos!");
                }

                primerNombre = primerNombre.ToUpper();
                segundoNombre = segundoNombre.ToUpper();
                primerApellido = primerApellido.ToUpper();
                segundoApellido = segundoApellido.ToUpper();
                Persona p = os.FindObject<Persona>(Persona.Fields.PrimerNombre == primerNombre && Persona.Fields.SegundoNombre == segundoNombre &&
                    Persona.Fields.PrimerApellido == primerApellido && Persona.Fields.SegundoApellido == segundoApellido);

                if (p != null)
                {
                    personaNueva = false;
                    return p;
                }
                p = os.CreateObject<Persona>();
                p.PrimerNombre = primerNombre;
                p.SegundoNombre = segundoNombre;
                p.PrimerApellido = primerApellido;
                p.SegundoApellido = segundoApellido;
                p.Tipo = os.FindObject<NaturalezaPersona>(NaturalezaPersona.Fields.Codigo == "F");

                p.Save();
                personaNueva = true;
                return p;
            }
        }

        [Association("Adjunto-Persona")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<Adjunto> Adjuntos => GetCollection<Adjunto>(nameof(Adjuntos));

        [XafDisplayName("Meses de antigüedad laboral")]
        public int Antiguedad
        {
            get => antiguedad;
            set => SetPropertyValue(nameof(Antiguedad), ref antiguedad, value);
        }

        [XafDisplayName("Años de antigüedad laboral")]
        public int AntiguedadAños
        {
            get => antiguedadAños;
            set => SetPropertyValue(nameof(AntiguedadAños), ref antiguedadAños, value);
        }

        [Size(100)]
        [ImmediatePostData]
        [Appearance("", Enabled = false, Criteria = "Tipo.Codigo= 'J'", Context = nameof(DetailView))]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string PrimerApellido
        {
            get => primerApellido;
            set
            {
                bool cambio = SetPropertyValue(nameof(PrimerApellido), ref primerApellido, value);
                if (IsLoading || IsSaving || !cambio)
                {
                    return;
                }
                nombreCompleto = string.Concat(PrimerNombre, " ", SegundoNombre, " ", PrimerApellido, " ", SegundoApellido);
                OnChanged(nameof(NombreCompleto));
            }
        }

        [Size(100)]
        [ImmediatePostData]
        [Appearance("", Enabled = false, Criteria = "Tipo.Codigo= 'J'", Context = nameof(DetailView))]
        public string SegundoApellido
        {
            get => segundoApellido;
            set
            {
                bool cambio = SetPropertyValue(nameof(SegundoApellido), ref segundoApellido, value);
                if (IsLoading || IsSaving || !cambio)
                {
                    return;
                }
                nombreCompleto = string.Concat(PrimerNombre, " ", SegundoNombre, " ", PrimerApellido, " ", SegundoApellido);
                OnChanged(nameof(NombreCompleto));
            }
        }

        [PersistentAlias("Seguimientos.Count()")]
        [VisibleInDetailView(false)]
        public int CantSeguimientos => (int)EvaluateAlias(nameof(CantSeguimientos));

        [PersistentAlias("Solicitudes.Count()")]
        [VisibleInDetailView(false)]
        public int CantSolicitudes => (int)EvaluateAlias(nameof(CantSolicitudes));

        [PersistentAlias("!IsNull(Documento)")]
        [VisibleInDetailView(false)]
        public bool ConDocumento => (bool)EvaluateAlias(nameof(ConDocumento));

        public bool Contacto
        {
            get => contacto;
            set => SetPropertyValue(nameof(Contacto), ref contacto, value);
        }

        // [DataSourceCriteria("Descripcion='RECHAZADO' or Descripcion = 'DESCARTADO'")] //todo, parametrizar para SERSA
        [ModelDefault("AllowEdit", "false")]
        public EstadoPersona Estado
        {
            get => estadoPersona;
            set => SetPropertyValue(nameof(Estado), ref estadoPersona, value);
        }

        [Size(100)]
        public string CorreoLaboral
        {
            get => correoLaboral;
            set => SetPropertyValue(nameof(CorreoLaboral), ref correoLaboral, value);
        }

        [Size(100)]
        public string CorreoParticular
        {
            get => correoParticular;
            set => SetPropertyValue(nameof(CorreoParticular), ref correoParticular, value);
        }

        [Association("Persona-PersonaDireccion")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<PersonaDireccion> Direcciones => GetCollection<PersonaDireccion>(nameof(Direcciones));

        [VisibleInLookupListView(true)]
        [Size(30)]
        [SearchMemberOptions(SearchMemberMode.Include)]
        [Indexed]
        [ImmediatePostData]
        [RuleUniqueValue("DocDuplicado", DefaultContexts.Save, CriteriaEvaluationBehavior = CriteriaEvaluationBehavior.BeforeTransaction, SkipNullOrEmptyValues = false)]
        [RuleRequiredField(DefaultContexts.Save, SkipNullOrEmptyValues = false, TargetCriteria = "CampoObligatorio('Documento', this)")]
        public string Documento
        {
            get => documento;
            set => SetPropertyValue(nameof(Documento), ref documento, value);
        }

        public int Edad
        {
            get => edad;
            set => SetPropertyValue(nameof(Edad), ref edad, value);
        }

        [Size(1)]
        public string FajaInformconf
        {
            get => fajaInformconf;
            set => SetPropertyValue(nameof(FajaInformconf), ref fajaInformconf, value);
        }

        [Association("Persona-PersonaEmpleo")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<PersonaEmpleo> Empleos => GetCollection<PersonaEmpleo>(nameof(Empleos));

        [Association("Persona-Empresa")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<Empresa> Empresas => GetCollection<Empresa>(nameof(Empresas));

        [Association("Nota-Persona")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<NotaPersona> Notas => GetCollection<NotaPersona>(nameof(Notas));

        [Association("Persona-SolicitudSeguimiento")]
        public XPCollection<SolicitudSeguimiento> Escribania => GetCollection<SolicitudSeguimiento>(nameof(Escribania));

        public EstadoCivil EstadoCivil
        {
            get => estadoCivil;
            set => SetPropertyValue(nameof(EstadoCivil), ref estadoCivil, value);
        }

        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm:ss}")]
        [ModelDefault("EditMask", "dd/MM/yyyy HH:mm:ss")]
        [Appearance("", Enabled = false, Criteria = "!UsuarioEnRol('Administrator')")] //todo parametrizar para SERSA
        public DateTime FechaAlta
        {
            get => fechaAlta;
            set => SetPropertyValue(nameof(FechaAlta), ref fechaAlta, value);
        }

        [PersistentAlias("GetDate(FechaAlta)")]
        [VisibleInDetailView(false)]
        [XafDisplayName("Fecha Alta (día)")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]
        public DateTime FechaAltaDia => (DateTime)EvaluateAlias(nameof(FechaAltaDia));

        [XafDisplayName("Usuario de Creación")]
        [ModelDefault("AllowEdit", "false")]
        public Usuario UsuarioCreacion
        {
            get => usuarioCreacion;
            set => SetPropertyValue(nameof(usuarioCreacion), ref usuarioCreacion, value);
        }

        [ToolTip("Fecha de nacimiento")]
        [ImmediatePostData]
        public DateTime FechaNacimiento
        {
            get => fechaNacimiento;
            set
            {
                if (value != null) value = value.Date;
                bool cambio = SetPropertyValue(nameof(FechaNacimiento), ref fechaNacimiento, value);
                if (IsLoading || IsSaving || !cambio) return;
                this.Edad = Varios.CalcularEdad(value);
                OnChanged(nameof(Edad));
            }
        }

        [XafDisplayName("Motivo de Rechazo")]
        public MotivoRechazo MotivoRechazo
        {
            get => motivoRechazo;
            set => SetPropertyValue(nameof(MotivoRechazo), ref motivoRechazo, value);
        }

        [XafDisplayName("Motivo de Descarte")]
        public MotivoDescarte MotivoDescarte
        {
            get => motivoDescarte;
            set => SetPropertyValue(nameof(MotivoDescarte), ref motivoDescarte, value);
        }

        public static void CambiarEstadoPersona(Persona persona, bool guardarReg)
        {

            if (persona != null)
            {
                if (persona.Estado?.Descripcion != "DESCARTADO" && persona.Estado?.Descripcion != "RECHAZADO") //Si el estado no se cambio manualmente
                {
                    //Posible
                    if (persona.Seguimientos.Any())
                        persona.Estado = persona.Session.FindObject<EstadoPersona>(CriteriaOperator.Parse("Descripcion= 'POSIBLE'"));

                    //Potencial
                    if (persona.Solicitudes.Count() > 0)
                    {
                        persona.Estado = persona.Session.FindObject<EstadoPersona>(CriteriaOperator.Parse("Descripcion= 'POTENCIAL'"));
                    }
                    else
                    {
                        var producto = persona.Session.FindObject<Producto>(Producto.Fields.Default == (CriteriaOperator)true);
                        bool cumpleRequisitosSol = true;

                        if (producto != null)
                        {
                            foreach (var item in producto.GeneracionSolicitud)
                            {
                                if ((bool)persona.Evaluate(item.Criterio) == false)
                                {
                                    cumpleRequisitosSol = false;
                                    break;
                                }
                            }
                        }
                        if (cumpleRequisitosSol)
                            persona.Estado = persona.Session.FindObject<EstadoPersona>(CriteriaOperator.Parse("Descripcion= 'POTENCIAL'"));
                    }
                }

                //Descartado o rechazado
                if (persona.Solicitudes != null)
                {
                    var solPer = (persona.Solicitudes.Where(s => s.Solicitud.Estado.Codigo == "DESCARTADA"
                                                || s.Solicitud.Estado.Codigo == "RECHAZADA")
                                                .OrderByDescending(o => o.Solicitud.Fecha).ToList());

                    if (solPer != null && solPer.Count() > 0 && solPer.Count() == persona.Solicitudes.Count()) //si el cliente solo tiene solicitudes descartadas o rechazadas
                    {
                        if (solPer[0].Solicitud.Estado?.Codigo == "DESCARTADA")
                        {
                            persona.Estado = persona.Session.FindObject<EstadoPersona>(CriteriaOperator.Parse("Descripcion = 'DESCARTADO'"));
                            persona.MotivoDescarte = solPer[0].Solicitud.MotivoDescarte;
                        }
                        if (solPer[0].Solicitud.Estado?.Codigo == "RECHAZADA")
                        {
                            persona.Estado = persona.Session.FindObject<EstadoPersona>(CriteriaOperator.Parse("Descripcion = 'RECHAZADO'"));
                            persona.MotivoRechazo = solPer[0].Solicitud.MotivoRechazo;
                        }
                    }

                    //Cliente
                    if (persona.Solicitudes.Any(s => s.Solicitud.Estado.Codigo == "FINALIZADA"))
                        persona.Estado = persona.Session.FindObject<EstadoPersona>(CriteriaOperator.Parse("Descripcion= 'CLIENTE'"));
                }

                if (guardarReg)
                {
                    persona.Save();
                }
            }
        }

        public new static FieldsClass Fields
        {
            get
            {
                if (ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
            }
        }

        public FormaDeSeguiminetoPreferida FormaDeSeguiminetoPreferida
        {
            get => formadeSeguiminetoPreferida;
            set => SetPropertyValue(nameof(FormaDeSeguiminetoPreferida), ref formadeSeguiminetoPreferida, value);
        }

        [Size(1)]
        public Genero Genero
        {
            get => genero;
            set => SetPropertyValue(nameof(Genero), ref genero, value);
        }

        [Size(100)]
        public string HorarioSeguimiento
        {
            get => horarioSeguimineto;
            set => SetPropertyValue(nameof(HorarioSeguimiento), ref horarioSeguimineto, value);
        }

        [Size(15)]
        public string Codigo
        {
            get => _codigo;
            set => SetPropertyValue(nameof(Codigo), ref _codigo, value);
        }

        [Association("Inmueble-Propietarios")]
        public XPCollection<Inmueble> Inmuebles => GetCollection<Inmueble>(nameof(Inmuebles));

        [CaptionsForBoolValues("Si", "No")]
        public bool? InteresadoPrestamo
        {
            get => interesadoPrestamo;
            set => SetPropertyValue(nameof(InteresadoPrestamo), ref interesadoPrestamo, value);
        }

        [ImmediatePostData]
        [DataSourceCriteria("Activo=true")]
        [LookupEditorMode(LookupEditorMode.AllItemsWithSearch)]
        public MedioIngreso MedioIngreso
        {
            get => medioIngreso;
            set => SetPropertyValue(nameof(MedioIngreso), ref medioIngreso, value);
        }

        public decimal MontoSolicitado
        {
            get => montoSolicitado;
            set => SetPropertyValue(nameof(MontoSolicitado), ref montoSolicitado, value);
        }

        public MotivoSolicitud MotivoSolicitud
        {
            get => motivoSolicitud;
            set => SetPropertyValue(nameof(MotivoSolicitud), ref motivoSolicitud, value);
        }

        [ImmediatePostData]
        [Appearance("", Enabled = false, Criteria = "Tipo.Codigo = 'F'", Context = nameof(DetailView))]
        [Size(250)]
        [SearchMemberOptions(SearchMemberMode.Include)]
        [VisibleInDetailView(false)]
        [ModelDefault("AllowEdit", "false")]
        public string NombreCompleto
        {
            get => nombreCompleto;
            set => SetPropertyValue(nameof(NombreCompleto), ref nombreCompleto, value);
        }

        [Size(100)]
        [ImmediatePostData]
        [Appearance("", Enabled = false, Criteria = "Tipo.Codigo= 'J'", Context = nameof(DetailView))]
        //[RuleRequiredField(DefaultContexts.Save)]
        public string PrimerNombre
        {
            get => primerNombre;
            set
            {
                bool cambio = SetPropertyValue(nameof(PrimerNombre), ref primerNombre, value);
                if (IsLoading || IsSaving || !cambio)
                {
                    return;
                }

                nombreCompleto = string.Concat(PrimerNombre, " ", SegundoNombre, " ", PrimerApellido, " ", SegundoApellido);
                OnChanged(nameof(NombreCompleto));
            }
        }

        [Size(100)]
        [ImmediatePostData]
        [Appearance("", Enabled = false, Criteria = "Tipo.Codigo= 'J'", Context = nameof(DetailView))]
        public string SegundoNombre
        {
            get => segundoNombre;
            set
            {
                bool cambio = SetPropertyValue(nameof(SegundoNombre), ref segundoNombre, value);
                if (IsLoading || IsSaving || !cambio)
                {
                    return;
                }

                nombreCompleto = string.Concat(PrimerNombre, " ", SegundoNombre, " ", PrimerApellido, " ", SegundoApellido);
                OnChanged(nameof(NombreCompleto));
            }
        }

        [Browsable(false)]
        [VisibleInDetailView(false)]
        public IObjectSpace ObjectSpace { get; set; }

        public EstadoPregunta PoseeMora
        {
            get => poseeMora;
            set => SetPropertyValue(nameof(PoseeMora), ref poseeMora, value);
        }

        [ModelDefault("AllowEdit", "false")]
        [XafDisplayName("Judicial")]
        public EstadoPregunta PoseeDemanda
        {
            get => poseeDemanda;
            set => SetPropertyValue(nameof(PoseeDemanda), ref poseeDemanda, value);
        }

        [XafDisplayName("Posee Inhibición")]
        public EstadoPregunta PoseeInhibicion
        {
            get => poseeInhibicion;
            set => SetPropertyValue(nameof(PoseeInhibicion), ref poseeInhibicion, value);
        }

        [XafDisplayName("Detalle de Mora/Demandas/Inhib.")]
        [Size(SizeAttribute.Unlimited)]
        public string OpeMoraDemandaInhiDet
        {
            get => opeConMoraDemandasInhiDet;
            set => SetPropertyValue(nameof(OpeMoraDemandaInhiDet), ref opeConMoraDemandasInhiDet, value);
        }

        [Size(SizeAttribute.Unlimited)]
        [XafDisplayName("Observación sobre motivo de rechazo o descarte")]
        public string ObservacionRechDesc
        {
            get => observacionRechDesc;
            set => SetPropertyValue(nameof(ObservacionRechDesc), ref observacionRechDesc, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("País emisor del documento de identidad")]
        public Pais PaisDocumento
        {
            get => paisDocumento;
            set => SetPropertyValue(nameof(PaisDocumento), ref paisDocumento, value);
        }

        public Pais Nacionalidad
        {
            get => nacionalidad;
            set => SetPropertyValue(nameof(Nacionalidad), ref nacionalidad, value);
        }

        public bool? ProcesoCreditoExplicado
        {
            get => procesoCreditoExplicado;
            set => SetPropertyValue(nameof(ProcesoCreditoExplicado), ref procesoCreditoExplicado, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public Profesion Profesion
        {
            get => profesion;
            set => SetPropertyValue(nameof(Profesion), ref profesion, value);
        }

        public PropietarioInquilino PropietarioInquilino
        {
            get => propietarioInquilino;
            set => SetPropertyValue(nameof(PropietarioInquilino), ref propietarioInquilino, value);
        }

        [ImmediatePostData]
        [Appearance("", Enabled = false, Criteria = "Tipo.Codigo = 'J'", Context = nameof(DetailView))]
        [Size(101)]
        public string RazonSocial
        {
            get => razonSocial;
            set => SetPropertyValue(nameof(RazonSocial), ref razonSocial, value);
        }

        [Association("Persona-Resultados")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<Resultado> Resultados => GetCollection<Resultado>(nameof(Resultados));

        [Size(50)]
        public string Ruc
        {
            get => ruc;
            set => SetPropertyValue(nameof(Ruc), ref ruc, value);
        }

        //[RuleRequiredField(DefaultContexts.SaveActualizarEstadoPersona
        public decimal Salario
        {
            get => salario;
            set => SetPropertyValue(nameof(Salario), ref salario, value);
        }

        public TipoIngreso TipoIngreso
        {
            get => tipoIngreso;
            set => SetPropertyValue(nameof(TipoIngreso), ref tipoIngreso, value);
        }

        protected override void OnSaving()
        {
            string actEstadoPersona = ConfigurationManager.AppSettings["ActualizarEstadoPersona"];

            if (actEstadoPersona == "S")
            {
                if (this.Estado == null)
                    this.Estado = this.Session.FindObject<EstadoPersona>(CriteriaOperator.Parse("Descripcion = 'NUEVO'"));

                if (this.Estado?.Descripcion == "RECHAZADO" && this.MotivoRechazo == null)
                    throw new Exception("Debe cargar un motivo de rechazo");

                if (this.Estado?.Descripcion == "DESCARTADO" && this.MotivoDescarte == null)
                    throw new Exception("Debe cargar un motivo de descarte");

                CambiarEstadoPersona(this, false);
            }

            this.DescripcionPersona = (string)this.Evaluate(CriteriaOperator.Parse(PDNOriginacionModule.GlbCriteriaDescPersonal));

            int maxNumero = 0;
            if (this.Empleos.Count > 0)
                maxNumero = Empleos.Max(e => e.NumeroEmpleo);

            PersonaEmpleo personaEmpleo = this.Empleos.Where(e => e.NumeroEmpleo == 0).FirstOrDefault();
            if (personaEmpleo != null)
            {
                personaEmpleo.NumeroEmpleo = maxNumero + 1;
                personaEmpleo.Save();
            }

            base.OnSaving();
        }

        [Association("Seguimiento-Persona")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<Seguimiento> Seguimientos => GetCollection<Seguimiento>(nameof(Seguimientos));

        [Association("SolicitudPersona-Persona")]
        public XPCollection<SolicitudPersona> Solicitudes => GetCollection<SolicitudPersona>(nameof(Solicitudes));

        [VisibleInLookupListView(true)]
        [SearchMemberOptions(SearchMemberMode.Include)]
        [PersistentAlias("Iif(Telefonos[Preferido].Count == 1,Telefonos[Preferido].Single(), null)")]
        public Telefono TelefonoPreferido => (Telefono)EvaluateAlias(nameof(TelefonoPreferido));

        [ImmediatePostData]
        [Size(250)]
        [SearchMemberOptions(SearchMemberMode.Include)]
        [ModelDefault("AllowEdit", "false")]
        public string DescripcionPersona
        {
            get => descripcionPersona;
            set => SetPropertyValue(nameof(DescripcionPersona), ref descripcionPersona, value);
        }
        /*[VisibleInLookupListView(true)]
        public string DescripcionPersona 
        {
            get => (string)this.Evaluate(CriteriaOperator.Parse(PDNOriginacionModule.GlbCriteriaDescPersonal));
        }*/

        /*        [Association("Persona-CuotaPagada")]
                public XPCollection<V_CuotaPagada> CuotasPagadas
                {
                    get
                    {
                        return GetCollection<V_CuotaPagada>(nameof(CuotasPagadas));
                    }
                }*/

        [Association("Persona-PrestamoCabecera")]
        public XPCollection<PrestamoCabecera> PrestamosCabecera
        {
            get
            {
                return GetCollection<PrestamoCabecera>(nameof(PrestamosCabecera));
            }
        }

        [Association("Persona-PrestamoCodeudor")]
        public XPCollection<PrestamoCabecera> PrestamosCodeudor
        {
            get
            {
                return GetCollection<PrestamoCabecera>(nameof(PrestamosCodeudor));
            }
        }

        [Association("Persona-Telefono")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<Telefono> Telefonos => GetCollection<Telefono>(nameof(Telefonos));

        [Association("Persona-PersonaVinculo")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<PersonaVinculo> Vinculos => GetCollection<PersonaVinculo>(nameof(Vinculos));

        [VisibleInLookupListView(true)]
        [PersistentAlias("Iif(Direcciones[Tipo.Codigo = 'P'].Count == 1,Direcciones[Tipo.Codigo = 'P'].Single(), null)")]
        public PersonaDireccion DireccionParticular => (PersonaDireccion)EvaluateAlias(nameof(DireccionParticular));

        [VisibleInLookupListView(true)]
        [PersistentAlias("Iif(Direcciones[Principal = true].Count == 1, Direcciones[Principal = true].Single(), null)")]
        public PersonaDireccion DireccionPrincipal => (PersonaDireccion)EvaluateAlias(nameof(DireccionPrincipal));

        [NonPersistent]
        public string Empleo1 => Empleos?.FirstOrDefault(e => e.NumeroEmpleo == 1)?.Empresa;

        [NonPersistent]
        public string Empleo2 => Empleos?.FirstOrDefault(e => e.NumeroEmpleo == 2)?.Empresa;

        [NonPersistent]
        public string Empleo3 => Empleos?.FirstOrDefault(e => e.NumeroEmpleo == 3)?.Empresa;

        [NonPersistent]
        public string Empleo4 => Empleos?.FirstOrDefault(e => e.NumeroEmpleo == 4)?.Empresa;

        [CaptionsForBoolValues("Sí", "No")]
        public bool? TieneComoDemostrarIngresos
        {
            get => tieneComoDemostrarIngresos;
            set => SetPropertyValue(nameof(TieneComoDemostrarIngresos), ref tieneComoDemostrarIngresos, value);
        }

        [ImmediatePostData]
        //[RuleRequiredField(DefaultContexts.Save)]
        public NaturalezaPersona Tipo
        {
            get => tipo;
            set => SetPropertyValue(nameof(Tipo), ref tipo, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        public TipoDocumento TipoDocumento
        {
            get => tipoDocumento;
            set => SetPropertyValue(nameof(TipoDocumento), ref tipoDocumento, value);
        }

        [XafDisplayName("Linea de crédito")]
        public Decimal LineaCredito
        {
            get => lineaCredito;
            set => SetPropertyValue(nameof(LineaCredito), ref lineaCredito, value);
        }

        public Decimal MaximaCuota
        {
            get => maximaCuota;
            set => SetPropertyValue(nameof(MaximaCuota), ref maximaCuota, value);
        }

        [NonPersistent]
        public int CantidadPrestamosActivos
        {
            get
            {
                if (!IsLoading && !IsSaving)
                    PrestamosActivos();
                return cantidadPrestamosActivos;
            }
        }

        [NonPersistent]
        public int CantidadPrestamosCancelados
        {
            get
            {
                if (!IsLoading && !IsSaving)
                    PrestamosCancelados();
                return cantidadPrestamosCancelados;
            }
        }

        private void PrestamosActivos()
        {
            // var totalPrestamos = CuotasPagadas.GroupBy(x => x.PrestamoNumero).Where(c => c.Sum(t => t.SaldoCuota) > 0);
            var totalPrestamos = PrestamosCabecera.Where(c => c.Estado == BusinessObjects.Estado.Activo);
            cantidadPrestamosActivos = totalPrestamos.Count();

            if (cantidadPrestamosActivos > 0)
            {
                estadoPersona = Session.FindObject<EstadoPersona>(EstadoPersona.Fields.Activo == (CriteriaOperator)true);
                // var judiciales = CuotasPagadas.GroupBy(x => new { x.PrestamoNumero, x.EnJuicio }).Where(y => y.Key.EnJuicio == "S");
                var judiciales = PrestamosCabecera.Where(c => c.Judicial == Judicial.Si);

                if (judiciales.Count() > 0)
                {
                    poseeDemanda = EstadoPregunta.Si;
                }
                else
                {
                    poseeDemanda = EstadoPregunta.No;
                }
            }
            else
            {
                estadoPersona = Session.FindObject<EstadoPersona>(EstadoPersona.Fields.Activo == (CriteriaOperator)false);
            }
        }

        private void PrestamosCancelados()
        {
            // var totalPrestamos = CuotasPagadas.GroupBy(x => x.PrestamoNumero).Where(c => c.Sum(t => t.SaldoCuota) <= 0);
            var totalPrestamos = PrestamosCabecera.Where(c => c.Estado == BusinessObjects.Estado.Cancelado);
            cantidadPrestamosCancelados = totalPrestamos.Count();
        }


        #region CargaRapidaInmueble
        [NonPersistent]
        [XafDisplayName("Cuenta Catastral")]
        public string CRI_CuentaCatastral
        {
            get
            {
                if (cri_cuentaCatastral == null || cri_cuentaCatastral == string.Empty)
                    return DateTime.Now.ToString("dd/MM") + SecuritySystem.CurrentUserName;
                else
                    return cri_cuentaCatastral;
            }
            set => SetPropertyValue(nameof(CRI_CuentaCatastral), ref cri_cuentaCatastral, value);
        }

        [NonPersistent]
        [DataSourceProperty("CRI_Departamento.Ciudades", DataSourcePropertyIsNullMode.SelectAll)]
        [ImmediatePostData]
        [XafDisplayName("Ciudad")]
        public Ciudad CRI_Ciudad
        {
            get => cri_ciudad;
            set => SetPropertyValue(nameof(CRI_Ciudad), ref cri_ciudad, value);
        }

        [ImmediatePostData]
        [NonPersistent]
        [XafDisplayName("Departamento")]
        public Departamento CRI_Departamento
        {
            get => cri_departamento;
            set
            {
                bool cambio = SetPropertyValue(nameof(CRI_Departamento), ref cri_departamento, value);
                if (!IsLoading && !IsSaving && cambio)
                {
                    if (cri_departamento != null)
                    {
                        cri_ciudad = Session.FindObject<Ciudad>(Ciudad.Fields.Departamento == cri_departamento & Ciudad.Fields.Default == (CriteriaOperator)true);
                    }
                    else
                    {
                        cri_ciudad = null;
                    }
                    OnChanged("CRI_Ciudad");
                }
            }
        }

        [DataSourceProperty("CRI_Ciudad.Barrios")]
        [NonPersistent]
        [XafDisplayName("Barrio")]
        public Barrio CRI_Barrio
        {
            get => cri_barrio;
            set => SetPropertyValue(nameof(CRI_Barrio), ref cri_barrio, value);
        }

        [NonPersistent]
        [XafDisplayName("Calle")]
        public string CRI_Calle
        {
            get
            {
                if (string.IsNullOrEmpty(cri_calle))
                    return "Sin asignar";
                else
                    return cri_calle;
            }
            set => SetPropertyValue(nameof(CRI_Calle), ref cri_calle, value);
        }

        [NonPersistent]
        [Size(8)]
        [XafDisplayName("Número")]
        public string CRI_Numero
        {
            get
            {
                if (string.IsNullOrEmpty(cri_numero))
                    return "Sin Nro";
                else
                    return cri_numero;
            }
            set => SetPropertyValue(nameof(CRI_Numero), ref cri_numero, value);
        }

        [NonPersistent]
        [XafDisplayName("Superficie en M2")]
        public decimal CRI_SuperficieM2
        {
            get => cri_superficieM2;
            set => SetPropertyValue(nameof(CRI_SuperficieM2), ref cri_superficieM2, value);
        }

        [NonPersistent]
        [XafDisplayName("Tipo de Camino")]
        public TipoCamino CRI_TipoCamino
        {
            get => cri_tipoCamino;
            set => SetPropertyValue(nameof(CRI_TipoCamino), ref cri_tipoCamino, value);
        }

        [NonPersistent]
        [XafDisplayName("Tipo de Inmueble")]
        public TipoInmueble CRI_TipoInmueble
        {
            get => cri_tipoInmueble;
            set => SetPropertyValue(nameof(CRI_TipoInmueble), ref cri_tipoInmueble, value);
        }

        [NonPersistent]
        [XafDisplayName("Valor Aproximado")]
        public decimal CRI_ValorAproximado
        {
            get => cri_valorAproximado;
            set => SetPropertyValue(nameof(CRI_ValorAproximado), ref cri_valorAproximado, value);
        }

        [NonPersistent]
        [XafDisplayName("Estado del Título")]  
        public EstadoTitulo CRI_EstadoTitulo
        {
            get => cri_estadoTitulo;
            set => SetPropertyValue(nameof(CRI_EstadoTitulo), ref cri_estadoTitulo, value);
        }

        [NonPersistent]
        [XafDisplayName("Tiene impuesto al día")]
        public bool CRI_ImpuestoAlDia
        {
            get => cri_impuestoAlDia;
            set => SetPropertyValue(nameof(CRI_ImpuestoAlDia), ref cri_impuestoAlDia, value);
        }

        [NonPersistent]
        [Size(500)]
        [XafDisplayName("Observaciones")]
        public string CRI_Observaciones
        {
            get => cri_observaciones;
            set => SetPropertyValue(nameof(CRI_Observaciones), ref cri_observaciones, value);
        }
        #endregion CargaRapidaInmueble

        #region CargaRapidaDireccion
        [NonPersistent]
        [ImmediatePostData]
        [XafDisplayName("País")]
        public Pais CRD_Pais
        {
            get 
            {
                if (crd_pais != null)
                    return  crd_pais;
                else
                    return Session.FindObject<Pais>(Pais.Fields.Codigo == "1");
            }
            set
            {
                bool cambio = SetPropertyValue(nameof(CRD_Pais), ref crd_pais, value);
                if (!IsLoading && !IsSaving && cambio)
                {
                    if (crd_pais != null)
                    {
                        crd_departamento = null;
                    }
                    OnChanged("CRD_Departamento");
                }
            }
        }

        [NonPersistent]
        [DataSourceProperty("CRD_Departamento.Ciudades")]
        [ImmediatePostData]
        [XafDisplayName("Ciudad")]
        public Ciudad CRD_Ciudad
        {
            get => crd_ciudad;
            set => SetPropertyValue(nameof(CRD_Ciudad), ref crd_ciudad, value);
        }

        [ImmediatePostData]
        [NonPersistent]
        [XafDisplayName("Departamento")]
        public Departamento CRD_Departamento
        {
            get => crd_departamento;
            set
            {
                bool cambio = SetPropertyValue(nameof(CRD_Departamento), ref crd_departamento, value);
                if (!IsLoading && !IsSaving && cambio)
                {
                    if (crd_departamento != null)
                    {
                        crd_ciudad = Session.FindObject<Ciudad>(Ciudad.Fields.Departamento == crd_departamento & Ciudad.Fields.Default == (CriteriaOperator)true);
                    }
                    else
                    {
                        crd_ciudad = null;
                    }
                    OnChanged("CRD_Ciudad");
                }
            }
        }

        [DataSourceProperty("CRD_Ciudad.Barrios")]
        [NonPersistent]
        [XafDisplayName("Barrio")]
        public Barrio CRD_Barrio
        {
            get => crd_barrio;
            set => SetPropertyValue(nameof(CRD_Barrio), ref crd_barrio, value);
        }

        [NonPersistent]
        [XafDisplayName("Calle")]
        public string CRD_Calle
        {
            get
            {
                if (string.IsNullOrEmpty(crd_calle))
                    return "Sin asignar";
                else
                    return crd_calle;
            }
            set => SetPropertyValue(nameof(CRD_Calle), ref crd_calle, value);
        }

        [NonPersistent]
        [XafDisplayName("Número")]
        [Size(8)]
        public string CRD_Numero
        {
            get
            {
                if (string.IsNullOrEmpty(crd_numero))
                    return "Sin Nro";
                else
                    return crd_numero;
            }
            set => SetPropertyValue(nameof(CRD_Numero), ref crd_numero, value);
        }
        #endregion CargaRapidaDireccion

        #region CargaRapidaTefono 
        [NonPersistent]
        [XafDisplayName("Tipo de teléfono")]
        public TipoTelefono CRT_TipoTelefono
        {
            get => crt_tipoTelefono;
            set
            {
                bool cambio = SetPropertyValue(nameof(CRT_TipoTelefono), ref crt_tipoTelefono, value);
                if (IsLoading || IsSaving || !cambio)
                {
                    return;
                }
            }
        }

        [NonPersistent]
        [XafDisplayName("Whatsapp")]
        public bool CRT_Whatsapp
        {
            get => crt_whatsapp;
            set => SetPropertyValue(nameof(CRT_Whatsapp), ref crt_whatsapp, value);
        }

   

        [DataSourceCriteria("Tipo = '@This.CRT_Tipo'")]
        [NonPersistent]
        [XafDisplayName("Prefijo")]
        public Prefijo CRT_Prefijo
        {
            get => crt_prefijo;
            set
            {
                bool cambio = SetPropertyValue(nameof(CRT_Prefijo), ref crt_prefijo, value);
                if (IsLoading || IsSaving || !cambio)
                {
                    return;
                }                
            }
        }

        [Size(10)]
        [NonPersistent]
        [XafDisplayName("Número")]
        public string CRT_Numero
        {
            get => crt_numero;
            set
            {
                bool cambio = SetPropertyValue(nameof(CRT_Numero), ref crt_numero, value);
                if (IsLoading || IsSaving || !cambio)
                {
                    return;
                }
           }
        }

        [NonPersistent]
        [ImmediatePostData]
        [XafDisplayName("Tipo")]
        public GrupoTelefono CRT_Tipo
        {
            get => crt_tipo;
            set => SetPropertyValue(nameof(CRT_Tipo), ref crt_tipo, value);
        }

        [NonPersistent]
        [XafDisplayName("Preferido")]
        public bool CRT_Preferido
        {
            get => crt_preferido;
            set
            {
                SetPropertyValue(nameof(CRT_Preferido), ref crt_preferido, value);             
            }
        }
        #endregion CargaRapidaTelefono

        private XPCollection<AuditDataItemPersistent> auditoria;

        public XPCollection<AuditDataItemPersistent> Auditoria
        {
            get
            {
                if (auditoria == null)
                {
                    auditoria = AuditedObjectWeakReference.GetAuditTrail(Session, this);
                }
                return auditoria;
            }
        }

        public new class FieldsClass : PersistentBase.FieldsClass
        {
            public FieldsClass()
            {

            }

            public FieldsClass(string propertyName) : base(propertyName)
            {

            }

            public OperandProperty Adjuntos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Adjuntos"));
                }
            }

            public OperandProperty Agenda
            {
                get
                {
                    return new OperandProperty(GetNestedName("Agenda"));
                }
            }

            public OperandProperty Antiguedad
            {
                get
                {
                    return new OperandProperty(GetNestedName("Antiguedad"));
                }
            }

            public OperandProperty PrimerApellido
            {
                get
                {
                    return new OperandProperty(GetNestedName("PrimerApellido"));
                }
            }

            public OperandProperty SegundoApellido
            {
                get
                {
                    return new OperandProperty(GetNestedName("SegundoApellido"));
                }
            }

            public OperandProperty CantSeguimientos
            {
                get
                {
                    return new OperandProperty(GetNestedName("CantSeguimientos"));
                }
            }

            public OperandProperty CantSolicitudes
            {
                get
                {
                    return new OperandProperty(GetNestedName("CantSolicitudes"));
                }
            }

            public OperandProperty ConDocumento
            {
                get
                {
                    return new OperandProperty(GetNestedName("ConDocumento"));
                }
            }

            public OperandProperty Contacto
            {
                get
                {
                    return new OperandProperty(GetNestedName("Contacto"));
                }
            }

            public EstadoPersona.FieldsClass Estado
            {
                get
                {
                    return new EstadoPersona.FieldsClass(GetNestedName("Estado"));
                }
            }

            public OperandProperty CorreoLaboral
            {
                get
                {
                    return new OperandProperty(GetNestedName("CorreoLaboral"));
                }
            }

            public OperandProperty CorreoParticular
            {
                get
                {
                    return new OperandProperty(GetNestedName("CorreoParticular"));
                }
            }

            public OperandProperty Direcciones
            {
                get
                {
                    return new OperandProperty(GetNestedName("Direcciones"));
                }
            }

            public OperandProperty Documento
            {
                get
                {
                    return new OperandProperty(GetNestedName("Documento"));
                }
            }

            public OperandProperty Edad
            {
                get
                {
                    return new OperandProperty(GetNestedName("Edad"));
                }
            }

            public OperandProperty Empleos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Empleos"));
                }
            }

            public OperandProperty Empresas
            {
                get
                {
                    return new OperandProperty(GetNestedName("Empresas"));
                }
            }

            public OperandProperty Notas
            {
                get
                {
                    return new OperandProperty(GetNestedName("Notas"));
                }
            }

            public OperandProperty EstadoCivil
            {
                get
                {
                    return new OperandProperty(GetNestedName("EstadoCivil"));
                }
            }

            public OperandProperty FechaAlta
            {
                get
                {
                    return new OperandProperty(GetNestedName("FechaAlta"));
                }
            }

            public OperandProperty FechaNacimiento
            {
                get
                {
                    return new OperandProperty(GetNestedName("FechaNacimiento"));
                }
            }

            public OperandProperty FormaDeSeguiminetoPreferida
            {
                get
                {
                    return new OperandProperty(GetNestedName("FormaDeSeguiminetoPreferida"));
                }
            }

            public OperandProperty Genero
            {
                get
                {
                    return new OperandProperty(GetNestedName("Genero"));
                }
            }

            public OperandProperty HorarioSeguimiento
            {
                get
                {
                    return new OperandProperty(GetNestedName("HorarioSeguimiento"));
                }
            }

            public OperandProperty Inmuebles
            {
                get
                {
                    return new OperandProperty(GetNestedName("Inmuebles"));
                }
            }

            public OperandProperty InteresadoPrestamo
            {
                get
                {
                    return new OperandProperty(GetNestedName("InteresadoPrestamo"));
                }
            }

            public MedioIngreso.FieldsClass MedioIngreso
            {
                get
                {
                    return new MedioIngreso.FieldsClass(GetNestedName("MedioIngreso"));
                }
            }

            public OperandProperty MontoSolicitado
            {
                get
                {
                    return new OperandProperty(GetNestedName("MontoSolicitado"));
                }
            }

            public MotivoSolicitud.FieldsClass MotivoSolicitud
            {
                get
                {
                    return new MotivoSolicitud.FieldsClass(GetNestedName("MotivoSolicitud"));
                }
            }

            public OperandProperty NombreCompleto
            {
                get
                {
                    return new OperandProperty(GetNestedName("NombreCompleto"));
                }
            }

            public OperandProperty PrimerNombre
            {
                get
                {
                    return new OperandProperty(GetNestedName("PrimerNombre"));
                }
            }

            public OperandProperty SegundoNombre
            {
                get
                {
                    return new OperandProperty(GetNestedName("SegundoNombre"));
                }
            }

            public OperandProperty ObjectSpace
            {
                get
                {
                    return new OperandProperty(GetNestedName("ObjectSpace"));
                }
            }

            public OperandProperty OpeMoraDemandaInhi
            {
                get
                {
                    return new OperandProperty(GetNestedName("OpeMoraDemandaInhi"));
                }
            }

            public OperandProperty OpeMoraDemandaInhiDet
            {
                get
                {
                    return new OperandProperty(GetNestedName("OpeMoraDemandaInhiDet"));
                }
            }

            public Pais.FieldsClass PaisDocumento
            {
                get
                {
                    return new Pais.FieldsClass(GetNestedName("PaisDocumento"));
                }
            }

            public OperandProperty PoseeInmueble
            {
                get
                {
                    return new OperandProperty(GetNestedName("PoseeInmueble"));
                }
            }

            public OperandProperty ProcesoCreditoExplicado
            {
                get
                {
                    return new OperandProperty(GetNestedName("ProcesoCreditoExplicado"));
                }
            }

            public OperandProperty Profesion
            {
                get
                {
                    return new OperandProperty(GetNestedName("Profesion"));
                }
            }

            public OperandProperty PropietarioInquilino
            {
                get
                {
                    return new OperandProperty(GetNestedName("PropietarioInquilino"));
                }
            }

            public OperandProperty RazonSocial
            {
                get
                {
                    return new OperandProperty(GetNestedName("RazonSocial"));
                }
            }

            public OperandProperty Resultados
            {
                get
                {
                    return new OperandProperty(GetNestedName("Resultados"));
                }
            }

            public OperandProperty Ruc
            {
                get
                {
                    return new OperandProperty(GetNestedName("Ruc"));
                }
            }

            public OperandProperty Salario
            {
                get
                {
                    return new OperandProperty(GetNestedName("Salario"));
                }
            }

            public OperandProperty Seguimientos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Seguimientos"));
                }
            }

            public OperandProperty Solicitudes
            {
                get
                {
                    return new OperandProperty(GetNestedName("Solicitudes"));
                }
            }

            public Telefono.FieldsClass TelefonoPreferido
            {
                get
                {
                    return new Telefono.FieldsClass(GetNestedName("TelefonoPreferido"));
                }
            }

            public OperandProperty Telefonos
            {
                get
                {
                    return new OperandProperty(GetNestedName("Telefonos"));
                }
            }

            public OperandProperty CuotasPagadas
            {
                get
                {
                    return new OperandProperty(GetNestedName("CuotasPagadas"));
                }
            }

            public PersonaDireccion.FieldsClass DireccionParticular
            {
                get
                {
                    return new PersonaDireccion.FieldsClass(GetNestedName("DireccionParticular"));
                }
            }

            public OperandProperty TieneComoDemostrarIngresos
            {
                get
                {
                    return new OperandProperty(GetNestedName("TieneComoDemostrarIngresos"));
                }
            }

            public NaturalezaPersona.FieldsClass Tipo
            {
                get
                {
                    return new NaturalezaPersona.FieldsClass(GetNestedName("Tipo"));
                }
            }

            public TipoDocumento.FieldsClass TipoDocumento
            {
                get
                {
                    return new TipoDocumento.FieldsClass(GetNestedName("TipoDocumento"));
                }
            }
        }
    }
}

namespace PDNOriginacion.Module.BusinessObjects
{
    public enum EstadoPregunta
    {
        Si,
        No,
        NoSabe
    }
}