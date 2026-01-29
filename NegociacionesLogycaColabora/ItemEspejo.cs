using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace NegociacionesLogycaColabora
{
    public class ItemEspejo
    {
        private static string grupo_imp = "";
        private static string tipo_inv = "";
        private static DataTable criterios_clasificacion = null;
        private static DataTable parametros_plan = null;
        private static DataTable precios_vta_adicion = null;
        private static DataTable portafolio = null;
        private static string[] barra = null;
        private static string[] descripcion_tecnica = null;
		private static bool seleccion = false;

        private static string desc_larga = "";
        private static string desc_corta = "";

        private static int ind_compra = 0;
        private static int ind_venta = 0;
        private static int ind_manufactura = 0;

        public static string GrupoImpositivo 
        {
            get { return grupo_imp; }
            set { grupo_imp = value; } 
        }

        public static string TipoInventario
        {
            get { return tipo_inv; }
            set { tipo_inv = value; }
        }

        public static DataTable CriteriosClasificacion
        {
            get { return criterios_clasificacion; }
            set { criterios_clasificacion = value; }
        }

        public static DataTable ParametrosPlaneacion
        {
            get { return parametros_plan; }
            set { parametros_plan = value; }
        }

        public static DataTable PreciosVenta
        {
            get { return precios_vta_adicion; }
            set { precios_vta_adicion = value; }
        }

        public static DataTable Portafolio
        {
            get { return portafolio; }
            set { portafolio = value; }
        }

        public static string[] Barra
        {
            get { return barra; }
            set { barra = value; }
        }

		public static string[] DescripcionTecnica
		{
			get { return descripcion_tecnica; }
			set { descripcion_tecnica = value; }
		}

		public static bool Seleccion
        {
            get { return seleccion; }
            set { seleccion = value; }
        }

        public static string DescripcionLarga
        {
            get { return desc_larga; }
            set { desc_larga = value; }
        }

        public static string DescripcionCorta
        {
            get { return desc_corta; }
            set { desc_corta = value; }
        }

        public static int IndCompra
        {
            get { return ind_compra; }
            set { ind_compra = value; }
        }

        public static int IndVenta
        {
            get { return ind_venta; }
            set { ind_venta = value; }
        }

        public static int IndManufactura
        {
            get { return ind_manufactura; }
            set { ind_manufactura = value; }
        }

        public static void LlenarGrupoImpTipoInv(string id)
        {
            string SQL = "select " +
                            "f120_id_grupo_impositivo, " +
                            "f120_id_tipo_inv_serv " +
                         "from " +
                            "t120_mc_items " +
                         "where " +
                            "f120_id=@ID and " +
                            "f120_id_cia=1";
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    dr.Read();
                    GrupoImpositivo = dr.GetString(0);
                    TipoInventario = dr.GetString(1);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el grupo impositivo y el tipo de inventario del ítem espejo: " + ex.Message);
            }
        }

        public static void LlenarCriteriosClasificacion(string id)
        {
            string SQL = "select " +
                            "f125_id_plan, " +
                            "f105_descripcion, " +
                            "f125_id_criterio_mayor, " +
                            "f106_descripcion " +
                         "from t125_mc_items_criterios " +
                         "inner join t105_mc_criterios_item_planes on f125_id_plan=f105_id and f105_id_cia=f125_id_cia " +
                         "inner join t106_mc_criterios_item_mayores on f106_id=f125_id_criterio_mayor and f106_id_plan=f105_id and f106_id_cia=f105_id_cia " +
                         "inner join t120_mc_items on f125_rowid_item=f120_rowid and f120_id_cia=f125_id_cia " +
                         "where f125_id_cia=1 and f120_id=@ID";
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                CriteriosClasificacion = dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los criterios de clasificación del ítem espejo: " + ex.Message);
            }
        }

        public static void LlenarParametrosPlaneacion(string id)
        {
            string SQL = "select " +
                       "  f132_id_instalacion " +
                       ", f150_id as bodega_asigna " +
                       ", f132_abc_rotacion_veces " +
                       ", f132_abc_rotacion_costo " +
                       ", f132_id_um_venta_suge " +
                       ", f132_periodo_cubrimiento " +
                       ", f132_tiempo_reposicion " +
                       ", f132_tiempo_seguridad " +
                       ", f132_mf_id_comprador " +
                       ", f132_mf_dias_horiz_planea " +
                       ", f132_mf_stock_segur_estatico " +
                       ", f132_mf_dias_horiz_stock_min " +
                       ", f132_mf_dias_stock_min " +
                       ", f132_mf_tiempo_repo_fijo " +
                       ", f132_mf_ind_politica_orden " +
                       ", f132_mf_tamano_lote, " +
                       "f132_mf_ind_genera_ord_pln " +
                    "from " +
                       "t132_mc_items_instalacion " +
                       "inner join t121_mc_items_extensiones on f132_rowid_item_ext=f121_rowid and f132_id_cia=f121_id_cia " +
                       "inner join t120_mc_items on f120_rowid=f121_rowid_item and f120_id_cia=f121_id_cia " +
                       "inner join t200_mm_terceros on f132_mf_rowid_tercero_prov_1=f200_rowid and f132_id_cia=f200_id_cia " +
                       "inner join t150_mc_bodegas on f132_mf_rowid_bodega_asigna=f150_rowid and f132_id_cia=f150_id_cia " +
                    "where " +
                        "f120_id=@ID and f132_id_cia=1 and f132_id_instalacion not in('115','701')";
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                ParametrosPlaneacion = dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener los parametros de planeación: " + ex.Message);
            }
        }

        public static void LlenarListaPrecios(string id)
        {
            string SQL = "select distinct " +
	                        "f126_id_lista_precio " +
                        "from " +
                           "t121_mc_items_extensiones " +
                           "inner join t120_mc_items on f120_rowid=f121_rowid_item and f120_id_cia=f121_id_cia " +
                           "inner join t122_mc_items_unidades on f122_rowid_item=f121_rowid_item and f122_id_cia=f121_id_cia " +
                           "inner join t126_mc_items_precios on f126_rowid_item=f122_rowid_item and f126_id_cia=f122_id_cia and f126_id_unidad_medida=f122_id_unidad " +
                           "inner join t112_mc_listas_precios on f126_id_lista_precio=f112_id and f112_id_cia=f126_id_cia " +
                           "inner join t1121_mc_listas_precios_co on f1121_id_lista_precio=f112_id and f1121_id_cia=f112_id_cia " +
                        "where " +
	                        "f120_id=@ID " +
	                        "and f121_id_cia=1 " +
	                        "and f121_ind_estado='1' " +
	                        "and (f126_fecha_inactivacion is null or f126_fecha_inactivacion > CURRENT_TIMESTAMP) " +
	                        "and f126_fecha_activacion<=CURRENT_TIMESTAMP " +
	                        "and f126_fecha_activacion=( " +
		                        "select " +
			                        "max(p.f126_fecha_activacion) " +
		                        "from " +
			                        "t126_mc_items_precios p " +
		                        "where " +
			                        "p.f126_id_unidad_medida=f122_id_unidad " +
			                        "and p.f126_rowid_item=f122_rowid_item " +
			                        "and p.f126_id_lista_precio=f112_id " +
			                        "and (p.f126_fecha_inactivacion is null or p.f126_fecha_inactivacion > CURRENT_TIMESTAMP) " +
			                        "and p.f126_fecha_activacion<=CURRENT_TIMESTAMP " +
                                    ") " +       
                        "order by  1;";
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt_result = new DataTable();
                da.Fill(dt_result);
                conn.Close();
                PreciosVenta = dt_result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de precios adición: " + ex.Message);
            }
        }
         
        public static void LlenarPortafolios(string id)
        {
            string SQL = "select " +
	                        "f136_id, " +
	                        "f136_descripcion, " +
                            "case " +
                                "f136_notas " +
                            "when null then f136_descripcion " +
                            "when '' then f136_descripcion " +
                            "else " +
                                "f136_notas " +
                            "end as f136_notas " +
                        "from " +
	                        "t136_mc_portafolio " +
	                        "inner join t137_mc_portafolio_items on f137_id_portafolio=f136_id and f136_id_cia=f137_id_cia " +
	                        "inner join t121_mc_items_extensiones on f121_rowid=f137_rowid_item_ext and f121_id_cia=f137_id_cia " +
	                        "inner join t120_mc_items on f120_rowid=f121_rowid_item and f120_id_cia=f121_id_cia " +
	                        "inner join t285_co_centro_op on f285_id_portafolio=f136_id and f285_id_cia=f136_id_cia " +
                        "where " +
	                        "f120_id=@ID and f136_ind_estado=1 and f136_id_cia=1";
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                Portafolio = dt;
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener la lista de portafolios: " + ex.Message);
            }
        }

        public static void LlenarBarra(string id)
        {
            string SQL = "select " +
	                    "f131_cant_unidad_medida, " +
	                    "f131_ind_tipo, " +
	                    "f131_ind_factor, " +
	                    "f131_factor " +
                    "from " +
	                    "t131_mc_items_barras " +
	                    "inner join t121_mc_items_extensiones on f121_rowid=f131_rowid_item_ext and f121_id_cia=f131_id_cia " +
                        "inner join t120_mc_items on f120_rowid=f121_rowid_item and f120_id_cia=f121_id_cia " +
                    "where " +
                        "f120_id=@ID and " +
                        "f131_id_cia=1 and " +
                        "f121_ind_estado=1 and " +
                        "f121_id_barras_principal=f131_id";
            
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.AddWithValue("@ID", id);
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    Barra = new string[4];
                    dr.Read();
                    barra[0] = Convert.ToString( dr.GetDecimal(0));
                    barra[1] = Convert.ToString( dr.GetInt16(1));
                    barra[2] = Convert.ToString(dr.GetInt16(2));
                    barra[3] = Convert.ToString(dr.GetDecimal(3));
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener información de la barra: " + ex.Message);
            }
        }

		public static void LlenarDescripcionTecnica(string id)
		{
			string SQL = @"select 
	                        d1.f123_dato descipcion1,
	                        d2.f123_dato descipcion2,
	                        d3.f123_dato descipcion3
                        from
	                        t120_mc_items
                            inner join t121_mc_items_extensiones on f121_id_cia = f120_id_cia and f121_rowid_item = f120_rowid 
                            left outer join t123_mc_items_desc_tecnicas d1 on d1.f123_id_cia=f120_id_cia and d1.f123_rowid_item=f120_rowid and d1.f123_rowid_campo=86 
                            left outer join t123_mc_items_desc_tecnicas d2 on d2.f123_id_cia = f120_id_cia and d2.f123_rowid_item = f120_rowid and d2.f123_rowid_campo = 87
                            left outer join t123_mc_items_desc_tecnicas d3 on d3.f123_id_cia = f120_id_cia and d3.f123_rowid_item = f120_rowid and d3.f123_rowid_campo = 88 
                        where
	                        f120_id=@ID
	                        and f120_id_cia=1";

			try
			{
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
				cmd.CommandType = CommandType.Text;
				cmd.Parameters.AddWithValue("@ID", id);
				SqlDataReader dr = cmd.ExecuteReader();

				if (dr.HasRows)
				{
					DescripcionTecnica = new string[3];
					dr.Read();
					DescripcionTecnica[0] = dr.GetString(0);
					DescripcionTecnica[1] = dr.GetString(1);
					DescripcionTecnica[2] = dr.GetString(2);
				}
				dr.Close();
				conn.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("Error al obtener descripción tecnica: " + ex.Message);
			}
		}

		/// <summary>
		/// Obtiene el listado de items que coinciden con una descripción.
		/// </summary>
		/// <param name="texto">Descripción del item.</param>
		/// <returns>Devuelve un objeto de tipo datatable que contiene la información.</returns>
		public DataTable ListarItemsEspejo(string texto, string param)
        {
            string SQL = "";
            if (param.Equals("desc"))
            {
                SQL = "select " +
                            "f120_id, " +
                            "rtrim(f120_referencia) f120_referencia, " +
                            "f120_descripcion, " +
                            "f120_descripcion_corta," +
                            "f120_ind_compra, " +
                            "f120_ind_venta, " +
                            "f120_ind_manufactura " +
                      "from " +
                        "t120_mc_items " +
                        "inner join t121_mc_items_extensiones on f120_rowid=f121_rowid_item and f121_id_cia=f120_id_cia " +
                     "where " +
                        "f120_descripcion like '%" + texto + "%' and " +
                        "f120_id_cia='1' and " +
                        "f121_ind_estado='1'";
            }
            else
            {
                SQL = "select " +
                        "f120_id, " +
                        "f120_referencia, " +
                        "f120_descripcion, " +
                        "f120_descripcion_corta " +
                     "from " +
                        "t120_mc_items " +
                        "inner join t121_mc_items_extensiones on f120_rowid=f121_rowid_item and f121_id_cia=f120_id_cia " +
                     "where " +
                        "f120_referencia='" + texto + "' and " +
                        "f120_id_cia='1' and " +
                        "f121_ind_estado='1'";
            }
            DataTable dt = null;
            try
            {
				SqlConnection conn = new SqlConnection(Conexion.CadenaConexionUnoee);
				conn.Open();
				SqlCommand cmd = new SqlCommand(SQL, conn);
                cmd.CommandTimeout = 600;
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                conn.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener el listado de ítems espajo: " + ex.Message);
            }
            return dt;
        }

    }
}
