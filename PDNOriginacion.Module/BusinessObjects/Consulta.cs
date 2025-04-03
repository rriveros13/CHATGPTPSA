using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.ComponentModel;

namespace PDNOriginacion.Module.BusinessObjects
{
    [DefaultClassOptions]
    [DefaultProperty("Id")]
    [RuleCriteria("MontoRule", DefaultContexts.Save, "iif(TipoProducto.DescripcionPDN!='DESCUENTO', Monto > 0, true)", "Monto debe ser mayor que 0", SkipNullOrEmptyValues = false)]
    /*[Appearance("NoMostrarScoring", AppearanceItemType = "Action", TargetItems = "Consulta.ActionMethod", Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "Resultados.Count > 0")]
    [Appearance("NoMostrarEnviar", AppearanceItemType = "Action", TargetItems = "Consulta.EnviarSolicitud", Context = "Any", Visibility = ViewItemVisibility.Hide, Criteria = "Resultados.Count = 0 or Enviado = true")]*/
    // [Appearance("NoPermitirEdicion", AppearanceItemType = "ViewItem", TargetItems = "*;Notas;Adjuntos", Context = "Any", Enabled = false, Criteria = "!IsNull(Resultados)", Priority = 1)]
    //[Appearance("MostrarConsultasAprobadas", AppearanceItemType = "ViewItem", TargetItems = "*", Criteria = "Estado = 4", Context = "ListView", FontColor = "Green", FontStyle = System.Drawing.FontStyle.Bold, Priority = 2)]
    //[Appearance("MostrarConsultasRechazadas", AppearanceItemType = "ViewItem", TargetItems = "*", Criteria = "Estado = 5", Context = "ListView", FontColor = "Red", FontStyle = System.Drawing.FontStyle.Bold, Priority = 2)]*/
    public class Consulta : BaseObject//, IObjectSpaceLink
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (http://documentation.devexpress.com/#Xaf/CustomDocument3146).

        static FieldsClass _Fields;
        private Cliente _cliente;
        private string _documento;
        /*private string _codigo_producto;
        private bool _enviado;*/
        private ConsultaEstados _estado;
        private DateTime _fecha;
        //[Appearance("NoMostrarObjectSpace", AppearanceItemType = "ViewItem", TargetItems = "ObjectSpace", Context = "Any", Visibility = ViewItemVisibility.Hide)]
        // public IObjectSpace ObjectSpace { get; set; }
        private Modelo _modelo;
        private int _monto;
        private Pais _paisfocumento;
        private TipoDocumento _tipodocumento;
        private string _usuario;

        public Consulta(Session session) : base(session)
        {
            //GetClienteUser.Register();
        }

        protected override void OnSaving()
        {
            if(Adjuntos.Count == 0)
            {
                Adjunto a1 = new Adjunto(Session)
                {
                    Adjuntado = false,
                    Consulta = this,
                    Descripcion = "cedula_frente",
                    Fecha = DateTime.Now
                };
                a1.Save();

                Adjunto a2 = new Adjunto(Session)
                {
                    Adjuntado = false,
                    Consulta = this,
                    Descripcion = "cedula_reverso",
                    Fecha = DateTime.Now
                };
                a2.Save();
            }

            base.OnSaving();
        }
        /*[Association("Consulta-Notas"), DevExpress.Xpo.Aggregated]
        [NonCloneable]
        public XPCollection<Nota> Notas
        {
        get { return GetCollection<Nota>("Notas"); }
        }*/
        /*private void EnviarSolicitud()
        {
        if (this.Resultados[0].Accion.ToUpper().Equals("RECHAZAR"))
        throw new Exception("La solicitud fue rechazada por el motor de decisión. No puede enviar la solicitud");
        else if ((this.Adjuntos.Where(r => r.archivoAdjuntado == false)).Count() > 0)
        throw new Exception("No se adjuntaron todos los documentos requeridos.");
        else
        EnviarDatosCK();
        }
        
        [Appearance("CodigoProducto", Enabled = false, Context = "DetailView")]
        public string CodigoProducto
        {
        get { return _codigo_producto; }
        set { SetPropertyValue("CodigoProducto", ref _codigo_producto, value); }
        }
        
        [Appearance("Enviado", Enabled = false, Context = "DetailView")]
        public bool Enviado
        {
        get { return _enviado; }
        set { SetPropertyValue("Enviado", ref _enviado, value); }
        }
        
        private void EnviarDatosCK()
        {
        CKImportador.ImportServiceClient serv = new CKImportador.ImportServiceClient();
        string xmlCliente = @"<credikac_import>
        	<client>
        		<active>true</active>
        		<birthdate>10/01/1989</birthdate>
        		<civilstatus>CA</civilstatus>
        		<document>" + this.Documento + @"</document>
        		<documenttype>CI</documenttype>
        		<gender>M</gender>
        		<email>hola@prueba.com</email>
        		<name>" + this.Resultados[0].Nombres + @"</name>
        		<nationality>PY</nationality>
        		<phonenumber>1111222333</phonenumber>
        		<surname>" + this.Resultados[0].Apellidos + @"</surname>
        		<type>0</type>
        		<address_value>25 de mayo y Mayor Fleitas, Asunción</address_value>
        		<address_number>366</address_number>
        		<address_aditionalinfo>Edificio de la esquina</address_aditionalinfo>
        		<address_principal>true</address_principal>
        		<address_latitud>25.121212212</address_latitud>
        		<address_longitud>-25.5646545</address_longitud>
        		<address_addresstype>PA</address_addresstype>
        	</client>
        </credikac_import>
        ";
        string res = serv.Client_CreateOrUpdate("a97f3f7e6c37c0ec5c6414e5d6c2f02f", "bflores", "1hojitaCK", xmlCliente);
        
        int codProducto = (new Random()).Next(9999999, 99999999);
        string xmlProducto = @"<credikac_import>
        <product>
        <type>SOL-IT</type>
        <enddate>10/01/1989</enddate>
        <clientdocumenttype>CI</clientdocumenttype>
        <clientdocument>" + this.Documento + @"</clientdocument>
        <amount>" + this.Monto.ToString() + @"</amount>
        <dues>0</dues>
        <code>" + codProducto.ToString() + @"</code>
        <pin>ASDF</pin>
        <active>true</active>
        </product>
        </credikac_import>
        ";
        res = serv.Product_CreateOrUpdate("a97f3f7e6c37c0ec5c6414e5d6c2f02f", "bflores", "1hojitaCK", xmlProducto);
        
        this.Enviado = true;
        this.CodigoProducto = codProducto.ToString();
        this.Save();
        }
        
        
        
        private void GuardarDatosInfocheck(Resultado nres, XmlNode nodeResultados)
        {
        /* Resultado_Icheck resultadoI = new Resultado_Icheck(nres.Session);
        resultadoI.Resultado = nres;
        
        #region Cargar datos a Resultados Icheck
        resultadoI.Nombres = nodeResultados.SelectSingleNode("nombres")?.InnerText;
        resultadoI.Apellidos = nodeResultados.SelectSingleNode("apellidos")?.InnerText;
        resultadoI.Explicacion = nodeResultados.SelectSingleNode("explicacion")?.InnerText;
        resultadoI.ich_inhab_cant = Int32.Parse(nodeResultados.SelectSingleNode("ich_inhab_cant")?.InnerText ?? "0");
        resultadoI.ich_ch_sus_cant_total = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_sus_cant_total")?.InnerText ?? "0");
        resultadoI.ich_ch_sus_dist_banco = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_sus_dist_banco")?.InnerText ?? "0");
        resultadoI.ich_ch_c_emb_cant_total = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_c_emb_cant_total")?.InnerText ?? "0");
        resultadoI.ich_ch_c_emb_dist_banco = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_c_emb_dist_banco")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_cant_giro = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_cant_giro")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_cant_i_fon = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_cant_i_fon")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_cant_def_f = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_cant_def_f")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_cant_no_imput = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_cant_no_imput")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_dist_banco = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_dist_banco")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_monto_giro_gs = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_monto_giro_gs")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_i_fon_gs = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_i_fon_gs")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_def_f_gs = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_def_f_gs")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_monto_ot_mon = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_monto_ot_mon")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_monto_giro_ot_mon = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_monto_giro_ot_mon")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_monto_i_fon_ot_mon = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_monto_i_fon_ot_mon")?.InnerText ?? "0");
        resultadoI.ich_ch_re_h_monto_def_f_ot_mon = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_monto_def_f_ot_mon")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_cant_total = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_cant_total")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_admin = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_admin")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_ccte_com = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_ccte_com")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_sg = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_sg")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_cred_com = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_cred_com")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_dto_ch_doc = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_dto_ch_doc")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_cred_imp_cta_exp = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_cred_imp_cta_exp")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_pr_per = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_pr_per")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_tarj = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_tarj")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_otros = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_otros")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_cant_dist_ent = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_cant_dist_ent")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_cant_banco = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_cant_banco")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_cant_finan = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_cant_finan")?.InnerText ?? "0");
        resultadoI.ich_cant_inhab_dist = Int32.Parse(nodeResultados.SelectSingleNode("ich_cant_inhab_dist")?.InnerText ?? "0");
        resultadoI.ich_cons_cant_total = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_cant_total")?.InnerText ?? "0");
        resultadoI.ich_cons_cant_dist_ent = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_cant_dist_ent")?.InnerText ?? "0");
        resultadoI.ich_cons_cant_bancos = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_cant_bancos")?.InnerText ?? "0");
        resultadoI.ich_cons_cant_fin = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_cant_fin")?.InnerText ?? "0");
        resultadoI.ich_cons_cant_coop = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_cant_coop")?.InnerText ?? "0");
        resultadoI.ich_cons_cant_comercio = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_cant_comercio")?.InnerText ?? "0");
        resultadoI.ich_situacion_bancaria = nodeResultados.SelectSingleNode("ich_situacion_bancaria")?.InnerText;
        resultadoI.ich_ch_re_h_monto_gs = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_h_monto_gs")?.InnerText ?? "0");
        resultadoI.ich_ch_re_monto_gs = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_monto_gs")?.InnerText ?? "0");
        resultadoI.ich_cons_priv_banco_cred_com = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_priv_banco_cred_com")?.InnerText ?? "0");
        resultadoI.ich_meses_ult_rech = Int32.Parse(nodeResultados.SelectSingleNode("ich_meses_ult_rech")?.InnerText ?? "0");
        resultadoI.ich_ch_rech_i_fon_dist_banc = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_rech_i_fon_dist_banc")?.InnerText ?? "0");
        resultadoI.ich_ch_rech_cant_u60m = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_rech_cant_u60m")?.InnerText ?? "0");
        resultadoI.ich_ch_rech_noreg_cant_i_fon = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_rech_noreg_cant_i_fon")?.InnerText ?? "0");
        resultadoI.ich_ch_rech_noreg_cant_i_fon_u3m = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_rech_noreg_cant_i_fon_u3m")?.InnerText ?? "0");
        resultadoI.ich_peor_tipo_falta = Int32.Parse(nodeResultados.SelectSingleNode("ich_peor_tipo_falta")?.InnerText ?? "0");
        resultadoI.ich_ch_re_cant_total = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_re_cant_total")?.InnerText ?? "0");
        resultadoI.ich_ch_re_cant_giro = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_re_cant_giro")?.InnerText ?? "0");
        resultadoI.ich_ch_re_cant_i_fond = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_re_cant_i_fond")?.InnerText ?? "0");
        resultadoI.ich_ch_re_cant_def_f = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_re_cant_def_f")?.InnerText ?? "0");
        resultadoI.ich_ch_re_cant_no_imput = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_re_cant_no_imput")?.InnerText ?? "0");
        resultadoI.ich_ch_re_dist_banco = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_re_dist_banco")?.InnerText ?? "0");
        resultadoI.ich_ch_re_monto_giro_gs = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_monto_giro_gs")?.InnerText ?? "0");
        resultadoI.ich_ch_re_monto_i_fond_gs = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_monto_i_fond_gs")?.InnerText ?? "0");
        resultadoI.ich_ch_re_monto_def_f_gs = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_monto_def_f_gs")?.InnerText ?? "0");
        resultadoI.ich_ch_re_monto_ot_mon = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_monto_ot_mon")?.InnerText ?? "0");
        resultadoI.ich_ch_re_monto_giro_ot_mon = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_monto_giro_ot_mon")?.InnerText ?? "0");
        resultadoI.ich_ch_re_monto_i_fond_ot_mon = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_monto_i_fond_ot_mon")?.InnerText ?? "0");
        resultadoI.ich_ch_re_monto_def_f_ot_mon = decimal.Parse(nodeResultados.SelectSingleNode("ich_ch_re_monto_def_f_ot_mon")?.InnerText ?? "0");
        resultadoI.ich_ch_anu_cant_total = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_anu_cant_total")?.InnerText ?? "0");
        resultadoI.ich_ch_anu_dist_banco = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_anu_dist_banco")?.InnerText ?? "0");
        resultadoI.ich_ch_ext_cant_total = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_ext_cant_total")?.InnerText ?? "0");
        resultadoI.ich_ch_ext_dist_banco = Int32.Parse(nodeResultados.SelectSingleNode("ich_ch_ext_dist_banco")?.InnerText ?? "0");
        resultadoI.ich_cant_inhab_hist = Int32.Parse(nodeResultados.SelectSingleNode("ich_cant_inhab_hist")?.InnerText ?? "0");
        resultadoI.ich_cons_cant_proc_tar = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_cant_proc_tar")?.InnerText ?? "0");
        
        DateTime iconf_fecha_sol;
        if (DateTime.TryParseExact(nodeResultados.SelectSingleNode("iconf_fecha_solicitud")?.InnerText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out iconf_fecha_sol))
        resultadoI.iconf_fecha_solicitud = iconf_fecha_sol;
        #endregion Cargar datos a Resultados Icheck
        }
        
        private void GuardarDatosInformconfCC(Resultado nres, XmlNode nodeResultados)
        {
        /* Resultado_Informconf_CC resultadoI = new Resultado_Informconf_CC(nres.Session);
        resultadoI.Resultado = nres;
        
        #region Cargar datos a tabla de Resultados Iconf
        resultadoI.Nombres = nodeResultados.SelectSingleNode("nombres")?.InnerText;
        resultadoI.Apellidos = nodeResultados.SelectSingleNode("apellidos")?.InnerText;
        resultadoI.Explicacion = nodeResultados.SelectSingleNode("explicacion")?.InnerText;
        resultadoI.banco_cheque = nodeResultados.SelectSingleNode("banco_cheque")?.InnerText;
        resultadoI.cantidad_operaciones = Int32.Parse(nodeResultados.SelectSingleNode("cantidad_operaciones")?.InnerText ?? "0");
        resultadoI.che_rech_activos = Int32.Parse(nodeResultados.SelectSingleNode("che_rech_activos")?.InnerText ?? "0");
        resultadoI.che_rech_inactivos = Int32.Parse(nodeResultados.SelectSingleNode("che_rech_inactivos")?.InnerText ?? "0");
        resultadoI.dias_morosidad_interna = Int32.Parse(nodeResultados.SelectSingleNode("dias_morosidad_interna")?.InnerText ?? "0");
        resultadoI.ich_cons_cant_total_dist_cli = Int32.Parse(nodeResultados.SelectSingleNode("ich_cons_cant_total_dist_cli")?.InnerText ?? "0");
        resultadoI.ich_meses_ult_cons_banc = Int32.Parse(nodeResultados.SelectSingleNode("ich_meses_ult_cons_banc")?.InnerText ?? "0");
        resultadoI.ich_meses_ult_cons_finan = Int32.Parse(nodeResultados.SelectSingleNode("ich_meses_ult_cons_finan")?.InnerText ?? "0");
        resultadoI.iconf_apellido_casada = nodeResultados.SelectSingleNode("iconf_apellido_casada")?.InnerText;
        resultadoI.iconf_apellido_casada_cony = nodeResultados.SelectSingleNode("iconf_apellido_casada_cony")?.InnerText;
        resultadoI.iconf_cons_cant_u1m = Int32.Parse(nodeResultados.SelectSingleNode("iconf_cons_cant_u1m")?.InnerText ?? "0");
        resultadoI.iconf_cons_detalle = nodeResultados.SelectSingleNode("iconf_cons_detalle")?.InnerText;
        resultadoI.iconf_cons_detalle_cony = nodeResultados.SelectSingleNode("iconf_cons_detalle_cony")?.InnerText;
        resultadoI.iconf_convocatorias_resumen = nodeResultados.SelectSingleNode("iconf_convocatorias_resumen")?.InnerText;
        resultadoI.iconf_conv_cant_abiertas = Int32.Parse(nodeResultados.SelectSingleNode("iconf_conv_cant_abiertas")?.InnerText ?? "0");
        if (nodeResultados.SelectSingleNode("iconf_conv_cant_abiertas_conyuge").InnerText != string.Empty)
        resultadoI.iconf_conv_cant_abiertas_conyuge = Int32.Parse(nodeResultados.SelectSingleNode("iconf_conv_cant_abiertas_conyuge")?.InnerText ?? "0");
        resultadoI.iconf_conv_resumen_cony = nodeResultados.SelectSingleNode("iconf_conv_resumen_cony")?.InnerText;
        resultadoI.iconf_demandas_resumen = nodeResultados.SelectSingleNode("iconf_demandas_resumen")?.InnerText;
        resultadoI.iconf_dem_cant_abiertas = Int32.Parse(nodeResultados.SelectSingleNode("iconf_dem_cant_abiertas")?.InnerText ?? "0");
        resultadoI.iconf_dem_cant_abiertas_spub = Int32.Parse(nodeResultados.SelectSingleNode("iconf_dem_cant_abiertas_spub")?.InnerText ?? "0");
        resultadoI.iconf_dem_cant_abiertas_telco = Int32.Parse(nodeResultados.SelectSingleNode("iconf_dem_cant_abiertas_telco")?.InnerText ?? "0");
        resultadoI.iconf_dem_resumen_cony = nodeResultados.SelectSingleNode("iconf_dem_resumen_cony")?.InnerText;
        resultadoI.iconf_direccion_cony = nodeResultados.SelectSingleNode("iconf_direccion_cony")?.InnerText;
        resultadoI.iconf_direc_detalle = nodeResultados.SelectSingleNode("iconf_direc_detalle")?.InnerText;
        resultadoI.iconf_documento_cony = nodeResultados.SelectSingleNode("iconf_documento_cony")?.InnerText;
        resultadoI.iconf_estado_civil = nodeResultados.SelectSingleNode("iconf_estado_civil")?.InnerText;
        resultadoI.iconf_estado_civil_cony = nodeResultados.SelectSingleNode("iconf_estado_civil_cony")?.InnerText;
        resultadoI.iconf_faja_score = nodeResultados.SelectSingleNode("iconf_faja_score")?.InnerText;
        resultadoI.iconf_faja_score_conyuge = nodeResultados.SelectSingleNode("iconf_faja_score_conyuge")?.InnerText;
        
        DateTime iconf_fecha_dem_ult_finiq;
        if (DateTime.TryParseExact(nodeResultados.SelectSingleNode("iconf_fecha_dem_ult_finiq")?.InnerText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out iconf_fecha_dem_ult_finiq))
        resultadoI.iconf_fecha_dem_ult_finiq = iconf_fecha_dem_ult_finiq;
        
        DateTime iconf_fecha_nac;
        if (DateTime.TryParseExact(nodeResultados.SelectSingleNode("iconf_fecha_nac")?.InnerText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out iconf_fecha_nac))
        resultadoI.iconf_fecha_nac = iconf_fecha_nac;
        
        DateTime iconf_fecha_nac_cony;
        if (DateTime.TryParseExact(nodeResultados.SelectSingleNode("iconf_fecha_nac_cony")?.InnerText, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out iconf_fecha_nac_cony))
        resultadoI.iconf_fecha_nac_cony = iconf_fecha_nac_cony;
        
        resultadoI.iconf_inc_cant_abiertas = Int32.Parse(nodeResultados.SelectSingleNode("iconf_inc_cant_abiertas")?.InnerText ?? "0");
        if (nodeResultados.SelectSingleNode("iconf_inc_cant_abiertas_conyuge").InnerText != string.Empty)
        resultadoI.iconf_inc_cant_abiertas_conyuge = Int32.Parse(nodeResultados.SelectSingleNode("iconf_inc_cant_abiertas_conyuge")?.InnerText ?? "0");
        resultadoI.iconf_inc_resumen_cony = nodeResultados.SelectSingleNode("iconf_inc_resumen_cony")?.InnerText;
        resultadoI.iconf_inhabilitaciones_resumen = nodeResultados.SelectSingleNode("iconf_inhabilitaciones_resumen")?.InnerText;
        resultadoI.iconf_inhibiciones_resumen = nodeResultados.SelectSingleNode("iconf_inhibiciones_resumen")?.InnerText;
        resultadoI.iconf_inhib_cant_abiertas = Int32.Parse(nodeResultados.SelectSingleNode("iconf_inhib_cant_abiertas")?.InnerText ?? "0");
        resultadoI.iconf_inh_resumen_cony = nodeResultados.SelectSingleNode("iconf_inh_resumen_cony")?.InnerText;
        resultadoI.iconf_lug_trab_cony = nodeResultados.SelectSingleNode("iconf_lug_trab_cony")?.InnerText;
        resultadoI.iconf_lug_trab_detalle = nodeResultados.SelectSingleNode("iconf_lug_trab_detalle")?.InnerText;
        resultadoI.iconf_morc30_cant_abiertas = Int32.Parse(nodeResultados.SelectSingleNode("iconf_morc30_cant_abiertas")?.InnerText ?? "0");
        resultadoI.iconf_morc30_dias_atraso = Int32.Parse(nodeResultados.SelectSingleNode("iconf_morc30_dias_atraso")?.InnerText ?? "0");
        resultadoI.iconf_morc30_saldo_abiertas_gs = Decimal.Parse(nodeResultados.SelectSingleNode("iconf_morc30_saldo_abiertas_gs")?.InnerText ?? "0");
        resultadoI.iconf_morc30_saldo_abiertas_usd = Decimal.Parse(nodeResultados.SelectSingleNode("iconf_morc30_saldo_abiertas_usd")?.InnerText ?? "0");
        resultadoI.iconf_morme_cant_abiertas = Int32.Parse(nodeResultados.SelectSingleNode("iconf_morme_cant_abiertas")?.InnerText ?? "0");
        resultadoI.iconf_morme_cant_abier_afi = Int32.Parse(nodeResultados.SelectSingleNode("iconf_morme_cant_abier_afi")?.InnerText ?? "0");
        resultadoI.iconf_morme_cant_abier_noafi = Int32.Parse(nodeResultados.SelectSingleNode("iconf_morme_cant_abier_noafi")?.InnerText ?? "0");
        resultadoI.iconf_morme_dias_atraso = Int32.Parse(nodeResultados.SelectSingleNode("iconf_morme_dias_atraso")?.InnerText ?? "0");
        resultadoI.iconf_morme_dias_atraso_afi = Int32.Parse(nodeResultados.SelectSingleNode("iconf_morme_dias_atraso_afi")?.InnerText ?? "0");
        resultadoI.iconf_morme_dias_atr_noafi = Int32.Parse(nodeResultados.SelectSingleNode("iconf_morme_dias_atr_noafi")?.InnerText ?? "0");
        resultadoI.iconf_morme_saldo_abiertas_gs = Decimal.Parse(nodeResultados.SelectSingleNode("iconf_morme_saldo_abiertas_gs")?.InnerText ?? "0");
        resultadoI.iconf_morme_saldo_abiertas_usd = Decimal.Parse(nodeResultados.SelectSingleNode("iconf_morme_saldo_abiertas_usd")?.InnerText ?? "0");
        resultadoI.iconf_morme_saldo_abier_gs_afi = Int32.Parse(nodeResultados.SelectSingleNode("iconf_morme_saldo_abier_gs_afi")?.InnerText ?? "0");
        resultadoI.iconf_morme_saldo_abier_noafi_gs = Decimal.Parse(nodeResultados.SelectSingleNode("iconf_morme_saldo_abier_noafi_gs")?.InnerText ?? "0");
        resultadoI.iconf_morme_saldo_abier_noafi_usd = Decimal.Parse(nodeResultados.SelectSingleNode("iconf_morme_saldo_abier_noafi_usd")?.InnerText ?? "0");
        resultadoI.iconf_morme_saldo_abier_usd_afi = Decimal.Parse(nodeResultados.SelectSingleNode("iconf_morme_saldo_abier_usd_afi")?.InnerText ?? "0");
        resultadoI.iconf_morosidades_resumen = nodeResultados.SelectSingleNode("iconf_morosidades_resumen")?.InnerText;
        resultadoI.iconf_mor_cant_abiertas = Int32.Parse(nodeResultados.SelectSingleNode("iconf_mor_cant_abiertas")?.InnerText ?? "0");
        resultadoI.iconf_mor_cant_abiertas_spub = Int32.Parse(nodeResultados.SelectSingleNode("iconf_mor_cant_abiertas_spub")?.InnerText ?? "0");
        resultadoI.iconf_mor_cant_abiertas_telco = Int32.Parse(nodeResultados.SelectSingleNode("iconf_mor_cant_abiertas_telco")?.InnerText ?? "0");
        resultadoI.iconf_mor_resumen_cony = nodeResultados.SelectSingleNode("iconf_mor_resumen_cony")?.InnerText;
        resultadoI.iconf_nacionalidad = nodeResultados.SelectSingleNode("iconf_nacionalidad")?.InnerText;
        resultadoI.iconf_nacionalidad_cony = nodeResultados.SelectSingleNode("iconf_nacionalidad_cony")?.InnerText;
        resultadoI.iconf_primer_apellido = nodeResultados.SelectSingleNode("iconf_primer_apellido")?.InnerText;
        resultadoI.iconf_primer_apellido_cony = nodeResultados.SelectSingleNode("iconf_primer_apellido_cony")?.InnerText;
        resultadoI.iconf_primer_nombre = nodeResultados.SelectSingleNode("iconf_primer_nombre")?.InnerText;
        resultadoI.iconf_primer_nombre_cony = nodeResultados.SelectSingleNode("iconf_primer_nombre_cony")?.InnerText;
        resultadoI.iconf_profesion = nodeResultados.SelectSingleNode("iconf_profesion")?.InnerText;
        resultadoI.iconf_profesion_cony = nodeResultados.SelectSingleNode("iconf_profesion_cony")?.InnerText;
        resultadoI.iconf_quiebras_resumen = nodeResultados.SelectSingleNode("iconf_quiebras_resumen")?.InnerText;
        resultadoI.iconf_qui_cant_abiertas = Int32.Parse(nodeResultados.SelectSingleNode("iconf_qui_cant_abiertas")?.InnerText ?? "0");
        resultadoI.iconf_qui_resumen_cony = nodeResultados.SelectSingleNode("iconf_qui_resumen_cony")?.InnerText;
        resultadoI.iconf_remates_resumen = nodeResultados.SelectSingleNode("iconf_remates_resumen")?.InnerText;
        resultadoI.iconf_rem_cant_abiertas = Int32.Parse(nodeResultados.SelectSingleNode("iconf_rem_cant_abiertas")?.InnerText ?? "0");
        resultadoI.iconf_rem_resumen_cony = nodeResultados.SelectSingleNode("iconf_rem_resumen_cony")?.InnerText;
        resultadoI.iconf_segundo_apellido = nodeResultados.SelectSingleNode("iconf_segundo_apellido")?.InnerText;
        resultadoI.iconf_segundo_apellido_cony = nodeResultados.SelectSingleNode("iconf_segundo_apellido_cony")?.InnerText;
        resultadoI.iconf_segundo_nombre = nodeResultados.SelectSingleNode("iconf_segundo_nombre")?.InnerText;
        resultadoI.iconf_segundo_nombre_cony = nodeResultados.SelectSingleNode("iconf_segundo_nombre_cony")?.InnerText;
        resultadoI.iconf_sexo = nodeResultados.SelectSingleNode("iconf_sexo")?.InnerText;
        resultadoI.iconf_sexo_cony = nodeResultados.SelectSingleNode("iconf_sexo_cony")?.InnerText;
        resultadoI.punt_base = Int32.Parse(nodeResultados.SelectSingleNode("punt_base")?.InnerText ?? "0");
        resultadoI.punt_ich_cons_cant_dist_ent = Int32.Parse(nodeResultados.SelectSingleNode("punt_ich_cons_cant_dist_ent")?.InnerText ?? "0");
        resultadoI.punt_ich_meses_ult_cons_ban = Int32.Parse(nodeResultados.SelectSingleNode("punt_ich_meses_ult_cons_ban")?.InnerText ?? "0");
        resultadoI.punt_ich_meses_ult_cons_finan = Int32.Parse(nodeResultados.SelectSingleNode("punt_ich_meses_ult_cons_finan")?.InnerText ?? "0");
        resultadoI.punt_iconf_faja_score = Int32.Parse(nodeResultados.SelectSingleNode("punt_iconf_faja_score")?.InnerText ?? "0");
        
        resultadoI.Edad = DateTime.Today.Year - resultadoI.iconf_fecha_nac.Year;
        if (DateTime.Today < resultadoI.iconf_fecha_nac.AddYears((int)resultadoI.Edad))
        resultadoI.Edad--;
        
        
        //campos calculados
        resultadoI.calc_mor_otros = resultadoI.iconf_mor_cant_abiertas - (resultadoI.iconf_mor_cant_abiertas_spub + resultadoI.iconf_mor_cant_abiertas_telco);
        
        #endregion Cargar datos a tabla de Resultados Iconf
        }*/

        public override void AfterConstruction()
        {
            base.AfterConstruction();
            _fecha = DateTime.Now;
            _usuario = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId).UserName;
            _tipodocumento = Session.FindObject<TipoDocumento>(TipoDocumento.Fields.Codigo == "CI");
            _paisfocumento = Session.FindObject<Pais>(Pais.Fields.Codigo == "PY");

            Usuario currentUser = Session.GetObjectByKey<Usuario>(SecuritySystem.CurrentUserId);
            if(currentUser != null)
            {
                _cliente = currentUser.Cliente;
            }
        }
        //[PersistentAlias("ToStr(1))")]
        //public int Id
        //{
        // get { return (int)EvaluateAlias("Id"); }
        //}

