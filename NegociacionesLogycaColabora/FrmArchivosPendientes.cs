using ClosedXML.Excel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Windows.Forms;

namespace NegociacionesLogycaColabora
{
	public partial class FrmArchivosPendientes : Form
	{
		string raiz;

		public FrmArchivosPendientes()
		{
			InitializeComponent();

			raiz = Conexion.RaizArchivos;
		}

		private void Limpiar()
		{
			txt_proveedor.Text = "";
			txt_razon_soc.Text = "";
			txt_doc.Text = "";
			txt_fecha.Text = "";
			txt_tipo.Text = "";
			txt_accion.Text = "";
			txt_cantidad.Text = "";
			cmb_sucursal.SelectedIndex = -1;
			cmb_sucursal.DataSource = null;
		}

		private void CargarListaArchivos(string ruta)
		{
			try
			{
				string[] files = Directory.GetFiles(ruta);

				lvArchivos.Items.Clear();

				foreach (var item in files)
				{
					string extension = Path.GetExtension(item);

					ListViewItem listviewItem = new ListViewItem(Path.GetFileName(item), extension);
					listviewItem.ImageIndex = 2;
					lvArchivos.Items.Add(listviewItem);
					lvArchivos.LargeImageList = _imageList2;
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void CargarTodoListaArchivos(string ruta, string accion)
		{
			try
			{
				Datos datos = new Datos();
				DataTable dt_compradores = datos.ListarCompradores();
				lvArchivos.Items.Clear();
				foreach (DataRow fila in dt_compradores.Rows)
				{
					string[] files = Directory.GetFiles(ruta + fila[1] + accion, "*.xlsx", SearchOption.TopDirectoryOnly);
					lvArchivos.Groups.Add(new ListViewGroup(fila[1].ToString(), fila[2].ToString() + " [" + fila[1].ToString() + "]"));
					foreach (var item in files)
					{
						string extension = Path.GetExtension(item);
						ListViewItem listviewItem = new ListViewItem(Path.GetFileName(item), extension);
						listviewItem.Group = lvArchivos.Groups[fila[1].ToString()];
						listviewItem.ImageIndex = 2;
						lvArchivos.Items.Add(listviewItem);
						lvArchivos.LargeImageList = _imageList2;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		private void btn_cerrar_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void btn_adicion_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				lvArchivos.Items.Clear();
				if (Datos.TipoUsuario.Equals("3") || Datos.TipoUsuario.Equals("4"))
				{
					CargarTodoListaArchivos(raiz, "\\ADICION");
					lbl_accion.Text = "Adición";
				}
				else
				{
					CargarListaArchivos(raiz + txt_gln.Text + "\\ADICION");
					lbl_accion.Text = "Adición";

				}
				Limpiar();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			this.Cursor = Cursors.Default;
		}

		private void btn_cambioPrecio_Click(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				lvArchivos.Items.Clear();
				if (Datos.TipoUsuario.Equals("3"))
				{
					CargarTodoListaArchivos(raiz, "\\MODIFICACION\\PRECIO");
					lbl_accion.Text = "Cambio Precio";
				}
				else
				{
					CargarListaArchivos(raiz + txt_gln.Text + "\\MODIFICACION\\PRECIO");
					lbl_accion.Text = "Cambio Precio";
				}
				Limpiar();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			this.Cursor = Cursors.Default;
		}

		private void FrmArchivosPendientes_Load(object sender, EventArgs e)
		{
			try
			{
				FrmLogin _FrmLogin = new FrmLogin();
				_FrmLogin.ShowDialog(this);

				this.Text = "Negociaciones Logyca Colabora [" + Datos.Usuario + "]";

				if (Datos.salir == false)//SI LA VARIABLE salir ES false (NO SE DIO CLIC EN Cerrar DEL LOGIN). 
				{
					if (!Directory.Exists(Conexion.RaizArchivos))
					{
						Directory.CreateDirectory(Conexion.RaizArchivos);
					}
					//////AQUI SE ORDENAN LOS ARCHIVOS/////////
					this.Cursor = Cursors.WaitCursor;

					Conectores.AsignarArchivos();

					this.Cursor = Cursors.Default;
					txt_gln.Text = Datos.GlnComprador;
					txt_comprador.Text = Datos.Comprador;

					if (Datos.TipoUsuario.Equals("4"))
					{
						btn_cambioPrecio.Enabled = false;
					}

					lbl_version.Text = "V " + Application.ProductVersion.ToString();
				}
			}
			catch (Exception ex)
			{
				this.Cursor = Cursors.Default;
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}


		private void btn_procesar_Click(object sender, EventArgs e)
		{
			string numero_doc = "";
			string nombre_doc = "";
			string tipo_archivo = "";
			string accion = "";
			string gln_proveedor = "";
			string gln_comprador = "";
			string nit_proveedor = "";
			string razon_social = "";
			string suc_proveedor = "";
			string fecha_elaboracion = "";

			Datos.NumeroDocumento = "";
			Datos.NombreDocumento = "";
			Datos.GlnProveedor = "";

			string separador = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;//Esta variable contiene el caracter que el sistema operativo usa para la separación de cifras decimales.

			XLWorkbook wbook = null;
			IXLWorksheet wsheet = null;

			if (lvArchivos.SelectedItems.Count > 0)
			{
				if (lbl_accion.Text == "Adición")
				{
					try
					{
						this.Cursor = Cursors.WaitCursor;
						Conectores.tipoArchivo = TipoArchivo.NIGUNO;

						string NRO_DOC = "IDENTIFICADOR NEGOCIACIÓN";
						string GLN_PROVEEDOR = "GLN PROVEEDOR";
						string GLN_COMPRADOR = "GLN COMPRADOR";
						string ACCION = "ACCIONES";
						string FECHA_ELABORACION = "FECHA CREACION";

						int COLUMNA_NRO_DOC = 0;
						int COLUMNA_GLN_PROVEEDOR = 0;
						int COLUMNA_GLN_COMPRADOR = 0;
						int COLUMNA_ACCION = 0;
						int COLUMNA_FECHA_ELABORACION = 0;

						if (Datos.TipoUsuario.Equals("3") || Datos.TipoUsuario.Equals("4"))
						{
							wbook = new XLWorkbook(raiz + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name + "\\ADICION\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text);
							wsheet = wbook.Worksheet(1);

							Datos.GlnComprador = lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name;
							gln_comprador = lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name;

							COLUMNA_NRO_DOC = Datos.GetColumnIndex(NRO_DOC, wsheet);
							COLUMNA_GLN_PROVEEDOR = Datos.GetColumnIndex(GLN_PROVEEDOR, wsheet);
							COLUMNA_GLN_COMPRADOR = Datos.GetColumnIndex(GLN_COMPRADOR, wsheet);
							COLUMNA_ACCION = Datos.GetColumnIndex(ACCION, wsheet);
							COLUMNA_FECHA_ELABORACION = Datos.GetColumnIndex(FECHA_ELABORACION, wsheet);
						}
						else
						{
							wbook = new XLWorkbook(raiz + txt_gln.Text + "\\ADICION\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text);
							wsheet = wbook.Worksheet(1);
							gln_comprador = txt_gln.Text;

							COLUMNA_NRO_DOC = Datos.GetColumnIndex(NRO_DOC, wsheet);
							COLUMNA_GLN_PROVEEDOR = Datos.GetColumnIndex(GLN_PROVEEDOR, wsheet);
							COLUMNA_GLN_COMPRADOR = Datos.GetColumnIndex(GLN_COMPRADOR, wsheet);
							COLUMNA_ACCION = Datos.GetColumnIndex(ACCION, wsheet);
							COLUMNA_FECHA_ELABORACION = Datos.GetColumnIndex(FECHA_ELABORACION, wsheet);
						}
						Datos datos = new Datos();

						Datos.NombreDocumento = lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text;// + "_" + datos.ObtenerConsecutivoNumeroDocumento(151).ToString();

						if (COLUMNA_ACCION > 0)
						{
							accion = Datos.GetValue(COLUMNA_ACCION, wsheet);
						}
						if (accion == "Adición")//SUPONIENDO QUE LA ACCION SEA NUMERICA.
						{
							//SE BUSCA EL NIT Y LA SUCURSAL DEL PROVEEDOR A PARTIR DEL GLN
							if (COLUMNA_GLN_PROVEEDOR > 0)
							{
								gln_proveedor = Datos.GetValue(COLUMNA_GLN_PROVEEDOR, wsheet);
							}

							Datos.GlnProveedor = gln_proveedor;

							string[] info = null;
							info = datos.ObtenerNitProveedor(gln_proveedor);
							if (info != null)
							{
								nit_proveedor = info[0].ToString();
								razon_social = info[3].ToString();
								suc_proveedor = cmb_sucursal.Text;
							}
							else
							{
								MessageBox.Show("El Gln de Provedor no tiene un NIT asociado en la base de datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								this.Cursor = Cursors.Default;
								return;
							}
							if (COLUMNA_NRO_DOC > 0)
							{
								numero_doc = Datos.GetValue(COLUMNA_NRO_DOC, wsheet);
							}

							Datos.NumeroDocumento = numero_doc;

							nombre_doc = Datos.NombreDocumento;
							tipo_archivo = "LogycaColabora";//TIPO NEGOCIACIÓN

							if (COLUMNA_FECHA_ELABORACION > 0)
							{
								string fecha = Datos.GetValue(COLUMNA_FECHA_ELABORACION, wsheet);

								fecha_elaboracion = Convert.ToDateTime(fecha, CultureInfo.InvariantCulture).ToString("yyyyMMdd");
							}

							datos.GuardarInfoDocumento(numero_doc, nombre_doc, gln_proveedor, gln_comprador, tipo_archivo, accion, nit_proveedor, razon_social, suc_proveedor,
															fecha_elaboracion, Datos.Usuario);

							datos.GuardarInfoItems(numero_doc, nombre_doc, gln_proveedor, gln_comprador, wsheet);
							datos.GuardarInfoCotizacion(numero_doc, nombre_doc, gln_proveedor, gln_comprador, wsheet);
							datos.GuardarInfoDescripcionTecnica(numero_doc, nombre_doc, gln_proveedor, gln_comprador, wsheet);
							datos.GuardarInfoCodigoBarras(numero_doc, nombre_doc, gln_proveedor, gln_comprador, wsheet);

							datos.GuardarInfoOtrosDatos(numero_doc, nombre_doc, gln_proveedor, gln_comprador, wsheet);

							FrmAdicionItems _frmAdicionItems = new FrmAdicionItems();
							_frmAdicionItems.txt_nro_doc.Text = numero_doc;
							_frmAdicionItems.txt_nomb_doc.Text = nombre_doc;
							_frmAdicionItems.ShowDialog(this);
						}
						else
						{
							MessageBox.Show("La acción de este archivo no es de adición", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							Conectores.tipoArchivo = TipoArchivo.NIGUNO;
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					this.Cursor = Cursors.Default;
				}

				else if (lbl_accion.Text == "Cambio Precio")
				{
					try
					{
						this.Cursor = Cursors.WaitCursor;
						Conectores.tipoArchivo = TipoArchivo.NIGUNO;


						string NRO_DOC = "IDENTIFICADOR NEGOCIACIÓN";
						string GLN_PROVEEDOR = "GLN PROVEEDOR";
						string GLN_COMPRADOR = "GLN COMPRADOR";
						string ACCION = "ACCIONES";
						string FECHA_ELABORACION = "FECHA CREACION";

						int COLUMNA_NRO_DOC = 0;
						int COLUMNA_GLN_PROVEEDOR = 0;
						int COLUMNA_GLN_COMPRADOR = 0;
						int COLUMNA_ACCION = 0;
						int COLUMNA_FECHA_ELABORACION = 0;

						if (Datos.TipoUsuario.Equals("3"))
						{
							wbook = new XLWorkbook(raiz + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name + "\\MODIFICACION\\PRECIO\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text);
							wsheet = wbook.Worksheet(1);

							Datos.GlnComprador = lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name;
							gln_comprador = lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name;

							COLUMNA_NRO_DOC = Datos.GetColumnIndex(NRO_DOC, wsheet);
							COLUMNA_GLN_PROVEEDOR = Datos.GetColumnIndex(GLN_PROVEEDOR, wsheet);
							COLUMNA_GLN_COMPRADOR = Datos.GetColumnIndex(GLN_COMPRADOR, wsheet);
							COLUMNA_ACCION = Datos.GetColumnIndex(ACCION, wsheet);
							COLUMNA_FECHA_ELABORACION = Datos.GetColumnIndex(FECHA_ELABORACION, wsheet);
						}
						else
						{
							wbook = new XLWorkbook(raiz + txt_gln.Text + "\\MODIFICACION\\PRECIO\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text);
							wsheet = wbook.Worksheet(1);
							gln_comprador = txt_gln.Text;

							COLUMNA_NRO_DOC = Datos.GetColumnIndex(NRO_DOC, wsheet);
							COLUMNA_GLN_PROVEEDOR = Datos.GetColumnIndex(GLN_PROVEEDOR, wsheet);
							COLUMNA_GLN_COMPRADOR = Datos.GetColumnIndex(GLN_COMPRADOR, wsheet);
							COLUMNA_ACCION = Datos.GetColumnIndex(ACCION, wsheet);
							COLUMNA_FECHA_ELABORACION = Datos.GetColumnIndex(FECHA_ELABORACION, wsheet);

						}

						Datos.NombreDocumento = lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text;

						if (COLUMNA_ACCION > 0)
						{
							accion = Datos.GetValue(COLUMNA_ACCION, wsheet);
						}

						if (accion == "Modificación")
						{
							Datos validacion = new Datos();
							if (COLUMNA_GLN_PROVEEDOR > 0)
							{
								gln_proveedor = Datos.GetValue(COLUMNA_GLN_PROVEEDOR, wsheet);
							}
							if (gln_proveedor.Equals(""))
							{
								MessageBox.Show("No hay Gln de Proveedor en el documento de negociación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								this.Cursor = Cursors.Default;
								return;
							}
							Datos.GlnProveedor = gln_proveedor;

							string[] info = null;
							info = validacion.ObtenerNitProveedor(gln_proveedor);
							if (info != null)
							{
								nit_proveedor = info[0].ToString();
								razon_social = info[3].ToString();
								suc_proveedor = cmb_sucursal.Text;
							}
							else
							{
								MessageBox.Show("El Gln de Proveedor no tiene un NIT asociado en la base de datos", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
								this.Cursor = Cursors.Default;
								return;
							}

							if (COLUMNA_NRO_DOC > 0)
							{
								numero_doc = Datos.GetValue(COLUMNA_NRO_DOC, wsheet);
							}
							Datos.NumeroDocumento = numero_doc;

							nombre_doc = Datos.NombreDocumento;
							tipo_archivo = "LogycaColabora";

							if (COLUMNA_FECHA_ELABORACION > 0)
							{
								string fecha = Datos.GetValue(COLUMNA_FECHA_ELABORACION, wsheet);

								fecha_elaboracion = Convert.ToDateTime(fecha, CultureInfo.InvariantCulture).ToString("yyyyMMdd");
							}

							Datos guardar = new Datos();
							guardar.GuardarInfoDocumento(numero_doc, nombre_doc, gln_proveedor, Datos.GlnComprador, tipo_archivo, accion, nit_proveedor, razon_social, suc_proveedor, fecha_elaboracion, Datos.Usuario);
							guardar.GuardarInfoCambioPrecio(numero_doc, nombre_doc, gln_proveedor, Datos.GlnComprador, wsheet);

							FrmCambioPrecioItem _FrmCambioPrecioItem = new FrmCambioPrecioItem();
							_FrmCambioPrecioItem.txt_nro_doc.Text = numero_doc;
							_FrmCambioPrecioItem.txt_nomb_doc.Text = nombre_doc;
							_FrmCambioPrecioItem.ShowDialog(this);
						}
						else
						{
							MessageBox.Show("La acción de este archivo no es de modificación", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
							Conectores.tipoArchivo = TipoArchivo.NIGUNO;
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
					this.Cursor = Cursors.Default;
				}
				lvArchivos.Items.Clear();
				txt_proveedor.Text = "";
				txt_razon_soc.Text = "";
				txt_doc.Text = "";
				txt_fecha.Text = "";
				txt_tipo.Text = "";
				txt_accion.Text = "";
				txt_cantidad.Text = "";
				cmb_sucursal.SelectedIndex = -1;
				cmb_sucursal.DataSource = null;
			}
		}

		private void FrmArchivosPendientes_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void lvArchivos_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.Cursor = Cursors.WaitCursor;
			try
			{
				Limpiar();

				string ACCION = "ACCIONES";
				string GLN_PROVEEDOR = "GLN PROVEEDOR";
				string NRO_DOC = "IDENTIFICADOR NEGOCIACIÓN";
				string FECHA_ELABORACION = "FECHA CREACION";

				int COLUMNA_ACCION = 0;
				int COLUMNA_GLN_PROVEEDOR = 0;
				int COLUMNA_NRO_DOC = 0;
				int COLUMNA_FECHA_ELABORACION = 0;

				Datos validacion = new Datos();
				if (lvArchivos.SelectedIndices.Count > 0)
				{
					switch (lbl_accion.Text)
					{
						case "Adición":

							XLWorkbook wbook_adicion = null;
							IXLWorksheet wsheet_adicion = null;
							if (Datos.TipoUsuario.Equals("3") || Datos.TipoUsuario.Equals("4"))
							{
								wbook_adicion = new XLWorkbook(raiz + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name + "\\ADICION\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text);
								wsheet_adicion = wbook_adicion.Worksheet(1);

								COLUMNA_ACCION = Datos.GetColumnIndex(ACCION, wsheet_adicion);
								COLUMNA_GLN_PROVEEDOR = Datos.GetColumnIndex(GLN_PROVEEDOR, wsheet_adicion);
								COLUMNA_NRO_DOC = Datos.GetColumnIndex(NRO_DOC, wsheet_adicion);
								COLUMNA_FECHA_ELABORACION = Datos.GetColumnIndex(FECHA_ELABORACION, wsheet_adicion);
							}
							else
							{
								wbook_adicion = new XLWorkbook(raiz + txt_gln.Text + "\\ADICION\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text);
								wsheet_adicion = wbook_adicion.Worksheet(1);

								COLUMNA_ACCION = Datos.GetColumnIndex(ACCION, wsheet_adicion);
								COLUMNA_GLN_PROVEEDOR = Datos.GetColumnIndex(GLN_PROVEEDOR, wsheet_adicion);
								COLUMNA_NRO_DOC = Datos.GetColumnIndex(NRO_DOC, wsheet_adicion);
								COLUMNA_FECHA_ELABORACION = Datos.GetColumnIndex(FECHA_ELABORACION, wsheet_adicion);
							}

							int cantidad_adicion = wsheet_adicion.LastRowUsed().RowNumber() - 1;
							string accion_adicion = "";
							string nro_doc_adicion = "";
							string gln_proveedor_adicion = "";
							string fecha_elaboracion_adicion = "";
							string tipo_archivo_adicion = "LogycaColabora";

							if (COLUMNA_ACCION > 0)
							{
								accion_adicion = Datos.GetValue(COLUMNA_ACCION, wsheet_adicion);
							}
							if (COLUMNA_NRO_DOC > 0)
							{
								nro_doc_adicion = Datos.GetValue(COLUMNA_NRO_DOC, wsheet_adicion);
							}
							if (COLUMNA_GLN_PROVEEDOR > 0)
							{
								gln_proveedor_adicion = Datos.GetValue(COLUMNA_GLN_PROVEEDOR, wsheet_adicion);
							}
							if (COLUMNA_FECHA_ELABORACION > 0)
							{
								fecha_elaboracion_adicion = Datos.GetValue(COLUMNA_FECHA_ELABORACION, wsheet_adicion);
							}

							txt_doc.Text = nro_doc_adicion;
							txt_fecha.Text = fecha_elaboracion_adicion;
							txt_tipo.Text = tipo_archivo_adicion;
							txt_accion.Text = "Adición";
							txt_cantidad.Text = cantidad_adicion.ToString();
							string[] info_adicion = null;

							info_adicion = validacion.ObtenerNitProveedor(gln_proveedor_adicion);
							if (info_adicion != null)
							{
								txt_proveedor.Text = info_adicion[0];
								txt_razon_soc.Text = info_adicion[3];
								List<string> sucursales = validacion.ObtenerSucursalesProveedor(info_adicion[2].Trim());
								cmb_sucursal.DataSource = sucursales;
							}
							else
							{
								MessageBox.Show("No hay información para el proveedor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}

							break;

						case "Cambio Precio":
							XLWorkbook wbook_cambio = null;
							IXLWorksheet wsheet_cambio = null;
							if (Datos.TipoUsuario.Equals("3"))
							{
								wbook_cambio = new XLWorkbook(raiz + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name + "\\MODIFICACION\\PRECIO\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text);
								wsheet_cambio = wbook_cambio.Worksheet(1);

								COLUMNA_ACCION = Datos.GetColumnIndex(ACCION, wsheet_cambio);
								COLUMNA_GLN_PROVEEDOR = Datos.GetColumnIndex(GLN_PROVEEDOR, wsheet_cambio);
								COLUMNA_NRO_DOC = Datos.GetColumnIndex(NRO_DOC, wsheet_cambio);
								COLUMNA_FECHA_ELABORACION = Datos.GetColumnIndex(FECHA_ELABORACION, wsheet_cambio);
							}
							else
							{
								wbook_cambio = new XLWorkbook(raiz + txt_gln.Text + "\\MODIFICACION\\PRECIO\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text);
								wsheet_cambio = wbook_cambio.Worksheet(1);

								COLUMNA_ACCION = Datos.GetColumnIndex(ACCION, wsheet_cambio);
								COLUMNA_GLN_PROVEEDOR = Datos.GetColumnIndex(GLN_PROVEEDOR, wsheet_cambio);
								COLUMNA_NRO_DOC = Datos.GetColumnIndex(NRO_DOC, wsheet_cambio);
								COLUMNA_FECHA_ELABORACION = Datos.GetColumnIndex(FECHA_ELABORACION, wsheet_cambio);
							}

							int cantidad_cambio = wsheet_cambio.LastRowUsed().RowNumber() - 1;
							string accion_cambio = "";
							string nro_doc_cambio = "";
							string gln_proveedor_cambio = "";
							string fecha_elaboracion_cambio = "";
							string tipo_archivo_cambio = "LogycaColabora";

							if (COLUMNA_ACCION > 0)
							{
								accion_cambio = Datos.GetValue(COLUMNA_ACCION, wsheet_cambio);
							}
							if (COLUMNA_NRO_DOC > 0)
							{
								nro_doc_cambio = Datos.GetValue(COLUMNA_NRO_DOC, wsheet_cambio);
							}
							if (COLUMNA_GLN_PROVEEDOR > 0)
							{
								gln_proveedor_cambio = Datos.GetValue(COLUMNA_GLN_PROVEEDOR, wsheet_cambio);
							}
							if (COLUMNA_FECHA_ELABORACION > 0)
							{
								fecha_elaboracion_cambio = Datos.GetValue(COLUMNA_FECHA_ELABORACION, wsheet_cambio);
							}

							txt_doc.Text = nro_doc_cambio;
							txt_fecha.Text = fecha_elaboracion_cambio;
							txt_tipo.Text = tipo_archivo_cambio;
							txt_accion.Text = "Cambio precio";
							txt_cantidad.Text = cantidad_cambio.ToString();
							string[] info_cambio = null;

							info_cambio = validacion.ObtenerNitProveedor(gln_proveedor_cambio);
							if (info_cambio != null)
							{
								txt_proveedor.Text = info_cambio[0];
								txt_razon_soc.Text = info_cambio[3];
								List<string> sucursales = validacion.ObtenerSucursalesProveedor(info_cambio[2].Trim());
								cmb_sucursal.DataSource = sucursales;
							}
							else
							{
								MessageBox.Show("No hay información para el proveedor", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
							}
							break;
					}
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			this.Cursor = Cursors.Default;
		}

		private void crearConectorToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FrmConectores().ShowDialog(this);
		}

		private void consultarProcesadosToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FrmConsultaProcesados().ShowDialog(this);
		}

		private void consultaCambioDePrecioToolStripMenuItem_Click(object sender, EventArgs e)
		{
			new FrmConsultaCambioPrecio().ShowDialog(this);
		}

		private void btn_mover_Click(object sender, EventArgs e)
		{
			try
			{
				if (lbl_accion.Text == "Adición")
				{

					if (MessageBox.Show("¿Confirma mover esta archivo?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
					{
						Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(raiz + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name + "\\ADICION\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text,
										   raiz + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name + "\\PROCESADOS\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text,
										   Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
						btn_adicion.PerformClick();
					}
				}
				if (lbl_accion.Text == "Cambio Precio")
				{
					if (MessageBox.Show("¿Confirma mover esta archivo?", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question).Equals(DialogResult.Yes))
					{
						Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(raiz + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name + "\\MODIFICACION\\PRECIO\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text,
									   raiz + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Group.Name + "\\PROCESADOS\\" + lvArchivos.Items[lvArchivos.SelectedIndices[0]].Text,
									   Microsoft.VisualBasic.FileIO.UIOption.AllDialogs, Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);
						btn_cambioPrecio.PerformClick();
					}
				}
			}
			catch (IOException ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
