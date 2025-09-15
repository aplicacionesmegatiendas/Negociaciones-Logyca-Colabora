using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Transitions;
namespace NegociacionesLogycaColabora
{
    public partial class FrmAdicionItems : Form
    {
        DataTable dt_plan = null;
        DataTable dt_criterio = null;

        List<GrupoImpositivo> lstGrupo = null;
        List<TipoInventario> lstTipoInv = null;

        List<string> lista_items = null;
        int c = -1;

        int ci = 0;

        bool quitar = false;

        char separador = Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);//Esta variable contiene el caracter que el sistema operativo usa para la separación de cifras decimales.

        string raiz;

        string[] url_imgs = null;

        public FrmAdicionItems()
        {
            InitializeComponent();

            raiz = Conexion.RaizArchivos;
        }

        static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        private void Limpiar()
        {
            txt_nit.Text = "";
            txt_razon_social.Text = "";
            txt_sucursal.Text = "";

            txt_descripcion.Text = "";
            txt_descripcion.Tag = null;
            _toolTip.SetToolTip(txt_descripcion, "");

            txt_descripcion_corta.Text = "";
            txt_descripcion_corta.Tag = null;
            _toolTip.SetToolTip(txt_descripcion_corta, "");

            lbl_cat_logyca.Text = "";

            txt_impuesto.Text = "";

            txt_peso_und_inv.Text = "";

            txt_fact_und_orden.Text = "";


            txt_factor_peso_orden.Text = "";

            txt_fact_und_emp.Text = "";

            /////////////////////////////////COTIZACION////////////////////////////////////
            txt_moneda.Text = "";
            txt_precio.Text = "";
            txt_tmpo_entrega.Text = "";
            dgv_descuento.Rows.Clear();
            txt_fecha_act.Text = "";
            txt_fecha_hasta.Text = "";
            ///////////////////////////////DESCRIPCION_TECNICA/////////////////////////////
            txt_alto_inv.Text = "";
            txt_ancho_inv.Text = "";
            txt_profundo_inv.Text = "";
            txt_alto_emp.Text = "";
            txt_ancho_emp.Text = "";
            txt_profundo_emp.Text = "";

            //////////////////////////PRECIOS DE VENTA////////////////////////////////////
            txt_precio2.Text = "";
            txt_und_medida2.Text = "";

            ///////////////////////CODIGO DE BARRAS/////////////////////////////////////
            txt_cod_barras.Text = "";
            txt_und_medida3.Text = "";


            txt_cant_und_med.Text = "";

            txt_factor_operacion.Text = "";

            chk_todos_dif.Checked = false;
            foreach (DataGridViewRow item in dgv_difusiones.Rows)
            {
                item.Cells["col_sel_dif"].Value = false;
            }
            chk_todos.Checked = false;
        }

        private void ObtenerListasPrecio()
        {
            Datos datos = new Datos();
            clb_lista_precios.DataSource = datos.ObtenerListasPrecio();
            clb_lista_precios.DisplayMember = "f112_descripcion";
            clb_lista_precios.ValueMember = "f112_id";
            clb_lista_precios.ClearSelected();
        }

        public string MonthName(int month)
        {
            DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
            return dtinfo.GetMonthName(month);
        }