        [NonPersistent]
        public string Accion
        {
            get
            {
                Resultado resultado = Session.FindObject<Resultado>(Resultado.Fields.Consulta == Oid);
                if(resultado != null)
                {
                    return resultado.Accion;
                }

                return string.Empty;
            }
        }

        [Association("Consulta-Adjuntos")]
        [DevExpress.Xpo.Aggregated]
        public XPCollection<Adjunto> Adjuntos => GetCollection<Adjunto>(nameof(Adjuntos));

        [Appearance("NoVerCliente", AppearanceItemType = nameof(ViewItem), TargetItems = nameof(Cliente), Context = "Any", Visibility = ViewItemVisibility.Hide)]
        //[RuleRequiredField(DefaultContexts.Save)]
        public Cliente Cliente
        {
            get => _cliente;
            set => SetPropertyValue(nameof(Cliente), ref _cliente, value);
        }

        [Size(30)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("Número de documento de identidad")]
        public string Documento
        {
            get => _documento;
            set => SetPropertyValue(nameof(Documento), ref _documento, value);
        }

        [ImmediatePostData]
        [Appearance("NoEditarEstado", AppearanceItemType = nameof(ViewItem), TargetItems = nameof(Estado), Context = "Any", Enabled = false)]
        public ConsultaEstados Estado
        {
            get => _estado;
            set
            {
                try
                {
                    ConsultaEstados valueOr = value;
                    ConsultaEstados estadoOr = _estado;

                    if(!IsLoading)
                    {
                        if(Session.IsNewObject(this))
                        {
                            throw new Exception("Debe guardar la solicitud.");
                        }

                        if(estadoOr != valueOr)
                        {
                            if(value == ConsultaEstados.ProcesadoMotor)
                            {
                                if(Resultados[0].Accion.ToUpper() == "ACEPTAR")
                                {
                                    value = ConsultaEstados.Aprobado;
                                }
                                else if(Resultados[0].Accion.ToUpper() == "EVALUAR")
                                {
                                    value = ConsultaEstados.PreAprobado;
                                }
                                else if(Resultados[0].Accion.ToUpper() == "RECHAZAR")
                                {
                                    value = ConsultaEstados.Rechazado;
                                }
                            }
                        }

                        SetPropertyValue(nameof(Estado), ref _estado, value);

                        /*if (ObjectSpace != null)
                        {
                        ObjectSpace.CommitChanges();
                        }*/                    }
                    else
                    {
                        SetPropertyValue(nameof(Estado), ref _estado, value);
                    }
                }
                catch(NullReferenceException)
                {
                    throw new Exception("Debe cargar todos los datos de la consulta");
                }
            }
        }

