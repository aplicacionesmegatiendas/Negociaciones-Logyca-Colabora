using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Transitions;

namespace NegociacionesLogycaColabora
{
	public partial class FrmCambioPrecioItem : Form
	{
		List<string> lista_items = null;
		string separador = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;//Esta variable contiene el caracter que el sistema operativo usa para la separación de cifras decimales.

		int alto = 0;

		string raiz;
		public FrmCambioPrecioItem()
		{
			InitializeComponent();

			raiz = Conexion.RaizArchivos;
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

		public string MonthName(int month)
		{
			DateTimeFormatInfo dtinfo = new CultureInfo("es-ES", false).DateTimeFormat;
			return dtinfo.GetMonthName(month);
		}

		private void CargarMotivos()
		{
			DataGridViewComboBoxColumn comboboxColumn = dgv_items.Columns[1] as DataGridViewComboBoxColumn;
			Datos motivos = new Datos();
			comboboxColumn.DataSource = motivos.ListarMotivos();
			comboboxColumn.DisplayMember = "mo_descripcion";
			comboboxColumn.ValueMember = "mo_descripcion";
			comboboxColumn.DropDownWidth = 250;

			cmb_motivos.DataSource = motivos.ListarMotivos();
			cmb_motivos.DisplayMember = "mo_descripcion";
			cmb_motivos.ValueMember = "mo_descripcion";
			cmb_motivos.DropDownWidth = 250;
			cmb_motivos.SelectedIndex = -1;
		}

		private void ObtenerListasPrecio()
		{
			Datos datos = new Datos();
			clb_lista_precios.DataSource = datos.ObtenerListasPrecio();
			clb_lista_precios.DisplayMember = "f112_descripcion";
			clb_lista_precios.ValueMember = "f112_id";
			clb_lista_precios.ClearSelected();
		}

		private void ListarItems()
		{
			Datos datos = new Datos();
			lista_items = datos.ObtenerListadoGeneralCambioPrecio(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador);
		}

		private void ObtenerDatosItem()
		{
			Datos datos = new Datos();

			DataTable dt = datos.ObtenerListasPrecio();
			foreach (DataRow item in dt.Rows)//SE AGREGAN LAS LISTAS DE PRECIO.
			{
				if (!dgv_items.Columns.Contains(item[1].ToString().Trim().Split('-')[0]))
				{
					DataGridViewTextBoxColumn co = new DataGridViewTextBoxColumn();
					co.Name = item[1].ToString().Trim().Split('-')[0];
					co.Visible = true;
					co.ReadOnly = true;
					dgv_items.Columns.Add(co);
				}
			}
			if (!dgv_items.Columns.Contains("fecha_act"))
			{
				DataGridViewTextBoxColumn fecha_act = new DataGridViewTextBoxColumn();
				fecha_act.HeaderText = "FECHA ACTIVACIÓN PVP";
				fecha_act.Name = "fecha_act";
				fecha_act.Visible = true;
				fecha_act.ReadOnly = true;
				dgv_items.Columns.Add(fecha_act);
			}
			if (!dgv_items.Columns.Contains("fecha_inact"))
			{
				DataGridViewTextBoxColumn fecha_inact = new DataGridViewTextBoxColumn();
				fecha_inact.HeaderText = "FECHA INACTIVACIÓN PVP";
				fecha_inact.Name = "fecha_inact";
				fecha_inact.Visible = true;
				fecha_inact.ReadOnly = true;
				dgv_items.Columns.Add(fecha_inact);
			}
			dgv_items.Rows.Clear();

			decimal sum_costo_act = 0;
			decimal sum_costo_nuevo = 0;
			decimal variacion_total = 0;
			foreach (string item in lista_items)
			{
				List<List<object>> info_item = datos.ObtenerDatosItems(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, item.Trim(), 10);
				txt_nit.Text = Convert.ToString(info_item[0][4]);
				txt_razon_social.Text = Convert.ToString(info_item[0][5]);
				txt_sucursal.Text = Convert.ToString(info_item[0][6]);
				decimal costo_actual = Convert.ToDecimal(datos.ObtenerCostoActual(info_item[0][8].ToString(), txt_nit.Text.Trim(), txt_sucursal.Text.Trim()));
				sum_costo_act += costo_actual;
				decimal costo_nuevo = Convert.ToDecimal(info_item[0][11]);
				sum_costo_nuevo += costo_nuevo;
				decimal variacion = 0;

				if (costo_actual > 0 && costo_nuevo > 0)
				{
					variacion = ((costo_nuevo - costo_actual) / costo_actual) * 100;
				}

				dgv_items.Rows.Add(Convert.ToBoolean(info_item[0][18])
					, info_item[0][21], info_item[0][8], info_item[0][9], info_item[0][31], info_item[0][12], info_item[0][10], costo_actual, info_item[0][11], info_item[0][22], variacion.ToString("0.##"),
									info_item[0][19], info_item[0][20], info_item[0][13], info_item[0][14],
									info_item[0][15], info_item[0][16], info_item[0][17],
									info_item[0][23], info_item[0][24], info_item[0][25], info_item[0][26],
									info_item[0][27], info_item[0][28], info_item[0][29], info_item[0][30]);
				if (Convert.ToBoolean(info_item[0][18]).Equals(false))
				{
					dgv_items[1, dgv_items.RowCount - 1].ReadOnly = false;

					DataGridViewComboBoxCell stateCell = (DataGridViewComboBoxCell)(dgv_items.Rows[dgv_items.RowCount - 1].Cells[1]);
					stateCell.Value = info_item[0][21];
				}
				else
				{
					dgv_items[1, dgv_items.RowCount - 1].ReadOnly = true;
				}

				DataTable dtPVP = datos.ListarPreciosItem(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, item.Trim());
				if (dtPVP.Rows.Count > 0)
				{
					foreach (DataRow pvp in dtPVP.Rows)
					{
						for (int i = 26; i < dgv_items.ColumnCount - 2; i++)
						{
							if (pvp[6].ToString().Trim().Equals(dgv_items.Columns[i].HeaderText.Trim()))
							{
								dgv_items[i, dgv_items.RowCount - 1].Value = pvp[7];
								dgv_items[i, dgv_items.RowCount - 1].Tag = pvp[10];
								dgv_items[i, dgv_items.RowCount - 1].ToolTipText = Convert.ToString(pvp[10]);
							}
						}
						if (Convert.ToDateTime(pvp[8], CultureInfo.InvariantCulture).ToString("yyyyMMdd") == "19000101")
						{
							dgv_items[dgv_items.ColumnCount - 2, dgv_items.RowCount - 1].Value = "";
						}
						else
						{
							dgv_items[dgv_items.ColumnCount - 2, dgv_items.RowCount - 1].Value = Convert.ToDateTime(pvp[8], CultureInfo.InvariantCulture).ToString("yyyyMMdd");
						}
						if (Convert.ToDateTime(pvp[9], CultureInfo.InvariantCulture).ToString("yyyyMMdd") == "19000101")
						{
							dgv_items[dgv_items.ColumnCount - 1, dgv_items.RowCount - 1].Value = "";
						}
						else
						{
							dgv_items[dgv_items.ColumnCount - 1, dgv_items.RowCount - 1].Value = Convert.ToDateTime(pvp[9], CultureInfo.InvariantCulture).ToString("yyyyMMdd");
						}
					}
				}

				for (int i = 26; i < dgv_items.ColumnCount - 2; i++)
				{
					dgv_items.Columns[i].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
					dgv_items.Columns[i].DefaultCellStyle.Format = "N2";
				}
			}

			variacion_total += Math.Abs(((sum_costo_act - sum_costo_nuevo) / sum_costo_act) * 100);
			lbl_variacion_total.Text = variacion_total.ToString("0.##");
		}

		private void FrmCambioPrecioItem_Load(object sender, EventArgs e)
		{
			try
			{
				ListarItems();
				if (lista_items != null)
				{
					ObtenerListasPrecio();
					CargarMotivos();

					ObtenerDatosItem();
					lbl_nro.Text = dgv_items.Rows.Count.ToString();
				}
				else
				{
					MessageBox.Show("No hay items para procesar", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					//PARA UBICAR EL ARCHIVO CUANDO NO HAY ITEMS PARA PROCESAR.
					Datos actualizar = new Datos();
					Conectores conector = new Conectores();
					actualizar.ActualizarDocumento();//QUE SE HACE DESPUES
					Conectores.MoverArchivoProcesado(raiz + Datos.GlnComprador + "\\MODIFICACION\\PRECIO\\" + Datos.NombreDocumento);
					conector.RegistrarActividad(Datos.Usuario, Datos.GlnComprador, Datos.GlnProveedor, "CAMBIO_PRECIO", Datos.NumeroDocumento, Datos.NombreDocumento);

					this.Close();
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
				Datos actualizar = new Datos();
				if (actualizar.VerificarItemsPrecioVenta() == 0)
				{
					if (MessageBox.Show("La información de Precio de Venta no esta completa, ¿desea continuar?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.No))
					{
						this.Cursor = Cursors.Default;
						return;
					}
				}
				Conectores conector = new Conectores();
				Configuration config = ConfigurationManager.OpenExeConfiguration(Application.StartupPath + "\\" + Application.ProductName + ".exe");
				AppSettingsSection section = config.AppSettings;

				//conector.CrearBandejas();

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
					conector.CrearConectorCambioPrecio(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador, /*Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\MODIFICACION\\PRECIO\\9_CAMBIO_COSTO_" + txt_razon_social.Text + "_" + fechahora + ".TXT",*/ lista_items);
				}
				catch (Exception ex)
				{
					errores += ex.Message + Environment.NewLine;
				}
				try
				{
					conector.CrearConectorItemPreciosVenta(Datos.NumeroDocumento, Datos.NombreDocumento, Datos.GlnProveedor, Datos.GlnComprador,/* Environment.GetFolderPath((Environment.SpecialFolder.MyDocuments)) + "\\CONECTORES\\MODIFICACION\\PRECIO\\10_CAMBIO_PRECIO_VTA_" + txt_razon_social.Text + "_" + fechahora + ".TXT",*/ lista_items);
				}
				catch (Exception ex)
				{
					errores += ex.Message + Environment.NewLine;
				}

				if (Conectores.dt_resumen.Rows.Count > 0)
					new FrmResumen(Conectores.dt_resumen).ShowDialog(this);
				if (errores != "")
					MessageBox.Show(errores, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				if (Conectores.dt_resumen.Rows.Count.Equals(0) && errores.Equals(string.Empty))
				{
					MessageBox.Show("Conectores creados correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
				}

				actualizar.ActualizarDocumento();
				Conectores.MoverArchivoProcesado(raiz + Datos.GlnComprador + "\\MODIFICACION\\PRECIO\\" + Datos.NombreDocumento);

				conector.RegistrarActividad(Datos.Usuario, Datos.GlnComprador, Datos.GlnProveedor, "CAMBIO_PRECIO", Datos.NumeroDocumento, Datos.NombreDocumento);

				Conectores.LiberarResumen();
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

		private void btn_guardar_Click(object sender, EventArgs e)
		{
			if (txt_nit.Text.Trim().Equals("") || txt_razon_social.Text.Trim().Equals("") || txt_sucursal.Text.Trim().Equals(""))
			{
				MessageBox.Show("La información del proveedor no esta completa", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				Transition.run(lbl_tit_nit, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
				Transition.run(lbl_tit_razon, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
				Transition.run(lbl_tit_suc, "ForeColor", Color.Red, new TransitionType_Flash(5, 300));
				return;
			}
			if (chk_fecha_inact_costo.Checked == true)
			{
				if (dtp_fecha_act_costo.Value.Date > dtp_fecha_inact_costo.Value.Date)
				{
					MessageBox.Show("La Fecha Inactivación Costo no puede ser menor a la Fecha Activación Costo", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}

			if (chk_fecha_inact_pvp.Checked == true)
			{
				if (dtp_fecha_act_pvp.Value.Date > dtp_fecha_inact_pvp.Value.Date)
				{
					MessageBox.Show("La Fecha Inactivación PVP no puede ser menor a la Fecha Activación PVP", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
					return;
				}
			}

			try
			{
				Datos guardar = new Datos();

				foreach (DataGridViewRow item in dgv_items.Rows)
				{
					object[] valores = new object[5];
					valores[0] = item.Cells[2].Value;
					valores[1] = item.Cells[11].Value;
					valores[2] = item.Cells[12].Value;
					valores[3] = Convert.ToBoolean(item.Cells[0].Value);
					valores[4] = item.Cells[1].Value;

					guardar.ActualizarItemCambioPrecio(valores);
				}
				guardar.ActualizarItemPrecioVenta(dgv: dgv_items/*, margen:Convert.ToDecimal(txt_margen.Text)*/);

				MessageBox.Show("Información guardada correctamente", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);

				ObtenerDatosItem();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btn_agregar_precio_Click(object sender, EventArgs e)
		{
			if (txt_margen.Text.Equals(string.Empty))
			{
				MessageBox.Show("Escriba el margen", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txt_margen.Focus();
				return;
			}
			if (clb_lista_precios.CheckedItems.Count == 0)
			{
				MessageBox.Show("Seleccione las listas de precio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				clb_lista_precios.Focus();
				return;
			}
			try
			{
				string fecha_inact = dtp_fecha_inact_pvp.Value.Date.ToString("yyyyMMdd");
				if (!chk_fecha_inact_pvp.Checked)
				{
					fecha_inact = "";
				}

				foreach (DataRowView item in clb_lista_precios.CheckedItems)
				{
					for (int i = 26; i < dgv_items.Columns.Count - 2; i++)
					{
						dgv_items.Columns[i].DefaultCellStyle.Format = "N2";

						if (item[1].ToString().Split('-')[0].Equals(dgv_items.Columns[i].HeaderText))
						{
							for (int j = 0; j < dgv_items.Rows.Count; j++)
							{
								decimal porc_margen = Convert.ToDecimal(txt_margen.Text);
								decimal nuevo_costo = Convert.ToDecimal(dgv_items[8, j].Value);
								decimal iva = Convert.ToDecimal(dgv_items[9, j].Value);

								decimal dscto = 0;
								decimal pvp_con_dscto = nuevo_costo;
								if (dgv_items.Rows.Count > 0)
								{
									for (int k = 14; k < 23; k++)
									{
										if (k == 14 || k == 18 || k == 22)
										{
											dscto = Convert.ToDecimal(dgv_items[k, j].Value.ToString().Replace(",", separador).Replace(".", separador));
											pvp_con_dscto = pvp_con_dscto - ((pvp_con_dscto * dscto) / 100);
										}
									}
								}

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

								dgv_items[i, j].Value = redondeado;
								dgv_items[i, j].Tag = txt_margen.Text;
								dgv_items[i, j].ToolTipText = txt_margen.Text;
								dgv_items[dgv_items.ColumnCount - 2, j].Value = dtp_fecha_act_pvp.Value.Date.ToString("yyyyMMdd");
								dgv_items[dgv_items.ColumnCount - 1, j].Value = fecha_inact;
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void dgv_items_CellContentClick(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				dgv_items[1, e.RowIndex].Selected = true;

			}
		}

		private void dgv_items_CellEndEdit(object sender, DataGridViewCellEventArgs e)
		{
			if (e.ColumnIndex == 0)
			{
				if (Convert.ToBoolean(dgv_items[e.ColumnIndex, e.RowIndex].Value) == false)
				{
					dgv_items[1, e.RowIndex].ReadOnly = false;
				}
				else
				{
					dgv_items[1, e.RowIndex].ReadOnly = true;
					dgv_items[1, e.RowIndex].Value = "";
				}
			}
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

		private void btn_agregar_precio_seleccionados_Click(object sender, EventArgs e)
		{
			if (txt_margen.Text.Equals(string.Empty))
			{
				MessageBox.Show("Escriba el margen", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				txt_margen.Focus();
				return;
			}

			if (clb_lista_precios.CheckedItems.Count == 0)
			{
				MessageBox.Show("Seleccione las listas de precio", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				clb_lista_precios.Focus();
				return;
			}
			if (dgv_items.SelectedRows.Count > 0)
			{
				try
				{
					string fecha_inact = dtp_fecha_inact_pvp.Value.Date.ToString("yyyyMMdd");
					if (!chk_fecha_inact_pvp.Checked)
					{
						fecha_inact = "";
					}

					foreach (DataRowView item in clb_lista_precios.CheckedItems)
					{
						for (int i = 26; i < dgv_items.Columns.Count - 2; i++)
						{
							dgv_items.Columns[i].DefaultCellStyle.Format = "N2";
							if (item[1].ToString().Split('-')[0].Equals(dgv_items.Columns[i].HeaderText))
							{
								for (int j = 0; j < dgv_items.Rows.Count; j++)
								{
									if (dgv_items.Rows[j].Selected == true)
									{
										decimal porc_margen = Convert.ToDecimal(txt_margen.Text);
										decimal nuevo_costo = Convert.ToDecimal(dgv_items[8, j].Value);
										decimal iva = Convert.ToDecimal(dgv_items[9, j].Value);

										decimal dscto = 0;
										decimal pvp_con_dscto = nuevo_costo;
										if (dgv_items.Rows.Count > 0)
										{
											for (int k = 14; k < 23; k++)
											{
												if (k == 14 || k == 18 || k == 22)
												{
													dscto = Convert.ToDecimal(dgv_items[k, j].Value.ToString().Replace(",", separador).Replace(".", separador));
													pvp_con_dscto = pvp_con_dscto - ((pvp_con_dscto * dscto) / 100);
												}
											}
										}

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

										dgv_items[i, j].Value = redondeado;
										dgv_items[i, j].Tag = txt_margen.Text;
										dgv_items[i, j].ToolTipText = txt_margen.Text;
										dgv_items[dgv_items.ColumnCount - 2, j].Value = dtp_fecha_act_pvp.Value.Date.ToString("yyyyMMdd");
										dgv_items[dgv_items.ColumnCount - 1, j].Value = fecha_inact;
									}
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void btn_quitar_precio_Click(object sender, EventArgs e)
		{
			if (dgv_items.SelectedRows.Count > 0)
			{
				try
				{
					foreach (DataRowView item in clb_lista_precios.CheckedItems)
					{
						for (int i = 26; i < dgv_items.ColumnCount; i++)
						{
							string co = item[0].ToString().Split('-')[0];
							string header = dgv_items.Columns[i].HeaderText;
							if (co.Trim().Equals(header.Trim()))
							{
								foreach (DataGridViewRow seleccionada in dgv_items.SelectedRows)
								{
									seleccionada.Cells[i].Value = "";
								}
							}
						}
					}
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}

		private void btn_tamaño_Click(object sender, EventArgs e)
		{
			if (btn_tamaño.ImageIndex == 0)
			{
				alto = panel_items.Height;
				panel_items.Dock = DockStyle.Fill;
				btn_tamaño.ImageIndex = 1;
				this.WindowState = FormWindowState.Maximized;
				return;
			}
			else
			{
				panel_items.Dock = DockStyle.None;
				panel_items.Width = panel_items.Width;
				panel_items.Height = alto;
				btn_tamaño.ImageIndex = 0;
				//this.WindowState = FormWindowState.Normal;
			}
		}

		private void dtp_fecha_act_pvp_ValueChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < dgv_items.RowCount; i++)
			{
				dgv_items[dgv_items.ColumnCount - 2, i].Value = dtp_fecha_act_pvp.Value.Date.ToString("yyyyMMdd");
			}
		}

		private void dtp_fecha_inact_pvp_ValueChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < dgv_items.RowCount; i++)
			{
				dgv_items[dgv_items.ColumnCount - 1, i].Value = dtp_fecha_inact_pvp.Value.Date.ToString("yyyyMMdd");
			}
		}

		private void dtp_fecha_act_costo_ValueChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < dgv_items.RowCount; i++)
			{
				dgv_items[11, i].Value = dtp_fecha_act_costo.Value.Date.ToString("yyyyMMdd");
			}
		}

		private void dtp_fecha_inact_costo_ValueChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < dgv_items.RowCount; i++)
			{
				dgv_items[12, i].Value = dtp_fecha_inact_costo.Value.Date.ToString("yyyyMMdd");
			}
		}

		private void chk_fecha_inact_CheckedChanged(object sender, EventArgs e)
		{
			if (chk_fecha_inact_pvp.CheckState.Equals(CheckState.Checked))
			{
				dtp_fecha_inact_pvp.Enabled = true;
				for (int i = 0; i < dgv_items.RowCount; i++)
				{
					dgv_items[dgv_items.ColumnCount - 1, i].Value = dtp_fecha_inact_pvp.Value.Date.ToString("yyyyMMdd");
				}

			}
			else
			{
				dtp_fecha_inact_pvp.Enabled = false;
				for (int i = 0; i < dgv_items.RowCount; i++)
				{
					dgv_items[dgv_items.ColumnCount - 1, i].Value = "";
				}
			}
		}

		private void chk_fecha_inact_costo_CheckedChanged(object sender, EventArgs e)
		{
			if (chk_fecha_inact_costo.CheckState.Equals(CheckState.Checked))
			{
				dtp_fecha_inact_costo.Enabled = true;
				for (int i = 0; i < dgv_items.RowCount; i++)
				{
					dgv_items[12, i].Value = dtp_fecha_inact_costo.Value.Date.ToString("yyyyMMdd");
				}
			}
			else
			{
				dtp_fecha_inact_costo.Enabled = false;
				for (int i = 0; i < dgv_items.RowCount; i++)
				{
					dgv_items[12, i].Value = "";
				}
			}
		}

		private void chk_aceptado_CheckedChanged_1(object sender, EventArgs e)
		{
			if (chk_aceptado.Checked == true)
			{
				cmb_motivos.Enabled = false;
				cmb_motivos.SelectedIndex = -1;

				for (int i = 0; i < dgv_items.RowCount; i++)
				{
					dgv_items[0, i].Value = Convert.ToBoolean(chk_aceptado.Checked);
					DataGridViewComboBoxCell stateCell = (DataGridViewComboBoxCell)(dgv_items.Rows[i].Cells[1]);
					stateCell.Value = "";
				}
			}
			else
			{
				cmb_motivos.Enabled = true;
				cmb_motivos.SelectedIndex = -1;
			}
		}

		private void cmb_motivos_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cmb_motivos.SelectedIndex >= 0)
			{
				for (int i = 0; i < dgv_items.RowCount; i++)
				{
					dgv_items[0, i].Value = Convert.ToBoolean(chk_aceptado.Checked);
					DataGridViewComboBoxCell stateCell = (DataGridViewComboBoxCell)(dgv_items.Rows[i].Cells[1]);
					stateCell.Value = cmb_motivos.Text;
				}
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

		private void dgv_items_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Delete)
			{
				foreach (DataGridViewCell cell in dgv_items.SelectedCells)
				{
					if (cell.ColumnIndex == 16 || cell.ColumnIndex == 17 ||
						cell.ColumnIndex == 20 || cell.ColumnIndex == 21 ||
						cell.ColumnIndex == 24 || cell.ColumnIndex == 25)
					{
						cell.Value = "";
					}

				}
			}
		}
	}
}
