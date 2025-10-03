using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Data.SqlClient;
using System.Data;
using ClosedXML.Excel;
using System.Configuration;
using System.Globalization;

namespace NegociacionesLogycaColabora
{
	public class Conectores
	{
		public static TipoArchivo tipoArchivo = TipoArchivo.NIGUNO;

		public static DataTable dt_resumen = null;

		public static void PrepararResumen()
		{
			if (dt_resumen == null)
			{
				dt_resumen = new DataTable();
				dt_resumen.Columns.Add("col_conector");
				dt_resumen.Columns.Add("col_linea");
				dt_resumen.Columns.Add("col_tipo_registro");
				dt_resumen.Columns.Add("col_sub_tipo_registro");
				dt_resumen.Columns.Add("col_version");
				dt_resumen.Columns.Add("col_nivel");
				dt_resumen.Columns.Add("col_error");
				dt_resumen.Columns.Add("col_detalle");
			}
		}

		public static void LiberarResumen()
		{
			if (dt_resumen != null)
			{
				dt_resumen.Dispose();
				dt_resumen = null;
			}
		}

		private void AddText(FileStream fs, string value)
		{
			string Resultado = string.Format("{0}{1}", value, Environment.NewLine);

			byte[] info = new UTF8Encoding(true).GetBytes(Resultado);

			fs.Write(info, 0, info.Length);
		}

		/// <summary>
		/// Formatea las cantidades y precios con los enteros y decimales correspondientes.
		/// </summary>
		/// <param name="valor">Cantidad a la que se le aplica el formato.</param>
		/// <param name="enteros">Cantidad de enteros que debe tener.</param>
		/// <param name="decimales">Cantidad de decimales que debe tener.</param>
		/// <returns></returns>
		private string FormatoCantidadPrecio(string valor, int enteros, int decimales)
		{
			int i, j;

			string partEnt = "";//Guarda la parte entera.
			string partDec = "";//Guarda la parte decimal.
			char[] nums = valor.ToCharArray();//Guarda el contenido de el parametro valor como un array de tipo char.
			char separador = Convert.ToChar(System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);//Esta variable contiene el caracter que el sistema operativo usa para la separación de cifras decimales.

			for (i = 0; i < nums.Length; i++)
			{
				if (!nums[i].Equals('.') && !nums[i].Equals(','))//Cuando no es un punto o una coma.
				{
					partEnt += nums[i];//Se le agrega un numero a la parte entera.
				}
				else
				{
					break;//Cuando encuentra un punto o una coma se sale del ciclo.
				}
			}

			partEnt = partEnt.PadLeft(enteros, '0');//Se le agrega a la variable partEnt ceros a la izquierda hasta que complete la longitud.

			for (j = i + 1; j < nums.Length; j++)
			{
				partDec += nums[j];//Se le agrega un numero a la parte decimal.
			}

			partDec = partDec.PadRight(decimales, '0');//Se le agrega a la variable partDec ceros a la izquierda hasta que complete la longitud.

			return (partEnt + "." + partDec);
		}
		/// <summary>
		/// Comprueba que la información para generar el conector item adición este completa.
		/// </summary>
		/// <returns>Devuelve el número de items</returns>
		public int ComprobarItemAdicion()
		{
			string SQL = "SELECT COUNT(it_gtin) nro " +
							"FROM Documentos " +
							"INNER JOIN " +
								"Items ON do_numero_doc=it_numero_doc AND do_nombre_doc=it_nombre_doc " +
										 "AND do_gln_proveedor=it_gln_proveedor AND do_gln_comprador=it_gln_comprador " +
							"WHERE do_numero_doc=@NRO_DOC AND do_nombre_doc=@NOMB_DOC " +
									"AND do_gln_proveedor=@GLN_PROV AND do_gln_comprador=@GLN_COMP " +
									"AND  it_grupo_impositivo is null " +
									"AND it_tipo_inventario is null " +
									"AND it_unidad_inventario is null " +
									"AND it_aceptado=1";
			int r = -1;

			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@NRO_DOC", Datos.NumeroDocumento);
				cmd.Parameters.AddWithValue("@NOMB_DOC", Datos.NombreDocumento);
				cmd.Parameters.AddWithValue("@GLN_PROV", Datos.GlnProveedor);
				cmd.Parameters.AddWithValue("@GLN_COMP", Datos.GlnComprador);
				r = Convert.ToInt32(cmd.ExecuteScalar());
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al comprobar item adición: " + ex.Message);
			}
			return r;
		}

		/// <summary>
		/// Crea el conector para adicion de item
		/// </summary>
		/// <param name="nombreArchivo">Nombre y ubicación del archivo.</param>
		/// <param name="datos">Información para formar el archivo.</param>
		public void CrearConectorItemAdicion(string numero_doc, string nombre_doc, string gln_prov, string gln_comp, /*string nombreArchivo*/ List<string> items)
		{
			////////////////////////VARIABLES///////////////////////
			string movimientos = "";
			int nroReg = 2;

			string REG_INICIO = "000000100000001001";

			string F_NUMERO_REG = ""; //Numerico 7 - Numero consecutivo
			string F_TIPO_REG = "0120"; //Numerico 4 - Valor fijo = 120
			string F_SUBTIPO_REG = "00"; //Numerico 2 - Valor fijo = 00
			string F_VERSION_REG = "07"; //Numerico 2 - Version = 01
			string F_CIA = "001"; // Numerico 3 - compañía a la cual pertenece la informacion del registro
			string F_ACTUALIZA_REG = "0";
			string f120_id = "0".PadLeft(7, '0');

			string f120_referencia = "";
			string f120_descripcion = "";
			string f120_descripcion_corta = "";
			string f120_id_grupo_impositivo = "";
			string f120_id_tipo_inv_serv = "";
			string f120_id_grupo_dscto = " ".PadRight(4, ' '); //NO APLICA
			string f120_id_segmento_costo = "1".PadLeft(3, '0');
			string f120_ind_tipo_item = "1";
			string f120_ind_compra = "";//traer
			string f120_ind_venta = "";//traer
			string f120_ind_manufactura = "";//traer
			string f120_ind_lote = "0";
			string f120_ind_lote_asignacion = "0";
			string f120_vida_util = "0".PadLeft(4, '0');
			string f120_id_tercero_prov = " ".PadRight(15, ' ');
			string f120_id_sucursal_prov = " ".PadRight(3, ' ');
			string f120_id_tercero_cli = " ".PadRight(15, ' ');
			string f120_id_sucursal_cli = " ".PadRight(3, ' ');
			string f120_id_unidad_inventario = "";
			string f120_factor_peso_inventario = "";
			string f120_factor_volumen_inventario = "";
			string f120_id_unidad_adicional = " ".PadRight(4, ' ');
			string f120_factor_adicional = "000000.0000";
			string f120_factor_peso_adicional = "000000.0000";
			string f120_factor_volumen_adicional = "000000.0000";
			string f120_id_unidad_orden = "";
			string f120_factor_orden = "";
			string f120_factor_peso_orden = "";
			string f120_factor_volumen_orden = "";
			string f120_id_unidad_empaque = "";
			string f120_factor_empaque = "";
			string f120_factor_peso_empaque = "";
			string f120_factor_volumen_empaque = "";
			string f120_ind_lista_precios_ext = "0";
			string f121_ind_estado = "1";
			string f121_fecha_inactivacion = " ".PadRight(8, ' ');
			string f121_fecha_creacion = DateTime.Now.Date.ToString("yyyyMMdd");
			string f120_notas = "";// "Generado desde Logyca Colabora".PadRight(255, ' ');
			string f120_ind_serial = "0";
			//////////////////////////////////////////////////
			string f120_id_cfg_serial = " ".PadRight(10, ' ');
			string f120_ind_paquete = "0";
			string f120_ind_exento = "0";
			string f120_ind_generico = "0";
			string f120_id_item_ext_generico = "0".PadLeft(7, '0');
			string f120_referencia_item_generico = " ".PadRight(50, ' ');
			string f121_id_ext1_detalle = " ".PadRight(20, ' ');
			string f121_id_ext2_detalle = " ".PadRight(20, ' ');

			string REG_CIERRE = "";

			/*FileStream fs = null;

			fs = File.Create(nombreArchivo);

			AddText(fs, REG_INICIO);*/

			WSUNOEE.WSUNOEE wsunoee = new WSUNOEE.WSUNOEE();
			int err = 1;
			string xml_item_adicion = $@"<?xml version=""1.0"" encoding=""utf-8""?>
                                             <Importar>
                                                <NombreConexion>unoee_invercomer</NombreConexion>
                                                <IdCia>1</IdCia>
                                                <Usuario>ws_importar_mega</Usuario>
												<Clave>rLFgx)bL</Clave>
                                                <Datos>
                                                    <Linea>{REG_INICIO}</Linea>";
			/*
			 <Usuario>osalcedo</Usuario>
			 <Clave>Auror@02</Clave>
			 */
			Datos datos = new Datos();
			foreach (string item in items)
			{
				List<List<object>> info_item = datos.ObtenerDatosItems(numero_doc, nombre_doc, gln_prov, gln_comp, item, 1);

				if (Convert.ToBoolean(info_item[0][22]) == true)
				{
					movimientos = "";

					F_NUMERO_REG = nroReg.ToString().PadLeft(7, '0');
					movimientos += F_NUMERO_REG;

					movimientos += F_TIPO_REG;

					movimientos += F_SUBTIPO_REG;

					movimientos += F_VERSION_REG;

					movimientos += F_CIA;

					movimientos += F_ACTUALIZA_REG;

					movimientos += f120_id;

					f120_referencia = Convert.ToString(info_item[0][8]).TrimEnd('\n', '\r').Trim().PadRight(/*20*/50, ' ');
					movimientos += f120_referencia;

					f120_descripcion = Convert.ToString(info_item[0][9]).TrimEnd('\n', '\r').Trim().PadRight(40, ' ');
					movimientos += f120_descripcion;

					f120_descripcion_corta = Convert.ToString(info_item[0][10]).TrimEnd('\n', '\r').Trim().PadRight(20, ' ');
					movimientos += f120_descripcion_corta;

					f120_id_grupo_impositivo = Convert.ToString(info_item[0][11]).Trim().PadRight(4, ' ');
					movimientos += f120_id_grupo_impositivo;

					f120_id_tipo_inv_serv = Convert.ToString(info_item[0][12]).Trim().PadRight(10, ' ');
					movimientos += f120_id_tipo_inv_serv;

					movimientos += f120_id_grupo_dscto;

					movimientos += f120_id_segmento_costo;

					movimientos += f120_ind_tipo_item;

					f120_ind_compra = Convert.ToInt32(info_item[0][25]).ToString();
					movimientos += f120_ind_compra;

					f120_ind_venta = Convert.ToInt32(info_item[0][26]).ToString();
					movimientos += f120_ind_venta;

					f120_ind_manufactura = Convert.ToInt32(info_item[0][27]).ToString();
					movimientos += f120_ind_manufactura;

					movimientos += f120_ind_lote;

					movimientos += f120_ind_lote_asignacion;

					movimientos += f120_vida_util;

					movimientos += f120_id_tercero_prov;

					movimientos += f120_id_sucursal_prov;

					movimientos += f120_id_tercero_cli;

					movimientos += f120_id_sucursal_cli;

					f120_id_unidad_inventario = Convert.ToString(info_item[0][13]).Trim().PadRight(4, ' ');
					movimientos += f120_id_unidad_inventario;

					f120_factor_peso_inventario = FormatoCantidadPrecio(Convert.ToString(info_item[0][14]).Trim(), 6, 4);
					movimientos += f120_factor_peso_inventario;

					f120_factor_volumen_inventario = FormatoCantidadPrecio("1", 6, 4);
					movimientos += f120_factor_volumen_inventario;

					movimientos += f120_id_unidad_adicional;

					movimientos += f120_factor_adicional;

					movimientos += f120_factor_peso_adicional;

					movimientos += f120_factor_volumen_adicional;

					f120_id_unidad_orden = Convert.ToString(info_item[0][15]).Trim().PadRight(4, ' ');
					movimientos += f120_id_unidad_orden;

					f120_factor_orden = FormatoCantidadPrecio(Convert.ToString(info_item[0][16]).Trim(), 6, 4);
					movimientos += f120_factor_orden;

					f120_factor_peso_orden = FormatoCantidadPrecio(Convert.ToString(info_item[0][17]).Trim(), 6, 4);
					movimientos += f120_factor_peso_orden;

					f120_factor_volumen_orden = FormatoCantidadPrecio("1", 6, 4);
					movimientos += f120_factor_volumen_orden;

					f120_id_unidad_empaque = Convert.ToString(info_item[0][18]).Trim().PadRight(4, ' ');
					movimientos += f120_id_unidad_empaque;

					f120_factor_empaque = FormatoCantidadPrecio(Convert.ToString(info_item[0][19]).Trim(), 6, 4);
					movimientos += f120_factor_empaque;

					f120_factor_peso_empaque = FormatoCantidadPrecio(Convert.ToString(info_item[0][20]).Trim(), 6, 4);
					movimientos += f120_factor_peso_empaque;

					f120_factor_volumen_empaque = FormatoCantidadPrecio("1", 6, 4);
					movimientos += f120_factor_volumen_empaque;

					movimientos += f120_ind_lista_precios_ext;

					movimientos += f121_ind_estado;

					movimientos += f121_fecha_inactivacion;

					movimientos += f121_fecha_creacion;

					f120_notas = Convert.ToString(info_item[0][9]).Trim().PadRight(255, ' ');//nuevo
					movimientos += f120_notas;

					movimientos += f120_ind_serial;
					//////////////////////////////////
					movimientos += f120_id_cfg_serial;

					movimientos += f120_ind_paquete;

					movimientos += f120_ind_exento;

					movimientos += f120_ind_generico;

					movimientos += f120_id_item_ext_generico;

					movimientos += f120_referencia_item_generico;

					movimientos += f121_id_ext1_detalle;

					movimientos += f121_id_ext2_detalle;

					nroReg++;
					//AddText(fs, movimientos);

					xml_item_adicion += $"<Linea>{movimientos}</Linea>{Environment.NewLine}";
				}
			}
			REG_CIERRE = nroReg.ToString().PadLeft(7, '0') + "99990001001";