        [NonPersistent]
        public string Explicacion
        {
            get
            {
                Resultado resultado = Session.FindObject<Resultado>(CriteriaOperator.Parse("Consulta = '" +
                    Oid.ToString() +
                    "'"));
                return resultado != null ? resultado.Explicacion : string.Empty;
            }
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [NonCloneable]
        [ModelDefault("DisplayFormat", "dd/MM/yyyy HH:mm")]
        [Appearance("NoEditarFecha", AppearanceItemType = nameof(ViewItem), TargetItems = nameof(Fecha), Context = "Any", Enabled = false)]
        public DateTime Fecha
        {
            get => _fecha;
            set => SetPropertyValue(nameof(Fecha), ref _fecha, value);
        }

        public new static FieldsClass Fields
        {
            get
            {
                if(ReferenceEquals(_Fields, null))
                {
                    _Fields = new FieldsClass();
                }

                return _Fields;
            }
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("Modelo de regla de negocios a aplicar")]
        // TODO filtrar tambien por tipo de producto
        [DataSourceCriteria("Cliente= GetClienteUser() and Vigente=true")]
        public Modelo Modelo
        {
            get => _modelo;
            set => SetPropertyValue(nameof(Modelo), ref _modelo, value);
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [Appearance(nameof(Monto), Visibility = ViewItemVisibility.Hide, Criteria = "TipoProducto.DescripcionPDN='DESCUENTO'", Context = nameof(DetailView))]
        [ToolTip("Monto solicitado en guaraníes")]
        public int Monto
        {
            get => _monto;
            set => SetPropertyValue(nameof(Monto), ref _monto, value);
        }

        [NonPersistent] //todo mostrar solo cuando hay resultados

        public string NombreCompleto
        {
            get
            {
                Resultado resultado = Session.FindObject<Resultado>(CriteriaOperator.Parse("Consulta = '" +
                    Oid.ToString() +
                    "'"));
                if(resultado != null)
                {
                    return resultado.Nombres + " " + resultado.Apellidos;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        //[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("País emisor del documento de identidad")]
        public Pais PaisDocumento
        {
            get => _paisfocumento;
            set => SetPropertyValue(nameof(PaisDocumento), ref _paisfocumento, value);
        }

        [Association("Consulta-Resultados")]
        [DevExpress.Xpo.Aggregated]
        [NonCloneable]
        [ImmediatePostData]
        //[RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "No se puede eliminar una consulta ya procesada!")]
        public XPCollection<Resultado> Resultados => GetCollection<Resultado>(nameof(Resultados));

        //[RuleRequiredField(DefaultContexts.Save)]
        [ToolTip("Tipo de documento de identidad")]
        public TipoDocumento TipoDocumento
        {
            get => _tipodocumento;
            set => SetPropertyValue(nameof(TipoDocumento), ref _tipodocumento, value);
        }

        [Size(100)]
        //[RuleRequiredField(DefaultContexts.Save)]
        [XafDisplayName(nameof(Usuario))]
        [ToolTip("Usuario que realiza la consulta")]
        [NonCloneable]
        [Appearance("NoEditarUsuario", AppearanceItemType = nameof(ViewItem), TargetItems = nameof(Usuario), Context = "Any", Enabled = false)]
        public string Usuario
        {
            get => _usuario;
            set => SetPropertyValue(nameof(Usuario), ref _usuario, value);
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

            public Cliente.FieldsClass Cliente
            {
                get
                {
                    return new Cliente.FieldsClass(GetNestedName("Cliente"));
                }
            }

            public OperandProperty Documento
            {
                get
                {
                    return new OperandProperty(GetNestedName("Documento"));
                }
            }

            public OperandProperty Estado
            {
                get
                {
                    return new OperandProperty(GetNestedName("Estado"));
                }
            }

            public OperandProperty Fecha
            {
                get
                {
                    return new OperandProperty(GetNestedName("Fecha"));
                }
            }

            public Modelo.FieldsClass Modelo
            {
                get
                {
                    return new Modelo.FieldsClass(GetNestedName("Modelo"));
                }
            }

            public OperandProperty Monto
            {
                get
                {
                    return new OperandProperty(GetNestedName("Monto"));
                }
            }

            public Pais.FieldsClass PaisDocumento
            {
                get
                {
                    return new Pais.FieldsClass(GetNestedName("PaisDocumento"));
                }
            }

            public OperandProperty Resultados
            {
                get
                {
                    return new OperandProperty(GetNestedName("Resultados"));
                }
            }

            public TipoDocumento.FieldsClass TipoDocumento
            {
                get
                {
                    return new TipoDocumento.FieldsClass(GetNestedName("TipoDocumento"));
                }
            }

            public OperandProperty Usuario
            {
                get
                {
                    return new OperandProperty(GetNestedName("Usuario"));
                }
            }
        }
    }

    public class GetClienteUser : ICustomFunctionOperator
    {
        static GetClienteUser()
        {
            GetClienteUser instance = new GetClienteUser();
            if(CriteriaOperator.GetCustomFunction(instance.Name) == null)
            {
                CriteriaOperator.RegisterCustomFunction(instance);
            }
        }

        public object Evaluate(params object[] operands)
        {
            Usuario usr = (Usuario)SecuritySystem.CurrentUser;
            return usr.Cliente.Nombre;
        }
        public static void Register()
        {
        }
        public Type ResultType(params Type[] operands) => typeof(string);

        public string Name => nameof(GetClienteUser);
    }

    public enum ConsultaEstados
    {
        Nuevo, ProcesadoMotor, EnviarSolicitud, PreAprobado, Aprobado, Rechazado
    }
}
