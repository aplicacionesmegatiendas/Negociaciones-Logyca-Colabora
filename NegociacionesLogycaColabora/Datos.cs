using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using ClosedXML.Excel;
using System.Globalization;
using System.ComponentModel;

namespace NegociacionesLogycaColabora
{
	public class Datos
	{
		private static string usuario = "";
		private static string tipo_usuario = "";
		private static string numero_documento = "";
		private static string nombre_documento = "";
		private static string gln_proveedor = "";
		private static string gln_comprador = "";
		private static string comprador = "";

		private static string descripcion = "";

		public static bool salir = false;

		public static string Usuario
		{
			get { return usuario; }
			set { usuario = value; }
		}

		public static string TipoUsuario
		{
			get { return tipo_usuario; }
			set { tipo_usuario = value; }
		}

		public static string NumeroDocumento
		{
			get { return numero_documento; }
			set { numero_documento = value; }
		}

		public static string NombreDocumento
		{
			get { return nombre_documento; }
			set { nombre_documento = value; }
		}

		public static string GlnProveedor
		{
			get { return gln_proveedor; }
			set { gln_proveedor = value; }
		}

		public static string GlnComprador
		{
			get { return gln_comprador; }
			set { gln_comprador = value; }
		}

		public static string Comprador
		{
			get { return comprador; }
			set { comprador = value; }
		}

		public static string Descripcion
		{
			get { return descripcion; }
			set { descripcion = value; }
		}

		public static string GetValue(int columna, IXLWorksheet sheet)
		{
			string res = "";
			for (int i = 2; i < sheet.LastRowUsed().RowNumber() + 1; i++)
			{
				if (!sheet.Cell(i, columna).GetValue<string>().Trim().Equals(""))
				{
					res = sheet.Cell(i, columna).GetValue<string>().Trim();
					break;
				}
			}
			return res;
		}

		public static int GetColumnIndex(string texto_columna, IXLWorksheet sheet)
		{
			int res = 0;
			for (int i = 1; i < sheet.LastColumnUsed().ColumnNumber() + 1; i++)
			{
				string data = sheet.Cell(1, i).GetValue<string>();

				if (data.Trim().Equals(texto_columna))
				{
					res = i;
					break;
				}
			}
			return res;
		}

		/// <summary>
		/// Obtiene el listado de grupos impositivos.
		/// </summary>
		/// <returns>Devuelve un objeto de tipo List con la información.</returns>
		public List<GrupoImpositivo> ListarGruposImpositivos()
		{
			string SQL = "SELECT " +
							"f113_id, " +
							"f113_descripcion " +
						"FROM " +
							"t113_mc_grupos_impositivos " +
						"WHERE " +
							"f113_id_cia = 1 " +
						"ORDER BY " +
							"CASE f113_id " +
									"WHEN '0204' THEN 1 " +
									"WHEN '0210' THEN 2 " +
									"WHEN '0043' THEN 3 " +
									"WHEN '0045' THEN 4 " +
									"WHEN '0050' THEN 5 " +
									"WHEN '0050' THEN 6 " +
									"WHEN '0051' THEN 7 " +
									"WHEN '0052' THEN 8 " +
									"WHEN '0053' THEN 10 " +
									"ELSE 11 " +
							"END";
			try
			{
				List<GrupoImpositivo> listado = null;
				GrupoImpositivo item = null;
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				SqlDataReader dr = cmd.ExecuteReader();

				if (dr.HasRows)
				{
					listado = new List<GrupoImpositivo>();
					while (dr.Read())
					{
						item = new GrupoImpositivo();
						item.Id = dr.GetString(0).Trim();
						item.Descripción = dr.GetString(1).Trim();
						listado.Add(item);
					}
				}
				dr.Close();
				conn.Close();
				return listado;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de grupos impositivos: " + ex.Message);
			}
		}

		public string ObtenerTasa(string grupo_impositivo)
		{
			string SQL = "SELECT DISTINCT " +
						   "convert(decimal(18, 2), f037_tasa) f037_tasa  " +
						"FROM " +
							"t113_mc_grupos_impositivos " +
							"left join t114_mc_grupos_impo_impuestos on f113_id = f114_grupo_impositivo and f113_id_cia = f114_id_cia " +
							"left join t037_mm_llaves_impuesto on f114_id_llave_impuesto = f037_id and f114_id_cia = f037_id_cia " +
															  "and f114_id_clase_impuesto = f037_id_clase_impuesto " +
						"WHERE " +
							"f113_id_cia = 1 " +
							"and f037_ind_estado = '1' " +
							"and f037_id_clase_impuesto = '1' " +
							"and f113_id = @ID";
			string res = "";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@ID", grupo_impositivo);
				res = Convert.ToDecimal(cmd.ExecuteScalar()).ToString("0.##");
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener la tasa del grupo impositivo: " + ex.Message);
			}
			return res;
		}

		/// <summary>
		/// Obtiene el listado de tipos de inventario.
		/// </summary>
		/// <returns>Devuelve un objeto de tipo List con la información.</returns>
		public List<TipoInventario> ListarTiposInventario()
		{
			string SQL = "SELECT " +
							"f149_id, f149_descripcion " +
						 "FROM " +
							  "t149_mc_tipo_inv_serv " +
						 "WHERE f149_id_cia=1 " +
						 "ORDER BY " +
							"CASE f149_id " +
								"WHEN  'CO-COM3505' THEN 1 " +
								"WHEN 'CO-COM3510' THEN 2 " +
								"WHEN 'CO-COM3515' THEN 3 " +
								"ELSE 4 " +
							 "END";
			try
			{
				List<TipoInventario> listado = null;
				TipoInventario item = null;
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				SqlDataReader dr = cmd.ExecuteReader();

				if (dr.HasRows)
				{
					listado = new List<TipoInventario>();
					while (dr.Read())
					{
						item = new TipoInventario();
						item.Id = dr.GetString(0).Trim();
						item.Descripción = dr.GetString(1).Trim();
						listado.Add(item);
					}
				}
				dr.Close();
				conn.Close();
				return listado;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de tipos de inventario: " + ex.Message);
			}
		}

