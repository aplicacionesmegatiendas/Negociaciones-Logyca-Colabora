using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NegociacionesLogycaColabora
{
	public partial class FrmConectores : Form
	{
		public FrmConectores()
		{
			InitializeComponent();
		}

		private void ListarProveedores()
		{
			cmb_proveedores.DisplayMember = "do_razon_social";
			cmb_proveedores.ValueMember = "do_nit";
			Datos dato = new Datos();
			cmb_proveedores.DataSource = dato.ListarProvedoresDocumentos();
			cmb_proveedores.SelectedIndex = -1;
		}

		private void ListarDocumentos(string fecha_ini, string fecha_fin, string nit)
		{
			dgv_documentos.AutoGenerateColumns = false;
			dgv_documentos.Columns[0].DataPropertyName = "do_accion";
			dgv_documentos.Columns[1].DataPropertyName = "do_numero_doc";
			dgv_documentos.Columns[2].DataPropertyName = "do_nombre_doc";
			dgv_documentos.Columns[3].DataPropertyName = "do_gln_comprador";
			dgv_documentos.Columns[4].DataPropertyName = "do_gln_proveedor";
			Datos dato = new Datos();
			dgv_documentos.DataSource = dato.ListarDocumentos(fecha_ini, fecha_fin, nit);
		}

		private void btn_cerrar_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void FrmConectores_Load(object sender, EventArgs e)
		{
			try
			{
				ListarProveedores();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btn_buscar_Click(object sender, EventArgs e)
		{
			if (cmb_proveedores.SelectedIndex == -1)
			{
				MessageBox.Show("Seleccione el proveedor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				cmb_proveedores.Focus();
				return;
			}
			try
			{
				ListarDocumentos(dtp_fecha_ini.Value.Date.ToString("yyyyMMdd"), dtp_fecha_fin.Value.Date.ToString("yyyyMMdd"), Convert.ToString(cmb_proveedores.SelectedValue));
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void dgv_documentos_SelectionChanged(object sender, EventArgs e)
		{
			if (dgv_documentos.RowCount > 0)
			{
				if (dgv_documentos[0, dgv_documentos.CurrentRow.Index].Value.Equals("Adición"))
				{
					adiciónToolStripMenuItem.Enabled = true;
					cambioDePrecioToolStripMenuItem.Enabled = false;
				}
				else if (dgv_documentos[0, dgv_documentos.CurrentRow.Index].Value.Equals("Cambio precio"))
				{
					adiciónToolStripMenuItem.Enabled = false;
					cambioDePrecioToolStripMenuItem.Enabled = true;
				}
			}
		}

		private void itemToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();

				Conectores.PrepararResumen();

				List<string> lista_items = datos.ObtenerListadoGeneralAdicion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value),true);
				Conectores conector = new Conectores();
				//conector.CrearBandejas();
				conector.CrearConectorItemAdicion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\1_AD_CONECTOR_ITEM_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void descripciónTecnicaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();

				Conectores.PrepararResumen();

				List<string> lista_items = datos.ObtenerListadoGeneralAdicion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value),true);
				Conectores conector = new Conectores();
				//conector.CrearBandejas();
				conector.CrearConectorItemDescripcionTecnica(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value),/* Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\2_AD_CONECTOR_ITEM_DESC_TEC_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void criterioToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();

				Conectores.PrepararResumen();

				List<string> lista_items = datos.ObtenerListadoGeneralAdicion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), true);
				Conectores conector = new Conectores();
				//conector.CrearBandejas();
				conector.CrearConectorItemCriterio(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\3_AD_CONECTOR_ITEM_CRITERIO_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void parametrosDePlaneaciónToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();

				Conectores.PrepararResumen();

				List<string> lista_items = datos.ObtenerListadoGeneralAdicion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), true);
				Conectores conector = new Conectores();
				//conector.CrearBandejas();
				conector.CrearConectorItemParametrosPlaneacion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\4_AD_CONECTOR_ITEM_PARAMS_PLAN_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void códigoDeBarrasToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();

				Conectores.PrepararResumen();

				List<string> lista_items = datos.ObtenerListadoGeneralAdicion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), true);
				Conectores conector = new Conectores();
				//conector.CrearBandejas();
				conector.CrearConectorItemCodigoBarras(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\5_AD_CONECTOR_ITEM_CODIGO_BARRAS_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void cotizaciónToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();

				Conectores.PrepararResumen();

				List<string> lista_items = datos.ObtenerListadoGeneralAdicion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), true);
				Conectores conector = new Conectores();
				//conector.CrearBandejas();
				conector.CrearConectorItemCotizacion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\6_AD_CONECTOR_ITEM_COTIZACION_" + cmb_proveedores.Text + ".TXT",*/ Convert.ToString(cmb_proveedores.SelectedValue), "001", lista_items);
				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void precioDeVentaToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();

				Conectores.PrepararResumen();

				List<string> lista_items = datos.ObtenerListadoGeneralAdicion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), true);
				Conectores conector = new Conectores();
				//conector.CrearBandejas();
				conector.CrearConectorItemPreciosVenta(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\7_AD_CONECTOR_ITEM_PRECIO_VTA_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void portafoliosToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				Conectores conector = new Conectores();

				Conectores.PrepararResumen();

				List<string> lista_portafolios = conector.ObtenerListadoPortafolios(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value));

				conector.CrearConectorItemPortafolios(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\",*/ lista_portafolios, cmb_proveedores.Text);
				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void todosToolStripMenuItem_Click(object sender, EventArgs e)
		{
			string errores = "";
			Cursor = Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();

				Conectores.PrepararResumen();

				List<string> lista_items = datos.ObtenerListadoGeneralAdicion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), true);
				Conectores conector = new Conectores();
				//conector.CrearBandejas();
				try
				{
					conector.CrearConectorItemAdicion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\1_AD_CONECTOR_ITEM_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				}
				catch (Exception ex)
				{
					errores += $"ConectorItemAdicion: {ex.Message}{Environment.NewLine}";
				}
				try
				{
					conector.CrearConectorItemDescripcionTecnica(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value),/* Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\2_AD_CONECTOR_ITEM_DESC_TEC_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				}
				catch (Exception ex)
				{
					errores += $"ConectorItemDescripcionTecnica: {ex.Message}{Environment.NewLine}";
				}
				try
				{
					conector.CrearConectorItemCriterio(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\3_AD_CONECTOR_ITEM_CRITERIO_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				}
				catch (Exception ex)
				{
					errores += $"ConectorItemCriterio: {ex.Message}{Environment.NewLine}";
				}
				try
				{
					conector.CrearConectorItemParametrosPlaneacion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\4_AD_CONECTOR_ITEM_PARAMS_PLAN_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				}
				catch (Exception ex)
				{
					errores += $"ConectorItemParametrosPlaneacion: {ex.Message}{Environment.NewLine}";
				}
				try
				{
					conector.CrearConectorItemCodigoBarras(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\5_AD_CONECTOR_ITEM_CODIGO_BARRAS_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				}
				catch (Exception ex)
				{
					errores += $"ConectorItemCodigoBarras: {ex.Message}{Environment.NewLine}";
				}
				try
				{
					conector.CrearConectorItemCotizacion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\6_AD_CONECTOR_ITEM_COTIZACION_" + cmb_proveedores.Text + ".TXT",*/ Convert.ToString(cmb_proveedores.SelectedValue), "001", lista_items);
				}
				catch (Exception ex)
				{
					errores += $"ConectorItemCotizacion: {ex.Message}{Environment.NewLine}";
				}
				try
				{
					conector.CrearConectorItemPreciosVenta(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\7_AD_CONECTOR_ITEM_PRECIO_VTA_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				}
				catch (Exception ex)
				{
					errores += $"ConectorItemPreciosVenta: {ex.Message}{Environment.NewLine}";
				}
				try
				{
					List<string> lista_portafolios = conector.ObtenerListadoPortafolios(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value));
					conector.CrearConectorItemPortafolios(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\",*/ lista_portafolios, cmb_proveedores.Text);
				}
				catch (Exception ex)
				{
					errores += $"ConectorItemPortafolios: {ex.Message}{Environment.NewLine}";
				}
				try
				{
					conector.CrearConectorItemPum(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\ADICION\\5_AD_CONECTOR_ITEM_CODIGO_BARRAS_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				}
				catch (Exception ex)
				{
					errores += $"ConectorItemPum: {ex.Message}{Environment.NewLine}";
				}

				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else if (errores != "")
					MessageBox.Show(errores, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void costoToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor= Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();

				Conectores.PrepararResumen();

				List<string> lista_items = datos.ObtenerListadoGeneralCambioPrecio(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value));

				Conectores conector = new Conectores();
				//conector.CrearBandejas();
				conector.CrearConectorCambioPrecio(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value),/* Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\MODIFICACION\\PRECIO\\1CAMBIO_COSTO_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void pVPToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();
				Conectores.PrepararResumen();
				List<string> lista_items = datos.ObtenerListadoGeneralCambioPrecio(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value));

				Conectores conector = new Conectores();
				//conector.CrearBandejas();
				conector.CrearConectorItemPreciosVenta(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\MODIFICACION\\PRECIO\\2CAMBIO_PRECIO_VTA_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void todosToolStripMenuItem1_Click(object sender, EventArgs e)
		{
			string errores = "";
			Cursor = Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();
				List<string> lista_items = datos.ObtenerListadoGeneralCambioPrecio(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value));

				Conectores conector = new Conectores();
				Conectores.PrepararResumen();
				//conector.CrearBandejas();
				try
				{
					conector.CrearConectorCambioPrecio(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\MODIFICACION\\PRECIO\\1CAMBIO_COSTO_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				}
				catch (Exception ex)
				{
					errores += ex.Message + Environment.NewLine;
				}
				try
				{
					conector.CrearConectorItemPreciosVenta(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\MODIFICACION\\PRECIO\\2CAMBIO_PRECIO_VTA_" + cmb_proveedores.Text + ".TXT",*/ lista_items);
				}
				catch (Exception ex)
				{
					errores += ex.Message + Environment.NewLine;
				}

				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else if (errores != "")
					MessageBox.Show(errores, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}

		private void pumToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Cursor = Cursors.WaitCursor;
			try
			{
				Datos datos = new Datos();

				Conectores.PrepararResumen();

				List<string> lista_items = datos.ObtenerListadoGeneralAdicion(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), true);
				Conectores conector = new Conectores();
				//conector.CrearBandejas();
				conector.CrearConectorItemPum(Convert.ToString(dgv_documentos[1, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[2, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[4, dgv_documentos.CurrentRow.Index].Value), Convert.ToString(dgv_documentos[3, dgv_documentos.CurrentRow.Index].Value), lista_items);
				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				else
					MessageBox.Show("Conector creado correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				Conectores.LiberarResumen();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			Cursor = Cursors.Default;
		}
	}
}