			//AddText(fs, REG_CIERRE);

			xml_item_adicion += $@"<Linea>{REG_CIERRE}</Linea>
                                            </Datos>
                                           </Importar> ";
			//fs.Close();
			wsunoee.Timeout = 600000;
			DataSet ds_item_adicion = wsunoee.ImportarXML(xml_item_adicion, ref err);
			DataTable dt_item_adicion = ds_item_adicion.Tables[0];

			if (dt_item_adicion.Rows.Count > 0)
			{
				foreach (DataRow dr in dt_item_adicion.Rows)
					dt_resumen.Rows.Add("ConectorItemAdicion", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]);
			}
		}

		/// <summary>
		/// Crea el conector item cotización.
		/// </summary>
		/// <param name="nombreArchivo">Nombre y ubicación del archivo.</param>
		/// <param name="datos">Información para formar el archivo.</param>
		public void CrearConectorItemCotizacion(string numero_doc, string nombre_doc, string gln_prov, string gln_comp/*, string nombreArchivo*/, string nit, string sucursal, List<string> items)
		{
			////////////////////////VARIABLES COMUNES///////////////////////
			string movimientos = "";
			int nroReg = 2;

			string REG_INICIO = "000000100000001001";

			string F_NUMERO_REG = ""; //Numerico 7 - Numero consecutivo
			string F_TIPO_REG = ""; //Numerico 4 - VARIA EN CADA HOJA 212, 213, 214
			string F_SUBTIPO_REG = "00"; //Numerico 2 - Valor fijo = 00
			string F_VERSION_REG = "01"; //Numerico 2 - Version = 01
			string F_CIA = "001"; // Numerico 3 - compañía a la cual pertenece la informacion del registro
			string F_ACTUALIZA_REG = "1";

			string F212_ID_TERCERO = "";//CRUZAR CON UNOEE.
			string F212_ID_SUCURSAL = "001";
			string F212_ID_MONEDA = "";
			string F212_ID_ITEM = "0000000";
			string F212_REFERENCIA_ITEM = "".PadRight(20, ' ');//AQUI VA EL EAN 13 VIENE DE LOS ARCHIVOS.
			string F212_CODIGO_BARRAS_ITEM = "";
			string F212_ID_EXT1_DETALLE = "".PadRight(4, ' ');
			string F212_ID_EXT2_DETALLE = "".PadRight(4, ' ');
			string F212_FECHA_ACTIVACION = "";//??
			string F212_ID_UM = "";//UNIDAD DE MEDIDA.

			////////////////////////VARIABLES COTIZACIONES///////////////////////
			string F212_PRECIO = "";//15 enteros un punto cuatro decimales. VIENE DE LOS ARCHIVOS.
			string F212_TIEMPO_ENTREGA = ""; //VA DE 1 - 999. DE TRES DIGITOS.
			string F212_FECHA_HASTA = "";//FORMATO FECHA YYYYMMDD. DE OCHO CARACTERES.

			////////////////////////VARIABLES IMPUESTOS///////////////////////
			// string F213_ID_LLAVE_IMPUESTO = "";//??
			// string F213_VALOR_IMP = "";//VALOR IMPUESTO 15 ENTEROS UN PUNTO CUATRO DECIMALES.

			////////////////////////VARIABLES DESCUENTOS///////////////////////
			string F214_ORDEN = "";//VA de 1 - 9.
			string F214_CANTIDAD_HASTA = "";//15 ENTEROS UN PUNTO CUATRO DECIMALES.
			string F214_PORCENTAJE_DSCTO = "";//3 ENTEROS UN PUNTO CUATRO DECIMALES. APLICA SI ES %.
			string F214_VALOR_DSCTO = "";//15 ENTEROS UN PUNTO CUATRO DECIMALES. APLICA SI ES VALOR.

			string REG_CIERRE = "";

			/*FileStream fs = null;

			fs = File.Create(nombreArchivo);

			AddText(fs, REG_INICIO);*/

			WSUNOEE.WSUNOEE wsunoee = new WSUNOEE.WSUNOEE();
			int err = 1;
			string xml_item_cotizacion = $@"<?xml version=""1.0"" encoding=""utf-8""?>
							 <Importar>
								<NombreConexion>unoee_invercomer</NombreConexion>
								<IdCia>1</IdCia>
								<Usuario>ws_importar_mega</Usuario>
								<Clave>rLFgx)bL</Clave>
								<Datos>
									<Linea>{REG_INICIO}</Linea>";


			Datos datos = new Datos();
			foreach (string item in items)
			{
				List<List<object>> info_item = datos.ObtenerDatosItems(numero_doc, nombre_doc, gln_prov, gln_comp, item, 2);
				if (Convert.ToBoolean(info_item[0][16]) == true)
				{
					for (int i = 0; i < 2; i++)//SE PONE HASTA < 3 SI SE USAN LAS OTRAS DOS PAGINAS DEL CONECTOR.
					{
						if (i == 1 && (Convert.ToDecimal(info_item[0][12]) == 0 &&
										Convert.ToDecimal(info_item[0][19]) == 0 &&
										Convert.ToDecimal(info_item[0][23]) == 0))//PARA VERIFICAR SI i ESTA EN UNO, Y SI EL DESCUENTO ES CERO NO SE GENERA LA LINEA DE DESCUENTO.
						{
							continue;
						}
						movimientos = "";

						F_NUMERO_REG = nroReg.ToString().PadLeft(7, '0');
						movimientos += F_NUMERO_REG;

						F_TIPO_REG = "";
						switch (i)
						{
							case 0:
								F_TIPO_REG = "212".PadLeft(4, '0');
								break;
							//case 1:
							//    F_TIPO_REG = "213".PadLeft(4, '0');
							//    break;
							case 1:
								F_TIPO_REG = "214".PadLeft(4, '0');
								break;
						}
						movimientos += F_TIPO_REG;

						movimientos += F_SUBTIPO_REG;

						movimientos += F_VERSION_REG;

						movimientos += F_CIA;

						movimientos += F_ACTUALIZA_REG;

						F212_ID_TERCERO = nit.PadRight(15, ' ');
						movimientos += F212_ID_TERCERO;

						if (F212_ID_SUCURSAL.Trim().Equals(""))
						{
							F212_ID_SUCURSAL = "001";
						}
						else
						{
							F212_ID_SUCURSAL = sucursal.PadRight(3, ' ');
						}
						movimientos += F212_ID_SUCURSAL;

						F212_ID_MONEDA = Convert.ToString(info_item[0][8]).Trim().PadRight(3, ' ');
						movimientos += F212_ID_MONEDA;

						movimientos += F212_ID_ITEM;

						movimientos += F212_REFERENCIA_ITEM;

						F212_CODIGO_BARRAS_ITEM = Convert.ToString(info_item[0][7]).Trim().PadRight(20, ' ');
						movimientos += F212_CODIGO_BARRAS_ITEM;

						movimientos += F212_ID_EXT1_DETALLE;

						movimientos += F212_ID_EXT2_DETALLE;

						if (Convert.ToDateTime(info_item[0][17], CultureInfo.InvariantCulture).ToString("yyyyMMdd").Trim().Equals("19000101"))
						{
							F212_FECHA_ACTIVACION = "".PadRight(8, ' ');
						}
						else
						{
							F212_FECHA_ACTIVACION = Convert.ToDateTime(info_item[0][17], CultureInfo.InvariantCulture).ToString("yyyyMMdd").Trim();
						}

						movimientos += F212_FECHA_ACTIVACION;

						F212_ID_UM = Convert.ToString(info_item[0][10]).Trim().PadRight(4, ' ');
						movimientos += F212_ID_UM;

						switch (i)
						{
							case 0:
								F212_PRECIO = FormatoCantidadPrecio(Convert.ToString(info_item[0][9]).Trim(), 15, 4);//VALIDAR LA COLUMNA
								movimientos += F212_PRECIO;

								F212_TIEMPO_ENTREGA = Convert.ToString(info_item[0][11]).Trim().PadLeft(3, '0');
								movimientos += F212_TIEMPO_ENTREGA;
								if (Convert.ToDateTime(info_item[0][18], CultureInfo.InvariantCulture).ToString("yyyyMMdd").Trim().Equals("19000101"))
								{
									F212_FECHA_HASTA = "".Trim().PadRight(8, ' ');
								}
								else
								{
									F212_FECHA_HASTA = Convert.ToDateTime(info_item[0][18], CultureInfo.InvariantCulture).ToString("yyyyMMdd").Trim().PadRight(8, ' ');
								}
								movimientos += F212_FECHA_HASTA;
								break;
							//case 1:
							//    F213_ID_LLAVE_IMPUESTO = "0028"; //Convert.ToString(info_item[0][12]);//VALIDAR 
							//    movimientos += F213_ID_LLAVE_IMPUESTO;

							//    F213_VALOR_IMP = FormatoCantidadPrecio(Convert.ToString(info_item[0][13]), 15, 4);//VALIDAR ORIGEN
							//    movimientos += F213_VALOR_IMP;
							//    break;
							case 1:
								if (Convert.ToDecimal(info_item[0][12]) > 0)
								{
									F214_ORDEN = "1";//DE 1 - 9 AQUI VAN LOS DESCUENTOS QUE VENGAN EN LA NEGOCIACION
									movimientos += F214_ORDEN;

									F214_CANTIDAD_HASTA = FormatoCantidadPrecio(Convert.ToString("999"/*row.Cells[5].Value*/), 15, 4);//VALIDAR ORIGEN
									movimientos += F214_CANTIDAD_HASTA;

									F214_PORCENTAJE_DSCTO = FormatoCantidadPrecio(Convert.ToString(info_item[0][12]).Trim(), 3, 4);//VALIDAR ORIGEN, SI ES PORCENTAJE
									movimientos += F214_PORCENTAJE_DSCTO;

									F214_VALOR_DSCTO = FormatoCantidadPrecio("0", 15, 4);  //CantidadPrecio(Convert.ToString(row.Cells[5].Value), 15, 4);//VALIDAR ORIGEN, SI ES VALOR
									movimientos += F214_VALOR_DSCTO;

								}
								if (Convert.ToDecimal(info_item[0][19]) > 0)
								{
									F214_ORDEN = "2";
									movimientos += F214_ORDEN;

									F214_CANTIDAD_HASTA = FormatoCantidadPrecio(Convert.ToString("999"), 15, 4);
									movimientos += F214_CANTIDAD_HASTA;

									F214_PORCENTAJE_DSCTO = FormatoCantidadPrecio(Convert.ToString(info_item[0][19]).Trim(), 3, 4);
									movimientos += F214_PORCENTAJE_DSCTO;

									F214_VALOR_DSCTO = FormatoCantidadPrecio("0", 15, 4);
									movimientos += F214_VALOR_DSCTO;

								}
								if (Convert.ToDecimal(info_item[0][23]) > 0)
								{
									F214_ORDEN = "3";
									movimientos += F214_ORDEN;

									F214_CANTIDAD_HASTA = FormatoCantidadPrecio(Convert.ToString("999"), 15, 4);
									movimientos += F214_CANTIDAD_HASTA;

									F214_PORCENTAJE_DSCTO = FormatoCantidadPrecio(Convert.ToString(info_item[0][23]).Trim(), 3, 4);
									movimientos += F214_PORCENTAJE_DSCTO;

									F214_VALOR_DSCTO = FormatoCantidadPrecio("0", 15, 4);
									movimientos += F214_VALOR_DSCTO;

								}
								break;
						}
						nroReg++;
						//AddText(fs, movimientos);

						xml_item_cotizacion += $"<Linea>{movimientos}</Linea>{Environment.NewLine}";
					}
				}
			}

			REG_CIERRE = nroReg.ToString().PadLeft(7, '0') + "99990001001";

			//AddText(fs, REG_CIERRE);

			xml_item_cotizacion += $@"<Linea>{REG_CIERRE}</Linea>
                                </Datos>
                               </Importar> ";

			//fs.Close();
			wsunoee.Timeout = 600000;
			DataSet ds_item_cotizacion = wsunoee.ImportarXML(xml_item_cotizacion, ref err);
			DataTable dt_item_cotizacion = ds_item_cotizacion.Tables[0];

			if (dt_item_cotizacion.Rows.Count > 0)
			{
				foreach (DataRow dr in dt_item_cotizacion.Rows)
					dt_resumen.Rows.Add("ConectorItemCotizacion", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]);
			}
		}


		/// <summary>
		/// Crea el conector item cotización.
		/// </summary>
		/// <param name="nombreArchivo">Nombre y ubicación del archivo.</param>
		/// <param name="datos">Información para formar el archivo.</param>
		public void CrearConectorItemCriterio(string numero_doc, string nombre_doc, string gln_prov, string gln_comp/*, string nombreArchivo*/, List<string> items)
		{
			////////////////////////VARIABLES///////////////////////
			string movimientos = "";
			int nroReg = 2;

			string REG_INICIO = "000000100000001001";

			string F_NUMERO_REG = ""; //Numerico 7 - Numero consecutivo
			string F_TIPO_REG = "0125"; //Numerico 4 
			string F_SUBTIPO_REG = "00"; //Numerico 2 - Valor fijo = 00
			string F_VERSION_REG = "01"; //Numerico 2 - Version = 01
			string F_CIA = "001"; // Numerico 3 - compañía a la cual pertenece la informacion del registro
			string F_ACTUALIZA_REG = "1";

			string f120_id = "0000000";
			string f120_referencia = "";//GTIN de archivo General.
			string f125_id_plan = "";//Viene de la tabla Categorias en la BD Pricat.
			string f125_id_criterio_mayor = "";///Viene de la tabla Categorias en la BD Pricat.

			string REG_CIERRE = "";

			/*FileStream fs = null;

			fs = File.Create(nombreArchivo);

			AddText(fs, REG_INICIO);*/

			WSUNOEE.WSUNOEE wsunoee = new WSUNOEE.WSUNOEE();
			int err = 1;
			string xml_item_criterio = $@"<?xml version=""1.0"" encoding=""utf-8""?>
							 <Importar>
								<NombreConexion>unoee_invercomer</NombreConexion>
								<IdCia>1</IdCia>
								<Usuario>ws_importar_mega</Usuario>
								<Clave>rLFgx)bL</Clave>
								<Datos>
									<Linea>{REG_INICIO}</Linea>";

			Datos datos = new Datos();
			foreach (string item in items)
			{
				List<List<object>> info_item = datos.ObtenerDatosItems(numero_doc, nombre_doc, gln_prov, gln_comp, item, 4);

				foreach (List<object> lista in info_item[0])
				{

					movimientos = "";

					F_NUMERO_REG = nroReg.ToString().PadLeft(7, '0');
					movimientos += F_NUMERO_REG;

					movimientos += F_TIPO_REG;

					movimientos += F_SUBTIPO_REG;

					movimientos += F_VERSION_REG;

					movimientos += F_CIA;

					movimientos += F_ACTUALIZA_REG;

					movimientos += f120_id;

					f120_referencia = Convert.ToString(lista[5]).Trim().PadRight(20, ' ');
					movimientos += f120_referencia;

					f125_id_plan = Convert.ToString(lista[6]).Trim().PadRight(3, ' ');
					movimientos += f125_id_plan;

					f125_id_criterio_mayor = Convert.ToString(lista[8]).Trim().PadRight(4, ' ');
					movimientos += f125_id_criterio_mayor;

					nroReg++;
					//AddText(fs, movimientos);

					xml_item_criterio += $"<Linea>{movimientos}</Linea>{Environment.NewLine}";
				}
			}

			REG_CIERRE = nroReg.ToString().PadLeft(7, '0') + "99990001001";

			//AddText(fs, REG_CIERRE);

			//fs.Close();

			xml_item_criterio += $@"<Linea>{REG_CIERRE}</Linea>
                                </Datos>
                               </Importar> ";
			wsunoee.Timeout = 600000;
			DataSet ds_item_criterio = wsunoee.ImportarXML(xml_item_criterio, ref err);
			DataTable dt_item_criterio = ds_item_criterio.Tables[0];

			if (dt_item_criterio.Rows.Count > 0)
			{
				foreach (DataRow dr in dt_item_criterio.Rows)
					dt_resumen.Rows.Add("ConectorItemCriterio", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]);
			}
		}

		/// <summary>
		/// Conector para la descripción tecnica del item
		/// </summary>
		/// <param name="nombreArchivo"></param>
		/// <param name="datos"></param>
		public void CrearConectorItemDescripcionTecnica(string numero_doc, string nombre_doc, string gln_prov, string gln_comp/*, string nombreArchivo*/, List<string> items)
		{
			////////////////////////VARIABLES///////////////////////
			string movimientos = "";
			int nroReg = 2;

			string REG_INICIO = "000000100000001001";

			string F_NUMERO_REG = ""; //Numerico 7 - Numero consecutivo
			string F_TIPO_REG = "0123"; //Numerico 4 - Valor fijo = 120
			string F_SUBTIPO_REG = "00"; //Numerico 2 - Valor fijo = 00
			string F_VERSION_REG = "02"; //Numerico 2 - Version = 01
			string F_CIA = "001"; // Numerico 3 - compañía a la cual pertenece la informacion del registro
			string F_ACTUALIZA_REG = "1";
			string f120_id = "".PadLeft(7, '0');

			string f120_referencia = "";
			string f104_id_descripcion_tecnica = "DTI";
			string f104_id = "";
			string f123_dato = "";

			string REG_CIERRE = "";

			/*FileStream fs = null;

			fs = File.Create(nombreArchivo);

			AddText(fs, REG_INICIO);*/

			WSUNOEE.WSUNOEE wsunoee = new WSUNOEE.WSUNOEE();
			int err = 1;
			string xml_item_desc_tec = $@"<?xml version=""1.0"" encoding=""utf-8""?>
							 <Importar>
								<NombreConexion>unoee_invercomer</NombreConexion>
								<IdCia>1</IdCia>
								<Usuario>ws_importar_mega</Usuario>
								<Clave>rLFgx)bL</Clave>
								<Datos>
									<Linea>{REG_INICIO}</Linea>";

			Datos datos = new Datos();
			foreach (string item in items)
			{
				List<List<object>> info_item = datos.ObtenerDatosItems(numero_doc, nombre_doc, gln_prov, gln_comp, item, 3);
				if (Convert.ToBoolean(info_item[0][12]).Equals(true))
				{
					for (int i = 0; i < 6; i++)
					{
						movimientos = "";

						F_NUMERO_REG = nroReg.ToString().PadLeft(7, '0');
						movimientos += F_NUMERO_REG;

						movimientos += F_TIPO_REG;

						movimientos += F_SUBTIPO_REG;

						movimientos += F_VERSION_REG;

						movimientos += F_CIA;

						movimientos += F_ACTUALIZA_REG;

						movimientos += f120_id;

						f120_referencia = Convert.ToString(info_item[0][5]).PadRight(50, ' ');
						movimientos += f120_referencia;
						switch (i)
						{
							case 0:
								movimientos += f104_id_descripcion_tecnica;
								f104_id = "Alto(cm)".PadRight(50, ' ');
								movimientos += f104_id;
								f123_dato = Convert.ToString(info_item[0][6]).Trim().PadRight(60, ' ');
								movimientos += f123_dato;
								break;
							case 1:
								movimientos += f104_id_descripcion_tecnica;
								f104_id = "Ancho(cm)".PadRight(50, ' ');
								movimientos += f104_id;
								f123_dato = Convert.ToString(info_item[0][7]).Trim().PadRight(60, ' ');
								movimientos += f123_dato;
								break;
							case 2:
								movimientos += f104_id_descripcion_tecnica;
								f104_id = "Profundo(cm)".PadRight(50, ' ');
								movimientos += f104_id;
								f123_dato = Convert.ToString(info_item[0][8]).Trim().PadRight(60, ' ');
								movimientos += f123_dato;
								break;

							case 3:
								movimientos += f104_id_descripcion_tecnica;
								f104_id = "Alto_UM_Empaque(cm)".PadRight(50, ' ');
								movimientos += f104_id;
								f123_dato = Convert.ToString(info_item[0][9]).Trim().PadRight(60, ' ');
								movimientos += f123_dato;
								break;
							case 4:
								movimientos += f104_id_descripcion_tecnica;
								f104_id = "Ancho_UM_Empaque(cm)".PadRight(50, ' ');
								movimientos += f104_id;
								f123_dato = Convert.ToString(info_item[0][10]).Trim().PadRight(60, ' ');
								movimientos += f123_dato;
								break;
							case 5:
								movimientos += f104_id_descripcion_tecnica;
								f104_id = "Profundo_UM_Empaque(cm)".PadRight(50, ' ');
								movimientos += f104_id;
								f123_dato = Convert.ToString(info_item[0][11]).Trim().PadRight(60, ' ');
								movimientos += f123_dato;
								break;
						}
						nroReg++;
						//AddText(fs, movimientos);
						xml_item_desc_tec += $"<Linea>{movimientos}</Linea>{Environment.NewLine}";
					}
				}
			}
			REG_CIERRE = nroReg.ToString().PadLeft(7, '0') + "99990001001";

			//AddText(fs, REG_CIERRE);

			xml_item_desc_tec += $@"<Linea>{REG_CIERRE}</Linea>
                                </Datos>
                               </Importar> ";

			//fs.Close();
			wsunoee.Timeout = 600000;
			DataSet ds_item_desc_tec = wsunoee.ImportarXML(xml_item_desc_tec, ref err);
			DataTable dt_item_desc_tec = ds_item_desc_tec.Tables[0];

			if (dt_item_desc_tec.Rows.Count > 0)
			{
				foreach (DataRow dr in dt_item_desc_tec.Rows)
					dt_resumen.Rows.Add("ItemDescripcionTecnica", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]);
			}
		}

		public void CrearConectorItemParametrosPlaneacion(string numero_doc, string nombre_doc, string gln_prov, string gln_comp/*, string nombreArchivo*/, List<string> items)
		{
			////////////////////////VARIABLES///////////////////////
			string movimientos = "";
			int nroReg = 2;

			string REG_INICIO = "000000100000001001";

			string F_NUMERO_REG = ""; //Numerico 7 - Numero consecutivo
			string F_TIPO_REG = "0132"; //Numerico 4 
			string F_SUBTIPO_REG = "00"; //Numerico 2 - Valor fijo = 00
			string F_VERSION_REG = "05"; //Numerico 2 - Version = 01
			string F_CIA = "001"; // Numerico 3 - compañía a la cual pertenece la informacion del registro
			string F_ACTUALIZA_REG = "0";
			string F_CAMPO = "".PadRight(35, ' ');
			string f132_id_instalacion = "";
			string f132_abc_rotacion_veces = "";
			string f132_abc_rotacion_costo = "";
			string f132_categoria_ciclo_conteo = "".PadRight(3, ' ');
			string f132_id_um_venta_suge = "";
			string f132_periodo_cubrimiento = "";
			string f132_tiempo_reposicion = "";
			string f132_tiempo_seguridad = "";
			string f132_mf_id_planificador = "".PadRight(4, ' ');
			string f132_mf_id_comprador = "";
			string f132_mf_dias_horiz_planea = "";
			string f132_mf_stock_segur_estatico = "";
			string f132_mf_dias_horiz_stock_min = "";
			string f132_mf_dias_stock_min = "";
			string f132_mf_tiempo_repo_fijo = "";
			string f132_mf_tasa_produccion_dia = "0000000000.0000";
			string f132_mf_ind_mps = "0";
			string f132_mf_dias_corte_demanda = "000";
			string f132_mf_porc_rendimiento = "100.000";
			string f132_mf_ind_demanda_1 = "4";
			string f132_mf_ind_demanda_2 = "0";
			string f132_mf_ind_tipo_orden = "0";
			string f132_mf_ind_politica_orden = "";
			string f132_mf_tamano_lote = "";
			string f132_mf_cant_incremental_lote = "0000000000.0000";
			string f132_mf_porc_minimo_orden_plan = "000.000";
			string f132_mf_dias_periodos_fijo = "000";
			string f132_mf_id_formulador = "".PadRight(4, ' ');
			string f132_mf_revision_formula = "".PadRight(20, ' ');
			string f132_mf_id_ruta = "".PadRight(20, ' ');
			string f132_mf_id_bodega_asigna = "";
			string f132_mf_ind_asigna_instalacion = "0";
			string f132_mf_ind_generar_orden_prod = "0";
			string f132_mf_ind_item_critico = "0";
			string f132_mf_id_tercero_prov_1 = "";
			string f132_mf_id_sucursal_prov_1 = "";
			string f132_mf_id_tercero_prov_2 = "".PadRight(15, ' ');
			string f132_mf_id_sucursal_prov_2 = "".PadRight(3, ' ');
			string f132_porc_min_margen = "0000.00";
			string f132_porc_max_margen = "0000.00";
			string f120_id = "0000000";
			string f120_referencia = "";//GTIN de archivo General.
			string f121_id_ext1_detalle = "".PadRight(20, ' ');
			string f121_id_ext2_detalle = "".PadRight(20, ' ');
			string f132_mf_tasa_produccion_hora = "0000000000.000000";
			string f132_porcentaje_exceso_compra = "0000.00";
			string f132_mf_ind_genera_ord_pln = "";

			string REG_CIERRE = "";

			/*FileStream fs = null;

			fs = File.Create(nombreArchivo);

			AddText(fs, REG_INICIO);*/

			WSUNOEE.WSUNOEE wsunoee = new WSUNOEE.WSUNOEE();
			int err = 1;
			string xml_item_params = $@"<?xml version=""1.0"" encoding=""utf-8""?>
							 <Importar>
								<NombreConexion>unoee_invercomer</NombreConexion>
								<IdCia>1</IdCia>
								<Usuario>ws_importar_mega</Usuario>
								<Clave>rLFgx)bL</Clave>
								<Datos>
									<Linea>{REG_INICIO}</Linea>";

			Datos datos = new Datos();
			foreach (string item in items)
			{
				List<List<object>> info_item = datos.ObtenerDatosItems(numero_doc, nombre_doc, gln_prov, gln_comp, item, 5);

				foreach (List<object> lista in info_item[0])
				{
					movimientos = "";

					F_NUMERO_REG = nroReg.ToString().PadLeft(7, '0');
					movimientos += F_NUMERO_REG;

					movimientos += F_TIPO_REG;

					movimientos += F_SUBTIPO_REG;

					movimientos += F_VERSION_REG;

					movimientos += F_CIA;

					movimientos += F_ACTUALIZA_REG;

					movimientos += F_CAMPO;

					f132_id_instalacion = Convert.ToString(lista[6]).Trim().PadRight(3, ' ');
					movimientos += f132_id_instalacion;

					f132_abc_rotacion_veces = Convert.ToString(lista[8]).Trim();
					movimientos += f132_abc_rotacion_veces;

					f132_abc_rotacion_costo = Convert.ToString(lista[9]).Trim();
					movimientos += f132_abc_rotacion_costo;

					movimientos += f132_categoria_ciclo_conteo;

					f132_id_um_venta_suge = Convert.ToString(lista[10]).Trim().PadRight(4, ' ');
					movimientos += f132_id_um_venta_suge;

					f132_periodo_cubrimiento = Convert.ToString(lista[11]).Trim().PadLeft(3, '0');
					movimientos += f132_periodo_cubrimiento;

					f132_tiempo_reposicion = Convert.ToString(lista[12]).Trim().PadLeft(3, '0');
					movimientos += f132_tiempo_reposicion;

					f132_tiempo_seguridad = Convert.ToString(lista[13]).Trim().PadLeft(3, '0');
					movimientos += f132_tiempo_seguridad;

					movimientos += f132_mf_id_planificador;

					f132_mf_id_comprador = Convert.ToString(lista[14]).Trim().PadRight(4, ' ');
					movimientos += f132_mf_id_comprador;

					f132_mf_dias_horiz_planea = Convert.ToString(lista[15]).Trim().PadLeft(3, '0');
					movimientos += f132_mf_dias_horiz_planea;

					f132_mf_stock_segur_estatico = FormatoCantidadPrecio(Convert.ToString(lista[16]).Trim(), 10, 4);
					movimientos += f132_mf_stock_segur_estatico;

					f132_mf_dias_horiz_stock_min = Convert.ToString(lista[17]).Trim().PadLeft(3, '0');
					movimientos += f132_mf_dias_horiz_stock_min;

					f132_mf_dias_stock_min = Convert.ToString(lista[18]).Trim().PadLeft(3, '0');
					movimientos += f132_mf_dias_stock_min;

					f132_mf_tiempo_repo_fijo = Convert.ToString(lista[19]).Trim().PadLeft(3, '0');
					movimientos += f132_mf_tiempo_repo_fijo;

					movimientos += f132_mf_tasa_produccion_dia;

					movimientos += f132_mf_ind_mps;

					movimientos += f132_mf_dias_corte_demanda;

					movimientos += f132_mf_porc_rendimiento;

					movimientos += f132_mf_ind_demanda_1;

					movimientos += f132_mf_ind_demanda_2;

					movimientos += f132_mf_ind_tipo_orden;

					f132_mf_ind_politica_orden = Convert.ToString(lista[20]).Trim();
					movimientos += f132_mf_ind_politica_orden;

					f132_mf_tamano_lote = FormatoCantidadPrecio(Convert.ToString(lista[21]).Trim(), 10, 4);
					movimientos += f132_mf_tamano_lote;

					movimientos += f132_mf_cant_incremental_lote;

					movimientos += f132_mf_porc_minimo_orden_plan;

					movimientos += f132_mf_dias_periodos_fijo;

					movimientos += f132_mf_id_formulador;

					movimientos += f132_mf_revision_formula;

					movimientos += f132_mf_id_ruta;

					f132_mf_id_bodega_asigna = Convert.ToString(lista[7]).Trim().PadRight(5, ' ');
					movimientos += f132_mf_id_bodega_asigna;

					movimientos += f132_mf_ind_asigna_instalacion;

					movimientos += f132_mf_ind_generar_orden_prod;

					movimientos += f132_mf_ind_item_critico;

					f132_mf_id_tercero_prov_1 = Convert.ToString(lista[22]).Trim().PadRight(15, ' ');
					movimientos += f132_mf_id_tercero_prov_1;

					f132_mf_id_sucursal_prov_1 = Convert.ToString(lista[24]).Trim().PadRight(3, ' ');
					movimientos += f132_mf_id_sucursal_prov_1;

					movimientos += f132_mf_id_tercero_prov_2;

					movimientos += f132_mf_id_sucursal_prov_2;

					movimientos += f132_porc_min_margen;

					movimientos += f132_porc_max_margen;

					movimientos += f120_id;

					f120_referencia = Convert.ToString(lista[5]).Trim().PadRight(50, ' ');
					movimientos += f120_referencia;

					movimientos += f121_id_ext1_detalle;

					movimientos += f121_id_ext2_detalle;

					movimientos += f132_mf_tasa_produccion_hora;

					movimientos += f132_porcentaje_exceso_compra;

					f132_mf_ind_genera_ord_pln = Convert.ToInt32(lista[25]).ToString();
					movimientos += f132_mf_ind_genera_ord_pln;

					nroReg++;
					//AddText(fs, movimientos);

					xml_item_params += $"<Linea>{movimientos}</Linea>{Environment.NewLine}";
				}
			}

			REG_CIERRE = nroReg.ToString().PadLeft(7, '0') + "99990001001";

			//AddText(fs, REG_CIERRE);

			xml_item_params += $@"<Linea>{REG_CIERRE}</Linea>
                                </Datos>
                               </Importar> ";
			//fs.Close();
			wsunoee.Timeout = 600000;
			DataSet ds_item_params = wsunoee.ImportarXML(xml_item_params, ref err);
			DataTable dt_item_params = ds_item_params.Tables[0];

			if (dt_item_params.Rows.Count > 0)
			{
				foreach (DataRow dr in dt_item_params.Rows)
					dt_resumen.Rows.Add("ItemParametrosPlaneacion", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]);
			}
		}

		public void CrearConectorItemPreciosVenta(string numero_doc, string nombre_doc, string gln_prov, string gln_comp/*, string nombreArchivo*/, List<string> items)
		{
			////////////////////////VARIABLES///////////////////////
			string movimientos = "";
			int nroReg = 2;

			string REG_INICIO = "000000100000001001";

			string F_NUMERO_REG = ""; //Numerico 7 - Numero consecutivo
			string F_TIPO_REG = "0126"; //Numerico 4 
			string F_SUBTIPO_REG = "00"; //Numerico 2 - Valor fijo = 00
			string F_VERSION_REG = "02"; //Numerico 2 - Version = 01
			string F_CIA = "001"; // Numerico 3 - compañía a la cual pertenece la informacion del registro
			string F_ACTUALIZA_REG = "1";

			string f126_id_lista_precio = "";
			string f126_id_item = "0000000";
			string f126_referencia_item = "".PadRight(50, ' ');//GTIN de archivo General.
			string F126_CODIGO_BARRAS_ITEM = "";
			string f126_id_ext1_detalle = "".PadRight(20, ' ');
			string f126_id_ext2_detalle = "".PadRight(20, ' ');
			string f126_fecha_activacion = "";
			string f126_fecha_inactivacion = "";
			string f126_id_promo_dscto = "00000000";
			string f126_id_unidad_medida = "";
			string f126_precio = "";
			string f126_precio_minimo = "000000000000000.0000";
			string f126_precio_maximo = "000000000000000.0000";
			string f126_precio_sugerido = "000000000000000.0000";
			string REG_CIERRE = "";

			/*FileStream fs = null;

			fs = File.Create(nombreArchivo);

			AddText(fs, REG_INICIO);*/

			WSUNOEE.WSUNOEE wsunoee = new WSUNOEE.WSUNOEE();
			int err = 1;
			string xml_item_precio_vta = $@"<?xml version=""1.0"" encoding=""utf-8""?>
							 <Importar>
								<NombreConexion>unoee_invercomer</NombreConexion>
								<IdCia>1</IdCia>
								<Usuario>ws_importar_mega</Usuario>
								<Clave>rLFgx)bL</Clave>
								<Datos>
									<Linea>{REG_INICIO}</Linea>" + Environment.NewLine;


			Datos datos = new Datos();
			foreach (string item in items)
			{
				List<List<object>> info_item = datos.ObtenerDatosItems(numero_doc, nombre_doc, gln_prov, gln_comp, item, 6);

				foreach (List<object> lista in info_item[0])
				{
					movimientos = "";

					F_NUMERO_REG = nroReg.ToString().PadLeft(7, '0');
					movimientos += F_NUMERO_REG;

					movimientos += F_TIPO_REG;

					movimientos += F_SUBTIPO_REG;

					movimientos += F_VERSION_REG;

					movimientos += F_CIA;

					movimientos += F_ACTUALIZA_REG;

					f126_id_lista_precio = Convert.ToString(lista[8]).Trim().PadRight(3, ' ');
					movimientos += f126_id_lista_precio;

					movimientos += f126_id_item;

					movimientos += f126_referencia_item;

					F126_CODIGO_BARRAS_ITEM = Convert.ToString(lista[5]).Trim().PadRight(20, ' ');
					movimientos += F126_CODIGO_BARRAS_ITEM;

					movimientos += f126_id_ext1_detalle;

					movimientos += f126_id_ext2_detalle;

					f126_fecha_activacion = Convert.ToDateTime(lista[10], CultureInfo.InvariantCulture).ToString("yyyyMMdd").Trim();
					movimientos += f126_fecha_activacion;

					if (Convert.ToDateTime(lista[11], CultureInfo.InvariantCulture).ToString("yyyyMMdd").Trim().Equals("19000101"))
					{
						f126_fecha_inactivacion = "".PadRight(8, ' ');
					}
					else
					{
						f126_fecha_inactivacion = Convert.ToDateTime(lista[11], CultureInfo.InvariantCulture).ToString("yyyyMMdd").Trim().PadRight(8, ' ');
					}

					movimientos += f126_fecha_inactivacion;

					movimientos += f126_id_promo_dscto;

					f126_id_unidad_medida = Convert.ToString(lista[6]).Trim().PadRight(4, ' ');
					movimientos += f126_id_unidad_medida;

					f126_precio = FormatoCantidadPrecio(Convert.ToString(lista[9]).Trim(), 15, 4);
					movimientos += f126_precio;

					movimientos += f126_precio_minimo;

					movimientos += f126_precio_maximo;

					movimientos += f126_precio_sugerido;

					nroReg++;
					//AddText(fs, movimientos);

					xml_item_precio_vta += $"<Linea>{movimientos}</Linea>{Environment.NewLine}";
				}
			}

			REG_CIERRE = nroReg.ToString().PadLeft(7, '0') + "99990001001";

			//AddText(fs, REG_CIERRE);

			xml_item_precio_vta += $@"<Linea>{REG_CIERRE}</Linea>
                                </Datos>
                               </Importar> ";
			//fs.Close();
			wsunoee.Timeout = 600000;
			DataSet ds_item_precio_vta = wsunoee.ImportarXML(xml_item_precio_vta, ref err);
			DataTable dt_item_precio_vta = ds_item_precio_vta.Tables[0];

			if (dt_item_precio_vta.Rows.Count > 0)
			{
				foreach (DataRow dr in dt_item_precio_vta.Rows)
					dt_resumen.Rows.Add("ItemPreciosVenta", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]);
			}
		}

		public List<string> ObtenerListadoPortafolios(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador)
		{
			string SQL = "SELECT DISTINCT " +
							"po_id_portafolio, " +
							"po_descripcion, " +
							"po_nota " +
						"FROM " +
							"Documentos " +
							"INNER JOIN Portafolio ON do_numero_doc=po_numero_doc AND do_nombre_doc=po_nombre_doc " +
												   "AND do_gln_proveedor=po_gln_proveedor AND do_gln_comprador=po_gln_comprador " +
						"WHERE " +
							"do_numero_doc=@NRO_DOC " +
							"AND do_nombre_doc=@NOMB_DOC " +
							"AND do_gln_proveedor=@GLN_PROV " +
							"AND do_gln_comprador=@GLN_COMP";
			List<string> listado = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@NRO_DOC", numero_doc);
				cmd.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
				cmd.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
				cmd.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
				SqlDataReader dr = cmd.ExecuteReader();

				if (dr.HasRows)
				{
					listado = new List<string>();
					while (dr.Read())
					{
						listado.Add(dr.GetString(0) + "-" + dr.GetString(1) + "-" + dr.GetString(2));
					}
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de portafolios: " + ex.Message);
			}
			return listado;
		}

		/// <summary>
		/// Obtiene la secuencia actual de un portafolio.
		/// </summary>
		/// <param name="portafolio">Id del portafolio.</param>
		/// <returns>Devuelve la secuencia.</returns>
		private int ObtenerSecuenciaPortafolio(string portafolio)
		{
			string SQL = "select " +
							"max(f137_secuencia) f137_secuencia " +
						 "from " +
							"t137_mc_portafolio_items " +
						 "where " +
							"f137_id_portafolio=@ID and " +
							"f137_id_cia=1";
			int secuencia = -1;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@ID", portafolio);
				object s = cmd.ExecuteScalar();
				if (!Convert.IsDBNull(s))
				{
					secuencia = Convert.ToInt32(s);
				}
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener la secuencia del portafolio: " + ex.Message);
			}
			return secuencia;
		}

		public void CrearConectorItemPortafolios(string numero_doc, string nombre_doc, string gln_prov, string gln_com/*, string nombreArchivo*/, List<string> portafolios, string proveedor)
		{
			string año = DateTime.Now.Year.ToString();
			string mes = DateTime.Now.Month.ToString("00");
			string dia = DateTime.Now.Day.ToString("00");
			string hora = DateTime.Now.Hour.ToString("00");
			string minuto = DateTime.Now.Minute.ToString("00");
			string segundo = DateTime.Now.Second.ToString("00");
			string fechahora = año + mes + dia + hora + minuto + segundo;
			////////////////////////ENCABEZADO///////////////////////

			string REG_INICIO = "000000100000001001";

			string encabezado = "";
			const string E_F_NUMERO_REG = "0000002"; //Numerico 7 - Numero consecutivo
			string E_F_TIPO_REG = "0136"; //Numerico 4 - Valor fijo = 440
			string E_F_SUBTIPO_REG = "00"; //Numerico 2 - Valor fijo = 00
			string E_F_VERSION_REG = "01"; //Numerico 2 - Version = 01
			string E_F_CIA = "001"; // Numerico 3 
			string E_F_ACTUALIZA_REG = "1";
			string E_f136_id = "";
			string E_f136_descripcion = "";
			string E_f136_notas = "";

			///////////////////////DETALLE////////////////////////////
			string movimientos = "";
			int nroReg = 3;
			string M_F_NUMERO_REG = ""; //Numerico 7 - Numero consecutivo
			string M_F_TIPO_REG = "0137"; //Numerico 4 
			string M_F_SUBTIPO_REG = "00"; //Numerico 2 - Valor fijo = 00
			string M_F_VERSION_REG = "01"; //Numerico 2 - Version = 01
			string M_F_CIA = "001"; // Numerico 3 - compañía a la cual pertenece la informacion del registro
			string M_f137_id_portafolio = "";
			string M_f137_id_item = "0000000";
			string M_f137_referencia_item = "".PadRight(50, ' ');
			string M_f137_codigo_barras = "";
			string M_f137_id_ext1_detalle = "".PadRight(20, ' ');
			string M_f137_id_ext2_detalle = "".PadRight(20, ' ');
			string M_f137_secuencia = "";
			string M_f137_id_paquete = "".PadRight(20, ' ');

			string REG_CIERRE = "";

			foreach (string item in portafolios)
			{
				Datos datos = new Datos();
				List<List<object>> info_item = datos.ObtenerDatosItems(numero_doc, nombre_doc, gln_prov, gln_com, item.Split('-')[0], 8);
				nroReg = 3;
				encabezado = "";
				if (info_item[0].Count > 0)
				{
					encabezado += E_F_NUMERO_REG;
					encabezado += E_F_TIPO_REG;
					encabezado += E_F_SUBTIPO_REG;
					encabezado += E_F_VERSION_REG;
					encabezado += E_F_CIA;
					encabezado += E_F_ACTUALIZA_REG;

					E_f136_id = item.Split('-')[0].Trim().PadRight(10, ' ');
					encabezado += E_f136_id;

					int sec = ObtenerSecuenciaPortafolio(E_f136_id.Trim());

					/*FileStream fs = null;
					fs = File.Create(nombreArchivo + "8_AD_PORTAFOLIO_" + E_f136_id.Trim() + "_" + proveedor + "_" + fechahora + ".TXT");*/

					//AddText(fs, REG_INICIO);

					WSUNOEE.WSUNOEE wsunoee = new WSUNOEE.WSUNOEE();
					int err = 1;
					string xml_item_portafolio = $@"<?xml version=""1.0"" encoding=""utf-8""?>
							 <Importar>
								<NombreConexion>unoee_invercomer</NombreConexion>
								<IdCia>1</IdCia>
								<Usuario>ws_importar_mega</Usuario>
								<Clave>rLFgx)bL</Clave>
								<Datos>
									<Linea>{REG_INICIO}</Linea>";

					E_f136_descripcion = item.Split('-')[1].Trim().PadRight(40, ' ');
					encabezado += E_f136_descripcion;

					E_f136_notas = item.Split('-')[2].Trim().PadRight(255, ' ');
					encabezado += E_f136_notas;

					//AddText(fs, encabezado);

					xml_item_portafolio += $"<Linea>{encabezado}</Linea>{Environment.NewLine}";

					foreach (List<object> portafolio in info_item[0])
					{
						movimientos = "";

						M_F_NUMERO_REG = nroReg.ToString().PadLeft(7, '0');
						movimientos += M_F_NUMERO_REG;

						movimientos += M_F_TIPO_REG;

						movimientos += M_F_SUBTIPO_REG;

						movimientos += M_F_VERSION_REG;

						movimientos += M_F_CIA;

						M_f137_id_portafolio = Convert.ToString(portafolio[1]).Trim().PadRight(10, ' ');
						movimientos += M_f137_id_portafolio;

						movimientos += M_f137_id_item;

						movimientos += M_f137_referencia_item;

						M_f137_codigo_barras = Convert.ToString(portafolio[0]).Trim().PadRight(20, ' ');
						movimientos += M_f137_codigo_barras;

						movimientos += M_f137_id_ext1_detalle;

						movimientos += M_f137_id_ext2_detalle;

						sec++;
						M_f137_secuencia = sec.ToString().Trim().PadLeft(6, '0');
						movimientos += M_f137_secuencia;

						movimientos += M_f137_id_paquete;

						nroReg++;

						//AddText(fs, movimientos);
						xml_item_portafolio += $"<Linea>{movimientos}</Linea>{Environment.NewLine}";
					}

					REG_CIERRE = nroReg.ToString().PadLeft(7, '0') + "99990001001";

					//AddText(fs, REG_CIERRE);

					xml_item_portafolio += $@"<Linea>{REG_CIERRE}</Linea>
                                </Datos>
                               </Importar> ";

					//fs.Close();
					wsunoee.Timeout = 600000;
					DataSet ds_item_portafolio = wsunoee.ImportarXML(xml_item_portafolio, ref err);
					DataTable dt_item_portafolio = ds_item_portafolio.Tables[0];

					if (dt_item_portafolio.Rows.Count > 0)
					{
						foreach (DataRow dr in dt_item_portafolio.Rows)
							dt_resumen.Rows.Add("ItemPortafolios", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]);
					}
				}
			}
		}

		public void CrearConectorItemCodigoBarras(string numero_doc, string nombre_doc, string gln_prov, string gln_comp/*, string nombreArchivo*/, List<string> items)
		{
			////////////////////////VARIABLES///////////////////////
			string movimientos = "";
			int nroReg = 2;

			string REG_INICIO = "000000100000001001";

			string F_NUMERO_REG = ""; //Numerico 7 - Numero consecutivo
			string F_TIPO_REG = "0131"; //Numerico 4 
			string F_SUBTIPO_REG = "00"; //Numerico 2 - Valor fijo = 00
			string F_VERSION_REG = "05"; //Numerico 2 - Version = 01
			string F_CIA = "001"; // Numerico 3 - compañía a la cual pertenece la informacion del registro
			string F_ACTUALIZA_REG = "0";
			string f131_codigo_barras = "";//buscar barra caja
			string f131_id_item = "0000000";
			string f131_referencia_item = "";
			string f131_id_ext1_detalle = "".PadRight(20, ' ');
			string f131_id_ext2_detalle = "".PadRight(20, ' ');
			string f131_id_bodega = "".PadRight(5, ' ');
			string f131_id_unidad_medida = "";
			string f131_cant_unidad_medida = "";
			string f131_ind_tipo = "";
			string f131_ind_factor = "";
			string f131_factor = "";
			string f131_cant_interna_1 = "0";
			string f131_cant_interna_2 = "0";
			string f131_ind_factor_interno_1 = "0";
			string f131_ind_factor_interno_2 = "0";

			string f131_ind_proveedor = "0";
			string f131_id_co = "".PadRight(3, ' ');
			string f_ind_codigo_principal = "";

			string REG_CIERRE = "";

			/*FileStream fs = null;

			fs = File.Create(nombreArchivo);

			AddText(fs, REG_INICIO);*/

			WSUNOEE.WSUNOEE wsunoee = new WSUNOEE.WSUNOEE();
			int err = 1;
			string xml_item_cod_barra = $@"<?xml version=""1.0"" encoding=""utf-8""?>
							 <Importar>
								<NombreConexion>unoee_invercomer</NombreConexion>
								<IdCia>1</IdCia>
								<Usuario>ws_importar_mega</Usuario>
								<Clave>rLFgx)bL</Clave>
								<Datos>
									<Linea>{REG_INICIO}</Linea>";

			Datos datos = new Datos();
			foreach (string item in items)
			{
				List<List<object>> info_item = datos.ObtenerDatosItems(numero_doc, nombre_doc, gln_prov, gln_comp, item, 9);
				if (Convert.ToBoolean(info_item[0][11]) == true)
				{
					movimientos = "";

					F_NUMERO_REG = nroReg.ToString().PadLeft(7, '0');
					movimientos += F_NUMERO_REG;

					movimientos += F_TIPO_REG;

					movimientos += F_SUBTIPO_REG;

					movimientos += F_VERSION_REG;

					movimientos += F_CIA;

					movimientos += F_ACTUALIZA_REG;

					f131_codigo_barras = Convert.ToString(info_item[0][5]).TrimEnd('\n', '\r').Trim().PadRight(20, ' ');
					movimientos += f131_codigo_barras;

					movimientos += f131_id_item;

					f131_referencia_item = Convert.ToString(info_item[0][4]).Trim().PadRight(50, ' ');
					movimientos += f131_referencia_item;

					movimientos += f131_id_ext1_detalle;

					movimientos += f131_id_ext2_detalle;

					movimientos += f131_id_bodega;

					f131_id_unidad_medida = Convert.ToString(info_item[0][6]).Trim().PadRight(4, ' ');
					movimientos += f131_id_unidad_medida;

					f131_cant_unidad_medida = FormatoCantidadPrecio(Convert.ToString(info_item[0][7]).Trim(), 15, 4);
					movimientos += f131_cant_unidad_medida;

					f131_ind_tipo = Convert.ToString(info_item[0][8]).Trim();
					movimientos += f131_ind_tipo;

					f131_ind_factor = Convert.ToString(info_item[0][9]).Trim();
					movimientos += f131_ind_factor;

					f131_factor = FormatoCantidadPrecio(Convert.ToString(info_item[0][10]).Trim(), 15, 4);
					movimientos += f131_factor;

					movimientos += FormatoCantidadPrecio(f131_cant_interna_1, 15, 4);

					movimientos += FormatoCantidadPrecio(f131_cant_interna_2, 15, 4);

					movimientos += f131_ind_factor_interno_1;

					movimientos += f131_ind_factor_interno_2;

					movimientos += f131_ind_proveedor;

					movimientos += f131_id_co;

					f_ind_codigo_principal = Convert.ToByte(info_item[0][12]).ToString();
					movimientos += f_ind_codigo_principal;

					nroReg++;
					//AddText(fs, movimientos);
					xml_item_cod_barra += $"<Linea>{movimientos}</Linea>{Environment.NewLine}";
				}
			}

			REG_CIERRE = nroReg.ToString().PadLeft(7, '0') + "99990001001";

			//AddText(fs, REG_CIERRE);

			xml_item_cod_barra += $@"<Linea>{REG_CIERRE}</Linea>
                                </Datos>
                               </Importar> ";

			//fs.Close();
			wsunoee.Timeout = 600000;
			DataSet ds_item_cod_barra = wsunoee.ImportarXML(xml_item_cod_barra, ref err);
			DataTable dt_item_cod_barra = ds_item_cod_barra.Tables[0];

			if (dt_item_cod_barra.Rows.Count > 0)
			{
				foreach (DataRow dr in dt_item_cod_barra.Rows)
					dt_resumen.Rows.Add("ItemCodigoBarras", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]);
			}
		}

		public void CrearConectorItemPum(string numero_doc, string nombre_doc, string gln_prov, string gln_comp, List<string> items)//ESTE
		{
			////////////////////////VARIABLES///////////////////////
			string movimientos = "";
			int nroReg = 2;

			string REG_INICIO = "000000100000001001";

			string F_NUMERO_REG = ""; //Numerico 7 - Numero consecutivo
			string F_TIPO_REG = "0753"; //Numerico 4 
			string F_SUBTIPO_REG = "01"; //Numerico 2 - Valor fijo = 00
			string F_VERSION_REG = "02"; //Numerico 2 - Version = 01
			string F_CIA = "001"; // Numerico 3 - compañía a la cual pertenece la informacion del registro
			string F_ACTUALIZA_REG = "0";
			string f120_id_item = "0000000";

			string f120_referencia_item = "";
			string f753_id_grupo_entidad = "UnoEESuper_items _abc".PadRight(30, ' ');
			string f753_id_entidad = "001".PadRight(30, ' ');

			string f753_id_atributo = "";
			string f753_dato_numerico = "";
			string f753_dato_texto = " ".PadRight(2000, ' ');
			string f753_dato_fecha_hora = " ".PadRight(8, ' ');
			string f753_id_maestro = "";
			string f753_id_maestro_detalle = "";
			string f753_id_tipo_entidad = "M120".PadRight(8, ' ');
			string f753_nro_fila = "0000";
			string REG_CIERRE = "";

			/*FileStream fs = null;

			fs = File.Create(nombreArchivo);

			AddText(fs, REG_INICIO);*/

			WSUNOEE.WSUNOEE wsunoee = new WSUNOEE.WSUNOEE();
			int err = 1;
			string xml_item_pum = $@"<?xml version=""1.0"" encoding=""utf-8""?>
							 <Importar>
								<NombreConexion>unoee_invercomer</NombreConexion>
								<IdCia>1</IdCia>
								<Usuario>ws_importar_mega</Usuario>
								<Clave>rLFgx)bL</Clave>
								<Datos>
									<Linea>{REG_INICIO}</Linea>";

			Datos datos = new Datos();
			foreach (string item in items)
			{
				List<List<object>> info_item = datos.ObtenerDatosItems(numero_doc, nombre_doc, gln_prov, gln_comp, item, 11);
				//if (Convert.ToBoolean(info_item[0][11]) == true)
				//{
				for (int i = 0; i < 2; i++)
				{
					movimientos = "";
					F_NUMERO_REG = nroReg.ToString().PadLeft(7, '0');
					movimientos += F_NUMERO_REG;

					movimientos += F_TIPO_REG;

					movimientos += F_SUBTIPO_REG;

					movimientos += F_VERSION_REG;

					movimientos += F_CIA;

					movimientos += F_ACTUALIZA_REG;

					movimientos += f120_id_item;

					f120_referencia_item = Convert.ToString(info_item[0][29]).Trim().PadRight(50, ' ');

					movimientos += f120_referencia_item;

					movimientos += f753_id_grupo_entidad;

					movimientos += f753_id_entidad;
					switch (i)
					{
						case 0:
							f753_id_atributo = "UND".PadRight(30, ' ');
							f753_dato_numerico = "00000000000000000.0000000000";
							break;
						case 1:
							f753_id_atributo = "FACTOR".PadRight(30, ' ');
							f753_dato_numerico = FormatoCantidadPrecio(Convert.ToString(info_item[0][24]).Trim(), 17, 10); ;
							break;
					}
					movimientos += f753_id_atributo;
					movimientos += f753_dato_numerico;
					movimientos += f753_dato_texto;
					movimientos += f753_dato_fecha_hora;

					switch (i)
					{
						case 0:
							f753_id_maestro = "UNIDAD PUM";
							f753_id_maestro_detalle = Convert.ToString(info_item[0][28]).Trim().PadRight(20, ' ');
							break;
						case 1:
							f753_id_maestro = " ".PadRight(10, ' ');
							f753_id_maestro_detalle = " ".PadRight(20, ' ');
							break;
					}
					movimientos += f753_id_maestro;
					movimientos += f753_id_maestro_detalle;
					movimientos += f753_id_tipo_entidad;
					movimientos += f753_nro_fila;
					nroReg++;
					//AddText(fs, movimientos);
					xml_item_pum += $"<Linea>{movimientos}</Linea>{Environment.NewLine}";
				}

				//}
			}

			REG_CIERRE = nroReg.ToString().PadLeft(7, '0') + "99990001001";

			//AddText(fs, REG_CIERRE);

			xml_item_pum += $@"<Linea>{REG_CIERRE}</Linea>
                                </Datos>
                               </Importar> ";

			//fs.Close();
			wsunoee.Timeout = 600000;
			DataSet ds_item_pum = wsunoee.ImportarXML(xml_item_pum, ref err);
			DataTable dt_item_pum = ds_item_pum.Tables[0];

			if (dt_item_pum.Rows.Count > 0)
			{
				foreach (DataRow dr in dt_item_pum.Rows)
					dt_resumen.Rows.Add("ItemPum", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]);
			}
		}

		public void CrearConectorCambioPrecio(string numero_doc, string nombre_doc, string gln_prov, string gln_comp/*, string nombreArchivo*/, List<string> items)
		{
			////////////////////////VARIABLES COMUNES///////////////////////
			string movimientos = "";
			int nroReg = 2;

			string REG_INICIO = "000000100000001001";

			string F_NUMERO_REG = ""; //Numerico 7 - Numero consecutivo
			string F_TIPO_REG = ""; //Numerico 4 - VARIA EN CADA HOJA 212, 213, 214
			string F_SUBTIPO_REG = "00"; //Numerico 2 - Valor fijo = 00
			string F_VERSION_REG = "01"; //Numerico 2 - Version = 01
			string F_CIA = "001"; // Numerico 3 - compañía a la cual pertenece la informacion del registro
			string F_ACTUALIZA_REG = "1";

			string F212_ID_TERCERO = "";//CRUZAR CON UNOEE.
			string F212_ID_SUCURSAL = "";
			string F212_ID_MONEDA = "";
			string F212_ID_ITEM = "0000000";
			string F212_REFERENCIA_ITEM = "".PadRight(20, ' ');//AQUI VA EL EAN 13 VIENE DE LOS ARCHIVOS.
			string F212_CODIGO_BARRAS_ITEM = "";
			string F212_ID_EXT1_DETALLE = "".PadRight(4, ' ');
			string F212_ID_EXT2_DETALLE = "".PadRight(4, ' ');
			string F212_FECHA_ACTIVACION = "";//??
			string F212_ID_UM = "";//UNIDAD DE MEDIDA.

			////////////////////////VARIABLES COTIZACIONES///////////////////////
			string F212_PRECIO = "";//15 enteros un punto cuatro decimales. VIENE DE LOS ARCHIVOS.
			string F212_TIEMPO_ENTREGA = ""; //VA DE 1 - 999. DE TRES DIGITOS.
			string F212_FECHA_HASTA = "";//FORMATO FECHA YYYYMMDD. DE OCHO CARACTERES.

			////////////////////////VARIABLES IMPUESTOS///////////////////////
			// string F213_ID_LLAVE_IMPUESTO = "";//??
			// string F213_VALOR_IMP = "";//VALOR IMPUESTO 15 ENTEROS UN PUNTO CUATRO DECIMALES.

			////////////////////////VARIABLES DESCUENTOS///////////////////////
			string F214_ORDEN = "";//VA de 1 - 9.
			string F214_CANTIDAD_HASTA = "";//15 ENTEROS UN PUNTO CUATRO DECIMALES.
			string F214_PORCENTAJE_DSCTO = "";//3 ENTEROS UN PUNTO CUATRO DECIMALES. APLICA SI ES %.
			string F214_VALOR_DSCTO = "";//15 ENTEROS UN PUNTO CUATRO DECIMALES. APLICA SI ES VALOR.

			string REG_CIERRE = "";

			/*FileStream fs = null;

			fs = File.Create(nombreArchivo);

			AddText(fs, REG_INICIO);*/

			WSUNOEE.WSUNOEE wsunoee = new WSUNOEE.WSUNOEE();
			int err = 1;
			string xml_cambio_precio = $@"<?xml version=""1.0"" encoding=""utf-8""?>
							 <Importar>
								<NombreConexion>unoee_invercomer</NombreConexion>
								<IdCia>1</IdCia>
								<Usuario>ws_importar_mega</Usuario>
								<Clave>rLFgx)bL</Clave>
								<Datos>
									<Linea>{REG_INICIO}</Linea>";

			Datos datos = new Datos();
			foreach (string item in items)
			{
				List<List<object>> info_item = datos.ObtenerDatosItems(numero_doc, nombre_doc, gln_prov, gln_comp, item, 10);
				if (Convert.ToBoolean(info_item[0][18]) == true)
				{
					for (int i = 0; i < 2; i++)//SE PONE HASTA < 3 SI SE USAN LAS OTRAS DOS PAGINAS DEL CONECTOR.
					{
						if (i == 1 && Convert.ToDecimal(info_item[0][14]) == 0)//PARA VERIFICAR SI i ESTA EN UNO, Y SI EL DESCUENTO ES CERO NO SE GENERA LA LINEA DE DESCUENTO.
						{
							continue;
						}
						movimientos = "";

						F_NUMERO_REG = nroReg.ToString().PadLeft(7, '0');
						movimientos += F_NUMERO_REG;

						F_TIPO_REG = "";
						switch (i)
						{
							case 0:
								F_TIPO_REG = "212".PadLeft(4, '0');
								break;
							//case 1:
							//    F_TIPO_REG = "213".PadLeft(4, '0');
							//    break;
							case 1:
								F_TIPO_REG = "214".PadLeft(4, '0');
								break;
						}
						movimientos += F_TIPO_REG;

						movimientos += F_SUBTIPO_REG;

						movimientos += F_VERSION_REG;

						movimientos += F_CIA;

						movimientos += F_ACTUALIZA_REG;

						F212_ID_TERCERO = Convert.ToString(info_item[0][4]).Trim().PadRight(15, ' ');
						movimientos += F212_ID_TERCERO;

						F212_ID_SUCURSAL = Convert.ToString(info_item[0][6]).Trim().PadRight(3, ' ');

						movimientos += F212_ID_SUCURSAL;

						F212_ID_MONEDA = Convert.ToString(info_item[0][10]).Trim().PadRight(3, ' ');
						movimientos += F212_ID_MONEDA;

						movimientos += F212_ID_ITEM;

						movimientos += F212_REFERENCIA_ITEM;

						F212_CODIGO_BARRAS_ITEM = Convert.ToString(info_item[0][8]).Trim().PadRight(20, ' ');
						movimientos += F212_CODIGO_BARRAS_ITEM;

						movimientos += F212_ID_EXT1_DETALLE;

						movimientos += F212_ID_EXT2_DETALLE;

						F212_FECHA_ACTIVACION = info_item[0][19].ToString().Trim().PadRight(8, ' ');
						movimientos += F212_FECHA_ACTIVACION;

						F212_ID_UM = Convert.ToString(info_item[0][12]).Trim().PadRight(4, ' ');
						movimientos += F212_ID_UM;

						switch (i)
						{
							case 0:
								F212_PRECIO = FormatoCantidadPrecio(Convert.ToString(info_item[0][11]).Trim(), 15, 4);//VALIDAR LA COLUMNA
								movimientos += F212_PRECIO;

								F212_TIEMPO_ENTREGA = Convert.ToString(info_item[0][13]).Trim().PadLeft(3, '0');//validar
								movimientos += F212_TIEMPO_ENTREGA;
								if (info_item[0][20].ToString().Trim().Equals("19000101"))
								{
									F212_FECHA_HASTA = "".Trim().PadRight(8, ' ');
								}
								else
								{
									F212_FECHA_HASTA = info_item[0][20].ToString().Trim().PadRight(8, ' ');
								}

								movimientos += F212_FECHA_HASTA;
								break;
							//case 1:
							//    F213_ID_LLAVE_IMPUESTO = "0028"; //Convert.ToString(info_item[0][12]);//VALIDAR 
							//    movimientos += F213_ID_LLAVE_IMPUESTO;

							//    F213_VALOR_IMP = FormatoCantidadPrecio(Convert.ToString(info_item[0][13]), 15, 4);//VALIDAR ORIGEN
							//    movimientos += F213_VALOR_IMP;
							//    break;
							case 1:
								if (Convert.ToDecimal(info_item[0][14]) > 0)
								{
									F214_ORDEN = "1";//DE 1 - 9
									movimientos += F214_ORDEN;

									F214_CANTIDAD_HASTA = FormatoCantidadPrecio(Convert.ToString("999"/*row.Cells[5].Value*/), 15, 4);//VALIDAR ORIGEN
									movimientos += F214_CANTIDAD_HASTA;

									F214_PORCENTAJE_DSCTO = FormatoCantidadPrecio(Convert.ToString(info_item[0][14]).Trim(), 3, 4);//VALIDAR ORIGEN, SI ES PORCENTAJE
									movimientos += F214_PORCENTAJE_DSCTO;

									F214_VALOR_DSCTO = FormatoCantidadPrecio("0", 15, 4);  //CantidadPrecio(Convert.ToString(row.Cells[5].Value), 15, 4);//VALIDAR ORIGEN, SI ES VALOR
									movimientos += F214_VALOR_DSCTO;
								}
								if (Convert.ToDecimal(info_item[0][23]) > 0)
								{
									F214_ORDEN = "2";
									movimientos += F214_ORDEN;

									F214_CANTIDAD_HASTA = FormatoCantidadPrecio(Convert.ToString("999"), 15, 4);
									movimientos += F214_CANTIDAD_HASTA;

									F214_PORCENTAJE_DSCTO = FormatoCantidadPrecio(Convert.ToString(info_item[0][23]).Trim(), 3, 4);
									movimientos += F214_PORCENTAJE_DSCTO;

									F214_VALOR_DSCTO = FormatoCantidadPrecio("0", 15, 4);
									movimientos += F214_VALOR_DSCTO;
								}
								if (Convert.ToDecimal(info_item[0][27]) > 0)
								{
									F214_ORDEN = "3";
									movimientos += F214_ORDEN;

									F214_CANTIDAD_HASTA = FormatoCantidadPrecio(Convert.ToString("999"), 15, 4);
									movimientos += F214_CANTIDAD_HASTA;

									F214_PORCENTAJE_DSCTO = FormatoCantidadPrecio(Convert.ToString(info_item[0][27]).Trim(), 3, 4);
									movimientos += F214_PORCENTAJE_DSCTO;

									F214_VALOR_DSCTO = FormatoCantidadPrecio("0", 15, 4);
									movimientos += F214_VALOR_DSCTO;
								}
								break;
						}
						nroReg++;
						//AddText(fs, movimientos);

						xml_cambio_precio += $"<Linea>{movimientos}</Linea>{Environment.NewLine}";
					}
				}
			}

			REG_CIERRE = nroReg.ToString().PadLeft(7, '0') + "99990001001";

			//AddText(fs, REG_CIERRE);

			xml_cambio_precio += $@"<Linea>{REG_CIERRE}</Linea>
                                </Datos>
                               </Importar> ";

			//fs.Close();
			wsunoee.Timeout = 600000;
			DataSet ds_cambio_precio = wsunoee.ImportarXML(xml_cambio_precio, ref err);
			DataTable dt_cambio_precio = ds_cambio_precio.Tables[0];

			if (dt_cambio_precio.Rows.Count > 0)
			{
				foreach (DataRow dr in dt_cambio_precio.Rows)
					dt_resumen.Rows.Add("CambioPrecio", dr[0], dr[1], dr[2], dr[3], dr[4], dr[5], dr[6]);
			}
		}

		public static void AsignarArchivos()
		{
			bool asignado = false;

			List<string> asignados = new List<string>();

			string raiz = Conexion.RaizArchivos;

			string SQL = "SELECT " +
							"co_gln " +
						 "FROM " +
							"Compradores " +
							"INNER JOIN Usuarios ON co_usuario=us_id " +
													"AND us_tipo NOT IN('3','4')";
			string[] archivosEntrada = Directory.GetFiles(raiz, "*.xlsx", SearchOption.TopDirectoryOnly);
			string errores = "";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				SqlDataReader dr = cmd.ExecuteReader();
				List<string> compradores = null;
				if (dr.HasRows)
				{
					compradores = new List<string>();
					while (dr.Read())
					{
						compradores.Add(dr.GetString(0));
					}
				}
				dr.Close();
				conn.Close();

				foreach (string comprador in compradores)
				{
					CrearEstructuraDirectorios(comprador);
				}

				if (archivosEntrada.GetLongLength(0) > 0)
				{
					foreach (string comprador in compradores)
					{
						foreach (string archivo in archivosEntrada)
						{
							try
							{
								asignado = false;

								XLWorkbook wbook = new XLWorkbook(archivo);

								IXLWorksheet wsheet = wbook.Worksheet(1);

								string ultima_columna = wsheet.LastColumnUsed().ColumnLetter() + wsheet.LastColumnUsed().ColumnNumber();

								string ACCION = "ACCIONES";
								string GLN_COMPRADOR = "GLN COMPRADOR";
								string NRO_DOC = "IDENTIFICADOR NEGOCIACIÓN";

								int COLUMNA_ACCION = 0;
								int COLUMNA_GLN_COMPRADOR = 0;
								int COLUMNA_NRO_DOC = 0;

								for (int i = 1; i < wsheet.LastColumnUsed().ColumnNumber() + 1; i++)
								{
									string data = wsheet.Cell(1, i).GetValue<string>();

									if (data.Trim().Equals(ACCION))
									{
										COLUMNA_ACCION = i;
									}
									if (data.Trim().Equals(GLN_COMPRADOR))
									{
										COLUMNA_GLN_COMPRADOR = i;
									}
									if (data.Trim().Equals(NRO_DOC))
									{
										COLUMNA_NRO_DOC = i;
									}
								}

								string nombreArchivo = archivo.Split('\\')[archivo.Split('\\').Length - 1];
								for (int i = 2; i < wsheet.LastRowUsed().RowNumber() + 1; i++)
								{
									string accion = "";
									string nro_doc = "";
									string gln_comprador = "";

									if (COLUMNA_ACCION > 0)
									{
										accion = wsheet.Cell(i, COLUMNA_ACCION).GetValue<string>();
									}
									if (COLUMNA_NRO_DOC > 0)
									{
										nro_doc = wsheet.Cell(i, COLUMNA_NRO_DOC).GetValue<string>();
									}
									if (COLUMNA_GLN_COMPRADOR > 0)
									{
										gln_comprador = wsheet.Cell(i, COLUMNA_GLN_COMPRADOR).GetValue<string>();
									}

									string rutaEntrada = raiz + comprador;
									if (comprador.Trim().Equals(gln_comprador))
									{
										switch (accion/*accion.Split('-')[0].Trim()*/)
										{

											case "Adición":
												if (!File.Exists(rutaEntrada + "\\PROCESADOS\\" + nombreArchivo))
												{
													File.Copy(archivo, rutaEntrada + "\\ADICION\\" + nombreArchivo, true);
												}
												asignado = true;
												break;

											case "Modificación":
												if (!File.Exists(rutaEntrada + "\\PROCESADOS\\" + nombreArchivo))
												{
													File.Copy(archivo, rutaEntrada + "\\MODIFICACION\\PRECIO\\" + nombreArchivo, true);
												}
												asignado = true;
												break;
										}
										break;
									}
								}

								if (asignado)
								{
									asignados.Add(nombreArchivo);
								}
							}
							catch (Exception ex)
							{
								errores += ex.Message + Environment.NewLine;
								continue;
							}
						}
					}
					foreach (string archivo in archivosEntrada)
					{
						string nombreArchivo = archivo.Split('\\')[archivo.Split('\\').Length - 1];
						foreach (string item in asignados.Distinct().ToList<string>())
						{
							if (item.Equals(nombreArchivo))
							{
								if (!Directory.Exists(raiz + "\\ASIGNADOS"))
								{
									Directory.CreateDirectory(raiz + "\\ASIGNADOS");
								}
								//File.Move(archivo, raiz + "\\ASIGNADOS\\" + nombreArchivo);
								Microsoft.VisualBasic.FileIO.FileSystem.MoveFile(archivo,
													raiz + "\\ASIGNADOS\\" + nombreArchivo, Microsoft.VisualBasic.FileIO.UIOption.OnlyErrorDialogs, Microsoft.VisualBasic.FileIO.UICancelOption.DoNothing);
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al ordenar los archivos: " + ex.Message);
			}
			if (errores != "")
			{
				MessageBox.Show(errores, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		public static void CrearEstructuraDirectorios(string comprador)
		{
			string raiz = Conexion.RaizArchivos;

			if (!Directory.Exists(raiz + comprador + "\\PROCESADOS"))
			{
				Directory.CreateDirectory(raiz + comprador + "\\PROCESADOS");
			}

			if (!Directory.Exists(raiz + comprador + "\\ADICION"))
			{
				Directory.CreateDirectory(raiz + comprador + "\\ADICION");
			}
			if (!Directory.Exists(raiz + comprador + "\\MODIFICACION\\PRECIO"))
			{
				Directory.CreateDirectory(raiz + comprador + "\\MODIFICACION\\PRECIO");
			}
			if (!Directory.Exists(raiz + comprador + "\\MODIFICACION\\ACTIVACION"))
			{
				Directory.CreateDirectory(raiz + comprador + "\\MODIFICACION\\ACTIVACION");
			}
			if (!Directory.Exists(raiz + comprador + "\\MODIFICACION\\INACTIVACION"))
			{
				Directory.CreateDirectory(raiz + comprador + "\\MODIFICACION\\INACTIVACION");
			}
			if (!Directory.Exists(raiz + comprador + "\\RETIRO"))
			{
				Directory.CreateDirectory(raiz + comprador + "\\RETIRO");
			}
		}

		/// <summary>
		/// Crea en la tabla Actividades un registro por cada acccion que se realice (Adición, Activación, Inactivación, Cambio de Precio).
		/// </summary>
		/// <param name="usuario">Nombre del usuario que realiza la acción.</param>
		/// <param name="comprador">Gln de comprador asociado al usuario que realiza la acción.</param>
		/// <param name="accion">Acción que se realiza (Adición, Activación, Inactivación, Cambio de Precio).</param>
		/// <param name="archivo">Nombre del archivo con el que se realizo la acción.</param>
		public void RegistrarActividad(string usuario, string comprador, string proveedor, string accion, string numero_doc, string nombre_doc)
		{
			string SQL = "INSERT INTO Actividades (lg_usuario, lg_comprador, lg_proveedor, lg_accion, lg_numero_doc, lg_nombre_doc, lg_fecha) " +
						 "VALUES(@USUARIO, @COMP, @PROV, @ACCION, @NRO_DOC, @NOMB_DOC, GETDATE())";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@USUARIO", usuario);
				cmd.Parameters.AddWithValue("@COMP", comprador);
				cmd.Parameters.AddWithValue("@PROV", proveedor);
				cmd.Parameters.AddWithValue("@ACCION", accion);
				cmd.Parameters.AddWithValue("@NRO_DOC", numero_doc);
				cmd.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al registrar la actividad: " + ex.Message);
			}
		}

		/// <summary>
		/// Mueve el archivo procesado a la carpeta PROCESADOS
		/// </summary>
		/// <param name="origen">Ubicación inicial del archivo.</param>
		public static void MoverArchivoProcesado(string origen)
		{
			try
			{
				string raiz = Conexion.RaizArchivos;

				if (!File.Exists(raiz + Datos.GlnComprador + "\\PROCESADOS\\" + Datos.NombreDocumento))
				{
					File.Move(origen, raiz + Datos.GlnComprador + "\\PROCESADOS\\" + Datos.NombreDocumento);
				}
				else
				{
					File.Delete(origen);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al mover el archivo procesado: " + ex.Message);
			}
		}
	}
}