		/// <summary>
		/// Obtiene el nit del proveedor a partir de un código gln.
		/// </summary>
		/// <param name="gln">Código gln.</param>
		/// <returns>Devuelve el nit del proveedor.</returns>
		public string[] ObtenerNitProveedor(string gln)
		{
			string SQL = "select " +
							"ISNULL(f200_nit,'') AS f200_nit, " +
							"MIN(ISNULL(f202_id_sucursal,'')) AS f202_id_sucursal, " +
							"f200_rowid, " +
							"f200_razon_social " +
						 "from " +
							"( " +
								 "select distinct " +
									"case when (f015_direccion2 IS null OR f015_direccion2=' ') then " +
										"f015_direccion3 " +
									"else " +
										"f015_direccion2 " +
									"end gln, " +
									"f200_id, " +
									"f200_nit, " +
									"f200_razon_social, " +
									"f015_contacto, " +
									"f015_direccion1, " +
									"f015_email, " +
									"f202_id_sucursal, f200_rowid " +
								"from " +
									"t200_mm_terceros " +
									"left join t202_mm_proveedores on f202_rowid_tercero=f200_rowid and f202_id_cia=f200_id_cia " +
									"left join t015_mm_contactos on f015_rowid=f202_rowid_contacto and f015_id_cia=f202_id_cia " +
								"where " +
									"(" +
										"isnumeric(f015_direccion2)>0 or " +
										"isnumeric(f015_direccion3)>0" +
									") and " +
									"f200_id_cia=1 " +
							")gln_proveedor " +
						"where " +
							"gln=@GLN " +
						"group by " +
							"f200_nit," +
							"f200_razon_social," +
							"f200_rowid";
			string[] res = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@GLN", gln);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new string[4];
					dr.Read();
					res[0] = dr.GetString(0);
					res[1] = dr.GetString(1);
					res[2] = Convert.ToString(dr.GetInt32(2));
					res[3] = dr.GetString(3);
				}
				dr.Close();
				conn.Close();
				return res;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el nit del proveedor: " + ex.Message);
			}
		}

		public List<string> ObtenerSucursalesProveedor(string rowid)
		{
			string SQL = "select " +
							"f202_id_sucursal " +
						 "from " +
							"t202_mm_proveedores " +
						 "where " +
							"f202_rowid_tercero = @ROWID and " +
							"f202_id_cia=1";
			List<string> res = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@ROWID", rowid);
				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new List<string>();
					while (dr.Read())
					{
						res.Add(dr[0].ToString());
					}
				}
				dr.Close();
				conn.Close();
				return res;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener las sucursales del proveedor: " + ex.Message);
			}
		}

		/// <summary>
		/// Obtiene el listado de planes desde unoee.
		/// </summary>
		/// <returns>Devuelve un DataTable que contiene la información</returns>
		public DataTable ObtenerPlanes()
		{
			string SQL = "SELECT " +
							"f105_id, " +
							"f105_id + ' - ' + f105_descripcion f105_descripcion " +
						 "FROM " +
							"t105_mc_criterios_item_planes " +
						 "WHERE " +
							"f105_id_cia=1";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener los Planes: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Obtiene el listado de Criterios Mayor.
		/// </summary>
		/// <param name="plan">Codigo del Plan</param>
		/// <returns>Devuelve un DataTable que contiene la información.</returns>
		public DataTable ObtenerCriteriosMayor(string plan)
		{
			string SQL = "SELECT " +
							"f106_id, " +
							"f106_id + ' - ' + f106_descripcion f106_descripcion " +
						 "FROM " +
							"t106_mc_criterios_item_mayores " +
						 "WHERE " +
							"f106_id_plan=@PLAN " +
							"AND f106_id_cia=1";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.AddWithValue("@PLAN", plan);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener los Criterios Mayor: " + ex.Message);
			}
			return dt;
		}

		public DataTable ListarEquivalenciasCategoriaLogyca()
		{
			string SQL = "SELECT " +
							"DISTINCT ca_id, " +
							"ca_catlogyca, " +
							"ca_plan, " +
							"ca_desc_plan, " +
							"ca_criterio, " +
							"ca_desc_mayor " +
						 "FROM " +
							"Categorias";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de equivalencias Pricat: " + ex.Message);
			}
			return dt;
		}

		public int GuardarEquivalenciaCategoriaLogyca(string categoria, string plan, string descplan, string criterio, string desccriterio)
		{
			string SQL = "INSERT INTO " +
							"Categorias " +
							"(" +
								"ca_catlogyca, " +
								"ca_criterio, " +
								"ca_desc_mayor, " +
								"ca_plan, " +
								"ca_desc_plan" +
							") " +
						 "VALUES " +
							"(" +
								"@CAT_LOGYCA, " +
								"@CRITERIO, " +
								"@DESC_CRIT, " +
								"@PLAN, " +
								"@DESC_PLAN" +
							")";
			int res = -1;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@CAT_LOGYCA", categoria);
				cmd.Parameters.AddWithValue("@CRITERIO", criterio);
				cmd.Parameters.AddWithValue("@DESC_CRIT", desccriterio);
				cmd.Parameters.AddWithValue("@PLAN", plan);
				cmd.Parameters.AddWithValue("@DESC_PLAN", descplan);
				res = cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar la Categoría Pricat: " + ex.Message);
			}

			return res;
		}

		public int EliminarEquivalenciaCategoriaLogyca(int id)
		{
			string SQL = "DELETE " +
							"Categorias " +
						 "WHERE " +
							"ca_id=@ID";
			int res = -1;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@ID", id);

				res = cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al eliminar la Categoría Pricat: " + ex.Message);
			}

			return res;
		}

		/// <summary>
		/// Obtiene el listado de unidades de medida.
		/// </summary>
		/// <returns>Devuelve un objeto de tipo DataTable que contiene la información.</returns>
		public DataTable ListarEquivalenciaUnidadesMedida()
		{
			string SQL = "SELECT " +
							"un_id, " +
							"un_unidad_megatiendas, " +
							"un_unidad_logyca " +
						 "FROM " +
							"Unidades";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de unidades de medida: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Obtiene el listado de unidades de medida.
		/// </summary>
		/// <returns>Devuelve un objeto List que contiene la información.</returns>
		public List<UnidadMedida> ListarUnidadesMedida()
		{
			string SQL = @"SELECT
							RTRIM(f101_id) AS f101_id, 
							(f101_id + ' - ' + f101_descripcion) AS f101_descripcion 
						 FROM 
							t101_mc_unidades_medida 
						 WHERE 
							f101_id_cia=1 
						 ORDER BY 
							1";
			try
			{
				List<UnidadMedida> listado = null;
				UnidadMedida item = null;
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				SqlDataReader dr = cmd.ExecuteReader();

				if (dr.HasRows)
				{
					listado = new List<UnidadMedida>();
					while (dr.Read())
					{
						item = new UnidadMedida();
						item.Id = dr.GetString(0);
						item.Descripción = dr.GetString(1);
						listado.Add(item);
					}
				}
				dr.Close();
				conn.Close();
				return listado;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de unidades de medida: " + ex.Message);
			}
		}

		public string ObtenerIdUnidadMedida(string unidad_medida)
		{
			string SQL = @"select
								un_id_unidad_pum
							from
								Unidades
							where
								un_unidad_megatiendas =@und";
			string res = "";
			try
			{				
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@und", unidad_medida);
				res=Convert.ToString(cmd.ExecuteScalar()).Trim();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener id de unidad de medida: " + ex.Message);
			}
			return res;
		}

		/// <summary>
		/// Guarda la unidad de medida Megatiendas y su equivalente Logyca.
		/// </summary>
		/// <param name="und_mega">Unidad de medida Megatiendas.</param>
		/// <param name="und_logyca">Unidad de medida Logyca.</param>
		public void GuardarUnidadMedida(string und_mega, string und_logyca)
		{
			string SQL = "IF NOT EXISTS" +
							"(" +
								"SELECT " +
									"* " +
								"FROM " +
									"unidades " +
								"WHERE " +
									"un_unidad_megatiendas=@MEGA " +
									"AND un_unidad_logyca=@LOGYCA" +
							") " +
						 "BEGIN " +
							 "INSERT INTO " +
								"unidades " +
								"(" +
									"un_unidad_megatiendas, " +
									"un_unidad_logyca" +
								") " +
								"VALUES" +
								"(" +
									"@MEGA," +
									"@LOGYCA" +
								") " +
						 "END";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@MEGA", und_mega);
				cmd.Parameters.AddWithValue("@LOGYCA", und_logyca);

				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar la unidad de medida: " + ex.Message);
			}
		}

		/// <summary>
		/// Elimina una unidad de medida.
		/// </summary>
		/// <param name="id">Id de la unidad de medida.</param>
		/// <returns></returns>
		public int EliminarUnidadMedida(int id)
		{
			string SQL = "DELETE " +
							"Unidades " +
						 "WHERE " +
							"un_id=@ID";
			int res = -1;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@ID", id);

				res = cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al eliminar la unidad de medida: " + ex.Message);
			}

			return res;
		}

		/// <summary>
		/// Obtiene el grupo de un item existente.
		/// </summary>
		/// <param name="referencia">Referencia del item.</param>
		/// <returns>Devuelve el grupo impositivo.</returns>
		public string ObtenerGrupoImpositivo(string referencia)
		{
			string SQL = "select " +
							"f120_id_grupo_impositivo " +
						 "from " +
							"t120_mc_items " +
						 "where " +
							"f120_referencia=@REF AND " +
							"f120_id_cia=1";
			string res = "";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@REF", referencia);
				res = Convert.ToString(cmd.ExecuteScalar());
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el grupo impositivo del item: " + ex.Message);
			}
			return res;
		}

		/// <summary>
		/// Obtiene el lisatdo de compradores.
		/// </summary>
		/// <returns>Devuelve un objeto DataTable que contiene la información.</returns>
		public DataTable ListarCompradores()
		{
			string SQL = "SELECT " +
							"co_id, " +
							"co_gln, " +
							"co_nombre, " +
							"co_email, " +
							"us_nombre " +
						"FROM " +
							"Compradores " +
							"INNER JOIN Usuarios ON co_usuario=us_id AND us_tipo=2";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de compradores: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Obtiene el listado de usuarios que no estan asociados a un comprador.
		/// </summary>
		/// <returns>Devuelve un objeto DataTable que contiene la información.</returns>
		public DataTable ListarUsuariosDisponibles()
		{
			string SQL = "SELECT " +
							"us_id, " +
							"us_nombre " +
						 "FROM " +
							"Usuarios " +
						 "WHERE " +
							"us_tipo IN('2','3','4') AND " +
							"us_id NOT IN" +
										"(" +
											"SELECT " +
												"co_usuario " +
											"FROM " +
												"Compradores" +
										")";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de usuarios disponibles: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Guarda la información del comprador en la base de datos.
		/// </summary>
		/// <param name="gln">Gln del comprador.</param>
		/// <param name="nombre">Nombre del comprador.</param>
		/// <param name="usuario">Usuario del programa que se asocia al comprador.</param>
		/// <returns></returns>
		public int GuardarComprador(string gln, string nombre, string email, int usuario = -1, int id = -1)
		{
			string SQL = "IF NOT EXISTS " +
							"(" +
								"SELECT " +
									"co_gln " +
								"FROM " +
									"Compradores " +
								"WHERE " +
									"co_gln=@gln " +
							") " +
							"BEGIN " +
								"INSERT INTO " +
									"Compradores " +
									"( " +
										"co_gln, " +
										"co_nombre, " +
										"co_email, " +
										"co_usuario " +
									") " +
									"VALUES " +
									"( " +
										"@gln, " +
										"@nomb, " +
										"@email, " +
										"@usuario " +
									") " +
							"END " +
						 "ELSE " +
							"BEGIN " +
								"UPDATE " +
									"Compradores " +
								"SET " +
									"co_nombre= @nomb, " +
									"co_email=@email, " +
									"co_usuario=@usuario " +
								"WHERE " +
									"co_id=@id " +
							"END ";
			int res = -1;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@gln", gln);
				cmd.Parameters.AddWithValue("@nomb", nombre);
				cmd.Parameters.AddWithValue("@email", email);
				cmd.Parameters.AddWithValue("@usuario", usuario);
				cmd.Parameters.AddWithValue("@id", id);
				res = cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar el comprador: " + ex.Message);
			}
			return res;
		}

		/// <summary>
		/// Elimina un comprador de la base de datos.
		/// </summary>
		/// <param name="id">Id del comprador.</param>
		public void EliminarComprador(int id)
		{
			string SQL = "DELETE " +
							"Compradores " +
						 "WHERE " +
							"co_id=@id";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@id", id);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al eliminar el comprador: " + ex.Message);
			}
		}

		/// <summary>
		/// Obtiene el listado de activides en un rango de fecha. 
		/// </summary>
		/// <param name="fecha_ini">Fecha inicial.</param>
		/// <param name="fecha_fin">Fecha final.</param>
		public DataTable ListarActividades(string fecha_ini, string fecha_fin)
		{
			string SQL = "SELECT " +
							"lg_usuario, " +
							"co_gln, " +
							"co_nombre, " +
							"lg_fecha, " +
							"lg_accion, " +
							"lg_archivo " +
						 "FROM " +
							"Actividades " +
						 "INNER JOIN " +
							"Usuarios ON lg_usuario=us_nombre " +
						 "INNER JOIN " +
							"Compradores ON co_gln=lg_Comprador " +
						 "WHERE " +
							"lg_fecha BETWEEN @FI AND @FF";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@FI", fecha_ini);
				cmd.Parameters.AddWithValue("@FF", fecha_fin);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable("Actividades");
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de actividades: " + ex.Message);
			}
			return dt;
		}

		public int ObtenerConsecutivoNumeroDocumento(int id)
		{
			string SQL = @"select
                            se_secuencia
                        from
                            Secuencia
                        where
                            se_id = @id";
			int res = 0;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@id", id);
				res = (int)cmd.ExecuteScalar();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener consecutivo número de documento: " + ex.Message);
			}
			return res;
		}

		public void ActualizarConsecutivoNumeroDocumento(int id)
		{
			string SQL = @"update
	                        Secuencia
                        set
	                        se_secuencia=se_secuencia + 1
                        where
	                        se_id=@id";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@id", id);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actulizar consecutivo número de documento: " + ex.Message);
			}
		}

		public void GuardarInfoDocumento(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador, string tipo, string accion, string nit, string razon_soc, string sucursal, string fecha_elab, string usuario)
		{
			string SQL = "SP_AgregarDocumento";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.Parameters.Clear();
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@NRO_DOC", numero_doc);
				cmd.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
				cmd.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
				cmd.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
				cmd.Parameters.AddWithValue("@TIPO", tipo);
				cmd.Parameters.AddWithValue("@ACCION", accion);
				cmd.Parameters.AddWithValue("@NIT", nit);
				cmd.Parameters.AddWithValue("@RAZON_SOC", razon_soc);
				cmd.Parameters.AddWithValue("@SUCURSAL", sucursal);
				cmd.Parameters.AddWithValue("@FECHA_ELAB", fecha_elab);
				cmd.Parameters.AddWithValue("@USUARIO", usuario);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar la información del documento: " + ex.Message);
			}
		}

		public void GuardarInfoItems(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador, IXLWorksheet lineas)
		{
			string SQL = "SP_AgregarItem";

			string GLN_COMPRADOR = "GLN COMPRADOR";
			string GTIN = "GTIN";
			string DESCRIPCION_LARGA = "DESCRIPCIÓN LARGA";
			string DESCRIPCION_CORTA = "DESCRIPCIÓN CORTA";
			string PESO_NETO = "PESO NETO";
			string CALIFICADOR_PESO_NETO = "CALIFICADOR DE PESO NETO";
			string PESO_BRUTO = "PESO BRUTO";
			string CALIFICADOR_PESO_BRUTO = "CALIFICADOR DE PESO BRUTO";
			string CANT_MIN_ORD = "CANTIDAD MINIMA A ORDENAR";
			string CATEGORIA_LOGYCA = "CATEGORIA LOGYCA";
			string TIPO_IVA = "TIPO IVA-1";
			string PORC_IVA = "TIPO DE REPRESENTACIÓN IVA MODAL-1";
			string URL_IMAGEN = "URLIMAGEN";

			int COLUMNA_GLN_COMPRADOR = 0;
			int COLUMNA_GTIN = 0;
			int COLUMNA_DESCRIPCION_LARGA = 0;
			int COLUMNA_DESCRIPCION_CORTA = 0;
			int COLUMNA_PESO_NETO = 0;
			int COLUMNA_CALIFICADOR_PESO_NETO = 0;
			int COLUMNA_PESO_BRUTO = 0;
			int COLUMNA_CALIFICADOR_PESO_BRUTO = 0;
			int COLUMNA_CANT_MIN_ORD = 0;
			int COLUMNA_CATEGORIA_LOGYCA = 0;
			int COLUMNA_TIPO_IVA = 0;
			int COLUMNA_PORC_IVA = 0;
			int COLUMNA_URL_IMAGEN = 0;

			SqlTransaction trans = null;

			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd = new SqlCommand(SQL, conn, trans);
				cmd.CommandType = CommandType.StoredProcedure;

				List<string> estan = new List<string>();

				DataTable dt_unds_medida = ListarEquivalenciaUnidadesMedida();

				COLUMNA_GLN_COMPRADOR = GetColumnIndex(GLN_COMPRADOR, lineas);
				COLUMNA_GTIN = GetColumnIndex(GTIN, lineas);
				COLUMNA_DESCRIPCION_LARGA = GetColumnIndex(DESCRIPCION_LARGA, lineas);
				COLUMNA_DESCRIPCION_CORTA = GetColumnIndex(DESCRIPCION_CORTA, lineas);
				COLUMNA_PESO_NETO = GetColumnIndex(PESO_NETO, lineas);
				COLUMNA_CALIFICADOR_PESO_NETO = GetColumnIndex(CALIFICADOR_PESO_NETO, lineas);
				COLUMNA_PESO_BRUTO = GetColumnIndex(PESO_BRUTO, lineas);
				COLUMNA_CALIFICADOR_PESO_BRUTO = GetColumnIndex(CALIFICADOR_PESO_BRUTO, lineas);
				COLUMNA_CANT_MIN_ORD = GetColumnIndex(CANT_MIN_ORD, lineas);
				COLUMNA_CATEGORIA_LOGYCA = GetColumnIndex(CATEGORIA_LOGYCA, lineas);
				COLUMNA_TIPO_IVA = GetColumnIndex(TIPO_IVA, lineas);
				COLUMNA_PORC_IVA = GetColumnIndex(PORC_IVA, lineas);
				COLUMNA_URL_IMAGEN = GetColumnIndex(URL_IMAGEN, lineas);

				for (int i = 2; i < lineas.LastRowUsed().RowNumber() + 1; i++)
				{
					string id = "";
					if (COLUMNA_GTIN > 0)
					{
						if (lineas.Cell(i, COLUMNA_GTIN).GetValue<string>().Trim().Length > 13)
						{
							continue;
						}
						id = VerificarItemExiste(lineas.Cell(i, COLUMNA_GTIN).GetValue<string>());
						if (id.Equals(""))
						{
							if (gln_comprador.Trim().Equals(lineas.Cell(i, COLUMNA_GLN_COMPRADOR).GetValue<string>()))
							{
								string und_orden_emp = "";
								if (COLUMNA_CALIFICADOR_PESO_NETO > 0)
								{
									foreach (DataRow und in dt_unds_medida.Rows)
									{
										string und_logyca = lineas.Cell(i, COLUMNA_CALIFICADOR_PESO_NETO).GetValue<string>().Trim().Split('-')[0].Trim();
										if (und[2].Equals(und_logyca))
										{
											und_orden_emp = und[1].ToString();
											break;
										}
									}
								}
								else
								{
									if (COLUMNA_CALIFICADOR_PESO_BRUTO > 0)
									{
										foreach (DataRow und in dt_unds_medida.Rows)
										{
											if (und[2].Equals(lineas.Cell(i, COLUMNA_CALIFICADOR_PESO_BRUTO).GetValue<string>().Trim().Split('-')[0].Trim()))
											{
												und_orden_emp = und[1].ToString();
												break;
											}
										}
									}
								}
								cmd.Parameters.Clear();
								cmd.Parameters.AddWithValue("@NRO_DOC", numero_doc);
								cmd.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
								cmd.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
								cmd.Parameters.AddWithValue("@GLN_COMP", gln_comprador);

								cmd.Parameters.AddWithValue("@GTIN", lineas.Cell(i, COLUMNA_GTIN).GetValue<string>().TrimEnd('\n', '\r').Trim());
								if (COLUMNA_DESCRIPCION_LARGA > 0)
								{
									string desc_larga = lineas.Cell(i, COLUMNA_DESCRIPCION_LARGA).GetValue<string>().TrimEnd('\n', '\r').Trim();
									if (desc_larga.Length > 40)
									{
										desc_larga = desc_larga.Substring(0, 40);
									}
									cmd.Parameters.AddWithValue("@DESC_LARGA", desc_larga);
								}
								if (COLUMNA_DESCRIPCION_CORTA > 0)
								{
									string desc_corta = lineas.Cell(i, COLUMNA_DESCRIPCION_CORTA).GetValue<string>().TrimEnd('\n', '\r').Trim();
									if (desc_corta.Length > 20)
									{
										desc_corta = desc_corta.Substring(0, 20);
									}
									cmd.Parameters.AddWithValue("@DESC_CORTA", desc_corta);
								}

								if (COLUMNA_CALIFICADOR_PESO_NETO > 0)
								{
									decimal.TryParse(lineas.Cell(i, COLUMNA_PESO_NETO).GetValue<string>().Replace('.', ','), out decimal p);
									if (lineas.Cell(i, COLUMNA_CALIFICADOR_PESO_NETO).GetValue<string>().Split('-')[0].Equals("GRM"))
									{
										cmd.Parameters.AddWithValue("@FACT_PESO_INV", p / 1000);
									}
									else if (lineas.Cell(i, COLUMNA_CALIFICADOR_PESO_NETO).GetValue<string>().Split('-')[0].Equals("MGM"))
									{
										cmd.Parameters.AddWithValue("@FACT_PESO_INV", p / 1000000);
									}
									else if (lineas.Cell(i, COLUMNA_CALIFICADOR_PESO_NETO).GetValue<string>().Split('-')[0].Equals("KGM"))
									{
										cmd.Parameters.AddWithValue("@FACT_PESO_INV", p);
									}
									else
									{
										cmd.Parameters.AddWithValue("@FACT_PESO_INV", 0);
									}
								}
								else
								{
									if (COLUMNA_CALIFICADOR_PESO_BRUTO > 0)
									{
										decimal.TryParse(lineas.Cell(i, COLUMNA_PESO_BRUTO).GetValue<string>().Replace('.', ','), out decimal p);
										if (lineas.Cell(i, COLUMNA_CALIFICADOR_PESO_BRUTO).GetValue<string>()/*.Split('-')[0]*/.Equals("GRM"))
										{
											cmd.Parameters.AddWithValue("@FACT_PESO_INV", p / 1000);
										}
										else if (lineas.Cell(i, COLUMNA_CALIFICADOR_PESO_BRUTO).GetValue<string>()/*.Split('-')[0]*/.Equals("MGM"))
										{
											cmd.Parameters.AddWithValue("@FACT_PESO_INV", p / 1000000);
										}
										else if (lineas.Cell(i, COLUMNA_CALIFICADOR_PESO_BRUTO).GetValue<string>()/*.Split('-')[0]*/.Equals("KGM"))
										{
											cmd.Parameters.AddWithValue("@FACT_PESO_INV", p);
										}
										else
										{
											cmd.Parameters.AddWithValue("@FACT_PESO_INV", 0);
										}
									}
									else
									{
										cmd.Parameters.AddWithValue("@FACT_PESO_INV", 0);
									}
								}
								cmd.Parameters.AddWithValue("@UND_ORDEN", und_orden_emp);
								if (COLUMNA_CANT_MIN_ORD > 0)
								{
									decimal.TryParse(lineas.Cell(i, COLUMNA_CANT_MIN_ORD).GetValue<string>(), out decimal r);
									cmd.Parameters.AddWithValue("@FACTOR_ORDEN", r);
									cmd.Parameters.AddWithValue("@FACTOR_EMP", r);
								}
								else
								{
									cmd.Parameters.AddWithValue("@FACTOR_ORDEN", 0);
									cmd.Parameters.AddWithValue("@FACTOR_EMP", 0);
								}

								cmd.Parameters.AddWithValue("@UND_EMP", und_orden_emp);

								if (COLUMNA_CATEGORIA_LOGYCA > 0)
								{
									cmd.Parameters.AddWithValue("@CAT_LOGYCA", lineas.Cell(i, COLUMNA_CATEGORIA_LOGYCA).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@CAT_LOGYCA", "");
								}
								if (COLUMNA_TIPO_IVA > 0)
								{
									if (lineas.Cell(i, COLUMNA_TIPO_IVA).GetValue<string>().Trim().ToLower().Equals("2"/*porcentaje*/))
									{
										if (COLUMNA_PORC_IVA > 0)
										{
											decimal.TryParse(lineas.Cell(i, COLUMNA_PORC_IVA).GetValue<string>().Replace('.', ','), out decimal imp);
											cmd.Parameters.AddWithValue("@IMPUESTO", imp);
										}
										else
										{
											cmd.Parameters.AddWithValue("@IMPUESTO", 0);
										}
									}
									else if (lineas.Cell(i, COLUMNA_TIPO_IVA).GetValue<string>().Trim().ToLower().Equals("1"/*valor*/))
									{
										if (COLUMNA_PORC_IVA > 0)
										{
											decimal.TryParse(lineas.Cell(i, COLUMNA_PORC_IVA).GetValue<string>().Replace('.', ','), out decimal imp);
											cmd.Parameters.AddWithValue("@IMPUESTO", imp);
										}
										else
										{
											cmd.Parameters.AddWithValue("@IMPUESTO", 0);
										}
									}
									else
									{
										cmd.Parameters.AddWithValue("@IMPUESTO", 0);
									}
								}
								else
								{
									cmd.Parameters.AddWithValue("@IMPUESTO", 0);
								}

								if (COLUMNA_URL_IMAGEN > 0)
								{
									cmd.Parameters.AddWithValue("@IMAGEN", lineas.Cell(i, COLUMNA_URL_IMAGEN).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@IMAGEN", DBNull.Value);
								}
								/*for (int f = 0; f < cmd.Parameters.Count; f++)
								{
									System.Diagnostics.Debug.WriteLine(cmd.Parameters[f].ParameterName + " - " + cmd.Parameters[f].Value);
								}*/
								cmd.ExecuteNonQuery();

								cmd.Parameters.Clear();
							}
						}
						else
						{
							if (COLUMNA_GTIN > 0 && COLUMNA_DESCRIPCION_LARGA > 0)
							{
								estan.Add(lineas.Cell(i, COLUMNA_GTIN).GetValue<string>() + "|" + lineas.Cell(i, COLUMNA_DESCRIPCION_LARGA).GetValue<string>());
							}
						}
					}
				}

				if (estan.Count > 0)
				{
					string sql = "select " +
									"do_razon_social, " +
									"do_nit " +
								 "from " +
									"documentos " +
								 "where " +
									"do_numero_doc=@NRO_DOC and " +
									"do_nombre_doc=@NOMB_DOC and " +
									"do_gln_proveedor=@GLN_PROV and " +
									"do_gln_comprador=@GLN_COMP";

					string[] info_prov = new string[2];

					SqlCommand cmd2 = new SqlCommand(sql, conn, trans);
					cmd2.CommandType = CommandType.Text;
					cmd2.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd2.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd2.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd2.Parameters.AddWithValue("@GLN_COMP", gln_comprador);

					SqlDataReader dr = cmd2.ExecuteReader();
					if (dr.HasRows)
					{
						dr.Read();
						info_prov[0] = dr[0].ToString().Trim();
						info_prov[1] = dr[1].ToString().Trim();
					}
					dr.Close();

					string sql2 = "if not exists" +
									"(" +
										"select " +
											"* " +
										"from " +
											"NoProcesados " +
										"where " +
											"np_numero_doc = @nrodoc and " +
											"np_nombre_doc = @nombdoc and " +
											"np_gln_proveedor = @prov and " +
											"np_gln_comprador = @comp and " +
											"np_gtin = @gtin" +
									 ") " +
								"begin " +
									"insert into " +
										"NoProcesados" +
										"(" +
											"np_numero_doc, " +
											"np_nombre_doc, " +
											"np_gln_proveedor, " +
											"np_gln_comprador, " +
											"np_gtin, " +
											"np_descripcion, " +
											"np_existe, " +
											"np_fecha" +
										") " +
									"values" +
									"(" +
										"@nrodoc, " +
										"@nombdoc, " +
										"@prov, " +
										"@comp, " +
										"@gtin, " +
										"@descripcion, " +
										"1, " +
										"getdate()" +
									") " +
								"end";

					FrmItemsNoEstan _FrmItemsNoEstan = new FrmItemsNoEstan(info_prov[0], info_prov[1], nombre_doc, numero_doc, true);
					_FrmItemsNoEstan.dgv_items.AutoGenerateColumns = false;
					SqlCommand cmd3 = new SqlCommand(sql2, conn, trans);
					cmd3.CommandType = CommandType.Text;

					for (int i = 0; i < estan.Count; i++)
					{
						string gtin = estan[i].Split('|')[0];
						string desc = estan[i].Split('|')[1];
						if (desc.Length > 40)
						{
							desc = desc.Substring(0, 40);
						}

						cmd3.Parameters.AddWithValue("@nrodoc", numero_doc);
						cmd3.Parameters.AddWithValue("@nombdoc", nombre_doc);
						cmd3.Parameters.AddWithValue("@prov", gln_proveedor);
						cmd3.Parameters.AddWithValue("@comp", gln_comprador);
						cmd3.Parameters.AddWithValue("@gtin", gtin);
						cmd3.Parameters.AddWithValue("@descripcion", desc);
						cmd3.ExecuteNonQuery();
						cmd3.Parameters.Clear();

						_FrmItemsNoEstan.dgv_items.Rows.Add(gtin, desc);
					}
					_FrmItemsNoEstan.ShowDialog();
				}

				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al guardar la información del item: " + ex.Message);
			}
		}

		public void GuardarInfoCotizacion(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador, IXLWorksheet lineas)
		{
			string separador = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;//Esta variable contiene el caracter que el sistema operativo usa para la separación de cifras decimales.
			string SQL = "SP_AgregarItemCotizacion";

			string GTIN = "GTIN";
			string GLN_COMPRADOR = "GLN COMPRADOR";
			string MONEDA = "MONEDA DEL PRECIO";
			string PRECIO = "PRECIO-1";
			string FECHA_INICIAL_PRECIO = "F. INICIAL PRECIO-1";
			string FECHA_FINAL_PRECIO = "F. FINAL PRECIO-1";
			string LEAD_TIME = "TIEMPO MINIMO DE ENTREGA";
			string PORC_DESCUENTO = "VALOR-1";
			string EVENTO_COMERCIAL = "NOMBRE DEL EVENTO COMERCIAL-1";
			string FECHA_INICIAL_EVENTO = "F.INICIAL DESCUENTO-1";
			string FECHA_FINAL_EVENTO = "F.FINAL DESCUENTO-1";
			string PORC_DESCUENTO2 = "VALOR-2";
			string EVENTO_COMERCIAL2 = "NOMBRE DEL EVENTO COMERCIAL-2";
			string FECHA_INICIAL_EVENTO2 = "F.INICIAL DESCUENTO-2";
			string FECHA_FINAL_EVENTO2 = "F.FINAL DESCUENTO-2";
			string PORC_DESCUENTO3 = "VALOR-3";
			string EVENTO_COMERCIAL3 = "NOMBRE DEL EVENTO COMERCIAL-3";
			string FECHA_INICIAL_EVENTO3 = "F.INICIAL DESCUENTO-3";
			string FECHA_FINAL_EVENTO3 = "F.FINAL DESCUENTO-3";

			int COLUMNA_GTIN = 0;
			int COLUMNA_GLN_COMPRADOR = 0;
			int COLUMNA_MONEDA = 0;
			int COLUMNA_PRECIO = 0;
			int COLUMNA_FECHA_INICIAL_PRECIO = 0;
			int COLUMNA_FECHA_FINAL_PRECIO = 0;
			int COLUMNA_LEAD_TIME = 0;
			int COLUMNA_PORC_DESCUENTO = 0;
			int COLUMNA_EVENTO_COMERCIAL = 0;
			int COLUMNA_FECHA_INICIAL_EVENTO = 0;
			int COLUMNA_FECHA_FINAL_EVENTO = 0;
			int COLUMNA_PORC_DESCUENTO2 = 0;
			int COLUMNA_EVENTO_COMERCIAL2 = 0;
			int COLUMNA_FECHA_INICIAL_EVENTO2 = 0;
			int COLUMNA_FECHA_FINAL_EVENTO2 = 0;
			int COLUMNA_PORC_DESCUENTO3 = 0;
			int COLUMNA_EVENTO_COMERCIAL3 = 0;
			int COLUMNA_FECHA_INICIAL_EVENTO3 = 0;
			int COLUMNA_FECHA_FINAL_EVENTO3 = 0;

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd = new SqlCommand(SQL, conn, trans);
				cmd.CommandType = CommandType.StoredProcedure;

				DataTable dt_unds_medida = ListarEquivalenciaUnidadesMedida();

				COLUMNA_GTIN = GetColumnIndex(GTIN, lineas);
				COLUMNA_GLN_COMPRADOR = GetColumnIndex(GLN_COMPRADOR, lineas);
				COLUMNA_MONEDA = GetColumnIndex(MONEDA, lineas);
				COLUMNA_PRECIO = GetColumnIndex(PRECIO, lineas);
				COLUMNA_FECHA_INICIAL_PRECIO = GetColumnIndex(FECHA_INICIAL_PRECIO, lineas);
				COLUMNA_FECHA_FINAL_PRECIO = GetColumnIndex(FECHA_FINAL_PRECIO, lineas);
				COLUMNA_LEAD_TIME = GetColumnIndex(LEAD_TIME, lineas);
				COLUMNA_PORC_DESCUENTO = GetColumnIndex(PORC_DESCUENTO, lineas);
				COLUMNA_EVENTO_COMERCIAL = GetColumnIndex(EVENTO_COMERCIAL, lineas);
				COLUMNA_FECHA_INICIAL_EVENTO = GetColumnIndex(FECHA_INICIAL_EVENTO, lineas);
				COLUMNA_FECHA_FINAL_EVENTO = GetColumnIndex(FECHA_FINAL_EVENTO, lineas);
				COLUMNA_PORC_DESCUENTO2 = GetColumnIndex(PORC_DESCUENTO2, lineas);
				COLUMNA_EVENTO_COMERCIAL2 = GetColumnIndex(EVENTO_COMERCIAL2, lineas);
				COLUMNA_FECHA_INICIAL_EVENTO2 = GetColumnIndex(FECHA_INICIAL_EVENTO2, lineas);
				COLUMNA_FECHA_FINAL_EVENTO2 = GetColumnIndex(FECHA_FINAL_EVENTO2, lineas);
				COLUMNA_PORC_DESCUENTO3 = GetColumnIndex(PORC_DESCUENTO3, lineas);
				COLUMNA_EVENTO_COMERCIAL3 = GetColumnIndex(EVENTO_COMERCIAL3, lineas);
				COLUMNA_FECHA_INICIAL_EVENTO3 = GetColumnIndex(FECHA_INICIAL_EVENTO3, lineas);
				COLUMNA_FECHA_FINAL_EVENTO3 = GetColumnIndex(FECHA_FINAL_EVENTO3, lineas);

				for (int i = 2; i < lineas.LastRowUsed().RowNumber() + 1; i++)
				{
					if (COLUMNA_GTIN > 0)
					{
						if (lineas.Cell(i, COLUMNA_GTIN).GetValue<string>().Length > 13)
						{
							continue;
						}
						string id = VerificarItemExiste(lineas.Cell(i, COLUMNA_GTIN).GetValue<string>());
						if (id.Equals(""))
						{
							if (gln_comprador.Trim().Equals(lineas.Cell(i, COLUMNA_GLN_COMPRADOR).GetValue<string>()))
							{
								cmd.Parameters.Clear();
								cmd.Parameters.AddWithValue("@NRO_DOC", numero_doc);
								cmd.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
								cmd.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
								cmd.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
								cmd.Parameters.AddWithValue("@GTIN", lineas.Cell(i, COLUMNA_GTIN).GetValue<string>());

								if (COLUMNA_MONEDA > 0)
								{
									cmd.Parameters.AddWithValue("@MONEDA", lineas.Cell(i, COLUMNA_MONEDA).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@MONEDA", "COP");
								}
								if (COLUMNA_PRECIO > 0)
								{
									decimal.TryParse(lineas.Cell(i, COLUMNA_PRECIO).GetValue<string>().Replace('.', ','), out decimal precio);
									cmd.Parameters.AddWithValue("@PRECIO", precio);
								}
								else
								{
									cmd.Parameters.AddWithValue("@PRECIO", 0);
								}
								if (COLUMNA_LEAD_TIME > 0)
								{
									cmd.Parameters.AddWithValue("@TMPO_ENT", lineas.Cell(i, COLUMNA_LEAD_TIME).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@TMPO_ENT", "1");
								}
								if (COLUMNA_PORC_DESCUENTO > 0)
								{
									if (!lineas.Cell(i, COLUMNA_PORC_DESCUENTO).GetValue<string>().Equals(string.Empty))
									{
										decimal.TryParse(lineas.Cell(i, COLUMNA_PORC_DESCUENTO).GetValue<string>().Replace('.', ','), out decimal porc_desc);
										cmd.Parameters.AddWithValue("@PORC_DESCTO", porc_desc);
										if (COLUMNA_EVENTO_COMERCIAL > 0)
										{
											cmd.Parameters.AddWithValue("@EVENTO_COM", lineas.Cell(i, COLUMNA_EVENTO_COMERCIAL).GetValue<string>());
										}
										else
										{
											cmd.Parameters.AddWithValue("@EVENTO_COM", DBNull.Value);
										}
										if (COLUMNA_FECHA_INICIAL_EVENTO > 0)
										{
											cmd.Parameters.AddWithValue("@FECHA_INI", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_INICIAL_EVENTO).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
										}
										else
										{
											cmd.Parameters.AddWithValue("@FECHA_INI", DBNull.Value);
										}
										if (COLUMNA_FECHA_FINAL_EVENTO > 0)
										{
											cmd.Parameters.AddWithValue("@FECHA_FIN", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_FINAL_EVENTO).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
										}
										else
										{
											cmd.Parameters.AddWithValue("@FECHA_FIN", DBNull.Value);
										}
									}
									else
									{
										cmd.Parameters.AddWithValue("@PORC_DESCTO", DBNull.Value);
										cmd.Parameters.AddWithValue("@EVENTO_COM", DBNull.Value);
										cmd.Parameters.AddWithValue("@FECHA_INI", DBNull.Value);
										cmd.Parameters.AddWithValue("@FECHA_FIN", DBNull.Value);
									}
								}
								else
								{
									cmd.Parameters.AddWithValue("@PORC_DESCTO", DBNull.Value);
									cmd.Parameters.AddWithValue("@EVENTO_COM", DBNull.Value);
									cmd.Parameters.AddWithValue("@FECHA_INI", DBNull.Value);
									cmd.Parameters.AddWithValue("@FECHA_FIN", DBNull.Value);
								}

								if (COLUMNA_PORC_DESCUENTO2 > 0)
								{
									if (!lineas.Cell(i, COLUMNA_PORC_DESCUENTO2).GetValue<string>().Equals(string.Empty))
									{
										decimal.TryParse(lineas.Cell(i, COLUMNA_PORC_DESCUENTO2).GetValue<string>().Replace('.', ','), out decimal porc_desc);
										cmd.Parameters.AddWithValue("@PORC_DESCTO2", porc_desc);
										if (COLUMNA_EVENTO_COMERCIAL2 > 0)
										{
											cmd.Parameters.AddWithValue("@EVENTO_COM2", lineas.Cell(i, COLUMNA_EVENTO_COMERCIAL2).GetValue<string>());
										}
										else
										{
											cmd.Parameters.AddWithValue("@EVENTO_COM2", DBNull.Value);
										}
										if (COLUMNA_FECHA_INICIAL_EVENTO2 > 0)
										{
											cmd.Parameters.AddWithValue("@FECHA_INI2", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_INICIAL_EVENTO2).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
										}
										else
										{
											cmd.Parameters.AddWithValue("@FECHA_INI2", DBNull.Value);
										}
										if (COLUMNA_FECHA_FINAL_EVENTO2 > 0)
										{
											cmd.Parameters.AddWithValue("@FECHA_FIN2", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_FINAL_EVENTO2).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
										}
										else
										{
											cmd.Parameters.AddWithValue("@FECHA_FIN2", DBNull.Value);
										}
									}
									else
									{
										cmd.Parameters.AddWithValue("@PORC_DESCTO2", DBNull.Value);
										cmd.Parameters.AddWithValue("@EVENTO_COM2", DBNull.Value);
										cmd.Parameters.AddWithValue("@FECHA_INI2", DBNull.Value);
										cmd.Parameters.AddWithValue("@FECHA_FIN2", DBNull.Value);
									}
								}
								else
								{
									cmd.Parameters.AddWithValue("@PORC_DESCTO2", DBNull.Value);
									cmd.Parameters.AddWithValue("@EVENTO_COM2", DBNull.Value);
									cmd.Parameters.AddWithValue("@FECHA_INI2", DBNull.Value);
									cmd.Parameters.AddWithValue("@FECHA_FIN2", DBNull.Value);
								}

								if (COLUMNA_PORC_DESCUENTO3 > 0)
								{
									if (!lineas.Cell(i, COLUMNA_PORC_DESCUENTO3).GetValue<string>().Equals(string.Empty))
									{
										decimal.TryParse(lineas.Cell(i, COLUMNA_PORC_DESCUENTO3).GetValue<string>().Replace('.', ','), out decimal porc_desc);
										cmd.Parameters.AddWithValue("@PORC_DESCTO3", porc_desc);
										if (COLUMNA_EVENTO_COMERCIAL3 > 0)
										{
											cmd.Parameters.AddWithValue("@EVENTO_COM3", lineas.Cell(i, COLUMNA_EVENTO_COMERCIAL3).GetValue<string>());
										}
										else
										{
											cmd.Parameters.AddWithValue("@EVENTO_COM3", DBNull.Value);
										}
										if (COLUMNA_FECHA_INICIAL_EVENTO3 > 0)
										{
											cmd.Parameters.AddWithValue("@FECHA_INI3", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_INICIAL_EVENTO3).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
										}
										else
										{
											cmd.Parameters.AddWithValue("@FECHA_INI3", DBNull.Value);
										}
										if (COLUMNA_FECHA_FINAL_EVENTO2 > 0)
										{
											cmd.Parameters.AddWithValue("@FECHA_FIN3", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_FINAL_EVENTO3).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
										}
										else
										{
											cmd.Parameters.AddWithValue("@FECHA_FIN3", DBNull.Value);
										}
									}
									else
									{
										cmd.Parameters.AddWithValue("@PORC_DESCTO3", DBNull.Value);
										cmd.Parameters.AddWithValue("@EVENTO_COM3", DBNull.Value);
										cmd.Parameters.AddWithValue("@FECHA_INI3", DBNull.Value);
										cmd.Parameters.AddWithValue("@FECHA_FIN3", DBNull.Value);
									}
								}
								else
								{
									cmd.Parameters.AddWithValue("@PORC_DESCTO3", DBNull.Value);
									cmd.Parameters.AddWithValue("@EVENTO_COM3", DBNull.Value);
									cmd.Parameters.AddWithValue("@FECHA_INI3", DBNull.Value);
									cmd.Parameters.AddWithValue("@FECHA_FIN3", DBNull.Value);
								}

								if (COLUMNA_FECHA_INICIAL_PRECIO > 0)
								{
									string fecha_ini = lineas.Cell(i, COLUMNA_FECHA_INICIAL_PRECIO).GetValue<string>();
									if (fecha_ini.Equals(""))
										cmd.Parameters.AddWithValue("@FECHA_ACT", DBNull.Value);
									else
										cmd.Parameters.AddWithValue("@FECHA_ACT", Convert.ToDateTime(fecha_ini, CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
								}
								else
								{
									cmd.Parameters.AddWithValue("@FECHA_ACT", DBNull.Value);
								}
								if (COLUMNA_FECHA_FINAL_PRECIO > 0)
								{
									string fecha_fin = lineas.Cell(i, COLUMNA_FECHA_FINAL_PRECIO).GetValue<string>();
									if (fecha_fin.Equals(""))
										cmd.Parameters.AddWithValue("@FECHA_HASTA", DBNull.Value);
									else
										cmd.Parameters.AddWithValue("@FECHA_HASTA", Convert.ToDateTime(fecha_fin, CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
								}
								else
								{
									cmd.Parameters.AddWithValue("@FECHA_HASTA", DBNull.Value);
								}

								cmd.ExecuteNonQuery();

								cmd.Parameters.Clear();
							}
						}
					}
				}
				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al guardar la información de item cotización: " + ex.Message);
			}
		}

		/// <summary>
		/// Guarda la información de la descripción tecnica del item.
		/// </summary>
		/// <param name="numero_doc">Numero del documento.</param>
		/// <param name="nombre_doc">Nombre del documento.</param>
		/// <param name="gln_proveedor">Gln proveedor.</param>
		/// <param name="gln_comprador">Gln comprador.</param>
		/// <param name="lineas">hoja que contiene la niformación de los items.</param>
		public void GuardarInfoDescripcionTecnica(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador, IXLWorksheet lineas)
		{
			string separador = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;//Esta variable contiene el caracter que el sistema operativo usa para la separación de cifras decimales.
			string SQL = "SP_AgregarDescripcionTecnica";

			string GTIN = "GTIN";// "PRODUCTGTIN";
			string GLN_COMPRADOR = "GLN COMPRADOR";
			string ALTO = "ALTO";
			string ANCHO = "ANCHO";
			string PROFUNDO = "PROFUNDO";
			string UND_MEDIDA_DIMENSION = "UNIDAD DE MEDIDA DIMENSIÓN";

			string GTIN_CONTENIDO = "GTIN CONTENIDO";

			int COLUMNA_GTIN = 0;
			int COLUMNA_GLN_COMPRADOR = 0;
			int COLUMNA_ALTO = 0;
			int COLUMNA_ANCHO = 0;
			int COLUMNA_PROFUNDO = 0;
			int COLUMNA_UND_MEDIDA_DIMENSION;

			int COLUMNA_GTIN_CONTENIDO = 0;

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd = new SqlCommand(SQL, conn, trans);
				cmd.CommandType = CommandType.StoredProcedure;

				COLUMNA_GTIN = GetColumnIndex(GTIN, lineas);
				COLUMNA_GLN_COMPRADOR = GetColumnIndex(GLN_COMPRADOR, lineas);
				COLUMNA_ALTO = GetColumnIndex(ALTO, lineas);
				COLUMNA_ANCHO = GetColumnIndex(ANCHO, lineas);
				COLUMNA_PROFUNDO = GetColumnIndex(PROFUNDO, lineas);
				COLUMNA_UND_MEDIDA_DIMENSION = GetColumnIndex(UND_MEDIDA_DIMENSION, lineas);

				COLUMNA_GTIN_CONTENIDO = GetColumnIndex(GTIN_CONTENIDO, lineas);

				for (int i = 2; i < lineas.LastRowUsed().RowNumber() + 1; i++)
				{
					if (COLUMNA_GTIN > 0)
					{
						if (lineas.Cell(i, COLUMNA_GTIN).GetValue<string>().Length > 13)
						{
							continue;
						}
						string id = VerificarItemExiste(lineas.Cell(i, COLUMNA_GTIN).GetValue<string>());
						if (id.Equals(""))
						{
							if (gln_comprador.Trim().Equals(lineas.Cell(i, COLUMNA_GLN_COMPRADOR).GetValue<string>()))
							{
								cmd.Parameters.AddWithValue("@NRO_DOC", numero_doc);
								cmd.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
								cmd.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
								cmd.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
								cmd.Parameters.AddWithValue("@GTIN", lineas.Cell(i, COLUMNA_GTIN).GetValue<string>());
								if (COLUMNA_UND_MEDIDA_DIMENSION > 0)
								{
									if (COLUMNA_ALTO > 0)
									{
										decimal.TryParse(lineas.Cell(i, COLUMNA_ALTO).GetValue<string>().Replace('.', ','), out decimal alto);
										if (lineas.Cell(i, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>()/*.Split('-')[0]*/.Equals("MMT"))
										{
											cmd.Parameters.AddWithValue("@ALTO", alto / 10);//SE DIVIDE ENTRE 10 PARA CONVERTIR MILIMETROS EN CENTIMETROS
										}
										else if (lineas.Cell(i, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>()/*.Split('-')[0]*/.Equals("MTR"))
										{
											cmd.Parameters.AddWithValue("@ALTO", alto * 100);//SE MUTIPLICA POR 100 PARA CONVERTIR METROS EN CENTIMETROS
										}
										else if (lineas.Cell(i, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>()/*.Split('-')[0]*/.Equals("CMT"))
										{
											cmd.Parameters.AddWithValue("@ALTO", alto);//CENTIMETROS
										}
										else
										{
											cmd.Parameters.AddWithValue("@ALTO", DBNull.Value);
										}
									}
									else
									{
										cmd.Parameters.AddWithValue("@ALTO", DBNull.Value);
									}
									if (COLUMNA_ANCHO > 0)
									{
										decimal.TryParse(lineas.Cell(i, COLUMNA_ANCHO).GetValue<string>().Replace('.', ','), out decimal ancho);
										if (lineas.Cell(i, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>()/*.Split('-')[0]*/.Equals("MMT"))
										{
											cmd.Parameters.AddWithValue("@ANCHO", ancho / 10);//SE DIVIDE ENTRE 10 PARA CONVERTIR MILIMETROS EN CENTIMETROS
										}
										else if (lineas.Cell(i, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>()/*.Split('-')[0]*/.Equals("MTR"))
										{
											cmd.Parameters.AddWithValue("@ANCHO", ancho * 100);//SE MUTIPLICA POR 100 PARA CONVERTIR METROS EN CENTIMETROS
										}
										else if (lineas.Cell(i, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>()/*.Split('-')[0]*/.Equals("CMT"))
										{
											cmd.Parameters.AddWithValue("@ANCHO", ancho);//CENTIMETROS
										}
										else
										{
											cmd.Parameters.AddWithValue("@ANCHO", DBNull.Value);//CENTIMETROS
										}
									}
									else
									{
										cmd.Parameters.AddWithValue("@ANCHO", DBNull.Value);
									}
									if (COLUMNA_PROFUNDO > 0)
									{
										decimal.TryParse(lineas.Cell(i, COLUMNA_PROFUNDO).GetValue<string>().Replace('.', ','), out decimal profundo);
										if (lineas.Cell(i, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>()/*.Split('-')[0]*/.Equals("MMT"))
										{
											cmd.Parameters.AddWithValue("@PROFUNDO", profundo / 10);//SE DIVIDE ENTRE 10 PARA CONVERTIR MILIMETROS EN CENTIMETROS
										}
										else if (lineas.Cell(i, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>()/*.Split('-')[0]*/.Equals("MTR"))
										{
											cmd.Parameters.AddWithValue("@PROFUNDO", profundo * 100);//SE MUTIPLICA POR 100 PARA CONVERTIR METROS EN CENTIMETROS
										}
										else if (lineas.Cell(i, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>()/*.Split('-')[0]*/.Equals("CMT"))
										{
											cmd.Parameters.AddWithValue("@PROFUNDO", profundo);//CENTIMETROS
										}
										else
										{
											cmd.Parameters.AddWithValue("@PROFUNDO", DBNull.Value);//CENTIMETROS
										}
									}
									else
									{
										cmd.Parameters.AddWithValue("@PROFUNDO", DBNull.Value);
									}

									decimal alto_emp = 0;
									decimal ancho_emp = 0;
									decimal profundo_emp = 0;
									if (COLUMNA_GTIN_CONTENIDO > 0)
									{
										for (int j = 2; j < lineas.LastRowUsed().RowNumber() + 1; j++)
										{
											alto_emp = 0;
											ancho_emp = 0;
											profundo_emp = 0;
											//COLUMNA_GTIN_CONTENIDO
											if (lineas.Cell(i, COLUMNA_GTIN).GetValue<string>().Trim().Equals(lineas.Cell(j, COLUMNA_GTIN_CONTENIDO).GetValue<string>().Trim()))
											{
												if (COLUMNA_UND_MEDIDA_DIMENSION > 0)
												{
													if (COLUMNA_ALTO > 0)
													{
														decimal.TryParse(lineas.Cell(j, COLUMNA_ALTO).GetValue<string>().Replace('.', ','), out alto_emp);
														if (lineas.Cell(j, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>().Split('-')[0].Equals("MMT"))
														{
															alto_emp = alto_emp / 10;//SE DIVIDE ENTRE 10 PARA CONVERTIR MILIMETROS EN CENTIMETROS
														}
														if (lineas.Cell(j, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>().Split('-')[0].Equals("MTR"))
														{
															alto_emp = alto_emp * 100;//SE MUTIPLICA POR 100 PARA CONVERTIR METROS EN CENTIMETROS
														}
														//if (lineas.Cell(j, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>().Split('-')[0].Equals("CMT"))
														//{
														//    alto_emp;//CENTIMETROS
														//}
													}

													if (COLUMNA_ANCHO > 0)
													{
														decimal.TryParse(lineas.Cell(j, COLUMNA_ANCHO).GetValue<string>().Replace('.', ','), out ancho_emp);
														if (lineas.Cell(j, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>().Split('-')[0].Equals("MMT"))
														{
															ancho_emp = ancho_emp / 10;//SE DIVIDE ENTRE 10 PARA CONVERTIR MILIMETROS EN CENTIMETROS
														}
														if (lineas.Cell(j, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>().Split('-')[0].Equals("MTR"))
														{
															ancho_emp = ancho_emp * 100;//SE MUTIPLICA POR 100 PARA CONVERTIR METROS EN CENTIMETROS
														}
														//if (lineas.Cell(j, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>().Split('-')[0].Equals("CMT"))
														//{
														//    ancho_emp = ancho_emp;//CENTIMETROS
														//}
													}

													if (COLUMNA_PROFUNDO > 0)
													{
														decimal.TryParse(lineas.Cell(j, COLUMNA_PROFUNDO).GetValue<string>().Replace('.', ','), out profundo_emp);
														if (lineas.Cell(j, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>().Split('-')[0].Equals("MMT"))
														{
															profundo_emp = profundo_emp / 10;//SE DIVIDE ENTRE 10 PARA CONVERTIR MILIMETROS EN CENTIMETROS
														}
														if (lineas.Cell(j, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>().Split('-')[0].Equals("MTR"))
														{
															profundo_emp = profundo_emp * 100;//SE MUTIPLICA POR 100 PARA CONVERTIR METROS EN CENTIMETROS
														}
														//if (lineas.Cell(j, COLUMNA_UND_MEDIDA_DIMENSION).GetValue<string>().Split('-')[0].Equals("CMT"))
														//{
														//    profundo_emp = profundo_emp;//CENTIMETROS
														//}
													}
												}
												break;
											}
										}
									}
									if (alto_emp > 0)
									{
										cmd.Parameters.AddWithValue("@ALTO_EMP", alto_emp);
									}
									else
									{
										cmd.Parameters.AddWithValue("@ALTO_EMP", DBNull.Value);
									}
									if (ancho_emp > 0)
									{
										cmd.Parameters.AddWithValue("@ANCHO_EMP", ancho_emp);
									}
									else
									{
										cmd.Parameters.AddWithValue("@ANCHO_EMP", DBNull.Value);
									}
									if (profundo_emp > 0)
									{
										cmd.Parameters.AddWithValue("@PROFUNDO_EMP", profundo_emp);
									}
									else
									{
										cmd.Parameters.AddWithValue("@PROFUNDO_EMP", DBNull.Value);
									}
								}
								else
								{
									cmd.Parameters.AddWithValue("@ALTO", DBNull.Value);
									cmd.Parameters.AddWithValue("@ANCHO", DBNull.Value);
									cmd.Parameters.AddWithValue("@PROFUNDO", DBNull.Value);

									cmd.Parameters.AddWithValue("@ALTO_EMP", DBNull.Value);
									cmd.Parameters.AddWithValue("@ANCHO_EMP", DBNull.Value);
									cmd.Parameters.AddWithValue("@PROFUNDO_EMP", DBNull.Value);
								}
								cmd.ExecuteNonQuery();

								cmd.Parameters.Clear();
							}
						}
					}
				}
				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al guardar la descripción técnica del item: " + ex.Message);
			}
		}

		public void GuardarInfoCodigoBarras(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador, IXLWorksheet lineas)
		{
			string SQL = "IF NOT EXISTS" +
						"(" +
							"SELECT " +
								"* " +
							"FROM " +
								"CodigoBarras " +
								 "INNER JOIN Documentos ON do_numero_doc=br_numero_doc AND do_nombre_doc=br_nombre_doc " +
														  "AND do_gln_proveedor=br_gln_proveedor AND do_gln_comprador=br_gln_comprador " +
						   "WHERE " +
								"do_numero_doc=@NRO_DOC " +
								"AND do_nombre_doc=@NOMB_DOC " +
								"AND do_gln_proveedor=@GLN_PROV " +
								"AND do_gln_comprador=@GLN_COMP " +
								"AND br_gtin=@GTIN" +
						 ") " +
						"BEGIN " +
							"INSERT INTO " +
								"CodigoBarras " +
								"(" +
									"br_numero_doc, " +
									"br_nombre_doc, " +
									"br_gln_proveedor, " +
									"br_gln_comprador, " +
									"br_gtin, " +
									"br_barra" +
								") " +
							"VALUES" +
								"(" +
									"@NRO_DOC, " +
									"@NOMB_DOC, " +
									"@GLN_PROV, " +
									"@GLN_COMP, " +
									"@GTIN, " +
									"@BARRA" +
								 ") " +
						"END";

			string GTIN = "GTIN";// "PRODUCTGTIN";
			string GLN_COMPRADOR = "GLN COMPRADOR";

			int COLUMNA_GTIN = 0;
			int COLUMNA_GLN_COMPRADOR = 0;

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd = new SqlCommand(SQL, conn, trans);
				cmd.CommandType = CommandType.Text;

				COLUMNA_GTIN = GetColumnIndex(GTIN, lineas);
				COLUMNA_GLN_COMPRADOR = GetColumnIndex(GLN_COMPRADOR, lineas);

				for (int i = 2; i < lineas.LastRowUsed().RowNumber() + 1; i++)
				{
					if (COLUMNA_GTIN > 0)
					{
						if (lineas.Cell(i, COLUMNA_GTIN).GetValue<string>().Length > 13)
						{
							continue;
						}
						string id = VerificarItemExiste(lineas.Cell(i, COLUMNA_GTIN).GetValue<string>().Trim());
						if (id.Equals(""))
						{
							if (gln_comprador.Trim().Equals(lineas.Cell(i, COLUMNA_GLN_COMPRADOR).GetValue<string>()))
							{
								cmd.Parameters.AddWithValue("@NRO_DOC", numero_doc);
								cmd.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
								cmd.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
								cmd.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
								cmd.Parameters.AddWithValue("@GTIN", lineas.Cell(i, COLUMNA_GTIN).GetValue<string>());
								cmd.Parameters.AddWithValue("@BARRA", lineas.Cell(i, COLUMNA_GTIN).GetValue<string>());

								cmd.ExecuteNonQuery();

								cmd.Parameters.Clear();
							}
						}
					}
				}

				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al guardar la descripción técnica del item: " + ex.Message);
			}
		}

		public void GuardarInfoOtrosDatos(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador, IXLWorksheet lineas)
		{
			string SQL = "IF NOT EXISTS" +
						"(" +
							"SELECT " +
								"* " +
							"FROM " +
								"OtrosDatos " +
								 "INNER JOIN Documentos ON do_numero_doc=ot_numero_doc AND do_nombre_doc=ot_nombre_doc " +
														  "AND do_gln_proveedor=ot_gln_proveedor AND do_gln_comprador=ot_gln_comprador " +
						   "WHERE " +
								"do_numero_doc=@NRO_DOC " +
								"AND do_nombre_doc=@NOMB_DOC " +
								"AND do_gln_proveedor=@GLN_PROV " +
								"AND do_gln_comprador=@GLN_COMP " +
								"AND ot_gtin=@GTIN" +
						 ") " +
						"BEGIN " +
							"INSERT INTO " +
								"OtrosDatos " +
								"(" +
									"ot_numero_doc, " +
									"ot_nombre_doc, " +
									"ot_gln_proveedor, " +
									"ot_gln_comprador, " +
									"ot_gtin, " +
									"ot_nombre_comercial, " +
									"ot_imagen, " +
									"ot_marca, " +
									"ot_fabricante, " +
									"ot_registro_sanitario, " +
									"ot_tipo_producto, " +
									"ot_linea, " +
									"ot_fragancia, " +
									"ot_sabor, " +
									"ot_recomendaciones, " +
									"ot_advertencia, " +
									"ot_precauciones, " +
									"ot_ficha_tecnica, " +
									"ot_tipo_empaque, " +
									"ot_multiplos_despacho, " +
									"ot_proveedor, " +
									"ot_cant_contenida, " +
									"ot_unds_embalaje, " +
									"ot_sublinea, " +
									"ot_calificador_cantidad_contenida, " +
									"ot_id_calificador_cantidad_contenida" +
								") " +
							"VALUES" +
								"(" +
									"@NRO_DOC, " +
									"@NOMB_DOC, " +
									"@GLN_PROV, " +
									"@GLN_COMP, " +
									"@GTIN, " +
									"@NOMB_COM, " +
									"@IMG, " +
									"@MARCA, " +
									"@FABRICANTE, " +
									"@REG_SANITARIO, " +
									"@TIPO_PROD, " +
									"@LINEA, " +
									"@FRAGANCIA, " +
									"@SABOR, " +
									"@RECOMENDACIONES, " +
									"@ADVERTENCIA, " +
									"@PRECAUCIONES, " +
									"@FICHA_TECNICA, " +
									"@TIPO_EMPAQUE, " +
									"@MULTIPLOS_DESPACHO, " +
									"@PROVEEDOR, " +
									"@CANT_CONTENIDA, " +
									"@UNDS_EMBALAJE, " +
									"@SUBLINEA," +
									"@CALIF_CANT_CONTENIDA, " +
									"@ID_CALIF_CANT_CONTENIDA" +
								 ") " +
						"END";

			string GTIN = "GTIN";
			string GLN_COMPRADOR = "GLN COMPRADOR";
			string NOMB_COM = "NOMBRE COMERCIAL DEL PRODUCTO";
			string IMAGEM = "URLIMAGEN";
			string MARCA = "MARCA DEL PRODUCTO";
			string FABRICANTE = "FABRICANTE";
			string REG_SAN = "REGISTRO SANITARIO";
			string TIPO_PROD = "TIPOPRODUCTO";
			string LINEA = "LINEA";
			string FRAGANCIA = "FRAGANCIA";
			string SABOR = "SABOR";
			string RECOMENDACIONES = "RECOMENDACIONES";
			string ADVERTENCIA = "ADVERTENCIA";
			string PRECAUCIONES = "PRECAUCIONES";
			string FICHA_TECNICA = "FICHA TECNICA";

			string TIPO_DE_EMPAQUE = "TIPO DE EMPAQUE";
			string MULTIPLOS_DE_DESPACHO = "MULTIPLOS DE DESPACHO";
			string PROVEEDOR = "PROVEEDOR";
			string CANTIDAD_CONTENIDA = "CANTIDAD CONTENIDA";
			string CALIFICADOR_CANTIDAD_CONTENIDA = "CALIFICADOR CANTIDAD CONTENIDA";
			string UNIDADES_EN_EMBALAJE = "UNIDADES EN EL EMBALAJE";
			string SUBLINEA = "SUBLINEA	";

			int COLUMNA_GTIN = 0;
			int COLUMNA_GLN_COMPRADOR = 0;
			int COLUMNA_NOMB_COM = 0;
			int COLUMNA_IMAGEM = 0;
			int COLUMNA_MARCA = 0;
			int COLUMNA_FABRICANTE = 0;
			int COLUMNA_REG_SAN = 0;
			int COLUMNA_TIPO_PROD = 0;
			int COLUMNA_LINEA = 0;
			int COLUMNA_FRAGANCIA = 0;
			int COLUMNA_SABOR = 0;
			int COLUMNA_RECOMENDACIONES = 0;
			int COLUMNA_ADVERTENCIA = 0;
			int COLUMNA_PRECAUCIONES = 0;
			int COLUMNA_FICHA_TECNICA = 0;

			int COLUMNA_TIPO_DE_EMPAQUE = 0;
			int COLUMNA_MULTIPLOS_DE_DESPACHO = 0;
			int COLUMNA_PROVEEDOR = 0;
			int COLUMNA_CANTIDAD_CONTENIDA = 0;
			int COLUMNA_CALIFICADOR_CANTIDAD_CONTENIDA = 0;
			int COLUMNA_UNIDADES_EN_EMBALAJE = 0;
			int COLUMNA_SUBLINEA = 0;

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd = new SqlCommand(SQL, conn, trans);
				cmd.CommandType = CommandType.Text;

				COLUMNA_GTIN = GetColumnIndex(GTIN, lineas);
				COLUMNA_GLN_COMPRADOR = GetColumnIndex(GLN_COMPRADOR, lineas);

				COLUMNA_NOMB_COM = GetColumnIndex(NOMB_COM, lineas);
				COLUMNA_IMAGEM = GetColumnIndex(IMAGEM, lineas);
				COLUMNA_MARCA = GetColumnIndex(MARCA, lineas);
				COLUMNA_FABRICANTE = GetColumnIndex(FABRICANTE, lineas);
				COLUMNA_REG_SAN = GetColumnIndex(REG_SAN, lineas);
				COLUMNA_TIPO_PROD = GetColumnIndex(TIPO_PROD, lineas);
				COLUMNA_LINEA = GetColumnIndex(LINEA, lineas);
				COLUMNA_FRAGANCIA = GetColumnIndex(FRAGANCIA, lineas);
				COLUMNA_SABOR = GetColumnIndex(SABOR, lineas);
				COLUMNA_RECOMENDACIONES = GetColumnIndex(RECOMENDACIONES, lineas);
				COLUMNA_ADVERTENCIA = GetColumnIndex(ADVERTENCIA, lineas);
				COLUMNA_PRECAUCIONES = GetColumnIndex(PRECAUCIONES, lineas);
				COLUMNA_FICHA_TECNICA = GetColumnIndex(FICHA_TECNICA, lineas);

				COLUMNA_TIPO_DE_EMPAQUE = GetColumnIndex(TIPO_DE_EMPAQUE, lineas);
				COLUMNA_MULTIPLOS_DE_DESPACHO = GetColumnIndex(MULTIPLOS_DE_DESPACHO, lineas);
				COLUMNA_PROVEEDOR = GetColumnIndex(PROVEEDOR, lineas);
				COLUMNA_CANTIDAD_CONTENIDA = GetColumnIndex(CANTIDAD_CONTENIDA, lineas);
				COLUMNA_CALIFICADOR_CANTIDAD_CONTENIDA = GetColumnIndex(CALIFICADOR_CANTIDAD_CONTENIDA, lineas);
				COLUMNA_UNIDADES_EN_EMBALAJE = GetColumnIndex(UNIDADES_EN_EMBALAJE, lineas);
				COLUMNA_SUBLINEA = GetColumnIndex(SUBLINEA, lineas);

				for (int i = 2; i < lineas.LastRowUsed().RowNumber() + 1; i++)
				{
					if (COLUMNA_GTIN > 0)
					{
						if (lineas.Cell(i, COLUMNA_GTIN).GetValue<string>().Length > 13)
						{
							continue;
						}
						string id = VerificarItemExiste(lineas.Cell(i, COLUMNA_GTIN).GetValue<string>().Trim());
						if (id.Equals(""))
						{
							if (gln_comprador.Trim().Equals(lineas.Cell(i, COLUMNA_GLN_COMPRADOR).GetValue<string>()))
							{
								cmd.Parameters.AddWithValue("@NRO_DOC", numero_doc);
								cmd.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
								cmd.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
								cmd.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
								cmd.Parameters.AddWithValue("@GTIN", lineas.Cell(i, COLUMNA_GTIN).GetValue<string>());
								if (COLUMNA_NOMB_COM > 0)
								{
									cmd.Parameters.AddWithValue("@NOMB_COM", lineas.Cell(i, COLUMNA_NOMB_COM).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@NOMB_COM", DBNull.Value);
								}
								if (COLUMNA_IMAGEM > 0)
								{
									cmd.Parameters.AddWithValue("@IMG", lineas.Cell(i, COLUMNA_IMAGEM).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@IMG", DBNull.Value);
								}
								if (COLUMNA_MARCA > 0)
								{
									cmd.Parameters.AddWithValue("@MARCA", lineas.Cell(i, COLUMNA_MARCA).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@MARCA", DBNull.Value);
								}
								if (COLUMNA_FABRICANTE > 0)
								{
									cmd.Parameters.AddWithValue("@FABRICANTE", lineas.Cell(i, COLUMNA_FABRICANTE).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@FABRICANTE", DBNull.Value);
								}
								if (COLUMNA_REG_SAN > 0)
								{
									cmd.Parameters.AddWithValue("@REG_SANITARIO", lineas.Cell(i, COLUMNA_REG_SAN).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@REG_SANITARIO", DBNull.Value);
								}
								if (COLUMNA_TIPO_PROD > 0)
								{
									cmd.Parameters.AddWithValue("@TIPO_PROD", lineas.Cell(i, COLUMNA_TIPO_PROD).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@TIPO_PROD", DBNull.Value);
								}
								if (COLUMNA_LINEA > 0)
								{
									cmd.Parameters.AddWithValue("@LINEA", lineas.Cell(i, COLUMNA_LINEA).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@LINEA", DBNull.Value);
								}
								if (COLUMNA_FRAGANCIA > 0)
								{
									cmd.Parameters.AddWithValue("@FRAGANCIA", lineas.Cell(i, COLUMNA_FRAGANCIA).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@FRAGANCIA", DBNull.Value);
								}
								if (COLUMNA_SABOR > 0)
								{
									cmd.Parameters.AddWithValue("@SABOR", lineas.Cell(i, COLUMNA_SABOR).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@SABOR", DBNull.Value);
								}
								if (COLUMNA_RECOMENDACIONES > 0)
								{
									cmd.Parameters.AddWithValue("@RECOMENDACIONES", lineas.Cell(i, COLUMNA_RECOMENDACIONES).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@RECOMENDACIONES", DBNull.Value);
								}
								if (COLUMNA_ADVERTENCIA > 0)
								{
									cmd.Parameters.AddWithValue("@ADVERTENCIA", lineas.Cell(i, COLUMNA_ADVERTENCIA).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@ADVERTENCIA", DBNull.Value);
								}
								if (COLUMNA_PRECAUCIONES > 0)
								{
									cmd.Parameters.AddWithValue("@PRECAUCIONES", lineas.Cell(i, COLUMNA_PRECAUCIONES).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@PRECAUCIONES", DBNull.Value);
								}
								if (COLUMNA_FICHA_TECNICA > 0)
								{
									cmd.Parameters.AddWithValue("@FICHA_TECNICA", lineas.Cell(i, COLUMNA_FICHA_TECNICA).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@FICHA_TECNICA", DBNull.Value);
								}

								//
								if (COLUMNA_TIPO_DE_EMPAQUE > 0)
								{
									cmd.Parameters.AddWithValue("@TIPO_EMPAQUE", lineas.Cell(i, COLUMNA_TIPO_DE_EMPAQUE).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@TIPO_EMPAQUE", DBNull.Value);
								}
								if (COLUMNA_MULTIPLOS_DE_DESPACHO > 0)
								{
									cmd.Parameters.AddWithValue("@MULTIPLOS_DESPACHO", lineas.Cell(i, COLUMNA_MULTIPLOS_DE_DESPACHO).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@MULTIPLOS_DESPACHO", DBNull.Value);
								}
								if (COLUMNA_PROVEEDOR > 0)
								{
									cmd.Parameters.AddWithValue("@PROVEEDOR", lineas.Cell(i, COLUMNA_PROVEEDOR).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@PROVEEDOR", DBNull.Value);
								}
								if (COLUMNA_CANTIDAD_CONTENIDA > 0)
								{
									cmd.Parameters.AddWithValue("@CANT_CONTENIDA", lineas.Cell(i, COLUMNA_CANTIDAD_CONTENIDA).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@CANT_CONTENIDA", DBNull.Value);
								}

								if (COLUMNA_CALIFICADOR_CANTIDAD_CONTENIDA > 0)
								{
									DataTable dt_unds_medida = ListarEquivalenciaUnidadesMedida();
									string calif_cant_contenida = "";
									string und_logyca = lineas.Cell(i, COLUMNA_CALIFICADOR_CANTIDAD_CONTENIDA).GetValue<string>().Trim();
									foreach (DataRow und in dt_unds_medida.Rows)
									{
										if (und[2].Equals(und_logyca))
										{
											calif_cant_contenida = und[1].ToString();
											break;
										}
									}
									cmd.Parameters.AddWithValue("@CALIF_CANT_CONTENIDA", calif_cant_contenida);

									Datos datos = new Datos();
									string id_und = datos.ObtenerIdUnidadMedida(calif_cant_contenida);
									cmd.Parameters.AddWithValue("@ID_CALIF_CANT_CONTENIDA", id_und);
								}
								else
								{
									cmd.Parameters.AddWithValue("@CALIF_CANT_CONTENIDA", DBNull.Value);
								}

								if (COLUMNA_UNIDADES_EN_EMBALAJE > 0)
								{
									cmd.Parameters.AddWithValue("@UNDS_EMBALAJE", lineas.Cell(i, COLUMNA_UNIDADES_EN_EMBALAJE).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@UNDS_EMBALAJE", DBNull.Value);
								}
								if (COLUMNA_SUBLINEA > 0)
								{
									cmd.Parameters.AddWithValue("@SUBLINEA", lineas.Cell(i, COLUMNA_SUBLINEA).GetValue<string>());
								}
								else
								{
									cmd.Parameters.AddWithValue("@SUBLINEA", DBNull.Value);
								}
								//
								cmd.ExecuteNonQuery();

								cmd.Parameters.Clear();
							}
						}
					}
				}

				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al guardar los otros datos: " + ex.Message);
			}
		}

		private string[] ObtenerUnidadMedidaTmpoEntrega(string referencia)
		{
			string SQL = "select " +
							"f212_id_um, " +
							"f212_tiempo_entrega, " +
							"f120_descripcion, " +
							"f120_id, " +
							"f120_ind_compra, " +
							"isnull(convert(decimal(18,2),f037_tasa),0) f037_tasa " +
						  "from " +
							"t120_mc_items " +
							"inner join t121_mc_items_extensiones on f120_rowid = f121_rowid_item " +
								"and f120_id_cia = f121_id_cia " +
							"inner join t131_mc_items_barras on f121_rowid = f131_rowid_item_ext " +
								"and f121_id_cia = f131_id_cia " +
							"inner join t212_mm_cotizaciones on f212_rowid_item_ext = f131_rowid_item_ext " +
								"and f131_id_cia = f212_id_cia " +
							"inner join t113_mc_grupos_impositivos on f113_id = f120_id_grupo_impositivo " +
								"and f113_id_cia = f120_id_cia " +
							"left join t114_mc_grupos_impo_impuestos on f114_grupo_impositivo = f113_id " +
								"and f114_id_cia = f113_id_cia " +
							"left join t037_mm_llaves_impuesto on f037_id = f114_id_llave_impuesto " +
								"and f114_id_cia = f037_id_cia " +
						"where " +
							"f131_id = @REF " +
							"and f120_id_cia = 1 " +
							"and f121_ind_estado = 1 " +
							"and f212_fecha_activacion = ( " +
									"select " +
										"max(f212_fecha_activacion) " +
									"from " +
										"t212_mm_cotizaciones " +
									"where " +
										"f212_rowid_item_ext = f121_rowid and f121_id_cia = 1)";
			string[] res = null;

			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@REF", referencia);

				SqlDataReader dr = cmd.ExecuteReader();
				if (dr.HasRows)
				{
					res = new string[6];
					dr.Read();
					res[0] = dr.GetString(0);
					res[1] = Convert.ToString(dr.GetInt16(1));
					res[2] = dr.GetString(2);
					res[3] = Convert.ToString(dr.GetInt32(3));
					res[4] = Convert.ToString(dr.GetInt16(4));
					res[5] = Convert.ToString(dr.GetDecimal(5));
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener la unidad de medida y el tiempo de entrega: " + ex.Message);
			}
			return res;
		}

		private string VerificarItemExiste(string referencia)
		{
			string SQL = "select " +
							"f120_referencia " +
						 "from " +
							"t120_mc_items " +
						 "where " +
							"f120_referencia=@REF and " +
						 "f120_id_cia=1";
			string res = "";

			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@REF", referencia);
				res = Convert.ToString(cmd.ExecuteScalar());
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al verificar item existe: " + ex.Message);
			}
			return res;
		}

		public void GuardarInfoCambioPrecio(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador, IXLWorksheet lineas)
		{
			string SQL = "SP_AgregarItemCambioPrecio";

			string GTIN = "GTIN";
			string DESCRIPCION_LARGA = "DESCRIPCIÓN LARGA";
			string GLN_COMPRADOR = "GLN COMPRADOR";
			string MONEDA = "MONEDA DEL PRECIO";
			string PRECIO = "PRECIO-1";
			string FECHA_INICIAL_PRECIO = "F. INICIAL PRECIO-1";
			string FECHA_FINAL_PRECIO = "F. FINAL PRECIO-1";
			string EVENTO_COMERCIAL = "NOMBRE DEL EVENTO COMERCIAL-1";
			string PORC_DESCUENTO = "VALOR-1";
			string FECHA_INICIAL_EVENTO = "F.INICIAL DESCUENTO-1";
			string FECHA_FINAL_EVENTO = "F.FINAL DESCUENTO-1";
			string EVENTO_COMERCIAL2 = "NOMBRE DEL EVENTO COMERCIAL-2";
			string PORC_DESCUENTO2 = "VALOR-2";
			string FECHA_INICIAL_EVENTO2 = "F.INICIAL DESCUENTO-2";
			string FECHA_FINAL_EVENTO2 = "F.FINAL DESCUENTO-2";
			string EVENTO_COMERCIAL3 = "NOMBRE DEL EVENTO COMERCIAL-2";
			string PORC_DESCUENTO3 = "VALOR-3";
			string FECHA_INICIAL_EVENTO3 = "F.INICIAL DESCUENTO-3";
			string FECHA_FINAL_EVENTO3 = "F.FINAL DESCUENTO-3";
			string CATEGORIA_LOGYCA = "CATEGORIA LOGYCA";

			int COLUMNA_GTIN = 0;
			int COLUMNA_DESCRIPCION_LARGA = 0;
			int COLUMNA_GLN_COMPRADOR = 0;
			int COLUMNA_MONEDA = 0;
			int COLUMNA_PRECIO = 0;
			int COLUMNA_FECHA_INICIAL_PRECIO = 0;
			int COLUMNA_FECHA_FINAL_PRECIO = 0;
			int COLUMNA_PORC_DESCUENTO = 0;
			int COLUMNA_EVENTO_COMERCIAL = 0;
			int COLUMNA_FECHA_INICIAL_EVENTO = 0;
			int COLUMNA_FECHA_FINAL_EVENTO = 0;
			int COLUMNA_PORC_DESCUENTO2 = 0;
			int COLUMNA_EVENTO_COMERCIAL2 = 0;
			int COLUMNA_FECHA_INICIAL_EVENTO2 = 0;
			int COLUMNA_FECHA_FINAL_EVENTO2 = 0;
			int COLUMNA_PORC_DESCUENTO3 = 0;
			int COLUMNA_EVENTO_COMERCIAL3 = 0;
			int COLUMNA_FECHA_INICIAL_EVENTO3 = 0;
			int COLUMNA_FECHA_FINAL_EVENTO3 = 0;
			int COLUMNA_CATEGORIA_LOGYCA = 0;

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd = new SqlCommand(SQL, conn, trans);
				cmd.CommandType = CommandType.StoredProcedure;

				List<string> no_estan = new List<string>();

				COLUMNA_GTIN = GetColumnIndex(GTIN, lineas);
				COLUMNA_DESCRIPCION_LARGA = GetColumnIndex(DESCRIPCION_LARGA, lineas);
				COLUMNA_GLN_COMPRADOR = GetColumnIndex(GLN_COMPRADOR, lineas);
				COLUMNA_MONEDA = GetColumnIndex(MONEDA, lineas);
				COLUMNA_PRECIO = GetColumnIndex(PRECIO, lineas);
				COLUMNA_FECHA_INICIAL_PRECIO = GetColumnIndex(FECHA_INICIAL_PRECIO, lineas);
				COLUMNA_FECHA_FINAL_PRECIO = GetColumnIndex(FECHA_FINAL_PRECIO, lineas);
				COLUMNA_PORC_DESCUENTO = GetColumnIndex(PORC_DESCUENTO, lineas);
				COLUMNA_EVENTO_COMERCIAL = GetColumnIndex(EVENTO_COMERCIAL, lineas);
				COLUMNA_FECHA_INICIAL_EVENTO = GetColumnIndex(FECHA_INICIAL_EVENTO, lineas);
				COLUMNA_FECHA_FINAL_EVENTO = GetColumnIndex(FECHA_FINAL_EVENTO, lineas);
				COLUMNA_PORC_DESCUENTO2 = GetColumnIndex(PORC_DESCUENTO2, lineas);
				COLUMNA_EVENTO_COMERCIAL2 = GetColumnIndex(EVENTO_COMERCIAL2, lineas);
				COLUMNA_FECHA_INICIAL_EVENTO2 = GetColumnIndex(FECHA_INICIAL_EVENTO2, lineas);
				COLUMNA_FECHA_FINAL_EVENTO2 = GetColumnIndex(FECHA_FINAL_EVENTO2, lineas);
				COLUMNA_PORC_DESCUENTO3 = GetColumnIndex(PORC_DESCUENTO3, lineas);
				COLUMNA_EVENTO_COMERCIAL3 = GetColumnIndex(EVENTO_COMERCIAL3, lineas);
				COLUMNA_FECHA_INICIAL_EVENTO3 = GetColumnIndex(FECHA_INICIAL_EVENTO3, lineas);
				COLUMNA_FECHA_FINAL_EVENTO3 = GetColumnIndex(FECHA_FINAL_EVENTO3, lineas);
				COLUMNA_CATEGORIA_LOGYCA = GetColumnIndex(CATEGORIA_LOGYCA, lineas);

				for (int i = 2; i < lineas.LastRowUsed().RowNumber() + 1; i++)
				{
					descripcion = "";
					if (COLUMNA_GTIN > 0)
					{
						if (lineas.Cell(i, COLUMNA_GTIN).GetValue<string>().Length > 13)
						{
							continue;
						}

						if (gln_comprador.Trim().Equals(lineas.Cell(i, COLUMNA_GLN_COMPRADOR).GetValue<string>()))
						{
							string[] info_item = null;

							info_item = ObtenerUnidadMedidaTmpoEntrega(lineas.Cell(i, COLUMNA_GTIN).GetValue<string>());
							if (info_item != null)
							{
								if (info_item[4].Equals("1"))
								{
									cmd.Parameters.Clear();
									cmd.Parameters.AddWithValue("@NRO_DOC", numero_doc);
									cmd.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
									cmd.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
									cmd.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
									cmd.Parameters.AddWithValue("@GTIN", lineas.Cell(i, COLUMNA_GTIN).GetValue<string>());
									cmd.Parameters.AddWithValue("@DESCRIPCION", info_item[2]);
									if (COLUMNA_MONEDA > 0)
									{
										cmd.Parameters.AddWithValue("@MONEDA", lineas.Cell(i, COLUMNA_MONEDA).GetValue<string>());
									}
									else
									{
										cmd.Parameters.AddWithValue("@MONEDA", "COP");
									}
									if (COLUMNA_PRECIO > 0)
									{
										cmd.Parameters.AddWithValue("@PRECIO", Convert.ToDecimal(lineas.Cell(i, COLUMNA_PRECIO).GetValue<string>()));
									}
									else
									{
										cmd.Parameters.AddWithValue("@PRECIO", 0);
									}
									cmd.Parameters.AddWithValue("@UND_MED", info_item[0]);

									cmd.Parameters.AddWithValue("@TMPO_ENT", Convert.ToInt32(info_item[1]));

									//DESCUENTO 1
									if (COLUMNA_PORC_DESCUENTO > 0)
									{
										string dsc1 = lineas.Cell(i, COLUMNA_PORC_DESCUENTO).GetValue<string>();
										cmd.Parameters.AddWithValue("@PORC_DESCTO", dsc1 == "" ? (object)DBNull.Value : Convert.ToDecimal(dsc1));
									}
									else
									{
										cmd.Parameters.AddWithValue("@PORC_DESCTO", (object)DBNull.Value);
									}

									if (COLUMNA_EVENTO_COMERCIAL > 0)
									{
										cmd.Parameters.AddWithValue("@EVENTO_COM", lineas.Cell(i, COLUMNA_EVENTO_COMERCIAL).GetValue<string>());
									}
									else
									{
										cmd.Parameters.AddWithValue("@EVENTO_COM", (object)DBNull.Value);
									}
									if (COLUMNA_FECHA_INICIAL_EVENTO > 0)
									{
										cmd.Parameters.AddWithValue("@FECHA_INI", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_INICIAL_EVENTO).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
									}
									else
									{
										cmd.Parameters.AddWithValue("@FECHA_INI", (object)DBNull.Value);
									}
									if (COLUMNA_FECHA_FINAL_EVENTO > 0)
									{
										cmd.Parameters.AddWithValue("@FECHA_FIN", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_FINAL_EVENTO).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
									}
									else
									{
										cmd.Parameters.AddWithValue("@FECHA_FIN", (object)DBNull.Value);
									}

									//DESCUENTO 2
									if (COLUMNA_PORC_DESCUENTO2 > 0)
									{
										string dsc2 = lineas.Cell(i, COLUMNA_PORC_DESCUENTO2).GetValue<string>();
										cmd.Parameters.AddWithValue("@PORC_DESCTO2", dsc2 == "" ? (object)DBNull.Value : Convert.ToDecimal(dsc2));
									}
									else
									{
										cmd.Parameters.AddWithValue("@PORC_DESCTO2", (object)DBNull.Value);
									}

									if (COLUMNA_EVENTO_COMERCIAL2 > 0)
									{
										cmd.Parameters.AddWithValue("@EVENTO_COM2", lineas.Cell(i, COLUMNA_EVENTO_COMERCIAL2).GetValue<string>());
									}
									else
									{
										cmd.Parameters.AddWithValue("@EVENTO_COM2", (object)DBNull.Value);
									}
									if (COLUMNA_FECHA_INICIAL_EVENTO2 > 0)
									{
										cmd.Parameters.AddWithValue("@FECHA_INI2", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_INICIAL_EVENTO2).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
									}
									else
									{
										cmd.Parameters.AddWithValue("@FECHA_INI2", (object)DBNull.Value);
									}
									if (COLUMNA_FECHA_FINAL_EVENTO2 > 0)
									{
										cmd.Parameters.AddWithValue("@FECHA_FIN2", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_FINAL_EVENTO2).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
									}
									else
									{
										cmd.Parameters.AddWithValue("@FECHA_FIN2", (object)DBNull.Value);
									}
									//

									//DESCUENTO 3
									if (COLUMNA_PORC_DESCUENTO3 > 0)
									{
										string dsc3 = lineas.Cell(i, COLUMNA_PORC_DESCUENTO3).GetValue<string>();
										cmd.Parameters.AddWithValue("@PORC_DESCTO3", dsc3 == "" ? (object)DBNull.Value : Convert.ToDecimal(dsc3));
									}
									else
									{
										cmd.Parameters.AddWithValue("@PORC_DESCTO3", (object)DBNull.Value);
									}

									if (COLUMNA_EVENTO_COMERCIAL3 > 0)
									{
										cmd.Parameters.AddWithValue("@EVENTO_COM3", lineas.Cell(i, COLUMNA_EVENTO_COMERCIAL3).GetValue<string>());
									}
									else
									{
										cmd.Parameters.AddWithValue("@EVENTO_COM3", (object)DBNull.Value);
									}
									if (COLUMNA_FECHA_INICIAL_EVENTO3 > 0)
									{
										cmd.Parameters.AddWithValue("@FECHA_INI3", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_INICIAL_EVENTO3).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
									}
									else
									{
										cmd.Parameters.AddWithValue("@FECHA_INI3", (object)DBNull.Value);
									}
									if (COLUMNA_FECHA_FINAL_EVENTO3 > 0)
									{
										cmd.Parameters.AddWithValue("@FECHA_FIN3", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_FINAL_EVENTO3).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
									}
									else
									{
										cmd.Parameters.AddWithValue("@FECHA_FIN3", (object)DBNull.Value);
									}
									//
									if (COLUMNA_FECHA_INICIAL_PRECIO > 0)
									{
										cmd.Parameters.AddWithValue("@FECHA_ACT", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_INICIAL_PRECIO).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
									}
									else
									{
										cmd.Parameters.AddWithValue("@FECHA_ACT", (object)DBNull.Value);
									}
									if (COLUMNA_FECHA_FINAL_PRECIO > 0)
									{
										cmd.Parameters.AddWithValue("@FECHA_HASTA", Convert.ToDateTime(lineas.Cell(i, COLUMNA_FECHA_FINAL_PRECIO).GetValue<string>(), CultureInfo.CurrentCulture).ToString("yyyyMMdd"));
									}
									else
									{
										cmd.Parameters.AddWithValue("@FECHA_HASTA", (object)DBNull.Value);
									}

									cmd.Parameters.AddWithValue("@ID", info_item[3]);
									cmd.Parameters.AddWithValue("@IMPUESTO", Convert.ToDecimal(info_item[5]));

									if (COLUMNA_CATEGORIA_LOGYCA > 0)
									{
										cmd.Parameters.AddWithValue("@CAT_LOGYCA", lineas.Cell(i, COLUMNA_CATEGORIA_LOGYCA).GetValue<string>());
									}
									else
									{
										cmd.Parameters.AddWithValue("@CAT_LOGYCA", (object)DBNull.Value);
									}

									cmd.ExecuteNonQuery();
									cmd.Parameters.Clear();
								}
							}
							else
							{
								if (COLUMNA_DESCRIPCION_LARGA > 0)
								{
									no_estan.Add(lineas.Cell(i, COLUMNA_GTIN).GetValue<string>() + "|" + lineas.Cell(i, COLUMNA_DESCRIPCION_LARGA).GetValue<string>());
								}
								else
								{
									no_estan.Add(lineas.Cell(i, COLUMNA_GTIN).GetValue<string>() + "|");
								}
							}
						}
					}
				}

				if (no_estan.Count > 0)
				{

					string sql = "select " +
									"do_razon_social, " +
									"do_nit " +
								 "from " +
									"documentos " +
								 "where " +
									"do_numero_doc=@NRO_DOC and " +
									"do_nombre_doc=@NOMB_DOC and " +
									"do_gln_proveedor=@GLN_PROV and " +
									"do_gln_comprador=@GLN_COMP";

					string[] info_prov = new string[2];

					SqlCommand cmd2 = new SqlCommand(sql, conn, trans);
					cmd2.CommandType = CommandType.Text;
					cmd2.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd2.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd2.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd2.Parameters.AddWithValue("@GLN_COMP", gln_comprador);

					SqlDataReader dr = cmd2.ExecuteReader();
					if (dr.HasRows)
					{
						dr.Read();
						info_prov[0] = dr[0].ToString().Trim();
						info_prov[1] = dr[1].ToString().Trim();
					}
					dr.Close();

					string sql2 = "if not exists(select " +
													"* " +
												"from " +
													"NoProcesados " +
												"where " +
													"np_numero_doc = @nrodoc and " +
													"np_nombre_doc = @nombdoc and " +
													"np_gln_proveedor = @prov and " +
													"np_gln_comprador = @comp and " +
													"np_gtin = @gtin) " +
								"begin " +
									"insert into " +
										"NoProcesados" +
										"(" +
											"np_numero_doc, " +
											"np_nombre_doc, " +
											"np_gln_proveedor, " +
											"np_gln_comprador, " +
											"np_gtin, " +
											"np_descripcion, " +
											"np_existe, " +
											"np_fecha" +
										") " +
									"values" +
									"(" +
										"@nrodoc, " +
										"@nombdoc, " +
										"@prov, " +
										"@comp, " +
										"@gtin, " +
										"@descripcion, " +
										"0, " +
										"getdate()" +
									") " +
								"end";

					FrmItemsNoEstan _FrmItemsNoEstan = new FrmItemsNoEstan(info_prov[0], info_prov[1], nombre_doc, numero_doc, false);
					_FrmItemsNoEstan.dgv_items.AutoGenerateColumns = false;
					SqlCommand cmd3 = new SqlCommand(sql2, conn, trans);
					cmd3.CommandType = CommandType.Text;

					for (int i = 0; i < no_estan.Count; i++)
					{
						cmd3.Parameters.AddWithValue("@nrodoc", numero_doc);
						cmd3.Parameters.AddWithValue("@nombdoc", nombre_doc);
						cmd3.Parameters.AddWithValue("@prov", gln_proveedor);
						cmd3.Parameters.AddWithValue("@comp", gln_comprador);
						cmd3.Parameters.AddWithValue("@gtin", no_estan[i].Split('|')[0]);

						string desc = no_estan[i].Split('|')[1].Length > 40 ? no_estan[i].Split('|')[1].Substring(0, 40) : no_estan[i].Split('|')[1];

						cmd3.Parameters.AddWithValue("@descripcion", desc);
						cmd3.ExecuteNonQuery();
						cmd3.Parameters.Clear();
						_FrmItemsNoEstan.dgv_items.Rows.Add(no_estan[i].Split('|')[0], no_estan[i].Split('|')[1]);
					}
					_FrmItemsNoEstan.ShowDialog();
				}
				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al guardar la información de ítem cotización cambio de precio: " + ex.Message);
			}
		}


		/// <summary>
		/// Obtiene la información de un item que esta asociado a un numero de documento, a un nombre de documento, a un proveedor y a un comprador.
		/// </summary>
		/// <param name="numero_doc">Número de documento.</param>
		/// <param name="nombre_doc">Nombre de documento.</param>
		/// <param name="gln_proveedor">Gln de proveedor.</param>
		/// <param name="gln_comprador">Gln de comprador.</param>
		/// <returns></returns>
		public List<List<object>> ObtenerDatosItems(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador, string gtin, int consulta = 0)
		{
			string SQL1 = "SELECT " +
								"do_numero_doc, " +
								"do_nombre_doc, " +
								"do_gln_proveedor, " +
								"do_gln_comprador, " +
								"do_nit, " +
								"do_razon_social, " +
								"do_sucursal, " +
								"do_accion, " +
								"it_gtin, " +
								"it_descripcion_larga, " +
								"it_descripcion_corta, " +
								"ISNULL(it_grupo_impositivo,'') it_grupo_impositivo, " +
								"ISNULL(it_tipo_inventario,'') it_tipo_inventario, " +
								"ISNULL(it_unidad_inventario,'') it_unidad_inventario, " +
								"isnull(it_factor_peso_inventario,0) it_factor_peso_inventario, " +
								"it_unidad_orden, " +
								"isnull(it_factor_orden,0) it_factor_orden, " +
								"ISNULL(it_factor_peso_orden,0) it_factor_peso_orden, " +
								"it_unidad_empaque, " +
								"isnull(it_factor_empaque,0) it_factor_empaque, " +
								"ISNULL(it_factor_peso_empaque,0) it_factor_peso_empaque, " +
								"it_cat_logyca, " +
								"it_aceptado, " +
								"ISNULL(it_motivo,'') it_motivo, " +
								"isnull(it_impuesto,0) it_impuesto, " +
								"isnull(it_ind_compra,0) it_ind_compra, " +
								"isnull(it_ind_venta,0) it_ind_venta, " +
								"isnull(it_ind_manufactura,0) it_ind_manufactura, " +
								"it_imagen " +
						"FROM " +
							"Documentos " +
							"INNER JOIN " +
							"Items ON do_numero_doc=it_numero_doc AND do_nombre_doc=it_nombre_doc " +
																	"AND do_gln_proveedor=it_gln_proveedor AND do_gln_comprador=it_gln_comprador " +
						"WHERE do_numero_doc=@NRO_DOC AND do_nombre_doc=@NOMB_DOC " +
															"AND do_gln_proveedor=@GLN_PROV AND do_gln_comprador=@GLN_COMP  AND it_gtin=@GTIN";

			string SQL2 = "SELECT " +
							"do_numero_doc, " +
							"do_nombre_doc, " +
							"do_gln_proveedor, " +
							"do_gln_comprador, " +
							"do_nit, " +
							"do_sucursal, " +
							"do_accion, " +
							"ct_gtin, " +
							"ct_moneda, " +
							"ct_precio, " +
							"ISNULL(ct_unidad_med,'') ct_unidad_med, " +
							"ct_tiempo_entrega, " +
							"ISNULL(ct_porc_dscto,0) ct_porc_dscto, " +
							"ISNULL(ct_evento_comercial,'') ct_evento_comercial, " +
							"ISNULL(ct_fecha_ini,'') ct_fecha_ini, " +
							"ISNULL(ct_fecha_fin,'') ct_fecha_fin, " +
							"ct_aceptado, " +
							"ISNULL(ct_fecha_act,'') ct_fecha_act, " +
							"ISNULL(ct_fecha_hasta,'') ct_fecha_hasta, " +
							"ISNULL(ct_porc_dscto2,0) ct_porc_dscto2, " +
							"ISNULL(ct_evento_comercial2,'') ct_evento_comercial2, " +
							"ISNULL(ct_fecha_ini2,'') ct_fecha_ini2, " +
							"ISNULL(ct_fecha_fin2,'') ct_fecha_fin2, " +
							"ISNULL(ct_porc_dscto3,0) ct_porc_dscto3, " +
							"ISNULL(ct_evento_comercial3,'') ct_evento_comercial3, " +
							"ISNULL(ct_fecha_ini3,'') ct_fecha_ini3, " +
							"ISNULL(ct_fecha_fin3,'') ct_fecha_fin3 " +
						"FROM Documentos " +
						"INNER JOIN Cotizacion ON do_numero_doc=ct_numero_doc AND do_nombre_doc=ct_nombre_doc " +
																			"AND do_gln_proveedor=ct_gln_proveedor AND do_gln_comprador=ct_gln_comprador " +
						"WHERE do_numero_doc=@NRO_DOC AND do_nombre_doc=@NOMB_DOC " +
															"AND do_gln_proveedor=@GLN_PROV AND do_gln_comprador=@GLN_COMP  AND ct_gtin=@GTIN";

			string SQL3 = "SELECT " +
							"do_numero_doc, " +
							"do_nombre_doc, " +
							"do_gln_proveedor, " +
							"do_gln_comprador, " +
							"do_accion, " +
							"dt_gtin, " +
							"isnull(dt_alto,0) dt_alto, " +
							"isnull(dt_ancho,0) dt_ancho, " +
							"isnull(dt_profundo,0) dt_profundo, " +
							"isnull(dt_alto_emp,0) dt_alto_emp, " +
							"isnull(dt_ancho_emp,0) dt_ancho_emp, " +
							"isnull(dt_profundo_emp,0) dt_profundo_emp, " +
							"isnull(dt_descripcion_tec1,'') dt_descripcion_tec1," +
							"isnull(dt_descripcion_tec2,'') dt_descripcion_tec2, " +
							"isnull(dt_descripcion_tec3,'') dt_descripcion_tec3, " +
							"dt_aceptado " +
						"FROM Documentos " +
						"INNER JOIN DescripcionTecnica ON do_numero_doc=dt_numero_doc AND do_nombre_doc=dt_nombre_doc " +
																			"AND do_gln_proveedor=dt_gln_proveedor AND do_gln_comprador=dt_gln_comprador " +
						"WHERE do_numero_doc=@NRO_DOC AND do_nombre_doc=@NOMB_DOC " +
															"AND do_gln_proveedor=@GLN_PROV AND do_gln_comprador=@GLN_COMP  AND dt_gtin=@GTIN";

			string SQL4 = "SELECT " +
							"do_numero_doc, " +
							"do_nombre_doc, " +
							"do_gln_proveedor, " +
							"do_gln_comprador, " +
							"do_accion, " +
							"cc_gtin, " +
							"cc_plan, " +
							"ISNULL(cc_desc_plan,'') AS cc_desc_plan, " +
							"cc_criterio_mayor, " +
							"ISNULL(cc_desc_criterio,'') AS cc_desc_criterio " +
						"FROM " +
							"Documentos " +
						"INNER JOIN CriteriosClasificacion ON do_numero_doc=cc_numero_doc AND do_nombre_doc=cc_nombre_doc " +
																			"AND do_gln_proveedor=cc_gln_proveedor AND do_gln_comprador=cc_gln_comprador " +
						"WHERE do_numero_doc=@NRO_DOC AND do_nombre_doc=@NOMB_DOC " +
															"AND do_gln_proveedor=@GLN_PROV AND do_gln_comprador=@GLN_COMP  AND cc_gtin=@GTIN";

			string SQL5 = "SELECT " +
							"do_numero_doc, " +
							"do_nombre_doc, " +
							"do_gln_proveedor, " +
							"do_gln_comprador, " +
							"do_accion, " +
							"pp_gtin, " +
							"pp_instalacion, " +
							"pp_bodega_asig, " +
							"pp_rotacion_veces, " +
							"pp_rotacion_costo, " +
							"pp_und_vta, " +
							"pp_periodo_cubrimiento, " +
							"pp_tmpo_reposicion, " +
							"pp_tmpo_seguridad, " +
							"ISNULL(pp_comprador,'') pp_comprador, " +
							"isnull(pp_dias_horiz_plan,0) pp_dias_horiz_plan, " +
							"pp_stock_seguridad_estatico, " +
							"pp_dias_horiz_stock_min, " +
							"pp_dias_stock_min, " +
							"pp_tmpo_reposicion_fijo, " +
							"pp_politica_orden, " +
							"pp_tamaño_lote, " +
							"pp_tercero_prov, " +
							"pp_razon_soc, " +
							"pp_sucursal_prov, " +
							"isnull(pp_gen_orden_plan,1) pp_gen_orden_plan  " +
						"FROM Documentos " +
						"INNER JOIN ParametrosPlaneacion ON do_numero_doc=pp_numero_doc AND do_nombre_doc=pp_nombre_doc " +
																			"AND do_gln_proveedor=pp_gln_proveedor AND do_gln_comprador=pp_gln_comprador " +
						"WHERE do_numero_doc=@NRO_DOC AND do_nombre_doc=@NOMB_DOC " +
															"AND do_gln_proveedor=@GLN_PROV AND do_gln_comprador=@GLN_COMP  AND pp_gtin=@GTIN";

			string SQL6 = "SELECT " +
							"do_numero_doc, " +
							"do_nombre_doc, " +
							"do_gln_proveedor, " +
							"do_gln_comprador, " +
							"do_accion, " +
							"pv_gtin, " +
							"ISNULL(pv_unidad_medida,'') pv_unidad_medida, " +
							"ISNULL(pv_co,'') pv_co, " +
							"ISNULL(pv_lista_precio,'') pv_lista_precio, " +
							"ISNULL(pv_precio,0) pv_precio, " +
							"pv_fecha_activacion, " +
							"isnull(pv_fecha_inactivacion,'') pv_fecha_inactivacion, " +
							"isnull(pv_margen, 0) pv_margen " +
						  "FROM Documentos " +
						  "INNER JOIN PrecioVenta ON do_numero_doc=pv_numero_doc AND do_nombre_doc=pv_nombre_doc " +
													   "AND do_gln_proveedor=pv_gln_proveedor AND do_gln_comprador=pv_gln_comprador " +
						  "WHERE do_numero_doc=@NRO_DOC AND do_nombre_doc=@NOMB_DOC " +
								  "AND do_gln_proveedor=@GLN_PROV AND do_gln_comprador=@GLN_COMP  AND pv_gtin=@GTIN";

			//ESTA ES POR ITEM PARA MOSTRAR LA INFORMACION EN EL FORMULARIO
			string SQL7 = "SELECT " +
							"do_numero_doc, " +
							"do_nombre_doc, " +
							"do_gln_proveedor, " +
							"do_gln_comprador, " +
							"do_accion, " +
							"po_gtin, " +
							"po_id_portafolio, " +
							"po_descripcion, " +
							"po_nota " +
						  "FROM Documentos " +
						  "INNER JOIN Portafolio ON do_numero_doc=po_numero_doc AND do_nombre_doc=po_nombre_doc " +
													   "AND do_gln_proveedor=po_gln_proveedor AND do_gln_comprador=po_gln_comprador " +
							"WHERE do_numero_doc=@NRO_DOC AND do_nombre_doc=@NOMB_DOC " +
								  "AND do_gln_proveedor=@GLN_PROV AND do_gln_comprador=@GLN_COMP  AND po_gtin=@GTIN";

			//ESTA ES POR PORTAFOLIO PARA CREAR EL CONECTOR
			string SQL8 = "SELECT " +
							"po_gtin, " +
							"po_id_portafolio, " +
							"po_descripcion, " +
							"po_nota " +
						  "FROM Documentos " +
						  "INNER JOIN Portafolio ON do_numero_doc=po_numero_doc AND do_nombre_doc=po_nombre_doc " +
													   "AND do_gln_proveedor=po_gln_proveedor AND do_gln_comprador=po_gln_comprador " +
						  "WHERE do_numero_doc=@NRO_DOC AND do_nombre_doc=@NOMB_DOC " +
								  "AND do_gln_proveedor=@GLN_PROV AND do_gln_comprador=@GLN_COMP  AND po_id_portafolio=@PORT";

			string SQL9 = "SELECT " +
							"do_numero_doc, " +
							"do_nombre_doc, " +
							"do_gln_proveedor, " +
							"do_gln_comprador, " +
							"br_gtin, " +
							"isnull(br_barra,'') br_barra, " +
							"isnull(br_unidad_medida,'') br_unidad_medida, " +
							"isnull(br_cantidad_unidad_medida,0) br_cantidad_unidad_medida, " +
							"isnull(br_tipo,'') br_tipo, " +
							"isnull(br_ind_operacion,'') br_ind_operacion, " +
							"isnull(br_factor_operacion, 0) br_factor_operacion, " +
							"br_aceptado, isnull(br_principal,1) br_principal " +
						  "FROM Documentos " +
						  "INNER JOIN " +
								"CodigoBarras ON do_numero_doc=br_numero_doc AND do_nombre_doc=br_nombre_doc " +
																		"AND do_gln_proveedor=br_gln_proveedor AND do_gln_comprador=br_gln_comprador " +
						  "WHERE do_numero_doc=@NRO_DOC AND do_nombre_doc=@NOMB_DOC " +
																"AND do_gln_proveedor=@GLN_PROV AND do_gln_comprador=@GLN_COMP  AND br_gtin=@GTIN";

			string SQL10 = "SELECT " +
								"do_numero_doc, " +
								"do_nombre_doc, " +
								"do_gln_proveedor, " +
								"do_gln_comprador, " +
								"do_nit, " +
								"do_razon_social, " +
								"do_sucursal, " +
								"do_accion, " +
								"cp_gtin, " +
								"cp_descripcion, " +
								"cp_moneda, " +
								"isnull(cp_precio,0) cp_precio, " +
								"cp_unidad_med, " +
								"isnull(cp_tiempo_entrega,0) cp_tiempo_entrega, " +
								"isnull(cp_porc_dscto,0) cp_porc_dscto, " +
								"isnull(cp_evento_comercial,'') cp_evento_comercial, " +
								"cp_fecha_ini, " +
								"cp_fecha_fin, " +
								"cp_aceptado, " +
								"cp_fecha_act, " +
								"cp_fecha_hasta, " +
								"ISNULL(cp_motivo,'') cp_motivo, " +
								"cp_impuesto, " +
								"isnull(cp_porc_dscto2,0) cp_porc_dscto2, " +
								"isnull(cp_evento_comercial2,'') cp_evento_comercial2, " +
								"cp_fecha_ini2, " +
								"cp_fecha_fin2, " +
								"isnull(cp_porc_dscto3,0) cp_porc_dscto3, " +
								"isnull(cp_evento_comercial3,'') cp_evento_comercial3, " +
								"cp_fecha_ini3, " +
								"cp_fecha_fin3, " +
								"isnull(cp_cat_logyca, '') cp_cat_logyca " +
							"FROM " +
								"Documentos " +
							"INNER JOIN CambioPrecio ON do_numero_doc=cp_numero_doc AND do_nombre_doc=cp_nombre_doc " +
														"AND do_gln_proveedor=cp_gln_proveedor AND do_gln_comprador=cp_gln_comprador " +
							"WHERE do_numero_doc=@NRO_DOC AND do_nombre_doc=@NOMB_DOC " +
								  "AND do_gln_proveedor=@GLN_PROV AND do_gln_comprador=@GLN_COMP  AND cp_gtin=@GTIN";

			string SQL11 = @"SELECT 
                                do_numero_doc, 
                                do_nombre_doc, 
                                do_gln_proveedor, 
                                do_gln_comprador, 
                                do_nit, 
                                do_razon_social, 
                                do_sucursal, 
                                do_accion,
	                            isnull(ot_nombre_comercial,'') ot_nombre_comercial,
	                            isnull(ot_imagen,'') ot_imagen,
	                            isnull(ot_marca,'') ot_marca,
	                            isnull(ot_fabricante,'') ot_fabricante,
	                            isnull(ot_registro_sanitario,'') ot_registro_sanitario,
	                            isnull(ot_tipo_producto,'') ot_tipo_producto,
	                            isnull(ot_linea,'') ot_linea,
	                            isnull(ot_fragancia,'') ot_fragancia,
	                            isnull(ot_sabor,'') ot_sabor,
	                            isnull(ot_recomendaciones,'') ot_recomendaciones,
	                            isnull(ot_advertencia,'') ot_advertencia,
	                            isnull(ot_precauciones, '') ot_precauciones,
                                isnull(ot_ficha_tecnica, '') ot_ficha_tecnica,
                                
                                isnull(ot_tipo_empaque, '') ot_tipo_empaque,
                                isnull(ot_multiplos_despacho, 0) ot_multiplos_despacho,
                                isnull(ot_proveedor, '') ot_proveedor,
                                isnull(ot_cant_contenida, 0) ot_cant_contenida,
                                isnull(ot_unds_embalaje, 0) ot_unds_embalaje,
                                isnull(ot_sublinea, '') ot_sublinea,
								isnull(ot_calificador_cantidad_contenida, '') ot_calificador_cantidad_contenida,
								isnull(ot_id_calificador_cantidad_contenida, '') ot_id_calificador_cantidad_contenida,
								ot_gtin
                            FROM
								Documentos
								INNER JOIN Items on it_numero_doc=do_numero_doc AND it_nombre_doc=do_nombre_doc AND it_gln_proveedor=do_gln_proveedor AND it_gln_comprador=do_gln_comprador 
								INNER JOIN OtrosDatos ON ot_numero_doc=it_numero_doc AND ot_nombre_doc=it_nombre_doc AND ot_gln_proveedor=it_gln_proveedor AND it_gln_comprador=do_gln_comprador and ot_gtin=it_gtin
                            WHERE 
								do_numero_doc = @NRO_DOC 
								AND do_nombre_doc = @NOMB_DOC
                                AND do_gln_proveedor = @GLN_PROV 
								AND do_gln_comprador = @GLN_COMP  
								AND ot_gtin = @GTIN
								AND it_aceptado=1";

			List<List<object>> listado_general = new List<List<object>>();
			List<object> listado_items = null;
			List<object> listado_cotizacion = null;
			List<object> listado_descripcion_tecnica = null;
			List<object> listado_criterios_clasificacion = new List<object>();
			List<object> listado_parametros_plan = new List<object>();
			List<object> listado_precios_vta = new List<object>();
			List<object> listado_portafolios = new List<object>();
			List<object> listado_portafolios2 = new List<object>();
			List<object> listado_barras = null;
			List<object> listado_cambio_precio = null;
			List<object> listado_otros_datos = null;
			try
			{
				if (consulta == 0 || consulta == 1)
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd1 = new SqlCommand(SQL1, conn);
					cmd1.CommandTimeout = 180;
					cmd1.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd1.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd1.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd1.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
					cmd1.Parameters.AddWithValue("@GTIN", gtin);
					SqlDataReader dr1 = cmd1.ExecuteReader();
					if (dr1.HasRows)
					{
						dr1.Read();
						listado_items = new List<object>();
						listado_items.Add(dr1.GetString(0));
						listado_items.Add(dr1.GetString(1));
						listado_items.Add(dr1.GetString(2));
						listado_items.Add(dr1.GetString(3));
						listado_items.Add(dr1.GetString(4));
						listado_items.Add(dr1.GetString(5));
						listado_items.Add(dr1.GetString(6));
						listado_items.Add(dr1.GetString(7));
						listado_items.Add(dr1.GetString(8));
						listado_items.Add(dr1.GetString(9));
						listado_items.Add(dr1.GetString(10));
						listado_items.Add(dr1.GetString(11));
						listado_items.Add(dr1.GetString(12));
						listado_items.Add(dr1.GetString(13));
						listado_items.Add(dr1.GetDecimal(14));
						listado_items.Add(dr1.GetString(15));
						listado_items.Add(dr1.GetDecimal(16));
						listado_items.Add(dr1.GetDecimal(17));
						listado_items.Add(dr1.GetString(18));
						listado_items.Add(dr1.GetDecimal(19));
						listado_items.Add(dr1.GetDecimal(20));
						listado_items.Add(dr1.GetString(21));
						listado_items.Add(dr1.GetBoolean(22));
						listado_items.Add(dr1.GetString(23));
						listado_items.Add(dr1.GetDecimal(24));
						listado_items.Add(dr1.GetBoolean(25));
						listado_items.Add(dr1.GetBoolean(26));
						listado_items.Add(dr1.GetBoolean(27));
						listado_items.Add(dr1.GetString(28));
					}
					dr1.Close();
					conn.Close();

					listado_general.Add(listado_items);
				}
				if (consulta == 0 || consulta == 2)
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd2 = new SqlCommand(SQL2, conn);
					cmd2.CommandTimeout = 180;
					cmd2.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd2.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd2.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd2.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
					cmd2.Parameters.AddWithValue("@GTIN", gtin);

					SqlDataReader dr2 = cmd2.ExecuteReader();
					if (dr2.HasRows)
					{
						dr2.Read();
						listado_cotizacion = new List<object>();
						listado_cotizacion.Add(dr2.GetString(0));
						listado_cotizacion.Add(dr2.GetString(1));
						listado_cotizacion.Add(dr2.GetString(2));
						listado_cotizacion.Add(dr2.GetString(3));
						listado_cotizacion.Add(dr2.GetString(4));
						listado_cotizacion.Add(dr2.GetString(5));
						listado_cotizacion.Add(dr2.GetString(6));
						listado_cotizacion.Add(dr2.GetString(7));
						listado_cotizacion.Add(dr2.GetString(8));
						listado_cotizacion.Add(dr2.GetDecimal(9));
						listado_cotizacion.Add(dr2.GetString(10));
						listado_cotizacion.Add(dr2.GetInt32(11));
						listado_cotizacion.Add(dr2.GetDecimal(12));
						listado_cotizacion.Add(dr2.GetString(13));
						listado_cotizacion.Add(dr2.GetDateTime(14));
						listado_cotizacion.Add(dr2.GetDateTime(15));
						listado_cotizacion.Add(dr2.GetBoolean(16));
						listado_cotizacion.Add(dr2.GetDateTime(17));
						listado_cotizacion.Add(dr2.GetDateTime(18));

						listado_cotizacion.Add(dr2.GetDecimal(19));
						listado_cotizacion.Add(dr2.GetString(20));
						listado_cotizacion.Add(dr2.GetDateTime(21));
						listado_cotizacion.Add(dr2.GetDateTime(22));
						listado_cotizacion.Add(dr2.GetDecimal(23));
						listado_cotizacion.Add(dr2.GetString(24));
						listado_cotizacion.Add(dr2.GetDateTime(25));
						listado_cotizacion.Add(dr2.GetDateTime(26));

					}
					dr2.Close();
					conn.Close();
					listado_general.Add(listado_cotizacion);
				}

				if (consulta == 0 || consulta == 3)
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd3 = new SqlCommand(SQL3, conn);
					cmd3.CommandTimeout = 180;
					cmd3.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd3.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd3.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd3.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
					cmd3.Parameters.AddWithValue("@GTIN", gtin);

					SqlDataReader dr3 = cmd3.ExecuteReader();
					if (dr3.HasRows)
					{
						dr3.Read();
						listado_descripcion_tecnica = new List<object>();
						listado_descripcion_tecnica.Add(dr3.GetString(0));
						listado_descripcion_tecnica.Add(dr3.GetString(1));
						listado_descripcion_tecnica.Add(dr3.GetString(2));
						listado_descripcion_tecnica.Add(dr3.GetString(3));
						listado_descripcion_tecnica.Add(dr3.GetString(4));
						listado_descripcion_tecnica.Add(dr3.GetString(5));
						listado_descripcion_tecnica.Add(dr3.GetDecimal(6));
						listado_descripcion_tecnica.Add(dr3.GetDecimal(7));
						listado_descripcion_tecnica.Add(dr3.GetDecimal(8));
						listado_descripcion_tecnica.Add(dr3.GetDecimal(9));
						listado_descripcion_tecnica.Add(dr3.GetDecimal(10));
						listado_descripcion_tecnica.Add(dr3.GetDecimal(11));
						listado_descripcion_tecnica.Add(dr3.GetString(12));
						listado_descripcion_tecnica.Add(dr3.GetString(13));
						listado_descripcion_tecnica.Add(dr3.GetString(14));
						listado_descripcion_tecnica.Add(dr3.GetBoolean(15));
					}
					dr3.Close();
					conn.Close();

					listado_general.Add(listado_descripcion_tecnica);
				}

				if (consulta == 0 || consulta == 4)
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd4 = new SqlCommand(SQL4, conn);
					cmd4.CommandTimeout = 180;
					cmd4.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd4.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd4.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd4.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
					cmd4.Parameters.AddWithValue("@GTIN", gtin);

					SqlDataReader dr4 = cmd4.ExecuteReader();
					if (dr4.HasRows)
					{
						while (dr4.Read())
						{
							List<object> clasif = new List<object>();
							clasif.Add(dr4.GetString(0));
							clasif.Add(dr4.GetString(1));
							clasif.Add(dr4.GetString(2));
							clasif.Add(dr4.GetString(3));
							clasif.Add(dr4.GetString(4));
							clasif.Add(dr4.GetString(5));
							clasif.Add(dr4.GetString(6));
							clasif.Add(dr4.GetString(7));
							clasif.Add(dr4.GetString(8));
							clasif.Add(dr4.GetString(9));
							listado_criterios_clasificacion.Add(clasif);
						}
					}
					dr4.Close();
					conn.Close();
					listado_general.Add(listado_criterios_clasificacion);
				}

				if (consulta == 0 || consulta == 5)
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd5 = new SqlCommand(SQL5, conn);
					cmd5.CommandTimeout = 180;
					cmd5.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd5.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd5.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd5.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
					cmd5.Parameters.AddWithValue("@GTIN", gtin);

					SqlDataReader dr5 = cmd5.ExecuteReader();
					if (dr5.HasRows)
					{
						while (dr5.Read())
						{
							List<object> plan = new List<object>();
							plan.Add(dr5.GetString(0));
							plan.Add(dr5.GetString(1));
							plan.Add(dr5.GetString(2));
							plan.Add(dr5.GetString(3));
							plan.Add(dr5.GetString(4));
							plan.Add(dr5.GetString(5));
							plan.Add(dr5.GetString(6));
							plan.Add(dr5.GetString(7));
							plan.Add(dr5.GetString(8));
							plan.Add(dr5.GetString(9));
							plan.Add(dr5.GetString(10));
							plan.Add(dr5.GetInt32(11));
							plan.Add(dr5.GetInt32(12));
							plan.Add(dr5.GetInt32(13));
							plan.Add(dr5.GetString(14));
							plan.Add(dr5.GetInt32(15));
							plan.Add(dr5.GetDecimal(16));
							plan.Add(dr5.GetInt32(17));
							plan.Add(dr5.GetInt32(18));
							plan.Add(dr5.GetInt32(19));
							plan.Add(dr5.GetInt32(20));
							plan.Add(dr5.GetDecimal(21));
							plan.Add(dr5.GetString(22));
							plan.Add(dr5.GetString(23));
							plan.Add(dr5.GetString(24));
							plan.Add(dr5.GetBoolean(25));
							listado_parametros_plan.Add(plan);
						}
					}
					dr5.Close();
					conn.Close();
					listado_general.Add(listado_parametros_plan);
				}

				if (consulta == 0 || consulta == 6)
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd6 = new SqlCommand(SQL6, conn);
					cmd6.CommandTimeout = 180;
					cmd6.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd6.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd6.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd6.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
					cmd6.Parameters.AddWithValue("@GTIN", gtin);

					SqlDataReader dr6 = cmd6.ExecuteReader();
					if (dr6.HasRows)
					{
						while (dr6.Read())
						{
							List<object> precio = new List<object>();
							precio.Add(dr6.GetString(0));
							precio.Add(dr6.GetString(1));
							precio.Add(dr6.GetString(2));
							precio.Add(dr6.GetString(3));
							precio.Add(dr6.GetString(4));
							precio.Add(dr6.GetString(5));
							precio.Add(dr6.GetString(6));
							precio.Add(dr6.GetString(7));
							precio.Add(dr6.GetString(8));
							precio.Add(dr6.GetDecimal(9));
							precio.Add(dr6.GetDateTime(10));
							precio.Add(dr6.GetDateTime(11));
							precio.Add(dr6.GetDecimal(12));
							listado_precios_vta.Add(precio);
						}
					}
					dr6.Close();
					conn.Close();
					listado_general.Add(listado_precios_vta);
				}

				if (consulta == 0 || consulta == 7)
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd7 = new SqlCommand(SQL7, conn);
					cmd7.CommandTimeout = 180;
					cmd7.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd7.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd7.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd7.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
					cmd7.Parameters.AddWithValue("@GTIN", gtin);

					SqlDataReader dr7 = cmd7.ExecuteReader();
					if (dr7.HasRows)
					{
						while (dr7.Read())
						{
							List<object> portafolio = new List<object>();
							portafolio.Add(dr7.GetString(0));
							portafolio.Add(dr7.GetString(1));
							portafolio.Add(dr7.GetString(2));
							portafolio.Add(dr7.GetString(3));
							portafolio.Add(dr7.GetString(4));
							portafolio.Add(dr7.GetString(5));
							portafolio.Add(dr7.GetString(6));
							portafolio.Add(dr7.GetString(7));
							portafolio.Add(dr7.GetString(8));

							listado_portafolios.Add(portafolio);
						}
					}
					dr7.Close();
					conn.Close();
					listado_general.Add(listado_portafolios);
				}

				if (consulta == 8)
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd8 = new SqlCommand(SQL8, conn);
					cmd8.CommandTimeout = 180;
					cmd8.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd8.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd8.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd8.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
					cmd8.Parameters.AddWithValue("@PORT", gtin);

					SqlDataReader dr8 = cmd8.ExecuteReader();
					if (dr8.HasRows)
					{
						while (dr8.Read())
						{
							List<object> portafolio = new List<object>();
							portafolio.Add(dr8.GetString(0));
							portafolio.Add(dr8.GetString(1));
							portafolio.Add(dr8.GetString(2));
							portafolio.Add(dr8.GetString(3));

							listado_portafolios2.Add(portafolio);
						}
					}
					dr8.Close();
					conn.Close();
					listado_general.Add(listado_portafolios2);
				}

				if (consulta == 0 || consulta == 9)
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd9 = new SqlCommand(SQL9, conn);
					cmd9.CommandTimeout = 180;
					cmd9.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd9.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd9.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd9.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
					cmd9.Parameters.AddWithValue("@GTIN", gtin);
					SqlDataReader dr9 = cmd9.ExecuteReader();
					if (dr9.HasRows)
					{
						dr9.Read();
						listado_barras = new List<object>();
						listado_barras.Add(dr9.GetString(0));
						listado_barras.Add(dr9.GetString(1));
						listado_barras.Add(dr9.GetString(2));
						listado_barras.Add(dr9.GetString(3));
						listado_barras.Add(dr9.GetString(4));
						listado_barras.Add(dr9.GetString(5));
						listado_barras.Add(dr9.GetString(6));
						listado_barras.Add(dr9.GetDecimal(7));
						listado_barras.Add(dr9.GetString(8));
						listado_barras.Add(dr9.GetString(9));
						listado_barras.Add(dr9.GetDecimal(10));
						listado_barras.Add(dr9.GetBoolean(11));
						listado_barras.Add(dr9.GetBoolean(12));
					}

					dr9.Close();
					conn.Close();
					listado_general.Add(listado_barras);
				}

				if (consulta == 10)
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd10 = new SqlCommand(SQL10, conn);
					cmd10.CommandTimeout = 180;
					cmd10.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd10.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd10.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd10.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
					cmd10.Parameters.AddWithValue("@GTIN", gtin);
					SqlDataReader dr10 = cmd10.ExecuteReader();
					if (dr10.HasRows)
					{
						dr10.Read();
						listado_cambio_precio = new List<object>();
						listado_cambio_precio.Add(dr10.GetString(0));
						listado_cambio_precio.Add(dr10.GetString(1));
						listado_cambio_precio.Add(dr10.GetString(2));
						listado_cambio_precio.Add(dr10.GetString(3));
						listado_cambio_precio.Add(dr10.GetString(4));
						listado_cambio_precio.Add(dr10.GetString(5));
						listado_cambio_precio.Add(dr10.GetString(6));
						listado_cambio_precio.Add(dr10.GetString(7));
						listado_cambio_precio.Add(dr10.GetString(8));
						listado_cambio_precio.Add(dr10.GetString(9));
						listado_cambio_precio.Add(dr10.GetString(10));
						listado_cambio_precio.Add(dr10.GetDecimal(11));
						listado_cambio_precio.Add(dr10.GetString(12));
						listado_cambio_precio.Add(dr10.GetInt32(13));
						listado_cambio_precio.Add(dr10.GetDecimal(14));
						listado_cambio_precio.Add(dr10.GetString(15));
						string fecha1 = "";
						if (!Convert.IsDBNull(dr10[16]))
						{
							fecha1 = dr10.GetDateTime(16).ToString("yyyyMMdd");
						}
						listado_cambio_precio.Add(fecha1);
						string fecha2 = "";
						if (!Convert.IsDBNull(dr10[17]))
						{
							fecha2 = dr10.GetDateTime(17).ToString("yyyyMMdd");
						}
						listado_cambio_precio.Add(fecha2);
						listado_cambio_precio.Add(dr10.GetBoolean(18));
						string fecha3 = "";
						if (!Convert.IsDBNull(dr10[19]))
						{
							fecha3 = dr10.GetDateTime(19).ToString("yyyyMMdd");
						}
						listado_cambio_precio.Add(fecha3);
						string fecha4 = "";
						if (!Convert.IsDBNull(dr10[20]))
						{
							fecha4 = dr10.GetDateTime(20).ToString("yyyyMMdd");
						}
						listado_cambio_precio.Add(fecha4);
						listado_cambio_precio.Add(dr10.GetString(21));
						listado_cambio_precio.Add(dr10.GetDecimal(22));

						listado_cambio_precio.Add(dr10.GetDecimal(23));
						listado_cambio_precio.Add(dr10.GetString(24));
						string fecha5 = "";
						if (!Convert.IsDBNull(dr10[25]))
						{
							fecha5 = dr10.GetDateTime(25).ToString("yyyyMMdd");
						}
						listado_cambio_precio.Add(fecha5);
						string fecha6 = "";
						if (!Convert.IsDBNull(dr10[26]))
						{
							fecha6 = dr10.GetDateTime(26).ToString("yyyyMMdd");
						}
						listado_cambio_precio.Add(fecha6);
						listado_cambio_precio.Add(dr10.GetDecimal(27));
						listado_cambio_precio.Add(dr10.GetString(28));
						string fecha7 = "";
						if (!Convert.IsDBNull(dr10[29]))
						{
							fecha7 = dr10.GetDateTime(29).ToString("yyyyMMdd");
						}
						listado_cambio_precio.Add(fecha7);
						string fecha8 = "";
						if (!Convert.IsDBNull(dr10[30]))
						{
							fecha8 = dr10.GetDateTime(30).ToString("yyyyMMdd");
						}
						listado_cambio_precio.Add(fecha8);
						listado_cambio_precio.Add(dr10.GetString(31));
					}

					dr10.Close();
					conn.Close();
					listado_general.Add(listado_cambio_precio);
				}

				if (consulta == 0 || consulta == 11)
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd11 = new SqlCommand(SQL11, conn);
					cmd11.CommandTimeout = 180;
					cmd11.Parameters.AddWithValue("@NRO_DOC", numero_doc);
					cmd11.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
					cmd11.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
					cmd11.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
					cmd11.Parameters.AddWithValue("@GTIN", gtin);
					SqlDataReader dr11 = cmd11.ExecuteReader();
					if (dr11.HasRows)
					{
						dr11.Read();
						listado_otros_datos = new List<object>();
						listado_otros_datos.Add(dr11.GetString(0));
						listado_otros_datos.Add(dr11.GetString(1));
						listado_otros_datos.Add(dr11.GetString(2));
						listado_otros_datos.Add(dr11.GetString(3));
						listado_otros_datos.Add(dr11.GetString(4));
						listado_otros_datos.Add(dr11.GetString(5));
						listado_otros_datos.Add(dr11.GetString(6));
						listado_otros_datos.Add(dr11.GetString(7));
						listado_otros_datos.Add(dr11.GetString(8));
						listado_otros_datos.Add(dr11.GetString(9));
						listado_otros_datos.Add(dr11.GetString(10));
						listado_otros_datos.Add(dr11.GetString(11));
						listado_otros_datos.Add(dr11.GetString(12));
						listado_otros_datos.Add(dr11.GetString(13));
						listado_otros_datos.Add(dr11.GetString(14));
						listado_otros_datos.Add(dr11.GetString(15));
						listado_otros_datos.Add(dr11.GetString(16));
						listado_otros_datos.Add(dr11.GetString(17));
						listado_otros_datos.Add(dr11.GetString(18));
						listado_otros_datos.Add(dr11.GetString(19));
						listado_otros_datos.Add(dr11.GetString(20));
						listado_otros_datos.Add(dr11.GetString(21));
						listado_otros_datos.Add(dr11.GetInt32(22));
						listado_otros_datos.Add(dr11.GetString(23));
						listado_otros_datos.Add(dr11.GetDecimal(24));
						listado_otros_datos.Add(dr11.GetInt32(25));
						listado_otros_datos.Add(dr11.GetString(26));
						listado_otros_datos.Add(dr11.GetString(27));
						listado_otros_datos.Add(dr11.GetString(28));
						listado_otros_datos.Add(dr11.GetString(29));
					}

					dr11.Close();
					conn.Close();
					listado_general.Add(listado_otros_datos);
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener los datos del ítem: " + ex.Message);
			}
			return listado_general;
		}

		/// <summary>
		/// Obtiene el listado general de items que estan asociados a un numero de documento, a un nombre de documento, a un proveedor y a un comprador.
		/// </summary>
		/// <param name="numero_doc">Número del documento.</param>
		/// <param name="nombre_doc">Nombre del documento.</param>
		/// <param name="gln_proveedor">Gln proveedor.</param>
		/// <param name="gln_comprador">Gln comprador.</param>
		/// <returns></returns>
		public List<string> ObtenerListadoGeneralAdicion(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador)
		{
			string SQL = @"SELECT 
							it_gtin 
						 FROM 
						   Documentos 
						   INNER JOIN Items ON do_numero_doc=it_numero_doc AND do_nombre_doc=it_nombre_doc 
											   AND do_gln_proveedor=it_gln_proveedor AND do_gln_comprador=it_gln_comprador 
						WHERE 
							do_numero_doc=@NRO_DOC AND 
							do_nombre_doc=@NOMB_DOC 
							AND do_gln_proveedor=@GLN_PROV AND 
							do_gln_comprador=@GLN_COMP";
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
						listado.Add(dr.GetString(0));
					}
					dr.Close();
				}
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de items: " + ex.Message);
			}
			return listado;
		}
		public List<string> ObtenerListadoGeneralCambioPrecio(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador)
		{
			string SQL = "SELECT " +
							"cp_gtin " +
						 "FROM " +
							"Documentos " +
						"INNER JOIN " +
							"CambioPrecio ON do_numero_doc=cp_numero_doc AND do_nombre_doc=cp_nombre_doc " +
										  "AND do_gln_proveedor=cp_gln_proveedor AND do_gln_comprador=cp_gln_comprador " +
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
						listado.Add(dr.GetString(0));
					}
					dr.Close();
				}
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de items para cambio de precio: " + ex.Message);
			}
			return listado;
		}

		/// <summary>
		/// Actualiza la información de un item.
		/// </summary>
		/// <param name="valores">Valores que reciben los parametros.</param>
		public int ActualizarItem(object[] valores)
		{
			string SQL = "SP_ActualizarItem";
			string SQL2 = "SP_ActualizarItemAceptado";
			int nro = 0;
			try
			{
				if (Convert.ToBoolean(valores[13]).Equals(true))
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd = new SqlCommand(SQL, conn);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
					cmd.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
					cmd.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
					cmd.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
					cmd.Parameters.AddWithValue("@GTIN", valores[0]);
					cmd.Parameters.AddWithValue("@DESC_LARGA", valores[1]);
					cmd.Parameters.AddWithValue("@DESC_CORTA", valores[2]);
					cmd.Parameters.AddWithValue("@GRUPO_IMP", valores[3]);
					cmd.Parameters.AddWithValue("@TIPO_INV", valores[4]);
					cmd.Parameters.AddWithValue("@UND_INV", valores[5]);
					cmd.Parameters.AddWithValue("@FACT_PESO_INV", Convert.ToDecimal(valores[6]));
					cmd.Parameters.AddWithValue("@UND_ORDEN", valores[7]);
					cmd.Parameters.AddWithValue("@FACT_ORDEN", Convert.ToDecimal(valores[8]));
					cmd.Parameters.AddWithValue("@FACT_PESO_ORDEN", Convert.ToDecimal(valores[9]));
					cmd.Parameters.AddWithValue("@UND_EMP", valores[10]);
					cmd.Parameters.AddWithValue("@FACT_EMP", Convert.ToDecimal(valores[11]));
					cmd.Parameters.AddWithValue("@FACT_PESO_EMP", Convert.ToDecimal(valores[12]));
					cmd.Parameters.AddWithValue("@ACEPTADO", valores[13]);
					cmd.Parameters.AddWithValue("@IND_COMPRA", valores[15]);
					cmd.Parameters.AddWithValue("@IND_VTA", valores[16]);
					cmd.Parameters.AddWithValue("@IND_MANUFACTURA", valores[17]);

					nro = cmd.ExecuteNonQuery();
					conn.Close();
				}
				else
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd = new SqlCommand(SQL2, conn);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
					cmd.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
					cmd.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
					cmd.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
					cmd.Parameters.AddWithValue("@GTIN", valores[0]);
					cmd.Parameters.AddWithValue("@ACEPTADO", valores[13]);
					cmd.Parameters.AddWithValue("@MOTIVO", valores[14]);
					cmd.ExecuteNonQuery();
					conn.Close();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar el item: " + ex.Message);
			}
			return nro;
		}

		public void ActualizarItemCotizacion(object[] valores)
		{
			string SQL = "SP_ActualizarItemCotizacion";
			string SQL2 = "SP_ActualizarItemCotizacionAceptado";

			try
			{
				if (Convert.ToBoolean(valores[4]).Equals(true))
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd = new SqlCommand(SQL, conn);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
					cmd.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
					cmd.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
					cmd.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
					cmd.Parameters.AddWithValue("@GTIN", valores[0]);
					cmd.Parameters.AddWithValue("@UND_MED", valores[1]);
					cmd.Parameters.AddWithValue("@FECHA_ACT", valores[2]);
					cmd.Parameters.AddWithValue("@FECHA_HASTA", /*valores[3]*/DBNull.Value);
					cmd.Parameters.AddWithValue("@ACEPTADO", valores[4]);
					cmd.ExecuteNonQuery();
					conn.Close();
				}
				else
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd = new SqlCommand(SQL2, conn);
					cmd.CommandType = CommandType.StoredProcedure;
					cmd.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
					cmd.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
					cmd.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
					cmd.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
					cmd.Parameters.AddWithValue("@GTIN", valores[0]);
					cmd.Parameters.AddWithValue("@ACEPTADO", valores[4]);
					cmd.ExecuteNonQuery();
					conn.Close();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar el item cotización: " + ex.Message);
			}
		}

		public void ActualizarItemDescripcionTecnica(object[] valores)
		{
			string SQL = "SP_ActualizarItemDescripcionTecnica";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
				cmd.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
				cmd.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
				cmd.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
				cmd.Parameters.AddWithValue("@GTIN", valores[0]);

				cmd.Parameters.AddWithValue("@ALTO", Convert.ToDecimal(valores[1]));
				cmd.Parameters.AddWithValue("@ANCHO",Convert.ToDecimal(valores[2]));
				cmd.Parameters.AddWithValue("@PROFUNDO", Convert.ToDecimal(valores[3]));
				cmd.Parameters.AddWithValue("@ALTO_EMP", Convert.ToDecimal(valores[4]));
				cmd.Parameters.AddWithValue("@ANCHO_EMP", Convert.ToDecimal(valores[5]));
				cmd.Parameters.AddWithValue("@PROFUNDO_EMP", Convert.ToDecimal(valores[6]));

				string desc_tec1 = Convert.ToString(valores[7]);
				cmd.Parameters.AddWithValue("@DESCRIPCION_TEC1", desc_tec1.Equals("")?(object)DBNull.Value: desc_tec1);
				string desc_tec2 = Convert.ToString(valores[8]);
				cmd.Parameters.AddWithValue("@DESCRIPCION_TEC2", desc_tec2.Equals("") ? (object)DBNull.Value : desc_tec2);
				string desc_tec3 = Convert.ToString(valores[9]);
				cmd.Parameters.AddWithValue("@DESCRIPCION_TEC3", desc_tec3.Equals("") ? (object)DBNull.Value : desc_tec3);

				cmd.Parameters.AddWithValue("@ACEPTADO", valores[10]);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar el estado de aceptado en Descripción Técnica: " + ex.Message);
			}
		}

		public DataTable ObtenerEquivalenciasCategoriaLogyca(string categoria)
		{
			string SQL = "select " +
							"ca_plan, " +
							"ca_desc_plan, " +
							"ca_criterio, " +
							"ca_desc_mayor " +
						 "from " +
							"Categorias " +
						 "where " +
							"ca_catlogyca=@CAT_LOGYCA";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@CAT_LOGYCA", categoria);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener la equivalencia de la categoría Logyca: " + ex.Message);
			}
			return dt;
		}

		public void ActualizarItemCriteriosClasificacion(string gtin, DataGridView dgv, bool aceptado)
		{
			string SQL1 = "DELETE " +
							"CriteriosClasificacion " +
						  "WHERE " +
							"cc_numero_doc=@NRO_DOC AND " +
							"cc_nombre_doc=@NOMB_DOC AND " +
							"cc_gln_proveedor=@GLN_PROV AND " +
							"cc_gln_comprador=@GLN_COMP AND " +
							"cc_gtin=@GTIN";
			string SQL2 = "INSERT INTO " +
							"CriteriosClasificacion " +
							"(cc_numero_doc, " +
							"cc_nombre_doc, " +
							"cc_gln_proveedor, " +
							"cc_gln_comprador, " +
							"cc_gtin, " +
							"cc_plan, " +
							"cc_desc_plan, " +
							"cc_criterio_mayor, " +
							"cc_desc_criterio) " +
						  "VALUES(" +
							"@NRO_DOC, " +
							"@NOMB_DOC, " +
							"@GLN_PROV, " +
							"@GLN_COMP, " +
							"@GTIN, " +
							"@PLAN, " +
							"@DESC_PLAN, " +
							"@CRITERIO, " +
							"@DESC_CRITERIO)";

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd1 = new SqlCommand(SQL1, conn, trans);
				cmd1.CommandType = CommandType.Text;
				cmd1.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
				cmd1.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
				cmd1.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
				cmd1.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
				cmd1.Parameters.AddWithValue("@GTIN", gtin);
				cmd1.ExecuteNonQuery();
				if (aceptado)
				{
					for (int i = 0; i < dgv.RowCount; i++)
					{
						SqlCommand cmd2 = new SqlCommand(SQL2, conn, trans);
						cmd2.CommandType = CommandType.Text;
						cmd2.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
						cmd2.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
						cmd2.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
						cmd2.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
						cmd2.Parameters.AddWithValue("@GTIN", gtin);
						cmd2.Parameters.AddWithValue("@PLAN", dgv[0, i].Value);
						cmd2.Parameters.AddWithValue("@DESC_PLAN", dgv[1, i].Value);
						cmd2.Parameters.AddWithValue("@CRITERIO", dgv[2, i].Value);
						cmd2.Parameters.AddWithValue("@DESC_CRITERIO", dgv[3, i].Value);
						cmd2.ExecuteNonQuery();
					}
				}
				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al actualizar los Criterios de NEGOCIACION: " + ex.Message);
			}
		}

		public void ActualizarItemParametrosPlaneacion(string gtin, DataGridView dgv, bool aceptado)
		{
			string SQL1 = "DELETE " +
							"ParametrosPlaneacion " +
						  "WHERE " +
							"pp_numero_doc=@NRO_DOC AND " +
							"pp_nombre_doc=@NOMB_DOC AND " +
							"pp_gln_proveedor=@GLN_PROV AND " +
							"pp_gln_comprador=@GLN_COMP AND " +
							"pp_gtin=@GTIN";
			string SQL2 = "INSERT INTO " +
							"ParametrosPlaneacion " +
								"(pp_numero_doc, " +
								"pp_nombre_doc, " +
								"pp_gln_proveedor, " +
								"pp_gln_comprador, " +
								"pp_gtin, " +
								"pp_instalacion, " +
								"pp_bodega_asig, " +
								"pp_rotacion_veces, " +
								"pp_rotacion_costo, " +
								"pp_und_vta, " +
								"pp_periodo_cubrimiento, " +
								"pp_tmpo_reposicion, " +
								"pp_tmpo_seguridad, " +
								"pp_comprador, " +
								"pp_dias_horiz_plan, " +
								"pp_stock_seguridad_estatico, " +
								"pp_dias_horiz_stock_min, " +
								"pp_dias_stock_min, " +
								"pp_tmpo_reposicion_fijo, " +
								"pp_politica_orden, " +
								"pp_tamaño_lote, " +
								"pp_tercero_prov, " +
								"pp_razon_soc, " +
								"pp_sucursal_prov, " +
								"pp_gen_orden_plan) " +
						  "VALUES(" +
							  "@NRO_DOC, " +
							  "@NOMB_DOC, " +
							  "@GLN_PROV, " +
							  "@GLN_COMP, " +
							  "@GTIN, " +
							  "@INSTALACION, " +
							  "@BODEGA_ASIG, " +
							  "@ROTAC_VECES, " +
							  "@ROTAC_COSTO, " +
							  "@UND_VTA, " +
							  "@PERIODO_CUB, " +
							  "@TMPO_REP," +
							  " @TMPO_SEG, " +
							  "@COMPRADOR, " +
							  "@DIAS_HORIZ_PLAN, " +
							  "@STOCK_SEG_STATICO, " +
							  "@DIAS_HORIZ_STOCK_MIN, " +
							  "@DIAS_STOCK_MIN, " +
							  "@TMPO_REP_FIJO, " +
							  "@POLITICA_ORDEN, " +
							  "@TAM_LOTE, " +
							  "@PROVEEDOR, " +
							  "@RAZON_SOC, " +
							  "@SUCURSAL, " +
							  "@GEN_ORDEN_PLAN)";

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd1 = new SqlCommand(SQL1, conn, trans);
				cmd1.CommandType = CommandType.Text;
				cmd1.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
				cmd1.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
				cmd1.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
				cmd1.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
				cmd1.Parameters.AddWithValue("@GTIN", gtin);
				cmd1.ExecuteNonQuery();
				if (aceptado)
				{
					for (int i = 0; i < dgv.RowCount; i++)
					{
						SqlCommand cmd2 = new SqlCommand(SQL2, conn, trans);
						cmd2.CommandType = CommandType.Text;
						cmd2.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
						cmd2.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
						cmd2.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
						cmd2.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
						cmd2.Parameters.AddWithValue("@GTIN", gtin);

						cmd2.Parameters.AddWithValue("@INSTALACION", dgv[0, i].Value);
						cmd2.Parameters.AddWithValue("@BODEGA_ASIG", dgv[1, i].Value);
						cmd2.Parameters.AddWithValue("@ROTAC_VECES", dgv[2, i].Value);
						cmd2.Parameters.AddWithValue("@ROTAC_COSTO", dgv[3, i].Value);
						cmd2.Parameters.AddWithValue("@UND_VTA", dgv[4, i].Value);
						cmd2.Parameters.AddWithValue("@PERIODO_CUB", Convert.ToInt32(dgv[5, i].Value));
						cmd2.Parameters.AddWithValue("@TMPO_REP", Convert.ToInt32(dgv[6, i].Value));
						cmd2.Parameters.AddWithValue("@TMPO_SEG", Convert.ToInt32(dgv[7, i].Value));
						cmd2.Parameters.AddWithValue("@COMPRADOR", dgv[8, i].Value);
						cmd2.Parameters.AddWithValue("@DIAS_HORIZ_PLAN", Convert.ToInt32(dgv[9, i].Value));
						cmd2.Parameters.AddWithValue("@STOCK_SEG_STATICO", Convert.ToDecimal(dgv[10, i].Value));
						cmd2.Parameters.AddWithValue("@DIAS_HORIZ_STOCK_MIN", Convert.ToInt32(dgv[11, i].Value));
						cmd2.Parameters.AddWithValue("@DIAS_STOCK_MIN", Convert.ToInt32(dgv[12, i].Value));
						cmd2.Parameters.AddWithValue("@TMPO_REP_FIJO", Convert.ToInt32(dgv[13, i].Value));
						cmd2.Parameters.AddWithValue("@POLITICA_ORDEN", Convert.ToInt32(dgv[14, i].Value));
						cmd2.Parameters.AddWithValue("@TAM_LOTE", Convert.ToDecimal(dgv[15, i].Value));
						cmd2.Parameters.AddWithValue("@PROVEEDOR", dgv[16, i].Value);
						cmd2.Parameters.AddWithValue("@RAZON_SOC", dgv[17, i].Value);
						cmd2.Parameters.AddWithValue("@SUCURSAL", dgv[18, i].Value);
						DataGridViewCheckBoxCell chk = dgv[19, i] as DataGridViewCheckBoxCell;
						if (Convert.ToBoolean(chk.EditedFormattedValue) == true)
						{
							cmd2.Parameters.AddWithValue("@GEN_ORDEN_PLAN", true);
						}
						else
						{
							cmd2.Parameters.AddWithValue("@GEN_ORDEN_PLAN", false);
						}

						cmd2.ExecuteNonQuery();
					}
				}
				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al actualizar los Parametros de Planeación: " + ex.Message);
			}
		}

		public void ActualizarItemPrecioVenta(string gtin, DataGridView dgv, bool aceptado, string um)
		{
			string SQL1 = "DELETE " +
							"PrecioVenta " +
						  "WHERE " +
							"pv_numero_doc=@NRO_DOC AND " +
							"pv_nombre_doc=@NOMB_DOC AND " +
							"pv_gln_proveedor=@GLN_PROV AND " +
							"pv_gln_comprador=@GLN_COMP AND " +
							"pv_gtin=@GTIN";
			string SQL2 = "INSERT INTO " +
							"PrecioVenta " +
							"(pv_numero_doc, " +
							"pv_nombre_doc, " +
							"pv_gln_proveedor, " +
							"pv_gln_comprador, " +
							"pv_gtin, " +
							"pv_co, " +
							"pv_unidad_medida, " +
							"pv_lista_precio, " +
							"pv_precio, " +
							"pv_fecha_activacion, " +
							"pv_fecha_inactivacion, " +
							"pv_margen) " +
						  "VALUES(" +
							"@NRO_DOC, " +
							"@NOMB_DOC, " +
							"@GLN_PROV, " +
							"@GLN_COMP, " +
							"@GTIN, " +
							"@CO, " +
							"@UNIDAD_MEDIDA, " +
							"@LISTA_PRECIO, " +
							"@PRECIO, " +
							"@FECHA_ACT, " +
							"@FECHA_INACT, " +
							"@MARGEN)";

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd1 = new SqlCommand(SQL1, conn, trans);
				cmd1.CommandType = CommandType.Text;
				cmd1.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
				cmd1.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
				cmd1.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
				cmd1.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
				cmd1.Parameters.AddWithValue("@GTIN", gtin);
				cmd1.ExecuteNonQuery();
				if (aceptado)
				{
					for (int i = 0; i < dgv.RowCount; i++)
					{
						SqlCommand cmd2 = new SqlCommand(SQL2, conn, trans);
						cmd2.CommandType = CommandType.Text;
						cmd2.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
						cmd2.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
						cmd2.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
						cmd2.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
						cmd2.Parameters.AddWithValue("@GTIN", gtin);

						cmd2.Parameters.AddWithValue("@CO", "");
						cmd2.Parameters.AddWithValue("@UNIDAD_MEDIDA", um);
						cmd2.Parameters.AddWithValue("@LISTA_PRECIO", dgv[0, i].Value);
						cmd2.Parameters.AddWithValue("@PRECIO", dgv[1, i].Value);
						cmd2.Parameters.AddWithValue("@FECHA_ACT", dgv[2, i].Value);
						cmd2.Parameters.AddWithValue("@FECHA_INACT", DBNull.Value);
						cmd2.Parameters.AddWithValue("@MARGEN", dgv[5, i].Value);
						cmd2.ExecuteNonQuery();
					}
				}
				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al actualizar los Precios de venta: " + ex.Message);
			}
		}

		public int VerificarItemsPrecioVenta()
		{
			string SQL = "select " +
							"COUNT(*) " +
						 "from " +
							"PrecioVenta " +
						"where " +
							"pv_numero_doc = @NRO_DOC AND " +
							"pv_nombre_doc = @NOMB_DOC AND " +
							"pv_gln_proveedor = @GLN_PROV AND " +
							"pv_gln_comprador = @GLN_COMP";

			int res;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
				cmd.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
				cmd.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
				cmd.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
				res = (int)cmd.ExecuteScalar();
				conn.Close();
				return res;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al verificar los items del Precio de Venta: " + ex.Message);
			}
		}

		public void ActualizarItemPrecioVenta(DataGridView dgv)
		{
			string SQL1 = "DELETE " +
							"PrecioVenta " +
						  "WHERE " +
							"pv_numero_doc=@NRO_DOC AND " +
							"pv_nombre_doc=@NOMB_DOC AND " +
							"pv_gln_proveedor=@GLN_PROV AND " +
							"pv_gln_comprador=@GLN_COMP";

			string SQL2 = "INSERT INTO " +
							"PrecioVenta " +
							"(" +
								"pv_numero_doc, " +
								"pv_nombre_doc, " +
								"pv_gln_proveedor, " +
								"pv_gln_comprador, " +
								"pv_gtin, " +
								"pv_co, " +
								"pv_unidad_medida, " +
								"pv_lista_precio, " +
								"pv_precio, " +
								"pv_fecha_activacion, " +
								"pv_fecha_inactivacion, " +
								"pv_margen" +
							") " +
						  "VALUES" +
						  "(" +
							"@NRO_DOC, " +
							"@NOMB_DOC, " +
							"@GLN_PROV, " +
							"@GLN_COMP, " +
							"@GTIN, " +
							"@CO, " +
							"@UNIDAD_MEDIDA, " +
							"@LISTA_PRECIO, " +
							"@PRECIO, " +
							"@FECHA_ACT, " +
							"@FECHA_INACT, " +
							"@MARGEN" +
						  ")";

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd1 = new SqlCommand(SQL1, conn, trans);
				cmd1.CommandType = CommandType.Text;
				cmd1.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
				cmd1.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
				cmd1.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
				cmd1.Parameters.AddWithValue("@GLN_COMP", GlnComprador);

				cmd1.ExecuteNonQuery();

				for (int i = 0; i < dgv.Rows.Count; i++)
				{
					if (Convert.ToBoolean(dgv[0, i].Value))
					{
						for (int j = 26; j < dgv.Columns.Count - 2; j++)
						{
							if (Convert.ToString(dgv[j, i].Value).Trim() != "")
							{
								SqlCommand cmd2 = new SqlCommand(SQL2, conn, trans);
								cmd2.CommandType = CommandType.Text;
								cmd2.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
								cmd2.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
								cmd2.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
								cmd2.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
								cmd2.Parameters.AddWithValue("@GTIN", dgv[2, i].Value);

								cmd2.Parameters.AddWithValue("@CO", "");
								cmd2.Parameters.AddWithValue("@UNIDAD_MEDIDA", dgv[5, i].Value);
								cmd2.Parameters.AddWithValue("@LISTA_PRECIO", dgv.Columns[j].HeaderText.Split('-')[0].Trim());
								cmd2.Parameters.AddWithValue("@PRECIO", dgv[j, i].Value);
								if (Convert.ToString(dgv[dgv.ColumnCount - 2, i].Value) == "")
								{
									cmd2.Parameters.AddWithValue("@FECHA_ACT", DBNull.Value);
								}
								else
								{
									cmd2.Parameters.AddWithValue("@FECHA_ACT", Convert.ToString(dgv[dgv.ColumnCount - 2, i].Value));
								}
								if (Convert.ToString(dgv[dgv.ColumnCount - 1, i].Value) == "")
								{
									cmd2.Parameters.AddWithValue("@FECHA_INACT", DBNull.Value);
								}
								else
								{
									cmd2.Parameters.AddWithValue("@FECHA_INACT", Convert.ToString(dgv[dgv.ColumnCount - 1, i].Value));
								}
								decimal margen = Convert.ToDecimal(dgv[j, i].Tag);
								cmd2.Parameters.AddWithValue("@Margen", margen);
								cmd2.ExecuteNonQuery();
								cmd2.Parameters.Clear();
							}
						}
					}
				}

				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al actualizar los Precios de venta: " + ex.Message);
			}
		}

		public void ActualizarItemPortafolio(string gtin, DataGridView dgv, bool aceptado)
		{
			string SQL1 = "DELETE " +
							"Portafolio " +
						  "WHERE " +
							"po_numero_doc=@NRO_DOC AND " +
							"po_nombre_doc=@NOMB_DOC AND " +
							"po_gln_proveedor=@GLN_PROV AND " +
							"po_gln_comprador=@GLN_COMP AND " +
							"po_gtin=@GTIN";

			string SQL2 = "INSERT INTO " +
							"Portafolio " +
							"(" +
								"po_numero_doc, " +
								"po_nombre_doc, " +
								"po_gln_proveedor, " +
								"po_gln_comprador, " +
								"po_gtin, " +
								"po_id_portafolio, " +
								"po_descripcion, " +
								"po_nota) " +
						  "VALUES" +
						  "(" +
							"@NRO_DOC, " +
							"@NOMB_DOC, " +
							"@GLN_PROV, " +
							"@GLN_COMP, " +
							"@GTIN, " +
							"@PORT, " +
							"@DESC, " +
							"@NOTA)";

			SqlTransaction trans = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				trans = conn.BeginTransaction();
				SqlCommand cmd1 = new SqlCommand(SQL1, conn, trans);
				cmd1.CommandType = CommandType.Text;
				cmd1.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
				cmd1.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
				cmd1.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
				cmd1.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
				cmd1.Parameters.AddWithValue("@GTIN", gtin);
				cmd1.ExecuteNonQuery();
				if (aceptado)
				{
					for (int i = 0; i < dgv.RowCount; i++)
					{
						SqlCommand cmd2 = new SqlCommand(SQL2, conn, trans);
						cmd2.CommandType = CommandType.Text;
						cmd2.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
						cmd2.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
						cmd2.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
						cmd2.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
						cmd2.Parameters.AddWithValue("@GTIN", gtin);

						cmd2.Parameters.AddWithValue("@PORT", dgv[0, i].Value);
						cmd2.Parameters.AddWithValue("@DESC", dgv[1, i].Value);
						cmd2.Parameters.AddWithValue("@NOTA", dgv[2, i].Value);

						cmd2.ExecuteNonQuery();
					}
				}
				trans.Commit();
				conn.Close();
			}
			catch (Exception ex)
			{
				trans.Rollback();
				throw new Exception("Error al actualizar los Portafolios: " + ex.Message);
			}
		}

		public void ActualizarItemCodigoBarras(string gtin, object[] valores)
		{
			string SQL = "UPDATE " +
							"CodigoBarras " +
						 "SET " +
							"br_unidad_medida=@UND_MED, " +
							"br_cantidad_unidad_medida=@CANT_UND_MED, " +
							"br_tipo=@TIPO, " +
							"br_ind_operacion=@IND_OPERACION, " +
							"br_factor_operacion=@FACT_OPERACION, " +
							"br_aceptado=@ACEPTADO, " +
							"br_principal=@PPAL " +
						 "WHERE " +
							"br_numero_doc=@NRO_DOC AND " +
							"br_nombre_doc=@NOMB_DOC AND " +
							"br_gln_proveedor=@GLN_PROV AND " +
							"br_gln_comprador=@GLN_COMP AND " +
							"br_gtin=@GTIN AND " +
							"(SELECT " +
								"do_estado " +
							"FROM " +
								"Documentos " +
							"WHERE " +
								"do_numero_doc=@NRO_DOC AND " +
								"do_nombre_doc=@NOMB_DOC AND " +
								"do_gln_proveedor=@GLN_PROV AND " +
								"do_gln_comprador=@GLN_COMP)= 0";

			string SQL2 = "UPDATE " +
							"CodigoBarras " +
						  "SET " +
							"br_aceptado=@ACEPTADO " +
						 "WHERE " +
							"br_numero_doc=@NRO_DOC AND " +
							"br_nombre_doc=@NOMB_DOC AND " +
							"br_gln_proveedor=@GLN_PROV AND " +
							"br_gln_comprador=@GLN_COMP AND " +
							"br_gtin=@GTIN AND " +
							"(SELECT " +
								"do_estado " +
							"FROM " +
								"Documentos " +
							"WHERE " +
								"do_numero_doc=@NRO_DOC AND " +
								"do_nombre_doc=@NOMB_DOC AND " +
								"do_gln_proveedor=@GLN_PROV AND " +
								"do_gln_comprador=@GLN_COMP)= 0";
			try
			{
				if (Convert.ToBoolean(valores[5]).Equals(true))
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd = new SqlCommand(SQL, conn);
					cmd.CommandType = CommandType.Text;
					cmd.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
					cmd.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
					cmd.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
					cmd.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
					cmd.Parameters.AddWithValue("@GTIN", gtin);
					cmd.Parameters.AddWithValue("@UND_MED", valores[0]);
					cmd.Parameters.AddWithValue("@CANT_UND_MED", Convert.ToDecimal(valores[1]));
					cmd.Parameters.AddWithValue("@TIPO", valores[2]);
					cmd.Parameters.AddWithValue("@IND_OPERACION", valores[3]);
					cmd.Parameters.AddWithValue("@FACT_OPERACION", Convert.ToDecimal(valores[4]));
					cmd.Parameters.AddWithValue("@ACEPTADO", valores[5]);
					cmd.Parameters.AddWithValue("@PPAL", valores[6]);
					cmd.ExecuteNonQuery();
					conn.Close();
				}
				else
				{
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd = new SqlCommand(SQL2, conn);
					cmd.CommandType = CommandType.Text;
					cmd.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
					cmd.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
					cmd.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
					cmd.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
					cmd.Parameters.AddWithValue("@GTIN", gtin);
					cmd.Parameters.AddWithValue("@ACEPTADO", valores[5]);
					cmd.ExecuteNonQuery();
					conn.Close();
				}
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar la información de código de barras: " + ex.Message);
			}
		}

		public void ActualizarOtrosDatos(string gtin, object[] valores)
		{
			string SQL = @"update
								OtrosDatos
							set
								ot_cant_contenida=@cant_contenida,
								ot_id_calificador_cantidad_contenida=@id_calificador_cantidad_contenida
							where
								ot_numero_doc=@nro_doc
								and ot_nombre_doc=@nomb_doc
								and ot_gln_proveedor=@gln_prov
								and ot_gln_comprador=@gln_comp
								and ot_gtin=@gtin
								and 
								(
									select 
										do_estado 
									from 
										Documentos 
									where 
										do_numero_doc=@nro_doc AND 
										do_nombre_doc=@nomb_doc AND 
										do_gln_proveedor=@gln_prov AND 
										do_gln_comprador=@gln_comp
								)= 0";

			try
			{
				
					SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
					conn.Open();
					SqlCommand cmd = new SqlCommand(SQL, conn);
					cmd.CommandType = CommandType.Text;
					cmd.Parameters.AddWithValue("@nro_doc", NumeroDocumento);
					cmd.Parameters.AddWithValue("@nomb_doc", NombreDocumento);
					cmd.Parameters.AddWithValue("@gln_prov", GlnProveedor);
					cmd.Parameters.AddWithValue("@gln_comp", GlnComprador);
					cmd.Parameters.AddWithValue("@gtin", gtin);
					cmd.Parameters.AddWithValue("@cant_contenida", Convert.ToDecimal(valores[0]));
					cmd.Parameters.AddWithValue("@id_calificador_cantidad_contenida", valores[1]);
					cmd.ExecuteNonQuery();
					conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar la información de otros datos: " + ex.Message);
			}
		}

		public void ActualizarItemCambioPrecio(object[] valores)
		{
			string SQL = "UPDATE " +
							"CambioPrecio " +
						 "SET " +
							"cp_fecha_act=@FECHA_ACT, " +
							"cp_fecha_hasta=@FECHA_INACT, " +
							"cp_aceptado=@ACEPTADO, " +
							"cp_motivo=@MOTIVO " +
						 "WHERE " +
							"cp_numero_doc=@NRO_DOC AND " +
							"cp_nombre_doc=@NOMB_DOC AND " +
							"cp_gln_proveedor=@GLN_PROV AND " +
							"cp_gln_comprador=@GLN_COMP AND " +
							"cp_gtin=@GTIN AND " +
							"(SELECT " +
								"do_estado " +
							 "FROM " +
									"Documentos " +
							"WHERE " +
								"do_numero_doc=@NRO_DOC AND " +
								"do_nombre_doc=@NOMB_DOC AND " +
								"do_gln_proveedor=@GLN_PROV AND " +
								"do_gln_comprador=@GLN_COMP)=0";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
				cmd.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
				cmd.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
				cmd.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
				cmd.Parameters.AddWithValue("@GTIN", valores[0]);
				if (Convert.ToString(valores[1]) == "")
				{
					cmd.Parameters.AddWithValue("@FECHA_ACT", DBNull.Value);
				}
				else
				{
					cmd.Parameters.AddWithValue("@FECHA_ACT", valores[1]);
				}
				if (Convert.ToString(valores[2]) == "")
				{
					cmd.Parameters.AddWithValue("@FECHA_INACT", DBNull.Value);
				}
				else
				{
					cmd.Parameters.AddWithValue("@FECHA_INACT", valores[2]);
				}
				cmd.Parameters.AddWithValue("@ACEPTADO", valores[3]);
				if (valores[4] == null)
				{
					cmd.Parameters.AddWithValue("@MOTIVO", "");
				}
				else
				{
					cmd.Parameters.AddWithValue("@MOTIVO", valores[4]);
				}
				/* cmd.Parameters.AddWithValue("@FECHA_INI_DES", valores[5]);
                 cmd.Parameters.AddWithValue("@FECHA_FIN_DES", valores[6]);*/
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar el item cotización: " + ex.Message);
			}
		}

		public void ActualizarDocumento()
		{
			string SQL = "UPDATE " +
							"Documentos " +
						 "SET " +
							"do_estado=1, " +
							"do_fecha=GETDATE() " +
						 "WHERE " +
							"do_numero_doc=@NRO_DOC AND " +
							"do_nombre_doc=@NOMB_DOC AND " +
							"do_gln_proveedor=@GLN_PROV AND " +
							"do_gln_comprador=@GLN_COMP";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@NRO_DOC", NumeroDocumento);
				cmd.Parameters.AddWithValue("@NOMB_DOC", NombreDocumento);
				cmd.Parameters.AddWithValue("@GLN_PROV", GlnProveedor);
				cmd.Parameters.AddWithValue("@GLN_COMP", GlnComprador);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al actualizar el item cotización: " + ex.Message);
			}
		}

		/// <summary>
		/// Obtiene el listado de centros de operación.
		/// </summary>
		/// <returns>Devuelve un objeto de tipo datatable que contiene la información.</returns>
		public DataTable ListarCentrosOperacion()
		{
			Configuration config = ConfigurationManager.OpenExeConfiguration(Application.StartupPath + "\\" + Application.ProductName + ".exe");
			AppSettingsSection section = config.AppSettings;
			string no_instalacion = section.Settings["no_instalacion"].Value.ToString();

			string SQL = $@"SELECT
							f285_id, f285_id + '-' + f285_descripcion AS f285_descripcion
						 FROM
							t285_co_centro_op
						 INNER JOIN t280_co_regionales ON f285_id_regional=f280_id AND f280_id_cia=f285_id_cia
						 WHERE
							f285_id_cia=1 AND
							f285_id NOT IN({no_instalacion}) AND
							f285_id_portafolio IS NOT NULL
						 ORDER BY
							f285_id, f285_id_regional";

			DataTable dt = null;

			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, Conexion.CadenaConexionUnoee);
				dt = new DataTable();
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al listar los centros de operación: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Obtiene el listado de listas de precio de un centro de operación.
		/// </summary>
		/// <param name="co">Devuelve un objeto datatable que contiene la información.</param>
		/// <returns></returns>
		public DataTable ObtenerListasPrecio(string co)
		{
			string SQL = "select " +
							"f112_id " +
							",f112_id + ' - ' + f112_descripcion f112_descripcion " +
						"from " +
							"t285_co_centro_op " +
						"inner join " +
							"t1121_mc_listas_precios_co " +
							"on " +
							"f1121_id_co=f285_id and f1121_id_cia=f285_id_cia " +
						"inner join " +
							"t112_mc_listas_precios " +
							 "on f112_id=f1121_id_lista_precio and f112_id_cia=f1121_id_cia " +
						"where " +
							"f112_ind_estado=1 and  f112_ind_tipo_lista=1 and f285_id_cia=1 and f285_id=@CO " +
						"order by 1";
			DataTable dt = null;
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, Conexion.CadenaConexionUnoee);
				da.SelectCommand.Parameters.AddWithValue("@CO", co);
				dt = new DataTable();
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener las listas de precio: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Obtiene las listas de precio.
		/// </summary>
		/// <returns></returns>
		public DataTable ObtenerListasPrecio()
		{
			string SQL = "SP_Listas_de_precio";
			DataTable dt = null;
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, Conexion.CadenaConexionLogyca);

				dt = new DataTable();
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener las listas de precio: " + ex.Message);
			}
			return dt;
		}

		public DataTable ObtenerListaPrecioPVP()
		{
			string SQL = "SELECT " +
							"DISTINCT " +
							"pv_lista_precio " +
						 "FROM " +
							"Documentos " +
						 "INNER JOIN PrecioVenta ON do_numero_doc = pv_numero_doc AND " +
													"do_nombre_doc = pv_nombre_doc AND " +
													"do_gln_proveedor = pv_gln_proveedor AND " +
													"do_gln_comprador = pv_gln_comprador " +
						"WHERE " +
							"do_accion = '3'";

			DataTable dt = null;
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, Conexion.CadenaConexionLogyca);

				dt = new DataTable();
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener las listas de precio desde PrecioVenta: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Obtiene el listado general de portafolios.
		/// </summary>
		/// <returns>Devuelve un objeto datatable que contiene la información.</returns>
		public DataTable ListarPortafolios()
		{
			string SQL = "select " +
							"rtrim(f136_id) f136_id, " +
							"rtrim(f136_descripcion) f136_descripcion " +
						"from " +
							"t136_mc_portafolio " +
							"inner join t285_co_centro_op on f285_id_portafolio=f136_id and f285_id_cia=f136_id_cia " +
						"where " +
							"f136_id_cia=1 and " +
							"f285_id not in('115','701')";
			DataTable dt = null;
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, Conexion.CadenaConexionUnoee);
				dt = new DataTable();
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener los portafolios: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Obtiene la nota de un portafolio.
		/// </summary>
		/// <param name="id">Id del portafolio.</param>
		/// <returns>Devuele la nota, si no hay nota devuelve la misma descripción del portafolio.</returns>
		public string ObtenerNotaPortafolio(string id)
		{
			string SQL = "select " +
							"case " +
								"f136_notas " +
								"when null then f136_descripcion " +
								"when '' then f136_descripcion " +
							"else " +
								"f136_notas " +
							"end as f136_notas " +
						"from " +
							"t136_mc_portafolio " +
						"where " +
							"f136_id_cia=1 and " +
							"f136_id=@ID";
			string res = "";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@ID", id);
				res = Convert.ToString(cmd.ExecuteScalar());
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener la nota del portafolio: " + ex.Message);
			}
			return res;
		}

		/// <summary>
		/// Obtiene el listado de compradores.
		/// </summary>
		/// <returns>Devuelve un objeto de tipo datatable que contiene la información.</returns>
		public DataTable ListarCompradoresUnoee()
		{
			string SQL = "select " +
							"f211_id, f211_id + '-' + f200_razon_social AS f200_razon_social " +
						 "from " +
							"t211_mm_funcionarios " +
							"inner join t200_mm_terceros AS t1 on f211_rowid_tercero=t1.f200_rowid " +
															"and f211_id_cia=t1.f200_id_cia " +
						 "where " +
							"f211_ind_comprador=1 and " +
							"f211_id_cia=1 order by 1";

			DataTable dt = null;
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, Conexion.CadenaConexionUnoee);
				dt = new DataTable();
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al listar los compradores: " + ex.Message);
			}
			return dt;
		}

		public void GuardarProveedor(string gln, string nit, string razonsocial, string emailnotificacion)
		{
			string SQL = "IF NOT EXISTS(" +
										"SELECT " +
											"* " +
										"FROM " +
											"Proveedores " +
									   "WHERE " +
										"pr_gln=@GLN AND " +
										"pr_nit=@NIT AND " +
										"pr_razon_soc=@RAZON_SOC) " +
								"INSERT INTO " +
									"Proveedores " +
									"(" +
										"pr_gln, " +
										"pr_nit, " +
										"pr_razon_soc, " +
										"pr_email_notif) " +
								"VALUES" +
								"(" +
									"@GLN, " +
									"@NIT, " +
									"@RAZON_SOC, " +
									"@EMAIL_NOTIF) " +
						"ELSE " +
							"UPDATE " +
								"Proveedores " +
							"SET " +
								"pr_email_notif=@EMAIL_NOTIF " +
							"WHERE " +
								"pr_gln=@GLN AND " +
								"pr_nit=@NIT AND " +
								"pr_razon_soc= @RAZON_SOC";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@GLN", gln);
				cmd.Parameters.AddWithValue("@NIT", nit);
				cmd.Parameters.AddWithValue("@RAZON_SOC", razonsocial);
				cmd.Parameters.AddWithValue("@EMAIL_NOTIF", emailnotificacion);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar el proveedor: " + ex.Message);
			}
		}

		public void EliminarProveedor(int id)
		{
			string SQL = "DELETE " +
							"Proveedores " +
						 "where " +
							"pr_id=@ID";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@ID", id);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al eliminar el proveedor: " + ex.Message);
			}
		}

		public DataTable ListarProveedores()
		{
			string SQL = "SELECT " +
							"pr_gln, " +
							"pr_nit, " +
							"pr_razon_soc, " +
							"pr_email_notif, " +
							"pr_id " +
						 "FROM " +
							"Proveedores";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al listar los proveedores: " + ex.Message);
			}
			return dt;
		}

		public object ObtenerCostoActual(string referencia, string nit, string sucursal)
		{
			string SQL = "SELECT " +
							"CAST(f212_precio AS DECIMAL(18,2)) AS f212_precio, " +
							"f120_descripcion " +
						"FROM " +
							"t212_mm_cotizaciones t1 " +
							"INNER JOIN t121_mc_items_extensiones t2 ON t2.f121_rowid=t1.f212_rowid_item_ext and t2.f121_id_cia=t1.f212_id_cia " +
							"INNER JOIN t120_mc_items t3 ON t3.f120_rowid=t2.f121_rowid_item and t3.f120_id_cia=t2.f121_id_cia " +
							"INNER JOIN t131_mc_items_barras t4 on t2.f121_rowid=t4.f131_rowid_item_ext and t2.f121_id_cia=t4.f131_id_cia " +
							"INNER JOIN t200_mm_terceros t6 ON t6.f200_rowid=t1.f212_rowid_tercero " +
						"WHERE " +
							"t4.f131_id=@REF AND " +
							"t1.f212_id_cia=1 AND " +
							"f200_nit=@NIT " +
							"AND f212_rowid=" +
							"(" +
								"SELECT " +
									"MAX(f212_rowid) " +
								"FROM " +
									"t212_mm_cotizaciones t5 " +
								"WHERE " +
									"t5.f212_rowid_item_ext=t2.f121_rowid " +
									"AND t5.f212_fecha_activacion<=GETDATE() " +
									"AND t5.f212_id_cia=1 " +
									"AND t5.f212_rowid_tercero=t1.f212_rowid_tercero AND " +
									"t5.f212_id_sucursal=@SUC" +
							")";
			object res = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.CommandTimeout = 180;
				cmd.Parameters.AddWithValue("@REF", referencia);
				cmd.Parameters.AddWithValue("@NIT", nit);
				cmd.Parameters.AddWithValue("@SUC", sucursal);
				res = cmd.ExecuteScalar();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el precio actual: " + ex.Message);
			}
			return res;
		}

		public DataTable ListarMotivos()
		{
			string SQL = "SELECT " +
							"mo_descripcion, " +
							"mo_id " +
						 "FROM " +
							"MotivosNoAceptacion";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable("Motivos");
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al listar los motivos de no aceptación: " + ex.Message);
			}
			return dt;
		}

		public void GuardarMotivo(string descripcion)
		{
			string SQL = "IF NOT EXISTS" +
									"(" +
										"SELECT " +
											"* " +
										"FROM " +
											"MotivosNoAceptacion " +
										"WHERE " +
										"mo_descripcion=@DESC" +
									") " +
							"BEGIN " +
								"INSERT INTO " +
									"MotivosNoAceptacion " +
									"(" +
										"mo_descripcion" +
									") " +
									"VALUES" +
									"(" +
										"@DESC" +
									") " +
							"END";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@DESC", descripcion);
				cmd.ExecuteNonQuery();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al guardar el motivo: " + ex.Message);
			}
		}


		public DataTable ObtenerItemsNoAceptadosAdicion(string numerodoc, string nombredoc, string glnproveedor, string glncomprador, string accion)
		{
			string SQL = "SELECT " +
							"do_numero_doc, " +
							"do_nombre_doc, " +
							"do_gln_proveedor, " +
							"do_gln_comprador, " +
							"do_nit, " +
							"do_razon_social, " +
							"do_accion, " +
							"it_gtin, " +
							"it_descripcion_larga, " +
							"it_aceptado, " +
							"it_motivo " +
						"FROM Documentos " +
							"INNER JOIN Items ON do_numero_doc=it_numero_doc AND do_nombre_doc=it_nombre_doc " +
												 "AND do_gln_proveedor=it_gln_proveedor AND do_gln_comprador=it_gln_comprador " +
						"WHERE do_numero_doc=@NRO_DOC " +
							"AND do_nombre_doc=@NOMB_DOC " +
							"AND do_gln_proveedor=@GLN_PROV " +
							"AND do_gln_comprador=@GLN_COMP " +
							"AND it_aceptado=0 and do_accion=@ACCION";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@NRO_DOC", numerodoc);
				cmd.Parameters.AddWithValue("@NOMB_DOC", nombredoc);
				cmd.Parameters.AddWithValue("@GLN_PROV", glnproveedor);
				cmd.Parameters.AddWithValue("@GLN_COMP", glncomprador);
				cmd.Parameters.AddWithValue("@ACCION", accion);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener los items no aceptados: " + ex.Message);
			}
			return dt;
		}

		public DataTable ObtenerItemsNoAceptadosCambioPrecio(string numerodoc, string nombredoc, string glnproveedor, string glncomprador, string accion)
		{
			string SQL = "SELECT " +
							"do_numero_doc, " +
							"do_nombre_doc, " +
							"do_gln_proveedor, " +
							"do_gln_comprador, " +
							"do_nit, " +
							"do_razon_social, " +
							"do_accion, " +
							"cp_gtin, " +
							"cp_descripcion, " +
							"cp_aceptado, " +
							"cp_motivo " +
						"FROM Documentos " +
							"INNER JOIN CambioPrecio ON do_numero_doc=cp_numero_doc AND do_nombre_doc=cp_nombre_doc " +
														"AND do_gln_proveedor=cp_gln_proveedor AND do_gln_comprador=cp_gln_comprador " +
						"WHERE do_numero_doc=@NRO_DOC " +
							"AND do_nombre_doc=@NOMB_DOC " +
							"AND do_gln_proveedor=@GLN_PROV " +
							"AND do_gln_comprador=@GLN_COMP " +
							"AND cp_aceptado=0 and do_accion=@ACCION";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@NRO_DOC", numerodoc);
				cmd.Parameters.AddWithValue("@NOMB_DOC", nombredoc);
				cmd.Parameters.AddWithValue("@GLN_PROV", glnproveedor);
				cmd.Parameters.AddWithValue("@GLN_COMP", glncomprador);
				cmd.Parameters.AddWithValue("@ACCION", accion);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener los items no aceptados: " + ex.Message);
			}
			return dt;
		}

		public string ObtenerEmailProveedor(string gln)
		{
			string SQL = "SELECT ISNULL(pr_email_notif,'') pr_email_notif FROM Proveedores WHERE pr_gln=@GLN";
			string email = "";
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@GLN", gln);
				email = Convert.ToString(cmd.ExecuteScalar());
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el email del proveedor: " + ex.Message);
			}
			return email;
		}

		public void EnviarCorreoProveedor(string emailproveedor, string mensaje, string asunto)
		{
			try
			{
				string _from = "";
				string _pwd = "";
				string smtp = "";
				int puerto;
				string[] _cc = null;

				Configuration config = ConfigurationManager.OpenExeConfiguration(Application.StartupPath + "\\" + Application.ProductName + ".exe");
				AppSettingsSection section = config.AppSettings;

				_from = section.Settings["from"].Value;
				_pwd = section.Settings["pwd"].Value;
				_cc = section.Settings["cc"].Value.Split(';');

				smtp = section.Settings["smtp"].Value;
				puerto = Convert.ToInt32(section.Settings["puerto"].Value);

				MailMessage mail = new MailMessage();
				SmtpClient SmtpServer = new SmtpClient(smtp);

				mail.From = new MailAddress(_from);
				mail.To.Add(emailproveedor);
				if (_cc != null)
				{
					for (int i = 0; i < _cc.GetLongLength(0); i++)
					{
						if (!_cc[i].Equals(""))
						{
							mail.CC.Add(_cc[i]);
						}
					}
				}
				mail.IsBodyHtml = true;
				mail.Subject = "Pricat Megatiendas: " + asunto;
				mail.Body = mensaje;
				SmtpServer.Port = puerto;
				SmtpServer.Credentials = new System.Net.NetworkCredential(_from, _pwd);
				SmtpServer.EnableSsl = false;

				ServicePointManager.ServerCertificateValidationCallback +=
							  delegate (
							  Object sender1,
							  X509Certificate certificate,
							  X509Chain chain,
							  SslPolicyErrors sslPolicyErrors)
							  {
								  return true;
							  };

				SmtpServer.Send(mail);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al notificar al proveedor: " + ex.Message);
			}
		}

		public DataTable ListarPreciosItem(string numero_doc, string nombre_doc, string gln_proveedor, string gln_comprador, string gtin)
		{
			string SQL = "SELECT " +
							"do_numero_doc, " +
							"do_nombre_doc, " +
							"do_gln_proveedor, " +
							"do_gln_comprador, " +
							"do_accion, " +
							"pv_gtin, " +
							"ISNULL(pv_lista_precio,'') pv_lista_precio, " +
							"ISNULL(pv_precio,0) pv_precio, " +
							"ISNULL(pv_fecha_activacion,'') pv_fecha_activacion, " +
							"ISNULL(pv_fecha_inactivacion,'') pv_fecha_inactivacion, " +
							"pv_margen " +
						"FROM Documentos " +
							"INNER JOIN PrecioVenta ON do_numero_doc=pv_numero_doc AND do_nombre_doc=pv_nombre_doc " +
													   "AND do_gln_proveedor=pv_gln_proveedor AND do_gln_comprador=pv_gln_comprador " +
						"WHERE " +
							"do_numero_doc=@NRO_DOC AND " +
							"do_nombre_doc=@NOMB_DOC AND " +
							"do_gln_proveedor=@GLN_PROV AND " +
							"do_gln_comprador=@GLN_COMP  AND " +
							"pv_gtin=@GTIN";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.CommandTimeout = 180;
				cmd.Parameters.AddWithValue("@NRO_DOC", numero_doc);
				cmd.Parameters.AddWithValue("@NOMB_DOC", nombre_doc);
				cmd.Parameters.AddWithValue("@GLN_PROV", gln_proveedor);
				cmd.Parameters.AddWithValue("@GLN_COMP", gln_comprador);
				cmd.Parameters.AddWithValue("@GTIN", gtin);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable("dtCosPricat");
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de listas de precio por item: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Trae el istado de proveedores desde la base de datos Pricat.
		/// </summary>
		/// <returns></returns>
		public DataTable ListarProvedoresDocumentos()
		{
			string SQL = "select distinct " +
								"do_nit, " +
								"do_razon_social " +
						 "from " +
							"Documentos " +
						 "order by " +
							"2";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.CommandTimeout = 180;
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de proveedores: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// Obtiene el listado de documentos para el proveedor en un rango de fecha.
		/// </summary>
		/// <param name="fecha_ini"></param>
		/// <param name="fecha_fin"></param>
		/// <param name="nit"></param>
		/// <returns></returns>
		public DataTable ListarDocumentos(string fecha_ini, string fecha_fin, string nit)
		{
			string SQL = @"select 
                            case do_accion
                                when 'A' then 'Adición'
                                when 'M' then 'Cambio precio'
                            end do_accion,
                            do_numero_doc,
                            do_nombre_doc,
                            do_gln_proveedor,
                            do_gln_comprador
                        from
                            Documentos
                        where
                            do_fecha BETWEEN @FECHA_INI and @FECHA_FIN
                            and do_nit = @NIT
                            and do_estado = 1
							and
							(
								(
									SELECT 
										COUNT(it_gtin) nro 
									FROM 
										Items
									WHERE 
										it_numero_doc=do_numero_doc
										AND it_nombre_doc=do_nombre_doc
										AND it_gln_proveedor=do_gln_proveedor
										AND it_gln_comprador=do_gln_comprador
										AND it_aceptado=1
								)>0
								or
								(
			
									SELECT 
										COUNT(cp_gtin) nro 
									FROM 
										CambioPrecio
									WHERE 
										cp_numero_doc=do_numero_doc
										AND cp_nombre_doc=do_nombre_doc
										AND cp_gln_proveedor=do_gln_proveedor
										AND cp_gln_comprador=do_gln_comprador
										AND cp_aceptado=1
								)>0
							)";
			DataTable dt = null;
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.CommandTimeout = 180;
				cmd.Parameters.AddWithValue("@FECHA_INI", fecha_ini);
				cmd.Parameters.AddWithValue("@FECHA_FIN", fecha_fin);
				cmd.Parameters.AddWithValue("@NIT", nit);
				SqlDataAdapter da = new SqlDataAdapter(cmd);
				dt = new DataTable();
				da.Fill(dt);
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener el listado de documentos: " + ex.Message);
			}
			return dt;
		}

		public DataTable BuscarProcesados(string desde, string hasta, string proveedor = "")
		{
			string SQL_ADICION = "select " +
									"do_numero_doc, " +
									"'Adición' accion, " +
									"do_fecha, " +
									"do_razon_social, " +
									"it_gtin gtin, " +
									"it_descripcion_larga descripcion, " +
									"it_aceptado aceptado, " +
									"isnull(it_motivo,'') motivo, " +
									"co_nombre comprador " +
								"from " +
									"documentos " +
									"inner join items on do_numero_doc = it_numero_doc and do_nombre_doc = it_nombre_doc " +
													 "and do_gln_proveedor = it_gln_proveedor and do_gln_comprador = it_gln_comprador " +
									"inner join compradores on do_gln_comprador = co_gln " +
								"where " +
									"do_estado = 1 and " +
									"do_fecha >= @DESDE and " +
									"do_fecha <= @HASTA and " +
									"do_accion='A'";

			string SQL_CAMBIO = "select " +
									"do_numero_doc, " +
									"'Cambio Precio' accion, " +
									"do_fecha, " +
									"do_razon_social, " +
									"cp_gtin gtin, " +
									"cp_descripcion descripcion, " +
									"cp_aceptado aceptado, " +
									"isnull(cp_motivo,'') motivo, " +
									"co_nombre comprador " +
								"from " +
									"documentos " +
									"inner join cambioprecio on do_numero_doc = cp_numero_doc and do_nombre_doc = cp_nombre_doc " +
															"and do_gln_proveedor = cp_gln_proveedor and do_gln_comprador = cp_gln_comprador " +
									"inner join compradores on do_gln_comprador = co_gln  " +
								"where " +
									"do_estado = 1 and " +
									"do_fecha >= @DESDE and do_fecha <= @HASTA and " +
									"do_accion='M'";
			if (!proveedor.Equals(""))
			{
				SQL_ADICION += " and do_nit=@NIT";
				SQL_CAMBIO += " and do_nit=@NIT";
			}
			DataTable dt = new DataTable();
			dt.Columns.Add("do_numero_doc");
			dt.Columns.Add("accion");
			dt.Columns.Add("do_fecha");
			dt.Columns.Add("do_razon_social");
			dt.Columns.Add("gtin");
			dt.Columns.Add("descripcion");
			dt.Columns.Add("aceptado");
			dt.Columns.Add("motivo");
			dt.Columns.Add("comprador");
			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionLogyca);
				conn.Open();
				SqlCommand cmd_adicion = new SqlCommand(SQL_ADICION, conn);
				cmd_adicion.CommandType = CommandType.Text;
				cmd_adicion.Parameters.AddWithValue("@DESDE", desde);
				cmd_adicion.Parameters.AddWithValue("@HASTA", hasta);

				SqlDataReader dr_adicion = cmd_adicion.ExecuteReader();
				if (dr_adicion.HasRows)
				{
					while (dr_adicion.Read())
					{
						dt.Rows.Add(dr_adicion.GetString(0), dr_adicion.GetString(1), dr_adicion.GetDateTime(2).ToShortDateString(), dr_adicion.GetString(3)
									, dr_adicion.GetString(4), dr_adicion.GetString(5), dr_adicion.GetBoolean(6), dr_adicion.GetString(7), dr_adicion.GetString(8));
					}
				}
				dr_adicion.Close();

				SqlCommand cmd_cambio = new SqlCommand(SQL_CAMBIO, conn);
				cmd_cambio.CommandType = CommandType.Text;
				cmd_cambio.Parameters.AddWithValue("@DESDE", desde);
				cmd_cambio.Parameters.AddWithValue("@HASTA", hasta);

				SqlDataReader dr_cambio = cmd_cambio.ExecuteReader();
				if (dr_cambio.HasRows)
				{
					while (dr_cambio.Read())
					{
						dt.Rows.Add(dr_cambio.GetString(0), dr_cambio.GetString(1), dr_cambio.GetDateTime(2).ToShortDateString(), dr_cambio.GetString(3)
									, dr_cambio.GetString(4), dr_cambio.GetString(5), dr_cambio.GetBoolean(6), dr_cambio.GetString(7), dr_cambio.GetString(8));
					}
				}
				dr_cambio.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al buscar archivos procesados: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="desde"></param>
		/// <param name="hasta"></param>
		/// <param name="accion"></param>
		/// <param name="proveedor"></param>
		/// <returns></returns>
		public DataTable BuscarProcesadosCambioPrecio(string desde, string hasta, string proveedor)
		{
			string SQL = "DECLARE @Lista NVARCHAR(10) " +
						"DECLARE @ListaPVT NVARCHAR(MAX) " +
						"DECLARE @TableLista AS TABLE([Lista] VARCHAR(3) NOT NULL) " +
						"INSERT INTO @TableLista " +
						"SELECT DISTINCT pv_lista_precio FROM Documentos INNER JOIN PrecioVenta ON " +
							"do_numero_doc = pv_numero_doc " +
							"and do_nombre_doc = pv_nombre_doc " +
							"and do_gln_proveedor = pv_gln_proveedor " +
							"and do_gln_comprador = pv_gln_comprador " +
						"WHERE do_accion = 'M' " +
						"SET @Lista = (SELECT MIN([Lista]) FROM @TableLista) " +
						"SET @ListaPVT = N'' " +
						"WHILE @Lista IS NOT NULL " +
						"BEGIN " +
							"SET @ListaPVT = @ListaPVT + N',[' + CONVERT(NVARCHAR(10), @Lista) + N']' " +
							"SET @Lista = (SELECT MIN([Lista]) FROM @TableLista WHERE[Lista] > @Lista) " +
						"END " +
						"SET @ListaPVT = SUBSTRING(@ListaPVT, 2, LEN(@ListaPVT)) " +
						"DECLARE @SQL NVARCHAR(MAX) " +
						"SET @SQL = N' " +
						"SELECT * FROM(select cp_aceptado Aceptado, cp_motivo Motivo, do_numero_doc ''Nro.Doc.'', do_fecha Fecha, do_razon_social Proveedor, cp_gtin Referencia, cp_descripcion Descripción, cp_precio Costo, cp_impuesto Iva, " +
						"cp_fecha_act ''Fecha Act. Costo'', cp_fecha_hasta ''Fecha Inact. Costo'', pv_fecha_activacion ''Fecha Act. PVP'', pv_fecha_inactivacion ''Fecha Inact. PVP'', co_nombre Comprador, pv_lista_precio LP, sum(convert(decimal(18, 2), pv_precio)) PVP " +
						"FROM documentos " +
						"INNER JOIN cambioprecio ON " +
							"do_numero_doc = cp_numero_doc " +
							"AND do_nombre_doc = cp_nombre_doc " +
							"AND do_gln_proveedor = cp_gln_proveedor " +
							"AND do_gln_comprador = cp_gln_comprador " +
						"INNER JOIN precioventa ON " +
							"do_numero_doc = pv_numero_doc " +
							"AND do_nombre_doc = pv_nombre_doc and pv_nombre_doc = cp_nombre_doc " +
							"AND do_gln_proveedor = pv_gln_proveedor and pv_gln_proveedor = cp_gln_proveedor " +
							"AND do_gln_comprador = pv_gln_comprador and pv_gln_comprador = cp_gln_comprador " +
							"AND pv_gtin = cp_gtin " +
						"INNER JOIN compradores ON do_gln_comprador = co_gln " +
						"WHERE do_estado = ''1'' AND do_accion = ''M'' AND do_fecha >= ''' + @DESDE + ''' AND do_fecha <= ''' + @HASTA + '''";
			if (!proveedor.Equals(""))
			{
				SQL += " AND do_nit=''' + @NIT + '''";
			}
			SQL += "GROUP BY " +
				"do_numero_doc, do_fecha, do_razon_social, cp_gtin, cp_descripcion, " +
				"cp_aceptado, cp_motivo, co_nombre, pv_lista_precio, cp_precio, cp_impuesto, cp_fecha_act, cp_fecha_hasta, pv_fecha_activacion, pv_fecha_inactivacion " +
			") AS pvt " +
			"PIVOT(sum(PVP) FOR LP IN('+ @ListaPVT +')) as Resultado' " +
			"EXECUTE sp_executesql @SQL";

			DataTable dt = null;
			try
			{
				SqlDataAdapter da = new SqlDataAdapter(SQL, Conexion.CadenaConexionLogyca);

				da.SelectCommand.Parameters.AddWithValue("@DESDE", desde);
				da.SelectCommand.Parameters.AddWithValue("@HASTA", hasta);
				if (!proveedor.Equals(""))
				{
					da.SelectCommand.Parameters.AddWithValue("@NIT", proveedor);
				}

				dt = new DataTable();
				da.Fill(dt);
			}
			catch (Exception ex)
			{
				throw new Exception("Error al buscar archivos procesados: " + ex.Message);
			}
			return dt;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="fecha_ini"></param>
		/// <param name="fecha_fin"></param>
		/// <param name="terminal"></param>
		/// <returns></returns>
		public DataSet ConsultarDocumentos(string fecha_ini, string fecha_fin, byte terminal)
		{
			string SQL1 = "SELECT " +
							"do_numero_doc, " +
							"do_nombre_doc, " +
							"do_accion, " +
							"do_fecha_elaboracion, " +
							"CASE do_estado " +
								"WHEN 1 THEN 'Si' " +
								"WHEN 0 THEN 'No' " +
							"END do_estado, " +
							"do_fecha, " +
							"do_usuario, " +
							"do_razon_social, " +
							"co_nombre " +
							"FROM Documentos " +
							"INNER JOIN Compradores ON do_gln_comprador = co_gln " +
							"WHERE do_fecha>= @FECHA_INI AND do_fecha <= @FECHA_FIN";

			string SQL2 = "SELECT it_numero_doc, " +
							"it_nombre_doc, " +
							"it_gtin, " +
							"it_descripcion_larga, " +
							"it_grupo_impositivo, " +
							"it_tipo_inventario, " +
							"it_unidad_inventario, " +
							"it_factor_peso_inventario, " +
							"it_unidad_orden, " +
							"it_unidad_empaque, " +
							"it_impuesto, " +
							"FROM Items " +
							"WHERE it_numero_doc = @NRO_DOC AND it_nombre_doc = @NOMB_DOC";
			try
			{
				DataSet ds = new DataSet();

				SqlDataAdapter da1 = new SqlDataAdapter(SQL1, Conexion.CadenaConexionLogyca);

				da1.SelectCommand.Parameters.AddWithValue("@FECHA_INI", fecha_ini);
				da1.SelectCommand.Parameters.AddWithValue("@FECHA_FIN", fecha_fin);
				da1.SelectCommand.Parameters.AddWithValue("@TERMINAL", terminal);
				DataTable dt1 = new DataTable("Pedidos");
				ds.Tables.Add(dt1);
				da1.Fill(ds, "Pedidos");

				if (ds.Tables["Pedidos"].Rows.Count > 0)
				{
					foreach (DataRow item in ds.Tables["Pedidos"].Rows)
					{
						SQL2 = SQL2 + "'" + item[0] + "',";
					}
					SQL2 = SQL2.Trim(',') + ")";
					SqlDataAdapter da2 = new SqlDataAdapter(SQL2, ConfigurationManager.ConnectionStrings["pedidosmega"].ConnectionString);
					DataTable dt2 = new DataTable("Detalle");
					da2.SelectCommand.Parameters.AddWithValue("@TERMINAL", terminal);
					ds.Tables.Add(dt2);
					da2.Fill(ds, "Detalle");
					DataColumn[] dc_p = new DataColumn[2];
					DataColumn[] dc_c = new DataColumn[2];

					dc_p[0] = ds.Tables[0].Columns[0];
					dc_p[1] = ds.Tables[0].Columns[1];

					dc_c[0] = ds.Tables[1].Columns[0];
					dc_c[1] = ds.Tables[1].Columns[1];

					ds.Relations.Add("rel1", dc_p, dc_c);
				}
				else
				{
					ds.Tables.Clear();
				}
				return ds;
			}
			catch (Exception ex)
			{
				throw new Exception("Error al consultar los documentos: " + ex.Message);
			}
		}
	}
}