        private void SoloNumeros(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsPunctuation(e.KeyChar)))
            {
                e.Handled = true;
            }
            else if (e.KeyChar == ',' || e.KeyChar == '.')//Si lo que se digito fue una coma o un punto el programa lo reemplaza por el caracter que usa el sistema para separar decimales.
            {
                e.KeyChar = Convert.ToChar(separador);
            }
            else if (!(e.KeyChar == '1' || e.KeyChar == '2' || e.KeyChar == '3' || e.KeyChar == '4' || e.KeyChar == '5' || e.KeyChar == '6' || e.KeyChar == '7' || e.KeyChar == '8' || e.KeyChar == '9' || e.KeyChar == '0' || e.KeyChar == (char)8))
            {
                e.Handled = true;
            }
        }

        private void SoloNumeros2(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || char.IsControl(e.KeyChar) || char.IsPunctuation(e.KeyChar)))
            {
                e.Handled = true;
            }

            else if (!(e.KeyChar == '1' || e.KeyChar == '2' || e.KeyChar == '3' || e.KeyChar == '4' || e.KeyChar == '5' || e.KeyChar == '6' || e.KeyChar == '7' || e.KeyChar == '8' || e.KeyChar == '9' || e.KeyChar == '0' || e.KeyChar == (char)8))
            {
                e.Handled = true;
            }
        }

        private void ListarMotivos()
        {
            Datos datos = new Datos();
            cmb_motivo_dev.DisplayMember = "mo_descripcion";
            cmb_motivo_dev.ValueMember = "mo_id";
            cmb_motivo_dev.DataSource = datos.ListarMotivos();
            cmb_motivo_dev.SelectedIndex = -1;
        }

        private void ListarGruposImpositos()
        {
            Datos grupos = new Datos();
            lstGrupo = grupos.ListarGruposImpositivos();
            dgv_grupo_impositivo.AutoGenerateColumns = false;
            dgv_grupo_impositivo.Columns[0].DataPropertyName = "Id";
            dgv_grupo_impositivo.Columns[1].DataPropertyName = "Descripción";
            dgv_grupo_impositivo.DataSource = lstGrupo;
            dgv_grupo_impositivo.ClearSelection();
            dgv_grupo_impositivo.SelectionChanged += Dgv_grupo_impositivo_SelectionChanged;
        }

        private void ListarTiposInventario()
        {
            Datos tipos = new Datos();
            lstTipoInv = tipos.ListarTiposInventario();
            dgv_tipo_inv.AutoGenerateColumns = false;
            dgv_tipo_inv.Columns[0].DataPropertyName = "Id";
            dgv_tipo_inv.Columns[1].DataPropertyName = "Descripción";
            dgv_tipo_inv.DataSource = lstTipoInv;
        }

        private void ObtenerPlanes()
        {
            Datos datos = new Datos();
            dt_plan = datos.ObtenerPlanes();
            dgv_plan.AutoGenerateColumns = false;
            dgv_plan.Columns[0].DataPropertyName = "f105_id";
            dgv_plan.Columns[1].DataPropertyName = "f105_descripcion";
            dgv_plan.DataSource = dt_plan;
        }

        private void ObtenerMayores(string plan)
        {
            Datos datos = new Datos();
            dt_criterio = datos.ObtenerCriteriosMayor(plan);
            dgv_criterio.AutoGenerateColumns = false;
            dgv_criterio.Columns[0].DataPropertyName = "f106_id";
            dgv_criterio.Columns[1].DataPropertyName = "f106_descripcion";
            dgv_criterio.DataSource = dt_criterio;
        }

        private void ListarCentrosOperacion()
        {
            Datos datos = new Datos();
            cmb_instalacion.DataSource = datos.ListarCentrosOperacion();
            cmb_instalacion.DisplayMember = "f285_descripcion";
            cmb_instalacion.ValueMember = "f285_id";

            cmb_instalacion.SelectedIndex = -1;
            txt_bodega_co.Text = "";
        }

        private void ListarCompradores()
        {
            Datos datos = new Datos();
            cmb_comprador.DataSource = datos.ListarCompradoresUnoee();

            cmb_comprador.DisplayMember = "f200_razon_social";
            cmb_comprador.ValueMember = "f211_id";

            cmb_comprador.SelectedIndex = -1;
        }

        private void ListarDifusiones()
        {
            Difusiones difusiones = new Difusiones();
            dgv_difusiones.AutoGenerateColumns = false;
            col_cod_dif.DataPropertyName = "f08_codigo";
            col_dif.DataPropertyName = "f08_descripción";
            col_cod_cluster.DataPropertyName = "f08_cod_cluster";
            dgv_difusiones.DataSource = difusiones.ListarDifusionesES();
        }

        private void ListarPortafoliosDifusion(int difusion)
        {
            Difusiones difusiones = new Difusiones();
            System.Data.DataTable dt_portafolios = difusiones.ListarPortafoliosDifusion(difusion);
            col_co.Visible = true;
            if (dt_portafolios != null)
            {
                foreach (DataRow row in dt_portafolios.Rows)
                {
                    bool existe = false;
                    foreach (DataGridViewRow row2 in dgv_portafolio.Rows)
                    {
                        if (Convert.ToString(row2.Cells["col_portafolio"].Value) == row["f09_cod_portafolio"].ToString())
                        {
                            existe = true;
                            break;
                        }
                    }
                    if (existe == false)
                    {
                        dgv_portafolio.Rows.Add(false, row["f09_cod_portafolio"].ToString(), row["f09_co"].ToString());
                    }
                }
            }
        }

        private void ListarItems()
        {
            Datos datos = new Datos();
            lista_items = datos.ObtenerListadoGeneralAdicion(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador);
        }

        private void ObtenerDatosItem(string referencia)
        {
            Datos datos = new Datos();
            List<List<object>> info_item = datos.ObtenerDatosItems(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, referencia);

            /////////////////////////////////ITEM////////////////////////////////////
            txt_nit.Text = Convert.ToString(info_item[0][4]);
            txt_razon_social.Text = Convert.ToString(info_item[0][5]);
            txt_sucursal.Text = Convert.ToString(info_item[0][6]);

            txt_descripcion.Text = RemoveDiacritics(Convert.ToString(info_item[0][9]));
            txt_descripcion.Tag = RemoveDiacritics(Convert.ToString(info_item[0][9]));
            _toolTip.SetToolTip(txt_descripcion, Convert.ToString(info_item[0][9]));

            txt_descripcion_corta.Text = RemoveDiacritics(Convert.ToString(info_item[0][10]));
            txt_descripcion_corta.Tag = RemoveDiacritics(Convert.ToString(info_item[0][10]));
            _toolTip.SetToolTip(txt_descripcion_corta, Convert.ToString(info_item[0][10]));

            lbl_cat_logyca.Text = Convert.ToString(info_item[0][21]);

            chk_aceptar.Checked = Convert.ToBoolean(info_item[0][22]);

            cmb_motivo_dev.SelectedIndex = -1;
            if (chk_aceptar.Checked == false)
            {
                cmb_motivo_dev.Text = Convert.ToString(info_item[0][23]);
            }

            dgv_grupo_impositivo.FirstDisplayedScrollingRowIndex = 0;
            dgv_grupo_impositivo.ClearSelection();

            foreach (DataGridViewRow row in dgv_grupo_impositivo.Rows)
            {

                if (Convert.ToString(row.Cells[0].Value) == Convert.ToString(info_item[0][11]))
                {
                    row.Selected = true;
                    dgv_grupo_impositivo.FirstDisplayedScrollingRowIndex = row.Index;
                    Datos dato = new Datos();
                    lbl_impuesto.Text = dato.ObtenerTasa(Convert.ToString(info_item[0][11])).ToString();
                    break;
                }
            }

            dgv_tipo_inv.FirstDisplayedScrollingRowIndex = 0;
            dgv_tipo_inv.ClearSelection();
            foreach (DataGridViewRow row in dgv_tipo_inv.Rows)
            {
                if (Convert.ToString(row.Cells[0].Value) == Convert.ToString(info_item[0][12]))
                {
                    row.Selected = true;
                    dgv_tipo_inv.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }

            txt_impuesto.Text = Convert.ToString(info_item[0][24]);

            cmb_und_inv.SelectedIndex = -1;
            if (!info_item[0][13].Equals(""))
            {
                cmb_und_inv.SelectedValue = info_item[0][13];
            }
            txt_peso_und_inv.Text = Convert.ToString(info_item[0][14]);

            cmb_und_orden.SelectedIndex = -1;
            if (!info_item[0][15].Equals(""))
            {
                cmb_und_orden.SelectedValue = info_item[0][15];
            }
            if (!Convert.ToString(info_item[0][16]).Equals(""))
            {
                txt_fact_und_orden.Text = Convert.ToString(info_item[0][16]);
                txt_tamaño_lote.Text = txt_fact_und_orden.Text; //Convert.ToString(info_item[0][16]);
            }
            else
            {
                txt_fact_und_orden.Text = "0";
                txt_tamaño_lote.Text = "0";
            }

            if (!info_item[0][14].Equals("") && !info_item[0][16].Equals(""))
            {
                txt_factor_peso_orden.Text = (Convert.ToDecimal(info_item[0][14]) * Convert.ToDecimal(info_item[0][16])).ToString("0.#");
            }

            cmb_und_emp.SelectedIndex = -1;
            if (!info_item[0][18].Equals(""))
            {
                cmb_und_emp.SelectedValue = info_item[0][18];
            }
            else
            {
                cmb_und_emp.SelectedValue = "";
            }
            if (!Convert.ToString(info_item[0][19]).Equals(""))
            {
                txt_fact_und_emp.Text = Convert.ToString(info_item[0][19]);
            }
            else
            {
                txt_fact_und_emp.Text = "0";
            }
            if (!info_item[0][14].ToString().Trim().Equals("") && !info_item[0][19].ToString().Trim().Equals(""))
            {
                txt_factor_peso_emp.Text = (Convert.ToDecimal(info_item[0][14]) * Convert.ToDecimal(info_item[0][19])).ToString("0.#");
            }

            /////////////////////////////////COTIZACION////////////////////////////////////
            txt_moneda.Text = Convert.ToString(info_item[1][8]);
            txt_fecha_act.Text = Convert.ToDateTime(info_item[1][17], CultureInfo.InvariantCulture).ToString("yyyyMMdd");
            txt_precio.Text = Convert.ToString(info_item[1][9]);
            txt_tmpo_entrega.Text = Convert.ToString(info_item[1][11]);

            if (!Convert.ToString(info_item[1][13]).Equals(""))
            {
                dgv_descuento.Rows.Add(info_item[1][12], info_item[1][13], Convert.ToDateTime(info_item[1][14], CultureInfo.InvariantCulture).ToString("yyyyMMdd"), Convert.ToDateTime(info_item[1][15], CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
            }
            if (!Convert.ToString(info_item[1][20]).Equals(""))
            {
                dgv_descuento.Rows.Add(info_item[1][19], info_item[1][20], Convert.ToDateTime(info_item[1][21], CultureInfo.InvariantCulture).ToString("yyyyMMdd"), Convert.ToDateTime(info_item[1][22], CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
            }
            if (!Convert.ToString(info_item[1][24]).Equals(""))
            {
                dgv_descuento.Rows.Add(info_item[1][23], info_item[1][24], Convert.ToDateTime(info_item[1][25], CultureInfo.InvariantCulture).ToString("yyyyMMdd"), Convert.ToDateTime(info_item[1][26], CultureInfo.InvariantCulture).ToString("yyyyMMdd"));
            }

            chk_ind_compra.Checked = Convert.ToBoolean(info_item[0][25]);
            chk_ind_vta.Checked = Convert.ToBoolean(info_item[0][26]);
            chk_ind_manuf.Checked = Convert.ToBoolean(info_item[0][27]);

            ///////////////////////////////DESCRIPCION_TECNICA/////////////////////////////
            txt_alto_inv.Text = Convert.ToString(info_item[2][6]);
            txt_ancho_inv.Text = Convert.ToString(info_item[2][7]);
            txt_profundo_inv.Text = Convert.ToString(info_item[2][8]);

            txt_alto_emp.Text = Convert.ToString(info_item[2][9]);
            txt_ancho_emp.Text = Convert.ToString(info_item[2][10]);
            txt_profundo_emp.Text = Convert.ToString(info_item[2][11]);

            //////////////////////////////CRITERIOS DE CLASIFICACION//////////////////////
            dgv_criterios_clasif.Rows.Clear();
            foreach (List<object> item in info_item[3])
            {
                dgv_criterios_clasif.Rows.Add(item[6], item[7], item[8], item[9]);
            }

            ////////////////////////////PARAMETROS DE PLANEACION/////////////////////////
            dgv_parametros_plan.Rows.Clear();
            foreach (List<object> item in info_item[4])
            {
                bool orden_plan = Convert.ToBoolean(item[25]);
                dgv_parametros_plan.Rows.Add(item[6], item[7], item[8], item[9], item[10], item[11], item[12], item[13], item[14], item[15], item[16], item[17], item[18], item[19], item[20], item[21], item[22], item[23], item[24], orden_plan);
            }

            //////////////////////////PRECIOS DE VENTA////////////////////////////////////
            txt_precio2.Text = Convert.ToString(info_item[1][9]);
            txt_und_medida2.Text = Convert.ToString(info_item[0][13]);

            dgv_precios.Rows.Clear();
            foreach (List<object> item in info_item[5])
            {
                dgv_precios.Rows.Add(item[8], item[9], Convert.ToDateTime(item[10], CultureInfo.InvariantCulture).ToString("yyyyMMdd"), item[11], item[6], item[12]);
            }

            ////////////////////////PORTAFOLIO///////////////////////////////////////////
            dgv_portafolios.Rows.Clear();
            foreach (List<object> item in info_item[6])
            {
                dgv_portafolios.Rows.Add(item[6], item[7], item[8]);
            }

            ///////////////////////CODIGO DE BARRAS/////////////////////////////////////
            txt_cod_barras.Text = lbl_referencia.Text;
            txt_und_medida3.Text = txt_und_medida2.Text;
            if (Convert.ToString(info_item[7][7]).Trim().Equals(""))
            {
                txt_cant_und_med.Text = "1";
            }
            else
            {
                txt_cant_und_med.Text = Convert.ToString(info_item[7][7]).Trim();
            }

            int r1 = 0;
            if (int.TryParse(Convert.ToString(info_item[7][8]).Trim(), out r1))
            {
                cmb_tipo_codigo.SelectedIndex = r1 - 1;
            }
            else
            {
                cmb_tipo_codigo.SelectedIndex = -1;
            }

            int r2 = 0;
            if (int.TryParse(Convert.ToString(info_item[7][9]).Trim(), out r2))
            {
                cmb_ind_operacion.SelectedIndex = r2 - 1;
            }
            else
            {
                cmb_ind_operacion.SelectedIndex = -1;
            }

            if (Convert.ToString(info_item[7][10]).Trim().Equals(""))
            {
                txt_factor_operacion.Text = Convert.ToString(info_item[0][16]);
            }
            else
            {
                txt_factor_operacion.Text = Convert.ToString(info_item[7][10]).Trim();
            }

            chk_ppal.Checked = Convert.ToBoolean(info_item[7][12]);

            //OTROS DATOS//

            lbl_nomb_com.Text = Convert.ToString(info_item[8][8]).Trim();
            url_imgs = Convert.ToString(info_item[8][9]).Split(',');
            ci = 0;
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string url_img = url_imgs[0];

            if (!url_img.Trim().Equals(""))
            {
                lbl_nro_imgs.Text = $"{url_imgs.Length.ToString()} imagenes";
                lbl_nro.Text = "1";

                pbx_imagen.LoadAsync(url_img.Trim());
                pbx_imagen.Tag = url_img.Trim();
                pbx_imagen.Cursor = Cursors.Hand;
            }
            else
            {
                lbl_nro_imgs.Text = "0 imagenes";
                lbl_nro.Text = "0";
                pbx_imagen.Image = null;
                pbx_imagen.Tag = null;
                pbx_imagen.Cursor = Cursors.Default;
            }

            if (url_imgs.Length > 1)
                btn_img.Enabled = true;
            else
                btn_img.Enabled = false;

            lbl_marca.Text = Convert.ToString(info_item[8][10]).Trim();
            lbl_fabricante.Text = Convert.ToString(info_item[8][11]).Trim();
            lbl_reg_sanitario.Text = Convert.ToString(info_item[8][12]).Trim();
            lbl_tipo_prod.Text = Convert.ToString(info_item[8][13]).Trim();
            lbl_linea.Text = Convert.ToString(info_item[8][14]).Trim();
            lbl_fragancia.Text = Convert.ToString(info_item[8][15]).Trim();
            lbl_sabor.Text = Convert.ToString(info_item[8][16]).Trim();
            lbl_recomendaciones.Text = Convert.ToString(info_item[8][17]).Trim();
            lbl_advertencia.Text = Convert.ToString(info_item[8][18]).Trim();
            lbl_precauciones.Text = Convert.ToString(info_item[8][19]).Trim();
            string ficha_tecnica = Convert.ToString(info_item[8][20]).Trim();
            if (!ficha_tecnica.Equals(""))
            {
                lbl_ficha_tecnica.ForeColor = Color.Blue;
                lbl_ficha_tecnica.Cursor = Cursors.Hand;
                lbl_ficha_tecnica.Tag = ficha_tecnica;
            }
            else
            {
                lbl_ficha_tecnica.ForeColor = Color.Black;
                lbl_ficha_tecnica.Cursor = Cursors.Default;
                lbl_ficha_tecnica.Tag = null;
            }
            lbl_precauciones.Text = Convert.ToString(info_item[8][19]).Trim();

            lbl_tipo_emp.Text = Convert.ToString(info_item[8][21]).Trim();
            lbl_multiplos_desp.Text = Convert.ToString(info_item[8][22]).Trim();
            lbl_proveedor.Text = Convert.ToString(info_item[8][23]).Trim();
            lbl_cant_cont.Text = Convert.ToString(info_item[8][24]).Trim();
            lbl_unds_embalaje.Text = Convert.ToString(info_item[8][25]).Trim();
            lbl_sublinea.Text = Convert.ToString(info_item[8][26]).Trim();
        }

        private void ObtenerDatosItemEspejo(string referencia)
        {
            Datos datos = new Datos();
            List<List<object>> info_item = datos.ObtenerDatosItems(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, referencia);

            /////////////////////////////////ITEM////////////////////////////////////

            txt_desc_espejo.Text = RemoveDiacritics(Convert.ToString(info_item[0][9]));
            txt_desc_corta_espejo.Text = RemoveDiacritics(Convert.ToString(info_item[0][10]));

            dgv_grupo_impositivo.FirstDisplayedScrollingRowIndex = 0;
            dgv_grupo_impositivo.ClearSelection();

            foreach (DataGridViewRow row in dgv_grupo_impositivo.Rows)
            {
                if (Convert.ToString(row.Cells[0].Value) == Convert.ToString(info_item[0][11]))
                {
                    row.Selected = true;
                    dgv_grupo_impositivo.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }

            dgv_tipo_inv.FirstDisplayedScrollingRowIndex = 0;
            dgv_tipo_inv.ClearSelection();
            foreach (DataGridViewRow row in dgv_tipo_inv.Rows)
            {
                if (Convert.ToString(row.Cells[0].Value) == Convert.ToString(info_item[0][12]))
                {
                    row.Selected = true;
                    dgv_tipo_inv.FirstDisplayedScrollingRowIndex = row.Index;
                    break;
                }
            }

            if (Convert.ToInt32(info_item[0][25]) == 1)
            {
                chk_ind_compra.Checked = true;
            }
            else
            {
                chk_ind_compra.Checked = false;
            }

            if (Convert.ToInt32(info_item[0][26]) == 1)
            {
                chk_ind_vta.Checked = true;
            }
            else
            {
                chk_ind_vta.Checked = false;
            }

            if (Convert.ToInt32(info_item[0][27]) == 1)
            {
                chk_ind_manuf.Checked = true;
            }
            else
            {
                chk_ind_manuf.Checked = false;
            }

            //////////////////////////////CRITERIOS DE CLASIFICACION//////////////////////
            dgv_criterios_clasif.Rows.Clear();
            foreach (List<object> item in info_item[3])
            {
                dgv_criterios_clasif.Rows.Add(item[6], item[7], item[8], item[9]);
            }

            ////////////////////////////PARAMETROS DE PLANEACION/////////////////////////
            dgv_parametros_plan.Rows.Clear();
            foreach (List<object> item in info_item[4])
            {
                dgv_parametros_plan.Rows.Add(item[6], item[7], item[8], item[9], item[10], item[11], item[12], item[13], item[14], item[15], item[16], item[17], item[18], item[19], item[20], item[21], item[22], item[23], item[24], item[25]);
            }

            //////////////////////////PRECIOS DE VENTA////////////////////////////////////
            txt_precio2.Text = txt_precio.Text;
            txt_und_medida2.Text = Convert.ToString(info_item[0][13]);

            dgv_precios.Rows.Clear();
            foreach (List<object> item in info_item[5])
            {
                dgv_precios.Rows.Add(item[8], item[9], item[10], item[11], item[6]);
            }

            ////////////////////////PORTAFOLIO///////////////////////////////////////////
            dgv_portafolios.Rows.Clear();
            foreach (List<object> item in info_item[6])
            {
                dgv_portafolios.Rows.Add(item[6], item[7], item[8]);
            }

            ///////////////////////CODIGO DE BARRAS/////////////////////////////////////
            txt_cod_barras.Text = lbl_referencia.Text;
            txt_und_medida3.Text = txt_und_medida2.Text;
            if (Convert.ToString(info_item[7][7]).Trim().Equals(""))
            {
                txt_cant_und_med.Text = "1";
            }
            else
            {
                txt_cant_und_med.Text = Convert.ToString(info_item[7][7]).Trim();
            }

            int r1 = 0;
            if (int.TryParse(Convert.ToString(info_item[7][8]).Trim(), out r1))
            {
                cmb_tipo_codigo.SelectedIndex = r1 - 1;
            }
            else
            {
                cmb_tipo_codigo.SelectedIndex = -1;
            }

            int r2 = 0;
            if (int.TryParse(Convert.ToString(info_item[7][9]).Trim(), out r2))
            {
                cmb_ind_operacion.SelectedIndex = r2 - 1;
            }
            else
            {
                cmb_ind_operacion.SelectedIndex = -1;
            }

            if (Convert.ToString(info_item[7][10]).Trim().Equals(""))
            {
                txt_factor_operacion.Text = "1";
            }
            else
            {
                txt_factor_operacion.Text = Convert.ToString(info_item[7][10]).Trim();
            }
            chk_ppal.Checked = Convert.ToBoolean(info_item[7][12]);
        }

        private void ListarUnidadesMedida()
        {
            Datos unidades = new Datos();

            cmb_und_inv.DisplayMember = "Descripción";
            cmb_und_inv.ValueMember = "Id";
            cmb_und_inv.DataSource = unidades.ListarUnidadesMedida();
            cmb_und_inv.SelectedIndex = -1;

            cmb_und_orden.DisplayMember = "Descripción";
            cmb_und_orden.ValueMember = "Id";
            cmb_und_orden.DataSource = unidades.ListarUnidadesMedida();
            cmb_und_orden.SelectedIndex = -1;

            cmb_und_emp.DisplayMember = "Descripción";
            cmb_und_emp.ValueMember = "Id";
            List<UnidadMedida> unds = unidades.ListarUnidadesMedida();
            UnidadMedida item = new UnidadMedida();
            item.Id = "";
            item.Descripción = "";
            unds.Insert(0, item);
            cmb_und_emp.DataSource = unds;

            cmb_und_emp.SelectedIndex = -1;
        }

        private void FrmAdicionItems_Load(object sender, EventArgs e)
        {
            try
            {
                ListarItems();
                if (lista_items != null)
                {
                    ListarMotivos();

                    ListarGruposImpositos();

                    ListarTiposInventario();
                    ListarUnidadesMedida();

                    ObtenerPlanes();

                    ListarCentrosOperacion();

                    ObtenerListasPrecio();

                    ListarCompradores();

                    ListarDifusiones();

                    btn_adelante.PerformClick();
                }
                else
                {
                    MessageBox.Show("No hay items para procesar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //PARA UBICAR EL ARCHIVO CUANDO NO HAY ITEMS PARA PROCESAR.
                    Datos actualizar = new Datos();
                    Conectores conector = new Conectores();
                    actualizar.ActualizarDocumento();//QUE SE HACE DESPUES
                    Conectores.MoverArchivoProcesado(raiz + Datos.GlnComprador + "\\ADICION\\" + Datos.NombreDocumento);
                    conector.RegistrarActividad(Datos.Usuario, Datos.GlnComprador, Datos.GlnProveedor, "ADICION", Datos.NumeroDocumento, Datos.NombreDocumento);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Dgv_grupo_impositivo_SelectionChanged(object sender, EventArgs e)
        {
            try
            {
                Datos dato = new Datos();
                lbl_impuesto.Text = dato.ObtenerTasa(dgv_grupo_impositivo[0, dgv_grupo_impositivo.CurrentRow.Index].Value.ToString().Trim()).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_adelante_Click(object sender, EventArgs e)
        {
            c++;
            if (c > lista_items.Count - 1)
            {
                c = 0;
            }
            lbl_referencia.Text = lista_items[c];
            lbl_conteo.Text = (c + 1).ToString() + " de " + lista_items.Count.ToString();
            try
            {
                txt_desc_espejo.Text = "";
                txt_desc_corta_espejo.Text = "";
                txt_pvp.Text = "";
                txt_margen.Text = "";
                Limpiar();
                ObtenerDatosItem(lbl_referencia.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_atras_Click(object sender, EventArgs e)
        {
            if (c <= 0)
            {
                c = lista_items.Count;
            }
            c--;
            lbl_referencia.Text = lista_items[c];
            lbl_conteo.Text = (c + 1).ToString() + " de " + lista_items.Count.ToString();
            try
            {
                txt_desc_espejo.Text = "";
                txt_desc_corta_espejo.Text = "";
                txt_pvp.Text = "";
                txt_margen.Text = "";
                Limpiar();
                ObtenerDatosItem(lbl_referencia.Text.Trim());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_buscar_grupo_imp_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdb_id_grp_imp.Checked)
                {
                    dgv_grupo_impositivo.DataSource = lstGrupo.Where(item => item.Id.ToUpper().Contains(txt_buscar_grupo_imp.Text.Trim().ToUpper())).ToList();
                }
                if (rdb_desc_grp_imp.Checked)
                {
                    dgv_grupo_impositivo.DataSource = lstGrupo.Where(item => item.Descripción.ToUpper().Contains(txt_buscar_grupo_imp.Text.Trim().ToUpper())).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_buscar_tipo_inv_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (rdb_id_tipo_inv.Checked)
                {
                    dgv_tipo_inv.DataSource = lstTipoInv.Where(item => item.Id.ToUpper().Contains(txt_buscar_tipo_inv.Text.Trim().ToUpper())).ToList();
                }
                if (rdb_desc_tipo_inv.Checked)
                {
                    dgv_tipo_inv.DataSource = lstTipoInv.Where(item => item.Descripción.ToUpper().Contains(txt_buscar_tipo_inv.Text.Trim().ToUpper())).ToList();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_cerrar_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_crear_conectores_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;

            string errores = "";
            try
            {
                Conectores conector = new Conectores();

                Configuration config = ConfigurationManager.OpenExeConfiguration(Application.StartupPath + "\\" + Application.ProductName + ".exe");
                AppSettingsSection section = config.AppSettings;

                if (conector.ComprobarItemAdicion() >= 1)
                {
                    MessageBox.Show("La información de todos los items no esta completa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Cursor = Cursors.Default;
                    return;
                }

                string año = DateTime.Now.Year.ToString();
                string mes = DateTime.Now.Month.ToString("00");
                string dia = DateTime.Now.Day.ToString("00");
                string hora = DateTime.Now.Hour.ToString("00");
                string minuto = DateTime.Now.Minute.ToString("00");
                string segundo = DateTime.Now.Second.ToString("00");
                string fechahora = año + mes + dia + hora + minuto + segundo;

                Conectores.PrepararResumen();
                try
                {
                    conector.CrearConectorItemAdicion(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, /*Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CONECTORES\\ADICION\\1_AD_ITEM_" + txt_razon_social.Text + "_" + fechahora + ".TXT",*/ lista_items);
                }
                catch (Exception ex)
                {
                    errores += $"ConectorItemAdicion: {ex.Message}{Environment.NewLine}";
                }

                try
                {
                    conector.CrearConectorItemDescripcionTecnica(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, /*Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CONECTORES\\ADICION\\2_AD_DESC_TEC_" + txt_razon_social.Text + "_" + fechahora + ".TXT",*/ lista_items);
                }
                catch (Exception ex)
                {
                    errores += $"ConectorItemDescripcionTecnica: {ex.Message}{Environment.NewLine}";
                }
                try
                {
                    conector.CrearConectorItemCriterio(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\3_AD_CRITERIO_" + txt_razon_social.Text + "_" + fechahora + ".TXT",*/ lista_items);
                }
                catch (Exception ex)
                {
                    errores += $"ConectorItemCriterio: {ex.Message}{Environment.NewLine}";
                }
                try
                {
                    conector.CrearConectorItemParametrosPlaneacion(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, /*Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CONECTORES\\ADICION\\4_AD_PARAMS_PLAN_" + txt_razon_social.Text + "_" + fechahora + ".TXT",*/ lista_items);
                }
                catch (Exception ex)
                {
                    errores += $"ConectorItemParametrosPlaneacion: {ex.Message}{Environment.NewLine}";
                }
                try
                {
                    conector.CrearConectorItemCodigoBarras(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\5_AD_CODIGO_BARRAS_" + txt_razon_social.Text + "_" + fechahora + ".TXT",*/ lista_items);
                }
                catch (Exception ex)
                {
                    errores += $"ConectorItemCodigoBarras: {ex.Message}{Environment.NewLine}";
                }
                try
                {
                    conector.CrearConectorItemCotizacion(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, /*Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CONECTORES\\ADICION\\6_AD_COTIZACION_" + txt_razon_social.Text + "_" + fechahora + ".TXT",*/ txt_nit.Text, txt_sucursal.Text, lista_items);
                }
                catch (Exception ex)
                {
                    errores += $"ConectorItemCotizacion: {ex.Message}{Environment.NewLine}";
                }
                try
                {
                    conector.CrearConectorItemPreciosVenta(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, /*Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\CONECTORES\\ADICION\\7_AD_PRECIO_VTA_" + txt_razon_social.Text + "_" + fechahora + ".TXT",*/ lista_items);
                }
                catch (Exception ex)
                {
                    errores += $"ConectorItemPreciosVenta: {ex.Message}{Environment.NewLine}";
                }
                try
                {
                    List<string> lista_portafolios = conector.ObtenerListadoPortafolios(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador);
                    if (lista_portafolios != null)
                    {
                        conector.CrearConectorItemPortafolios(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\",*/ lista_portafolios, txt_razon_social.Text);
                    }
                }
                catch (Exception ex)
                {
                    errores += $"ConectorItemPortafolios: {ex.Message}{Environment.NewLine}";
                }

                if (Conectores.dt_resumen.Rows.Count > 0)
                    new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
                if (errores != "")
                    MessageBox.Show(errores, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (Conectores.dt_resumen.Rows.Count.Equals(0) && errores.Equals(string.Empty))
                    MessageBox.Show("Conectores creados correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                Conectores.LiberarResumen();

                Datos actualizar = new Datos();
                actualizar.ActualizarDocumento();
                Conectores.MoverArchivoProcesado(raiz + Datos.GlnComprador + "\\ADICION\\" + Datos.NombreDocumento);

                conector.RegistrarActividad(Datos.Usuario, Datos.GlnComprador, Datos.GlnProveedor, "ADICION", Datos.NumeroDocumento, Datos.NombreDocumento);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (errores != "")
            {
                MessageBox.Show(errores, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }

        private void dgv_plan_SelectionChanged(object sender, EventArgs e)
        {
            if (dgv_plan.Rows.Count > 0)
            {
                ObtenerMayores(Convert.ToString(dgv_plan[0, dgv_plan.CurrentRow.Index].Value));
            }
        }

        private void btn_quitar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_criterios_clasif.RowCount > 0)
                {
                    if (dgv_criterios_clasif.SelectedRows.Count > 0)
                    {
                        dgv_criterios_clasif.Rows.RemoveAt(dgv_criterios_clasif.CurrentRow.Index);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_agregar_Click(object sender, EventArgs e)
        {
            if (dgv_plan.SelectedRows.Count > 0 && dgv_criterio.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dgv_criterios_clasif.RowCount; i++)
                {
                    if (dgv_criterios_clasif[0, i].Value.ToString().Trim() == dgv_plan[0, dgv_plan.CurrentRow.Index].Value.ToString().Trim() && dgv_criterios_clasif[2, i].Value.ToString().Trim() == dgv_criterio[0, dgv_criterio.CurrentRow.Index].Value.ToString().Trim())
                    {
                        dgv_criterios_clasif.Rows[i].Selected = true;
                        dgv_criterios_clasif.FirstDisplayedScrollingRowIndex = i;
                        Transition.run(lbl_tit_crit_clasif, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                        return;
                    }
                }
                dgv_criterios_clasif.Rows.Add(dgv_plan[0, dgv_plan.CurrentRow.Index].Value, dgv_plan[1, dgv_plan.CurrentRow.Index].Value.ToString().Split('-')[1], dgv_criterio[0, dgv_criterio.CurrentRow.Index].Value, dgv_criterio[1, dgv_criterio.CurrentRow.Index].Value.ToString().Split('-')[1]);
            }
        }

        private void btn_guardar_Click(object sender, EventArgs e)
        {
            if (chk_aceptar.Checked)
            {
                if (txt_nit.Text.Trim().Equals("") || txt_razon_social.Text.Trim().Equals("") || txt_sucursal.Text.Trim().Equals(""))
                {
                    MessageBox.Show("La información del proveedor no esta completa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Transition.run(lbl_tit_nit, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    Transition.run(lbl_tit_razon, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    Transition.run(lbl_tit_suc, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }
                if (lbl_referencia.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Falta la referencia del item", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Transition.run(lbl_tit_ref, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }
                if (txt_descripcion.Text.Equals(""))
                {
                    MessageBox.Show("Escriba la descripción larga del producto", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Transition.run(lbl_tit_desc_larga, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    txt_descripcion.Focus();
                    return;
                }

                if (txt_descripcion_corta.Text.Equals(""))
                {
                    MessageBox.Show("Escriba la descripción corta del producto", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Transition.run(lbl_tit_desc_corta, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    txt_descripcion_corta.Focus();
                    return;
                }

                if (dgv_grupo_impositivo.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione el grupo impositivo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 0;
                    Transition.run(lbl_tit_grp_imp, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    dgv_grupo_impositivo.Focus();
                    return;
                }
                else
                {
                    if (!Convert.ToDecimal(txt_impuesto.Text.Trim()).Equals(Convert.ToDecimal(lbl_impuesto.Text)))
                    {
                        MessageBox.Show("El porcentaje del grupo impositivo seleccionado " +
                                        "no es igual al porcentaje del item", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        _tabControl.SelectedIndex = 0;
                        Transition.run(lbl_tit_impuesto, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                        Transition.run(lbl_tit_grp_imp, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                        Transition.run(lbl_impuesto, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                        dgv_grupo_impositivo.Focus();
                        return;
                    }
                }
                if (dgv_tipo_inv.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Seleccione el tipo de inventario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 0;
                    Transition.run(lbl_tit_tp_inv, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    dgv_tipo_inv.Focus();
                    return;
                }

                if (cmb_und_inv.SelectedIndex == -1)
                {
                    MessageBox.Show("Seleccione la unidad de inventario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 0;
                    Transition.run(lbl_tit_und_inv, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    cmb_und_inv.Focus();
                    return;
                }

                if (Convert.ToDecimal(txt_peso_und_inv.Text.Trim()) == 0)
                {
                    MessageBox.Show("El peso de la unidad de inventario debe ser mayor a cero", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 0;
                    Transition.run(lbl_tit_peso_inv, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    txt_peso_und_inv.Focus();
                    return;
                }

                if (cmb_und_orden.SelectedIndex == -1)
                {
                    MessageBox.Show("Seleccione la unidad de orden", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 0;
                    Transition.run(lbl_tit_und_orden, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    cmb_und_orden.Focus();
                    return;
                }

                if ((cmb_und_emp.Text.Trim().Equals(cmb_und_inv.Text.Trim())))
                {
                    MessageBox.Show("La unidad de empaque debe ser diferente a la unidad de inventario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 0;
                    return;
                }

                if (txt_precio.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Falta el precio del item", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 0;
                    Transition.run(lbl_tit_precio, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }

                if (txt_fecha_act.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Falta la fecha de activación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 0;
                    Transition.run(lbl_tit_fecha_act, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }

                if (chk_ind_compra.Checked == false && chk_ind_manuf.Checked == false && chk_ind_vta.Checked == false)
                {
                    MessageBox.Show("Debe tener por lo menos un indicador activo para compra, venta y manufactura ", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 0;
                    Transition.run(chk_ind_compra, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    Transition.run(chk_ind_vta, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    Transition.run(chk_ind_manuf, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }

                if (Convert.ToDecimal(txt_fact_und_orden.Text) > 1)
                {
                    if ((txt_alto_emp.Text == "" || Convert.ToDecimal(txt_alto_emp.Text) == 0) ||
                        (txt_ancho_emp.Text == "" || Convert.ToDecimal(txt_ancho_emp.Text) == 0) ||
                        (txt_profundo_emp.Text == "" || Convert.ToDecimal(txt_ancho_emp.Text) == 0))
                    {
                        if (MessageBox.Show("La información de la descripción técnica de la unidad de empaque no es valida o no esta completa.\r\n¿Desea continuar así?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
                        {
                            return;
                        }
                    }
                }

                if (txt_tmpo_entrega.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Falta el tiempo de entrega", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 0;
                    Transition.run(lbl_tit_tmpo_ent, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }

                if (dgv_criterios_clasif.RowCount == 0)
                {
                    MessageBox.Show("Faltan los criterios de clasificación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 1;
                    Transition.run(lbl_tit_crit_clasif, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }

                if (dgv_parametros_plan.RowCount == 0)
                {
                    MessageBox.Show("Faltan los parametros de planeación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 2;
                    Transition.run(lbl_tit_params_plan, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }

                if (dgv_precios.RowCount == 0)
                {
                    MessageBox.Show("Faltan los precios de venta", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 3;
                    Transition.run(lbl_tit_precio_vta, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }
                else
                {
                    for (int i = 0; i < dgv_precios.RowCount; i++)
                    {
                        if (Convert.ToString(dgv_precios[1, i].Value).Equals("") || Convert.ToString(dgv_precios[2, i].Value).Equals(""))
                        {
                            MessageBox.Show("La información de Precios de venta no esta completa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            _tabControl.SelectedIndex = 3;
                            Transition.run(lbl_tit_precio_vta, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                            return;
                        }
                    }
                }

                if (txt_cant_und_med.Text.Trim().Equals("") || cmb_tipo_codigo.SelectedIndex == -1 || cmb_ind_operacion.SelectedIndex == -1 || txt_factor_operacion.Text.Trim().Equals(""))
                {
                    MessageBox.Show("Falta la información de código de barras", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 4;
                    Transition.run(lbl_tit_cod_barra, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }

                if (dgv_portafolios.RowCount == 0)
                {
                    MessageBox.Show("Faltan los portafolios", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    _tabControl.SelectedIndex = 4;
                    Transition.run(lbl_tit_portafolios, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }
            }
            else
            {
                if (cmb_motivo_dev.SelectedIndex == -1)
                {
                    MessageBox.Show("Seleccione el motivo de no aceptación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    cmb_motivo_dev.Focus();
                    return;
                }
            }
            this.Cursor = Cursors.WaitCursor;
            Datos guardar = new Datos();
            try
            {
                object[] valores_item = new object[18];
                valores_item[0] = lbl_referencia.Text;
                valores_item[1] = txt_descripcion.Text.Trim();
                valores_item[2] = txt_descripcion_corta.Text.Trim();
                if (dgv_grupo_impositivo.SelectedRows.Count > 0)
                {
                    valores_item[3] = dgv_grupo_impositivo[0, dgv_grupo_impositivo.SelectedRows[0].Index].Value;
                }
                else
                {
                    valores_item[3] = "";
                }
                if (dgv_tipo_inv.SelectedRows.Count > 0)
                {
                    valores_item[4] = dgv_tipo_inv[0, dgv_tipo_inv.SelectedRows[0].Index].Value;
                }
                else
                {
                    valores_item[4] = "";
                }

                valores_item[5] = cmb_und_inv.SelectedValue;
                valores_item[6] = txt_peso_und_inv.Text;
                valores_item[7] = cmb_und_orden.SelectedValue;
                valores_item[8] = txt_fact_und_orden.Text;
                valores_item[9] = txt_factor_peso_orden.Text;
                if (cmb_und_emp.SelectedIndex == 0)
                {
                    valores_item[10] = "";
                    valores_item[11] = txt_fact_und_emp.Text;
                    valores_item[12] = txt_factor_peso_emp.Text;
                }
                else
                {
                    valores_item[10] = cmb_und_emp.SelectedValue;
                    valores_item[11] = txt_fact_und_emp.Text;
                    valores_item[12] = txt_factor_peso_emp.Text;
                }

                valores_item[13] = chk_aceptar.CheckState;
                valores_item[14] = cmb_motivo_dev.Text;
                valores_item[15] = chk_ind_compra.CheckState;
                valores_item[16] = chk_ind_vta.CheckState;
                valores_item[17] = chk_ind_manuf.CheckState;
                int nro = guardar.ActualizarItem(valores_item);
                if (nro > 0)
                {
                    object[] valores_cotizacion = new object[5];
                    valores_cotizacion[0] = lbl_referencia.Text;
                    valores_cotizacion[1] = cmb_und_inv.SelectedValue;
                    valores_cotizacion[2] = txt_fecha_act.Text.Trim();
                    valores_cotizacion[3] = "";
                    valores_cotizacion[4] = chk_aceptar.CheckState;
                    guardar.ActualizarItemCotizacion(valores_cotizacion);

                    guardar.ActualizarItemDescripcionTecnica(lbl_referencia.Text, chk_aceptar.Checked);

                    guardar.ActualizarItemCriteriosClasificacion(lbl_referencia.Text, dgv_criterios_clasif, chk_aceptar.Checked);

                    guardar.ActualizarItemParametrosPlaneacion(lbl_referencia.Text, dgv_parametros_plan, chk_aceptar.Checked);

                    guardar.ActualizarItemPrecioVenta(lbl_referencia.Text, dgv_precios, chk_aceptar.Checked, txt_und_medida2.Text);

                    guardar.ActualizarItemPortafolio(lbl_referencia.Text, dgv_portafolios, chk_aceptar.Checked);

                    object[] valores_barra = new object[7];
                    valores_barra[0] = txt_und_medida3.Text.Trim();
                    valores_barra[1] = txt_cant_und_med.Text.Trim();
                    valores_barra[2] = cmb_tipo_codigo.SelectedIndex + 1;
                    valores_barra[3] = cmb_ind_operacion.SelectedIndex + 1;
                    valores_barra[4] = txt_factor_operacion.Text.Trim();
                    valores_barra[5] = chk_aceptar.CheckState;
                    valores_barra[6] = chk_ppal.CheckState;
                    guardar.ActualizarItemCodigoBarras(lbl_referencia.Text, valores_barra);
                }
                else
                {
                    MessageBox.Show("No se pudo actualizar la información del ítem", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ObtenerDatosItem(lbl_referencia.Text.Trim());
                MessageBox.Show("Información guardada correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            this.Cursor = Cursors.Default;
        }


        private void btn_buscar_Click(object sender, EventArgs e)
        {
            try
            {
                string mensaje = "";

                FrmItemEspejo _itemespejo = new FrmItemEspejo();
                _itemespejo.ShowDialog(this);
                if (ItemEspejo.Seleccion == true)
                {
                    btn_usar_anterior.Enabled = true;

                    txt_desc_espejo.Text = RemoveDiacritics(ItemEspejo.DescripcionLarga.Trim());
                    txt_desc_corta_espejo.Text = RemoveDiacritics(ItemEspejo.DescripcionCorta.Trim());

                    if (ItemEspejo.IndCompra == 1)
                    {
                        chk_ind_compra.Checked = true;
                    }
                    else
                    {
                        chk_ind_compra.Checked = false;
                    }

                    if (ItemEspejo.IndVenta == 1)
                    {
                        chk_ind_vta.Checked = true;
                    }
                    else
                    {
                        chk_ind_vta.Checked = false;
                    }

                    if (ItemEspejo.IndManufactura == 1)
                    {
                        chk_ind_manuf.Checked = true;
                    }
                    else
                    {
                        chk_ind_manuf.Checked = false;
                    }

                    dgv_grupo_impositivo.FirstDisplayedScrollingRowIndex = 0;
                    dgv_grupo_impositivo.ClearSelection();
                    if (ItemEspejo.GrupoImpositivo != "")
                    {
                        foreach (DataGridViewRow row in dgv_grupo_impositivo.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value) == ItemEspejo.GrupoImpositivo)
                            {
                                dgv_grupo_impositivo.Rows[row.Index].Selected = true;
                                dgv_grupo_impositivo.FirstDisplayedScrollingRowIndex = row.Index;
                                break;
                            }
                        }
                        mensaje += "Grupo impositivo, ";
                    }

                    dgv_tipo_inv.FirstDisplayedScrollingRowIndex = 0;
                    dgv_tipo_inv.ClearSelection();
                    if (ItemEspejo.TipoInventario != "")
                    {
                        foreach (DataGridViewRow row in dgv_tipo_inv.Rows)
                        {
                            if (Convert.ToString(row.Cells[0].Value) == ItemEspejo.TipoInventario)
                            {
                                dgv_tipo_inv.Rows[row.Index].Selected = true;
                                dgv_tipo_inv.FirstDisplayedScrollingRowIndex = row.Index;
                                break;
                            }
                        }
                        mensaje += "Tipo de inventario, ";
                    }

                    if (ItemEspejo.CriteriosClasificacion != null)
                    {
                        dgv_criterios_clasif.Rows.Clear();
                        foreach (DataRow item in ItemEspejo.CriteriosClasificacion.Rows)
                        {
                            dgv_criterios_clasif.Rows.Add(item[0], item[1], item[2], item[3]);
                        }
                        mensaje += "Criterios de clasificación, ";
                    }
                    if (ItemEspejo.ParametrosPlaneacion != null)
                    {
                        dgv_parametros_plan.Rows.Clear();
                        foreach (DataRow item in ItemEspejo.ParametrosPlaneacion.Rows)
                        {
                            dgv_parametros_plan.Rows.Add(item[0], item[1], item[2], item[3], cmb_und_inv.SelectedValue, item[5], item[6], item[7], item[8], item[9], item[10], item[11], item[12], item[13], item[14], item[15], txt_nit.Text.Trim(), txt_razon_social.Text.Trim(), txt_sucursal.Text.Trim(), item[16]);
                        }

                        if (dgv_parametros_plan.RowCount > 0)
                        {
                            cmb_instalacion.SelectedValue = dgv_parametros_plan[0, 0].Value;
                            cmb_rotacion_veces.Text = Convert.ToString(dgv_parametros_plan[2, 0].Value);
                            cmb_rotacion_costo.Text = Convert.ToString(dgv_parametros_plan[3, 0].Value);
                            nud_periodo_cub.Value = Convert.ToDecimal(dgv_parametros_plan[5, 0].Value);
                            nud_tmpo_rep.Value = Convert.ToDecimal(dgv_parametros_plan[6, 0].Value);
                            nud_tmpo_seg.Value = Convert.ToDecimal(dgv_parametros_plan[7, 0].Value);
                            cmb_comprador.SelectedValue = dgv_parametros_plan[8, 0].Value;
                            nud_horiz_plan.Value = Convert.ToDecimal(dgv_parametros_plan[9, 0].Value);
                            txt_stock_seg_estatico.Text = Convert.ToString(dgv_parametros_plan[10, 0].Value);
                            nud_dias_horiz_stock_min.Value = Convert.ToDecimal(dgv_parametros_plan[11, 0].Value);
                            nud_dias_stock_min.Value = Convert.ToDecimal(dgv_parametros_plan[12, 0].Value);
                            nud_tmpo_rep_fijo.Value = Convert.ToDecimal(dgv_parametros_plan[13, 0].Value);
                            cmb_politicas_orden.SelectedIndex = Convert.ToInt32(dgv_parametros_plan[14, 0].Value) - 1;
                        }
                        mensaje += "Parámetros de planeación, ";
                    }                  

                    if (ItemEspejo.PreciosVenta != null)
                    {
                        dgv_precios.Rows.Clear();
                        foreach (DataRow item in ItemEspejo.PreciosVenta.Rows)
                        {
                            dgv_precios.Rows.Add(item[0]);
                        }
                        mensaje += "Precios de venta, ";
                    }

                    if (ItemEspejo.Portafolio != null)
                    {
                        dgv_portafolios.Rows.Clear();
                        foreach (DataRow item in ItemEspejo.Portafolio.Rows)
                        {
                            dgv_portafolios.Rows.Add(item[0], item[1], item[2]);
                        }
                        mensaje += "Portafolios, ";
                    }

                    if (ItemEspejo.Barra != null)
                    {
                        txt_cant_und_med.Text = ItemEspejo.Barra[0];
                        cmb_tipo_codigo.SelectedIndex = Convert.ToInt32(ItemEspejo.Barra[1]) - 1;
                        cmb_ind_operacion.SelectedIndex = Convert.ToInt32(ItemEspejo.Barra[2]) - 1;
                        txt_factor_operacion.Text = ItemEspejo.Barra[3];
                        mensaje += "Barra ";
                    }

                    MessageBox.Show("Los datos de Indicador de compra, Indicador de venta, indicador de manufactura, " +
                                mensaje.Trim().Trim(',') + " provienen de la información de otro item. " +
                                "Verifique estos datos y valide la información antes de guardar.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                ItemEspejo.GrupoImpositivo = "";
                ItemEspejo.TipoInventario = "";
                ItemEspejo.CriteriosClasificacion = null;

                ItemEspejo.ParametrosPlaneacion = null;
                ItemEspejo.PreciosVenta = null;
                ItemEspejo.Portafolio = null;
                ItemEspejo.Barra = null;

                ItemEspejo.Seleccion = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_mostrar_ocultar_Click(object sender, EventArgs e)
        {
            if (btn_mostrar_ocultar.Text.Equals("<<"))
            {
                Transition.run(panelParamsPlan, "Left", -333, new TransitionType_EaseInEaseOut(300));
                Transition.run(panelGrid, "Left", 8, new TransitionType_EaseInEaseOut(500));
                Transition.run(panelGrid, "Width", 1064, new TransitionType_EaseInEaseOut(500));
                Transition.run(btn_mostrar_ocultar, "Text", ">>", new TransitionType_Linear(700));
                return;
            }
            if (btn_mostrar_ocultar.Text.Equals(">>"))
            {
                Transition.run(panelGrid, "Left", 348, new TransitionType_EaseInEaseOut(300));
                Transition.run(panelGrid, "Width", 725, new TransitionType_EaseInEaseOut(300));
                Transition.run(panelParamsPlan, "Left", 9, new TransitionType_EaseInEaseOut(500));
                Transition.run(btn_mostrar_ocultar, "Text", "<<", new TransitionType_Linear(800));
            }
        }

        private void cmb_instalacion_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_bodega_co.Text = Convert.ToString(cmb_instalacion.SelectedValue) + "01";
        }

        private void txt_stock_seg_estatico_Leave(object sender, EventArgs e)
        {
            if (txt_stock_seg_estatico.Text.Equals(""))
            {
                txt_stock_seg_estatico.Text = "0";
            }
        }

        private void txt_tamaño_lote_Leave(object sender, EventArgs e)
        {
            if (txt_tamaño_lote.Text.Equals(""))
            {
                txt_tamaño_lote.Text = "0";
            }
        }

        private void btn_agregar_param_plan_Click(object sender, EventArgs e)
        {
            if (cmb_instalacion.SelectedIndex == -1 || cmb_rotacion_veces.SelectedIndex == -1 || cmb_rotacion_costo.SelectedIndex == -1 || cmb_comprador.SelectedIndex == -1 || cmb_politicas_orden.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un valor para todos los campos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            if (cmb_und_inv.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione la unidad de inventario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _tabControl.SelectedIndex = 0;
                Transition.run(lbl_tit_und_inv, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                cmb_und_inv.Focus();
                return;
            }

            if (nud_periodo_cub.Value == 0 || nud_tmpo_rep.Value == 0 || nud_tmpo_seg.Value == 0)
            {
                if (MessageBox.Show("Periodo de cubrimiento, Tiempo de reposición y Tiempo de seguridad deben ser valores mayores a cero.\n ¿Desea continuar?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
                {
                    return;
                }
            }

            dgv_parametros_plan.ClearSelection();
            int i = 0;
            for (i = 0; i < dgv_parametros_plan.RowCount; i++)
            {
                if (Convert.ToString(dgv_parametros_plan[0, i].Value).Equals(Convert.ToString(cmb_instalacion.SelectedValue)))
                {
                    dgv_parametros_plan.Rows[i].Selected = true;
                    dgv_parametros_plan.FirstDisplayedScrollingRowIndex = i;
                    Transition.run(lbl_tit_params_plan, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                    return;
                }
            }

            dgv_parametros_plan.Rows.Add(cmb_instalacion.SelectedValue, txt_bodega_co.Text, cmb_rotacion_veces.Text, cmb_rotacion_costo.Text,
                cmb_und_inv.SelectedValue, nud_periodo_cub.Value, nud_tmpo_rep.Value, nud_tmpo_seg.Value, cmb_comprador.SelectedValue, nud_horiz_plan.Value,
                txt_stock_seg_estatico.Text.Trim(), nud_dias_horiz_stock_min.Value, nud_dias_stock_min.Value, nud_tmpo_rep_fijo.Value, cmb_politicas_orden.SelectedIndex + 1,
                txt_tamaño_lote.Text.Trim(), txt_nit.Text, txt_razon_social.Text, txt_sucursal.Text);
        }

        private void btn_quitar_param_plan_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgv_parametros_plan.RowCount > 0)
                {
                    if (dgv_parametros_plan.SelectedRows.Count > 0)
                    {
                        int i = dgv_parametros_plan.CurrentRow.Index;
                        quitar = true;
                        dgv_parametros_plan.Rows.RemoveAt(i);

                        quitar = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_agregar_precio_Click(object sender, EventArgs e)
        {
            if (txt_und_medida2.Text.Trim().Equals(""))
            {
                MessageBox.Show("Seleccione la unidad de inventario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _tabControl.SelectedIndex = 0;
                Transition.run(lbl_tit_und_inv, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                cmb_und_inv.Focus();
                return;
            }
            if (txt_pvp.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba el precio de venta", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_margen.Focus();
                return;
            }

            if (clb_lista_precios.CheckedIndices.Count == 0)
            {
                MessageBox.Show("Seleccione la lista de precio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                clb_lista_precios.Focus();
                return;
            }

            if (chk_fecha_inact.Checked == true)
            {
                if (dtp_fecha_act.Value.Date > dtp_fecha_inact.Value.Date)
                {
                    MessageBox.Show("La Fecha inactivación no puede ser menor a la Fecha activación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            for (int i = 0; i < dgv_precios.Rows.Count; i++)
            {
                foreach (DataRowView item in clb_lista_precios.CheckedItems)
                {
                    if (dgv_precios[0, i].Value.Equals(item[0].ToString().Split('-')[0]))
                    {
                        MessageBox.Show($"La listas de precios {item[0].ToString().Split('-')[0]} ya esta agregada", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        dgv_precios.Rows[i].Selected = true;
                        dgv_precios.FirstDisplayedScrollingRowIndex = i;
                        Transition.run(lbl_tit_precio_vta, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                        return;
                    }
                }
            }

            if (Convert.ToDecimal(txt_precio2.Text.Trim().Replace('.', separador).Replace(',', separador)) > Convert.ToDecimal(txt_pvp.Text.Trim().Replace('.', separador).Replace(',', separador)))
            {
                if (MessageBox.Show("El precio de venta es menor al precio de compra, desea continuar...", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
                {
                    txt_margen.Focus();
                    txt_margen.SelectAll();
                    return;
                }
            }

            foreach (DataRowView item in clb_lista_precios.CheckedItems)
            {
                string fecha_inact = dtp_fecha_inact.Value.Date.ToString("yyyyMMdd");
                if (!chk_fecha_inact.Checked)
                {
                    fecha_inact = "";
                }
                dgv_precios.Rows.Add(item[0].ToString().Split('-')[0], txt_pvp.Text.Trim(), dtp_fecha_act.Value.Date.ToString("yyyyMMdd"), fecha_inact, txt_und_medida2.Text.Trim(), Convert.ToDecimal(txt_margen.Text.Trim()));
            }
        }

        private void btn_quitar_precio_Click(object sender, EventArgs e)
        {
            if (dgv_precios.RowCount > 0)
            {
                dgv_precios.Rows.RemoveAt(dgv_precios.CurrentRow.Index);
            }
        }

        private void cmb_und_inv_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_und_medida2.Text = Convert.ToString(cmb_und_inv.SelectedValue);
            txt_und_medida3.Text = Convert.ToString(cmb_und_inv.SelectedValue);
            for (int i = 0; i < dgv_parametros_plan.RowCount; i++)
            {
                dgv_parametros_plan[4, i].Value = cmb_und_inv.SelectedValue;
            }
        }

        private void chk_fecha_inact_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_fecha_inact.Checked)
            {
                dtp_fecha_inact.Enabled = true;
            }
            else
            {
                dtp_fecha_inact.Enabled = false;
            }
        }

        private void btn_completar_Click(object sender, EventArgs e)
        {
            if (txt_pvp.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba el precio de venta", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_margen.Focus();
                return;
            }

            if (Convert.ToDecimal(txt_pvp.Text.Trim()) <= Convert.ToDecimal(txt_precio2.Text.Trim().Replace('.', separador).Replace(',', separador)))
            {
                if (MessageBox.Show("El precio de venta no debe ser menor ni igual al precio de compra, desea continuar...", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
                {
                    txt_margen.Focus();
                    txt_margen.SelectAll();
                    return;
                }
            }
            if (txt_und_medida2.Text.Trim().Equals(""))
            {
                MessageBox.Show("Seleccione la unidad de inventario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _tabControl.SelectedIndex = 0;
                Transition.run(lbl_tit_und_inv, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                cmb_und_inv.Focus();
                return;
            }
            if (dgv_precios.RowCount > 0)
            {
                if (chk_fecha_inact.Checked == true)
                {
                    if (dtp_fecha_act.Value.Date > dtp_fecha_inact.Value.Date)
                    {
                        MessageBox.Show("La Fecha inactivación no puede ser menor a la Fecha activación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                }
                for (int i = 0; i < dgv_precios.RowCount; i++)
                {
                    dgv_precios[1, i].Value = txt_pvp.Text.Trim();
                    dgv_precios[2, i].Value = dtp_fecha_act.Value.Date.ToString("yyyyMMdd");
                    dgv_precios[4, i].Value = txt_und_medida2.Text.Trim();
                    decimal margen = 0;
                    _=decimal.TryParse(txt_margen.Text.Trim(),out margen);
                    dgv_precios[5, i].Value = margen;
                    if (chk_fecha_inact.Checked)
                    {
                        dgv_precios[3, i].Value = dtp_fecha_inact.Value.Date.ToString("yyyyMMdd");
                    }
                    else
                    {
                        dgv_precios[3, i].Value = "";
                    }
                }
            }
        }

        private void btn_quitar_Click_1(object sender, EventArgs e)
        {
            if (dgv_portafolios.RowCount > 0 && dgv_portafolios.SelectedRows.Count > 0)
            {
                dgv_portafolios.Rows.RemoveAt(dgv_portafolios.CurrentRow.Index);
            }
        }

        private void txt_factor_operacion_Leave(object sender, EventArgs e)
        {
            if (txt_factor_operacion.Text.Trim().Equals(""))
            {
                txt_factor_operacion.Text = "1";
            }
        }

        private void txt_cant_und_med_Leave(object sender, EventArgs e)
        {
            if (txt_cant_und_med.Text.Trim().Equals(""))
            {
                txt_cant_und_med.Text = "1";
            }
        }

        private void chk_aceptar_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_aceptar.Checked == true)
            {
                cmb_motivo_dev.SelectedIndex = -1;
            }
        }

        private void btn_act_Click(object sender, EventArgs e)
        {
            try
            {
                Datos datos = new Datos();
                dgv_criterios_clasif.AutoGenerateColumns = false;
                dgv_criterios_clasif.Columns[0].DataPropertyName = "ca_plan";
                dgv_criterios_clasif.Columns[1].DataPropertyName = "ca_desc_plan";
                dgv_criterios_clasif.Columns[2].DataPropertyName = "ca_criterio";
                dgv_criterios_clasif.Columns[3].DataPropertyName = "ca_desc_mayor";
                dgv_criterios_clasif.Rows.Clear();
                DataTable dt = datos.ObtenerEquivalenciasCategoriaLogyca(lbl_cat_logyca.Text);
                foreach (DataRow item in dt.Rows)
                {
                    dgv_criterios_clasif.Rows.Add(item[0], item[1], item[2], item[3]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmb_und_emp_SelectedValueChanged(object sender, EventArgs e)
        {
            if (cmb_und_emp.SelectedIndex == 0)
            {
                txt_fact_und_emp.Text = "0";
                txt_factor_peso_emp.Text = "0";
            }
            else
            {
                txt_fact_und_emp.Text = txt_fact_und_orden.Text;
                txt_factor_peso_emp.Text = txt_factor_peso_orden.Text;
            }
        }

        private void btn_todos_Click(object sender, EventArgs e)
        {
            try
            {
                dgv_portafolios.Rows.Clear();

                Datos datos = new Datos();
                for (int i = 0; i < dgv_portafolio.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgv_portafolio["col_sel", i].Value) == true)
                    {
                        string nota = datos.ObtenerNotaPortafolio(dgv_portafolio["col_portafolio", i].Value.ToString()).Trim();

                        dgv_portafolios.Rows.Add(dgv_portafolio["col_portafolio", i].Value.ToString(), dgv_portafolio["col_co", i].Value.ToString(), nota);
                        Application.DoEvents();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_descripcion_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Home))
            {
                txt_descripcion.Text = Convert.ToString(txt_descripcion.Tag);
            }
        }

        private void txt_descripcion_corta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode.Equals(Keys.Home))
            {
                txt_descripcion_corta.Text = Convert.ToString(txt_descripcion_corta.Tag);
            }
        }

        private void btn_agregar_todos_Click(object sender, EventArgs e)
        {
            if (cmb_rotacion_veces.SelectedIndex == -1 || cmb_rotacion_costo.SelectedIndex == -1 || cmb_comprador.SelectedIndex == -1 || cmb_politicas_orden.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione un valor para todos los campos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            if (cmb_und_inv.SelectedIndex == -1)
            {
                MessageBox.Show("Seleccione la unidad de inventario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _tabControl.SelectedIndex = 0;
                Transition.run(lbl_tit_und_inv, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                cmb_und_inv.Focus();
                return;
            }

            if (nud_periodo_cub.Value == 0 || nud_tmpo_rep.Value == 0 || nud_tmpo_seg.Value == 0)
            {
                if (MessageBox.Show("Periodo de cubrimiento, Tiempo de reposición y Tiempo de seguridad deben ser valores mayores a cero.\n ¿Desea continuar?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
                {
                    return;
                }
            }

            for (int i = 0; i < cmb_instalacion.Items.Count; i++)
            {
                cmb_instalacion.SelectedIndex = i;

                bool existe = false;

                for (int j = 0; j < dgv_parametros_plan.RowCount; j++)
                {
                    if (Convert.ToString(dgv_parametros_plan[0, j].Value).Equals(Convert.ToString(cmb_instalacion.SelectedValue)))
                    {
                        existe = true;
                        dgv_parametros_plan[0, j].Value = cmb_instalacion.SelectedValue;
                        dgv_parametros_plan[1, j].Value = txt_bodega_co.Text;
                        dgv_parametros_plan[2, j].Value = cmb_rotacion_veces.Text;
                        dgv_parametros_plan[3, j].Value = cmb_rotacion_costo.Text;
                        dgv_parametros_plan[4, j].Value = cmb_und_inv.SelectedValue;
                        dgv_parametros_plan[8, j].Value = cmb_comprador.SelectedValue;
                        dgv_parametros_plan[9, j].Value = nud_horiz_plan.Value;
                        dgv_parametros_plan[10, j].Value = txt_stock_seg_estatico.Text.Trim();
                        dgv_parametros_plan[11, j].Value = nud_dias_horiz_stock_min.Value;
                        dgv_parametros_plan[12, j].Value = nud_dias_stock_min.Value;
                        dgv_parametros_plan[13, j].Value = nud_tmpo_rep_fijo.Value;
                        dgv_parametros_plan[14, j].Value = cmb_politicas_orden.SelectedIndex + 1;
                        dgv_parametros_plan[15, j].Value = txt_tamaño_lote.Text.Trim();
                        dgv_parametros_plan[16, j].Value = txt_nit.Text;
                        dgv_parametros_plan[17, j].Value = txt_razon_social.Text;
                        dgv_parametros_plan[18, j].Value = txt_sucursal.Text;

                        break;
                    }
                }
                if (existe == false)
                {
                    dgv_parametros_plan.Rows.Add(cmb_instalacion.SelectedValue, txt_bodega_co.Text, cmb_rotacion_veces.Text, cmb_rotacion_costo.Text,
                                cmb_und_inv.SelectedValue, nud_periodo_cub.Value, nud_tmpo_rep.Value, nud_tmpo_seg.Value, cmb_comprador.SelectedValue, nud_horiz_plan.Value,
                                txt_stock_seg_estatico.Text.Trim(), nud_dias_horiz_stock_min.Value, nud_dias_stock_min.Value, nud_tmpo_rep_fijo.Value, cmb_politicas_orden.SelectedIndex + 1,
                                txt_tamaño_lote.Text.Trim(), txt_nit.Text, txt_razon_social.Text, txt_sucursal.Text, true);
                }

                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }
        }

        private void txt_margen_TextChanged(object sender, EventArgs e)
        {
            decimal r = 0;
            if (decimal.TryParse(txt_margen.Text, out r))
            {
                if (r > 0)
                {
                    try
                    {
                        decimal nuevo_costo = Convert.ToDecimal(txt_precio2.Text.Trim().Replace('.', separador).Replace(',', separador));
                        decimal iva = Convert.ToDecimal(txt_impuesto.Text.Trim().Replace('.', separador).Replace(',', separador));

                        decimal dscto = 0;
                        decimal pvp_con_dscto = nuevo_costo;
                        if (dgv_descuento.Rows.Count > 0)
                        {
                            for (int i = 0; i < dgv_descuento.Rows.Count; i++)
                            {
                                dscto = Convert.ToDecimal(dgv_descuento[0, i].Value.ToString().Trim().Replace('.', separador).Replace(',', separador));
                                pvp_con_dscto = pvp_con_dscto - ((pvp_con_dscto * dscto) / 100);
                            }
                        }

                        decimal porc_margen = Convert.ToDecimal(txt_margen.Text);

                        decimal pvp_con_iva = pvp_con_dscto + ((pvp_con_dscto * iva) / 100);

                        decimal pvp = pvp_con_iva / porc_margen;

                        int longitud = pvp.ToString("0").Length;
                        decimal valor = 0;
                        decimal valor1 = 0;
                        decimal redondeado = 0;

                        switch (longitud)
                        {
                            case 1:
                                redondeado = 50;
                                break;
                            case 2:
                                valor = Convert.ToDecimal(pvp.ToString("0"));
                                if (valor >= 10 && valor <= 50)
                                {
                                    redondeado = 50;
                                }
                                if (valor >= 51 && valor <= 90)
                                {
                                    redondeado = 90;
                                }
                                if (valor >= 91 && valor <= 99)
                                {
                                    redondeado = 100;
                                }
                                break;
                            default:
                                valor = Convert.ToDecimal(pvp.ToString("0"));
                                valor1 = Convert.ToDecimal(pvp.ToString("0").Substring(longitud - 2, 2));
                                if (valor1 == 0)
                                {
                                    redondeado = valor;
                                }
                                if (valor1 >= 1 && valor1 <= 50)
                                {
                                    decimal falta = 50 - valor1;
                                    redondeado = valor + falta;
                                }
                                if (valor1 >= 51 && valor1 <= 90)
                                {
                                    decimal falta = 90 - valor1;
                                    redondeado = valor + falta;
                                }
                                if (valor1 >= 91 && valor1 <= 99)
                                {
                                    decimal falta = 100 - valor1;
                                    redondeado = valor + falta;
                                }
                                break;
                        }

                        txt_pvp.Text = redondeado.ToString("0");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                txt_pvp.Text = "";
            }
        }

        private void txt_filtro_mayor_TextChanged(object sender, EventArgs e)
        {
            dt_criterio.DefaultView.RowFilter = "f106_descripcion Like'%" + txt_filtro_mayor.Text + "%'";
        }

        private void chk_todas_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_todas.CheckState.Equals(CheckState.Checked))
            {
                for (int i = 0; i < clb_lista_precios.Items.Count; i++)
                {
                    clb_lista_precios.SetItemChecked(i, true);
                }
            }
            else
            {
                for (int i = 0; i < clb_lista_precios.Items.Count; i++)
                {
                    clb_lista_precios.SetItemChecked(i, false);
                }
            }
        }

        private void btn_agregar_todos_precio_vta_Click(object sender, EventArgs e)
        {
            if (txt_und_medida2.Text.Trim().Equals(""))
            {
                MessageBox.Show("Seleccione la unidad de inventario", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                _tabControl.SelectedIndex = 0;
                Transition.run(lbl_tit_und_inv, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
                cmb_und_inv.Focus();
                return;
            }
            if (txt_pvp.Text.Trim().Equals(""))
            {
                MessageBox.Show("Escriba el precio de venta", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txt_pvp.Focus();
                return;
            }

            if (Convert.ToDecimal(txt_precio2.Text.Trim().Replace('.', separador).Replace(',', separador)) > Convert.ToDecimal(txt_pvp.Text.Trim().Replace('.', separador).Replace(',', separador)))
            {
                if (MessageBox.Show("El precio de venta es menor al precio de compra, desea continuar...", "Aviso", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
                {
                    txt_pvp.Focus();
                    txt_pvp.SelectAll();
                    return;
                }
            }

            if (chk_fecha_inact.Checked == true)
            {
                if (dtp_fecha_act.Value.Date > dtp_fecha_inact.Value.Date)
                {
                    MessageBox.Show("La Fecha inactivación no puede ser menor a la Fecha activación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
            }

            dgv_precios.Rows.Clear();
            chk_todas.Checked = true;
            foreach (DataRowView item in clb_lista_precios.CheckedItems)
            {
                string fecha_inact = dtp_fecha_inact.Value.Date.ToString("yyyyMMdd");
                if (!chk_fecha_inact.Checked)
                {
                    fecha_inact = "";
                }
                dgv_precios.Rows.Add(item[1].ToString().Split('-')[0], txt_pvp.Text.Trim(), dtp_fecha_act.Value.Date.ToString("yyyyMMdd"), fecha_inact, txt_und_medida2.Text.Trim(), Convert.ToDecimal(txt_margen.Text.Trim()));
                Application.DoEvents();
                System.Threading.Thread.Sleep(100);
            }
        }

        private void actualizarListasPrecioToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                ObtenerListasPrecio();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btn_usar_anterior_Click(object sender, EventArgs e)
        {
            if (!txt_item_ant.Text.Equals(""))
            {
                try
                {
                    ObtenerDatosItemEspejo(txt_item_ant.Text.Trim());
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            txt_item_ant.Text = "";
        }

        private void btn_rechazar_todos_Click(object sender, EventArgs e)
        {
            if (chk_aceptar.CheckState.Equals(CheckState.Unchecked) && cmb_motivo_dev.SelectedIndex > -1)
            {
                string motivo = cmb_motivo_dev.Text;
                for (int i = 0; i < lista_items.Count; i++)
                {
                    Application.DoEvents();
                    btn_adelante.PerformClick();
                    ///////////////////////////
                    this.Cursor = Cursors.WaitCursor;
                    Datos guardar = new Datos();
                    try
                    {
                        object[] valores_item = new object[15];
                        valores_item[0] = lbl_referencia.Text;
                        valores_item[1] = txt_descripcion.Text;
                        valores_item[2] = txt_descripcion_corta.Text;
                        if (dgv_grupo_impositivo.SelectedRows.Count > 0)
                        {
                            valores_item[3] = dgv_grupo_impositivo[0, dgv_grupo_impositivo.SelectedRows[0].Index].Value;
                        }
                        else
                        {
                            valores_item[3] = "";
                        }
                        if (dgv_tipo_inv.SelectedRows.Count > 0)
                        {
                            valores_item[4] = dgv_tipo_inv[0, dgv_tipo_inv.SelectedRows[0].Index].Value;
                        }
                        else
                        {
                            valores_item[4] = "";
                        }
                        valores_item[5] = cmb_und_inv.SelectedValue;
                        valores_item[6] = txt_peso_und_inv.Text;
                        valores_item[7] = cmb_und_orden.SelectedValue;
                        valores_item[8] = txt_fact_und_orden.Text;
                        valores_item[9] = txt_factor_peso_orden.Text;

                        if (cmb_und_emp.SelectedIndex == 0)
                        {
                            valores_item[10] = "";
                            valores_item[11] = "0";
                            valores_item[12] = "0";
                        }
                        else
                        {
                            valores_item[10] = cmb_und_emp.SelectedValue;
                            valores_item[11] = txt_fact_und_emp.Text;
                            valores_item[12] = txt_factor_peso_emp.Text;
                        }

                        valores_item[13] = false;
                        valores_item[14] = motivo;
                        guardar.ActualizarItem(valores_item);

                        object[] valores_cotizacion = new object[5];
                        valores_cotizacion[0] = lbl_referencia.Text;
                        valores_cotizacion[1] = "";
                        valores_cotizacion[2] = txt_fecha_act.Text.Trim();
                        valores_cotizacion[3] = "";
                        valores_cotizacion[4] = false;
                        guardar.ActualizarItemCotizacion(valores_cotizacion);

                        guardar.ActualizarItemDescripcionTecnica(lbl_referencia.Text, false);

                        guardar.ActualizarItemCriteriosClasificacion(lbl_referencia.Text, dgv_criterios_clasif, false);

                        guardar.ActualizarItemParametrosPlaneacion(lbl_referencia.Text, dgv_parametros_plan, false);

                        guardar.ActualizarItemPrecioVenta(lbl_referencia.Text, dgv_precios, false, txt_und_medida2.Text);

                        guardar.ActualizarItemPortafolio(lbl_referencia.Text, dgv_portafolios, false);

                        object[] valores_barra = new object[7];
                        valores_barra[0] = txt_und_medida3.Text.Trim();
                        valores_barra[1] = txt_cant_und_med.Text.Trim();
                        valores_barra[2] = cmb_tipo_codigo.SelectedIndex + 1;
                        valores_barra[3] = cmb_ind_operacion.SelectedIndex + 1;
                        valores_barra[4] = txt_factor_operacion.Text.Trim();
                        valores_barra[5] = false;
                        valores_barra[6] = chk_ppal.CheckState;
                        guardar.ActualizarItemCodigoBarras(lbl_referencia.Text, valores_barra);

                        ObtenerDatosItem(lbl_referencia.Text.Trim());
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    this.Cursor = Cursors.Default;
                    //////////////////////////
                    System.Threading.Thread.Sleep(200);
                }
                MessageBox.Show("Información guardada correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txt_descripcion_Leave(object sender, EventArgs e)
        {
            txt_descripcion.Text = RemoveDiacritics(txt_descripcion.Text.Trim());
        }

        private void txt_descripcion_corta_Leave(object sender, EventArgs e)
        {
            txt_descripcion_corta.Text = RemoveDiacritics(txt_descripcion_corta.Text.Trim());
        }

        private void lbl_ficha_tecnica_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (lbl_ficha_tecnica.ForeColor == Color.Blue)
            {
                try
                {
                    string ruta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\FICHA_TECNICA";
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }
                    using (var client = new WebClient())
                    {
                        client.DownloadFile(Convert.ToString(Convert.ToString(lbl_ficha_tecnica.Tag)), ruta + "\\ ficha.pdf");
                    }
                    ProcessStartInfo startInfo = new ProcessStartInfo(ruta + "\\ ficha.pdf");
                    startInfo.WindowStyle = ProcessWindowStyle.Maximized;
                    Process.Start(startInfo);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Cursor = Cursors.Default;
        }

        private void pbx_imagen_Click(object sender, EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if (pbx_imagen.Image != null)
            {
                try
                {
                    string ruta = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\IMAGENES";
                    if (!Directory.Exists(ruta))
                    {
                        Directory.CreateDirectory(ruta);
                    }
                    pbx_imagen.Image.Save(ruta + "\\imagen.jpg");
                    new FrmDimensionesImagen(ruta, "imagen.jpg", lbl_referencia.Text).ShowDialog();

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Cursor = Cursors.Default;
        }

        private void dgv_descuento_SelectionChanged(object sender, EventArgs e)
        {
            txt_margen.Text = "";
        }

        private void btn_img_Click(object sender, EventArgs e)
        {
            Application.DoEvents();
            btn_img.Enabled = false;
            Cursor = Cursors.WaitCursor;
            try
            {
                ci++;
                if (ci >= url_imgs.Length)
                {
                    ci = 0;
                }
                string url_img = url_imgs[ci];

                if (!url_img.Trim().Equals(""))
                {
                    /*using (var client = new WebClient())
					{
						client.Headers.Add("User-Agent", "Mozilla/5.0");
						byte[] imageBytes = client.DownloadData(url_img.Trim());
						using (var ms = new MemoryStream(imageBytes))
						{
							pbx_imagen.Image = Image.FromStream(ms);
						}
					}*/

                    pbx_imagen.LoadAsync(url_img.Trim());
                    pbx_imagen.Tag = url_img.Trim();
                    pbx_imagen.Cursor = Cursors.Hand;
                    pbx_imagen.Refresh();
                    lbl_nro.Text = (ci + 1).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            Cursor = Cursors.Default;
            btn_img.Enabled = true;
        }

        private void chk_todos_dif_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow row in dgv_difusiones.Rows)
                {
                    row.Cells["col_sel_dif"].Value = chk_todos_dif.Checked;
                }

                chk_todos.Checked = chk_todos_dif.Checked;

                Difusiones difusiones = new Difusiones();
                string no_portafolio = Convert.ToString(difusiones.ObtenerValorConfiguracion(2));
                string[] no_portafolio2 = no_portafolio.Split(',');
                foreach (DataGridViewRow item in dgv_portafolio.Rows)
                {
                    for (int i = 0; i < no_portafolio2.Length; i++)
                    {
                        if (Convert.ToString(item.Cells["col_portafolio"].Value).Trim().Equals(no_portafolio2[i]))
                        {
                            item.Cells["col_sel"].Value = false;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void chk_todos_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dgv_portafolio.Rows.Count; i++)
                {
                    dgv_portafolio["col_sel", i].Value = chk_todos.Checked;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgv_difusiones_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dgv_difusiones.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgv_difusiones_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 0 && e.RowIndex >= 0)
                {
                    Difusiones difusiones = new Difusiones();
                    int relacion_difusion = Convert.ToInt32(difusiones.ObtenerValorConfiguracion(1));
                    if (relacion_difusion == 1)
                    {
                        if (Convert.ToBoolean(dgv_difusiones["col_sel_dif", e.RowIndex].Value) == true)
                        {
                            foreach (DataGridViewRow row2 in dgv_difusiones.Rows)
                            {
                                if (Convert.ToInt32(row2.Cells["col_cod_dif"].Value) > 0 && (Convert.ToInt32(row2.Cells["col_cod_dif"].Value) < Convert.ToInt32(dgv_difusiones["col_cod_dif", e.RowIndex].Value)) &&
                                    (Convert.ToString(row2.Cells["col_cod_cluster"].Value) == Convert.ToString(dgv_difusiones["col_cod_cluster", e.RowIndex].Value)))
                                {
                                    row2.Cells["col_sel_dif"].Value = true;
                                }
                            }
                        }
                    }
                    dgv_portafolio.Rows.Clear();

                    foreach (DataGridViewRow row in dgv_difusiones.Rows)
                    {
                        if (Convert.ToBoolean(row.Cells["col_sel_dif"].Value) == true)
                        {
                            ListarPortafoliosDifusion(Convert.ToInt32(row.Cells["col_cod_dif"].Value)/*, cluster*/);
                        }
                    }
                    if (dgv_portafolio.Rows.Count > 0)
                    {
                        chk_todos.Checked = false;
                        chk_todos.Checked = true;
                    }
                    else
                    {
                        chk_todos.Checked = false;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
